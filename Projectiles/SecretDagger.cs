using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace GiuxItems.Projectiles
{
    class SecretDagger : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("SecretDagger");
        }

        public override void SetDefaults()
        {
            projectile.width = 30;
            projectile.height = 30;
            projectile.aiStyle = 23;
            aiType = 23;
            projectile.ranged = true;
            drawOffsetX = -25;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.damage = 50;

        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.LunarOre);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.LunarOre);
            return true;
        }
    }
}
