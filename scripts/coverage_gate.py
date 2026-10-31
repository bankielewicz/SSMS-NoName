#!/usr/bin/env python3
"""Check line coverage from a coverlet Cobertura report.

Finds the single coverage.cobertura.xml under RESULTS_DIR, prints the overall line coverage
and every class below 100% with its missed lines, and exits 1 when the line coverage is below
--minimum. The rate comes from the exact covered and valid line counts, so CI and a local run
give the same verdict. A report that can't be used exits 2, so it can't be mistaken for low
coverage: none or several reports, one that isn't XML, has no usable line counts or has a class
without a usable line-rate, or one that measured no lines.
Tests: tests/scripts/test_coverage_gate.py.

Usage: coverage_gate.py RESULTS_DIR --minimum PERCENT
In GitHub Actions a failure is also an ::error annotation, and a table goes to the job summary.
"""
import argparse
import os
import sys
import xml.etree.ElementTree as ET
from pathlib import Path


def main(argv=None):
    parser = argparse.ArgumentParser(description=__doc__.splitlines()[0])
    parser.add_argument("results_dir", help="folder that dotnet test --results-directory wrote to")
    parser.add_argument("--minimum", type=float, required=True, help="minimum line coverage, in percent")
    args = parser.parse_args(argv)

    reports = sorted(Path(args.results_dir).rglob("coverage.cobertura.xml"))
    if len(reports) != 1:
        # The paths go in the message, so a GitHub annotation carries them too.
        found = "".join(f"\n  {report}" for report in reports)
        fail(f"expected one coverage.cobertura.xml under {args.results_dir}, found {len(reports)}{found}")
        return 2

    try:
        root = ET.parse(reports[0]).getroot()
    except ET.ParseError as error:
        fail(f"could not parse {reports[0]}: {error}")
        return 2
    # The exact counts, not the line-rate attribute: coverlet rounds that to four decimals, which
    # would let 94.996% pass a 95% minimum. With nothing measured, line-rate is 1, so refuse that too.
    counts = {}
    for name in ("lines-covered", "lines-valid"):
        try:
            counts[name] = int(root.get(name))
        except (TypeError, ValueError):
            fail(f"the report has no usable {name} count: {reports[0]}")
            return 2
    covered, valid = counts["lines-covered"], counts["lines-valid"]
    if valid == 0:
        fail(f"the report measured no lines: {reports[0]}")
        return 2
    line_rate = 100 * covered / valid
    shown = 10000 * covered // valid / 100  # rounded down, so 94.996% never reads as 95.00%
    print(f"Line coverage {shown:.2f}% ({covered} of {valid} lines); minimum {args.minimum:.2f}%")

    gaps = []
    for cls in root.iter("class"):
        try:
            rate = float(cls.get("line-rate")) * 100
        except (TypeError, ValueError):
            fail(f"class {cls.get('name')} has no usable line-rate: {reports[0]}")
            return 2
        # Only the class's own <lines>: each line also appears under its method.
        lines = cls.find("lines")
        missed = [] if lines is None else [line.get("number") for line in lines.iter("line") if line.get("hits") == "0"]
        if missed:
            gaps.append((rate, cls.get("filename"), cls.get("name"), missed))
    for rate, filename, name, missed in sorted(gaps):
        print(f"  {rate:6.2f}%  {filename}  {name}  missed lines: {', '.join(missed)}")

    write_job_summary(line_rate, shown, covered, valid, args.minimum, gaps)

    if line_rate < args.minimum:
        fail(f"line coverage {shown:.2f}% is below the {args.minimum:.2f}% minimum")
        return 1
    return 0


def fail(message):
    if os.environ.get("GITHUB_ACTIONS") == "true":
        # A workflow command is one line, so encode the characters that would end or alter it.
        escaped = message.replace("%", "%25").replace("\r", "%0D").replace("\n", "%0A")
        print(f"::error title=Coverage::{escaped}")
    else:
        print(f"error: {message}", file=sys.stderr)


def write_job_summary(line_rate, shown, covered, valid, minimum, gaps):
    summary = os.environ.get("GITHUB_STEP_SUMMARY")
    if summary:
        verdict = "meets" if line_rate >= minimum else "is below"
        with open(summary, "a", encoding="utf-8") as f:
            f.write("### NoName.Diagnostics.Core coverage\n\n")
            f.write(f"Line coverage **{shown:.2f}%** ({covered} of {valid} lines) {verdict} the {minimum:.2f}% minimum.\n\n")
            if gaps:
                f.write("| Coverage | Class | Missed lines |\n| ---: | --- | --- |\n")
                for rate, _, name, missed in sorted(gaps):
                    f.write(f"| {rate:.1f}% | `{name}` | {', '.join(missed)} |\n")


if __name__ == "__main__":
    sys.exit(main())
