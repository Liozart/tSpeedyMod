using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace GiuxItems.Items.Placeables
{
	public class ExampleChest : ModItem
	{
		public override void SetStaticDefaults() {
			Tooltip.SetDefault("This is a modded chest.");
		}

		public override void SetDefaults() {
			item.width = 26;
			item.height = 22;
			item.maxStack = 99;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.consumable = true;
			item.value = 500;
			item.createTile = TileType<Tiles.ExampleChest>();
		}
	}
}