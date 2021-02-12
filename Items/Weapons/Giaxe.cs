using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using static Terraria.ModLoader.ModContent;
using static GiuxItems.NPCs.Speedy;
using static GiuxItems.NPCs.BadSpeedy;
using Microsoft.Xna.Framework.Graphics;

namespace GiuxItems.Items.Weapons
{
    public class Giaxe : ModItem
    {
        private int timerFramesSkill = 0;

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Giux's enormous axe\nIt gets quicker when hitting");
        }

        public override void SetDefaults()
        {
            item.damage = 30;
            item.melee = true;
            item.width = 94;
            item.height = 94;
            item.useTime = 30;
            item.useAnimation = 30;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.knockBack = 15;
            item.value = Item.buyPrice(gold: 10);
            item.rare = ItemRarityID.Green;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.ClimbingClaws, 1);
            recipe.AddIngredient(ItemID.BandofRegeneration, 1);
            recipe.AddIngredient(ItemType<BadSpeedyItem>(), 420);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override void UpdateInventory(Player player)
        {
            if (timerFramesSkill > 0)
                timerFramesSkill--;
            else
            {
                item.useTime = 30;
                item.useAnimation = 30;
            }
            base.UpdateInventory(player);
        }

        //Adds defense buffs when hitting
        public override void OnHitNPC(Player player, NPC target, int damage, float knockback, bool crit)
        {
            if (item.useTime > 10)
            {
                item.useTime -= 2;
                item.useAnimation -= 2;
                timerFramesSkill = 600;
            }
            else
                timerFramesSkill = 600;
        }

        //Some effects when hitting
        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (item.useAnimation < 30)
                Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, DustID.PinkFlame);
            else
                Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 221);
        }
    }
}