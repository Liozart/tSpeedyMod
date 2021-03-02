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
            projectile.width = 12;
            projectile.height = 18;
            projectile.aiStyle = 1;
            aiType = 1;
            projectile.ranged = true;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.damage = 24;
            projectile.ai[0] = 0.01f;
        }
        
        public override void AI()
        {
            if (Main.rand.Next(0, 3) == 0)
                Dust.NewDust(projectile.position, projectile.width, projectile.height, Main.rand.Next(46, 50));
            base.AI();
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.OnFire, 180);
            Dust.NewDust(target.position, projectile.width, projectile.height, 38);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Dust.NewDust(projectile.position, projectile.width, projectile.height, 38);
            return true;
        }

        public override void Kill(int timeLeft)
        {
            Dust.NewDust(projectile.position, projectile.width, projectile.height, 50);
            base.Kill(timeLeft);
        }
    }
}
