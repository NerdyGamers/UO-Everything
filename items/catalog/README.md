# Item Catalog

Every named item entry from the Ultima Online client tile data — 31,411 entries, one page per item.

## How to use

Pages are filed by item ID in hexadecimal: `0x0000.md` through `0xffff.md`. If you know an item's graphic ID, its page is `0x` plus the ID in 4-digit lowercase hex — for example, the lantern (`0x0A25`) lives at [0x0a25.md](0x0a25.md).

Each page lists the item's tile data straight from the client: flags (weapon, wearable, container, light source, and so on), weight, height, and the other tile fields.

## About the data

These pages are auto-generated from `tiledata.mul` and carry no editorial content — they're the raw catalog. Hand-written articles for notable items live under [Notable items](../notable/README.md), and the game systems around items are covered in the [Items](../README.md) section.

Unnamed tile entries have no page; only entries with a name in the client data are included.
