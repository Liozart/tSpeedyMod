using GiuxItems.Items.Placeables;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static GiuxItems.NPCs.Critters.Speedy;
using static Terraria.ModLoader.ModContent;

namespace GiuxItems.NPCs.Town
{
    // [AutoloadHead] and npc.townNPC are extremely important and absolutely both necessary for any Town NPC to work at all.
    [AutoloadHead]
    public class Giux : ModNPC
    {
        //public override string Texture => "Giux";

        public override bool Autoload(ref string name)
        {
            name = "Giux";
            return mod.Properties.Autoload;
        }

        public override void SetStaticDefaults()
        {
            // DisplayName automatically assigned from .lang files, but the commented line below is the normal approach.
            // DisplayName.SetDefault("Example Person");
            Main.npcFrameCount[npc.type] = 26;
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
                Dust.NewDust(npc.position, npc.width, npc.height, DustID.Electric);
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
                    if (item.type == ItemType<SpeedyItem>())
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public override string TownNPCName()
        {
            return "Giux";
        }

        public override string GetChat()
        {
            int partyGirl = NPC.FindFirstNPC(NPCID.Guide);
            switch (Main.rand.Next(4))
            {
                case 0:
                    Main.npcChatCornerItem = ItemType<BathriteBar>();
                    return $"La bathrite [i:{ItemType<Bathrite>()}] se trouve au bout de l'enfer, dans les deux sens."; ;
                case 1:
                    return "BAAAAAAATTTTHR";
                case 2:
                    {
                        // Main.npcChatCornerItem shows a single item in the corner, like the Angler Quest chat.
                        Main.npcChatCornerItem = ItemType<SpeedyItem>();
                        return $"Hey, si tu trouve un [i:{ItemType<SpeedyItem>()}], tu peux l'utiliser pour craft";
                    }
                default:
                    return "Coucou";
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
            shop.item[nextSlot].SetDefaults(ItemID.GoldButterfly);
            nextSlot++;
            shop.item[nextSlot].SetDefaults(ItemID.GoldGrasshopper);
            nextSlot++;
            shop.item[nextSlot].SetDefaults(ItemID.GoldWorm);
            nextSlot++;
            shop.item[nextSlot].SetDefaults(ItemID.Buggy);
            nextSlot++;
            shop.item[nextSlot].SetDefaults(ItemID.TreeNymphButterfly);
            nextSlot++;
            shop.item[nextSlot].SetDefaults(ItemID.LightningBug);
            nextSlot++;
            shop.item[nextSlot].SetDefaults(ItemID.Worm);
            nextSlot++;
            nextSlot++;
            nextSlot++;
            nextSlot++;
            shop.item[nextSlot].SetDefaults(ItemID.WormholePotion);
            nextSlot++;
            shop.item[nextSlot].SetDefaults(ItemID.GreaterHealingPotion);
            nextSlot++;
            shop.item[nextSlot].SetDefaults(ItemID.SpelunkerPotion);
            nextSlot++;
            shop.item[nextSlot].SetDefaults(ItemID.SpelunkerGlowstick);
            nextSlot++;
            nextSlot++;
            shop.item[nextSlot].SetDefaults(ItemID.Beenade);
            nextSlot++;
            shop.item[nextSlot].SetDefaults(ItemID.Ale);
            nextSlot++;
            shop.item[nextSlot].SetDefaults(ItemID.AleThrowingGlove);
        }

        public override void NPCLoot()
        {
            Item.NewItem(npc.getRect(), ItemID.Toxikarp);
        }

        public override void TownNPCAttackStrength(ref int damage, ref float knockback)
        {
            damage = 42;
            knockback = 8f;
        }

        public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown)
        {
            cooldown = 30;
            randExtraCooldown = 30;
        }

        public override void TownNPCAttackProj(ref int projType, ref int attackDelay)
        {
            projType = ProjectileID.FlyingKnife;
            attackDelay = 20;
        }

        public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
        {
            multiplier = 12f;
            randomOffset = 2f;
        }
    }
}
