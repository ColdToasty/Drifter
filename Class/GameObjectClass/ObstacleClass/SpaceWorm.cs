using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Drifter.Class.Tools;

namespace Drifter.Class.GameObjectClass.ObstacleClass
{
    internal class SpaceWorm : Obstacle
    {
        float moveOnX = 20;

        float XStart;

        public SpaceWorm(Vector2 startPosition, ObstacleType obstacleType = ObstacleType.Asteroid) : base(startPosition, obstacleType)
        {

            this.XStart = startPosition.X;
        }

        public SpaceWorm(Texture2D texture, Vector2 startPosition, ObstacleType obstacleType = ObstacleType.Asteroid) : base(texture, startPosition, obstacleType)
        {
            this.XStart = startPosition.X;
        }


        public override void Run(bool isMovingNegative, float EndOfScreenPosition)
        {
            base.Run(isMovingNegative, EndOfScreenPosition);

            if (this.Position.X >= this.XStart + Math.Abs(moveOnX))
            {
                moveOnX = moveOnX * -1;
            }
            else if (this.Position.X <= this.XStart - Math.Abs(moveOnX))
            {
                moveOnX = moveOnX * -1;

            }

            if (moveOnX < 0)
            {
                this.Position.X -= this.travelSpeed / 2f * (float)Globals.GameTime.ElapsedGameTime.TotalSeconds;
            }
            else
            {
                this.Position.X += this.travelSpeed / 2f * (float)Globals.GameTime.ElapsedGameTime.TotalSeconds;
            }
        }

    }
}
