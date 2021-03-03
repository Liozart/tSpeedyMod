using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using GiuxItems.Items.Placeables;
using GiuxItems.Buffs;

namespace GiuxItems.Items.Armors
{
	[AutoloadEquip(EquipType.Head)]
	public class BathriteHelmet : ModItem
	{
		public override void SetStaticDefaults() {
			Tooltip.SetDefault("Up Damages\nBonus set : Butterlies gardians");
		}

		public override void SetDefaults() {
			item.width = 18;
			item.height = 18;
			item.value = 42000;
			item.rare = ItemRarityID.LightPurple;
			item.defense = 8;
		}

		public override bool IsArmorSet(Item head, Item body, Item legs) {
			return body.type == ModContent.ItemType<BathriteChestplate>() && legs.type == ModContent.ItemType<BathriteLeggings>();
        }

        public override void UpdateEquip(Player player)
        {
            player.allDamage += 0.25f;
        }
        
        public override void UpdateArmorSet(Player player)
		{
            player.AddBuff(ModContent.BuffType<ButterflyBarrier>(), 10);
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