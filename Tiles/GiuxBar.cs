using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace GiuxItems.Tiles
{
    class GiuxBar : ModTile
    {
		public override void SetDefaults()
		{
            Main.tileShine[Type] = 1100;
            Main.tileSolid[Type] = true;
            Main.tileSolidTop[Type] = true;
            Main.tileFrameImportant[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.LavaDeath = false;
            TileObjectData.addTile(Type);

            ModTranslation name = CreateMapEntryName();
			name.SetDefault("GiuxBar");
			name.AddTranslation(GameCulture.French, "Barre de Giux");
			AddMapEntry(new Color(152, 171, 198), name);

            dustType = 87;
			drop = ModContent.ItemType<Items.Placeables.GiuxBar>();
			soundType = SoundID.Tink;
			soundStyle = 1;
			//mineResist = 4f;
			//minPick = 200;
		}
	}
}
