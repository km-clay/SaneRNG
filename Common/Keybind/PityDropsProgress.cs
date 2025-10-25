using Terraria.ModLoader;

namespace SaneRNG.Common.Keybind {
	public class PityDropsProgress : ModSystem {
		public static ModKeybind OpenProgressTracker { get; private set; }

		public override void Load() {
			OpenProgressTracker = KeybindLoader.RegisterKeybind(Mod, "Open Progress Tracker", "P");
		}

		public override void Unload() {
			OpenProgressTracker = null;
		}
	}
}
