using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Net.Mime;
using Microsoft.Xna.Framework.Content;
using Drifter.Class.Tools;
using Drifter.Class.AbstractClass;



namespace Drifter.Class.GameObjectClass
{
    internal class Player : GameObject
    {

        private bool itemPickedUp { get; set; }
        public bool ItemPickedUp { get { return itemPickedUp; } }


        public Player(Vector2 startingPosition) {
            this.travelSpeed = 100;
            this.itemPickedUp = false;
            this.Position = startingPosition;

            this.collisionCircle = new CollisionCircle(this.Position + new Vector2(8, 8), 16);
        }


        public override void Run(GameTime gameTime, bool isMovingNegative)
        {
            if (isMovingNegative)
            {
                this.Position.X -= this.travelSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else
            {
                this.Position.X += this.travelSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            this.UpdateCollisionCircle();
        }


        public void SetPositionAtEdgeOfScreen(float edgePosition)
        {
            this.Position.X = edgePosition;
        }

    }
}
