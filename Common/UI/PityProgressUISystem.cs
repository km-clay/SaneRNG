using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace SaneRNG.Common.UI {
	[Autoload(Side = ModSide.Client)]
	public class PityProgressUISystem : ModSystem {
		private UserInterface pityProgressInterface;
		internal PityProgressUIState pityProgressUI;

		public void ShowUI() {
			pityProgressInterface?.SetState(pityProgressUI);
			SoundEngine.PlaySound(SoundID.MenuOpen);
		}

		public void HideUI() {
			pityProgressInterface?.SetState(null);
			SoundEngine.PlaySound(SoundID.MenuClose);
		}

		public void ToggleUI() {
			if (pityProgressInterface?.CurrentState != null) {
				HideUI();
			} else {
				ShowUI();
			}
		}

		public override void PostSetupContent() {
			pityProgressInterface = new UserInterface();
			pityProgressUI = new PityProgressUIState();
			pityProgressUI.Activate();
		}

		public override void UpdateUI(GameTime gameTime) {
			if (pityProgressInterface?.CurrentState != null) {
				pityProgressInterface?.Update(gameTime);
			}
		}

		public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers) {
			int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
			if (mouseTextIndex != -1) {
				layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
					"SaneRNG: Pity Progress Tracker",
					delegate {
						if (pityProgressInterface?.CurrentState != null) {
							pityProgressInterface.Draw(Main.spriteBatch, new GameTime());
						}
						return true;
					},
					InterfaceScaleType.UI)
				);
			}
		}
	}
}
