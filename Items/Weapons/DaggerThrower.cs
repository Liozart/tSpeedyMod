using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using GiuxItems.Projectiles;
using static GiuxItems.NPCs.BadSpeedy;
using Microsoft.Xna.Framework.Graphics;

namespace GiuxItems.Items.Weapons
{
    public class DaggerThrower : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Secrets Giux's dimensional daggers");
        }

        public override void SetDefaults()
        {
            item.damage = 55;
            item.ranged = true;
            item.width = 52;
            item.height = 52;
            item.useTime = 18;
            item.useAnimation = 18;
            item.melee = false;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.knockBack = 4;
            item.value = Item.buyPrice(gold: 3);
            item.rare = ItemRarityID.Green;
            item.UseSound = SoundID.Item11;
            item.autoReuse = true;
            item.shoot = ProjectileType<SecretDagger>();
            item.shootSpeed = 8f;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Dust.NewDust(new Vector2(position.X, position.Y), item.width, item.height, 244);
            return true;
        }

        public override void HoldItem(Player player)
        {
            player.itemLocation.Y = player.Center.Y;
            player.itemLocation.X = player.Center.X - 15 * player.direction;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.FlareGun, 1);
            recipe.AddIngredient(ItemID.Flare, 42);
            recipe.AddIngredient(ItemType<BadSpeedyItem>(), 420);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
