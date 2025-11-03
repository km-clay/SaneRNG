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
		private static MouseStateCache mouseCache;

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
			int inventoryIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Inventory"));
			if (inventoryIndex != -1) {
				// Block mouse input before inventory processes it
				layers.Insert(0, new LegacyGameInterfaceLayer(
					"SaneRNG: Mouse Blocker",
					delegate {
						// Don't block mouse if settings or other overlay menus are open
						bool hasOverlay = Main.InGuideCraftMenu || Main.InReforgeMenu ||
						                   Main.playerInventory == false || Main.gameMenu;

						// Always check hover state fresh each frame - don't rely on cached state
						if (!hasOverlay && pityProgressInterface?.CurrentState != null && pityProgressUI.MainPanel.ContainsPoint(Main.MouseScreen)) {
							mouseCache = new MouseStateCache();
							mouseCache.BlockMouse();
						} else {
							// Not hovering - make sure we don't have a stale cache
							mouseCache = null;
						}
						return true;
					},
					InterfaceScaleType.UI)
				);

				// Draw UI and restore mouse AFTER Mouse Text (tooltips)
				int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
				if (mouseTextIndex != -1) {
					// Insert AFTER Mouse Text layer to keep mouse blocked during tooltip processing
					layers.Insert(mouseTextIndex + 1, new LegacyGameInterfaceLayer(
						"SaneRNG: Pity Progress Tracker",
						delegate {
							// Clear any tooltips that were set while mouse was blocked
							if (mouseCache != null && pityProgressUI.MainPanel.ContainsPoint(Main.MouseScreen)) {
								Main.hoverItemName = "";
								Main.HoverItem = new Item();
								Main.mouseText = false;
								Main.instance.MouseText("");
								Main.ItemIconCacheUpdate(0);
							}

							if (mouseCache != null) {
								mouseCache.RestoreMouse();
								mouseCache = null;
							}

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

	// Helper class to cache and restore mouse state
	internal class MouseStateCache {
		private readonly int oldMouseX, oldMouseY;
		private readonly bool oldMouseLeft, oldMouseLeftRelease, oldMouseRight, oldMouseRightRelease;

		public MouseStateCache() {
			oldMouseX = Main.mouseX;
			oldMouseY = Main.mouseY;
			oldMouseLeft = Main.mouseLeft;
			oldMouseLeftRelease = Main.mouseLeftRelease;
			oldMouseRight = Main.mouseRight;
			oldMouseRightRelease = Main.mouseRightRelease;
		}

		public void BlockMouse() {
			Main.mouseX = -1;
			Main.mouseY = -1;
			Main.mouseLeft = false;
			Main.mouseLeftRelease = false;
			Main.mouseRight = false;
			Main.mouseRightRelease = false;

			// Block tooltips
			Main.LocalPlayer.cursorItemIconEnabled = false;
			Main.instance.MouseText("");
			Main.mouseText = false;
		}

		public void RestoreMouse() {
			Main.mouseX = oldMouseX;
			Main.mouseY = oldMouseY;
			Main.mouseLeft = oldMouseLeft;
			Main.mouseLeftRelease = oldMouseLeftRelease;
			Main.mouseRight = oldMouseRight;
			Main.mouseRightRelease = oldMouseRightRelease;
		}
	}
}
