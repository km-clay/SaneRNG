using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameInput;
using System.Collections.Generic;
using System.Linq;
using SaneRNG.Common.Player;
using SaneRNG.Common.UI;
using SaneRNG.Common.Keybind;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;

namespace SaneRNG.Common.Player {
	public class PityDropsGrabBags : GlobalItem {
		private static int[] grabBags = [
			ItemID.HerbBag,
			ItemID.CanOfWorms,
			ItemID.LockBox,
			ItemID.ObsidianLockbox,
			ItemID.Oyster,
			ItemID.GoodieBag,
			ItemID.Present,
			ItemID.BossBagBetsy,
			ItemID.DeerclopsBossBag,
			ItemID.KingSlimeBossBag,
			ItemID.EyeOfCthulhuBossBag,
			ItemID.EaterOfWorldsBossBag,
			ItemID.BrainOfCthulhuBossBag,
			ItemID.QueenBeeBossBag,
			ItemID.SkeletronBossBag,
			ItemID.WallOfFleshBossBag,
			ItemID.QueenSlimeBossBag,
			ItemID.DestroyerBossBag,
			ItemID.TwinsBossBag,
			ItemID.SkeletronPrimeBossBag,
			ItemID.PlanteraBossBag,
			ItemID.GolemBossBag,
			ItemID.FishronBossBag,
			ItemID.FairyQueenBossBag,
		];

		private List<(int ItemID, float dropRate)> GetItemDrops(Item item, Terraria.Player player) {
			var result = new List<(int,float)>();

      // Use a fresh sample item instead of the consumed item
      if (!ContentSamples.ItemsByType.TryGetValue(item.type, out Item sampleItem)) {
				return result;
      }

			var itemLoot = new ItemLoot();
      ItemLoader.ModifyItemLoot(sampleItem, itemLoot);  // Use sampleItem instead of item


			foreach (var rule in itemLoot.Get()) {
				List<DropRateInfo> dropInfos = new();
				DropRateInfoChainFeed chainInfo = new DropRateInfoChainFeed(1f);
				rule.ReportDroprates(dropInfos,chainInfo);
				foreach (var info in dropInfos) {
					result.Add((info.itemId, info.dropRate) );
				}
			}

			return result;
		}

		public override bool ConsumeItem(Item item, Terraria.Player player) {
			return true;
			// This code is broken. Like literally does not work. Pretty sure it actually prevents bags from being consumed or something.

			/*
			if (!grabBags.Contains(item.type)) return true;

			var modPlayer = player.GetModPlayer<PityDropsPlayer>();
			var drops = GetItemDrops(item, player);

			foreach (var (itemID,dropRate) in drops) {
				if (dropRate == 1f || dropRate == 0f) continue; // Skip guaranteed or impossible drops

				float percentage = dropRate * 100f;
				modPlayer.AddProgress(itemID,percentage);
			}

			modPlayer.CheckAndForceDrops(drops, item);
			return true;
			*/
		}
	}

	public class PityDropsPlayer : ModPlayer {
		public Dictionary<int, float> itemProgress = new();
		public HashSet<int> pinnedItems = new();
		public HashSet<(uint,int)> recentForcedDrops = new();

		public override void ProcessTriggers(TriggersSet triggersSet) {
			if (PityDropsProgress.OpenProgressTracker.JustPressed) {
				ModContent.GetInstance<PityProgressUISystem>().ToggleUI();
			}
		}

		public override void SaveData(Terraria.ModLoader.IO.TagCompound tag) {
			if (itemProgress.Count > 0) {
				var itemIDs = new List<int>();
				var progressValues = new List<float>();

				foreach (var kvp in itemProgress) {
					itemIDs.Add(kvp.Key);
					progressValues.Add(kvp.Value);
				}

				tag["itemIDs"] = itemIDs;
				tag["progressValues"] = progressValues;
			}

			if (pinnedItems.Count > 0) {
				tag["pinnedItems"] = pinnedItems.ToList();
			}
		}

		public override void LoadData(Terraria.ModLoader.IO.TagCompound tag) {
			var itemIDs = tag.Get<List<int>>("itemIDs") ?? new();
			var progressValues = tag.Get<List<float>>("progressValues") ?? new();

			for (int i = 0; i < itemIDs.Count; i++) {
				int key = itemIDs[i];
				float value = progressValues[i];
				itemProgress[key] = value;
			}

			var pinned = tag.Get<List<int>>("pinnedItems") ?? new();
			pinnedItems = new HashSet<int>(pinned);
		}

		public void AddProgress(int itemID, float pct) {
			if (!itemProgress.ContainsKey(itemID)) {
				itemProgress[itemID] = 0f;
			}
			itemProgress[itemID] += pct;
		}

		public void PruneRecentDrops() {
			recentForcedDrops.RemoveWhere(drop => (Main.GameUpdateCount - drop.Item1 >= 20));
		}

		public void CheckAndForceDrops(List<(int itemID, float dropRate)> drops, NPC npc) {
			if (SaneRNG.DEBUG) {
				Main.NewText(Main.GameUpdateCount + " Checking for forced drops...", Color.Yellow);
			}
			foreach (var (itemID, _) in drops) {
				if (itemProgress.TryGetValue(itemID, out float progress) && progress >= 100f) {
					Item.NewItem(
						new EntitySource_Loot(npc),
						(int)npc.position.X,
						(int)npc.position.Y,
						(int)npc.Size.X,
						(int)npc.Size.Y,
						itemID
					);

					itemProgress[itemID] = 0f;

					recentForcedDrops.Add((Main.GameUpdateCount, itemID));
				}
			}
		}

		public void CheckAndForceDrops(List<(int itemID, float dropRate)> drops, Item grabBag) {
			foreach (var (itemID, _) in drops) {
				if (itemProgress.TryGetValue(itemID, out float progress) && progress >= 100f) {
					Item.NewItem(
						new EntitySource_ItemOpen(Player, grabBag.type),
						(int)Player.position.X,
						(int)Player.position.Y,
						(int)Player.Size.X,
						(int)Player.Size.Y,
						itemID
					);

					itemProgress[itemID] = 0f;

					recentForcedDrops.Add((Main.GameUpdateCount, itemID));
				}
			}
		}
	}
}

namespace SaneRNG.Common.NPCs {
	public class PityDropsGlobalNPC : GlobalNPC {

		private bool isEoWSegment(NPC npc) {
			return (npc.type == NPCID.EaterofWorldsHead
				|| npc.type == NPCID.EaterofWorldsBody
				|| npc.type == NPCID.EaterofWorldsTail);
		}

		public override void OnKill(NPC npc) {
			Terraria.Player killer = Main.player[npc.lastInteraction];
			var player = killer.GetModPlayer<PityDropsPlayer>();
			player.PruneRecentDrops();
			var drops = GetNPCDrops(npc, killer);
			if (SaneRNG.DEBUG) {
				Main.NewText(Main.GameUpdateCount + " Running OnKill for " + npc.FullName + ".", Color.Green);
			}

			foreach (var (itemID, dropRate) in drops) {
				if (dropRate == 1f || dropRate == 0f) continue; // Skip guaranteed or impossible drops


				if (isEoWSegment(npc)) {
					bool eowStillAlive = false;
					for (int i = 0; i < Main.maxNPCs; i++) {
						NPC other = Main.npc[i];
						if (other.active && isEoWSegment(other) && other.whoAmI != npc.whoAmI) {
							eowStillAlive = true;
							break;
						}
					}

					if (eowStillAlive) {
						// Fight is still going, ignore this kill
						return;
					}
				}

				float percentage = dropRate * 100f;
				player.AddProgress(itemID,percentage);
			}

			player.CheckAndForceDrops(drops, npc);
		}

		private List<(int itemID, float dropRate)> GetNPCDrops(NPC npc, Terraria.Player player) {
			var result = new List<(int,float)>();
			var rules = Main.ItemDropsDB.GetRulesForNPCID(npc.type,true);

			foreach (var rule in rules) {
				List<DropRateInfo> dropInfos = new();
				DropRateInfoChainFeed chainInfo = new DropRateInfoChainFeed(1f);
				rule.ReportDroprates(dropInfos,chainInfo);
				foreach (var info in dropInfos) {

					// We can't just naively add the itemid and droprate here
					// We have to actually check to make sure that it's possible
					// for the item to drop in the first place.
					bool canDrop = true;
					if (info.conditions != null) {
						foreach (var condition in info.conditions) {
							DropAttemptInfo attemptInfo = new DropAttemptInfo {
								npc = npc,
								player = player,
								IsExpertMode = Main.expertMode,
								IsMasterMode = Main.masterMode
							};
							if (!condition.CanDrop(attemptInfo)) {
								canDrop = false;
								break;
							}
						}
					}

					if (canDrop) {
						result.Add((info.itemId, info.dropRate) );
					}
				}
			}

			return result;
		}
	}

	public class PityDropsGlobalItem : GlobalItem {
		public override void OnSpawn(Item item, IEntitySource source) {
			if (source is EntitySource_Loot lootSrc && lootSrc.Entity is NPC npc) {
				if (SaneRNG.DEBUG) {
					Main.NewText(Main.GameUpdateCount + " OnSpawn detected drop of " + item.Name + " from " + npc.FullName, Color.Yellow);
				}
				Terraria.Player killer = Main.player[npc.lastInteraction];
				var modPlayer = killer.GetModPlayer<PityDropsPlayer>();

				if (modPlayer.recentForcedDrops.Any(drop => drop.Item2 == item.type)) {
					// the game has naturally rolled a drop on an item we just forced to spawn.
					// destroy it with utter impunity.

					if (SaneRNG.DEBUG) {
						Main.NewText(Main.GameUpdateCount + " Prevented duplicate drop of " + item.Name + " from " + npc.FullName, Color.Red);
					}
					item.TurnToAir();
					item.active = false;
					item.stack = 0;
					modPlayer.recentForcedDrops.RemoveWhere(drop => drop.Item2 == item.type);
				} else if (modPlayer.itemProgress.ContainsKey(item.type)) {
					modPlayer.itemProgress[item.type] = 0f;
				}
			}
		}
	}
}
