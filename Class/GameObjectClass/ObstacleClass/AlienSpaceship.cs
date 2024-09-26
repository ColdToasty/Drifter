using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Drifter.Class.Tools;
using Drifter.Class.Factory;
using Drifter.Class.Commands;
using Drifter.Class.AbstractClass;
using System.Drawing;

namespace Drifter.Class.GameObjectClass.ObstacleClass
{
    internal class AlienSpaceship : Obstacle
    {
        //Will track how long a command was last
        private Timer movementTimer;
        private Timer deathTimer;
        //Will place intervals on when projectile will spawn
        private Timer shootTimer;
        private enum Move { Move, Stop}

        private Move moveToMake;

        private int movePickRate = 8;
        private int stopPickRate = 2;

        private int movementPick;


        public AlienSpaceship(Texture2D texture, Vector2 startPosition, ObstacleType obstacleType = ObstacleType.AlienSpaceship) : base(texture, startPosition, obstacleType)
        {
            this.travelSpeed = 100;
            this.movementTimer = new Timer();
            this.shootTimer = new Timer();
            this.deathTimer = new Timer();
            this.moveToMake = Move.Move;
            this.animationPlayer = new AnimationPlayer(texture, 2, 5);
            this.animationPlayer.SetAnimationFramesRowLocations("idle", 0);
            this.animationPlayer.SetAnimationFramesRowLocations("death", 1);
            this.CurrentAnimationRectangle = animationPlayer.CurrentRectangleLocation;
            this.animationPlayer.SetFrameThreshHold(100);
            this.effectAnimationPlayer.SetFrameThreshHold(100);
        }



        public override void Run(bool isMovingNegative, float EndOfScreenPosition)
        {
            CheckMovementTimer(isMovingNegative);
            CheckShootTimer();
            if (!IsAlive)
            {
                animationPlayer.Play("death");
                effectAnimationPlayer.Play("explosion");
                this.CurrentAnimationRectangle = animationPlayer.CurrentRectangleLocation;
                this.CurrentExplosionAnimationRectangle = effectAnimationPlayer.CurrentRectangleLocation;
                if (animationPlayer.AnimationFinished)
                {
                    DestroyMyself();
                }
            }
            else
            {
                if (moveToMake == Move.Stop)
                {
                    animationPlayer.Play("idle", true);
                    this.CurrentAnimationRectangle = animationPlayer.CurrentRectangleLocation;
                }

                if (deathTimer.Set)
                {
                    if (Timer.CheckTimeReached(deathTimer))
                    {
                        DestroyMyself();
                    }
                }
                else
                {
                    deathTimer.SetStartTimeAndStopTime(30000);
                }
            }

        }

        private void CheckMovementTimer(bool isMovingNegative)
        {
            if (movementTimer.Set)
            {
                if (Timer.CheckTimeReached(movementTimer))
                {
                    movementTimer.ResetTimer();
                    animationPlayer.SetRectangle(0, 0);
                }
                else
                {
                    if (moveToMake == Move.Move)
                    {
                        if (isMovingNegative)
                        {
                            this.Position.X -= this.travelSpeed * (float)Globals.GameTime.ElapsedGameTime.TotalSeconds;
                        }
                        else
                        {
                            this.Position.X += this.travelSpeed * (float)Globals.GameTime.ElapsedGameTime.TotalSeconds;
                        }


                        this.CheckObjectAtEdge();
                        this.collisionCircle.Centre = this.Position + new Vector2(16, 16);
                    }
                }
            }
            else
            {
                movementPick = Globals.Random.Next(movePickRate + stopPickRate);

                if (movementPick < stopPickRate)
                {
                    moveToMake = Move.Stop;
                    movementTimer.SetStartTimeAndStopTime(5000);

                }
                else
                {
                    moveToMake = Move.Move;
                    movementTimer.SetStartTimeAndStopTime( 7000);
                }
            }
        }


        private void CheckShootTimer()
        {
            if (!shootTimer.Set)
            {
                if(moveToMake == Move.Move)
                {
                    GameObjectSpawner.CreateEnemyProjectile(CurrentPosition, false);
                    shootTimer.SetStartTimeAndStopTime(4000);
                    Globals.GetSoundEffect("enemyShoot").Play();
                }
            }
            else
            {
                if (Timer.CheckTimeReached(shootTimer))
                {
                    shootTimer.ResetTimer();
                }
            }
        }


        protected override void CheckObjectAtEdge()
        {
            if (CurrentPosition.X < 0)
            {
                SetPositionAtEdgeOfScreen(0);
                isMovingLeft = false;
            }

            else if (CurrentPosition.X + Texture.Width / 10 > Globals.ScreenWidth - Texture.Width / 10)
            {
                SetPositionAtEdgeOfScreen(Globals.ScreenWidth - Texture.Width / 5);
                isMovingLeft = true;
            }
        }


    }
}
