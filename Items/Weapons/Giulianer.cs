using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using static GiuxItems.NPCs.BadSpeedy;

namespace GiuxItems.Items.Weapons
{
    public class Giulianer : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("The Giulianer is the most powerful bow in the galaxy");
        }

        public override void SetDefaults()
        {
            item.damage = 45;
            item.ranged = true;
            item.width = 40;
            item.height = 20;
            item.useTime = 13;
            item.useAnimation = 13;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.noMelee = true;
            item.knockBack = 4;
            item.value = Item.buyPrice(gold: 5);
            item.rare = ItemRarityID.Green;
            item.UseSound = SoundID.Item11;
            item.autoReuse = true;
            item.shoot = ProjectileID.CursedArrow;
            item.shootSpeed = 16f;
            item.useAmmo = AmmoID.Arrow;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.GreenWrench, 1);
            recipe.AddIngredient(ItemID.SlimeGun, 3);
            recipe.AddIngredient(ItemType<BadSpeedyItem>(), 420);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        // Randomly shoot something
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            // Here we randomly set type to either the original (as defined by the ammo), a vanilla projectile, or a mod projectile.
            type = Main.rand.Next(new int[] { 1, 2, 4, 5, 41, 91, 103, 117, 120, 172, 225, 278, 282, 357, 469, 474,
            485, 495, 631, 639, 640});
            return true;
        }
    }
}
