# Loot System

Magic loot in Ultima Online is generated against an intensity budget: the game rolls how many properties an item gets and how strong each one is, scaled to the source's difficulty. Understanding the budget is what separates treasure from trash.

## How it works

Every magic item draws from the [property system](item-properties.md): a property count and per-property intensities, combined into a total intensity score. Tougher sources roll bigger budgets — a mongbat drops low-intensity two-property items, while a peerless boss or champion spawn can drop high-intensity pieces with five or more properties. Treasure maps scale with the map level, and champion-spawn loot is among the strongest random loot in the game (see [champion spawns](../mechanics/champion-spawns.md)).

Most loot loses the roll. Because the budget is random, the overwhelming majority of generated items land in awkward combinations — wrong properties for any real template, or intensities too low to matter. That is why most loot is unravel fodder: [unraveled at a Soul Forge](../crafting/imbuing-system.md) into Magical Residue, Enchanted Essence, or Relic Fragments for imbuing. Serious players evaluate loot by template fit (does any real build want exactly these properties?) and intensity (are the numbers near the property caps?), and discard the rest.

Named [artifacts](artifacts.md) and set pieces sit outside the random system with fixed properties.

## Era notes

Age of Shadows (2003) introduced the intensity-based loot system. Later publishes tuned budgets upward for high-end content (peerless encounters, champion spawns, revamped treasure maps) and added the unraveling loop, which gives junk loot a purpose.

## Sources
- UO playguides (loot generation, Age of Shadows onward)
- UOGuide / Stratics (loot intensity tables — community-documented)
