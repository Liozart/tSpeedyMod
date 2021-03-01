using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace GiuxItems.Projectiles
{
    class Speedarter : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 18;
            projectile.height = 44;
            projectile.aiStyle = 1;
            aiType = 1;
            projectile.ranged = true;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.damage = 24;
            projectile.ai[0] = 0.01f;
        }

       /* float offset = MathHelper.ToRadians(45f);*/
        public override void AI()
        {
            /* projectile.direction = projectile.spriteDirection = projectile.velocity.X > 0f ? 1 : -1;
             projectile.rotation = projectile.velocity.ToRotation() - (offset * projectile.direction);
             if (projectile.velocity.Y > 16f)
             {
                 projectile.velocity.Y = 16f;
             }
             // Since our sprite has an orientation, we need to adjust rotation to compensate for the draw flipping.
             if (projectile.spriteDirection == -1)
             {
                 projectile.rotation += MathHelper.Pi;
             }
             if (Main.rand.Next(0, 10) == 0)
                 Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.AmberBolt);
             if (isColliding)
                 projectile.ai[1] += 0.01f;
             if (projectile.ai[1] >= 0.99f)
                 projectile.Kill();*/

            Dust.NewDust(projectile.position, projectile.width, projectile.height, Main.rand.Next(46, 50));
            base.AI();
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Dust.NewDust(target.position, projectile.width, projectile.height, 38);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Dust.NewDust(projectile.position, projectile.width, projectile.height, 38);
            return true;
        }
    }
}
