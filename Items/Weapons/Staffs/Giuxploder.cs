using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using static GiuxItems.NPCs.Speedy;
using GiuxItems.Items.Placeables;

namespace GiuxItems.Items.Weapons.Staffs
{
	public class Giuxploder : ModItem
	{
		public override void SetStaticDefaults() {
			Tooltip.SetDefault("Simply explode");
			Item.staff[item.type] = true; //this makes the useStyle animate as a staff instead of as a gun
		}

		public override void SetDefaults() {
			item.damage = 30;
			item.magic = true;
			item.mana = 10;
			item.width = 44;
			item.height = 46;
			item.useTime = 55;
			item.useAnimation = 55;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.noMelee = true; //so the item's animation doesn't do damage
			item.knockBack = 5;
            item.value = Item.buyPrice(gold: 10);
            item.rare = ItemRarityID.LightPurple;
			item.UseSound = SoundID.Item19;
			item.autoReuse = true;
            item.shoot = ProjectileID.SeedlerNut;
            item.shootSpeed = 25f;
		}

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            float numberProjectiles = 4;
            float rotation = MathHelper.ToRadians(45);
            position += Vector2.Normalize(new Vector2(speedX, speedY)) * 30f;
            for (int i = 0; i < numberProjectiles; i++)
            {
                Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1))); // Watch out for dividing by 0 if there is only 1 projectile.
                Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
            }
            return false;
        }

        public override void AddRecipes() {
			ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.IceTorch, 5);
            recipe.AddIngredient(ItemID.Bunny, 3);
			recipe.AddIngredient(ItemType<BathrGeode>(), 1);
			recipe.AddIngredient(ItemType<SpeedyItem>(), 1);
            recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}