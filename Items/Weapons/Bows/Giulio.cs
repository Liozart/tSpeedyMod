using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using GiuxItems.Projectiles;
using static GiuxItems.NPCs.Critters.Speedy;
using System;
using System.Windows;

namespace GiuxItems.Items.Weapons.Bows
{
    public class Giulio : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Does weird stuff--");
        }

        public override void SetDefaults()
        {
            item.damage = 80;
            //item.ranged = true;
            item.magic = true;
            item.width = 40;
            item.height = 20;
            item.useTime = 16;
            item.useAnimation = 16;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.noMelee = true;
            item.knockBack = 4;
            item.value = Item.buyPrice(gold: 10);
            item.rare = ItemRarityID.Expert;
            item.UseSound = SoundID.Item11;
            item.autoReuse = true;
            item.shootSpeed = 18f;
            item.shoot = ProjectileID.VortexBeaterRocket;
            item.useAmmo = AmmoID.Arrow;
        }

        public override bool ConsumeAmmo(Player player)
		{
			return Main.rand.NextFloat() >= .55f;
		}

        public override void HoldItem(Player player)
        {
            player.nightVision = true;
            base.HoldItem(player);
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                item.useStyle = ItemUseStyleID.SwingThrow;
                item.useTime = 60;
                item.useAnimation = 60;
            }
            else
            {
                item.useStyle = ItemUseStyleID.HoldingOut;
                item.useTime = 16;
                item.useAnimation = 16;
            }
            return base.CanUseItem(player);
        }

        int t = 0;
        const int tmax = 2;
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            if (player.altFunctionUse == 2)
            {
                Vector2 target = player.Center;
                target.X += (300 * player.direction);
                float ceilingLimit = target.Y;
                if (ceilingLimit > player.Center.Y - 200f)
                {
                    ceilingLimit = player.Center.Y - 200f;
                }
                for (int i = -16; i < 16; i++)
                {
                    position = player.Center + new Vector2((-(float)(100 + i * 20)  * player.direction), -600f);
                    position.Y -= (100 * i);
                    Vector2 heading = target - position;
                    heading.Normalize();
                    heading *= new Vector2(speedX, speedY).Length();
                    speedY = heading.Y;
                    Projectile.NewProjectile(position.X, position.Y, speedX, speedY, ProjectileID.RubyBolt, damage * 2, knockBack, player.whoAmI, 0f, ceilingLimit);
                }
            }
            else
            {
                position = player.Center;
                float numberProjectiles = 3;
                float rotation = MathHelper.ToRadians(5);
                position += Vector2.Normalize(new Vector2(speedX, speedY)) * 45f;
                for (int i = 0; i < numberProjectiles; i++)
                {
                    Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1))); // Watch out for dividing by 0 if there is only 1 projectile.
                    if (i == 1 && t == tmax)
                    {
                        Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, ProjectileID.VortexBeaterRocket, damage, knockBack, player.whoAmI);
                        t = 0;
                    }
                    else
                        Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, ProjectileID.ShadowFlameArrow, damage, knockBack, player.whoAmI);
                }
                t++;
            }

            return false;
        }


        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.BorealWoodBow, 1);
            recipe.AddIngredient(ItemID.GoldButterfly, 1);
            recipe.AddIngredient(ItemType<SpeedyItem>(), 1);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
