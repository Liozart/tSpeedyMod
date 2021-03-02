using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using GiuxItems.Projectiles;

namespace GiuxItems.Buffs
{
	public class ButterflyBarrier : ModBuff
	{

		public override void SetDefaults()
		{
			DisplayName.SetDefault("Butterfly Barrier");
			Description.SetDefault("Butterlies fights for you !");
			Main.persistentBuff[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
			canBeCleared = true;
		}

		int timer = 60, cnt = 0;
		public override void Update(Player player, ref int buffIndex)
		{
			if (player.GetModPlayer<GiuxPlayer>().butterFliesCnt < 5 && cnt >= timer)
            {
				Projectile.NewProjectile(player.position, new Vector2(Main.rand.Next(-1, 2), -1), ModContent.ProjectileType<ButterflyGardian>(), 42, 5, player.whoAmI);
				player.GetModPlayer<GiuxPlayer>().butterFliesCnt++;
				cnt = 0;
			}
			cnt++;
			if (cnt >= timer)
				cnt = timer;
		}
	}
}
