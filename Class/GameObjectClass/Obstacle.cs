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

namespace Drifter.Class.GameObjectClass
{
    internal class Obstacle : GameObject
    {
        public enum ObstacleType { Asteroid }


        private ObstacleType obstacleType;


        public Texture2D Texture { get { return ObjectTexture; } }

        public Obstacle(Texture2D texture, Vector2 startPosition, ObstacleType obstacleType = ObstacleType.Asteroid)
        {
            this.Position = startPosition;
            this.ObjectTexture = texture;
            this.obstacleType = obstacleType;
            this.travelSpeed = 150;
            this.collisionCircle = new CollisionCircle(this.Position + new Vector2(8, 8), 16);
        }

        public override void Run(GameTime gameTime, bool IsMovingNegative)
        {
            this.Position.Y += this.travelSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            this.UpdateCollisionCircle();

        }

        public override void CollidedWithOtherGameObject()
        {
 
        }

    }
}
