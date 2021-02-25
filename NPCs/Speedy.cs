using Microsoft.Xna.Framework;
using MonoMod.Cil;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace GiuxItems.NPCs
{
    public class Speedy : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Speedy");
            Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.Bunny];
            Main.npcCatchable[npc.type] = true;
        }

        public override void SetDefaults()
        {
            //npc.width = 14;
            //npc.height = 14;
            //npc.aiStyle = 67;
            //npc.damage = 0;
            //npc.defense = 0;
            //npc.lifeMax = 5;
            //npc.HitSound = SoundID.NPCHit1;
            //npc.DeathSound = SoundID.NPCDeath1;
            //npc.npcSlots = 0.5f;
            //npc.noGravity = true;
            //npc.catchItem = 2007;

            npc.CloneDefaults(NPCID.Bunny);
            npc.scale *= 0.3f;
            npc.catchItem = (short)ItemType<SpeedyItem>();
            npc.lavaImmune = false;
            //npc.aiStyle = 0;
            npc.friendly = true; // We have to add this and CanBeHitByItem/CanBeHitByProjectile because of reasons.
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
            return SpawnCondition.OverworldDay.Chance * 0.1f;
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

        public override void OnCatchNPC(Player player, Item item)
        {
            item.stack = 1;

            try
            {
                var npcCenter = npc.Center.ToTileCoordinates();
                if (!WorldGen.SolidTile(npcCenter.X, npcCenter.Y) && Main.tile[npcCenter.X, npcCenter.Y].liquid == 0)
                {
                    Main.tile[npcCenter.X, npcCenter.Y].liquid = (byte)Main.rand.Next(50, 150);
                    Main.tile[npcCenter.X, npcCenter.Y].lava(false);
                    Main.tile[npcCenter.X, npcCenter.Y].honey(true);
                    WorldGen.SquareTileFrame(npcCenter.X, npcCenter.Y, true);
                }
            }
            catch
            {
                return;
            }
        }

        internal class SpeedyItem : ModItem
        {
            public override void SetStaticDefaults()
            {
                DisplayName.SetDefault("Gentil Speedy");
            }

            public override void SetDefaults()
            {
                //item.useStyle = 1;
                //item.autoReuse = true;
                //item.useTurn = true;
                //item.useAnimation = 15;
                //item.useTime = 10;
                //item.maxStack = 999;
                //item.consumable = true;
                //item.width = 12;
                //item.height = 12;
                //item.makeNPC = 360;
                //item.noUseGraphic = true;
                //item.bait = 15;

                item.CloneDefaults(ItemID.Bunny);
                item.consumable = false;
                item.material = true;
                item.value = Item.gold * 3;
                item.bait = 85;
                item.makeNPC = (short)NPCType<Speedy>();
            }
        }
    }
}
