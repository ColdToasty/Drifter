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
        }



        public override void Run(bool isMovingNegative, float EndOfScreenPosition)
        {
            CheckMovementTimer(isMovingNegative);
            CheckShootTimer();
            if (deathTimer.Set)
            {
                if(Timer.CheckTimeReached(deathTimer)){
                    DestroyMyself();
                }
            }
            else
            {
                deathTimer.SetStartTimeAndStopTime(30000);
            }
        }

        private void CheckMovementTimer(bool isMovingNegative)
        {
            if (movementTimer.Set)
            {
                if (Timer.CheckTimeReached(movementTimer))
                {
                    movementTimer.ResetTimer();
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
                movementPick = Game1.Random.Next(movePickRate + stopPickRate);

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
                    GameObjectSpawner.CreateEnemyProjectile(this.ObjectTexture, CurrentPosition, false);
                    shootTimer.SetStartTimeAndStopTime(4000);
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

            else if (CurrentPosition.X + Texture.Width / 2 > Globals.ScreenWidth - Texture.Width / 2)
            {
                SetPositionAtEdgeOfScreen(Globals.ScreenWidth - Texture.Width);
                isMovingLeft = true;
            }
        }


    }
}
