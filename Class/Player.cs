using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Net.Mime;
using Microsoft.Xna.Framework.Content;
using SpaceRider.Interface;



namespace SpaceRider.Class
{
    internal class Player : GameObject
    {
        private string playerTexture { get; set; }
        public string PlayerTexture { get { return playerTexture; } }


        private bool itemPickedUp { get; set; }
        public bool ItemPickedUp { get { return itemPickedUp; } }


        public Player(string playerTexture, Vector2 startingPosition) {
            this.playerTexture = playerTexture;
            this.travelSpeed = 100;
            this.itemPickedUp = false;
            this.Position = startingPosition;
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
        }

        public void SetPositionAtEdgeOfScreen(float edgePosition)
        {
            this.Position.X = edgePosition;
        }

    }
}
