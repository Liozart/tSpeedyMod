using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using static GiuxItems.NPCs.Speedy;
using GiuxItems.Items.Placeables;
using static GiuxItems.NPCs.BadSpeedy;

namespace GiuxItems.Items.Weapons.Staffs
{
    class WandOfProbing : ModItem
    {
        public override void SetStaticDefaults()
        {
            item.SetNameOverride("Wand of Probing");
            Tooltip.SetDefault("Wow");
            Item.staff[item.type] = true; //this makes the useStyle animate as a staff instead of as a gun
        }

        public override void SetDefaults()
        {
            item.damage = 65;
            item.ranged = true;
            item.width = 40;
            item.height = 20;
            item.useTime = 30;
            item.useAnimation = 30;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.noMelee = true;
            item.knockBack = 7;
            item.value = Item.buyPrice(gold: 55);
            item.rare = ItemRarityID.Cyan;
            item.UseSound = SoundID.Item11;
            item.autoReuse = true;
            item.shootSpeed = 12f;
            item.shoot = ProjectileID.StarWrath;
        }

        public override void HoldItem(Player player)
        {
            player.itemLocation.Y = player.Center.Y + 10 * player.direction;
            player.itemLocation.X = player.Center.X + 10 * player.direction;
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
            //DustID 20 = purification powder
            Dust.NewDust(position, 40, 40, 20);
            return false;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.SoulofSight, 5);
            recipe.AddIngredient(ItemType<BadSpeedyItem>(), 420);
            recipe.AddIngredient(ItemType<BathrGeode>(), 1);
            recipe.AddIngredient(ItemType<BathriteBar>(), 12);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
