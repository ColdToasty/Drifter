using Microsoft.Xna.Framework.Graphics;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Drifter.Class.Tools;
using Drifter.Class.AbstractClass;
using Drifter.Class.Tools.CollisionShapes;
using Drifter.Class.GameObjectClass.ObstacleClass;
using Drifter.Class.GameObjectClass.ItemClass;
using static Drifter.Class.GameObjectClass.Projectile;
using Drifter.Class.Factory;
using Drifter.Class.Commands;
using System;
using System.ComponentModel.Design;


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

        private bool isAlive;
        public bool IsAlive { get { return isAlive; } }

        public bool IsUnhurtable { get; private set; }

        private Timer itemDurationTimer;

        private Item.ItemType? itemTypeActive;

        private int driftFrameStart = 2;
        public Player(Texture2D texture, Vector2 startingPosition) {

            this.startingPosition = startingPosition;
            this.ObjectTexture = texture;
            Reset();
        }

        public void Reset()
        {
            this.itemDurationTimer = new Timer();
            this.travelSpeed = 100;
            this.infiniteProjectile = false;
            this.Position = startingPosition;
            projectileType = Projectile.ProjectileType.Missle;
            this.collisionCircle = new CollisionCircle(this.Position + new Vector2(16, 16), 8);
            this.isMovingLeft = false;
            this.isDrifting = false;
            this.isAlive = true;
            this.IsUnhurtable = false;

            //timer in animationPlayer is setting before GameTime has a chance to be set in the update loop
            //texture, number of rows, number of columns
            this.animationPlayer = new AnimationPlayer(this.ObjectTexture, 3, 4);
            this.animationPlayer.SetAnimationFramesRowLocations("move", 0);
            this.animationPlayer.SetAnimationFramesRowLocations("moveRight", 1);
            this.animationPlayer.SetAnimationFramesRowLocations("moveLeft", 2);
            ConsumeItem(new Item(Vector2.Zero, Item.ItemType.LaserBeam));
        }

        public override void Run(bool isMovingNegative, float EndOfScreenPosition)
        {

            if (Timer.CheckTimeReached(itemDurationTimer))
            {
                ExpireItem();
            }
            
            if (isMovingNegative)
            {
                this.Position.X -= this.travelSpeed * (float)Globals.GameTime.ElapsedGameTime.TotalSeconds;
                this.isMovingLeft = true;
            }
            else
            {
                this.Position.X += this.travelSpeed * (float)Globals.GameTime.ElapsedGameTime.TotalSeconds;
                this.isMovingLeft = false;
            }

            this.CheckObjectAtEdge();
            this.collisionCircle.Centre = this.Position + new Vector2(16, 16);
        }


        public override void PlayAnimation()
        {

            string animationToPlay;

            if (isDrifting)
            {
                if (isMovingLeft)
                {
                    animationToPlay = "moveLeft";
                }
                else
                {
                    animationToPlay = "moveRight";
                }
            }
            else{
                animationToPlay = "move";
            }

            animationPlayer.Play(animationToPlay, true);
            CurrentAnimationRectangle = animationPlayer.CurrentRectangleLocation;
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
                if (!this.IsUnhurtable)
                {
                    base.CollidedWithOtherGameObject();
                    isAlive = false;
                }

            }
            
            if (gameObject is Item)
            {
                Item item = (Item)gameObject;
                ConsumeItem(item);
            }
            else if(gameObject is Obstacle)
            {

                if (!this.IsUnhurtable)
                {
                    DestroyMyself();
                    isAlive = false;
                }

                
            }
            
        }

        //Applies the item's effect and sets the duration timer
        private void ConsumeItem(Item item)
        {

            switch (item.TypeOfItem)
            {
                case Item.ItemType.Coin:
                    Score.IncreaseScore(1000);
                    break;

                case Item.ItemType.Invincibility:
                    this.IsUnhurtable = true;
                    infiniteProjectile = false;
                    break;

                case Item.ItemType.Reflect:
                    this.IsUnhurtable = true;
                    infiniteProjectile = false;
                    break;

                case Item.ItemType.InfiniteMissiles:
                    infiniteProjectile = true;
                    this.IsUnhurtable = false;
                    break;

                case Item.ItemType.Laser:
                    this.projectileType = Projectile.ProjectileType.Laser;
                    this.IsUnhurtable = false;
                    break;

                case Item.ItemType.LaserBeam:
                    this.projectileType = Projectile.ProjectileType.LaserBeam;
                    infiniteProjectile = true;
                    this.IsUnhurtable = false;
                    break;

                case Item.ItemType.SuperNova:
                    GameObjectSpawner.DeleteAllEnemies();
                    break;
            }
            if((item.TypeOfItem != Item.ItemType.SuperNova && item.TypeOfItem != Item.ItemType.Coin))
            {
                itemTypeActive = item.TypeOfItem;
                itemDurationTimer.SetStartTimeAndStopTime(item.ItemDuration);
                System.Diagnostics.Trace.WriteLine(item.ItemDuration);
            }

        }

        //reset itemTypeActive and timer
        private void ExpireItem()
        {
            System.Diagnostics.Trace.WriteLine(itemTypeActive);
            switch (itemTypeActive)
            {
                case Item.ItemType.Invincibility:
                case Item.ItemType.Reflect:
                    this.IsUnhurtable = false;
                    break;


                case Item.ItemType.Laser:
                case Item.ItemType.LaserBeam:
                case Item.ItemType.InfiniteMissiles:
                    projectileType = ProjectileType.Missle;
                    infiniteProjectile = false;
                    break;


            }
            itemTypeActive = null;
            itemDurationTimer.ResetTimer();
        }

    }
}
