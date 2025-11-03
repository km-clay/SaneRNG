using System;
using Terraria;
using Terraria.ID;

namespace SaneRNG.Common.Player {
	public class WrappedInt {
		private int value;

		public WrappedInt(int value) {
			this.value = value;
		}

		public virtual int Get() {
			return value;
		}

		public void Set(int value) {
			this.value = value;
		}
	}

	public class DynInt : WrappedInt {
		private readonly Func<int> valueLambda;
		private bool hasExecuted = false;

		public DynInt(Func<int> valueLambda) : base(-1) {
			this.valueLambda = valueLambda;
		}

		public override int Get() {
			if (hasExecuted) {
				return base.Get();
			} else {
				int value = valueLambda();
				base.Set(value);
				hasExecuted = true;
				return value;
			}
		}
	}

	public class BagDropItem {
		private int ID;
		public WrappedInt qStart;
		public WrappedInt qEnd;

		public BagDropItem(int ID, WrappedInt qStart = null, WrappedInt qEnd = null) {
			if (qStart == null) {
				qStart = new WrappedInt(1);
			}
			if (qEnd == null) {
				qEnd = new WrappedInt(2);
			}
			this.ID = ID;
			this.qStart = qStart;
			this.qEnd = qEnd;
		}

		public virtual int GetItemID() {
			return ID;
		}
	};

	public class DynamicBagDropItem : BagDropItem {
		private readonly Func<int> itemIDLambda;
		public DynamicBagDropItem(Func<int> itemIDLambda, WrappedInt qStart = null, WrappedInt qEnd = null) : base(-1, qStart, qEnd) {
			this.itemIDLambda = itemIDLambda;
		}
		public override int GetItemID() => itemIDLambda();
	}

	public class BagDrop {
		public float dropChance = 100f;
		public float dropChanceHardMode = -1f;
		public bool hardModeOnly = false;
		// The number of items to roll for from this list
		// -1 means drop all of them
		public int dropCount = -1;
		// If true, this bag will attempt to re-roll duplicates
		// If it can't mathematically reach a state of no duplicates, it will just delete the dupes
		public bool forceUnique = false;

		// This is a 2D array because some bag drops will drop several items, like the halloween costume sets for instance
		public BagDropItem[][] items;
	}

	public class PityDropsBag {
		public virtual BagDrop[] BagDrops => [];

		public static PityDropsBag? GetBagById(int itemID) {
			switch (itemID) {
				case ItemID.KingSlimeBossBag: return new PityDropsKingSlimeBag();
				case ItemID.WallOfFleshBossBag: return new PityDropsWoFBag();
				default: return null;
			}
		}
	}

	public class PityDropsGoodieBag : PityDropsBag {
		public override BagDrop[] BagDrops => new BagDrop[] {
			new BagDrop() {
				dropChance = 0.67f,
				items = new BagDropItem[][] {
					new BagDropItem[] { new(ItemID.UnluckyYarn) },
				},
			},
			new BagDrop() {
				dropChance = 0.66f,
				items = new BagDropItem[][] {
					new BagDropItem[] { new(ItemID.BatHook) },
				},
			},
			new BagDrop() {
				dropChance = 24.67f,
				items = new BagDropItem[][] {
					new BagDropItem[] { new(ItemID.BatHook) },
				},
			},
			new BagDrop() {
				dropChance = 7.4f,
				items = new BagDropItem[][] {
					new BagDropItem[] { new(ItemID.JackingSkeletron) },
					new BagDropItem[] { new(ItemID.BitterHarvest) },
					new BagDropItem[] { new(ItemID.BloodMoonCountess) },
					new BagDropItem[] { new(ItemID.HallowsEve) },
					new BagDropItem[] { new(ItemID.MorbidCuriosity) },
				},
			},
			new BagDrop() {
				dropChance = 66.6f,
				items = new BagDropItem[][] {
					new BagDropItem[] {
						new(ItemID.CatMask),
						new(ItemID.CatShirt),
						new(ItemID.CatPants)
					},
					new BagDropItem[] {
						new(ItemID.CreeperMask),
						new(ItemID.CreeperShirt),
						new(ItemID.CreeperPants)
					},
					new BagDropItem[] {
						new(ItemID.GhostMask),
						new(ItemID.GhostShirt),
						new(ItemID.GhostWings)
					},
					new BagDropItem[] {
						new(ItemID.LeprechaunHat),
						new(ItemID.LeprechaunShirt),
						new(ItemID.LeprechaunPants)
					},
					new BagDropItem[] {
						new(ItemID.PixieShirt),
						new(ItemID.PixiePants)
					},
					new BagDropItem[] {
						new(ItemID.PrincessHat),
						new(ItemID.PrincessDressNew)
					},
					new BagDropItem[] {
						new(ItemID.PumpkinMask),
						new(ItemID.PumpkinShirt),
						new(ItemID.PumpkinPants)
					},
					new BagDropItem[] {
						new(ItemID.RobotMask),
						new(ItemID.RobotShirt),
						new(ItemID.RobotPants)
					},
					new BagDropItem[] {
						new(ItemID.VampireMask),
						new(ItemID.VampireShirt),
						new(ItemID.VampirePants)
					},
					new BagDropItem[] {
						new(ItemID.UnicornMask),
						new(ItemID.UnicornShirt),
						new(ItemID.UnicornPants)
					},
					new BagDropItem[] {
						new(ItemID.WitchHat),
						new(ItemID.WitchDress),
						new(ItemID.WitchBoots)
					},
					new BagDropItem[] {
						new(ItemID.BrideofFrankensteinMask),
						new(ItemID.BrideofFrankensteinDress)
					},
					new BagDropItem[] {
						new(ItemID.KarateTortoiseMask),
						new(ItemID.KarateTortoiseShirt),
						new(ItemID.KarateTortoisePants)
					},
					new BagDropItem[] {
						new(ItemID.ReaperHood),
						new(ItemID.ReaperRobe)
					},
					new BagDropItem[] {
						new(ItemID.FoxMask),
						new(ItemID.FoxShirt),
						new(ItemID.FoxPants)
					},
					new BagDropItem[] {
						new(ItemID.SpaceCreatureMask),
						new(ItemID.SpaceCreatureShirt),
						new(ItemID.SpaceCreaturePants)
					},
					new BagDropItem[] {
						new(ItemID.WolfMask),
						new(ItemID.WolfShirt),
						new(ItemID.WolfPants)
					},
					new BagDropItem[] {
						new(ItemID.TreasureHunterShirt),
						new(ItemID.TreasureHunterPants)
					},
					new BagDropItem[] {
						new(ItemID.CatEars)
					},
				},
			},
		};
	}

	public class PityDropsPresent : PityDropsBag {
		public override BagDrop[] BagDrops => new BagDrop[] {
			new BagDrop() {
				dropChance = 6.67f,
				hardModeOnly = true,
				items = new BagDropItem[][] {
					new BagDropItem[] { new(ItemID.SnowGlobe) },
				},
			},
			new BagDrop() {
				dropChance = 3.33f,
				dropChanceHardMode = 3.11f,
				items = new BagDropItem[][] {
					new BagDropItem[] { new(ItemID.Coal) },
				},
			},
			new BagDrop() {
				dropChance = 0.24f,
				dropChanceHardMode = 0.23f,
				items = new BagDropItem[][] {
					new BagDropItem[] { new(ItemID.DogWhistle) },
				},
			},
			new BagDrop() {
				dropChance = 0.64f,
				dropChanceHardMode = 0.6f,
				items = new BagDropItem[][] {
					new BagDropItem[] {
						new(ItemID.RedRyder),
						new(ItemID.MusketBall, new(30), new(61))
					},
				},
			},
			new BagDrop() {
				dropChance = 0.64f,
				dropChanceHardMode = 0.6f,
				items = new BagDropItem[][] {
					new BagDropItem[] { new(ItemID.CandyCaneSword) },
				},
			},
			new BagDrop() {
				dropChance = 0.63f,
				dropChanceHardMode = 0.59f,
				items = new BagDropItem[][] {
					new BagDropItem[] { new(ItemID.CnadyCanePickaxe /*(sic)*/) },
				},
			},
			new BagDrop() {
				dropChance = 0.63f,
				dropChanceHardMode = 0.59f,
				items = new BagDropItem[][] {
					new BagDropItem[] { new(ItemID.CandyCaneHook) },
				},
			},
			new BagDrop() {
				dropChance = 0.63f,
				dropChanceHardMode = 0.58f,
				items = new BagDropItem[][] {
					new BagDropItem[] { new(ItemID.FruitcakeChakram) },
				},
			},
			new BagDrop() {
				dropChance = 0.62f,
				dropChanceHardMode = 0.58f,
				items = new BagDropItem[][] {
					new BagDropItem[] { new(ItemID.HandWarmer) },
				},
			},
			new BagDrop() {
				dropChance = 0.31f,
				dropChanceHardMode = 0.29f,
				items = new BagDropItem[][] {
					new BagDropItem[] { new(ItemID.Toolbox) },
				},
			},
			new BagDrop() {
				dropChance = 2.31f,
				dropChanceHardMode = 2.15f,
				items = new BagDropItem[][] {
					new BagDropItem[] { new(ItemID.ReindeerAntlers) },
				},
			},
			new BagDrop() {
				dropChance = 9f,
				dropChanceHardMode = 8.4f,
				items = new BagDropItem[][] {
					new BagDropItem[] { new(ItemID.Holly) },
				},
			},
			new BagDrop() {
				dropChance = 1.08f,
				dropChanceHardMode = 1.01f,
				items = new BagDropItem[][] {
					new BagDropItem[] {
						new(ItemID.MrsClauseHat),
						new(ItemID.MrsClauseShirt),
						new(ItemID.MrsClauseHeels)
					},
				},
			},
			new BagDrop() {
				dropChance = 1.08f,
				dropChanceHardMode = 1.01f,
				items = new BagDropItem[][] {
					new BagDropItem[] {
						new(ItemID.ParkaHood),
						new(ItemID.ParkaCoat),
						new(ItemID.ParkaPants)
					},
				},
			},
			new BagDrop() {
				dropChance = 1.08f,
				dropChanceHardMode = 1.01f,
				items = new BagDropItem[][] {
					new BagDropItem[] {
						new(ItemID.TreeMask),
						new(ItemID.TreeShirt),
						new(ItemID.TreeTrunks)
					},
				},
			},
			new BagDrop() {
				dropChance = 1.08f,
				dropChanceHardMode = 1.01f,
				items = new BagDropItem[][] {
					new BagDropItem[] { new(ItemID.SnowHat) },
				},
			},
			new BagDrop() {
				dropChance = 1.08f,
				dropChanceHardMode = 1.01f,
				items = new BagDropItem[][] {
					new BagDropItem[] { new(ItemID.UglySweater) },
				},
			},
			new BagDrop() {
				dropChance = 3.6f,
				dropChanceHardMode = 3.36f,
				items = new BagDropItem[][] {
					new BagDropItem[] { new(ItemID.ChristmasPudding) },
				},
			},
			new BagDrop() {
				dropChance = 3.6f,
				dropChanceHardMode = 3.36f,
				items = new BagDropItem[][] {
					new BagDropItem[] { new(ItemID.SugarCookie) },
				},
			},
			new BagDrop() {
				dropChance = 3.6f,
				dropChanceHardMode = 3.36f,
				items = new BagDropItem[][] {
					new BagDropItem[] { new(ItemID.GingerbreadCookie) },
				},
			},
			new BagDrop() {
				dropChance = 8.1f,
				dropChanceHardMode = 7.56f,
				items = new BagDropItem[][] {
					new BagDropItem[] { new(ItemID.Eggnog) },
				},
			},
			new BagDrop() {
				dropChance = 6.3f,
				dropChanceHardMode = 5.88f,
				items = new BagDropItem[][] {
					new BagDropItem[] { new(ItemID.StarAnise) },
				},
			},
			new BagDrop() {
				dropChance = 16.8f,
				dropChanceHardMode = 15.68f,
				items = new BagDropItem[][] {
					new BagDropItem[] { new(ItemID.PineTreeBlock) },
				},
			},
			new BagDrop() {
				dropChance = 16.8f,
				dropChanceHardMode = 15.68f,
				items = new BagDropItem[][] {
					new BagDropItem[] { new(ItemID.CandyCaneBlock) },
				},
			},
			new BagDrop() {
				dropChance = 16.8f,
				dropChanceHardMode = 15.68f,
				items = new BagDropItem[][] {
					new BagDropItem[] { new(ItemID.GreenCandyCaneBlock) },
				},
			},
		};
	}

	public class PityDropsOyster : PityDropsBag {
		public override BagDrop[] BagDrops => new BagDrop[] {
			new BagDrop() {
				items = new BagDropItem[][] {
					new BagDropItem[] { new(ItemID.ShuckedOyster) },
				},
			},
			new BagDrop() {
				dropChance = 12.44f,
				items = new BagDropItem[][] {
					new BagDropItem[] { new(ItemID.WhitePearl) },
				},
			},
			new BagDrop() {
				dropChance = 6.22f,
				items = new BagDropItem[][] {
					new BagDropItem[] { new(ItemID.BlackPearl) },
				},
			},
			new BagDrop() {
				dropChance = 6.22f,
				items = new BagDropItem[][] {
					new BagDropItem[] { new(ItemID.PinkPearl) },
				},
			},
		};
	}

	public class PityDropsCanOfWorms : PityDropsBag {
		public override BagDrop[] BagDrops => new BagDrop[] {
			new BagDrop() {
				items = new BagDropItem[][] {
					new BagDropItem[] { new(ItemID.Worm) },
				},
			},
			new BagDrop() {
				dropChance = 30f,

				items = new BagDropItem[][] {
					new BagDropItem[] { new(ItemID.EnchantedNightcrawler) },
				},
			},
			new BagDrop() {
				dropChance = 5f,
				items = new BagDropItem[][] {
					new BagDropItem[] { new(ItemID.GoldWorm) },
				},
			},
		};
	}

	public class PityDropsGoldenLockBox : PityDropsBag {
		public override BagDrop[] BagDrops => new BagDrop[] {
			new BagDrop() {
				dropCount = 1,
				items = new BagDropItem[][] {
					new BagDropItem[] {
						new(ItemID.Muramasa),
						new(ItemID.CobaltShield),
						new(ItemID.Valor),
						new DynamicBagDropItem(() => (Main.zenithWorld || Main.remixWorld) ? ItemID.BubbleGun : ItemID.AquaScepter),
						new(ItemID.BlueMoon),
						new(ItemID.MagicMissile),
						new(ItemID.Handgun)
					},
				},
			},
			new BagDrop() {
				dropChance = 33.33f,
				items = new BagDropItem[][] {
					new BagDropItem[] { new(ItemID.ShadowKey) },
				},
			},
		};
	}

	public class PityDropsObsidianLockBox : PityDropsBag {
		public override BagDrop[] BagDrops => new BagDrop[] {
			new BagDrop() {
				dropCount = 1,
				items = new BagDropItem[][] {
					new BagDropItem[] {
						new(ItemID.DarkLance),
						new(ItemID.Sunfury),
						new DynamicBagDropItem(() => (Main.zenithWorld || Main.remixWorld) ? ItemID.UnholyTrident : ItemID.FlowerofFire),
						new(ItemID.Flamelash),
						new(ItemID.HellwingBow),
						new(ItemID.TreasureMagnet)
					}
				}
			}
		};
	}

	public class PityDropsKingSlimeBag : PityDropsBag {
		public override BagDrop[] BagDrops => new BagDrop[] {
			new BagDrop() {
				forceUnique = true,
				dropCount = 2,
				items = new BagDropItem[][] {
					new BagDropItem[] {
						new(ItemID.NinjaHood)
					},
					new BagDropItem[] {
						new(ItemID.NinjaShirt)
					},
					new BagDropItem[] {
						new(ItemID.NinjaPants)
					},
				}
			},
			new BagDrop() {
				items = new BagDropItem[][] {
					new BagDropItem[] { new(ItemID.RoyalGel) },
					new BagDropItem[] { new(ItemID.Solidifier) },
					new BagDropItem[] { new(ItemID.GoldCoin) }
				}
			},
			new BagDrop() {
				dropChance = 50f,
				items = new BagDropItem[][] {
					new BagDropItem[] { new(ItemID.SlimeGun) },
				}
			},
			new BagDrop() {
				dropChance = 50f,
				items = new BagDropItem[][] {
					new BagDropItem[] { new(ItemID.SlimeHook) },
				}
			},
			new BagDrop() {
				dropChance = 14.29f,
				items = new BagDropItem[][] {
					new BagDropItem[] { new(ItemID.KingSlimeMask) },
				}
			},
			new BagDrop() {
				dropChance = 14.29f,
				items = new BagDropItem[][] {
					new BagDropItem[] { new(ItemID.SlimySaddle) },
				}
			},
		};
	}

	public class PityDropsEyeOfCthulhuBag : PityDropsBag {
		public override BagDrop[] BagDrops => new BagDrop[] {
			new BagDrop() {
				items = new BagDropItem[][] {
					new BagDropItem[] {
						new(ItemID.EoCShield),
						new(ItemID.GoldCoin, new(3), new(4)),
						new DynamicBagDropItem(
							() => (WorldGen.crimson ? ItemID.CrimtaneOre : ItemID.DemoniteOre),
							new(30),
							new(91)
						),
						new DynamicBagDropItem(
							() => (WorldGen.crimson ? ItemID.CrimsonSeeds : ItemID.CorruptSeeds),
							new(1),
							new(4)
						),
						new DynamicBagDropItem(
							() => (WorldGen.crimson ? ItemID.UnholyArrow : ItemID.None),
							new(1),
							new(4)
						)
					}
				}
			},
			new BagDrop() {
				dropChance = 3.33f,
				items = new BagDropItem[][] {
					new BagDropItem[] { new(ItemID.Binoculars) },
				}
			},
			new BagDrop() {
				dropChance = 14.29f,
				items = new BagDropItem[][] {
					new BagDropItem[] { new(ItemID.EyeMask) },
				}
			}
		};
	}

	public class PityDropsEoWBag : PityDropsBag {
		public override BagDrop[] BagDrops => new BagDrop[] {
			new BagDrop() {
				items = new BagDropItem[][] {
					new BagDropItem[] { new(ItemID.WormScarf) },
					new BagDropItem[] {
						new(
							ItemID.DemoniteOre,
							new DynInt(() => Main.masterMode ? 110 : 80),
							new DynInt(() => Main.masterMode ? 136 : 111)
						),
					},
					new BagDropItem[] {
						new(
							ItemID.ShadowScale,
							new DynInt(() => Main.masterMode ? 30 : 20),
							new DynInt(() => Main.masterMode ? 135 : 110)
						),
					},
					new BagDropItem[] { new(ItemID.GoldCoin, new(3), new(4)), }
				}
			},
			new BagDrop() {
				dropChance = 14.29f,
				items = new BagDropItem[][] {
					new BagDropItem[] { new(ItemID.EaterMask) },
				}
			},
			new BagDrop() {
				dropChance = 5f,
				items = new BagDropItem[][] {
					new BagDropItem[] { new(ItemID.EatersBone) },
				}
			},
		};
	}

	public class PityDropsBoCBag : PityDropsBag {
		public override BagDrop[] BagDrops => new BagDrop[] {
			new BagDrop() {
				items = new BagDropItem[][] {
					new BagDropItem[] { new(ItemID.BrainOfConfusion) },
					new BagDropItem[] {
						new(
							ItemID.CrimtaneOre,
							new DynInt(() => Main.masterMode ? 110 : 80),
							new DynInt(() => Main.masterMode ? 136 : 111)
						),
					},
					new BagDropItem[] {
						new(
							ItemID.TissueSample,
							new DynInt(() => Main.masterMode ? 30 : 20),
							new DynInt(() => Main.masterMode ? 50 : 40)
						),
					},
					new BagDropItem[] { new(ItemID.GoldCoin), },
				}
			},
			new BagDrop() {
				dropChance = 14.29f,
				items = new BagDropItem[][] {
					new BagDropItem[] { new(ItemID.BrainMask) },
				}
			},
			new BagDrop() {
				dropChance = 5f,
				items = new BagDropItem[][] {
					new BagDropItem[] { new(ItemID.BoneRattle) },
				}
			}
		};
	}

	public class PityDropsQueenBeeBag : PityDropsBag {
		public override BagDrop[] BagDrops => new BagDrop[] {
			new BagDrop() {
				items = [
					[new(ItemID.HiveBackpack)],
					[new(ItemID.HiveWand)],
					[new(ItemID.Beenade, new(10), new(30))],
					[new(ItemID.BeeWax, new(17), new(30))],
					[new(ItemID.GoldCoin, new(10), new(11))],
				]
			},
			new BagDrop() {
				dropChance = 14.29f,
				items = [[new(ItemID.BeeMask)]]
			},
			new BagDrop() {
				dropCount = 1,
				items = [
					[new(ItemID.BeeGun)],
					[new(ItemID.BeeKeeper)],
					[new(ItemID.BeesKnees)]
				]
			},
			new BagDrop() {
				dropCount = 1,
				items = [
					[new(ItemID.BeeHat)],
					[new(ItemID.BeeShirt)],
					[new(ItemID.BeePants)]
				]
			},
			new BagDrop() {
				dropChance = 33f,
				items = [[new(ItemID.HoneyComb)]]
			},
			new BagDrop() {
				dropChance = 11f,
				items = [[new(ItemID.Nectar)]]
			},
		};
	}

	public class PityDropsSkeletronBag : PityDropsBag {
		public override BagDrop[] BagDrops => new BagDrop[] {
			new BagDrop() {
				items = [
					[new(ItemID.BoneGlove)],
					[new(ItemID.GoldCoin, new(5), new(6))],
				]
			},
			new BagDrop() {
				dropCount = 1,
				items = [
					[new(ItemID.SkeletronMask)],
					[new(ItemID.SkeletronHand)],
					[new(ItemID.BookofSkulls)],
				]
			},
		};
	}

	public class PityDropsDeerclopsBag : PityDropsBag {
		public override BagDrop[] BagDrops => new BagDrop[] {
			new BagDrop() {
				items = [
					[new(ItemID.BoneHelm)],
					[new(ItemID.GoldCoin,new(10),new(11))]
				]
			},
			new BagDrop() {
				dropCount = 1,
				items = [
					[new(ItemID.PewMaticHorn)],
					[new(ItemID.WeatherPain)],
					[new(ItemID.HoundiusShootius)],
					[new(ItemID.LucyTheAxe)],
				]
			},
			new BagDrop() {
				dropChance = 33f,
				items = [
					[new(ItemID.ChesterPetItem)],
				]
			},
			new BagDrop() {
				dropChance = 33f,
				items = [
					[new(ItemID.Eyebrella)],
				]
			},
			new BagDrop() {
				dropChance = 33f,
				items = [
					[new(ItemID.DontStarveShaderItem)],
				]
			},
		};
	}

	public class PityDropsWoFBag : PityDropsBag {
		public override BagDrop[] BagDrops => new BagDrop[] {
			new BagDrop() {
				items = [
					[new DynamicBagDropItem(() => Main.LocalPlayer.extraAccessory ? ItemID.None : ItemID.DemonHeart)],
					[new(ItemID.Pwnhammer)],
					[new(ItemID.GoldCoin, new(8), new(9))],
				]
			},
			new BagDrop() {
				dropChance = 14.29f,
				items = [[new(ItemID.FleshMask)]]
			},
			new BagDrop() {
				dropCount = 1,
				items = [
					[new(ItemID.BreakerBlade)],
					[new(ItemID.ClockworkAssaultRifle)],
					[new(ItemID.LaserRifle)],
					[new(ItemID.FireWhip)],
				]
			}
		};
	}

	public class PityDropsQueenSlimeBag : PityDropsBag {
		public override BagDrop[] BagDrops => new BagDrop[] {
			new BagDrop() {
				items = [
					[new(ItemID.VolatileGelatin)],
					[new(ItemID.GelBalloon)],
					[new(ItemID.GoldCoin, new(5), new(10))],
				]
			},
			new BagDrop() {
				dropChance = 14.29f,
				items = [
					[new(ItemID.QueenSlimeMask)],
				]
			},
			new BagDrop() {
				dropCount = 2,
				items = [
					[new(ItemID.CrystalNinjaHelmet)],
					[new(ItemID.CrystalNinjaChestplate)],
					[new(ItemID.CrystalNinjaLeggings)],
				]
			},
			new BagDrop() {
				dropChance = 50f,
				items = [[new(ItemID.QueenSlimeMountSaddle)]]
			},
			new BagDrop() {
				dropChance = 50f,
				items = [[new(ItemID.QueenSlimeHook)]]
			},
			new BagDrop() {
				dropChance = 33.33f,
				items = [[new(ItemID.Smolstar)]]
			}
		};
	}
}
