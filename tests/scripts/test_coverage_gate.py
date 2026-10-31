"""Tests for scripts/coverage_gate.py, run against Cobertura reports shaped like coverlet's output.

Run from the repository root: python3 -m unittest discover -s tests/scripts
"""
import contextlib
import io
import os
import re
import shutil
import sys
import tempfile
import unittest
from pathlib import Path

sys.path.insert(0, str(Path(__file__).resolve().parents[2] / "scripts"))
import coverage_gate  # noqa: E402

GITHUB_VARIABLES = ("GITHUB_ACTIONS", "GITHUB_OUTPUT", "GITHUB_STEP_SUMMARY")


def report(line_rate, covered, valid, classes):
    """A coverlet-style report: the totals on <coverage>, each class with <methods> and its own <lines>."""
    return (
        '<?xml version="1.0" encoding="utf-8"?>\n'
        f'<coverage line-rate="{line_rate}" branch-rate="1" version="1.9" timestamp="1793366245" '
        f'lines-covered="{covered}" lines-valid="{valid}" branches-covered="0" branches-valid="0">\n'
        "  <sources><source>/repo/src/NoName.Diagnostics.Core/</source></sources>\n"
        '  <packages><package name="NoName.Diagnostics.Core" line-rate="1" branch-rate="1" complexity="1"><classes>\n'
        + "".join(classes)
        + "  </classes></package></packages>\n</coverage>\n"
    )


def with_attribute(xml, name, value=None):
    """The report with attribute `name` on <coverage> set to `value`, or removed when value is None."""
    return re.sub(f' {name}="[^"]*"', "" if value is None else f' {name}="{value}"', xml, count=1)


def covered_class(name, hits):
    """A class whose lines 10, 11, ... have the given hit counts, listed under its method and again under the class."""
    lines = "".join(f'<line number="{10 + i}" hits="{h}" branch="False" />' for i, h in enumerate(hits))
    rate = sum(1 for h in hits if h) / len(hits)
    return (
        f'    <class name="{name}" filename="{name}.cs" line-rate="{rate}" branch-rate="1" complexity="1">'
        f'<methods><method name="Run" signature="()" line-rate="{rate}" branch-rate="1" complexity="1">'
        f"<lines>{lines}</lines></method></methods><lines>{lines}</lines></class>\n"
    )


class CoverageGateTests(unittest.TestCase):
    def setUp(self):
        # The gate appends to GitHub's output files when these are set; a test must never touch the real ones.
        self.saved = {name: os.environ.pop(name) for name in GITHUB_VARIABLES if name in os.environ}
        self.work = tempfile.TemporaryDirectory()
        self.results = Path(self.work.name) / "results"
        self.summary = Path(self.work.name) / "summary.md"

    def tearDown(self):
        self.work.cleanup()
        # Remove what a test set, then put back what the developer's shell had.
        for name in GITHUB_VARIABLES:
            os.environ.pop(name, None)
        os.environ.update(self.saved)

    def as_github_actions(self):
        os.environ["GITHUB_ACTIONS"] = "true"
        os.environ["GITHUB_STEP_SUMMARY"] = str(self.summary)

    def add_report(self, xml, folder="run-1"):
        path = self.results / folder / "coverage.cobertura.xml"
        path.parent.mkdir(parents=True)
        path.write_text(xml, encoding="utf-8")

    def run_gate(self, minimum="95"):
        out, err = io.StringIO(), io.StringIO()
        with contextlib.redirect_stdout(out), contextlib.redirect_stderr(err):
            code = coverage_gate.main([str(self.results), "--minimum", minimum])
        return code, out.getvalue(), err.getvalue()

    def test_coverage_below_minimum_fails_even_when_the_rounded_rate_reaches_it(self):
        # 94,996 of 100,000 lines is 94.996%; coverlet's four-decimal line-rate rounds that to 0.95.
        self.add_report(report("0.95", 94996, 100000, [covered_class("Probe", [1, 1])]))
        code, out, err = self.run_gate()
        self.assertEqual(code, 1)
        self.assertIn("Line coverage 94.99% (94996 of 100000 lines)", out)
        self.assertIn("94.99% is below the 95.00% minimum", err)

    def test_class_without_a_lines_element_still_gets_a_verdict(self):
        no_lines = (
            '    <class name="Empty" filename="Empty.cs" line-rate="1" branch-rate="1" complexity="1">'
            "<methods /></class>\n"
        )
        self.add_report(report("1", 2, 2, [covered_class("Probe", [3, 4]), no_lines]))
        code, out, err = self.run_gate()
        self.assertEqual(code, 0)
        self.assertIn("Line coverage 100.00% (2 of 2 lines)", out)

    def test_coverage_at_the_minimum_passes(self):
        self.add_report(report("0.95", 95, 100, [covered_class("Probe", [1])]))
        code, out, err = self.run_gate()
        self.assertEqual(code, 0)
        self.assertIn("Line coverage 95.00% (95 of 100 lines)", out)

    def test_each_missed_line_is_listed_once(self):
        self.add_report(report("0.5", 2, 4, [covered_class("Probe", [5, 0, 7, 0])]))
        code, out, err = self.run_gate()
        self.assertEqual(code, 1)
        self.assertIn("missed lines: 11, 13\n", out)

    def test_report_with_no_measured_lines_is_refused(self):
        # coverlet writes lines-valid="0" when nothing was instrumented, for example a wrong include filter.
        self.add_report(report("1", 0, 0, []))
        code, out, err = self.run_gate()
        self.assertEqual(code, 2)
        self.assertIn("measured no lines", err)

    def test_github_failure_is_an_error_annotation_and_a_job_summary_with_the_gaps(self):
        self.as_github_actions()
        self.summary.write_text("earlier step\n", encoding="utf-8")
        self.add_report(report("0.625", 5, 8, [
            covered_class("Json", [1, 1, 1, 0]),  # listed before the worse class: the table sorts
            covered_class("Probe", [5, 0, 7, 0]),
            covered_class("Sanitizer", [1, 1]),
        ]))
        code, out, err = self.run_gate()
        self.assertEqual(code, 1)
        # GitHub decodes %25 back to the percent signs.
        self.assertIn("::error title=Coverage::line coverage 62.50%25 is below the 95.00%25 minimum\n", out)
        self.assertEqual(err, "")
        self.assertIn("  50.00%  Probe.cs  Probe  missed lines: 11, 13\n", out)
        self.assertLess(out.index("Probe.cs"), out.index("Json.cs"), "the worst class is listed first")
        text = self.summary.read_text(encoding="utf-8")
        self.assertTrue(text.startswith("earlier step\n"), "the summary is appended to, not replaced")
        self.assertIn("### NoName.Diagnostics.Core coverage\n\n", text)
        self.assertIn("Line coverage **62.50%** (5 of 8 lines) is below the 95% minimum.\n", text)
        self.assertIn(
            "| Coverage | Class | Missed lines |\n| ---: | --- | --- |\n"
            "| 50.0% | `Probe` | 11, 13 |\n| 75.0% | `Json` | 13 |\n",
            text,
        )
        self.assertNotIn("Sanitizer", text)

    def test_github_pass_is_a_summary_without_a_table_or_an_error(self):
        self.as_github_actions()
        self.add_report(report("1", 2, 2, [covered_class("Probe", [3, 4])]))
        code, out, err = self.run_gate()
        self.assertEqual(code, 0)
        self.assertNotIn("::error", out)
        text = self.summary.read_text(encoding="utf-8")
        self.assertIn("Line coverage **100.00%** (2 of 2 lines) meets the 95% minimum.\n", text)
        self.assertNotIn("| Coverage |", text)

    def test_outside_github_there_is_no_summary_and_the_error_goes_to_stderr(self):
        self.add_report(report("0.5", 2, 4, [covered_class("Probe", [5, 0, 7, 0])]))
        code, out, err = self.run_gate()
        self.assertEqual(code, 1)
        self.assertNotIn("::error", out)
        self.assertEqual(err, "error: line coverage 50.00% is below the 95.00% minimum\n")
        self.assertFalse(self.summary.exists())

    def test_the_annotation_message_is_escaped(self):
        self.as_github_actions()
        out = io.StringIO()
        with contextlib.redirect_stdout(out):
            coverage_gate.fail("100% of a\r\nb\nc")
        self.assertEqual(out.getvalue(), "::error title=Coverage::100%25 of a%0D%0Ab%0Ac\n")

    def test_the_report_is_read_in_place_and_nothing_is_copied(self):
        self.add_report(report("1", 2, 2, [covered_class("Probe", [3, 4])]))
        code, out, err = self.run_gate()
        self.assertEqual(code, 0)
        self.assertEqual([entry.name for entry in Path(self.work.name).iterdir()], ["results"])
        self.assertEqual([entry.name for entry in (self.results / "run-1").iterdir()], ["coverage.cobertura.xml"])

    def test_github_output_is_not_written(self):
        # An earlier version published report= and line_rate= for the removed upload step.
        outputs = Path(self.work.name) / "github-output"
        os.environ["GITHUB_OUTPUT"] = str(outputs)
        self.add_report(report("1", 2, 2, [covered_class("Probe", [3, 4])]))
        code, out, err = self.run_gate()
        self.assertEqual(code, 0)
        self.assertFalse(outputs.exists())

    def test_report_without_usable_line_counts_is_refused(self):
        good = report("1", 2, 2, [covered_class("Probe", [3, 4])])
        cases = [
            ("lines-covered", with_attribute(good, "lines-covered")),
            ("lines-valid", with_attribute(good, "lines-valid")),
            ("lines-covered", with_attribute(good, "lines-covered", "many")),
            ("lines-valid", with_attribute(good, "lines-valid", "")),
        ]
        for name, xml in cases:
            with self.subTest(name=name, xml=xml[:200]):
                self.setUp_results()
                self.add_report(xml)
                code, out, err = self.run_gate()
                self.assertEqual(code, 2)
                self.assertIn(f"no usable {name} count", err)

    def test_report_that_is_not_xml_is_refused(self):
        for text in ("", "<coverage lines-valid=", "not xml at all"):
            with self.subTest(text=text):
                self.setUp_results()
                self.add_report(text)
                code, out, err = self.run_gate()
                self.assertEqual(code, 2)
                self.assertIn("could not parse", err)

    def setUp_results(self):
        """A fresh results folder, for tests that try several reports in turn."""
        if self.results.exists():
            shutil.rmtree(self.results)

    def test_two_reports_are_refused(self):
        xml = report("1", 1, 1, [covered_class("Probe", [1])])
        self.add_report(xml, "run-1")
        self.add_report(xml, "run-2")
        code, out, err = self.run_gate()
        self.assertEqual(code, 2)
        self.assertIn("found 2", err)
        # The paths are part of the one message, so a GitHub annotation carries them too.
        first, second = (self.results / "run-1" / "coverage.cobertura.xml"), (self.results / "run-2" / "coverage.cobertura.xml")
        self.assertEqual(
            err,
            f"error: expected one coverage.cobertura.xml under {self.results}, found 2\n  {first}\n  {second}\n",
        )

    def test_the_paths_of_several_reports_are_in_the_annotation(self):
        self.as_github_actions()
        xml = report("1", 1, 1, [covered_class("Probe", [1])])
        self.add_report(xml, "run-1")
        self.add_report(xml, "run-2")
        code, out, err = self.run_gate()
        self.assertEqual(code, 2)
        first, second = (self.results / "run-1" / "coverage.cobertura.xml"), (self.results / "run-2" / "coverage.cobertura.xml")
        self.assertEqual(
            out,
            f"::error title=Coverage::expected one coverage.cobertura.xml under {self.results}, found 2%0A  {first}%0A  {second}\n",
        )
        self.assertEqual(err, "")

    def test_no_report_is_refused(self):
        code, out, err = self.run_gate()
        self.assertEqual(code, 2)
        self.assertEqual(err, f"error: expected one coverage.cobertura.xml under {self.results}, found 0\n")

if __name__ == "__main__":
    unittest.main()
