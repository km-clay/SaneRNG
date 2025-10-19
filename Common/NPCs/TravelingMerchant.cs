using Terraria;
using Terraria.GameContent.UI;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.Audio;
using System.Collections.Generic;
using System.Linq;
using SaneRNG.Content.Items;
using SaneRNG.Content.Currencies;
using Microsoft.Xna.Framework;

namespace SaneRNG.Common.NPCs {
	public class SaneRNGGlobalNPC : GlobalNPC {
		public override void SetDefaults(NPC entity) {
			if (SaneRNG.DEBUG && entity.type == NPCID.TravellingMerchant) {
				entity.friendly = true;
			}
		}

		public override void OnSpawn(NPC npc, IEntitySource source) {
			if (npc.type == NPCID.TravellingMerchant) {
				SaneRNGTravelingMerchant.SetupTravelShopWithRequests();
			};
		}

		public override void ModifyActiveShop(NPC npc, string shopName, Item[] items) {
			if (npc.type != NPCID.TravellingMerchant) return;

			if (shopName == "SaneRNG:RequestsPage1") {
				SaneRNGTravelingMerchant.PopulateRequestsPage1(items, npc);
			}
			else if (shopName == "SaneRNG:RequestsPage2") {
				SaneRNGTravelingMerchant.PopulateRequestsPage2(items, npc);
			}
			else if (shopName == "SaneRNG:RequestsPage3") {
				SaneRNGTravelingMerchant.PopulateRequestsPage3(items, npc);
			}
		}
	}

	public class SaneRNGShopper : ModPlayer {
		private static bool purchaseHandled = false;

		public override void PreUpdate() {
			// Reset purchase flag when both mouse buttons are released
			if (!Main.mouseLeft && !Main.mouseRight) {
				purchaseHandled = false;
			}
		}

		public override bool CanBuyItem(NPC vendor, Item[] shopInventory, Item item) {
			if (vendor.type != NPCID.TravellingMerchant) return true;

			// Handle request item purchase - queue the request instead of buying
			if (item.shopSpecialCurrency == RequestVoucherCurrency.id) {
				// Prevent rapid-fire purchasing while mouse held down
				if (purchaseHandled) {
					return false;
				}

				// Check if player can afford the request
				int voucherCount = Player.CountItem(ModContent.ItemType<RequestVoucher>());
				int voucherCost = item.shopCustomPrice ?? 0;

				if (voucherCount >= voucherCost) {
					// Consume the vouchers manually
					for (int i = 0; i < voucherCost; i++) {
						Player.ConsumeItem(ModContent.ItemType<RequestVoucher>());
					}

					// Queue the request
					SaneRNGTravelingMerchant.PushRequest(item.type);
					Main.NewText($"Requested {item.Name} for next visit!", Color.BlueViolet);

					// Play request sound
					SoundEngine.PlaySound(new SoundStyle("SaneRNG/Content/Sound/request"));

					// Remove the item from the shop so it can't be requested again
					item.TurnToAir();

					// Mark that we've handled a purchase this click
					purchaseHandled = true;
				}

				// Return false to prevent the actual item purchase
				return false;
			}

			return true;
		}

		public override void PostBuyItem(NPC vendor, Item[] shopInventory, Item item) {
			if (vendor.type != NPCID.TravellingMerchant) return;

			// Handle voucher purchase - remove from shop after buying
			if (item.type == ModContent.ItemType<RequestVoucher>()) {
				for (int i = 0; i < shopInventory.Length; i++) {
					Item shopItem = shopInventory[i];

					if (!shopItem.IsAir && shopItem.type == ModContent.ItemType<RequestVoucher>()) {
						shopItem.TurnToAir();
						Main.travelShop[i] = ItemID.None;
						break;
					}
				}
			}
		}
	}

	public class SaneRNGTravelingMerchant : ModSystem {
		public static Queue<int> requestedItems = new Queue<int>();
		private static NPCShop requestsShop;

		public override void PostSetupContent() {
			RequestVoucherCurrency.id = CustomCurrencyManager.RegisterCurrency(new RequestVoucherCurrency(ModContent.ItemType<RequestVoucher>(), 9999)); requestsShop = new NPCShop(NPCID.TravellingMerchant, "Requests");

		}

		public static void PopulateRequestsPage1(Item[] items, NPC npc) {
			int slot = 0;

			// Common items - 1 voucher
			AddRequestToShop(items, ref slot, ItemID.YuumaTheBlueTiger, 1);
			AddRequestToShop(items, ref slot, ItemID.SunshineofIsrapony, 1);
			AddRequestToShop(items, ref slot, ItemID.DoNotEattheVileMushroom, 1);
			AddRequestToShop(items, ref slot, ItemID.ParsecPals, 1);
			AddRequestToShop(items, ref slot, ItemID.HoplitePizza, 1);
			AddRequestToShop(items, ref slot, ItemID.Duality, 1);
			AddRequestToShop(items, ref slot, ItemID.BennyWarhol, 1);
			AddRequestToShop(items, ref slot, ItemID.KargohsSummon, 1);
			AddRequestToShop(items, ref slot, ItemID.Stopwatch, 1);
			AddRequestToShop(items, ref slot, ItemID.LifeformAnalyzer, 1);
			AddRequestToShop(items, ref slot, ItemID.DPSMeter, 1);
			AddRequestToShop(items, ref slot, ItemID.LawnFlamingo, 1);
			AddRequestToShop(items, ref slot, ItemID.TeamBlockPink, 1);
			AddRequestToShop(items, ref slot, ItemID.TeamBlockYellow, 1);
			AddRequestToShop(items, ref slot, ItemID.TeamBlockGreen, 1);
			AddRequestToShop(items, ref slot, ItemID.TeamBlockBlue, 1);
			AddRequestToShop(items, ref slot, ItemID.TeamBlockRed, 1);
			AddRequestToShop(items, ref slot, ItemID.TeamBlockWhite, 1);
			AddRequestToShop(items, ref slot, ItemID.DynastyWood, 1);
			AddRequestToShop(items, ref slot, ItemID.RedDynastyShingles, 1);
			AddRequestToShop(items, ref slot, ItemID.BlueDynastyShingles, 1);
			AddRequestToShop(items, ref slot, ItemID.FancyDishes, 1);
			AddRequestToShop(items, ref slot, ItemID.SteampunkCup, 1);
			AddRequestToShop(items, ref slot, ItemID.ZebraSkin, 1);
			AddRequestToShop(items, ref slot, ItemID.LeopardSkin, 1);
			AddRequestToShop(items, ref slot, ItemID.Sake, 1);

			// Uncommon items - 4 vouchers
			if (Main.dontStarveWorld) {
				AddRequestToShop(items, ref slot, ItemID.PaintingWendy, 2);
				AddRequestToShop(items, ref slot, ItemID.PaintingWillow, 2);
				AddRequestToShop(items, ref slot, ItemID.PaintingWolfgang, 2);
				AddRequestToShop(items, ref slot, ItemID.PaintingWilson, 2);
			}

			if (!Main.remixWorld) {
				AddRequestToShop(items, ref slot, ItemID.Katana, 2);
			} else {
				AddRequestToShop(items, ref slot, ItemID.Keybrand, 2);
			}

			AddRequestToShop(items, ref slot, ItemID.ActuationAccessory, 2);
			AddRequestToShop(items, ref slot, ItemID.PortableCementMixer, 2);
			AddRequestToShop(items, ref slot, ItemID.PaintSprayer, 2);
			AddRequestToShop(items, ref slot, ItemID.ExtendoGrip, 2);
			AddRequestToShop(items, ref slot, ItemID.BrickLayer, 2);
			AddRequestToShop(items, ref slot, ItemID.PadThai, 2);
		}

		public static void PopulateRequestsPage2(Item[] items, NPC npc) {
			int slot = 0;

			// Rare items - 6 vouchers
			if (NPC.downedFrost) {
				AddRequestToShop(items, ref slot, ItemID.PaintingTheSeason, 3);
				AddRequestToShop(items, ref slot, ItemID.PaintingSnowfellas, 3);
				AddRequestToShop(items, ref slot, ItemID.PaintingCursedSaint, 3);
				AddRequestToShop(items, ref slot, ItemID.PaintingColdSnap, 3);
				AddRequestToShop(items, ref slot, ItemID.PaintingAcorns, 3);
			}

			if (NPC.downedMoonlord) {
				AddRequestToShop(items, ref slot, ItemID.MoonmanandCompany, 3);
				AddRequestToShop(items, ref slot, ItemID.MoonLordPainting, 3);
			}

			if (NPC.downedMartians) {
				AddRequestToShop(items, ref slot, ItemID.PaintingTheTruthIsUpThere, 3);
				AddRequestToShop(items, ref slot, ItemID.PaintingMartiaLisa, 3);
				AddRequestToShop(items, ref slot, ItemID.PaintingCastleMarsberg, 3);
			}

			if (NPC.downedMechBossAny) {
				AddRequestToShop(items, ref slot, ItemID.Code2, 3);
			}

			if (NPC.downedBoss1) {
				AddRequestToShop(items, ref slot, ItemID.Code1, 3);
			}

			if (Main.hardMode) {
				AddRequestToShop(items, ref slot, ItemID.ZapinatorGray, 3);
			}

			if (WorldGen.shadowOrbSmashed) {
				AddRequestToShop(items, ref slot, ItemID.Revolver, 3);
			}

			AddRequestToShop(items, ref slot, ItemID.UnicornHornHat, 3);
			AddRequestToShop(items, ref slot, ItemID.HeartHairpin, 3);
			AddRequestToShop(items, ref slot, ItemID.StarHairpin, 3);
			AddRequestToShop(items, ref slot, ItemID.VulkelfEar, 3);
			AddRequestToShop(items, ref slot, ItemID.PandaEars, 3);
			AddRequestToShop(items, ref slot, ItemID.DevilHorns, 3);
			AddRequestToShop(items, ref slot, ItemID.DemonHorns, 3);
			AddRequestToShop(items, ref slot, ItemID.LincolnsHood, 3);
			AddRequestToShop(items, ref slot, ItemID.LincolnsHoodie, 3);
			AddRequestToShop(items, ref slot, ItemID.LincolnsPants, 3);
			AddRequestToShop(items, ref slot, ItemID.StarPrincessCrown, 3);
			AddRequestToShop(items, ref slot, ItemID.CelestialWand, 3);
			AddRequestToShop(items, ref slot, ItemID.GameMasterShirt, 3);
			AddRequestToShop(items, ref slot, ItemID.GameMasterPants, 3);
			AddRequestToShop(items, ref slot, ItemID.ChefHat, 3);
			AddRequestToShop(items, ref slot, ItemID.ChefShirt, 3);
			AddRequestToShop(items, ref slot, ItemID.ChefPants, 3);
			AddRequestToShop(items, ref slot, ItemID.Gi, 3);
			AddRequestToShop(items, ref slot, ItemID.GypsyRobe, 3);
			AddRequestToShop(items, ref slot, ItemID.MagicHat, 3);
			AddRequestToShop(items, ref slot, ItemID.Fez, 3);
			AddRequestToShop(items, ref slot, ItemID.Pho, 3);
		}

		public static void PopulateRequestsPage3(Item[] items, NPC npc) {
			int slot = 0;

			// Very Rare items - 8 vouchers
			if (NPC.downedBoss3) {
				AddRequestToShop(items, ref slot, ItemID.SittingDucksFishingRod, 4);
			}

			if (NPC.downedMechBossAny) {
				AddRequestToShop(items, ref slot, ItemID.PulseBow, 4);
			}

			AddRequestToShop(items, ref slot, ItemID.BambooLeaf, 4);
			AddRequestToShop(items, ref slot, ItemID.BedazzledNectar, 4);
			AddRequestToShop(items, ref slot, ItemID.BlueEgg, 4);
			AddRequestToShop(items, ref slot, ItemID.ExoticEasternChewToy, 4);
			AddRequestToShop(items, ref slot, ItemID.BirdieRattle, 4);
			AddRequestToShop(items, ref slot, ItemID.AntiPortalBlock, 4);
			AddRequestToShop(items, ref slot, ItemID.CompanionCube, 4);
			AddRequestToShop(items, ref slot, ItemID.HunterCloak, 4);
			AddRequestToShop(items, ref slot, ItemID.WinterCape, 4);
			AddRequestToShop(items, ref slot, ItemID.RedCape, 4);
			AddRequestToShop(items, ref slot, ItemID.MysteriousCape, 4);
			AddRequestToShop(items, ref slot, ItemID.CrimsonCloak, 4);
			AddRequestToShop(items, ref slot, ItemID.DiamondRing, 4);
			AddRequestToShop(items, ref slot, ItemID.WaterGun, 4);
			AddRequestToShop(items, ref slot, ItemID.YellowCounterweight, 4);

			// Extremely Rare items - 10 vouchers
			if (Main.hardMode) {
				AddRequestToShop(items, ref slot, ItemID.BouncingShield, 4);
				AddRequestToShop(items, ref slot, ItemID.Gatligator, 4);
			}

			AddRequestToShop(items, ref slot, ItemID.ArcaneRuneWall, 4);
			AddRequestToShop(items, ref slot, ItemID.Kimono, 4);
			AddRequestToShop(items, ref slot, ItemID.BlackCounterweight, 4);
			AddRequestToShop(items, ref slot, ItemID.AngelHalo, 4);
		}

		private static void AddRequestToShop(Item[] items, ref int slot, int itemId, int voucherCost) {
			Item item = new Item();
			item.SetDefaults(itemId);
			item.shopCustomPrice = voucherCost;
			item.shopSpecialCurrency = RequestVoucherCurrency.id;
			items[slot] = item;
			slot++;
		}

		public override void PreUpdateWorld() {
			if (SaneRNG.DEBUG) {
				ForceSpawnTravelingMerchant();
			}
		}
		private void ForceSpawnTravelingMerchant() {
			if (Main.dayTime) {
				int travelerIdx = NPC.FindFirstNPC(NPCID.TravellingMerchant);

				if (travelerIdx == -1) {
					int newTraveler = NPC.NewNPC(
						Terraria.Entity.GetSource_TownSpawn(),
						Main.spawnTileX * 16,
						Main.spawnTileY * 16,
						NPCID.TravellingMerchant
					);
					Main.npc[newTraveler].homeless = true;
					Main.npc[newTraveler].netUpdate = true;
				}
			}
		}


		public static int PopRequest() {

			if (requestedItems.Count == 0) {
				return -1;
			}

			int lastRequest = requestedItems.Dequeue();

			return lastRequest;
		}

		public static void PushRequest(int Request) {
			requestedItems.Enqueue(Request);
		}

		public static void SetupTravelShopWithRequests() {
			Chest.SetupTravelShop();

			// Get all items currently in the vanilla shop (excluding None)
			var existingShopItems = Main.travelShop.Where(id => id != ItemID.None).ToHashSet();

			int slot = 0;
			while (slot < Main.travelShop.Length && Main.travelShop[slot] != ItemID.None) {
				slot++;
			}

			if (slot == Main.travelShop.Length) {
				return;
			}

			Main.travelShop[slot] = ModContent.ItemType<RequestVoucher>();
			slot++;

			// Process requested items, skipping duplicates already in the shop
			while (requestedItems.Count != 0) {
				// Get our requested item
				int request = PopRequest();

				// Skip if this item is already in the vanilla shop
				if (existingShopItems.Contains(request)) {
					continue;
				}

				// Add it to the next open slot
				Main.travelShop[slot] = request;
				slot++;

				// If we ran out of space, break.
				// This is an extreme case though, I am doubtful that it will ever happen in practice.
				if (slot == Main.travelShop.Length) {
					break;
				}
			}
		}
	}
}
