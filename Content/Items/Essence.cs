using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using SaneRNG.Common.Items;

namespace SaneRNG.Content.Items {
	public class SurfaceEssence : EssenceItem {
		protected override Color GetGlowColor() {
			return Color.ForestGreen;
		}
		protected override int[] GetCraftables() {
			return new int[] {
				ItemID.Spear,
				ItemID.Blowpipe,
				ItemID.WoodenBoomerang,
				ItemID.Aglet,
				ItemID.ClimbingClaws,
				ItemID.Umbrella,
				ItemID.CordageGuide,
				ItemID.WandofSparking,
				ItemID.Radar,
				ItemID.PortableStool
			};
		}
	}

	public class LivingWoodEssence : EssenceItem {
		protected override Color GetGlowColor() {
			return Color.ForestGreen;
		}
		protected override int[] GetCraftables() {
			return new int[] {
				ItemID.LivingWoodWand,
				ItemID.LeafWand,
				ItemID.BabyBirdStaff,
				ItemID.SunflowerMinecart,
				ItemID.LadybugMinecart,
			};
		}
	}

	public class UndergroundEssence : EssenceItem {
		protected override Color GetGlowColor() {
			return Color.Brown;
		}
		protected override int[] GetCraftables() {
			return new int[] {
				ItemID.Extractinator,
				ItemID.BandofRegeneration,
				ItemID.MagicMirror,
				ItemID.CloudinaBottle,
				ItemID.HermesBoots,
				ItemID.Mace,
				ItemID.ShoeSpikes,
				ItemID.FlareGun,
				ItemID.AngelStatue,
				ItemID.LavaCharm
			};
		}
	}

	public class DungeonEssence : EssenceItem {
		protected override Color GetGlowColor() {
			return Color.Purple;
		}
		protected override int[] GetCraftables() {
			return new int[] {
				ItemID.Muramasa,
				ItemID.CobaltShield,
				ItemID.AquaScepter,
				ItemID.BlueMoon,
				ItemID.MagicMissile,
				ItemID.Valor,
				ItemID.Handgun,
				ItemID.ShadowKey
			};
		}
	}

	public class IceEssence : EssenceItem {
		protected override Color GetGlowColor() {
			return Color.Cyan;
		}
		protected override int[] GetCraftables() {
			return new int[] {
				ItemID.IceBoomerang,
				ItemID.IceBlade,
				ItemID.IceSkates,
				ItemID.SnowballCannon,
				ItemID.BlizzardinaBottle,
				ItemID.FlurryBoots,
				ItemID.Extractinator,
				ItemID.IceMirror,
				ItemID.Fish,
			};
		}
	}

	public class ShadowEssence : EssenceItem {
		protected override Color GetGlowColor() {
			return Color.BlueViolet;
		}
		protected override int[] GetCraftables() {
			return new int[] {
				ItemID.Sunfury,
				ItemID.FlowerofFire,
				ItemID.UnholyTrident,
				ItemID.Flamelash,
				ItemID.DarkLance,
				ItemID.HellwingBow,
				ItemID.TreasureMagnet,
				ItemID.HellMinecart,
			};
		}
	}

	public class SandEssence : EssenceItem {
		protected override Color GetGlowColor() {
			return Color.SandyBrown;
		}
		protected override int[] GetCraftables() {
			return new int[] {
				ItemID.MagicConch,
				ItemID.MysticCoilSnake,
				ItemID.AncientChisel,
				ItemID.SandBoots,
				ItemID.ThunderSpear,
				ItemID.ThunderStaff,
				ItemID.CatBast,
				ItemID.DesertMinecart,
				ItemID.EncumberingStone,
			};
		} }

	public class JungleEssence : EssenceItem {
		protected override Color GetGlowColor() {
			return Color.DarkGreen;
		}
		protected override int[] GetCraftables() {
			return new int[] {
				ItemID.FeralClaws,
				ItemID.AnkletoftheWind,
				ItemID.StaffofRegrowth,
				ItemID.Boomstick,
				ItemID.FlowerBoots,
				ItemID.FiberglassFishingPole,
				ItemID.BeeMinecart,
				ItemID.Seaweed,
			};
		}
	}

	public class WaterEssence : EssenceItem {
		protected override Color GetGlowColor() {
			return Color.Blue;
		}
		protected override int[] GetCraftables() {
			return new int[] {
				ItemID.BreathingReed,
				ItemID.Flipper,
				ItemID.Trident,
				ItemID.FloatingTube,
				ItemID.WaterWalkingBoots,
			};
		}
	}

	public class SkyEssence : EssenceItem {
		protected override int RequiredEssence => 3;
		protected override Color GetGlowColor() {
			return Color.LightCyan;
		}

		protected override int[] GetCraftables() {
			return new int[] {
				ItemID.ShinyRedBalloon,
				ItemID.Starfury,
				ItemID.LuckyHorseshoe,
				ItemID.CelestialMagnet,
			};
		}
	}
}
