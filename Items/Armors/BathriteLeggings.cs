using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using GiuxItems.Items.Placeables;

namespace GiuxItems.Items.Armors
{
	[AutoloadEquip(EquipType.Legs)]
	public class BathriteLeggings : ModItem
	{
		public override void SetStaticDefaults() {
			Tooltip.SetDefault("Increase movement speed and jump height");
		}

		public override void SetDefaults() {
			item.width = 18;
			item.height = 18;
			item.value = 42000;
			item.rare = ItemRarityID.LightPurple;
			item.defense = 10;
		}

		public override void UpdateEquip(Player player) {
			player.moveSpeed += 0.30f;
            Player.jumpHeight += 15;
		}

		public override void AddRecipes() {
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<BathriteBar>(), 10);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}