using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using static GiuxItems.NPCs.Speedy;
using static Terraria.ModLoader.ModContent;

namespace GiuxItems.Items.Weapons
{
	public class GreenBunny : ModItem
	{
		public override void SetStaticDefaults() 
		{
            Tooltip.SetDefault("Speedy's sword, can smell tangerine leafs\nApplies horrible toxins\nAlternate throws flails");
        }

		public override void SetDefaults() 
		{
			item.damage = 35;
			item.melee = true;
			item.width = 74;
			item.height = 90;
			item.useTime = 35;
			item.useAnimation = 35;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.knockBack = 10;
			item.value = Item.buyPrice(gold: 10);
			item.rare = ItemRarityID.Green;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;
            item.shoot = ProjectileID.None;
            item.shootSpeed = 5f;
        }

		public override void AddRecipes() 
		{
			ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.GoldBar, 5);
            recipe.AddIngredient(ItemID.Shiverthorn, 3);
            recipe.AddIngredient(ItemID.Daybloom, 3);
            recipe.AddIngredient(ItemType<SpeedyItem>(), 1);
            recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

        //Adds defense buffs when hitting
        public override void OnHitNPC(Player player, NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.Venom, 1200);
            target.AddBuff(BuffID.Poisoned, 1200);
            target.AddBuff(BuffID.Midas, 1200);
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                item.useStyle = ItemUseStyleID.HoldingUp;
                item.useTime = 45;
                item.useAnimation = 45;
                item.shoot = ProjectileID.BallOHurt;
            }
            else
            {
                item.useStyle = ItemUseStyleID.SwingThrow;
                item.useTime = 35;
                item.useAnimation = 35;
                item.shoot = ProjectileID.None;
            }
            return base.CanUseItem(player);
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            position = Main.MouseWorld;
            float numberProjectiles = 3;
            float rotation = MathHelper.ToRadians(25);
            position += Vector2.Normalize(new Vector2(speedX, speedY)) * 45f;
            for (int i = 0; i < numberProjectiles; i++)
            {
                Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1))); // Watch out for dividing by 0 if there is only 1 projectile.
                Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
            }
            return false;
        }

        //Some effects when hitting
        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 131);
        }
    }
}