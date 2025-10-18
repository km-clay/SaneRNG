using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SaneRNG.Content.Items {
	public class RequestVoucher : ModItem {
		public override void SetStaticDefaults() {
			Item.ResearchUnlockCount = 10;
		}

		public override void SetDefaults() {
			Item.width = 20;
			Item.height = 20;
			Item.maxStack = Item.CommonMaxStack;
			Item.value = 0;
			Item.rare = ItemRarityID.Blue;
		}

		public override void AddRecipes() {
			if (SaneRNG.DEBUG) {
				CreateRecipe(1)
					.AddIngredient(ItemID.DirtBlock, 1)
					.Register();
			}
		}
	}
}
