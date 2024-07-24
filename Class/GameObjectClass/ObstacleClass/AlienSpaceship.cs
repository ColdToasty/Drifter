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


namespace Drifter.Class.GameObjectClass.ObstacleClass
{
    internal class AlienSpaceship : Obstacle
    {
        //Will track how long a command was last
        private Timer movementTimer;

        //Will place intervals on when projectile will spawn
        private Timer shootTimer;
        private Random random = new Random();
        private enum Move { Move, Stop}

        private Move moveToMake;

        private int movePickRate = 7;
        private int stopPickRate = 4;

        private int movementPick;
        public AlienSpaceship(Texture2D texture, Vector2 startPosition, ObstacleType obstacleType = ObstacleType.AlienSpaceship) : base(texture, startPosition, obstacleType)
        {
            this.travelSpeed = 100;
            this.movementTimer = new Timer();
            this.shootTimer = new Timer();
            this.moveToMake = Move.Move;
        }

        public override void Run(GameTime gameTime, bool isMovingNegative, float EndOfScreenPosition)
        {
            CheckMovementTimer(gameTime, isMovingNegative);
            CheckShootTimer(gameTime);
        }

        private void CheckMovementTimer(GameTime gameTime, bool isMovingNegative)
        {
            if (movementTimer.Set)
            {
                if (Timer.CheckTimeReached(gameTime, movementTimer))
                {
                    movementTimer.ResetTimer();
                }
                else
                {
                    if (moveToMake == Move.Move)
                    {
                        if (isMovingNegative)
                        {
                            this.Position.X -= this.travelSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                        }
                        else
                        {
                            this.Position.X += this.travelSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                        }


                        this.CheckObjectAtEdge();
                        this.collisionCircle.Centre = this.Position + new Vector2(16, 16);
                    }
                }
            }
            else
            {
                movementPick = random.Next(movePickRate + stopPickRate);

                if (movementPick <= stopPickRate)
                {
                    moveToMake = Move.Stop;
                    movementTimer.SetStartTimeAndStopTime(gameTime, 5000);
                }
                else
                {
                    moveToMake = Move.Move;
                    movementTimer.SetStartTimeAndStopTime(gameTime, 7000);
                }
            }
        }

        private void CheckShootTimer(GameTime gameTime)
        {
            if (!shootTimer.Set)
            {
                GameObjectSpawner.CreateEnemyProjectile(this.ObjectTexture, CurrentPosition, false);
                shootTimer.SetStartTimeAndStopTime(gameTime, 6000);
            }
            else
            {
                if (Timer.CheckTimeReached(gameTime, shootTimer))
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

            else if (CurrentPosition.X + Texture.Width / 2 > Game1.ScreenWidth - Texture.Width / 2)
            {
                SetPositionAtEdgeOfScreen(Game1.ScreenWidth - Texture.Width);
                isMovingLeft = true;
            }
        }



    }
}
