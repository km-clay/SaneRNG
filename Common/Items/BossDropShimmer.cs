using System;
using System.Collections.Generic;
using System.Reflection;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ModLoader;

namespace SaneRNG.Common.Items {
	public class BossDropShimmers : ShimmerManager {
		public static void RegisterBossShimmers() {
			BossDropShimmers manager = new();

			for (int npcID = -65; npcID < NPCLoader.NPCCount; npcID++) {
				NPC npc = new NPC();
				npc.SetDefaults(npcID);

				if (!npc.boss) continue;


				var rules = Main.ItemDropsDB.GetRulesForNPCID(npcID,false);

				List<List<int>> dropGroups = ExtractOneFromOptionsGroups(rules);

				foreach (var group in dropGroups) {
					if (group.Count > 1) {
						manager.RegisterShimmerTransformations(group.ToArray());
					}
				}
			}
		}

		private static int GetItemIdFromRule(IItemDropRule rule) {
			var itemIdField = rule.GetType().GetField("itemId");
			if (itemIdField != null && itemIdField.FieldType == typeof(int)) {
				return (int)itemIdField.GetValue(rule);
			}
			return -1;
		}

		private static List<List<int>> ExtractOneFromOptionsGroups(List<IItemDropRule> rules) {
			List<List<int>> result = new();

			foreach (var rule in rules) {
				if (rule is OneFromOptionsDropRule oneFromOptions) {
					List<int> group = new();
					foreach(int itemID in oneFromOptions.dropIds) {
						group.Add(itemID);
					}
					if (group.Count > 1) {
						result.Add(group);
					}
				}

				if (rule is OneFromOptionsNotScaledWithLuckDropRule oneFromOptionsNoLuck) {
					List<int> group = new();
					foreach(int itemID in oneFromOptionsNoLuck.dropIds) {
						group.Add(itemID);
					}
					if (group.Count > 1) {
						result.Add(group);
					}
				}

				if (rule is FromOptionsWithoutRepeatsDropRule fromOptionsNoRepeats) {
					List<int> group = new();
					foreach(int itemID in fromOptionsNoRepeats.dropIds) {
						group.Add(itemID);
					}
					if (group.Count > 1) {
						result.Add(group);
					}
				}

				if (rule is OneFromRulesRule oneFromRules) {
					List<int> group = new();
					foreach (var optionRule in oneFromRules.options) {
						int itemId = GetItemIdFromRule(optionRule);
						if (itemId != -1) {
							group.Add(itemId);
						}
					}

					if (group.Count > 1) {
						result.Add(group);
					}
				}

				// Handle Calamity's special snowflake drop rule
				if (rule.GetType().Name == "AllOptionsAtOnceWithPityDropRule") {
					var stacksField = rule.GetType().GetField("stacks");
					if (stacksField != null) {
						var stacks = stacksField.GetValue(rule) as Array;
						if (stacks != null && stacks.Length > 1) {
							List<int> group = new();
							foreach (var stack in stacks) {
								var itemIDField = stack.GetType().GetField("itemID",
									BindingFlags.Instance |
									BindingFlags.NonPublic);
								if (itemIDField != null) {
									int itemID = (int)itemIDField.GetValue(stack);
									group.Add(itemID);
								} else {
								}
							}

							if (group.Count > 1) {
								result.Add(group);
							}
						} else {
						}
					} else {
					}
				}

				if (rule.ChainedRules != null) {
					foreach (var chainRule in rule.ChainedRules) {
						var subRules = new List<IItemDropRule> { chainRule.RuleToChain };
						var subGroups = ExtractOneFromOptionsGroups(subRules);
						result.AddRange(subGroups);
					}
				}
			}

			return result;
		}
	}
}
