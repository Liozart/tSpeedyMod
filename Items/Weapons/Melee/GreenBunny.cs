using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using static GiuxItems.NPCs.Speedy;
using static Terraria.ModLoader.ModContent;
using GiuxItems.Items.Placeables;

namespace GiuxItems.Items.Weapons.Melee
{
	public class GreenBunny : ModItem
	{
		public override void SetStaticDefaults() 
		{
            Tooltip.SetDefault("Speedy's sword, can smell tangerine leafs\nApplies horrible toxins\nAlternate throws anchors");
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
            item.shootSpeed = 24f;
        }

		public override void AddRecipes() 
		{
			ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Shiverthorn, 3);
            recipe.AddIngredient(ItemID.Daybloom, 3);
            recipe.AddIngredient(ItemType<GiuxBar>(), 15);
            recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

        //Adds defense buffs when hitting
        public override void OnHitNPC(Player player, NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.Venom, 3600);
            target.AddBuff(BuffID.Poisoned, 3600);
            target.AddBuff(BuffID.CursedInferno, 3600);
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                item.useTime = 60;
                item.useAnimation = 60;
                item.shoot = ProjectileID.Anchor;
            }
            else
            {
                item.useTime = 35;
                item.useAnimation = 35;
                item.shoot = ProjectileID.None;
            }
            return base.CanUseItem(player);
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            float numberProjectiles = 3;
            float rotation = MathHelper.ToRadians(10);
            position += Vector2.Normalize(new Vector2(speedX, speedY)) * 25f;
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
            if (player.altFunctionUse != 2)
                Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 131);
        }
    }
}