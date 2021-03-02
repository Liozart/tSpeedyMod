using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using GiuxItems.Projectiles;
using GiuxItems.Items.Placeables;
using static GiuxItems.NPCs.Critters.Speedy;

namespace GiuxItems.Items.Weapons.Bows
{
    public class Speedart : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Basically an infinite bow");
        }

        public override void SetDefaults()
        {
            item.damage = 24;
            item.ranged = true;
            item.width = 38;
            item.height = 68;
            item.useTime = 16;
            item.useAnimation = 16;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.noMelee = true;
            item.knockBack = 4;
            item.value = Item.buyPrice(gold: 10);
            item.rare = ItemRarityID.Pink;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
            item.shootSpeed = 18f;
            item.shoot = ProjectileType<Speedarter>();
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<GiuxBar>(), 8);
            recipe.AddIngredient(ItemType<SpeedyItem>(), 8);
            recipe.AddIngredient(ItemID.WhiteString, 1);
            recipe.AddIngredient(ItemID.Hook, 1);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
