using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using SaneRNG.Content.Items;

namespace SaneRNG.Common.Items {
	public abstract class EssenceItem : ModItem {
		protected abstract int[] GetCraftables();
		protected virtual int RequiredEssence => 4;

		public void RegisterEssenceRecipes() {
			foreach (int itemID in GetCraftables()) {
				Recipe.Create(itemID)
					.AddIngredient(Type, RequiredEssence)
					.AddTile(TileID.DemonAltar)
					.Register();
			}

			// Debug recipe, allows for crafting each essence using 1 dirt block
			// CreateRecipe(1)
				// .AddIngredient(ItemID.DirtBlock, 1)
				// .Register();
		}

		public override void SetStaticDefaults() {
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(3,10));
			ItemID.Sets.AnimatesAsSoul[Item.type] = true;
			ItemID.Sets.ItemNoGravity[Item.type] = true;
		}

		public override void PostUpdate() {
			Lighting.AddLight(Item.Center, GetGlowColor().ToVector3() * 0.55f * Main.essScale);
		}

		protected virtual Color GetGlowColor() => Color.White;

		public override Color? GetAlpha(Color lightColor) {
			return new Color(255,255,255,50); // full-bright
		}

		public override void SetDefaults() {
			Item.width = 12;
			Item.height = 16;
			Item.maxStack = Item.CommonMaxStack;
			Item.rare = ItemRarityID.Blue;
			Item.value = 5000;
		}

		public override void AddRecipes() {
			RegisterEssenceRecipes();
		}
	}
}
