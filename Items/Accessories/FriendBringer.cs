using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using static GiuxItems.NPCs.Speedy;
using Terraria.Utilities;
using GiuxItems.Items.Placeables;

namespace GiuxItems.Items.Accessories
{
    class FriendBringer : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Friend Seeker");
            Tooltip.SetDefault("Right click while in the inventory to teleport to a friend !");
        }

        public override void SetDefaults()
        {
            item.width = 40;
            item.height = 40;
            item.accessory = true;
            item.value = Item.sellPrice(silver: 42);
            item.rare = ItemRarityID.Green;
        }

        public override bool CanRightClick()
        {
            return true;
        }

        public override void RightClick(Player player)
        {
            //Teleport to another player
            for(int i = 0; i < 255; i++)
                if (Main.player[i].active)
                {
                    player.Teleport(Main.player[i].position);
                    break;
                }
        }

        public override bool ConsumeItem(Player player)
        {
            return false;
        }

        public override int ChoosePrefix(UnifiedRandom rand)
        {
            // When the item is given a prefix, only roll the best modifiers for accessories
            return rand.Next(new int[] { PrefixID.Arcane, PrefixID.Lucky, PrefixID.Menacing, PrefixID.Quick, PrefixID.Violent, PrefixID.Warding });
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.LifeCrystal, 2);
            recipe.AddIngredient(ItemID.ManaCrystal, 2);
            recipe.AddIngredient(ItemType<BathriteBar>(), 10);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
