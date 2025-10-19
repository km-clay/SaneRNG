using Terraria;
using Terraria.ModLoader;
using Terraria.GameContent.ItemDropRules;
using System.Collections.Generic;
using SaneRNG.Common.Player;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;

namespace SaneRNG.Common.Player {
	public class PityDropsPlayer : ModPlayer {
		public Dictionary<int, float> itemProgress = new();

		public override void SaveData(Terraria.ModLoader.IO.TagCompound tag) {
			if (itemProgress.Count > 0) {
				var itemIDs = new List<int>();
				var progressValues = new List<float>();

				foreach (var kvp in itemProgress) {
					itemIDs.Add(kvp.Key);
					progressValues.Add(kvp.Value);
				}

				tag["itemIDs"] = itemIDs;
				tag["progressValues"] = progressValues;
			}
		}

		public override void LoadData(Terraria.ModLoader.IO.TagCompound tag) {
			var itemIDs = tag.Get<List<int>>("itemIDs") ?? new();
			var progressValues = tag.Get<List<float>>("progressValues") ?? new();

			for (int i = 0; i < itemIDs.Count; i++) {
				int key = itemIDs[i];
				float value = progressValues[i];
				itemProgress[key] = value;
			}
		}

		public void AddProgress(int itemID, float pct) {
			if (!itemProgress.ContainsKey(itemID)) {
				itemProgress[itemID] = 0f;
			}
			itemProgress[itemID] += pct;
		}

		public void CheckAndForceDrops(List<(int itemID, float dropRate)> drops, Vector2 position, Vector2 size) {
			foreach (var (itemID, _) in drops) {
				if (itemProgress.TryGetValue(itemID, out float progress) && progress >= 100f) {
					Item.NewItem(
						new EntitySource_Loot(Player),
						(int)position.X,
						(int)position.Y,
						(int)size.X,
						(int)size.Y,
						itemID
					);

					itemProgress[itemID] -= 100f;
				}
			}
		}
	}
}

namespace SaneRNG.Common.NPCs {
	public class PityDropsGlobalNPC : GlobalNPC {
		public override void OnKill(NPC npc) {
			Terraria.Player killer = Main.player[npc.lastInteraction];
			var player = killer.GetModPlayer<PityDropsPlayer>();
			var drops = GetNPCDrops(npc.type);

			foreach (var (itemID, dropRate) in drops) {
				float percentage = dropRate * 100f;
				player.AddProgress(itemID,percentage);
			}

			player.CheckAndForceDrops(drops, npc.position, npc.Size);
		}

		private List<(int itemID, float dropRate)> GetNPCDrops(int npcType) {
			var result = new List<(int,float)>();
			var rules = Main.ItemDropsDB.GetRulesForNPCID(npcType,false);

			foreach (var rule in rules) {
				List<DropRateInfo> dropInfos = new();
				DropRateInfoChainFeed chainInfo = new DropRateInfoChainFeed(1f);
				rule.ReportDroprates(dropInfos,chainInfo);
				foreach (var info in dropInfos) {
					result.Add((info.itemId, info.dropRate));
				}
			}

			return result;
		}
	}
}
