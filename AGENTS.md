# Agent Instructions for UO-Everything

These instructions apply to all files in this repository.

## Relationship to main README
- Follow the guidance in the root `README.md` for documentation scope, structure, labeling, and contribution workflow.
- When a repeated need arises, it is acceptable to add patch updates to this `AGENTS.md` to clarify process specifics.

## Documentation focus
- This project is a wiki-style knowledge base. Prefer structured Markdown with clear headings, short sections, and relative links between related pages.
- Label lore content as **Canon** or **Non-Canon** near the top of each document. Gameplay reference pages (skills, mechanics, crafting, items, etc.) don't need the label.
- Include era/shard context when relevant. Put sources as a simple annotation list at the bottom of each page, and note when information is community recollection.
- Favor neutral, descriptive writing; represent differing viewpoints with attribution.

## Front matter (Stage 1)
- Every content page carries YAML front matter per `schema/front-matter.schema.json`; enum values come from `taxonomy/taxonomy.yaml` (the single source of truth).
- New pages: copy the matching `templates/<type>.md` and fill in what the sources support. Never guess a field the sources don't establish — leave it absent.
- Run `pipelines/validate_front_matter.py` before committing content changes; content CI fails on validation errors.
- `taxonomy.yaml` slugs are stable: renaming one is a migration (update taxonomy, re-validate, fix every affected page in one commit).

## Repository hygiene
- Avoid try/catch around imports in code (if code is added later).
- Keep README files in subdirectories concise and focused on linking related documents.
- When adding new folders, include a short README that explains the contents and links to key files.

## Testing expectation
- For documentation-only changes, tests are not required.
- For code changes, run the appropriate linters/tests before committing and include command outputs in the final summary.

## PR / summary guidance
- Summaries should highlight major documentation additions or restructurings.
- When applicable, mention remaining follow-up tasks or areas needing more detail so contributors know what to tackle next.
