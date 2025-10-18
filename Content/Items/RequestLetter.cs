using SaneRNG.Common.Items;
using Terraria.ID;
using Terraria;
using System.Collections.Generic;

namespace SaneRNG.Content.Items {
	public class CommonRequest : RequestItem {
		protected override int VoucherCost => 2;

		protected override int[] GetSelectables() => new int[] {
			ItemID.YuumaTheBlueTiger,
			ItemID.SunshineofIsrapony,
			ItemID.DoNotEattheVileMushroom,
			ItemID.ParsecPals,
			ItemID.HoplitePizza,
			ItemID.Duality,
			ItemID.BennyWarhol,
			ItemID.KargohsSummon,
			ItemID.Stopwatch,
			ItemID.LifeformAnalyzer,
			ItemID.DPSMeter,
			ItemID.LawnFlamingo,
			ItemID.TeamBlockPink,
			ItemID.TeamBlockYellow,
			ItemID.TeamBlockGreen,
			ItemID.TeamBlockBlue,
			ItemID.TeamBlockRed,
			ItemID.TeamBlockWhite,
			ItemID.DynastyWood,
			ItemID.RedDynastyShingles,
			ItemID.BlueDynastyShingles,
			ItemID.FancyDishes,
			ItemID.SteampunkCup,
			ItemID.ZebraSkin,
			ItemID.LeopardSkin,
			ItemID.Sake,
		};
	}

	public class UncommonRequest : RequestItem {
		protected override int VoucherCost => 4;

		protected override int[] GetSelectables() {
			List<int> items = new List<int>();

			if (Main.dontStarveWorld) {
				items.AddRange([
					ItemID.PaintingWendy,
					ItemID.PaintingWillow,
					ItemID.PaintingWolfgang,
					ItemID.PaintingWilson,
				]);
			}

			if (!Main.remixWorld) {
				items.Add(ItemID.Katana);
			} else {
				items.Add(ItemID.Keybrand);
			}

			items.AddRange([
				ItemID.ActuationAccessory,
				ItemID.PortableCementMixer,
				ItemID.PaintSprayer,
				ItemID.ExtendoGrip,
				ItemID.BrickLayer,
				ItemID.PadThai,
			]);

			return items.ToArray();
		}
	}

	public class RareRequest : RequestItem {
		protected override int VoucherCost => 6;

		protected override int[] GetSelectables() {
			List<int> items = new List<int>();

			bool defeatedMechBoss = NPC.downedMechBoss1 || NPC.downedMechBoss2 || NPC.downedMechBoss3;
			bool defeatedPreHardmodeBoss = NPC.downedBoss1 || NPC.downedBoss2 || NPC.downedBoss3 || NPC.downedQueenBee;

			if (NPC.downedFrost) {
				items.AddRange([
					ItemID.PaintingTheSeason,
					ItemID.PaintingSnowfellas,
					ItemID.PaintingCursedSaint,
					ItemID.PaintingColdSnap,
					ItemID.PaintingAcorns,
				]);
			}

			if (NPC.downedMoonlord) {
				items.AddRange([
					ItemID.MoonmanandCompany,
					ItemID.MoonLordPainting,
				]);
			}

			if (NPC.downedMartians) {
				items.AddRange([
					ItemID.PaintingTheTruthIsUpThere,
					ItemID.PaintingMartiaLisa,
					ItemID.PaintingCastleMarsberg,
				]);
			}

			if (NPC.downedMechBossAny) {
				items.Add(ItemID.Code2);
			}

			if (NPC.downedBoss1) {
				items.Add(ItemID.Code1);
			}

			if (Main.hardMode) {
				items.Add(ItemID.ZapinatorGray);
			}

			if (WorldGen.shadowOrbSmashed) {
				items.Add(ItemID.Revolver);
			}

			items.AddRange([
				ItemID.UnicornHornHat,
				ItemID.HeartHairpin,
				ItemID.StarHairpin,
				ItemID.VulkelfEar,
				ItemID.PandaEars,
				ItemID.DevilHorns,
				ItemID.DemonHorns,
				ItemID.LincolnsHood,
				ItemID.LincolnsHoodie,
				ItemID.LincolnsPants,
				ItemID.StarPrincessCrown,
				ItemID.StarPrincessCrown,
				ItemID.CelestialWand,
				ItemID.GameMasterShirt,
				ItemID.GameMasterPants,
				ItemID.ChefHat,
				ItemID.ChefShirt,
				ItemID.ChefPants,
				ItemID.Gi,
				ItemID.GypsyRobe,
				ItemID.MagicHat,
				ItemID.Fez,
				ItemID.Pho,
			]);

			return items.ToArray();
		}
	}
	public class VeryRareRequest : RequestItem {
		protected override int VoucherCost => 8;

		protected override int[] GetSelectables() {
			List<int> items = new List<int>();

			if (Main.dontStarveWorld) {
				items.AddRange([
					ItemID.PaintingWendy,
					ItemID.PaintingWillow,
					ItemID.PaintingWolfgang,
					ItemID.PaintingWilson,
				]);
			}

			if (NPC.downedBoss3) {
				items.Add(ItemID.SittingDucksFishingRod);
			}

			if (NPC.downedMechBossAny) {
				items.Add(ItemID.PulseBow);
			}

			items.AddRange([
				ItemID.BambooLeaf,
				ItemID.BedazzledNectar,
				ItemID.BlueEgg,
				ItemID.ExoticEasternChewToy,
				ItemID.BirdieRattle,
				ItemID.AntiPortalBlock,
				ItemID.CompanionCube,
				ItemID.HunterCloak,
				ItemID.WinterCape,
				ItemID.RedCape,
				ItemID.MysteriousCape,
				ItemID.CrimsonCloak,
				ItemID.DiamondRing,
				ItemID.WaterGun,
				ItemID.YellowCounterweight,
			]);

			return items.ToArray();
		}
	}

	public class ExtremelyRareRequest : RequestItem {
		protected override int VoucherCost => 10;

		protected override int[] GetSelectables() {
			List<int> items = new List<int>();

			if (Main.hardMode) {
				items.AddRange([
					ItemID.BouncingShield,
					ItemID.Gatligator,
				]);
			}

			items.AddRange([
				ItemID.ArcaneRuneWall,
				ItemID.Kimono,
				ItemID.BlackCounterweight,
				ItemID.AngelHalo
			]);

			return items.ToArray();
		}
	}
}
