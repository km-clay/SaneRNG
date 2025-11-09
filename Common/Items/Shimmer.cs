using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SaneRNG.Common.Items {
	public abstract class ShimmerManager {
		public void RegisterShimmerTransformations(int[][] tables) {
			foreach (int[] table in tables) {
				RegisterShimmerTransformations(table);
			}
		}
		public void RegisterShimmerTransformations(int[] table) {
			int len = table.Length;

			for (int i = 0; i < len; i++) {
				if (i == (len - 1)) {
					ItemID.Sets.ShimmerTransformToItem[table[i]] = table[0];
				} else {
					ItemID.Sets.ShimmerTransformToItem[table[i]] = table[i + 1];
				}
			}
		}
	}

	public abstract class DirectShimmers : ShimmerManager {
		public virtual int[][] ItemTables => new int[][] {};
		public void RegisterAll() {
			RegisterShimmerTransformations(ItemTables);
		}
	}

	public class ItemShimmers : DirectShimmers {
		int[] SilverBullets = [
			ItemID.SilverBullet,
			ItemID.TungstenBullet
		];
		int[] PaladinItems = [
			ItemID.PaladinsHammer,
			ItemID.PaladinsShield,
		];
		int[] Counterweights = [
			ItemID.BlackCounterweight,
			ItemID.YellowCounterweight,
			ItemID.BlueCounterweight,
			ItemID.RedCounterweight,
			ItemID.PurpleCounterweight,
			ItemID.GreenCounterweight,
		];

		public virtual int[][] Tables => new[] {
			SilverBullets,
			PaladinItems,
			Counterweights
		};
	}

	public class AccessoryShimmers : DirectShimmers {
		int[] DivingGear = [
			ItemID.Flipper,
			ItemID.DivingHelmet
		];
		int[] JungleFlowers = [
			ItemID.NaturesGift,
			ItemID.JungleRose
		];
		int[] TigerClimbingGear = [
			ItemID.ClimbingClaws,
			ItemID.ShoeSpikes
		];
		int[] CompassDepthMeter = [
			ItemID.Compass,
			ItemID.DepthMeter,
		];
		int[] CelestialStone = [
			ItemID.SunStone,
			ItemID.MoonStone,
		];

		public virtual int[][] Tables => new[] {
			DivingGear,
			JungleFlowers,
			TigerClimbingGear,
			CompassDepthMeter,
			CelestialStone
		};
	}

	public class ShimmerRegistry : ModSystem {
		public override void PostSetupContent() {
		}

		public override void PostSetupRecipes() {
			ItemShimmers itemShimmers = new();
			AccessoryShimmers accessoryShimmers = new();

			if (ModContent.GetInstance<SaneRNGServerConfig>().EnableBossDropShimmer) {
				BossDropShimmers.RegisterBossShimmers();
			}
			itemShimmers.RegisterAll();
			accessoryShimmers.RegisterAll();
		}
	}
}
