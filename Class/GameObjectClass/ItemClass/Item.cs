using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Drifter.Class.AbstractClass;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Drifter.Class.Tools;
using Drifter.Class.Tools.CollisionShapes;

namespace Drifter.Class.GameObjectClass.ItemClass
{
    internal class Item : GameObject
    {

        public enum ItemType
        {
            Coin, Reflect, Invincibility, InfiniteMissiles, Laser, LaserBeam, SuperNova
        }


        public ItemType TypeOfItem { get; init; }

        //How long an item's effect will last for
        public int ItemDuration { get; init; }

        //item's duration it is active when player picks it up in milliseconds
        private static Dictionary<ItemType, int> itemTypeDuration = new Dictionary<ItemType, int>() {
            { ItemType.Coin, 0 },
            { ItemType.Reflect, 20000},
            { ItemType.Invincibility, 15000 },
            { ItemType.InfiniteMissiles, 12500 },
            { ItemType.Laser, 25000},
            { ItemType.LaserBeam, 15000},
            { ItemType.SuperNova, 0}
        };



        public Item(Vector2 startPosition, ItemType itemType = ItemType.Coin)
        {
            Position = startPosition;
            TypeOfItem = itemType;
            travelSpeed = 100;
            SetTexture();
            ItemDuration = itemTypeDuration[itemType];
            collisionCircle = new CollisionCircle(new Vector2(8, 8), 16);
        }


        public void SetTexture()
        {
            switch (TypeOfItem)
            {
                default:
                case ItemType.Coin:
                    this.ObjectTexture = Globals.GetTexture("coin");
                    break;

                case ItemType.Reflect:
                    this.ObjectTexture = Globals.GetTexture("laserItem");
                    break;

                case ItemType.Invincibility:
                    this.ObjectTexture = Globals.GetTexture("invincibility");
                    break;

                case ItemType.InfiniteMissiles:
                    this.ObjectTexture = Globals.GetTexture("infiniteMissiles");
                    break;

                case ItemType.Laser:
                    this.ObjectTexture = Globals.GetTexture("laserItem");
                    break;

                case ItemType.SuperNova:
                    this.ObjectTexture = Globals.GetTexture("superNova");
                    break;


            }
        }

        public override void Run(bool isMovingNegative, float EndOfScreenPosition)
        {
            Position.Y += travelSpeed * (float)Globals.GameTime.ElapsedGameTime.TotalSeconds;
            base.Run(isMovingNegative, EndOfScreenPosition);
        }

        public override void PlayAnimation()
        {
        }
    }
}
