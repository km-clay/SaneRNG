using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace SaneRNG.Common.UI {
	[Autoload(Side = ModSide.Client)]
	internal class TravelingMerchantUISystem : ModSystem {
		private UserInterface? userInterface;
		private GameTime? lastUpdateUiGameTime;

		public override void Load() {
			if (!Main.dedServ) {
				UIState travelingMerchantUI = new TravelingMerchantUI();
				travelingMerchantUI.Activate();
				userInterface = new UserInterface();
				userInterface.SetState(travelingMerchantUI);
			}
		}

		public override void UpdateUI(GameTime gameTime) {
			lastUpdateUiGameTime = gameTime;

			// Check if player is talking to Traveling Merchant
			bool talkingToMerchant = Main.LocalPlayer.talkNPC != -1 && Main.npc[Main.LocalPlayer.talkNPC].type == NPCID.TravellingMerchant;

			if (talkingToMerchant) {
				// If shop just closed and we're in category mode, restore the page selection dialogue
				if (Main.npcShop == 0 && TravelingMerchantUI.showingRequestCategories && Main.npcChatText == "") {
					Main.npcChatText = "What would you like to request?";
				}

				// Update UI if shop isn't open
				if (Main.npcShop != 99) {
					userInterface?.Update(gameTime);
				}
			}
			else {
				// Reset category state when not talking to merchant
				TravelingMerchantUI.showingRequestCategories = false;
			}
		}

		private bool DrawUI() {
			// Only draw if player is talking to Traveling Merchant and shop isn't already open
			if (Main.LocalPlayer.talkNPC != -1 && Main.npc[Main.LocalPlayer.talkNPC].type == NPCID.TravellingMerchant && Main.npcShop != 99) {
				userInterface?.Draw(Main.spriteBatch, lastUpdateUiGameTime);
			}
			return true;
		}

		public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers) {
			int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
			if (mouseTextIndex == -1) {
				return;
			}
			layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer("SaneRNG: Traveling Merchant UI", DrawUI, InterfaceScaleType.UI));
		}
	}
}
