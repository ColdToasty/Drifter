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
using Drifter.Class.Factory;

namespace Drifter.Class.GameObjectClass.ObstacleClass
{
    internal class Obstacle : GameObject
    {
        public enum ObstacleType { Asteroid, AngledAsteroid, ShatteringAsteroid, SpaceWorm, AlienSpaceship, KamakaziAlienSpaceship, Blackhole, SpacePipe }

        protected Dictionary<ObstacleType, int> healthValues, travelSpeeds, scoreIncreaseValues;

        protected ObstacleType obstacleType;

        protected int IncreaseScoreValue;

        private int health;

        public int Health { get { return health; } }


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
            collisionCircle = new CollisionCircle(Position + new Vector2(8, 8), 16);

            SetHealth();
            SetTravelSpeed();
            SetScoreIncreaseValue();
        }

        private void SetHealth()
        {
            healthValues = new Dictionary<ObstacleType, int>()
            {
                {ObstacleType.Asteroid, 1},
                {ObstacleType.AngledAsteroid, 1},
                {ObstacleType.ShatteringAsteroid, 1},
                {ObstacleType.SpaceWorm, 3 },
                {ObstacleType.AlienSpaceship, 3},
                {ObstacleType.KamakaziAlienSpaceship, 3},
                {ObstacleType.Blackhole, 50},
                {ObstacleType.SpacePipe, 50}
            };
            health = healthValues[obstacleType];
        }

        private void SetTravelSpeed()
        {
            travelSpeeds = new Dictionary<ObstacleType, int>()
            {
                {ObstacleType.Asteroid, 100},
                {ObstacleType.AngledAsteroid, 100},
                {ObstacleType.ShatteringAsteroid, 100},
                {ObstacleType.SpaceWorm, 50},
                {ObstacleType.AlienSpaceship, 100},
                {ObstacleType.KamakaziAlienSpaceship, 150},
                {ObstacleType.Blackhole, 50},
                {ObstacleType.SpacePipe, 50}
            };

            travelSpeed = travelSpeeds[obstacleType];
        }

        private void SetScoreIncreaseValue()
        {
            scoreIncreaseValues = new Dictionary<ObstacleType, int>()
            {
                {ObstacleType.Asteroid, 100},
                {ObstacleType.AngledAsteroid, 150},
                {ObstacleType.ShatteringAsteroid, 100},
                {ObstacleType.SpaceWorm, 300 },
                {ObstacleType.AlienSpaceship, 500},
                {ObstacleType.KamakaziAlienSpaceship, 1000},
                {ObstacleType.Blackhole, 0},
                {ObstacleType.SpacePipe, 0}
            };

            IncreaseScoreValue = scoreIncreaseValues[obstacleType];
        }



        public override void Run(GameTime gameTime, bool isMovingNegative, float EndOfScreenPosition)
        {
            Position.Y += travelSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            base.Run(gameTime, isMovingNegative, EndOfScreenPosition);
            collisionCircle.Centre = Position + new Vector2(16, 16);
        }


        public override void CollidedWithOtherGameObject(GameObject gameObject = null)
        {
            if(gameObject is null)
            {
                base.CollidedWithOtherGameObject();
                return;
            }

            if(gameObject is Projectile)
            {
                HitByProjectile(((Projectile)gameObject).TypeOfProjectile);
            }

        }

        private void HitByProjectile(Projectile.ProjectileType projectileType)
        {
            health--;
            if (health <= 0)
            {
                Score.IncreaseScore(IncreaseScoreValue);
                DestroyMyself();
            }
        }


    }
}
