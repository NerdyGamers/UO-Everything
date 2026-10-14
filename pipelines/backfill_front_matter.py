#!/usr/bin/env python3
"""Backfill YAML front matter on UO-Everything pages (Stage 1).

Derives only what is mechanically derivable: title (from the first `#` heading),
page type (from the directory path), and for catalog items the ItemID, decimal
ID, and tile data parsed from the page body. Anything the script cannot derive
is left absent so the validator flags it for human review -- never guessed.

Idempotent: files that already have front matter are skipped.
Dry-run by default; pass --write to modify files.

Usage:
    python backfill_front_matter.py --root /path/to/UO-Everything [--write] [--limit N]
"""

import argparse
import re
import sys
from pathlib import Path

import yaml

# Directories that are project infrastructure, not content.
SKIP_DIRS = {".github", ".git", "UOAssetSuite", "schema", "taxonomy", "templates", "pipelines"}

# path-prefix -> page type, longest match wins
TYPE_MAP = [
    ("items/catalog", "item"),
    ("items/notable", "item"),
    ("items", "item"),
    ("mobiles/npcs", "npc"),
    ("mobiles/bosses", "mobile"),
    ("mobiles", "mobile"),
    ("skills/magic/spells", "spell"),
    ("skills/combat/abilities", "ability"),
    ("skills", "skill"),
    ("lore", "lore"),
    ("locations", "location"),
    ("mechanics", "mechanic"),
    ("quests", "quest"),
    ("crafting", "crafting"),
    ("housing", "housing"),
    ("history", "history"),
    ("shards", "shard"),
    ("community", "community"),
]

ITEM_ID_RE = re.compile(r"\*\*Item ID:\*\*\s*`(0x[0-9A-Fa-f]{4})`\s*\((\d+)\)")
FLAGS_RE = re.compile(r"^- Flags:\s*(.+)$", re.MULTILINE)
WEIGHT_RE = re.compile(r"^- Weight:\s*([\d.]+)", re.MULTILINE)
HEIGHT_RE = re.compile(r"^- Height:\s*([\d.]+)", re.MULTILINE)
TITLE_RE = re.compile(r"^#\s+(.+)$", re.MULTILINE)


def page_type_for(rel: Path) -> str | None:
    parts = rel.parent.as_posix()
    for prefix, ptype in TYPE_MAP:
        if parts == prefix or parts.startswith(prefix + "/"):
            return ptype
    return None


def has_front_matter(text: str) -> bool:
    return text.startswith("---\n")


def extract_title(text: str, fallback: str) -> str:
    m = TITLE_RE.search(text)
    return m.group(1).strip() if m else fallback


def parse_catalog_item(text: str, path: Path) -> dict:
    fm: dict = {}
    m = ITEM_ID_RE.search(text)
    if m:
        fm["item_id"] = "0x" + m.group(1)[2:].upper()
        fm["dec_id"] = int(m.group(2))
    else:
        # Fall back to the filename: 0x0F0A.md
        stem = path.stem
        if re.fullmatch(r"0x[0-9A-Fa-f]{4}", stem):
            fm["item_id"] = "0x" + stem[2:].upper()
            fm["dec_id"] = int(stem, 16)
    m = FLAGS_RE.search(text)
    if m:
        fm["tile_flags"] = [f.strip().replace("`", "") for f in m.group(1).split(",")]
    m = WEIGHT_RE.search(text)
    if m:
        fm["weight"] = float(m.group(1))
    m = HEIGHT_RE.search(text)
    if m:
        fm["height"] = float(m.group(1))
    return fm


def build_front_matter(path: Path, rel: Path, text: str) -> dict | None:
    ptype = page_type_for(rel)
    if ptype is None:
        return None
    fm: dict = {}
    if path.name.lower() == "readme.md":
        fm["title"] = extract_title(text, rel.parent.name.replace("-", " ").title())
        fm["type"] = ptype
        fm["index_page"] = True
        return fm
    fm["title"] = extract_title(text, path.stem.replace("-", " ").title())
    fm["type"] = ptype
    if ptype == "item" and rel.parent.name == "catalog":
        fm.update(parse_catalog_item(text, path))
    if ptype == "item" and rel.parent.as_posix() == "items":
        # Top-level items/*.md are category/system overviews, not single items.
        fm["overview_page"] = True
    if ptype == "mobile" and "bosses" in rel.parts:
        fm["boss"] = True
    if ptype == "item" and "notable" in rel.parts:
        fm["artifact"] = True
    return fm


def render(fm: dict) -> str:
    body = yaml.safe_dump(fm, sort_keys=False, allow_unicode=True).strip()
    return f"---\n{body}\n---\n\n"


def main() -> int:
    ap = argparse.ArgumentParser()
    ap.add_argument("--root", required=True, help="Path to the UO-Everything repo")
    ap.add_argument("--write", action="store_true", help="Write changes (default: dry run)")
    ap.add_argument("--limit", type=int, default=0, help="Process at most N files (0 = all)")
    args = ap.parse_args()

    root = Path(args.root)
    files = sorted(
        p for p in root.rglob("*.md")
        if not any(part in SKIP_DIRS for part in p.relative_to(root).parts)
    )
    if args.limit:
        files = files[: args.limit]

    touched, skipped, unknown = 0, 0, 0
    for path in files:
        rel = path.relative_to(root)
        text = path.read_text(encoding="utf-8")
        if has_front_matter(text):
            skipped += 1
            continue
        fm = build_front_matter(path, rel, text)
        if fm is None:
            unknown += 1
            print(f"  ?? no type mapping: {rel}")
            continue
        if args.write:
            path.write_text(render(fm) + text, encoding="utf-8")
        touched += 1
        if touched <= 10 or args.limit <= 20:
            print(f"  {'wrote' if args.write else 'would write'}: {rel} -> {fm}")

    print(f"\n{touched} {'updated' if args.write else 'would update'}, "
          f"{skipped} already have front matter, {unknown} unmapped")
    return 0


if __name__ == "__main__":
    sys.exit(main())
