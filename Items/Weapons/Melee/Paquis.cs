using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using static GiuxItems.NPCs.Hostiles.BadSpeedy;
using GiuxItems.Items.Placeables;

namespace GiuxItems.Items.Weapons.Melee
{
	public class Paquis : ModItem
	{
		public override void SetStaticDefaults() 
		{
            Tooltip.SetDefault("Meanwhile aux Paquis..");
        }

		public override void SetDefaults() 
		{
			item.damage = 20;
			item.melee = true;
			item.width = 44;
			item.height = 46;
			item.useTime = 10;
			item.useAnimation = 10;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.knockBack = 2;
            item.crit *= 5;
			item.value = Item.buyPrice(gold: 10);
			item.rare = ItemRarityID.LightRed;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;
		}

        public override bool CanUseItem(Player player)
        {
            if (altie)
            {
                altie = false;
                item.useStyle = ItemUseStyleID.SwingThrow;
            }
            else
            {
                altie = true;
                item.useStyle = ItemUseStyleID.Stabbing;
            }
            return true;
        }

        bool altie = false;
        public override bool UseItem(Player player)
        {
            
            return true;
        }

        public override void AddRecipes() 
		{
			ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.GoldCoin, 10);
            recipe.AddIngredient(ItemType<BathrGeode>(), 1);
            recipe.AddIngredient(ItemType<BadSpeedyItem>(), 420);
            recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

        //Adds defense buffs when hitting
        public override void OnHitNPC(Player player, NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.ShadowFlame, 120);
            Dust.NewDust(target.position, target.width, target.height, 28);
        }
    }
}