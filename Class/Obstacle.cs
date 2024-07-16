using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceRider.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpaceRider.Class;
using Drifter.Class;
using Microsoft.Xna.Framework.Input;

namespace SpaceRider.Class
{
    internal class Obstacle : GameObject
    {
        public enum ObstacleType { Asteroid }


        private ObstacleType obstacleType;


        public Texture2D Texture { get { return ObjectTexture; } }

        public Vector2 startPosition { get; init; }

     
        public Obstacle(Texture2D texture, Vector2 startPosition, ObstacleType obstacleType = ObstacleType.Asteroid)
        {
            this.startPosition = startPosition;
            this.Position = startPosition;
            this.ObjectTexture = texture;
            this.obstacleType = obstacleType;
            this.travelSpeed = 150;
            this.collisionCircle = new CollisionCircle(new Vector2(8, 8), 8);
        }

        public override void Run(GameTime gameTime, bool IsMovingNegative)
        {
            this.Position.Y += this.travelSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            this.UpdateCollisionCircle();

        }
        


    }
}
