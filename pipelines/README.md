# Pipelines (Stage 1)

## backfill_front_matter.py

Derives front matter mechanically and inserts it at the top of pages that lack
it. Derives: `title` (first `#` heading), `type` (directory mapping),
catalog-item fields (`item_id`, `dec_id`, `tile_flags`, `weight`, `height`
parsed from the page body), `boss: true` for `mobiles/bosses/`,
`artifact: true` for `items/notable/`, `index_page: true` for READMEs.

Never guesses. Anything not derivable is left absent for human review.

```bash
pip install -r requirements.txt
python backfill_front_matter.py --root /path/to/UO-Everything        # dry run
python backfill_front_matter.py --root /path/to/UO-Everything --write # apply
python backfill_front_matter.py --root . --write --limit 50          # trial batch
```

## validate_front_matter.py

Every content page must have front matter conforming to
`../schema/front-matter.schema.json`, with enum fields restricted to
`../taxonomy/taxonomy.yaml`. Missing `summary`/`sources` is a warning, not an
error.

```bash
python validate_front_matter.py --root /path/to/UO-Everything
# exit 0 = clean (warnings ok), 1 = errors
```

Intended for CI: fail the build on errors, report warnings.
