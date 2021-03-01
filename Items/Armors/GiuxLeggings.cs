using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using GiuxItems.Items.Placeables;

namespace GiuxItems.Items.Armors
{
	[AutoloadEquip(EquipType.Legs)]
	public class GiuxLeggings : ModItem
	{
		public override void SetStaticDefaults() {
			Tooltip.SetDefault("Giux's trousers"
				+ "\nIncrease movement speed");
		}

		public override void SetDefaults() {
			item.width = 18;
			item.height = 18;
			item.value = 10000;
			item.rare = ItemRarityID.Green;
			item.defense = 5;
		}

		public override void UpdateEquip(Player player) {
			player.moveSpeed += 0.25f;
		}

		public override void AddRecipes() {
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<GiuxBar>(), 10);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}