using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace SaneRNG
{
	// Please read https://github.com/tModLoader/tModLoader/wiki/Basic-tModLoader-Modding-Guide#mod-skeleton-contents for more information about the various files in a mod.
	public class SaneRNG : Mod
	{
		// Enables debug behaviors
		// Cheap crafting recipes for essences, constant traveling merchant spawns, etc.
		// Disable this for release builds
		public const bool DEBUG = false;
	}
}
