using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using static GiuxItems.NPCs.Speedy;
using GiuxItems.Items.Placeables;

namespace GiuxItems.Items.Weapons.Guns
{
	public class Bathroo : ModItem
	{
		public override void SetStaticDefaults() {
			Tooltip.SetDefault("The Bathroo is the most powerful gun in the galaxy");
		}

		public override void SetDefaults() {
			item.damage = 25;
			item.ranged = true;
			item.width = 40;
			item.height = 20;
			item.useTime = 12;
			item.useAnimation = 12;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.noMelee = true; //so the item's animation doesn't do damage
			item.knockBack = 3;
			item.value = Item.buyPrice(gold: 5);
			item.rare = ItemRarityID.Expert;
			item.UseSound = SoundID.Item11;
			item.autoReuse = true;
            item.shoot = ProjectileID.CursedArrow;
			item.shootSpeed = 16f;
            item.useAmmo = AmmoID.Bullet;
        }

		public override void AddRecipes() {
			ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.IllegalGunParts, 2);
            recipe.AddIngredient(ItemID.Javelin, 50);
            recipe.AddIngredient(ItemID.Shuriken, 50);
            recipe.AddIngredient(ItemID.SilverBullet, 50);
            recipe.AddIngredient(ItemType<GiuxBar>(), 16);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
			recipe.AddRecipe();
		}

        // Randomly shoot something
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            // Here we randomly set type to either the original (as defined by the ammo), a vanilla projectile, or a mod projectile.
            type = Main.rand.Next(new int[] { 1, 2, 3, 4, 5, 9, 10, 14, 15, 16, 19, 20, 21, 22, 24, 27, 30, 34, 36, 41, 48, 54, 55, 93, 88, 89, 90, 91, 103,
                                                104, 106, 116, 117, 118, 119, 120, 162, 121, 122, 123, 124, 125, 126, 132, 134,
                                                156, 165, 157, 172, 173, 182, 206, 274, 278, 281, 282, 283, 284, 285, 286, 287, 304, 306, 477, 478, 479, 480});
            return true;
        }
    }
}
