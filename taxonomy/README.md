# Taxonomy

`taxonomy.yaml` is the single source of truth for every controlled vocabulary
used in page front matter: page types, eras, expansions, facets, skill groups,
spell schools, mobile families, item categories, location types, canon labels.

Rules:

- Slugs are kebab-case and **stable**. Renaming a slug is a migration: update
  the taxonomy, run the backfill/validator to find every page using the old
  slug, and fix them in one commit.
- Adding a new value (a new expansion, a new mobile family) is a normal edit —
  just keep it alphabetical where the list is a flat list.
- `eras` and `expansions` carry labels and years so tooling can render them
  without a second lookup.
- The validator (`../pipelines/validate_front_matter.py`) enforces these lists;
  the JSON schema marks governed fields with `x-taxonomy`.
