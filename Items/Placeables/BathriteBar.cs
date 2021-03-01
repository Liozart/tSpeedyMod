using Terraria.ID;
using Terraria.ModLoader;

namespace GiuxItems.Items.Placeables
{
	public class BathriteBar : ModItem
	{
		public override void SetStaticDefaults()
		{
			ItemID.Sets.SortingPriorityMaterials[item.type] = 58;
			DisplayName.SetDefault("Bathrite chunks");
			Tooltip.SetDefault("Big Chungus's ashes");
		}

		public override void SetDefaults()
		{
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useTurn = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.autoReuse = true;
			item.maxStack = 420;
			item.consumable = true;
			item.createTile = ModContent.TileType<Tiles.BathriteBar>();
			item.width = 17;
			item.height = 18;
			item.value = 30000;
            item.material = true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<Bathrite>(), 8);
            recipe.AddTile(TileID.Furnaces);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
