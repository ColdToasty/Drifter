using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Drifter.Class.Tools;
using Drifter.Class.AbstractClass;



namespace Drifter.Class.GameObjectClass
{
    internal class Player : GameObject
    {

        private Projectile.ProjectileType projectileType;
        public Projectile.ProjectileType ProjectileType { get { return projectileType; } }

        private bool infiniteProjectile { get; set; }
        public bool InfiniteProjectile { get { return infiniteProjectile; } }

        public bool isDrifting;

        private Vector2 startingPosition;

        public Player(Texture2D texture, Vector2 startingPosition) {

            this.startingPosition = startingPosition;
            this.ObjectTexture = texture;
            Reset();

        }


        public void Reset()
        {
            this.travelSpeed = 100;
            this.infiniteProjectile = false;
            this.Position = startingPosition;
            projectileType = Projectile.ProjectileType.Missle;
            this.collisionCircle = new CollisionCircle(this.Position + new Vector2(16, 16), 8);
            this.isMovingLeft = false;
            this.isDrifting = false;
        }


        public override void Run(bool isMovingNegative, float EndOfScreenPosition)
        {
            if (isMovingNegative)
            {
                this.Position.X -= this.travelSpeed * (float)Globals.GameTime.ElapsedGameTime.TotalSeconds;
            }
            else
            {
                this.Position.X += this.travelSpeed * (float)Globals.GameTime.ElapsedGameTime.TotalSeconds;
            }

            this.CheckObjectAtEdge();
            this.collisionCircle.Centre = this.Position + new Vector2(16, 16);
        }


        public void Drift()
        {
            if (isDrifting)
            {
                if (isMovingLeft)
                {
                    this.Position.X -= this.travelSpeed * (float)Globals.GameTime.ElapsedGameTime.TotalSeconds;
                }
                else
                {
                    this.Position.X += this.travelSpeed * (float)Globals.GameTime.ElapsedGameTime.TotalSeconds;
                }
                this.CheckObjectAtEdge();
                this.collisionCircle.Centre = this.Position + new Vector2(16, 16);
            }
        }


        public override void CollidedWithOtherGameObject(GameObject gameObject = null)
        {
            if (gameObject == null)
            {
                System.Diagnostics.Trace.WriteLine("Im dead");
                base.CollidedWithOtherGameObject();
                return;
            }
            
            if (gameObject is Item)
            {
                Item item = (Item)gameObject;
                ConsumeItem(item);
            }
            
        }

        private void ConsumeItem(Item item)
        {
            switch (item.TypeOfItem)
            {
                case Item.ItemType.Coin:
                    Score.IncreaseScore(1000);
                    break;
                case Item.ItemType.Invincibility:
                    break;
                case Item.ItemType.Reflect:
                    break;

                case Item.ItemType.InfiniteMissiles:
                    infiniteProjectile = true;
                    break;

                case Item.ItemType.LaserBeam:
                    SetProjectileType(Projectile.ProjectileType.LaserBeam);
                    break;

                case Item.ItemType.InfiniteLaserBeam:
                    SetProjectileType(Projectile.ProjectileType.LaserBeam);
                    infiniteProjectile = true;
                    break;

            }
        }

        private void SetProjectileType(Projectile.ProjectileType projectileType)
        {
            this.projectileType = projectileType;
        }

    }
}
