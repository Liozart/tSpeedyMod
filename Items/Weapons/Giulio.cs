using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using GiuxItems.Projectiles;
using static GiuxItems.NPCs.Speedy;
using System;
using System.Windows;

namespace GiuxItems.Items.Weapons
{
    public class Giulio : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Shoots homing flame arrows instead");
        }

        public float arrowOffset = 5f;
        public override void SetDefaults()
        {
            item.damage = 80;
            item.ranged = true;
            item.width = 40;
            item.height = 20;
            item.useTime = 16;
            item.useAnimation = 16;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.noMelee = true;
            item.knockBack = 4;
            item.value = Item.buyPrice(gold: 10);
            item.rare = ItemRarityID.Green;
            item.UseSound = SoundID.Item11;
            item.autoReuse = true;
            item.shootSpeed = 12f;
            item.shoot = ProjectileID.StarWrath;
        }

        public override void HoldItem(Player player)
        {
            player.lightOrb = true;
            base.HoldItem(player);
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            //Shoots ligned projectiles
            Vector2 target1 = Main.screenPosition + new Vector2((float)Main.mouseX, (float)Main.mouseY);
            position = player.Center;
            Vector2 heading1 = target1 - position;
            heading1.Normalize();
            double angle = Vector2.Dot(heading1, new Vector2(0 ,90));
            heading1 *= new Vector2(speedX, speedY).Length();
            speedX = heading1.X;
            speedY = heading1.Y;
            Vector2 r1 = new Vector2(arrowOffset * ((90 - (float)angle) / 100), arrowOffset * ((90 - (float)angle) / 100));
            Main.NewText(r1.X + " " + r1.Y);
            Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage, knockBack, player.whoAmI);
            Projectile.NewProjectile(position.X + r1.X, position.Y - r1.Y, speedX, speedY, type, damage, knockBack, player.whoAmI);
            Projectile.NewProjectile(position.X + r1.X, position.Y - r1.Y, speedX, speedY, type, damage, knockBack, player.whoAmI);

            return false;
        }


        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.GoldButterfly, 1);
            recipe.AddIngredient(ItemID.GuideVoodooDoll, 1);
            recipe.AddIngredient(ItemType<SpeedyItem>(), 1);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
