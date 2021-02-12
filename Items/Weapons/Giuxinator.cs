using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using static Terraria.ModLoader.ModContent;
using static GiuxItems.NPCs.Speedy;
using static GiuxItems.NPCs.BadSpeedy;

namespace GiuxItems.Items.Weapons
{
	public class Giuxinator : ModItem
	{
		public override void SetStaticDefaults() 
		{
            Tooltip.SetDefault("Giux's glorious sword\nAdds defence buffs on hit\nAlternate heals full for all your mana");
        }

		public override void SetDefaults() 
		{
			item.damage = 25;
			item.melee = true;
			item.width = 56;
			item.height = 120;
			item.useTime = 14;
			item.useAnimation = 14;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.knockBack = 6;
			item.value = Item.buyPrice(gold: 10);
			item.rare = ItemRarityID.Green;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;
		}

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                if (player.statMana == player.statManaMax2)
                {
                    player.HealEffect(player.statLifeMax2 - player.statLife);
                    player.statLife = player.statLifeMax2;
                    player.statMana = 0;
                    item.useStyle = ItemUseStyleID.HoldingOut;
                    item.useTime = 35;
                    item.useAnimation = 35;
                    return base.CanUseItem(player);
                }
                else return false;
            }
            else
            {
                item.useStyle = ItemUseStyleID.SwingThrow;
                item.useTime = 18;
                item.useAnimation = 18;
                return base.CanUseItem(player);
            }
        }

        public override void AddRecipes() 
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.IronShortsword, 1);
            recipe.AddIngredient(ItemID.Ruby, 1);
            recipe.AddIngredient(ItemID.Sapphire, 1);
            recipe.AddIngredient(ItemID.Topaz, 1);
            recipe.AddIngredient(ItemID.Emerald, 1);
            recipe.AddIngredient(ItemType<SpeedyItem>(), 1);
            recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

        //Adds defense buffs when hitting
        public override void OnHitNPC(Player player, NPC target, int damage, float knockback, bool crit)
        {
            player.ClearBuff(2);
            player.ClearBuff(5);
            player.ClearBuff(6);
            player.ClearBuff(114);
            player.AddBuff(2, 3600);
            player.AddBuff(5, 3600);
            player.AddBuff(6, 3600);
            player.AddBuff(114, 3600);
        }

        //Some effects when hitting
        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 279);
            if (Main.rand.NextBool(3))
            {
                //Emit dusts when the sword is swung
                Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, DustID.Blood);
            }
            else
            {
                Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, DustID.SomethingRed);
            }
        }
    }
}