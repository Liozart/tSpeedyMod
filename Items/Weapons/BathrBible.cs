using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using static GiuxItems.NPCs.BadSpeedy;
using GiuxItems.Projectiles;

namespace GiuxItems.Items.Weapons
{
    public class BathrBible : ModItem
    {
        public override void SetStaticDefaults()
        {
            item.SetNameOverride("The Bathr Bible");
            Tooltip.SetDefault("Feelin fuzzy, gettin dizzy");
            Item.staff[item.type] = true; //this makes the useStyle animate as a staff instead of as a gun
        }

        public override void SetDefaults()
        {
            item.damage = 420;
            item.magic = true;
            item.mana = 42;
            item.width = 80;
            item.height = 80;
            item.useTime = 60;
            item.useAnimation = 60;
            item.useStyle = ItemUseStyleID.HoldingUp;
            item.noMelee = true; //so the item's animation doesn't do damage
            item.knockBack = 24;
            item.value = Item.buyPrice(gold: 10);
            item.rare = ItemRarityID.Green;
            item.UseSound = SoundID.Item10;
            item.autoReuse = true;
            item.shoot = ProjectileID.FallingStar;
            item.shootSpeed = 7.5f;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Dust.NewDust(new Vector2(position.X, position.Y), item.width, item.height, 110);
            Vector2 target1 = Main.screenPosition + new Vector2((float)Main.mouseX, (float)Main.mouseY);
            position = player.Center;
            Vector2 heading1 = target1 - position;
            heading1.Normalize();
            heading1 *= new Vector2(speedX, speedY).Length();
            speedX = heading1.X;
            speedY = heading1.Y;
            Projectile.NewProjectile(position.X, position.Y, speedX, speedY, ProjectileType<SpriteFlame>(), damage, knockBack, player.whoAmI);
            return false;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.BeeGun, 1);
            recipe.AddIngredient(ItemID.JungleSpores, 1);
            recipe.AddIngredient(ItemType<BadSpeedyItem>(), 420);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}