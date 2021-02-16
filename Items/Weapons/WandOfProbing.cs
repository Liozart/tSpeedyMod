using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using static GiuxItems.NPCs.Speedy;

namespace GiuxItems.Items.Weapons
{
    class WandOfProbing : ModItem
    {
        public override void SetDefaults()
        {
            item.damage = 65;
            item.ranged = true;
            item.width = 40;
            item.height = 20;
            item.useTime = 22;
            item.useAnimation = 22;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.noMelee = true;
            item.knockBack = 7;
            item.value = Item.buyPrice(gold: 11);
            item.rare = ItemRarityID.Green;
            item.UseSound = SoundID.Item11;
            item.autoReuse = true;
            item.shootSpeed = 12f;
            item.shoot = ProjectileID.StarWrath;
        }

        public override void HoldItem(Player player)
        {
            player.nightVision = true;
            base.HoldItem(player);
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            position = Main.MouseWorld;
            float numberProjectiles = 12;
            float rotation = MathHelper.ToRadians(180);
            position += Vector2.Normalize(new Vector2(speedX, speedY)) * 45f;
            for (int i = 0; i < numberProjectiles; i++)
            {
                Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1))); // Watch out for dividing by 0 if there is only 1 projectile.
                Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
            }
            Dust.NewDust(position, player.width, player.height, DustID.PinkFlame);
            return false;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.GoldGrasshopper, 1);
            recipe.AddIngredient(ItemID.SoulofNight, 5);
            recipe.AddIngredient(ItemType<SpeedyItem>(), 1);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
