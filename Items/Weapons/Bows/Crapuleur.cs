using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using GiuxItems.Projectiles;
using GiuxItems.Items.Placeables;
using static GiuxItems.NPCs.Critters.Speedy;
using Microsoft.Xna.Framework;

namespace GiuxItems.Items.Weapons.Bows
{
    public class Crapuleur : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Turns seeds into frogs");
        }

        public override void SetDefaults()
        {
            item.damage = 32;
            item.ranged = true;
            item.width = 48;
            item.height = 24;
            item.useTime = 22;
            item.useAnimation = 22;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.noMelee = true;
            item.knockBack = 4;
            item.value = Item.buyPrice(silver: 8);
            item.rare = ItemRarityID.Pink;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
            item.shootSpeed = 12f;
            item.shoot = ProjectileType<Froggy>();
            item.useAmmo = ItemID.Seed;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(5, -8);
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Projectile.NewProjectile(position, new Vector2(speedX, speedY), ProjectileType<Froggy>(), damage, knockBack, player.whoAmI);
            return false;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Frog, 1);
            recipe.AddIngredient(ItemID.Blowpipe, 1);
            recipe.AddIngredient(ItemType<BathrGeode>(), 1);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
