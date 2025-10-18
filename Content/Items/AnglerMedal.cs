using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SaneRNG.Content.Items
{
	public class AnglerMedal : ModItem
	{
		public override void SetStaticDefaults() {
			Item.ResearchUnlockCount = 25; // Journey mode research amount
		}

		public override void SetDefaults() {
			Item.width = 20;
			Item.height = 20;
			Item.maxStack = Item.CommonMaxStack;
			Item.value = 0; // Currency items typically have no sell value
			Item.rare = ItemRarityID.Blue; // Blue rarity like other quest currencies
		}

		public override void AddRecipes() {
			CreateRecipe(1)
				.AddIngredient(ItemID.DirtBlock, 1)
				.AddTile(TileID.WorkBenches)
				.Register();
		}
	}
}
