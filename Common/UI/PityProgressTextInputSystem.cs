using Terraria;
using Terraria.GameInput;
using Terraria.ModLoader;

namespace SaneRNG.Common.UI {
	public class PityProgressTextInputSystem : ModSystem {
		public static string textInput = "";
		public static bool isActive = false;

		public override void PostUpdateInput() {
			if (isActive) {
				PlayerInput.WritingText = true;
				Main.instance.HandleIME();

				string newString = Main.GetInputText(textInput);
				if (newString != textInput) {
					textInput = newString;
				}
			}
		}
	}
}
