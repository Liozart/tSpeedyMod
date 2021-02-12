using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using static GiuxItems.NPCs.Speedy;

namespace GiuxItems.Items.Weapons
{
	public class LeosLove : ModItem
	{
		public override void SetStaticDefaults() {
            item.SetNameOverride("Léo's Love");
			Tooltip.SetDefault("You can feel his warmth");
			Item.staff[item.type] = true; //this makes the useStyle animate as a staff instead of as a gun
		}

		public override void SetDefaults() {
			item.damage = 35;
			item.magic = true;
			item.mana = 6;
			item.width = 22;
			item.height = 22;
			item.useTime = 20;
			item.useAnimation = 20;
			item.useStyle = ItemUseStyleID.HoldingUp;
			item.noMelee = true; //so the item's animation doesn't do damage
			item.knockBack = 5;
            item.value = Item.buyPrice(gold: 10);
            item.rare = ItemRarityID.Green;
			item.UseSound = SoundID.Item10;
			item.autoReuse = true;
			item.shoot = ProjectileID.DD2FlameBurstTowerT3Shot;

            item.shootSpeed = 16f;
		}

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            //Shoot fireball
            Vector2 target1 = Main.screenPosition + new Vector2((float)Main.mouseX, (float)Main.mouseY);
            position = player.Center;
            Vector2 heading1 = target1 - position;
            heading1.Normalize();
            heading1 *= new Vector2(speedX, speedY).Length();
            speedX = heading1.X;
            speedY = heading1.Y;
            Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage, knockBack, player.whoAmI);

            //Shoot stars from the sky
            Vector2 target = player.Center;
            float ceilingLimit = target.Y;
            if (ceilingLimit > player.Center.Y - 200f)
            {
                ceilingLimit = player.Center.Y - 200f;
            }
            for (int i = 0; i < 2; i++)
            {
                position = player.Center + new Vector2((-(float)Main.rand.Next(0, 401) * player.direction), -600f);
                position.Y -= (100 * i);
                Vector2 heading = target - position;
                if (heading.Y < 0f)
                {
                    heading.Y *= -1f;
                }
                if (heading.Y < 20f)
                {
                    heading.Y = 20f;
                }
                heading.Normalize();
                heading *= new Vector2(speedX, speedY).Length();
                speedY = heading.Y + Main.rand.Next(-40, 41);
                Projectile.NewProjectile(position.X, position.Y, speedX, speedY, ProjectileID.HallowStar, damage * 2, knockBack, player.whoAmI, 0f, ceilingLimit);
            }
            return false;
        }

        public override void OnHitNPC(Player player, NPC target, int damage, float knockback, bool crit)
        {
            // Add the Onfire buff to the NPC for 1 second when the weapon hits an NPC
            // 60 frames = 1 second
            target.AddBuff(BuffID.Lovestruck, 360);
        }

        public override void AddRecipes() {
			ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.HeartLantern, 1);
            recipe.AddIngredient(ItemID.SilverChainmail, 1);
            recipe.AddIngredient(ItemID.ClayBlock, 25);
            recipe.AddIngredient(ItemType<SpeedyItem>(), 1);
            recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}