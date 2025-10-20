using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using SaneRNG.Content.Items;

namespace SaneRNG.Common.World {
	public static class ChestStyles {
		// Surface Items, sometimes appear in the dungeon though.
		public static readonly (int TileType, int FrameX) Wood = (TileID.Containers, 0 * 36);

		// All Cavern/Underground items, no substantial differences for the purposes of this mod
		public static readonly (int TileType, int FrameX) Gold = (TileID.Containers, 1 * 36);
		public static readonly (int TileType, int FrameX) Granite = (TileID.Containers, 50 * 36);
		public static readonly (int TileType, int FrameX) Marble = (TileID.Containers, 51 * 36);
		public static readonly (int TileType, int FrameX) Mushroom = (TileID.Containers, 32 * 36);
		public static readonly (int TileType, int FrameX) GoldTrap = (TileID.Containers2, 4 * 36);

		// Dungeon items
		public static readonly (int TileType, int FrameX) Dungeon = (TileID.Containers, 2 * 36);

		// Shadow chests, locked variant
		public static readonly (int TileType, int FrameX) Shadow = (TileID.Containers, 4 * 36);

		// Both of these appear naturally in the jungle
		public static readonly (int TileType, int FrameX) Mahogany = (TileID.Containers, 8 * 36);
		public static readonly (int TileType, int FrameX) Ivy = (TileID.Containers, 10 * 36);

		// Snow biome
		public static readonly (int TileType, int FrameX) Frozen = (TileID.Containers, 11 * 36);

		// Floating Islands
		public static readonly (int TileType, int FrameX) Sky = (TileID.Containers, 13 * 36);

		// Underwater chests
		public static readonly (int TileType, int FrameX) Water = (TileID.Containers, 17 * 36);

		// Underground desert chests, note the TileID of Containers2 for this one.
		public static readonly (int TileType, int FrameX) Sand = (TileID.Containers2, 10 * 36);
	}

	public class ChestGen : ModSystem {
		public override void PostWorldGen() {
			foreach (Chest chest in Main.chest) {
				if (chest == null) {
					continue;
				}

				Tile chestTile = Main.tile[chest.x, chest.y];

				var chestType = (chestTile.TileType, chestTile.TileFrameX);

				int? essenceType = chestType switch {
					var t when t == ChestStyles.Wood && chest.y < Main.worldSurface => ModContent.ItemType<SurfaceEssence>(),

					var t when t == ChestStyles.Frozen => ModContent.ItemType<IceEssence>(),

					var t when t == ChestStyles.Gold => ModContent.ItemType<UndergroundEssence>(),
					var t when t == ChestStyles.GoldTrap => ModContent.ItemType<UndergroundEssence>(),
					var t when t == ChestStyles.Granite => ModContent.ItemType<UndergroundEssence>(),
					var t when t == ChestStyles.Marble => ModContent.ItemType<UndergroundEssence>(),
					var t when t == ChestStyles.Mushroom => ModContent.ItemType<UndergroundEssence>(),

					var t when t == ChestStyles.Wood => ModContent.ItemType<DungeonEssence>(),
					var t when t == ChestStyles.Dungeon => ModContent.ItemType<DungeonEssence>(),

					var t when t == ChestStyles.Shadow => ModContent.ItemType<ShadowEssence>(),

					var t when t == ChestStyles.Ivy => ModContent.ItemType<JungleEssence>(),
					var t when t == ChestStyles.Mahogany => ModContent.ItemType<JungleEssence>(),

					var t when t == ChestStyles.Sky => ModContent.ItemType<SkyEssence>(),

					var t when t == ChestStyles.Water => ModContent.ItemType<WaterEssence>(),

					var t when t == ChestStyles.Sand => ModContent.ItemType<SandEssence>(),

					_ => null
				};

				if (essenceType.HasValue) {
					for (int i = 0; i < Chest.maxItems; i++) {
						if (chest.item[i].type == ItemID.None) {
							chest.item[i].SetDefaults(essenceType.Value);
							break;
						}
					}
				}
			}
		}
	}
}
