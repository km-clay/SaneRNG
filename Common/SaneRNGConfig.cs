using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;
using System.ComponentModel;

namespace SaneRNG.Common {
	public class SaneRNGServerConfig : ModConfig {
		public override ConfigScope Mode => ConfigScope.ServerSide;

		[DefaultValue(true)]
		public bool EnablePityDrops { get; set; }

		[DefaultValue(false)]
		public bool EnableExtractinatorPity { get; set; }

		[DefaultValue(true)]
		public bool EnableBossDropShimmer { get; set; }

		[DefaultValue(true)]
		public bool EnableAnglerShops { get; set; }

		[DefaultValue(true)]
		public bool EnableTravelingMerchantRequests { get; set; }

		[DefaultValue(true)]
		public bool EnableChestEssenceItems { get; set; }
	}

	//public class SaneRNGClientConfig : ModConfig {
		//public override ConfigScope Mode => ConfigScope.ClientSide;
	//}
}
