using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace SaneRNG.Common.UI {
	[Autoload(Side = ModSide.Client)]
	internal class AnglerUISystem : ModSystem {
		private UserInterface? userInterface;
		private GameTime? lastUpdateUiGameTime;

		public override void Load() {
			if (!Main.dedServ) {
				UIState anglerUI = new AnglerUI();
				anglerUI.Activate();
				userInterface = new UserInterface();
				userInterface.SetState(anglerUI);
			}
		}

		public override void UpdateUI(GameTime gameTime) {
			lastUpdateUiGameTime = gameTime;

			// Check if player is talking to Angler
			bool talkingToAngler = Main.LocalPlayer.talkNPC != -1 && Main.npc[Main.LocalPlayer.talkNPC].type == NPCID.Angler;

			if (talkingToAngler) {
				// Update UI if shop isn't open
				if (Main.npcShop != 99) {
					userInterface?.Update(gameTime);
				}
			}
		}

		private bool DrawUI() {
			// Only draw if player is talking to Angler and shop isn't already open
			if (Main.LocalPlayer.talkNPC != -1 && Main.npc[Main.LocalPlayer.talkNPC].type == NPCID.Angler && Main.npcShop != 99) {
				userInterface?.Draw(Main.spriteBatch, lastUpdateUiGameTime);
			}
			return true;
		}

		public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers) {
			int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
			if (mouseTextIndex == -1) {
				return;
			}
			layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer("SaneRNG: Angler UI", DrawUI, InterfaceScaleType.UI));
		}
	}
}
