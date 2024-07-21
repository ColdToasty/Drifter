using System;
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
            Coin, Reflect, Invincibility, InfiniteMissiles, LaserBeam, InfiniteLaserBeam, SuperNova
        }

        public ItemType TypeOfItem { get; init; }
        
        public Item(Texture2D texture, Vector2 startPosition, ItemType itemType = ItemType.Coin)
        {
            this.ObjectTexture = texture;
            this.Position = startPosition;
            this.TypeOfItem = itemType;
            this.travelSpeed = 100;

            this.collisionCircle = new CollisionCircle(new Vector2(8, 8), 16);
        }
        
        public override void Run(GameTime gameTime, bool isMovingNegative)
        {
            this.Position.Y += travelSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            base.Run(gameTime, isMovingNegative);
        }


    }
}
