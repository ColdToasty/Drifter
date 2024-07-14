using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceRider.Class.Factory
{
    internal class ProjectileSpawner
    {
        public Projectile CreateProjectile(Texture2D projectileTexture, Vector2 startPosition,  Projectile.ProjectileType projectileType = Projectile.ProjectileType.Missle)
        {
            return new Projectile(projectileTexture, startPosition, projectileType);
        }
    }
}
