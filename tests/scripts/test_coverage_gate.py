"""Tests for scripts/coverage_gate.py, run against Cobertura reports shaped like coverlet's output.

Run from the repository root: python3 -m unittest discover -s tests/scripts
"""
import contextlib
import io
import os
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
        self.output = Path(self.work.name) / "coverage.cobertura.xml"

    def tearDown(self):
        self.work.cleanup()
        os.environ.update(self.saved)

    def add_report(self, xml, folder="run-1"):
        path = self.results / folder / "coverage.cobertura.xml"
        path.parent.mkdir(parents=True)
        path.write_text(xml, encoding="utf-8")

    def run_gate(self, minimum="95"):
        out, err = io.StringIO(), io.StringIO()
        with contextlib.redirect_stdout(out), contextlib.redirect_stderr(err):
            code = coverage_gate.main([str(self.results), str(self.output), "--minimum", minimum])
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

    def test_two_reports_are_refused(self):
        xml = report("1", 1, 1, [covered_class("Probe", [1])])
        self.add_report(xml, "run-1")
        self.add_report(xml, "run-2")
        code, out, err = self.run_gate()
        self.assertEqual(code, 2)
        self.assertIn("found 2", err)
        self.assertFalse(self.output.exists())


if __name__ == "__main__":
    unittest.main()
