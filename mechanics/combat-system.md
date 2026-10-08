# Combat System

Melee and ranged combat in Ultima Online resolves through contested skill checks: the attacker's weapon skill against the defender's defensive skill, followed by a damage calculation modified by stats, skills, and equipment.

## How it works

**Hit chance.** Each swing is a contest between the attacker's weapon skill (Swordsmanship, Fencing, Macing, Archery, Throwing, or Wrestling) and the defender's weapon skill, Parrying, or Wrestling. Evenly matched opponents hit roughly half the time; higher attacker skill raises the chance, higher defender skill lowers it. Hit Chance Increase (HCI) and Defense Chance Increase (DCI) from equipment shift the odds, each capped at 45% (community recollection on exact cap behavior across eras).

**Damage.** Base weapon damage is modified by several factors: Tactics adds a damage bonus scaling with skill, Anatomy adds a further bonus for melee weapons, and Strength contributes a smaller bonus. Damage Increase (DI) from items adds a percentage on top, capped at 100% in most circumstances.

**Armor.** Worn armor reduces incoming physical damage through the defender's Physical resistance (see [Resistances](resistances.md)). Armor pieces also carry the other four resists, and the game randomly determines which body location — and thus which armor pieces — absorbs each hit.

**Related systems.** [Swing Speed](swing-speed.md) governs attack rate, [Special Moves](special-moves.md) add mana-cost techniques, and [Resistances](resistances.md) govern damage mitigation. Key skills: [Tactics](../skills/combat/tactics.md), [Anatomy](../skills/combat/anatomy.md), [Healing](../skills/combat/healing.md), [Parrying](../skills/combat/parrying.md).

## Era notes

The pre-Age of Shadows system used raw armor rating absorption; the Age of Shadows (2003) overhaul replaced it with the resistance-based model still in use. Later publishes added HCI/DCI, DI, and SSI item properties.

## Sources
- UO official playguide (combat basics)
- Stratics combat guides
- Community documentation on hit chance and damage formulas
