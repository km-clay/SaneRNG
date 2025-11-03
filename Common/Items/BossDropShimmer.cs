using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SaneRNG.Common.Items {
	public abstract class DropNode : ShimmerManager {
		protected virtual int[][] DropTables => [];

		public abstract void Traverse();
	}

	public abstract class DropChild : DropNode {
		public override void Traverse() {
			RegisterShimmerTransformations(DropTables);
		}
	}

	public class BossDropTree : DropNode {
		private DropNode[] children = [
			new QueenBeeDrops(),
			new DeerclopsDrops(),
			new SkeletronDrops(),
			new WallOfFleshDrops(),
			new KingSlimeDrops(),
			new QueenSlimeDrops(),
			new PlanteraDrops(),
			new GolemDrops(),
			new DukeFishronDrops(),
			new EmpressOfLightDrops(),
			new MoonLordDrops(),
			new DarkMageDrops(),
			new OgreDrops(),
			new BetsyDrops(),
			new MourningWoodDrops(),
			new PumpkingDrops(),
			new EverscreamDrops(),
			new SantankDrops(),
			new IceQueenDrops(),
			new MartianSaucerDrops(),
		];

		public override void Traverse() {
			foreach (var child in children) {
				child.Traverse();
			}
		}
	}

	public class QueenBeeDrops : DropChild {
		protected override int[][] DropTables => [
			[
				ItemID.BeeGun,
				ItemID.BeeKeeper,
				ItemID.BeesKnees,
			],
			[
				ItemID.HiveWand,
				ItemID.BeeHat,
				ItemID.BeeShirt,
				ItemID.BeePants,
			]
		];
	}

	public class DeerclopsDrops : DropChild {
		protected override int[][] DropTables => [
			[
				ItemID.PewMaticHorn,
				ItemID.WeatherPain,
				ItemID.HoundiusShootius,
				ItemID.LucyTheAxe,
			]
		];
	}

	public class SkeletronDrops : DropChild {
		protected override int[][] DropTables => [
			[
				ItemID.SkeletronMask,
				ItemID.SkeletronHand,
				ItemID.BookofSkulls,
			]
		];
	}

	public class WallOfFleshDrops : DropChild {
		protected override int[][] DropTables => [
			[
				ItemID.BreakerBlade,
				ItemID.ClockworkAssaultRifle,
				ItemID.LaserRifle,
				ItemID.FireWhip,
			],
			[
				ItemID.WarriorEmblem,
				ItemID.RangerEmblem,
				ItemID.SorcererEmblem,
				ItemID.SummonerEmblem,
			]
		];
	}

	public class KingSlimeDrops : DropChild {
		protected override int[][] DropTables => [
			[
				ItemID.NinjaHood,
				ItemID.NinjaShirt,
				ItemID.NinjaPants,
			],
			[
				ItemID.SlimeHook,
				ItemID.SlimeGun,
			]
		];
	}

	public class QueenSlimeDrops : DropChild {
		protected override int[][] DropTables => [
			[
				ItemID.CrystalNinjaHelmet,
				ItemID.CrystalNinjaChestplate,
				ItemID.CrystalNinjaLeggings,
			]
		];
	}

	public class PlanteraDrops : DropChild {
		protected override int[][] DropTables => [
			[
				ItemID.GrenadeLauncher,
				ItemID.VenusMagnum,
				ItemID.NettleBurst,
				ItemID.LeafBlower,
				ItemID.FlowerPow,
				ItemID.WaspGun,
				ItemID.Seedler,
			]
		];
	}

	public class GolemDrops : DropChild {
		protected override int[][] DropTables => [
			[
				ItemID.Stynger,
				ItemID.PossessedHatchet,
				ItemID.SunStone,
				ItemID.EyeoftheGolem,
				ItemID.HeatRay,
				ItemID.StaffofEarth,
				ItemID.GolemFist,
			]
		];
	}

	public class DukeFishronDrops : DropChild {
		protected override int[][] DropTables => [
			[
				ItemID.BubbleGun,
				ItemID.Flairon,
				ItemID.RazorbladeTyphoon,
				ItemID.TempestStaff,
				ItemID.Tsunami,
			]
		];
	}

	public class EmpressOfLightDrops : DropChild {
		protected override int[][] DropTables => [
			[
				ItemID.FairyQueenMagicItem,
				ItemID.PiercingStarlight,
				ItemID.RainbowWhip,
				ItemID.FairyQueenRangedItem,
			]
		];
	}

	public class MoonLordDrops : DropChild {
		protected override int[][] DropTables => [
			[
				ItemID.Meowmere,
				ItemID.Terrarian,
				ItemID.StarWrath,
				ItemID.SDMG,
				ItemID.Celeb2,
				ItemID.LastPrism,
				ItemID.LunarFlareBook,
				ItemID.RainbowCrystalStaff,
				ItemID.MoonlordTurretStaff,
			]
		];
	}

	public class DarkMageDrops : DropChild {
		protected override int[][] DropTables => [
			[
				ItemID.SquireShield,
				ItemID.ApprenticeScarf,
			],
			[
				ItemID.DD2PetDragon,
				ItemID.DD2PetGato,
			]
		];
	}

	public class OgreDrops : DropChild {
		protected override int[][] DropTables => [
			[
				ItemID.HuntressBuckler,
				ItemID.MonkBelt,
			],
			[
				ItemID.BookStaff,
				ItemID.DD2PhoenixBow,
				ItemID.DD2SquireDemonSword,
				ItemID.MonkStaffT1,
				ItemID.MonkStaffT2,
			]
		];
	}

	public class BetsyDrops : DropChild {
		protected override int[][] DropTables => [
			[
				ItemID.DD2BetsyBow,
				ItemID.MonkStaffT3,
				ItemID.ApprenticeStaffT3,
				ItemID.DD2SquireBetsySword,
			]
		];
	}

	public class MourningWoodDrops : DropChild {
		protected override int[][] DropTables => [
			[
				ItemID.SpookyHook,
				ItemID.SpookyTwig,
				ItemID.StakeLauncher,
				ItemID.CursedSapling,
				ItemID.NecromanticScroll,
			]
		];
	}

	public class PumpkingDrops : DropChild {
		protected override int[][] DropTables => [
			[
				ItemID.CandyCornRifle,
				ItemID.JackOLanternLauncher,
				ItemID.BlackFairyDust,
				ItemID.TheHorsemansBlade,
				ItemID.BatScepter,
				ItemID.RavenStaff,
				ItemID.ScytheWhip,
				ItemID.SpiderEgg,
			]
		];
	}

	public class EverscreamDrops : DropChild {
		protected override int[][] DropTables => [
			[
				ItemID.ChristmasTreeSword,
				ItemID.ChristmasHook,
				ItemID.Razorpine,
				ItemID.FestiveWings,
			]
		];
	}

	public class SantankDrops : DropChild {
		protected override int[][] DropTables => [
			[
				ItemID.ElfMelter,
				ItemID.ChainGun,
			]
		];
	}

	public class IceQueenDrops : DropChild {
		protected override int[][] DropTables => [
			[
				ItemID.BlizzardStaff,
				ItemID.SnowmanCannon,
				ItemID.NorthPole,
				ItemID.BabyGrinchMischiefWhistle,
			]
		];
	}

	public class MartianSaucerDrops : DropChild {
		protected override int[][] DropTables => [
			[
				ItemID.Xenopopper,
				ItemID.XenoStaff,
				ItemID.LaserMachinegun,
				ItemID.ElectrosphereLauncher,
				ItemID.InfluxWaver,
				ItemID.CosmicCarKey,
			]
		];
	}
}
