﻿using Microsoft.Xna.Framework;
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
using Drifter.Class.Tools.CollisionShapes;

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
            explosionTexture = Globals.GetTexture("explosion");
            effectAnimationPlayer = new AnimationPlayer(explosionTexture, 1, 5);
            effectAnimationPlayer.SetAnimationFramesRowLocations("explosion", 0);
            this.effectAnimationPlayer.SetFrameThreshHold(100);

            Position = startPosition;
            //this.XStart = startPosition.X;
            ObjectTexture = texture;
            this.obstacleType = obstacleType;

            IncreaseScoreValue = 100;
            collisionCircle = new CollisionCircle(Position + new Vector2(8, 8), 16);
            IsAlive = true;

            SetHealth();
            SetTravelSpeed();
            SetScoreIncreaseValue();

            if (this.obstacleType == ObstacleType.Asteroid)
            {
                animationPlayer = new AnimationPlayer(texture, 1, 5);
                animationPlayer.SetAnimationFramesRowLocations("death",  0);
                this.CurrentAnimationRectangle = animationPlayer.CurrentRectangleLocation;
                this.CurrentExplosionAnimationRectangle = effectAnimationPlayer.CurrentRectangleLocation;
                this.animationPlayer.SetFrameThreshHold(100);
                int setAsteroidSpeed = Globals.Random.Next(3);
                if(setAsteroidSpeed == 0)
                {
                    return;
                }
                else if(setAsteroidSpeed == 1)
                {
                    this.travelSpeed = 125;
                }
                else
                {
                    this.travelSpeed = 150;
                }
            }
        }


        //Use when >1 texture
        public Obstacle(Vector2 startPosition, ObstacleType obstacleType = ObstacleType.Asteroid)
        {
            SetHealth();
            SetTravelSpeed();
            SetScoreIncreaseValue();

            this.Position = startPosition;
            this.obstacleType = obstacleType;
        }

        protected void SetHealth()
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

        protected void SetTravelSpeed()
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
                {ObstacleType.SpacePipe, 75}
            };

            travelSpeed = travelSpeeds[obstacleType];
        }

        protected void SetScoreIncreaseValue()
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




        public override void Run(bool isMovingNegative, float EndOfScreenPosition)
        {
            Position.Y += travelSpeed * (float)Globals.GameTime.ElapsedGameTime.TotalSeconds;
            base.Run(isMovingNegative, EndOfScreenPosition);
            collisionCircle.Centre = Position + new Vector2(16, 16);

            if (!IsAlive)
            {
                collisionCircle.DisableCircle();
                animationPlayer.Play("death");
                effectAnimationPlayer.Play("explosion");
                this.CurrentAnimationRectangle = animationPlayer.CurrentRectangleLocation;
                this.CurrentExplosionAnimationRectangle = effectAnimationPlayer.CurrentRectangleLocation;
                if (animationPlayer.AnimationFinished)
                {
                    DestroyMyself();
                }
            }
        }



        public override void CollidedWithOtherGameObject(GameObject gameObject = null)
        {
            if(gameObject is null || gameObject is Player)
            {
                base.CollidedWithOtherGameObject();
                return;
            }

            if(gameObject is Projectile)
            {
                HitByProjectile(((Projectile)gameObject).ProjectileDamge);
            }

        }

        private void HitByProjectile(int damageAmount)
        {
            health -= damageAmount;
            if(obstacleType is ObstacleType.AlienSpaceship)
            {
                System.Diagnostics.Trace.WriteLine(health);
            }
            if (health <= 0)
            {
                Score.IncreaseScore(IncreaseScoreValue);
                IsAlive = false;
            }
        }


        public void StopSoundEffects()
        {
            if(soundEffectInstance is not null)
            {
                soundEffectInstance.Dispose();
            }
        }

        public override void PlayAnimation()
        {
        }
    }
}
