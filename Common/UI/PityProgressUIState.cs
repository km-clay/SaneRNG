using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;
using SaneRNG.Common.Player;
using SaneRNG.Common.UI;
using Terraria.GameContent.UI.Elements;

namespace SaneRNG.Common.UI {
	public class PityProgressUIState : UIState {
		public ResizablePanel MainPanel;
		public UIList ProgressList;
		public UIScrollbar ProgressScrollbar;
		public FocusableTextBox SearchBox;
		public UIHoverImageButton ClearSearchButton;

		public override void OnInitialize() {
			// Main container panel
			MainPanel = new ResizablePanel(this);
			MainPanel.SetPadding(10);
			SetRectangle(MainPanel, left: 100f, top: 100f, width: 400f, height: 500f);
			MainPanel.BackgroundColor = new Color(33, 43, 79) * 0.95f;

			// Title text
			UIText titleText = new UIText("Item Progress Tracker", 1.2f);
			SetRectangle(titleText, left: 10f, top: 5f, width: 250f, height: 30f);
			MainPanel.Append(titleText);

			// Close button
			Asset<Texture2D> buttonCloseTexture = ModContent.Request<Texture2D>("Terraria/Images/UI/SearchCancel");
			UIHoverImageButton closeButton = new UIHoverImageButton(buttonCloseTexture, "Close");
			SetRectangle(closeButton, left: 360f, top: 5f, width: 22f, height: 22f);
			closeButton.OnLeftClick += CloseButtonClicked;
			MainPanel.Append(closeButton);

			// Search box
			SearchBox = new FocusableTextBox("Search...");
			SetRectangle(SearchBox, left: 10f, top: 35f, width: 340f, height: 30f);
			SearchBox.SetPadding(6);
			SearchBox.SetTextMaxLength(50);
			MainPanel.Append(SearchBox);

			// Clear search button
			UIHoverImageButton clearSearchButton = new UIHoverImageButton(buttonCloseTexture, "Clear Search");
			SetRectangle(clearSearchButton, left: 355f, top: 39f, width: 22f, height: 22f);
			clearSearchButton.OnLeftClick += ClearSearchClicked;
			MainPanel.Append(clearSearchButton);

			// Scrollable list for progress items
			ProgressList = new UIList();
			SetRectangle(ProgressList, left: 0f, top: 70f, width: 355f, height: 410f);
			ProgressList.SetPadding(5);
			ProgressList.ListPadding = 5f;
			// Disable automatic sorting to preserve our custom order (pinned items first)
			ProgressList.ManualSortMethod = (list) => { };
			MainPanel.Append(ProgressList);

			// Scrollbar for the list
			ProgressScrollbar = new UIScrollbar();
			SetRectangle(ProgressScrollbar, left: 360f, top: 70f, width: 20f, height: 385f); // Reduced from 410 to avoid resize handle
			MainPanel.Append(ProgressScrollbar);
			ProgressList.SetScrollbar(ProgressScrollbar);

			Append(MainPanel);
		}

		private void SetRectangle(UIElement uiElement, float left, float top, float width, float height) {
			uiElement.Left.Set(left, 0f);
			uiElement.Top.Set(top, 0f);
			uiElement.Width.Set(width, 0f);
			uiElement.Height.Set(height, 0f);
		}

		private void CloseButtonClicked(UIMouseEvent evt, UIElement listeningElement) {
			SoundEngine.PlaySound(SoundID.MenuClose);
			ModContent.GetInstance<PityProgressUISystem>().HideUI();
		}

		private void ClearSearchClicked(UIMouseEvent evt, UIElement listeningElement) {
			SoundEngine.PlaySound(SoundID.MenuClose);
			PityProgressTextInputSystem.textInput = "";
		}

		public void UpdateListHeight(float panelHeight) {
			float listHeight = panelHeight - 90f; // Account for title, search bar, and padding
			ProgressList.Height.Set(listHeight, 0f);
			ProgressScrollbar.Height.Set(listHeight - 25f, 0f); // Reduce by 25px to avoid resize handle
			ProgressList.Recalculate();
			ProgressScrollbar.Recalculate();
		}

		public override void Update(GameTime gameTime) {
			base.Update(gameTime);

			// Only block mouse input when hovering over the panel
			if (MainPanel.ContainsPoint(Main.MouseScreen)) {
				Main.LocalPlayer.mouseInterface = true;
			}

			// Refresh the progress list every frame
			RefreshProgressList();
		}

		private void RefreshProgressList() {
			var player = Main.LocalPlayer.GetModPlayer<PityDropsPlayer>();

			// Clear existing elements
			ProgressList.Clear();

			if (player.itemProgress.Count == 0) {
				UIText noProgressText = new UIText("No items being tracked yet!", 0.9f);
				noProgressText.TextColor = Color.Gray;
				SetRectangle(noProgressText, 0f, 0f, 360f, 30f);
				ProgressList.Add(noProgressText);
				return;
			}

			// Get search text
			string searchText = SearchBox.Text.Trim();
			bool isSearching = !string.IsNullOrEmpty(searchText) && searchText != "Search...";

			// Create a single list of all items with their pin status and sort priority
			var allItems = new List<(int itemID, double progress, bool isPinned, int sortPriority)>();

			foreach (var kvp in player.itemProgress) {
				// Apply search filter if needed
				if (isSearching) {
					string itemName = Lang.GetItemNameValue(kvp.Key);
					if (!itemName.Contains(searchText, StringComparison.OrdinalIgnoreCase)) {
						continue; // Skip items that don't match search
					}
				}

				bool isPinned = player.pinnedItems.Contains(kvp.Key);
				// Pinned items get priority 0, unpinned get priority 1
				int sortPriority = isPinned ? 0 : 1;
				allItems.Add((kvp.Key, kvp.Value, isPinned, sortPriority));
			}

			// Show "no results" message if nothing matches
			if (allItems.Count == 0) {
				UIText noResultsText = new UIText(isSearching ? "No items match your search." : "No items being tracked yet!", 0.9f);
				noResultsText.TextColor = Color.Gray;
				SetRectangle(noResultsText, 0f, 0f, 360f, 30f);
				ProgressList.Add(noResultsText);
				return;
			}

			// Sort: first by pin status (pinned first), then by progress (highest first)
			allItems.Sort((a, b) => {
				// First compare by sort priority (0 for pinned, 1 for unpinned)
				int priorityCompare = a.sortPriority.CompareTo(b.sortPriority);
				if (priorityCompare != 0) return priorityCompare;
				// If same priority, sort by progress descending
				string bName = Lang.GetItemNameValue(b.itemID);
				string aName = Lang.GetItemNameValue(a.itemID);
				return aName.CompareTo(bName);
			});

			// Add all items in sorted order
			foreach (var (itemID, progress, isPinned, _) in allItems) {
				UIProgressItem progressItem = new UIProgressItem(itemID, progress, isPinned);
				progressItem.Width.Set(360f, 0f);
				progressItem.Height.Set(50f, 0f);
				ProgressList.Add(progressItem);
			}
		}
	}

	// Custom UI element for displaying individual item progress
	public class UIProgressItem : UIElement {
		private int itemID;
		private double progress;
		private bool isPinned;

		public UIProgressItem(int itemID, double progress, bool isPinned) {
			this.itemID = itemID;
			this.progress = progress;
			this.isPinned = isPinned;
			this.Height.Set(50f, 0f);
			this.Width.Set(360f, 0f);
		}

		public override void LeftClick(UIMouseEvent evt) {
			base.LeftClick(evt);
			// Toggle pin status
			var player = Main.LocalPlayer.GetModPlayer<PityDropsPlayer>();
			if (player.pinnedItems.Contains(itemID)) {
				player.pinnedItems.Remove(itemID);
			} else {
				player.pinnedItems.Add(itemID);
			}
			SoundEngine.PlaySound(SoundID.MenuTick);
			int playerIdx = Main.LocalPlayer.whoAmI;
			player.SyncPlayer(playerIdx, playerIdx, false);
		}

		protected override void DrawSelf(SpriteBatch spriteBatch) {
			CalculatedStyle dimensions = GetDimensions();
			float x = dimensions.X;
			float y = dimensions.Y;
			float width = dimensions.Width;

			// Draw background
			Color bgColor = isPinned ? new Color(40, 50, 100) * 0.9f : new Color(26, 40, 89) * 0.8f;
			spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle((int)x, (int)y, (int)width, 50), bgColor);

			// Draw item icon (shift right if pinned to make room for star)
			Main.instance.LoadItem(itemID);
			Texture2D itemTexture = TextureAssets.Item[itemID].Value;
			float scale = 32f / Math.Max(itemTexture.Width, itemTexture.Height);
			float iconX = isPinned ? 35f : 25f;
			spriteBatch.Draw(itemTexture, new Vector2(x + iconX, y + 25), null, Color.White, 0f, itemTexture.Size() / 2f, scale, SpriteEffects.None, 0f);

			// Draw pin indicator if pinned (draw AFTER icon so it appears on top)
			if (isPinned) {
				Utils.DrawBorderString(spriteBatch, "★", new Vector2(x + 8, y + 18), Color.Gold, 1.0f);
			}

			// Draw item name (shift right if pinned)
			string itemName = Lang.GetItemNameValue(itemID);
			float nameX = isPinned ? 65f : 55f;
			Utils.DrawBorderString(spriteBatch, itemName, new Vector2(x + nameX, y + 8), Color.White, 0.9f);

			// Draw progress bar (adjust for pinned items)
			float barWidth = isPinned ? width - 130 : width - 120;
			float barHeight = 16f;
			float barX = isPinned ? x + 65 : x + 55;
			float barY = y + 28;

			// Background of progress bar
			spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle((int)barX, (int)barY, (int)barWidth, (int)barHeight), Color.Black * 0.5f);

			// Filled portion of progress bar
			double fillWidth = (barWidth - 4) * Math.Min(progress / 100f, 1f);
			Color barColor = progress >= 100f ? Color.Gold : Color.LimeGreen;
			spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle((int)barX + 2, (int)barY + 2, (int)fillWidth, (int)barHeight - 4), barColor * 0.8f);

			// Progress text
			string progressText = $"{progress:F1}%";
			Utils.DrawBorderString(spriteBatch, progressText, new Vector2(barX + barWidth + 10, barY), Color.White, 0.85f);
		}
	}

	// Resizable and draggable panel
	public class ResizablePanel : UIPanel {
		private Vector2 offset;
		private bool dragging;
		private bool resizing;
		private float minHeight = 160f; // Title (30) + Search (30) + Padding (20) + 1 Item (55) + some space (25)
		private float maxHeight = 800f;
		private PityProgressUIState parentState;
		private Asset<Texture2D> resizeTexture;

		public ResizablePanel(PityProgressUIState parent) {
			parentState = parent;
			resizeTexture = ModContent.Request<Texture2D>("SaneRNG/Assets/Resize");
		}

		public override void LeftMouseDown(UIMouseEvent evt) {
			base.LeftMouseDown(evt);

			// Check if clicking on resize handle
			CalculatedStyle dimensions = GetDimensions();
			Rectangle resizeHandle = new Rectangle((int)(dimensions.X + dimensions.Width - 20), (int)(dimensions.Y + dimensions.Height - 20), 20, 20);
			if (resizeHandle.Contains(Main.MouseScreen.ToPoint())) {
				resizing = true;
				return;
			}

			// Don't start dragging if clicking on an interactive child element
			UIElement target = evt.Target;
			if (target is FocusableTextBox || target is UIScrollbar || target.Parent is UIScrollbar || target is UIHoverImageButton || target is UIProgressItem) {
				return;
			}

			DragStart(evt);
		}

		public override void LeftMouseUp(UIMouseEvent evt) {
			base.LeftMouseUp(evt);

			// Stop resizing or dragging
			resizing = false;
			if (dragging) {
				DragEnd(evt);
			}
		}

		private void DragStart(UIMouseEvent evt) {
			offset = new Vector2(evt.MousePosition.X - Left.Pixels, evt.MousePosition.Y - Top.Pixels);
			dragging = true;
		}

		private void DragEnd(UIMouseEvent evt) {
			Vector2 endMousePosition = evt.MousePosition;
			dragging = false;

			Left.Set(endMousePosition.X - offset.X, 0f);
			Top.Set(endMousePosition.Y - offset.Y, 0f);

			Recalculate();
		}

		public override void Update(GameTime gameTime) {
			base.Update(gameTime);

			// Stop dragging/resizing if mouse button is released
			if (!Main.mouseLeft) {
				dragging = false;
				resizing = false;
			}

			if (resizing) {
				float newHeight = Main.mouseY - Top.Pixels;
				newHeight = Utils.Clamp(newHeight, minHeight, maxHeight);
				Height.Set(newHeight, 0f);
				Recalculate();
				parentState.UpdateListHeight(newHeight);
			}

			if (dragging) {
				Left.Set(Main.mouseX - offset.X, 0f);
				Top.Set(Main.mouseY - offset.Y, 0f);
				Recalculate();
			}

			// Here we check if the panel is outside the Parent's dimensions.
			var parentSpace = Parent.GetDimensions().ToRectangle();
			if (!GetDimensions().ToRectangle().Intersects(parentSpace)) {
				Left.Pixels = Utils.Clamp(Left.Pixels, 0, parentSpace.Right - Width.Pixels);
				Top.Pixels = Utils.Clamp(Top.Pixels, 0, parentSpace.Bottom - Height.Pixels);
				Recalculate();
			}
		}

		protected override void DrawSelf(SpriteBatch spriteBatch) {
			base.DrawSelf(spriteBatch);

			// Draw resize handle
			CalculatedStyle dimensions = GetDimensions();
			Vector2 resizePos = new Vector2(dimensions.X + dimensions.Width - 20, dimensions.Y + dimensions.Height - 20);
			spriteBatch.Draw(resizeTexture.Value, resizePos, Color.White);
		}
	}

	// Hover image button (simplified from ExampleMod)
	public class UIHoverImageButton : UIElement {
		private Asset<Texture2D> texture;
		private string hoverText;

		public UIHoverImageButton(Asset<Texture2D> texture, string hoverText) {
			this.texture = texture;
			this.hoverText = hoverText;
		}

		protected override void DrawSelf(SpriteBatch spriteBatch) {
			CalculatedStyle dimensions = GetDimensions();
			spriteBatch.Draw(texture.Value, dimensions.Position(), Color.White);

			if (IsMouseHovering) {
				Main.hoverItemName = hoverText;
			}
		}

		public override void MouseOver(UIMouseEvent evt) {
			base.MouseOver(evt);
			SoundEngine.PlaySound(SoundID.MenuTick);
		}
	}

	// Focusable text box that handles keyboard input
	public class FocusableTextBox : UIElement {
		private bool focused = false;
		private int cursorTimer = 0;

		public string Text => PityProgressTextInputSystem.textInput;

		public FocusableTextBox(string placeholder) {
			Width.Set(370f, 0f);
			Height.Set(30f, 0f);
		}

		public void SetTextMaxLength(int maxLength) {
			// Stored for later use if needed
		}

		public override void LeftClick(UIMouseEvent evt) {
			base.LeftClick(evt);
			Focus();
		}

		private void Focus() {
			if (!focused) {
				focused = true;
				PityProgressTextInputSystem.isActive = true;
				Main.blockInput = true;
				Main.clrInput();
			}
		}

		private void Unfocus() {
			if (focused) {
				focused = false;
				PityProgressTextInputSystem.isActive = false;
				Main.blockInput = false;
			}
		}

		public override void Update(GameTime gameTime) {
			base.Update(gameTime);

			cursorTimer++;
			cursorTimer %= 60;

			// Unfocus when clicking outside
			if (Main.mouseLeft && !ContainsPoint(Main.MouseScreen)) {
				Unfocus();
			}
		}

		protected override void DrawSelf(SpriteBatch spriteBatch) {
			CalculatedStyle dimensions = GetDimensions();

			// Draw background
			Color bgColor = new Color(26, 40, 89) * 0.9f;
			spriteBatch.Draw(TextureAssets.MagicPixel.Value, dimensions.ToRectangle(), bgColor);

			// Draw text or placeholder
			string displayText = string.IsNullOrEmpty(Text) && !focused ? "Search..." : Text;
			Color textColor = string.IsNullOrEmpty(Text) && !focused ? Color.Gray : Color.White;

			Vector2 textPos = new Vector2(dimensions.X + 8, dimensions.Y + 8);
			Utils.DrawBorderString(spriteBatch, displayText, textPos, textColor, 0.8f);

			// Draw cursor when focused
			if (focused && cursorTimer < 30) {
				float textWidth = FontAssets.MouseText.Value.MeasureString(Text).X * 0.8f;
				Utils.DrawBorderString(spriteBatch, "|", new Vector2(textPos.X + textWidth, textPos.Y), Color.White, 0.8f);
			}
		}
	}
}
