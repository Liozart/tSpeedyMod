using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using GiuxItems.Items.Placeables;
using GiuxItems.Buffs;

namespace GiuxItems.Items.Armors
{
	[AutoloadEquip(EquipType.Head)]
	public class GiuxHelmet : ModItem
	{
		public override void SetStaticDefaults() {
			Tooltip.SetDefault("Giux's Helmet\nUp Damages\nBonus set : Telesafe buff");
		}

		public override void SetDefaults() {
			item.width = 18;
			item.height = 18;
			item.value = 42000;
			item.rare = ItemRarityID.Green;
			item.defense = 6;
		}

		public override bool IsArmorSet(Item head, Item body, Item legs) {
			return body.type == ModContent.ItemType<GiuxChestplate>() && legs.type == ModContent.ItemType<GiuxLeggings>();
		}

		public override void UpdateArmorSet(Player player)
		{
			player.AddBuff(ModContent.BuffType<Telesafe>(), 10);
			player.thrownDamage -= 0.2f;
			player.magicDamage -= 0.2f;
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