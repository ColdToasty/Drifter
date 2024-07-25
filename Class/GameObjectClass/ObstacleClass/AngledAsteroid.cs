using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drifter.Class.GameObjectClass.ObstacleClass
{
    internal class AngledAsteroid : Obstacle
    {
        public AngledAsteroid(Texture2D texture, Vector2 startPosition, bool setCustomDirection = false, bool moveLeft = false, Obstacle.ObstacleType obstacleType = ObstacleType.AngledAsteroid) : base(texture, startPosition, obstacleType)
        {
            //if dont want to manually set it to move left / right
            //other wise determine move direction basted on startPosition
            if (!setCustomDirection)
            {
                switch (startPosition.X)
                {
                    case 0:
                        this.isMovingLeft = false;
                        break;

                    default:
                        this.isMovingLeft = true;
                        break;
                }
            }
            else
            {
                if (moveLeft)
                {
                    this.isMovingLeft = true;
                }
                else
                {
                    this.isMovingLeft = false;
                }
            }
        }



        public override void Run(GameTime gameTime, bool isMovingNegative, float EndOfScreenPosition)
        {
            base.Run(gameTime, isMovingNegative, EndOfScreenPosition);
            if (this.isMovingLeft)
            {
                this.Position.X -= travelSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else
            {
                this.Position.X += travelSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
        }


    }
}
