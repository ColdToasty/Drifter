﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Drifter.Class.AbstractClass;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Drifter.Class.Tools;

namespace Drifter.Class.GameObjectClass
{
    internal class Item : GameObject
    {
        
        public enum ItemType
        {
            Coin, Protection, Invincibility, InfiniteMissiles, LaserBeam, InfiniteLasterBeam
        }

        private ItemType itemType;



        public Item(Texture2D texture, Vector2 startPosition, ItemType itemType = ItemType.Coin)
        {
            this.ObjectTexture = texture;
            this.Position = startPosition;
            this.itemType = itemType;
            this.travelSpeed = 100;

            this.collisionCircle = new CollisionCircle(new Vector2(8, 8), 4);
        }
        

        public override void Run(GameTime gameTime, bool IsMovingNegative)
        {
            this.Position.Y += travelSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            this.UpdateCollisionCircle();
        }



    }
}
