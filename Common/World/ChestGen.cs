using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using SaneRNG.Content.Items;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;

namespace SaneRNG.Common.World {
	public static class ChestStyles {
		// Surface Items, sometimes appear in the dungeon though.
		// Wood chests in the dungeon always have a golden key.
		public static readonly (int TileType, int FrameX) Wood = (TileID.Containers, 0 * 36);

		// All Cavern/Underground items, no substantial differences for the purposes of this mod
		public static readonly (int TileType, int FrameX) Gold = (TileID.Containers, 1 * 36);
		public static readonly (int TileType, int FrameX) Granite = (TileID.Containers, 50 * 36);
		public static readonly (int TileType, int FrameX) Marble = (TileID.Containers, 51 * 36);
		public static readonly (int TileType, int FrameX) Mushroom = (TileID.Containers, 32 * 36);
		public static readonly (int TileType, int FrameX) GoldTrap = (TileID.Containers2, 4 * 36);

		// Living Wood Chests
		public static readonly (int TileType, int FrameX) LivingWood = (TileID.Containers, 12 * 36);

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
		private int[] DungeonTiles = [
			TileID.BlueDungeonBrick,
			TileID.CrackedBlueDungeonBrick,
			TileID.GreenDungeonBrick,
			TileID.CrackedGreenDungeonBrick,
			TileID.PinkDungeonBrick,
			TileID.CrackedPinkDungeonBrick
		];
		private int[] JungleTiles = [
			TileID.JungleGrass,
			TileID.JunglePlants,
			TileID.JungleVines,
			TileID.JunglePlants2,
			TileID.JungleThorns,
			TileID.Hive,
			TileID.LihzahrdBrick,
			TileID.BeeHive,
			TileID.LivingMahogany,
			TileID.LivingMahoganyLeaves,
		];

		private bool IsInBiome(int[] tileSet, int x, int y) {
			int topLeftX = x - 25;
			int topLeftY = y - 25;
			int bottomRightX = x + 25;
			int bottomRightY = y + 25;

			int biomeTiles = 0;

			for (int i = topLeftX; i < bottomRightX; i++) {
				for (int j = topLeftY; j < bottomRightY; j++) {
					if (!WorldGen.InWorld(i,j)) continue;

					Tile tile = Main.tile[i,j];

					if (tile.HasTile && tileSet.Contains(tile.TileType)) {
						biomeTiles++;
					}
				}
			}

			return biomeTiles > 140;
		}

		private int GetWoodEssence(Chest chest) {
			bool hasKey = chest.item[0].type == ItemID.GoldenKey;

			if (hasKey) {
				return ModContent.ItemType<DungeonEssence>();
			} else if (chest.y < Main.worldSurface) {
				return ModContent.ItemType<SurfaceEssence>();
			} else if (IsInBiome(JungleTiles, chest.x, chest.y)) {
				return ModContent.ItemType<JungleEssence>();
			} else {
				return ModContent.ItemType<UndergroundEssence>();
			}
		}

		public override void PostWorldGen() {
			if (ModContent.GetInstance<SaneRNGServerConfig>().EnableChestEssenceItems == false) return;
			foreach (Chest chest in Main.chest) {
				if (chest == null) {
					continue;
				}

				Tile chestTile = Main.tile[chest.x, chest.y];

				var chestType = (chestTile.TileType, chestTile.TileFrameX);

				int? essenceType = chestType switch {
					var t when t == ChestStyles.Wood => GetWoodEssence(chest),

					var t when t == ChestStyles.LivingWood => ModContent.ItemType<LivingWoodEssence>(),

					var t when t == ChestStyles.Frozen => ModContent.ItemType<IceEssence>(),

					var t when t == ChestStyles.Gold => ModContent.ItemType<UndergroundEssence>(),
					var t when t == ChestStyles.GoldTrap => ModContent.ItemType<UndergroundEssence>(),
					var t when t == ChestStyles.Granite => ModContent.ItemType<UndergroundEssence>(),
					var t when t == ChestStyles.Marble => ModContent.ItemType<UndergroundEssence>(),
					var t when t == ChestStyles.Mushroom => ModContent.ItemType<UndergroundEssence>(),

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
