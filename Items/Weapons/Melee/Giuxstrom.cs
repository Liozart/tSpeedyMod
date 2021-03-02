using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using static Terraria.ModLoader.ModContent;
using GiuxItems.Items.Placeables;

namespace GiuxItems.Items.Weapons.Melee
{
    public class Giuxstrom : ModItem
    {

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Giux's tool of world destruction");
        }

        public override void SetDefaults()
        {
            item.damage = 420;
            item.melee = true;
            item.width = 94;
            item.height = 94;
            item.useTime = 45;
            item.useAnimation = 45;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.knockBack = 18;
            item.value = Item.buyPrice(gold: 10);
            item.rare = ItemRarityID.Green;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
        }

        public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
        {
            target.AddBuff(BuffID.Ichor, 300);

            //Shoot stars from the sky
            for (int i = -2; i < 3; i++)
                Projectile.NewProjectile(target.position.X, target.position.Y - (50 * 16), (i * Main.rand.Next(1, 4)), 10, ProjectileID.PulseBolt, damage * 2, knockBack, player.whoAmI);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.BreakerBlade, 1);
            recipe.AddIngredient(ItemID.Shackle, 1);
            recipe.AddIngredient(ItemType<BathriteBar>(), 20);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        //Some effects when hitting
        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 107);
        }
    }
}