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
using System;

namespace SaneRNG.Common.Player {
	public static class PityDropsPacketType {
		public const byte FullSync = 0;
		public const byte ProgressUpdate = 1;
		public const byte PinUpdate = 2;
		public const byte VoucherUpdate = 3;
	}

	public class PityDropsExtractinator : GlobalItem {
		public static (int itemID, int maxStack, double dropPct)[] ExtractinatorDrops = [
			(ItemID.PlatinumCoin, 11, 0.0073),
			(ItemID.GoldCoin, 100, 0.1017),
			(ItemID.SilverCoin, 100, 1.3534),
			(ItemID.CopperCoin, 100, 64.2145),
			(ItemID.AmberMosquito, 1, 0.01),
			(ItemID.Amethyst, 16, 0.3333),
			(ItemID.Topaz, 16, 0.3333),
			(ItemID.Sapphire, 16, 0.3333),
			(ItemID.Emerald, 16, 0.3333),
			(ItemID.Ruby, 16, 0.3333),
			(ItemID.Diamond, 16, 0.3333),
			(ItemID.Amber, 16, 0.3333),
			(ItemID.CopperOre, 16, 3.9192),
			(ItemID.TinOre, 16, 3.9192),
			(ItemID.IronOre, 16, 3.9192),
			(ItemID.LeadOre, 16, 3.9192),
			(ItemID.SilverOre, 16, 3.9192),
			(ItemID.TungstenOre, 16, 3.9192),
			(ItemID.GoldOre, 16, 3.9192),
			(ItemID.PlatinumOre, 16, 3.9192),
			(ItemID.CobaltOre, 16, 3.9192),
			(ItemID.PalladiumOre, 16, 3.9192),
			(ItemID.MythrilOre, 16, 3.9192),
			(ItemID.OrichalcumOre, 16, 3.9192),
			(ItemID.AdamantiteOre, 16, 3.9192),
			(ItemID.TitaniumOre, 16, 3.9192),
		];

		public static (int itemID, int maxStack, double dropPct)[] DesertFossilExtDrops = [
			(ItemID.FossilOre, 7, 10),
			(ItemID.PlatinumCoin, 11, 0.0066),
			(ItemID.GoldCoin, 100, 0.0915),
			(ItemID.SilverCoin, 100, 1.2178),
			(ItemID.CopperCoin, 100, 57.7876),
			(ItemID.AmberMosquito, 1, 0.027),
			(ItemID.Amethyst, 16, 0.15),
			(ItemID.Topaz, 16, 0.15),
			(ItemID.Sapphire, 16, 0.15),
			(ItemID.Emerald, 16, 0.15),
			(ItemID.Ruby, 16, 0.15),
			(ItemID.Diamond, 16, 0.15),
			(ItemID.Amber, 16, 1.7629),
			(ItemID.CopperOre, 16, 1.9938),
			(ItemID.TinOre, 16, 1.9938),
			(ItemID.IronOre, 16, 1.9938),
			(ItemID.LeadOre, 16, 1.9938),
			(ItemID.SilverOre, 16, 1.9938),
			(ItemID.TungstenOre, 16, 1.9938),
			(ItemID.GoldOre, 16, 1.9938),
			(ItemID.PlatinumOre, 16, 1.9938),
			(ItemID.CobaltOre, 16, 1.9938),
			(ItemID.PalladiumOre, 16, 1.9938),
			(ItemID.MythrilOre, 16, 1.9938),
			(ItemID.OrichalcumOre, 16, 1.9938),
			(ItemID.AdamantiteOre, 16, 1.9938),
			(ItemID.TitaniumOre, 16, 1.9938),
		];

		public static (int itemID, int maxStack, double dropPct)[] FishingGarbageExtDrops = [
			(ItemID.Snail, 1, 16.67),
			(ItemID.ApprenticeBait, 1, 75),
			(ItemID.Worm, 1, 5.56),
			(ItemID.JourneymanBait, 1, 2.78),
		];

		public static (int itemID, int maxStack, double dropPct)[] GlowingMossExtrDrops = [
			(ItemID.GreenMoss, 1, 18),
			(ItemID.BrownMoss, 1, 18),
			(ItemID.RedMoss, 1, 18),
			(ItemID.BlueMoss, 1, 18),
			(ItemID.PurpleMoss, 1, 18),
			(ItemID.LavaMoss, 1, 2),
			(ItemID.KryptonMoss, 1, 2),
			(ItemID.XenonMoss, 1, 2),
			(ItemID.ArgonMoss, 1, 2),
			(ItemID.VioletMoss, 1, 2),
		];

		public (int itemID, int maxStack, double dropPct)[] GetExtrDropsFor(int extractType) {
			switch (extractType) {
				case (0):
					return ExtractinatorDrops;

				case (ItemID.DesertFossil):
					return DesertFossilExtDrops;

				case (ItemID.LavaMoss):
					return GlowingMossExtrDrops;

				case (ItemID.OldShoe):
					return FishingGarbageExtDrops;
			}
			return [];
		}

		private bool ExcludeHardmodeMetals(int extractinatorBlockType, int itemID) {
			return (extractinatorBlockType != ItemID.ChlorophyteExtractinator) &&
			(
				itemID == ItemID.CobaltOre ||
				itemID == ItemID.PalladiumOre ||
				itemID == ItemID.MythrilOre ||
				itemID == ItemID.OrichalcumOre ||
				itemID == ItemID.AdamantiteOre ||
				itemID == ItemID.TitaniumOre
			);
		}

		public override void ExtractinatorUse(
			int extractType,
			int extractinatorBlockType,
			ref int resultType,
			ref int resultStack
		) {
			if (!ModContent.GetInstance<SaneRNGServerConfig>().EnableExtractinatorPity) return;
			if (Main.netMode == NetmodeID.Server) return;
			Terraria.Player player = Main.LocalPlayer;
			PityDropsPlayer modPlayer = Main.LocalPlayer.GetModPlayer<PityDropsPlayer>();

			var drops = GetExtrDropsFor(extractType);

			(int, int) forcedDrop = (-1,-1);

			foreach(var (itemID, maxStack, dropRate) in drops) {
				if (dropRate >= 100 || dropRate <= 0) continue;
				if (ExcludeHardmodeMetals(extractinatorBlockType, itemID)) continue;

				modPlayer.AddProgress(itemID, dropRate);

				if (modPlayer.itemProgress.TryGetValue(itemID, out double progress) && progress >= 100f) {
					forcedDrop = (itemID, maxStack);
				}
			}

			if (forcedDrop != (-1,-1)) {
				resultType = forcedDrop.Item1;
				resultStack = Main.rand.Next(1, forcedDrop.Item2 + 1);
				modPlayer.itemProgress[forcedDrop.Item1] = 0f;
			} else if (resultType > 0 && modPlayer.itemProgress.ContainsKey(resultType)) {
				modPlayer.itemProgress[resultType] = 0f;
			}

			modPlayer.SyncPlayer(player.whoAmI, player.whoAmI, false);
		}
	}

	public class PityDropsGrabBags : GlobalItem {
		private static int[] grabBags = [
			ItemID.WoodenCrate,
			ItemID.WoodenCrateHard,
			ItemID.IronCrate,
			ItemID.IronCrateHard,
			ItemID.GoldenCrate,
			ItemID.GoldenCrateHard,
			ItemID.JungleFishingCrate,
			ItemID.JungleFishingCrateHard,
			ItemID.FloatingIslandFishingCrate,
			ItemID.FloatingIslandFishingCrateHard,
			ItemID.CorruptFishingCrate,
			ItemID.CorruptFishingCrateHard,
			ItemID.CrimsonFishingCrate,
			ItemID.CrimsonFishingCrateHard,
			ItemID.HallowedFishingCrate,
			ItemID.HallowedFishingCrateHard,
			ItemID.DungeonFishingCrate,
			ItemID.DungeonFishingCrateHard,
			ItemID.FrozenCrate,
			ItemID.FrozenCrateHard,
			ItemID.OasisCrate,
			ItemID.OasisCrateHard,
			ItemID.LavaCrate,
			ItemID.LavaCrateHard,
			ItemID.OceanCrate,
			ItemID.OceanCrateHard,
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

			var itemLoot = Main.ItemDropsDB.GetRulesForItemID(item.type);

			foreach (var rule in itemLoot) {
				List<DropRateInfo> dropInfos = new();
				DropRateInfoChainFeed chainInfo = new DropRateInfoChainFeed(1f);
				rule.ReportDroprates(dropInfos, chainInfo);

				foreach(var info in dropInfos) {
					bool canDrop = true;
					if (info.conditions != null) {
						foreach (var condition in info.conditions) {
							DropAttemptInfo attemptInfo = new DropAttemptInfo {
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

					if (canDrop && info.dropRate < 1f && info.dropRate > 0f) {
						result.Add((info.itemId, info.dropRate * 100f));
					}
				}
			}

			return result;
		}

		public override bool ConsumeItem(Item item, Terraria.Player player) {
			if (!grabBags.Contains(item.type)) return true;

			var modPlayer = player.GetModPlayer<PityDropsPlayer>();
			modPlayer.PruneRecentDrops();
			var drops = GetItemDrops(item, player);

			foreach (var (itemID, dropRate) in drops) {
				if (dropRate == 100f || dropRate == 0f) continue;
				modPlayer.AddProgress(itemID, dropRate);
			}

			foreach (var (itemID,_) in drops) {
				if (modPlayer.itemProgress.TryGetValue(itemID, out double progress) && progress >= 100f) {
					Item.NewItem(
						new EntitySource_ItemOpen(player, item.type, context: "forced_drop"),
						(int)player.position.X,
						(int)player.position.Y,
						(int)player.Size.X,
						(int)player.Size.Y,
						itemID
					);

					modPlayer.itemProgress[itemID] = 0f;
					modPlayer.recentForcedDrops.Add((Main.GameUpdateCount, itemID));
				}
			}

			return true;
		}
	}

	public class PityDropsPlayer : ModPlayer {
		public Dictionary<int, double> itemProgress = new();
		public HashSet<int> pinnedItems = new();
		public HashSet<(uint,int)> recentForcedDrops = new();

		public override void ProcessTriggers(TriggersSet triggersSet) {
			if (PityDropsProgress.OpenProgressTracker.JustPressed) {
				if (ModContent.GetInstance<SaneRNGServerConfig>().EnablePityDrops == false) {
					Main.NewText("Pity Drops is disabled on this server.", Color.Red);
				} else {
					ModContent.GetInstance<PityProgressUISystem>().ToggleUI();
				}
			}
		}

		public override void SaveData(Terraria.ModLoader.IO.TagCompound tag) {
			if (itemProgress.Count > 0) {
				var itemIDs = new List<int>();
				var progressValues = new List<double>();

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
			try {
				var itemIDs = tag.Get<List<int>>("itemIDs") ?? new();
				var progressValues = tag.Get<List<double>>("progressValues") ?? new();

				for (int i = 0; i < itemIDs.Count; i++) {
					int key = itemIDs[i];
					double value = progressValues[i];
					itemProgress[key] = value;
				}

				var pinned = tag.Get<List<int>>("pinnedItems") ?? new();
				pinnedItems = new HashSet<int>(pinned);
			} catch (Exception e) {
				Mod.Logger.Warn("Failed to load PityDropsPlayer data: " + e.Message);
				itemProgress = new();
				pinnedItems = new();
			}
		}

		public override void SyncPlayer(int toWho, int fromWho, bool newPlayer) {
			if (Main.netMode == NetmodeID.SinglePlayer) return;
			ModPacket packet = Mod.GetPacket();

			packet.Write(PityDropsPacketType.FullSync);
			packet.Write((byte)Player.whoAmI);

			packet.Write(itemProgress.Count);
			foreach (var kvp in itemProgress) {
				packet.Write(kvp.Key);
				packet.Write(kvp.Value);
			}

			packet.Write(pinnedItems.Count);
			foreach (var itemID in pinnedItems) {
				packet.Write(itemID);
			}

			packet.Send(toClient: toWho);
		}

		private bool IsMechBossSummonItem(int itemID) {
			return (itemID == ItemID.MechanicalEye || itemID == ItemID.MechanicalWorm || itemID == ItemID.MechanicalSkull);
		}

		public void AddProgress(int itemID, double pct) {
			if (IsMechBossSummonItem(itemID) && !Main.hardMode) {
				// Don't add progress for mech boss summon items if we're not in hardmode yet.
				return;
			}

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
				if (itemProgress.TryGetValue(itemID, out double progress) && progress >= 100f) {
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
				if (itemProgress.TryGetValue(itemID, out double progress) && progress >= 100f) {
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
			if (ModContent.GetInstance<SaneRNGServerConfig>().EnablePityDrops == false) return;
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
			player.SyncPlayer(killer.whoAmI, killer.whoAmI, false);
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
					if (SaneRNG.DEBUG) {
						Main.NewText(Main.GameUpdateCount + " Prevented duplicate drop of " + item.Name + " from " + npc.FullName, Color.Red);
					}

					// the game has naturally rolled a drop on an item we just forced to spawn.
					// destroy it with utter impunity.
					item.TurnToAir();
					item.active = false;
					item.stack = 0;
					modPlayer.recentForcedDrops.RemoveWhere(drop => drop.Item2 == item.type);
				} else if (modPlayer.itemProgress.ContainsKey(item.type)) {
					modPlayer.itemProgress[item.type] = 0f;
				}
			} else if (source is EntitySource_ItemOpen openSrc) {
				Terraria.Player player = openSrc.Player;
				var modPlayer = player.GetModPlayer<PityDropsPlayer>();

				if (openSrc.Context == "forced_drop") {
					return;
				}

				if (modPlayer.recentForcedDrops.Any(drop => drop.Item2 == item.type)) {
					if (SaneRNG.DEBUG) {
						Main.NewText(Main.GameUpdateCount + " Prevented duplicate drop of " + item.Name + " from opening " + Lang.GetItemNameValue(openSrc.ItemType), Color.Red);
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
