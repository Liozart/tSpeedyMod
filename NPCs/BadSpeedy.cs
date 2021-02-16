using Microsoft.Xna.Framework;
using MonoMod.Cil;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace GiuxItems.NPCs
{
    public class BadSpeedy : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Méchant Speedy");
            Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.Bunny];
            Main.npcCatchable[npc.type] = false;
        }

        public override void SetDefaults()
        {
            npc.CloneDefaults(NPCID.Bunny);
            npc.width = 100;
            npc.height = 100;
            //npc.aiStyle = 67;
            npc.damage = 40;
            npc.defense = 7;
            npc.lifeMax = 420;
            //npc.HitSound = SoundID.NPCHit1;
            //npc.DeathSound = SoundID.NPCDeath1;
            //npc.npcSlots = 0.5f;
            //npc.noGravity = true;
            //npc.catchItem = 2007;
            npc.lavaImmune = true;
            //npc.aiStyle = NPCID.Bunny;
            npc.friendly = false; // We have to add this and CanBeHitByItem/CanBeHitByProjectile because of reasons.
            aiType = NPCID.Bunny;
            animationType = NPCID.Bunny;
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
            return SpawnCondition.Cavern.Chance * 0.1f;
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
                Gore.NewGore(npc.position, npc.velocity, Main.rand.Next(0, GoreID.Count - 1), npc.scale);
            }
        }

        public override void NPCLoot()
        {
            if (Main.rand.Next(3) > 1)
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<BadSpeedyItem>(), 420);
        }

        internal class BadSpeedyItem : ModItem
        {
            public override void SetStaticDefaults()
            {
                DisplayName.SetDefault("Bathr");
                Tooltip.SetDefault("Finally some bathr...");
            }

            public override void SetDefaults()
            {
                item.width = 40;
                item.height = 42;
                item.consumable = false;
                item.material = true;
                item.maxStack = 420;
                item.rare = ItemRarityID.Green;
                item.material = true;
                item.value = Item.gold * 3;
            }
        }
    }
}
