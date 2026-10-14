#!/usr/bin/env python3
"""Validate YAML front matter on UO-Everything pages (Stage 1).

Checks every content page:
  1. has front matter,
  2. conforms to schema/front-matter.schema.json (structure, required fields,
     conditional requirements),
  3. uses only taxonomy values from taxonomy/taxonomy.yaml (fields marked
     x-taxonomy in the schema),
  4. carries recommended fields (warning, not error): summary, sources.

Usage:
    python validate_front_matter.py --root /path/to/UO-Everything [--quiet]
Exit code 0 = no errors (warnings ok), 1 = errors found.
"""

import argparse
import json
import sys
from pathlib import Path

import yaml
from jsonschema import Draft202012Validator

SKIP_DIRS = {".github", ".git", "UOAssetSuite", "schema", "taxonomy", "templates", "pipelines"}

RECOMMENDED = ["summary", "sources"]


def load_taxonomy(root: Path) -> dict:
    with open(root / "taxonomy" / "taxonomy.yaml", encoding="utf-8") as f:
        return yaml.safe_load(f)


def taxonomy_values(tax: dict, key: str) -> set:
    node = tax[key]
    return set(node.keys()) if isinstance(node, dict) else set(node)


def check_taxonomy(fm: dict, schema_props: dict, tax: dict, rel: Path) -> list[str]:
    errors = []
    for field, spec in schema_props.items():
        if field not in fm:
            continue
        x_tax = spec.get("x-taxonomy")
        items_tax = spec.get("items", {}).get("x-taxonomy")
        key = x_tax or items_tax
        if not key:
            continue
        allowed = taxonomy_values(tax, key)
        values = fm[field] if isinstance(fm[field], list) else [fm[field]]
        for v in values:
            if v not in allowed:
                errors.append(f"{rel}: field '{field}' value '{v}' not in taxonomy '{key}'")
    return errors


def main() -> int:
    ap = argparse.ArgumentParser()
    ap.add_argument("--root", required=True)
    ap.add_argument("--quiet", action="store_true")
    args = ap.parse_args()

    root = Path(args.root)
    with open(root / "schema" / "front-matter.schema.json", encoding="utf-8") as f:
        schema = json.load(f)
    tax = load_taxonomy(root)
    validator = Draft202012Validator(schema)

    files = sorted(
        p for p in root.rglob("*.md")
        if not any(part in SKIP_DIRS for part in p.relative_to(root).parts)
    )

    errors: list[str] = []
    warnings: list[str] = []
    checked = 0
    for path in files:
        rel = path.relative_to(root)
        text = path.read_text(encoding="utf-8")
        if not text.startswith("---\n"):
            errors.append(f"{rel}: missing front matter")
            continue
        end = text.find("\n---\n", 4)
        if end == -1:
            errors.append(f"{rel}: unterminated front matter block")
            continue
        try:
            fm = yaml.safe_load(text[4:end])
        except yaml.YAMLError as e:
            errors.append(f"{rel}: invalid YAML front matter: {e}")
            continue
        if not isinstance(fm, dict):
            errors.append(f"{rel}: front matter is not a mapping")
            continue
        checked += 1
        for e in validator.iter_errors(fm):
            errors.append(f"{rel}: schema: {e.message} (at {'/'.join(str(p) for p in e.path)})")
        errors.extend(check_taxonomy(fm, schema["properties"], tax, rel))
        for field in RECOMMENDED:
            if field not in fm and not fm.get("index_page"):
                warnings.append(f"{rel}: recommended field '{field}' missing")

    if not args.quiet:
        for w in warnings[:20]:
            print(f"WARN  {w}")
        if len(warnings) > 20:
            print(f"WARN  ... and {len(warnings) - 20} more warnings")
        for e in errors[:30]:
            print(f"ERROR {e}")
        if len(errors) > 30:
            print(f"ERROR ... and {len(errors) - 30} more errors")

    print(f"\n{checked} pages checked, {len(files) - checked} missing front matter, "
          f"{len(errors)} errors, {len(warnings)} warnings")
    return 1 if errors else 0


if __name__ == "__main__":
    sys.exit(main())
