using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using SaneRNG.Common.NPCs;
using SaneRNG.Content.Items;
using Terraria.UI;
using Terraria.GameContent.UI.Elements;
using System;
using System.Collections.Generic;
using Terraria.GameContent;
using ReLogic.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SaneRNG.Common.NPCs {
	public class CarrierBird : ModNPC {
		public override string Texture => $"Terraria/Images/NPC_{NPCID.Bird}";
		public override void SetDefaults() {
			NPC.CloneDefaults(NPCID.Bird);
			Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.Bird];
			AnimationType = NPCID.Bird;

			NPC.dontTakeDamage = true;
			NPC.noTileCollide = true;
			NPC.catchItem = -1;
		}
	}
}

namespace SaneRNG.Common.Items {

	public class RequestUISystem : ModSystem {
		internal UserInterface requestUI;
		internal RequestSelectionUIState requestState;

		public void ShowUI(int[] selectables, Action<int> onItemSelected) {
			requestState.SetItems(selectables, onItemSelected);
			requestUI?.SetState(requestState);
		}

		public void HideUI() => requestUI?.SetState(null);

		public override void PostSetupContent() {
			requestUI = new UserInterface();
			requestState = new RequestSelectionUIState();
			requestState.Activate();
		}

		public override void UpdateUI(GameTime gameTime) {
			if (requestUI?.CurrentState != null) {
				requestUI?.Update(gameTime);
			}
		}

		public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers) {
			int mouseTextIdx = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
			if (mouseTextIdx != -1) {
				layers.Insert(mouseTextIdx, new
					LegacyGameInterfaceLayer("SaneRNG: Request Selection",
					delegate {
						if (requestUI?.CurrentState != null) {
							requestUI.Draw(Main.spriteBatch, new GameTime());
						}
						return true;
					},
					InterfaceScaleType.UI)
				);
			}
		}
	}

	public class UISlot : UIElement {
		private int itemID;

		public UISlot(int itemID) {
			this.itemID = itemID;
		}

		protected override void DrawSelf(SpriteBatch spriteBatch) {
			Item item = new Item();
			item.SetDefaults(itemID);
			CalculatedStyle dimensions = GetDimensions();
			ItemSlot.Draw(spriteBatch, ref item, 0, dimensions.Position());
		}
	}

	public class RequestSelectionUIState : UIState {
		private Action<int> onItemSelectedCallback;
		private int[] selectables;
		private UIPanel panel;

		public override void OnInitialize() {
			panel = new UIPanel();
			panel.Left.Set(400f, 0f);
			panel.Top.Set(200f, 0f);
			panel.Width.Set(400f, 0f);
			panel.Height.Set(300f, 0f);
			Append(panel);
		}
		public override void Update(GameTime gameTime) {
			base.Update(gameTime);

			if (Main.keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Escape)
				&& !Main.oldKeyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Escape))
			{
				ModContent.GetInstance<RequestUISystem>().HideUI();
			};
		}
		public void SetItems(int[] selectables, Action<int> onItemSelected) {
			this.onItemSelectedCallback = onItemSelected;
			this.selectables = selectables;

			panel.RemoveAllChildren();

			int columns = 8;
			int slotSize = 40;
			int padding = 5;

			for (int i = 0; i < selectables.Length; i++) {
				int itemID = selectables[i];
				int row = i / columns;
				int col = i % columns;

				UISlot itemSlot = new UISlot (itemID);

				itemSlot.Left.Set(col * (slotSize + padding) + 10, 0f);
				itemSlot.Top.Set(row * (slotSize + padding) + 10, 0f);
				itemSlot.Width.Set(slotSize, 0f);
				itemSlot.Height.Set(slotSize, 0f);

				itemSlot.OnLeftClick += (evt, element) => ItemClicked(itemID);
				panel.Append(itemSlot);
			}

		}
		private void ItemClicked(int itemID) {
			onItemSelectedCallback(itemID);
			ModContent.GetInstance<RequestUISystem>().HideUI();
		}
	}

	public abstract class RequestItem : ModItem {
		protected virtual int VoucherCost => 2;
		protected virtual int[] GetSelectables() => new int[0];
		protected int RequestItemID { get; set; } = 0;

		public override bool AltFunctionUse(Player player) => true;

		public override void SetDefaults() {
			Item.width = 20;
			Item.height = 20;
			Item.maxStack = 10;
			Item.consumable = true;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 30;
			Item.useAnimation = 30;
		}

		private void OpenSelectionUI(Player player) {
			ModContent.GetInstance<RequestUISystem>().ShowUI(
				GetSelectables(),
				(itemID) => {
					RequestItemID = itemID;
					Main.NewText($"Selected {Lang.GetItemNameValue(itemID)}!");
				}
			);
		}

		public override bool? UseItem(Player player) {
			if (player.altFunctionUse == 2) {
				OpenSelectionUI(player);
				return false;
			} else {
				if (RequestItemID == 0) {

					return false;
				}
				SaneRNGTravelingMerchant.PushRequest(RequestItemID);

				int birdIndex = NPC.NewNPC(
					player.GetSource_ItemUse(Item),
					(int)player.Center.X,
					(int)player.Center.Y,
					ModContent.NPCType<CarrierBird>()
				);

				if (birdIndex != Main.maxNPCs) {
					Main.npc[birdIndex].velocity = new Vector2(0, -3f);
				}

				Main.NewText("Your request has been sent!");
				return true;
			}
		}

		public override void AddRecipes() {
			if (SaneRNG.DEBUG) {
				CreateRecipe(1)
					.AddIngredient(ItemID.DirtBlock, 1)
					.Register();
			}

			CreateRecipe(1)
				.AddIngredient<RequestVoucher>(VoucherCost)
				.AddRecipeGroup(RecipeGroupID.Birds)
				.Register();
		}
	}
}
