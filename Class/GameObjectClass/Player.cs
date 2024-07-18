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
using Drifter.Class.GameObjectClass;



namespace Drifter.Class.GameObjectClass
{
    internal class Player : GameObject
    {

        private Projectile.ProjectileType projectileType;
        public Projectile.ProjectileType ProjectileType { get { return projectileType; } }

        private bool infiniteProjectile { get; set; }
        public bool InfiniteProjectile { get { return infiniteProjectile; } }


        public Player(Vector2 startingPosition) {
            this.travelSpeed = 100;
            this.infiniteProjectile = false;
            this.Position = startingPosition;
            projectileType = Projectile.ProjectileType.Missle;
            this.collisionCircle = new CollisionCircle(this.Position + new Vector2(16, 16), 14);
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

            this.collisionCircle.Centre = this.Position + new Vector2(16, 16);
        }


        public void SetPositionAtEdgeOfScreen(float edgePosition)
        {
            this.Position.X = edgePosition;
        }

        public override void CollidedWithOtherGameObject(GameObject gameObject)
        {
            if(gameObject is Item)
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
