﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace GiuxItems.NPCs.Critters
{
    public class Froggy2 : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("White Froggy");
            Main.npcFrameCount[npc.type] = 13;
            Main.npcCatchable[npc.type] = true;
        }

        public override void SetDefaults()
        {
            npc.CloneDefaults(NPCID.Frog);
            npc.catchItem = (short)ItemType<Froggy2Item>();
            npc.lavaImmune = false;
            npc.friendly = true; // We have to add this and CanBeHitByItem/CanBeHitByProjectile because of reasons.
            aiType = NPCID.Frog;
            animationType = NPCID.Frog;
        }

        public override bool? CanBeHitByItem(Player player, Item item)
        {
            return true;
        }

        public override bool? CanBeHitByProjectile(Projectile projectile)
        {
            return true;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (Main.raining)
                return SpawnCondition.OverworldDay.Chance * 0.1f;
            else return 0f;
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (npc.life <= 0)
            {
                for (int i = 0; i < 6; i++)
                {
                    int dust = Dust.NewDust(npc.position, npc.width, npc.height, 200, 2 * hitDirection, -2f);
                    if (Main.rand.NextBool(2))
                    {
                        Main.dust[dust].noGravity = true;
                        Main.dust[dust].scale = 1.2f * npc.scale;
                    }
                    else
                    {
                        Main.dust[dust].scale = 0.7f * npc.scale;
                    }
                }
            }
        }

        internal class Froggy2Item : ModItem
        {
            public override void SetStaticDefaults()
            {
                DisplayName.SetDefault("Lil' white froggy");
            }

            public override void SetDefaults()
            {
                item.CloneDefaults(ItemID.Frog);
                item.consumable = false;
                item.material = true;
                item.value = Item.silver * 1;
                item.bait = 42;
                item.makeNPC = (short)NPCType<Froggy2>();
            }
        }
    }
}
