# Front-matter spec (Stage 1)

Every content page in this repo carries YAML front matter conforming to
`front-matter.schema.json`. Enum values come from `../taxonomy/taxonomy.yaml`
(the validator enforces them; the schema file marks which fields use which
taxonomy list via `x-taxonomy`).

## Required on every page

```yaml
---
title: Lord British
type: lore
---
```

- `title`: the page's `#` heading, verbatim.
- `type`: one of the `page_types` taxonomy list.

## Conditional requirements

- `type: lore` → `canon` is required (`canon` or `non-canon`).
- `type: item` → `item_id` is required, format `0x` + 4 hex digits (e.g. `0x0F0A`) —
  unless `overview_page: true` (category/system overviews like `items/armor.md`
  cover many entities and carry no single ID).

## Recommended (validator warns, does not fail)

- `summary`: one or two sentences. Feeds search results and, later, RAG chunks.
- `eras`, `expansions`, `facets`: taxonomy slugs when the page is tied to them.
- `related`: relative paths to closely related pages (mirrors the body's cross-links).
- `sources`: the page's source list, duplicated as data.
- `community_recollection: true`: when any fact on the page is hedged as community recollection.

## Type-specific fields

| type | fields |
|---|---|
| `item` | `item_id` (req), `dec_id`, `item_category`, `tile_flags[]`, `weight`, `height`, `artifact` |
| `mobile` | `body_id`, `family`, `boss`, `champion`, `peerless` |
| `npc` | `body_id`, `role`, `location` (relative path) |
| `skill` | `skill_group`, `cap` (100 or 120) |
| `spell` | `school`, `circle` (1–8), `mana`, `skill_required` |
| `ability` | `discipline` (bushido/ninjitsu), `mana` |
| `location` | `location_type`, `facet` (single; use `facets[]` when several apply) |
| `lore` | `canon` (req) |
| `quest` | `role`/`location` reuse where sensible; quest-specific fields land in schema v2 |

## Index pages

Section `README.md` files get:

```yaml
---
title: Mobiles
type: mobile
index_page: true
---
```

They use the section's entity type so tooling can treat them as the section root.

## Naming conventions

- Content files: kebab-case (`ancient-lich.md`).
- Catalog item pages: `0xXXXX.md`, uppercase hex, zero-padded to 4 (`0x0F0A.md`).
- Item thumbnails (Stage 2): `images/items/0xXXXX.png` matching the page name.
- Mobile images (Stage 2): `images/mobiles/<body-id>.png`, idle frame `<body-id>-idle.png`.
- Taxonomy slugs: kebab-case, stable forever — renaming a slug is a migration, not an edit.

## Backfill provenance

Pages filled in by `pipelines/backfill_front_matter.py` carry only derivable
fields (title, type, IDs, tile data). Anything the script could not derive is
left absent so the validator flags it for human review — never guessed.
