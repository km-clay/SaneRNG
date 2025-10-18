using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.DataStructures;
using System.Collections.Generic;
using System.Linq;
using SaneRNG.Content.Items;

namespace SaneRNG.Common.NPCs {
	public class SaneRNGGlobalNPC : GlobalNPC {
		public override void SetDefaults(NPC entity) {
			if (entity.type == NPCID.TravellingMerchant) {
				entity.friendly = false;
			}
		}

		public override void OnSpawn(NPC npc, IEntitySource source) {
			if (npc.type == NPCID.TravellingMerchant) {
				SaneRNGTravelingMerchant.SetupTravelShopWithRequests();
			};
		}
	}

	public class SaneRNGShopper : ModPlayer {
		public override void PostBuyItem(NPC vendor, Item[] shopInventory, Item item) {
			// Here we will add logic to the hook for buying items.
			// Each traveling merchant visit will stock a single voucher.
			// After taking it, it will vanish from the Traveling Merchant's shop.
			if (vendor.type == NPCID.TravellingMerchant && item.type == ModContent.ItemType<RequestVoucher>()) {
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
		public static List<int> requestedItems = new List<int>();

		public override void PreUpdateWorld() {
			if (SaneRNG.DEBUG) {
				ForceSpawnTravelingMerchant();
				Main.time += 29;
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
			int last = requestedItems.Count;

			if (last == 0) {
				return -1;
			}

			int lastRequest = requestedItems[last - 1];
			requestedItems.RemoveAt(last - 1);

			return lastRequest;
		}

		public static void PushRequest(int Request) {
			requestedItems.Add(Request);
		}

		public static void SetupTravelShopWithRequests() {
			Chest.SetupTravelShop();

			int slot = 0;
			while (slot < Main.travelShop.Length && Main.travelShop[slot] != ItemID.None) {
				slot++;
			}

			if (slot == Main.travelShop.Length) {
				return;
			}

			Main.travelShop[slot] = ModContent.ItemType<RequestVoucher>();
			slot++;

			while (requestedItems.Count != 0) {
				// Get our requested item
				int request = PopRequest();

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
