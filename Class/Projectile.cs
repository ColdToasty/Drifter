using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Drifter.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drifter.Class
{
    internal class Projectile : GameObject
    {

        public enum ProjectileType
        {
            Missle, Laser, LaserBeam
        }

        private ProjectileType projectileType;

        public ProjectileType TypeOfProjectile { get { return projectileType;} }

        public Texture2D Texture { get { return ObjectTexture; } }
       
        
        
        public Projectile(Texture2D texture, Vector2 startPosition, ProjectileType projectileType = ProjectileType.Missle)
        {
            this.travelSpeed = 200;
            this.projectileType = projectileType;
            ObjectTexture = texture;

            this.Position = startPosition;
            this.Position.X += texture.Width / 2;
        }

        public override void Run(GameTime gameTime, bool isMovingNegative)
        {
            this.Position.Y -= this.travelSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
    }
}
