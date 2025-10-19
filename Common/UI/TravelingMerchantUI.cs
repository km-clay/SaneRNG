using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Reflection;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.UI;
using Terraria.UI.Chat;

namespace SaneRNG.Common.UI {
	internal class TravelingMerchantUI : UIState {
		// Main._textDisplayCache for positioning
		private static readonly object TextDisplayCache = typeof(Main).GetField("_textDisplayCache", BindingFlags.NonPublic | BindingFlags.Instance)!.GetValue(Main.instance)!;
		private static readonly PropertyInfo? AmountOfLines = TextDisplayCache.GetType().GetProperty("AmountOfLines", BindingFlags.Instance | BindingFlags.Public);

		public static bool showingRequestCategories = false;
		private static bool clickHandled = false; // Prevent retriggering while mouse held

		private bool requestsButtonFocused;
		private bool page1ButtonFocused;
		private bool page2ButtonFocused;
		private bool page3ButtonFocused;

		public override void Draw(SpriteBatch spriteBatch) {
			if (Main.npcChatText == string.Empty)
				return;

			base.Draw(spriteBatch);

			if (showingRequestCategories) {
				DrawPageButtons(spriteBatch);
			}
			else {
				DrawRequestsButton(spriteBatch);
			}
		}

		private void DrawRequestsButton(SpriteBatch spriteBatch) {
			int numLines = (int)AmountOfLines!.GetValue(TextDisplayCache)!;

			Vector2 scale = new(0.9f);
			string text = "Requests";
			Vector2 stringSize = ChatManager.GetStringSize(FontAssets.MouseText.Value, text, scale);

			Vector2 scaleAdjust = new(1f);
			if (stringSize.X > 260f)
				scaleAdjust.X *= 260f / stringSize.X;

			float posButton1 = 180 + (Main.screenWidth - 800) / 2f;
			float posButton2 = posButton1 + ChatManager.GetStringSize(FontAssets.MouseText.Value, Language.GetTextValue("LegacyInterface.64"), scale).X + 30f;
			float posRequestsButton = posButton2 + ChatManager.GetStringSize(FontAssets.MouseText.Value, Language.GetTextValue("LegacyInterface.52"), scale).X + 30f;

			Vector2 position = new(posRequestsButton, 130 + numLines * 30);

			if (Main.MouseScreen.Between(position, position + stringSize * scale * scaleAdjust.X) && !PlayerInput.IgnoreMouseInterface) {
				Main.LocalPlayer.mouseInterface = true;
				Main.LocalPlayer.releaseUseItem = false;
				scale *= 1.2f;

				if (!requestsButtonFocused) {
					SoundEngine.PlaySound(SoundID.MenuTick);
				}

				requestsButtonFocused = true;
			}
			else {
				if (requestsButtonFocused) {
					SoundEngine.PlaySound(SoundID.MenuTick);
				}

				requestsButtonFocused = false;
			}

			ChatManager.DrawColorCodedStringShadow(
				spriteBatch: spriteBatch,
				font: FontAssets.MouseText.Value,
				text: text,
				position: position + stringSize * scaleAdjust * 0.5f,
				baseColor: !requestsButtonFocused ? Color.Black : Color.Brown,
				rotation: 0f,
				origin: stringSize * 0.5f,
				baseScale: scale * scaleAdjust
			);

			ChatManager.DrawColorCodedString(
				spriteBatch: spriteBatch,
				font: FontAssets.MouseText.Value,
				text: text,
				position: position + stringSize * scaleAdjust * 0.5f,
				baseColor: !requestsButtonFocused ? new Color(228, 206, 114, Main.mouseTextColor / 2) : new Color(255, 231, 69),
				rotation: 0f,
				origin: stringSize * 0.5f,
				baseScale: scale
			);
		}

		private void DrawPageButtons(SpriteBatch spriteBatch) {
			int numLines = (int)AmountOfLines!.GetValue(TextDisplayCache)!;

			// Start at the same position as the Requests button
			float posButton1 = 180 + (Main.screenWidth - 800) / 2f;
			float posButton2 = posButton1 + ChatManager.GetStringSize(FontAssets.MouseText.Value, Language.GetTextValue("LegacyInterface.64"), new Vector2(0.9f)).X + 30f;
			float posRequestsButton = posButton2 + ChatManager.GetStringSize(FontAssets.MouseText.Value, Language.GetTextValue("LegacyInterface.52"), new Vector2(0.9f)).X + 30f;

			string[] categories = new string[] { "Page 1", "Page 2", "Page 3" };
			bool[] focusStates = new bool[] { page1ButtonFocused, page2ButtonFocused, page3ButtonFocused };

			float spacing = 0;

			for (int i = 0; i < categories.Length; i++) {
				Vector2 scale = new(0.9f);
				string text = categories[i];
				Vector2 stringSize = ChatManager.GetStringSize(FontAssets.MouseText.Value, text, scale);

				Vector2 scaleAdjust = new(1f);
				if (stringSize.X > 260f)
					scaleAdjust.X *= 260f / stringSize.X;

				// Horizontal layout starting from Requests button position
				float baseY = 130 + numLines * 30;
				Vector2 position = new(posRequestsButton + spacing, baseY);
				spacing += stringSize.X * scaleAdjust.X + 30f;

				bool isHovered = Main.MouseScreen.Between(position, position + stringSize * scale * scaleAdjust.X) && !PlayerInput.IgnoreMouseInterface;

				if (isHovered) {
					Main.LocalPlayer.mouseInterface = true;
					Main.LocalPlayer.releaseUseItem = false;
					scale *= 1.2f;

					if (!focusStates[i]) {
						SoundEngine.PlaySound(SoundID.MenuTick);
					}

					SetFocusState(i, true);
				}
				else {
					if (focusStates[i]) {
						SoundEngine.PlaySound(SoundID.MenuTick);
					}

					SetFocusState(i, false);
				}

				ChatManager.DrawColorCodedStringShadow(
					spriteBatch: spriteBatch,
					font: FontAssets.MouseText.Value,
					text: text,
					position: position + stringSize * scaleAdjust * 0.5f,
					baseColor: !focusStates[i] ? Color.Black : Color.Brown,
					rotation: 0f,
					origin: stringSize * 0.5f,
					baseScale: scale * scaleAdjust
				);

				ChatManager.DrawColorCodedString(
					spriteBatch: spriteBatch,
					font: FontAssets.MouseText.Value,
					text: text,
					position: position + stringSize * scaleAdjust * 0.5f,
					baseColor: !focusStates[i] ? new Color(228, 206, 114, Main.mouseTextColor / 2) : new Color(255, 231, 69),
					rotation: 0f,
					origin: stringSize * 0.5f,
					baseScale: scale
				);
			}
		}

		private void SetFocusState(int index, bool state) {
			switch (index) {
				case 0: page1ButtonFocused = state; break;
				case 1: page2ButtonFocused = state; break;
				case 2: page3ButtonFocused = state; break;
			}
		}

		public override void Update(GameTime gameTime) {
			base.Update(gameTime);

			// Reset click handled flag when mouse is released
			if (!Main.mouseLeft) {
				clickHandled = false;
			}

			if (!showingRequestCategories) {
				// Main dialogue - "Requests" button shows page buttons
				if (requestsButtonFocused && Main.mouseLeft && !clickHandled) {
					showingRequestCategories = true;
					Main.npcChatText = "What would you like to request?";
					SoundEngine.PlaySound(SoundID.MenuTick);
					clickHandled = true;
				}
			}
			else {
				// Page buttons - open respective shops
				if (Main.mouseLeft && !clickHandled) {
					if (page1ButtonFocused) {
						OpenRequestsShop("SaneRNG:RequestsPage1");
						showingRequestCategories = false; // Return to showing Requests button
						clickHandled = true;
					}
					else if (page2ButtonFocused) {
						OpenRequestsShop("SaneRNG:RequestsPage2");
						showingRequestCategories = false;
						clickHandled = true;
					}
					else if (page3ButtonFocused) {
						OpenRequestsShop("SaneRNG:RequestsPage3");
						showingRequestCategories = false;
						clickHandled = true;
					}
				}
			}
		}

		internal static void OpenRequestsShop(string shopName) {
			Main.playerInventory = true;
			Main.stackSplit = 9999;
			Main.npcChatText = "";
			Main.SetNPCShopIndex(1);
			Main.instance.shop[Main.npcShop].SetupShop(shopName, Main.LocalPlayer.TalkNPC);
			SoundEngine.PlaySound(SoundID.MenuTick);
			// Keep showingRequestCategories = true so closing shop returns to page menu
		}
	}
}
