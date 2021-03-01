using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace GiuxItems.Items.Placeables
{
    public class BathrGeode : ModItem
    {

        public override void SetStaticDefaults()
        {
            ItemID.Sets.SortingPriorityMaterials[item.type] = 58;
            DisplayName.SetDefault("Bathr Geode");
            Tooltip.SetDefault("Crystalized");
        }

        public override void SetDefaults()
        {
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.useTurn = true;
            item.useAnimation = 15;
            item.useTime = 10;
            item.autoReuse = true;
            item.width = 26;
            item.height = 26;
            item.consumable = true;
            item.material = true;
            item.maxStack = 42;
            item.rare = ItemRarityID.Expert;
            item.material = true;
            item.value = Item.gold * 10;
            item.createTile = ModContent.TileType<Tiles.BathrGeode>();
        }
    }
}
