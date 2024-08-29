﻿using Drifter.Class.AbstractClass;
using Drifter.Class.Factory;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Drifter.Class.Tools;

namespace Drifter.Class.GameObjectClass.ObstacleClass
{
    internal class ShatteringAsteroid : Obstacle
    {
        private bool destroyWithoutSpawningAsteroids;
        private bool spawnAsteroids;
        private Timer timer;

        public Texture2D ShatteredTextureOne { get; private set; }
        public Texture2D ShatteredTextureTwo { get; private set; }


        public ShatteringAsteroid(Texture2D texture, Vector2 startPosition, ObstacleType obstacleType = ObstacleType.ShatteringAsteroid) : base(texture, startPosition, obstacleType)
        {
            spawnAsteroids = false;
            timer = new Timer();
            destroyWithoutSpawningAsteroids = false;
        }


        public override void CollidedWithOtherGameObject(GameObject gameObject = null)
        {
            spawnAsteroids = true;

            if(gameObject is Projectile)
            {
                Projectile p = (Projectile)gameObject;
                if(p.TypeOfProjectile is Projectile.ProjectileType.Laser)
                {
                    destroyWithoutSpawningAsteroids = true;
                    spawnAsteroids = false;
                }
            }
        }

        public override void Run(bool isMovingNegative, float EndOfScreenPosition)
        {
            base.Run(isMovingNegative, EndOfScreenPosition);
            if (spawnAsteroids)
            {

                if (timer.Set)
                {
                    if(Timer.CheckTimeReached(timer))
                    {
                        GameObjectSpawner.CreateAngledAsteroid(this.Texture, CurrentPosition, true, true);
                        GameObjectSpawner.CreateAngledAsteroid(this.Texture, CurrentPosition, true, false);
                        DestroyMyself();
                    }
                }
                else
                {
                    timer.SetStartTimeAndStopTime(10);
                }
            }
            else if (destroyWithoutSpawningAsteroids)
            {
                DestroyMyself();
            }
        }

    }
}
