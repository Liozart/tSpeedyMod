using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace GiuxItems.Buffs
{
	public class Telesafe : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Telesafe");
			Description.SetDefault("Teleports you home on low life, for 5 gold coins\n 2 minute cooldown");
			Main.persistentBuff[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
			canBeCleared = false;
		}

		bool canDO = true;
		int timer = 7200, cnt = 0;
		public override void Update(Player player, ref int buffIndex)
		{
			if (canDO)
			{
				if (player.statLife < 20 && !player.dead)
				{
					//Check money
					int gold = 0;
					for (int i = 0; i <= 53; i++)
					{
						if (player.inventory[i].type == ItemID.GoldCoin)
							gold += player.inventory[i].stack;
					}
					if (gold >= 5)
					{
						player.Teleport(new Vector2(Main.spawnTileX * 16, (Main.spawnTileY - 3) * 16));
						canDO = false;
						//Remove 5 golds
						int rmgold = 5;
						for (int i = 0; i <= 53; i++)
						{
							if (player.inventory[i].type == ItemID.GoldCoin)
								for (int j = 0; j < player.inventory[i].stack; j++)
								{
									if (rmgold == 0)
										break;
									player.inventory[i].stack--;
									rmgold--;
								}
						}
					}
				}
			}
			else
            {
				cnt++;
				if (cnt >= timer)
                {
					cnt = 0;
					canDO = true;
						Main.NewText("OKOK");
                }
            }
		}
	}
}
