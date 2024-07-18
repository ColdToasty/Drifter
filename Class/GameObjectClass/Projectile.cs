
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Drifter.Class.Tools;
using Drifter.Class.AbstractClass;


namespace Drifter.Class.GameObjectClass
{
    internal class Projectile : GameObject
    {

        public enum ProjectileType
        {
            Missle, Laser, LaserBeam
        }

        private ProjectileType projectileType;

        public ProjectileType TypeOfProjectile { get { return projectileType; } }


        public Projectile(Texture2D texture, Vector2 startPosition, ProjectileType projectileType = ProjectileType.Missle)
        {
            this.travelSpeed = 200;
            this.projectileType = projectileType;
            ObjectTexture = texture;

            this.Position = startPosition;
            this.Position.X += texture.Width / 2;

            this.collisionCircle = new CollisionCircle(this.Position + new Vector2(8, -4), 8);
        }

        public override void Run(GameTime gameTime, bool isMovingNegative)
        {
            this.Position.Y -= this.travelSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            UpdateCollisionCircle();
        }


        protected override void UpdateCollisionCircle()
        {
            this.collisionCircle.Centre = this.Position + new Vector2(8, 8);
        }
    }
}
