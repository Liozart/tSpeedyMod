using GiuxItems.Items.Placeables;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace GiuxItems.Items.Armors
{
	[AutoloadEquip(EquipType.Body)]
	public class BathriteChestplate : ModItem
	{
		public override void SetStaticDefaults() {
			base.SetStaticDefaults();
			Tooltip.SetDefault("Add mana and life regen");
		}

		public override void SetDefaults() {
			item.width = 18;
			item.height = 18;
			item.value = 100000;
			item.rare = ItemRarityID.LightPurple;
			item.defense = 15;
		}

		public override void UpdateEquip(Player player) {
			player.buffImmune[BuffID.Frostburn] = true;
			player.manaRegen += 15;
            player.lifeRegen += 5;
			player.manaRegenDelay /= 3;
            player.lifeRegenTime /= 2;
			player.statManaMax2 += 65;
		}

		public override void AddRecipes() {
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<BathriteBar>(), 15);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}