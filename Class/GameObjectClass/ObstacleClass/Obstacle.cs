using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Drifter.Class.AbstractClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Drifter.Class.Tools;
using Microsoft.Xna.Framework.Input;
using System.Runtime.CompilerServices;

namespace Drifter.Class.GameObjectClass.ObstacleClass
{
    internal class Obstacle : GameObject
    {
        public enum ObstacleType { Asteroid, ShatteringAsteroid, SpaceWorm, AlienSpaceship, Blackhole, SpacePipe }

        protected ObstacleType obstacleType;

        protected int IncreaseScoreValue;

        protected int health;

        public int Health { get { return health; } }

        protected Random random;

        //For worm
        //private int moveOnX = 50;
        //private bool changeDirection = false;
        //private float XStart;
        public Obstacle(Texture2D texture, Vector2 startPosition, ObstacleType obstacleType = ObstacleType.Asteroid)
        {
            Position = startPosition;
            //this.XStart = startPosition.X;
            ObjectTexture = texture;
            this.obstacleType = obstacleType;
            travelSpeed = 100;
            IncreaseScoreValue = 100;
            health = 2;
            collisionCircle = new CollisionCircle(Position + new Vector2(8, 8), 16);
            this.random = new Random();
        }

        public override void Run(GameTime gameTime, bool isMovingNegative)
        {
            Position.Y += travelSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            collisionCircle.Centre = Position + new Vector2(16, 16);
        }

        public void HitByProjectile(Projectile.ProjectileType projectileType)
        {
            health--;
            if (health <= 0)
            {
                Score.IncreaseScore(IncreaseScoreValue);
            }
        }




        public override void CollidedWithOtherGameObject()
        {
        }

    }
}
