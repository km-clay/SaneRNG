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
	internal class AnglerUI : UIState {
		// Main._textDisplayCache for positioning
		private static readonly object TextDisplayCache = typeof(Main).GetField("_textDisplayCache", BindingFlags.NonPublic | BindingFlags.Instance)!.GetValue(Main.instance)!;
		private static readonly PropertyInfo? AmountOfLines = TextDisplayCache.GetType().GetProperty("AmountOfLines", BindingFlags.Instance | BindingFlags.Public);

		private static bool clickHandled = false; // Prevent retriggering while mouse held

		private bool shopButtonFocused;
		private bool decorButtonFocused;

		public override void Draw(SpriteBatch spriteBatch) {
			if (Main.npcChatText == string.Empty)
				return;

			base.Draw(spriteBatch);

			DrawShopButton(spriteBatch);
			DrawDecorButton(spriteBatch);
		}

		private void DrawShopButton(SpriteBatch spriteBatch) {
			int numLines = (int)AmountOfLines!.GetValue(TextDisplayCache)!;

			Vector2 scale = new(0.9f);
			string text = "Shop";
			Vector2 stringSize = ChatManager.GetStringSize(FontAssets.MouseText.Value, text, scale);

			Vector2 scaleAdjust = new(1f);
			if (stringSize.X > 260f)
				scaleAdjust.X *= 260f / stringSize.X;

			// Angler has multiple vanilla buttons, position after all of them
			float posButton1 = 180 + (Main.screenWidth - 800) / 2f;
			float posButton2 = posButton1 + ChatManager.GetStringSize(FontAssets.MouseText.Value, Language.GetTextValue("LegacyInterface.64"), scale).X + 17f;
			float posButton3 = posButton2 + ChatManager.GetStringSize(FontAssets.MouseText.Value, Language.GetTextValue("LegacyInterface.28"), scale).X + 30f;
			float posButton4 = posButton3 + ChatManager.GetStringSize(FontAssets.MouseText.Value, Language.GetTextValue("LegacyInterface.52"), scale).X + 30f;
			float posShopButton = posButton4 + ChatManager.GetStringSize(FontAssets.MouseText.Value, Language.GetTextValue("LegacyInterface.52"), scale).X + 30f;

			Vector2 position = new(posShopButton, 130 + numLines * 30);

			if (Main.MouseScreen.Between(position, position + stringSize * scale * scaleAdjust.X) && !PlayerInput.IgnoreMouseInterface) {
				Main.LocalPlayer.mouseInterface = true;
				Main.LocalPlayer.releaseUseItem = false;
				scale *= 1.2f;

				if (!shopButtonFocused) {
					SoundEngine.PlaySound(SoundID.MenuTick);
				}

				shopButtonFocused = true;
			}
			else {
				if (shopButtonFocused) {
					SoundEngine.PlaySound(SoundID.MenuTick);
				}

				shopButtonFocused = false;
			}

			ChatManager.DrawColorCodedStringShadow(
				spriteBatch: spriteBatch,
				font: FontAssets.MouseText.Value,
				text: text,
				position: position + stringSize * scaleAdjust * 0.5f,
				baseColor: !shopButtonFocused ? Color.Black : Color.Brown,
				rotation: 0f,
				origin: stringSize * 0.5f,
				baseScale: scale * scaleAdjust
			);

			ChatManager.DrawColorCodedString(
				spriteBatch: spriteBatch,
				font: FontAssets.MouseText.Value,
				text: text,
				position: position + stringSize * scaleAdjust * 0.5f,
				baseColor: !shopButtonFocused ? new Color(228, 206, 114, Main.mouseTextColor / 2) : new Color(255, 231, 69),
				rotation: 0f,
				origin: stringSize * 0.5f,
				baseScale: scale
			);
		}

		private void DrawDecorButton(SpriteBatch spriteBatch) {
			int numLines = (int)AmountOfLines!.GetValue(TextDisplayCache)!;

			Vector2 scale = new(0.9f);
			string text = "Decor";
			Vector2 stringSize = ChatManager.GetStringSize(FontAssets.MouseText.Value, text, scale);

			Vector2 scaleAdjust = new(1f);
			if (stringSize.X > 260f)
				scaleAdjust.X *= 260f / stringSize.X;

			// Angler has multiple vanilla buttons, position after all of them, then after our Shop button
			float posButton1 = 180 + (Main.screenWidth - 800) / 2f;
			float posButton2 = posButton1 + ChatManager.GetStringSize(FontAssets.MouseText.Value, Language.GetTextValue("LegacyInterface.64"), scale).X + 30f;
			float posButton3 = posButton2 + ChatManager.GetStringSize(FontAssets.MouseText.Value, Language.GetTextValue("LegacyInterface.28"), scale).X + 30f;
			float posButton4 = posButton3 + ChatManager.GetStringSize(FontAssets.MouseText.Value, Language.GetTextValue("LegacyInterface.52"), scale).X + 30f;
			float posShopButton = posButton4 + ChatManager.GetStringSize(FontAssets.MouseText.Value, Language.GetTextValue("LegacyInterface.52"), scale).X + 30f;
			float posDecorButton = posShopButton + ChatManager.GetStringSize(FontAssets.MouseText.Value, "Shop", scale).X + 30f;

			Vector2 position = new(posDecorButton, 130 + numLines * 30);

			if (Main.MouseScreen.Between(position, position + stringSize * scale * scaleAdjust.X) && !PlayerInput.IgnoreMouseInterface) {
				Main.LocalPlayer.mouseInterface = true;
				Main.LocalPlayer.releaseUseItem = false;
				scale *= 1.2f;

				if (!decorButtonFocused) {
					SoundEngine.PlaySound(SoundID.MenuTick);
				}

				decorButtonFocused = true;
			}
			else {
				if (decorButtonFocused) {
					SoundEngine.PlaySound(SoundID.MenuTick);
				}

				decorButtonFocused = false;
			}

			ChatManager.DrawColorCodedStringShadow(
				spriteBatch: spriteBatch,
				font: FontAssets.MouseText.Value,
				text: text,
				position: position + stringSize * scaleAdjust * 0.5f,
				baseColor: !decorButtonFocused ? Color.Black : Color.Brown,
				rotation: 0f,
				origin: stringSize * 0.5f,
				baseScale: scale * scaleAdjust
			);

			ChatManager.DrawColorCodedString(
				spriteBatch: spriteBatch,
				font: FontAssets.MouseText.Value,
				text: text,
				position: position + stringSize * scaleAdjust * 0.5f,
				baseColor: !decorButtonFocused ? new Color(228, 206, 114, Main.mouseTextColor / 2) : new Color(255, 231, 69),
				rotation: 0f,
				origin: stringSize * 0.5f,
				baseScale: scale
			);
		}

		public override void Update(GameTime gameTime) {
			base.Update(gameTime);

			// Reset click handled flag when mouse is released
			if (!Main.mouseLeft) {
				clickHandled = false;
			}

			// Handle button clicks
			if (Main.mouseLeft && !clickHandled) {
				if (shopButtonFocused) {
					OpenShop("SaneRNG:AnglerShop");
					clickHandled = true;
				}
				else if (decorButtonFocused) {
					OpenShop("SaneRNG:AnglerDecor");
					clickHandled = true;
				}
			}
		}

		internal static void OpenShop(string shopName) {
			Main.playerInventory = true;
			Main.stackSplit = 9999;
			Main.npcChatText = "";
			Main.SetNPCShopIndex(1);
			Main.instance.shop[Main.npcShop].SetupShop(shopName, Main.LocalPlayer.TalkNPC);
			SoundEngine.PlaySound(SoundID.MenuTick);
		}
	}
}
