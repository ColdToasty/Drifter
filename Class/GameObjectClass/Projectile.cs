
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Drifter.Class.Tools;
using Drifter.Class.AbstractClass;
using Drifter.Class.Factory;
using Drifter.Class.Tools.CollisionShapes;

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


        public bool IsMovingNegative = true;

        private Vector2 enemyProjectileCollisionCirclePlacement = new Vector2(8, 8);
        private Vector2 playerProjectileCollisionCirclePlacement = new Vector2(8, -4);

        private int enemyProjectileCollisionCircleRadius = 16;
        private int playerProjectileCollisionCircleRadius = 8;

        private int projectileDamage { get; init; }
        public int ProjectileDamge { get { return projectileDamage; } }


        private static readonly Dictionary<ProjectileType, int> ProjectileDamageValues = new Dictionary<ProjectileType, int>()
        {
            { ProjectileType.Missle, 1},
            { ProjectileType.EnemyProjectile, 1},
            { ProjectileType.Laser, 3},
            { ProjectileType.LaserBeam, 5}
        };

        public Projectile(Texture2D texture, Vector2 startPosition, bool isMovingNegative, ProjectileType projectileType = ProjectileType.Missle)
        {
            this.animationPlayer = new AnimationPlayer(texture, 1, 4);

            SetProjectileTravelSpeed(0);

            this.projectileType = projectileType;
            ObjectTexture = texture;

            this.Position = startPosition;
            this.Position.X += texture.Width / 8;
            this.IsMovingNegative = isMovingNegative;
            this.projectileDamage = ProjectileDamageValues[this.projectileType];

            SetUpCollisionCircle();
            this.animationPlayer.SetAnimationFramesRowLocations("move", 0);
        }

        private void SetUpCollisionCircle()
        {
            if (projectileType == ProjectileType.EnemyProjectile)
            {
                this.collisionCircle = new CollisionCircle(this.Position + enemyProjectileCollisionCirclePlacement, enemyProjectileCollisionCircleRadius);
            }
            else
            {
                this.collisionCircle = new CollisionCircle(this.Position + playerProjectileCollisionCirclePlacement, playerProjectileCollisionCircleRadius);
            }
        }



        public void SetProjectileTravelSpeed(int scoreValue)
        {
            if(projectileType == ProjectileType.EnemyProjectile)
            {
                this.travelSpeed = 150;
            }
            else
            {
                this.travelSpeed = 300;
            }
        }

        public override void CollidedWithOtherGameObject(GameObject gameObject = null)
        {
            if(gameObject is null)
            {
                base.CollidedWithOtherGameObject();
                return;
            }

            if(gameObject is Player)
            {

            }
            else if(gameObject is Projectile)
            {
                if (this.projectileType == ProjectileType.EnemyProjectile)
                {
                    //increase score by 100
                }
            }

            base.CollidedWithOtherGameObject();
        }

        public override void PlayAnimation()
        {
            this.animationPlayer.Play("move");
            CurrentAnimationRectangle = animationPlayer.CurrentRectangleLocation;
        }


        //moving negative means going up
        public override void Run(bool isMovingNegative, float EndOfScreenPosition)
        {
            if (isMovingNegative)
            {
                this.Position.Y -= this.travelSpeed * (float)Globals.GameTime.ElapsedGameTime.TotalSeconds;
                
            }
            else
            {
                this.Position.Y += this.travelSpeed * (float)Globals.GameTime.ElapsedGameTime.TotalSeconds;
            }
            UpdateCollisionCircle();
        }



        protected override void UpdateCollisionCircle()
        {
            if (IsMovingNegative)
            {
                this.collisionCircle.Centre = this.Position + playerProjectileCollisionCirclePlacement;
            }
            else
            {
                this.collisionCircle.Centre = this.Position + enemyProjectileCollisionCirclePlacement;
            }
        }
    }
}
