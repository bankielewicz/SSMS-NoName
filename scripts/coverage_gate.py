#!/usr/bin/env python3
"""Check line coverage from a coverlet Cobertura report.

Finds the single coverage.cobertura.xml under RESULTS_DIR, copies it to OUTPUT, prints the
overall line coverage and every class below 100% with its missed lines, and exits 1 when the
line coverage is below --minimum. GitHub's code-coverage rule also measures line coverage, so
this gives the same verdict in the workflow log, locally and for pull requests from forks.

Usage: coverage_gate.py RESULTS_DIR OUTPUT --minimum PERCENT
In GitHub Actions it also writes report= and line_rate= to $GITHUB_OUTPUT and a table to the
job summary.
"""
import argparse
import os
import shutil
import sys
import xml.etree.ElementTree as ET
from pathlib import Path


def main(argv=None):
    parser = argparse.ArgumentParser(description=__doc__.splitlines()[0])
    parser.add_argument("results_dir", help="folder that dotnet test --results-directory wrote to")
    parser.add_argument("output", help="where to copy the report for upload")
    parser.add_argument("--minimum", type=float, required=True, help="minimum line coverage, in percent")
    args = parser.parse_args(argv)

    reports = sorted(Path(args.results_dir).rglob("coverage.cobertura.xml"))
    if len(reports) != 1:
        fail(f"expected one coverage.cobertura.xml under {args.results_dir}, found {len(reports)}")
        for report in reports:
            print(f"  {report}", file=sys.stderr)
        return 2

    shutil.copyfile(reports[0], args.output)
    root = ET.parse(args.output).getroot()
    line_rate = float(root.get("line-rate")) * 100
    covered, valid = root.get("lines-covered"), root.get("lines-valid")
    print(f"Line coverage {line_rate:.2f}% ({covered} of {valid} lines); minimum {args.minimum:.2f}%")

    gaps = []
    for cls in root.iter("class"):
        rate = float(cls.get("line-rate")) * 100
        missed = [line.get("number") for line in cls.find("lines").iter("line") if line.get("hits") == "0"]
        if missed:
            gaps.append((rate, cls.get("filename"), cls.get("name"), missed))
    for rate, filename, name, missed in sorted(gaps):
        print(f"  {rate:6.2f}%  {filename}  {name}  missed lines: {', '.join(missed)}")

    write_github_outputs(args.output, line_rate, covered, valid, args.minimum, gaps)

    if line_rate < args.minimum:
        fail(f"line coverage {line_rate:.2f}% is below the {args.minimum:.2f}% minimum")
        return 1
    return 0


def fail(message):
    if os.environ.get("GITHUB_ACTIONS") == "true":
        print(f"::error title=Coverage::{message}")
    else:
        print(f"error: {message}", file=sys.stderr)


def write_github_outputs(report, line_rate, covered, valid, minimum, gaps):
    outputs = os.environ.get("GITHUB_OUTPUT")
    if outputs:
        with open(outputs, "a", encoding="utf-8") as f:
            f.write(f"report={report}\nline_rate={line_rate:.2f}\n")

    summary = os.environ.get("GITHUB_STEP_SUMMARY")
    if summary:
        verdict = "meets" if line_rate >= minimum else "is below"
        with open(summary, "a", encoding="utf-8") as f:
            f.write(f"### NoName.Diagnostics.Core coverage\n\n")
            f.write(f"Line coverage **{line_rate:.2f}%** ({covered} of {valid} lines) {verdict} the {minimum:.0f}% minimum.\n\n")
            if gaps:
                f.write("| Coverage | Class | Missed lines |\n| ---: | --- | --- |\n")
                for rate, _, name, missed in sorted(gaps):
                    f.write(f"| {rate:.1f}% | `{name}` | {', '.join(missed)} |\n")


if __name__ == "__main__":
    sys.exit(main())
