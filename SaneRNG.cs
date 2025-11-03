using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using SaneRNG.Common.Player;
using SaneRNG.Common.NPCs;

namespace SaneRNG
{
	// Please read https://github.com/tModLoader/tModLoader/wiki/Basic-tModLoader-Modding-Guide#mod-skeleton-contents for more information about the various files in a mod.
	public class SaneRNG : Mod
	{
		// Enables debug behaviors
		// Cheap crafting recipes for essences, constant traveling merchant spawns, etc.
		// Disable this for release builds
		public const bool DEBUG = false;

		public override void HandlePacket(System.IO.BinaryReader reader, int whoAmI) {
			byte packetType = reader.ReadByte();

			switch (packetType) {
				case PityDropsPacketType.FullSync:
					HandleFullSync(reader);
					break;
				case PityDropsPacketType.ProgressUpdate:
					HandleProgressUpdate(reader);
					break;
				case PityDropsPacketType.PinUpdate:
					HandlePinUpdate(reader);
					break;
				case PityDropsPacketType.VoucherUpdate:
					HandleVoucherUpdate(reader);
					break;
			}
		}

		private void HandleVoucherUpdate(System.IO.BinaryReader reader) {
			bool isReset = reader.ReadBoolean();

			if (isReset) {
				SaneRNGTravelingMerchant.hasTakenVoucherThisVisit.Clear();
			} else {
				int whoIsIt = reader.ReadInt32();
				SaneRNGTravelingMerchant.hasTakenVoucherThisVisit.Add(whoIsIt);
			}

		}

		private void HandleFullSync(System.IO.BinaryReader reader) {
			byte whoIsIt = reader.ReadByte();
			Terraria.Player player = Terraria.Main.player[whoIsIt];
			var modPlayer = player.GetModPlayer<PityDropsPlayer>();

			Dictionary<int, double> itemProgress = new();
			int itemProgressCount = reader.ReadInt32();
			for (int i = 0; i < itemProgressCount; i++) {
				int itemID = reader.ReadInt32();
				double progressPct = reader.ReadDouble();
				itemProgress[itemID] = progressPct;
			}
			modPlayer.itemProgress = itemProgress;

			HashSet<int> pinnedItems = new();
			int pinnedItemsCount = reader.ReadInt32();
			for (int i = 0; i < pinnedItemsCount; i++) {
				int itemID = reader.ReadInt32();
				pinnedItems.Add(itemID);
			}
			modPlayer.pinnedItems = pinnedItems;
		}
		private void HandleProgressUpdate(System.IO.BinaryReader reader) {
			byte whoIsIt = reader.ReadByte();
			Terraria.Player player = Terraria.Main.player[whoIsIt];
			var modPlayer = player.GetModPlayer<PityDropsPlayer>();

			Dictionary<int, double> itemProgress = new();
			int itemProgressCount = reader.ReadInt32();
			for (int i = 0; i < itemProgressCount; i++) {
				int itemID = reader.ReadInt32();
				double progressPct = reader.ReadDouble();
				itemProgress[itemID] = progressPct;
			}
			modPlayer.itemProgress = itemProgress;
		}
		private void HandlePinUpdate(System.IO.BinaryReader reader) {
			byte whoIsIt = reader.ReadByte();
			Terraria.Player player = Terraria.Main.player[whoIsIt];
			var modPlayer = player.GetModPlayer<PityDropsPlayer>();

			HashSet<int> pinnedItems = new();
			int pinnedItemsCount = reader.ReadInt32();
			for (int i = 0; i < pinnedItemsCount; i++) {
				int itemID = reader.ReadInt32();
				pinnedItems.Add(itemID);
			}
			modPlayer.pinnedItems = pinnedItems;
		}
	}
}
