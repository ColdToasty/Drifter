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

namespace Drifter.Class.GameObjectClass
{
    internal class Obstacle : GameObject
    {
        public enum ObstacleType { Asteroid, MovingAsteroid, SpaceWorm, UFO, Blackhole}

        protected ObstacleType obstacleType;

        protected int IncreaseScoreValue;

        //For worm
        //private int moveOnX = 50;
        //private bool changeDirection = false;
        //private float XStart;
        public Obstacle(Texture2D texture, Vector2 startPosition, ObstacleType obstacleType = ObstacleType.Asteroid)
        {
            this.Position = startPosition;
            //this.XStart = startPosition.X;
            this.ObjectTexture = texture;
            this.obstacleType = obstacleType;
            this.travelSpeed = 100;
            IncreaseScoreValue = 100;
            this.collisionCircle = new CollisionCircle(this.Position + new Vector2(8, 8), 16);
        }

        public override void Run(GameTime gameTime, bool isMovingNegative)
        {
            this.Position.Y += this.travelSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            this.collisionCircle.Centre = this.Position + new Vector2(16, 16);

            //For worm

            /*
            if (this.Position.X >= this.XStart+ Math.Abs(moveOnX)){
                moveOnX = moveOnX * -1;
            }
            else if(this.Position.X <= this.XStart - Math.Abs(moveOnX))
            {
                moveOnX = moveOnX * -1;

            }

            if(moveOnX < 0)
            {
                this.Position.X -= this.travelSpeed/2 * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else
            {
                this.Position.X += this.travelSpeed / 2 * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            */


        }

        public override void CollidedWithOtherGameObject()
        {
            Score.IncreaseScore(IncreaseScoreValue);
        }

    }
}
