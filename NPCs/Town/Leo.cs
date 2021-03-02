using GiuxItems.Items.Placeables;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static GiuxItems.NPCs.Hostiles.BadSpeedy;
using static Terraria.ModLoader.ModContent;

namespace GiuxItems.NPCs.Town
{
    // [AutoloadHead] and npc.townNPC are extremely important and absolutely both necessary for any Town NPC to work at all.
    [AutoloadHead]
    public class Leo : ModNPC
    {
        //public override string Texture => "Giux";

        public override bool Autoload(ref string name)
        {
            name = "Leo";
            return mod.Properties.Autoload;
        }

        public override void SetStaticDefaults()
        {
            // DisplayName automatically assigned from .lang files, but the commented line below is the normal approach.
            // DisplayName.SetDefault("Example Person");
            Main.npcFrameCount[npc.type] = 25;
            NPCID.Sets.ExtraFramesCount[npc.type] = 9;
            NPCID.Sets.AttackFrameCount[npc.type] = 4;
            NPCID.Sets.DangerDetectRange[npc.type] = 700;
            NPCID.Sets.AttackType[npc.type] = 0;
            NPCID.Sets.AttackTime[npc.type] = 90;
            NPCID.Sets.AttackAverageChance[npc.type] = 30;
            NPCID.Sets.HatOffsetY[npc.type] = 4;
        }

        public override void SetDefaults()
        {
            npc.townNPC = true;
            npc.friendly = true;
            npc.width = 18;
            npc.height = 40;
            npc.aiStyle = 7;
            npc.damage = 42;
            npc.defense = 42;
            npc.lifeMax = 420;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.knockBackResist = 0.5f;
            animationType = NPCID.Guide;
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            int num = npc.life > 0 ? 2 : 5;
            for (int k = 0; k < num; k++)
            {
                Dust.NewDust(npc.position, npc.width, npc.height, DustID.PurpleCrystalShard);
            }
        }

        public override bool CanTownNPCSpawn(int numTownNPCs, int money)
        {
            for (int k = 0; k < 255; k++)
            {
                Player player = Main.player[k];
                if (!player.active)
                {
                    continue;
                }
                foreach (Item item in player.inventory)
                {
                    if (item.type == ItemType<BadSpeedyItem>())
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public override string TownNPCName()
        {
            return "Léo";
        }

        public override string GetChat()
        {
            switch (Main.rand.Next(4))
            {
                case 0:
                    return "Out of bathr";
                case 1:
                    Main.npcChatCornerItem = ItemType<BathriteBar>();
                    return $"La bathrite [i:{ItemType<Bathrite>()}] se trouve au fin fond de l'enfer, gl hf"; ;
                case 2:
                    {
                        // Main.npcChatCornerItem shows a single item in the corner, like the Angler Quest chat.
                        Main.npcChatCornerItem = ItemType<BadSpeedyItem>();
                        return $"Hey, si tu trouve de la [i:{ItemType<BadSpeedyItem>()}], tu peux l'utiliser pour craft";
                    }
                case 3:
                    return "Kill bosses and i'll sell bags";
                default:
                    return "Yo yo";
            }
        }

        public override void SetChatButtons(ref string button, ref string button2)
        {
            button = "Shop";
            button2 = "";
        }

        public override void OnChatButtonClicked(bool firstButton, ref bool shop)
        {
            if (firstButton)
                shop = true;
        }

        public override void SetupShop(Chest shop, ref int nextSlot)
        {
            shop.item[nextSlot].SetDefaults(ItemID.RodofDiscord);
            nextSlot++;
            shop.item[nextSlot].SetDefaults(ItemID.CellPhone);
            nextSlot++;
            nextSlot++;
            if (NPC.downedBoss1)
            {
                shop.item[nextSlot].SetDefaults(ItemID.KingSlimeBossBag);
                shop.item[nextSlot].shopCustomPrice = Item.buyPrice(gold: 3);
                nextSlot++;
                shop.item[nextSlot].SetDefaults(ItemID.EaterOfWorldsBossBag);
                shop.item[nextSlot].shopCustomPrice = Item.buyPrice(gold: 5);
                nextSlot++;
                shop.item[nextSlot].SetDefaults(ItemID.EyeOfCthulhuBossBag);
                shop.item[nextSlot].shopCustomPrice = Item.buyPrice(gold: 5);
                nextSlot++;
                shop.item[nextSlot].SetDefaults(ItemID.BrainOfCthulhuBossBag);
                shop.item[nextSlot].shopCustomPrice = Item.buyPrice(gold: 5);
                nextSlot++;
                shop.item[nextSlot].SetDefaults(ItemID.QueenBeeBossBag);
                shop.item[nextSlot].shopCustomPrice = Item.buyPrice(gold: 10);
                nextSlot++;
                shop.item[nextSlot].SetDefaults(ItemID.SkeletronBossBag);
                shop.item[nextSlot].shopCustomPrice = Item.buyPrice(gold: 12);
                nextSlot++;
            }
            if (Main.hardMode)
            {
                shop.item[nextSlot].SetDefaults(ItemID.WallOfFleshBossBag);
                shop.item[nextSlot].shopCustomPrice = Item.buyPrice(gold: 20);
                nextSlot++;
            }
            if (NPC.downedMechBossAny)
            {
                shop.item[nextSlot].SetDefaults(ItemID.TwinsBossBag);
                shop.item[nextSlot].shopCustomPrice = Item.buyPrice(gold: 25);
                nextSlot++;
                shop.item[nextSlot].SetDefaults(ItemID.SkeletronPrimeBossBag);
                shop.item[nextSlot].shopCustomPrice = Item.buyPrice(gold: 25);
                nextSlot++;
                shop.item[nextSlot].SetDefaults(ItemID.DestroyerBossBag);
                shop.item[nextSlot].shopCustomPrice = Item.buyPrice(gold: 25);
                nextSlot++;
            }
            if (NPC.downedPlantBoss)
            {
                shop.item[nextSlot].SetDefaults(ItemID.PlanteraBossBag);
                shop.item[nextSlot].shopCustomPrice = Item.buyPrice(gold: 42);
                nextSlot++;
            }
            if (NPC.downedGolemBoss)
            {
                shop.item[nextSlot].SetDefaults(ItemID.GolemBossBag);
                shop.item[nextSlot].shopCustomPrice = Item.buyPrice(gold: 42);
                nextSlot++;
            }
            if (NPC.downedAncientCultist)
            {
                shop.item[nextSlot].SetDefaults(ItemID.BossBagDarkMage);
                shop.item[nextSlot].shopCustomPrice = Item.buyPrice(gold: 10);
                nextSlot++;
                shop.item[nextSlot].SetDefaults(ItemID.FishronBossBag);
                shop.item[nextSlot].shopCustomPrice = Item.buyPrice(gold: 42);
                nextSlot++;
            }
            if (NPC.downedTowers)
            {
                shop.item[nextSlot].SetDefaults(ItemID.BossBagBetsy);
                shop.item[nextSlot].shopCustomPrice = Item.buyPrice(gold: 40);
                nextSlot++;
                shop.item[nextSlot].SetDefaults(ItemID.BossBagOgre);
                shop.item[nextSlot].shopCustomPrice = Item.buyPrice(gold: 30);
                nextSlot++;
            }
            if (NPC.downedMoonlord)
            {
                shop.item[nextSlot].SetDefaults(ItemID.MoonLordBossBag);
                shop.item[nextSlot].shopCustomPrice = Item.buyPrice(gold: 50);
                nextSlot++;
            }
        }

        public override void NPCLoot()
        {
            Item.NewItem(npc.getRect(), ItemID.LeafBlower);
        }

        public override void TownNPCAttackStrength(ref int damage, ref float knockback)
        {
            damage = 4;
            knockback = 8f;
        }

        public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown)
        {
            cooldown = 5;
            randExtraCooldown = 30;
        }

        public override void TownNPCAttackProj(ref int projType, ref int attackDelay)
        {
            projType = ProjectileID.VampireKnife;
            attackDelay = 10;
        }

        public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
        {
            multiplier = 12f;
            randomOffset = 2f;
        }
    }
}
