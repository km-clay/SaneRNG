using Terraria;
using Terraria.GameContent.UI;
using Terraria.ModLoader;
using Terraria.ID;
using SaneRNG.Content.Currencies;
using SaneRNG.Content.Items;
using System.Collections.Generic;
using Terraria.DataStructures;

namespace SaneRNG.Common.NPCs {
	public class SaneRNGAnglerGlobalNPC : GlobalNPC {
		public override void ModifyActiveShop(NPC npc, string shopName, Item[] items) {
			if (npc.type != NPCID.Angler) return;

			if (shopName == "SaneRNG:AnglerShop") {
				SaneRNGAngler.PopulateAnglerShop(items);
			}
			else if (shopName == "SaneRNG:AnglerDecor") {
				SaneRNGAngler.PopulateAnglerDecor(items);
			}
		}
	}

	public class SaneRNGAngler : ModSystem {
		public override void Load() {
			On_Player.GetAnglerReward += ReplaceAnglerReward;
		}

		public override void Unload() {
			On_Player.GetAnglerReward -= ReplaceAnglerReward;
		}

		private static void ReplaceAnglerReward(On_Player.orig_GetAnglerReward orig, Terraria.Player self, NPC angler, int questItemType) {
			// Don't call orig() - we're replacing vanilla rewards entirely

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
			AddShopItem(items, ref slot, ItemID.FishermansGuide, 5);
			AddShopItem(items, ref slot, ItemID.WeatherRadio, 5);
			AddShopItem(items, ref slot, ItemID.Sextant, 5);
			AddShopItem(items, ref slot, ItemID.FishingBobber, 8);

			// Low value consumables - 1-4 medals
			AddShopItem(items, ref slot, ItemID.FishingPotion, 2);
			AddShopItem(items, ref slot, ItemID.SonarPotion, 2);
			AddShopItem(items, ref slot, ItemID.CratePotion, 2);
			AddShopItem(items, ref slot, ItemID.MasterBait, 4);
			AddShopItem(items, ref slot, ItemID.JourneymanBait, 3);
			AddShopItem(items, ref slot, ItemID.ApprenticeBait, 2);
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
	}
}
