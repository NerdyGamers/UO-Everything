# Agent Instructions for UO-Everything

These instructions apply to all files in this repository.

## Relationship to main README
- Follow the guidance in the root `README.md` for documentation scope, structure, labeling, and contribution workflow.
- When a repeated need arises, it is acceptable to add patch updates to this `AGENTS.md` to clarify process specifics.

## Documentation focus
- This project is a wiki-style knowledge base. Prefer structured Markdown with clear headings, short sections, and relative links between related pages.
- Label content as **Canon** or **Non-Canon** near the top of each document. Include era/shard context when relevant.
- Capture sources (official manuals, patch notes, shard forums, interviews) whenever possible and note when information is community recollection.
- Favor neutral, descriptive writing; represent differing viewpoints with attribution.

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
