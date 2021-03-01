using GiuxItems.Items.Placeables;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace GiuxItems.Items.Armors
{
	[AutoloadEquip(EquipType.Body)]
	public class GiuxChestplate : ModItem
	{
		public override void SetStaticDefaults() {
			base.SetStaticDefaults();
			DisplayName.SetDefault("Giux Chestplate");
			Tooltip.SetDefault("Giux's armor"
				+ "\nAdd mana and regen");
		}

		public override void SetDefaults() {
			item.width = 18;
			item.height = 18;
			item.value = 10000;
			item.rare = ItemRarityID.LightPurple;
			item.defense = 5;
		}

		public override void UpdateEquip(Player player) {
			player.buffImmune[BuffID.Frostburn] = true;
			player.manaRegen += 10;
			player.manaRegenDelay /= 2;
			player.statManaMax2 += 42;
		}

		public override void AddRecipes() {
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<GiuxBar>(), 15);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}