using Terraria;
using Terraria.GameContent.UI;
using Terraria.ModLoader;
using Terraria.ID;
using SaneRNG.Content.Currencies;
using SaneRNG.Content.Items;
using System.Collections.Generic;
using Terraria.DataStructures;
using Terraria.ModLoader.Default;
using Microsoft.Xna.Framework;
using System.Linq;

namespace SaneRNG.Common.NPCs {
	public class SaneRNGAnglerGlobalNPC : GlobalNPC {
		public override void ModifyActiveShop(NPC npc, string shopName, Item[] items) {
			if (ModContent.GetInstance<SaneRNGServerConfig>().EnableAnglerShops == false) return;
			if (npc.type != NPCID.Angler) return;

			if (shopName == "SaneRNG:AnglerShop") {
				SaneRNGAngler.PopulateAnglerShop(items);
				if (IsThisGuyAtTheFuckingOcean() || IsThisGuyNearThisOtherFuckingNPC(npc)) {
					AddTheFuckingPylons(items);
				}
			}
			else if (shopName == "SaneRNG:AnglerDecor") {
				SaneRNGAngler.PopulateAnglerDecor(items);
			}
		}

		// Hardcode of shame
		private bool IsThisGuyAtTheFuckingOcean() => Main.LocalPlayer.ZoneBeach;
		private bool IsThisGuyNearThisOtherFuckingNPC(NPC npc) {
			int[] likedNPCs = [
				NPCID.Princess,
				NPCID.PartyGirl,
				NPCID.Demolitionist,
				NPCID.TaxCollector
			];
			Vector2 anglerPos = npc.Center;
			foreach (NPC otherNpc in Main.npc) {
				Vector2 otherNpcPos = otherNpc.Center;
				int distance = (int)Vector2.Distance(anglerPos, otherNpcPos);
				if (distance <= 500 && likedNPCs.Contains(otherNpc.type)) {
					return true;
				}
			}
			return false;
		}
		private List<int> GetTheFuckingPylons() {
			List<int> pylons = [];
			if (Main.LocalPlayer.ZoneForest) {
				pylons.Add(ItemID.TeleportationPylonPurity);
			} else if (Main.LocalPlayer.ZoneDesert) {
				pylons.Add(ItemID.TeleportationPylonDesert);
			} else if (Main.LocalPlayer.ZoneJungle) {
				pylons.Add(ItemID.TeleportationPylonJungle);
			} else if (Main.LocalPlayer.ZoneSnow) {
				pylons.Add(ItemID.TeleportationPylonSnow);
			} else if (Main.LocalPlayer.ZoneHallow) {
				pylons.Add(ItemID.TeleportationPylonHallow);
			} else if (Main.LocalPlayer.ZoneGlowshroom) {
				pylons.Add(ItemID.TeleportationPylonMushroom);
			} else if (Main.LocalPlayer.ZoneBeach) {
				pylons.Add(ItemID.TeleportationPylonOcean);
			} else if (Main.LocalPlayer.ZoneNormalCaverns) {
				pylons.Add(ItemID.TeleportationPylonUnderground);
			}

			return pylons;
		}
		private void AddTheFuckingPylons(Item[] items) {
			Stack<int> pylons = new(GetTheFuckingPylons());

			for (int i = 0; i < items.Length; i++) {
				if (items[i] != null) continue;

				if (!pylons.TryPop(out int result)) break;
				Item item = new Item(result);
				items[i] = item;
			}
		}
	}

	public class SaneRNGAngler : ModSystem {
		public override void Load() {
			if (ModContent.GetInstance<SaneRNGServerConfig>().EnableAnglerShops == false) return;
			On_Player.GetAnglerReward += ReplaceAnglerReward;
		}

		public override void Unload() {
			if (ModContent.GetInstance<SaneRNGServerConfig>().EnableAnglerShops == false) return;
			On_Player.GetAnglerReward -= ReplaceAnglerReward;
		}

		private static void ReplaceAnglerReward(On_Player.orig_GetAnglerReward orig, Terraria.Player self, NPC angler, int questItemType) {
			orig(self, angler, questItemType);

			int type = ModContent.ItemType<AnglerMedal>();
			int stack = Main.rand.Next(2, 4); // 2-3 medals
			int index = Item.NewItem(
				new EntitySource_Gift(angler),
				(int)self.position.X,
				(int)self.position.Y,
				self.width,
				self.height,
				type,
				stack
			);

			if (Main.netMode == NetmodeID.MultiplayerClient) {
				NetMessage.SendData(MessageID.SyncItem, -1, -1, null, index, 1f);
			}
		}

		public override void PostSetupContent() {
			AnglerMedalCurrency.id = CustomCurrencyManager.RegisterCurrency(new AnglerMedalCurrency(ModContent.ItemType<AnglerMedal>(), 9999));
		}

		public override void PreUpdateWorld() {
			if (SaneRNG.DEBUG) {
				ForceSpawnAngler();
			}
		}

		private void ForceSpawnAngler() {
			int anglerIdx = NPC.FindFirstNPC(NPCID.Angler);

			if (anglerIdx == -1) {
				int newAngler = NPC.NewNPC(
					Terraria.Entity.GetSource_TownSpawn(),
					Main.spawnTileX * 16,
					Main.spawnTileY * 16,
					NPCID.Angler
				);
				Main.npc[newAngler].homeless = true;
				Main.npc[newAngler].netUpdate = true;
			}
		}

		public static void PopulateAnglerShop(Item[] items) {
			int slot = 0;

			// High value items - 10+ medals
			AddShopItem(items, ref slot, ItemID.HoneyAbsorbantSponge, 15);
			AddShopItem(items, ref slot, ItemID.BottomlessHoneyBucket, 15);
			AddShopItem(items, ref slot, ItemID.GoldenFishingRod, 20);
			if (Main.hardMode) AddShopItem(items, ref slot, ItemID.HotlineFishingHook, 20);
			if (Main.hardMode) AddShopItem(items, ref slot, ItemID.FinWings, 20);
			AddShopItem(items, ref slot, ItemID.BottomlessBucket, 15);
			AddShopItem(items, ref slot, ItemID.SuperAbsorbantSponge, 15);
			AddShopItem(items, ref slot, ItemID.GoldenBugNet, 20);
			AddShopItem(items, ref slot, ItemID.FishHook, 10);

			// Medium value items - 5-9 medals
			AddShopItem(items, ref slot, ItemID.FishMinecart, 8);
			AddShopItem(items, ref slot, ItemID.MermaidAdornment, 5);
			AddShopItem(items, ref slot, ItemID.MermaidTail, 5);
			AddShopItem(items, ref slot, ItemID.SeashellHairpin, 5);
			AddShopItem(items, ref slot, ItemID.FishCostumeFinskirt, 5);
			AddShopItem(items, ref slot, ItemID.FishCostumeShirt, 5);
			AddShopItem(items, ref slot, ItemID.FishCostumeMask, 5);
			AddShopItem(items, ref slot, ItemID.HighTestFishingLine, 5);
			AddShopItem(items, ref slot, ItemID.AnglerEarring, 5);
			AddShopItem(items, ref slot, ItemID.TackleBox, 5);
			AddShopItem(items, ref slot, ItemID.FishermansGuide, 5);
			AddShopItem(items, ref slot, ItemID.WeatherRadio, 5);
			AddShopItem(items, ref slot, ItemID.Sextant, 5);
			AddShopItem(items, ref slot, ItemID.FishingBobber, 8);

			// Low value consumables - 1-4 medals
			AddShopItem(items, ref slot, ItemID.FishingPotion, 2, 3);
			AddShopItem(items, ref slot, ItemID.SonarPotion, 2, 3);
			AddShopItem(items, ref slot, ItemID.CratePotion, 2, 3);
			AddShopItem(items, ref slot, ItemID.MasterBait, 2, 3);
			AddShopItem(items, ref slot, ItemID.JourneymanBait, 2, 5);
			AddShopItem(items, ref slot, ItemID.ApprenticeBait, 2, 10);
		}

		public static void PopulateAnglerDecor(Item[] items) {
			int slot = 0;

			// Decorative items - all 1 medal each (cheap cosmetics)
			AddShopItem(items, ref slot, ItemID.LifePreserver, 1);
			AddShopItem(items, ref slot, ItemID.ShipsWheel, 1);
			AddShopItem(items, ref slot, ItemID.CompassRose, 1);
			AddShopItem(items, ref slot, ItemID.WallAnchor, 1);
			AddShopItem(items, ref slot, ItemID.PillaginMePixels, 1);
			AddShopItem(items, ref slot, ItemID.TreasureMap, 1);
			AddShopItem(items, ref slot, ItemID.GoldfishTrophy, 1);
			AddShopItem(items, ref slot, ItemID.BunnyfishTrophy, 1);
			AddShopItem(items, ref slot, ItemID.SwordfishTrophy, 1);
			AddShopItem(items, ref slot, ItemID.SharkteethTrophy, 1);
			AddShopItem(items, ref slot, ItemID.ShipInABottle, 1);
			AddShopItem(items, ref slot, ItemID.SeaweedPlanter, 1);
			AddShopItem(items, ref slot, ItemID.NotSoLostInParadise, 1);
			AddShopItem(items, ref slot, ItemID.Crustography, 1);
			AddShopItem(items, ref slot, ItemID.WhatLurksBelow, 1);
			AddShopItem(items, ref slot, ItemID.Fangs, 1);
			AddShopItem(items, ref slot, ItemID.CouchGag, 1);
			AddShopItem(items, ref slot, ItemID.SilentFish, 1);
			AddShopItem(items, ref slot, ItemID.TheDuke, 1);
		}

		private static void AddShopItem(Item[] items, ref int slot, int itemID, int medalPrice) {
			Item item = new Item();
			item.SetDefaults(itemID);
			item.shopCustomPrice = medalPrice;
			item.shopSpecialCurrency = AnglerMedalCurrency.id;
			items[slot] = item;
			slot++;
		}

		private static void AddShopItem(Item[] items, ref int slot, int itemID, int medalPrice, int stackSize) {
			Item item = new Item();
			item.SetDefaults(itemID);
			item.stack = stackSize;
			item.shopCustomPrice = medalPrice;
			item.shopSpecialCurrency = AnglerMedalCurrency.id;
			items[slot] = item;
			slot++;
		}
	}
}
