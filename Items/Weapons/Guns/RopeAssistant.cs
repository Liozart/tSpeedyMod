using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using static GiuxItems.NPCs.Critters.Speedy;
using GiuxItems.Items.Placeables;

namespace GiuxItems.Items.Weapons.Guns
{
	public class RopeAssistant : ModItem
	{
		public override void SetStaticDefaults() {
			Tooltip.SetDefault("Won't let you in a hole");
		}

		public override void SetDefaults() {
			item.damage = 25;
			item.ranged = true;
			item.width = 54;
			item.height = 28;
			item.useTime = 50;
			item.useAnimation = 50;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.noMelee = true; //so the item's animation doesn't do damage
			item.knockBack = 3;
			item.value = Item.buyPrice(copper: 42);
			item.rare = ItemRarityID.Orange;
			item.UseSound = SoundID.Item11;
			item.autoReuse = true;
            item.shoot = ProjectileID.RopeCoil;
			item.shootSpeed = 16f;
        }

		public override void AddRecipes() {
			ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Rope, 42);
            recipe.AddIngredient(ItemType<GiuxBar>(), 3);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
			recipe.AddRecipe();
		}

        // Randomly shoot something
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            type = (int)ProjectileID.RopeCoil;
            switch (Main.rand.Next(0, 4))
            {
                case 0: type = ProjectileID.RopeCoil; break;
                case 1: type = ProjectileID.SilkRopeCoil; break;
                case 2: type = ProjectileID.VineRopeCoil; break;
                case 3: type = ProjectileID.WebRopeCoil; break;
            }
            Projectile.NewProjectile(position, new Vector2(speedX, speedY), type, 10, 1, player.whoAmI);
            return true;
        }
    }
}
