using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using GiuxItems.Projectiles;
using GiuxItems.Items.Placeables;
using Microsoft.Xna.Framework;
using static GiuxItems.NPCs.Critters.Froggy1;
using static GiuxItems.NPCs.Critters.Froggy2;

namespace GiuxItems.Items.Weapons.Bows
{
    public class Crapulator : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Turns more seeds into more frogs");
        }

        public override void SetDefaults()
        {
            item.damage = 46;
            item.ranged = true;
            item.width = 48;
            item.height = 24;
            item.useTime = 14;
            item.useAnimation = 14;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.noMelee = true;
            item.knockBack = 4;
            item.value = Item.buyPrice(silver: 88);
            item.rare = ItemRarityID.Expert;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
            item.shootSpeed = 12f;
            item.shoot = ProjectileType<Froggy>();
            item.useAmmo = ItemID.Seed;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(5, -5);
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            position = player.Center;
            float numberProjectiles = 4;
            float rotation = MathHelper.ToRadians(7);
            position += Vector2.Normalize(new Vector2(speedX, speedY)) * 45f;
            for (int i = 0; i < numberProjectiles; i++)
            {
                Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1))); // Watch out for dividing by 0 if there is only 1 projectile.
                Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, ProjectileType<Froggy>(), damage, knockBack, player.whoAmI);
            }
            item.useAnimation = Main.rand.Next(4, 36);
            item.useTime = Main.rand.Next(4, 36);
            return false;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<Froggy1Item>(), 3);
            recipe.AddIngredient(ItemType<Froggy2Item>(), 3);
            recipe.AddIngredient(ItemType<Crapuleur>(), 1);
            recipe.AddIngredient(ItemID.CursedFlame, 7);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
