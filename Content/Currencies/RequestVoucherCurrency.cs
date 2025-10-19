using Microsoft.Xna.Framework;
using Terraria.GameContent.UI;
using Terraria.Localization;
using Terraria.ModLoader;
using SaneRNG.Content.Items;

namespace SaneRNG.Content.Currencies {
	public class RequestVoucherCurrency : CustomCurrencySingleCoin {
		public static int id;

		public RequestVoucherCurrency(int coinItemID, long currencyCap) : base(coinItemID, currencyCap) {
			CurrencyTextKey = Language.GetTextValue("Mods.SaneRNG.Currencies.RequestVoucherCurrency");
			CurrencyTextColor = Color.LightBlue;
		}
	}
}
