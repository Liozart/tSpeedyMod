using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace GiuxItems.Tiles
{
    class GiuxOre : ModTile
    {
		public override void SetDefaults()
		{
			TileID.Sets.Ore[Type] = true;
			Main.tileSpelunker[Type] = true; // The tile will be affected by spelunker highlighting
			Main.tileValue[Type] = 410; // Metal Detector value, see https://terraria.gamepedia.com/Metal_Detector
			Main.tileShine2[Type] = true; // Modifies the draw color slightly.
			Main.tileShine[Type] = 975; // How often tiny dust appear off this tile. Larger is less frequently
			Main.tileMergeDirt[Type] = true;
			Main.tileSolid[Type] = true;
			Main.tileBlockLight[Type] = true;

			ModTranslation name = CreateMapEntryName();
			name.SetDefault("GiuxOre");
			name.AddTranslation(GameCulture.French, "Minerai de Giux");
			AddMapEntry(new Color(152, 171, 198), name);

			dustType = 84;
			drop = ModContent.ItemType<Items.Placeables.GiuxOre>();
			soundType = SoundID.Tink;
			soundStyle = 1;
			mineResist = 2f;
			minPick = 60;
		}
	}
}
