using GiuxItems.Buffs;
using GiuxItems.NPCs.Critters;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace GiuxItems.Projectiles
{
    class Froggy : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Froggy");
            Main.projFrames[projectile.type] = 3;
        }

        int spriteYOffset = 0, nmbSpritesInSheet = 3;
        public override void SetDefaults()
        {
            projectile.width = 20;
            projectile.height = 20;
            projectile.aiStyle = 1;
            aiType = 1;
            projectile.ranged = true;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.damage = 25;
            spriteYOffset = Main.rand.Next(0, nmbSpritesInSheet);
        }

        public override void AI()
        {

            // Loop through the 4 animation frames, spending 5 ticks on each.
            if (++projectile.frameCounter >= 3)
            {
                projectile.frameCounter = 0;
                if (++projectile.frame >= 3)
                {
                    projectile.frame = 0;
                }
            }

            projectile.direction = projectile.spriteDirection = projectile.velocity.X > 0f ? 1 : -1;
            projectile.rotation = projectile.velocity.ToRotation();
            if (projectile.velocity.Y > 16f)
            {
                projectile.velocity.Y = 16f;
            }
            // Since our sprite has an orientation, we need to adjust rotation to compensate for the draw flipping.
            if (projectile.spriteDirection == -1)
            {
                projectile.rotation += MathHelper.Pi;
            }
            //base.AI();
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            int critt = Main.rand.Next(0, 3);
            if (critt == 0)
                critt = NPCID.Frog;
            if (critt == 1)
                critt = ModContent.NPCType<Froggy1>();
            if (critt == 2)
                critt = ModContent.NPCType<Froggy2>();
            NPC.NewNPC((int)projectile.position.X, (int)projectile.position.Y + 16, critt);
            return base.OnTileCollide(oldVelocity);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.Slow, 1200);
            Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Blood);
        }

        // Some advanced drawing because the texture image isn't centered or symetrical.
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            SpriteEffects spriteEffects = SpriteEffects.None;
            if (projectile.spriteDirection == 1)
            {
                spriteEffects = SpriteEffects.FlipHorizontally;
            }
            Texture2D texture = Main.projectileTexture[projectile.type];
            int frameHeight = Main.projectileTexture[projectile.type].Height / Main.projFrames[projectile.type] / nmbSpritesInSheet;
            int startY = frameHeight * projectile.frame + (frameHeight * 3 * spriteYOffset);
            Rectangle sourceRectangle = new Rectangle(0, startY, texture.Width, frameHeight);
            Vector2 origin = sourceRectangle.Size() / 2f;
            origin.X = (float)(projectile.spriteDirection == 1 ? sourceRectangle.Width - 20 : 20);

            Color drawColor = projectile.GetAlpha(lightColor);
            Main.spriteBatch.Draw(texture,
                projectile.Center - Main.screenPosition + new Vector2(0f, projectile.gfxOffY),
                sourceRectangle, drawColor, projectile.rotation, origin, projectile.scale, spriteEffects, 0f);

            return false;
        }
    }
}
