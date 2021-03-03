using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using static GiuxItems.NPCs.Hostiles.BadSpeedy;
using GiuxItems.Items.Placeables;
using Microsoft.Xna.Framework;

namespace GiuxItems.Items.Weapons.Melee
{
	public class PestDagger : ModItem
	{
		public override void SetStaticDefaults() 
		{
            Tooltip.SetDefault("Filthy stabs");
        }

		public override void SetDefaults() 
		{
			item.damage = 25;
			item.melee = true;
			item.width = 36;
			item.height = 38;
			item.useTime = 8;
			item.useAnimation = 8;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.knockBack = 2;
            item.crit *= 2;
			item.value = Item.buyPrice(gold: 10);
			item.rare = ItemRarityID.LightRed;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;
		}
        
        public override bool UseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                Vector2 pos = new Vector2(player.position.X + (32 * player.direction), player.position.Y);
                for (int i = 0; i < 10; i++)
                    Dust.NewDust(pos, item.width, item.height, 61, 8 * player.direction, 0);
                if (Main.rand.Next(0, 9) == 0)
                    Projectile.NewProjectile(new Vector2(player.position.X + (36 * player.direction), player.position.Y + 24), new Vector2(10 * player.direction, 0), ProjectileID.EatersBite, 34, 6, player.whoAmI);
            }
            else
            {
                Vector2 pos = new Vector2(player.position.X + (25 * player.direction), player.position.Y + (55 * player.direction));
                for (int i = 0; i < 10; i++)
                    Dust.NewDust(pos, item.width, item.height, 61, 8 * player.direction, -2);
            }
            return true;
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                item.useStyle = ItemUseStyleID.Stabbing;
            }
            else
            {
                item.useStyle = ItemUseStyleID.SwingThrow;
            }
            return base.CanUseItem(player);
        }

        public override void AddRecipes() 
		{
			ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Obsidian, 4);
            recipe.AddIngredient(ItemType<BathriteBar>(), 4);
            recipe.AddIngredient(ItemType<GiuxBar>(), 4);
            recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

        //Adds defense buffs when hitting
        public override void OnHitNPC(Player player, NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.CursedInferno, 120);
        }
    }
}