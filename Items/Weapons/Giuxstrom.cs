using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using static Terraria.ModLoader.ModContent;
using static GiuxItems.NPCs.Speedy;
using static GiuxItems.NPCs.BadSpeedy;
using GiuxItems.Projectiles;

namespace GiuxItems.Items.Weapons
{
    public class Giuxstrom : ModItem
    {

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Giux's tool of world destruction");
        }

        public override void SetDefaults()
        {
            item.damage = 80;
            item.melee = true;
            item.width = 94;
            item.height = 94;
            item.useTime = 50;
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
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.GoldCrown, 1);
            recipe.AddIngredient(ItemID.Shackle, 1);
            recipe.AddIngredient(ItemType<BadSpeedyItem>(), 420);
            recipe.AddTile(TileID.WorkBenches);
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