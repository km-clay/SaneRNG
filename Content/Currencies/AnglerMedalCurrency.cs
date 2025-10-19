using Microsoft.Xna.Framework;
using Terraria.GameContent.UI;
using Terraria.Localization;

namespace SaneRNG.Content.Currencies {
	public class AnglerMedalCurrency : CustomCurrencySingleCoin {
		public static int id;
		public AnglerMedalCurrency(int coinItemID, long currencyCap) : base(coinItemID,currencyCap) {
			CurrencyTextKey = Language.GetTextValue("Mods.SaneRNG.Currencies.AnglerMedalCurrency");
			CurrencyTextColor = Color.SkyBlue;
		}
	}
}
