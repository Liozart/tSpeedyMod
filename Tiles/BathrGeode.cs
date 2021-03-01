using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace GiuxItems.Tiles
{
    public class BathrGeode : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = false;
            Main.tileSolidTop[Type] = true;
            Main.tileTable[Type] = true;
            Main.tileFrameImportant[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.LavaDeath = false;
            TileObjectData.newTile.CoordinateHeights = new int[] { 16 };
            TileObjectData.addTile(Type);

            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Bathr Geode");
            AddMapEntry(new Color(0, 244, 0), name);
            dustType = 87;
            drop = ModContent.ItemType<Items.Placeables.BathrGeode>();
            soundType = SoundID.Tink;
            soundStyle = 1;
            //mineResist = 4f;
            //minPick = 200;
        }
    }
}
