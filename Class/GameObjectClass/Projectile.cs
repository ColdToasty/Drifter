
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
            Missle, Laser, LaserBeam, EnemyProjectile
        }

        private ProjectileType projectileType;

        public ProjectileType TypeOfProjectile { get { return projectileType; } }

        public bool IsMovingNegative = false;

        public Projectile(Texture2D texture, Vector2 startPosition,bool isMovingNegative, ProjectileType projectileType = ProjectileType.Missle)
        {
            this.travelSpeed = 200;
            this.projectileType = projectileType;
            ObjectTexture = texture;

            this.Position = startPosition;
            this.Position.X += texture.Width / 2;
            this.IsMovingNegative = isMovingNegative;
            this.collisionCircle = new CollisionCircle(this.Position + new Vector2(8, -4), 8);
        }

        //moving negative means going up
        public override void Run(GameTime gameTime, bool isMovingNegative)
        {
            if (isMovingNegative)
            {
                this.Position.Y -= this.travelSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                
            }
            else
            {
                this.Position.Y += this.travelSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            UpdateCollisionCircle();
        }


        protected override void UpdateCollisionCircle()
        {
            if (IsMovingNegative)
            {
                this.collisionCircle.Centre = this.Position + new Vector2(8, 8);
            }
            else
            {
                this.collisionCircle.Centre = this.Position - new Vector2(8, 8);
            }


        }
    }
}
