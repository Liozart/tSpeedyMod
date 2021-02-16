using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using static GiuxItems.NPCs.Speedy;
using Terraria.Utilities;
using static GiuxItems.NPCs.BadSpeedy;

namespace GiuxItems.Items.Accessories
{
    class OCB : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("OCBs");
            Tooltip.SetDefault("You feel floaty");
        }

        public override void SetDefaults()
        {
            item.width = 40;
            item.height = 40;
            item.accessory = true;
            item.value = Item.sellPrice(copper: 3);
            item.rare = ItemRarityID.Green;
        }

        public override void UpdateEquip(Player player)
        {
            player.slowFall = true;
            player.noFallDmg = true;
            base.UpdateEquip(player);
        }

        public override int ChoosePrefix(UnifiedRandom rand)
        {
            // When the item is given a prefix, only roll the best modifiers for accessories
            return rand.Next(new int[] { PrefixID.Arcane, PrefixID.Lucky, PrefixID.Menacing, PrefixID.Quick, PrefixID.Violent, PrefixID.Warding });
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.ClimbingClaws, 1);
            recipe.AddIngredient(ItemID.SoulofNight, 3);
            recipe.AddIngredient(ItemType<BadSpeedyItem>(), 420);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
