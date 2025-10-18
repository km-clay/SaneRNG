# Sane Farming

Terraria is a great game, but there is no doubt that it has its flaws. Certain items with low drop rates can take an astronomically long time to farm for the unlucky. Obtaining the Fish Finder can take actual in-game months if the Angler just doesn't feel like giving you the components. Certain Traveling Merchant items run the very real risk of literally never appearing even a single time in a playthrough. Statistical anomalies happen, and vanilla Terraria does not do much to address this fact.

**Sane Farming** addresses these issues by providing more predictable and fair methods of obtaining rare items, while remaining largely unobtrusive to the core game experience.

---

## Underground/Surface Chests

Instead of having a set "rare item" that is chosen at world gen, chests contain a biome/location-specific token fragment. These token fragments can eventually be combined into a full token, which can then be crafted into an item from that location's chest drop table. Crates from fishing can contain these fragments as well.

This has the effect of reducing the overall number of items you obtain, but allows you to focus in on specific items that you need.

---

## Angler Quests

The current Angler quest system grants random rewards for completing fishing quests. While it does feature some built-in pity mechanics, high-tier items like the Fish Finder or Angler Tackle Bag still require an unreasonable number of quests to obtain.

**Sane Farming** overhauls the system by awarding **Angler Medals** for each completed quest. Players can exchange these medals for specific quest rewards, similar to the Tavernkeep system. Higher-tier rewards require more medals, so players still need to complete multiple quests for rare items, but they now know exactly how many quests remain to reach their goal.

---

## Traveling Merchant Goods

Several powerful, useful, and cosmetic items (e.g., Zapinator, Pulse Bow, Lifeform Analyzer, DPS Meter, capes) are locked behind the Traveling Merchant’s random item selection. This often results in rare items being unavailable for long stretches.

**Sane Farming** introduces **Request Vouchers**. Each time the Traveling Merchant visits, he brings **one voucher**. Players can spend multiple vouchers to request that specific items appear in his next inventory. The number of vouchers required scales with item rarity, ensuring that rare items still take time to obtain but guaranteeing they will eventually appear.

---

## Item Drops

Enemy drops in Terraria rely on independent RNG for each kill. For common items, this works fine, but for items with very low drop rates (1% or less), the possibility of extremely long unlucky streaks (e.g., 2,000 kills for a 1/500 drop) is unacceptable from a game design perspective. Players should have a sense of how much effort is required before starting a grind.

**Sane Farming** implements a **global pity system** for all item drops. Each item tracks how many enemies a player has defeated without obtaining it. When the number of kills reaches the inverse of the drop chance (e.g. 500 kills for a 1/500 drop chance), the item is guaranteed to drop, unless it has already. For items that can drop from multiple enemies with different probabilities, the system is proportional, so progress is fair regardless of which enemies are being farmed.

The only gameplay change caused by this will be removing the possibility of grinds taking longer than they should. Statistically, items will still drop about as often as they used to.

---

### Design Philosophy

The goal of **Sane Farming** is not to trivialize Terraria or remove the thrill of acquiring rare items. Instead, it ensures that all content is **respectful of the player’s time**. Rare items remain rare, but players can now pursue their goals with **predictable, bounded effort** rather than relying on punishing luck.
