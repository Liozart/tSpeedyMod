using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using static GiuxItems.NPCs.BadSpeedy;

namespace GiuxItems.Items.Weapons
{
    public class FornicatorV2 : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("The Fornicator V2 is definitely the fastest");
        }

        public override void SetDefaults()
        {
            item.damage = 67;
            item.ranged = true;
            item.width = 40;
            item.height = 20;
            item.useTime = 3;
            item.useAnimation = 3;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.noMelee = true;
            item.knockBack = 4;
            item.value = Item.buyPrice(gold: 15);
            item.rare = ItemRarityID.Green;
            item.UseSound = SoundID.Item11;
            item.autoReuse = true;
            item.shootSpeed = 16f;
            item.shoot = ProjectileID.CrystalBullet;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.BreakerBlade, 1);
            recipe.AddIngredient(ItemID.SoulofFright, 10);
            recipe.AddIngredient(ItemType<BadSpeedyItem>(), 420);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
