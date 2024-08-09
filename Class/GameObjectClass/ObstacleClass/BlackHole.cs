using Drifter.Class.AbstractClass;
using Drifter.Class.Tools;
using Drifter.Class.Tools.CollisionShapes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drifter.Class.GameObjectClass.ObstacleClass
{
    internal class BlackHole : Obstacle
    {
        //circle to pull player towards collisionCircle
        public CollisionCircle pullPlayerCircle;
        private bool isOnLeft;

        private int blackHolePullRange = (Globals.ScreenWidth / 5) * 2; 
        public BlackHole(Texture2D texture, Vector2 startPosition, ObstacleType obstacleType = ObstacleType.Blackhole) : base(texture, startPosition, obstacleType)
        {
            this.ObjectTexture = texture;
            this.Position = startPosition;
            if(startPosition.X <= 0)
            {
                isOnLeft = true;
            }
            else
            {
                isOnLeft = false;
            }

            this.collisionCircle = new CollisionCircle(this.Position + new Vector2(this.ObjectTexture.Width / 2, this.ObjectTexture.Height / 2), 32);
            this.pullPlayerCircle = new CollisionCircle(this.Position + new Vector2(this.ObjectTexture.Width, this.ObjectTexture.Height), blackHolePullRange);
        }

        public override void Run(bool isMovingNegative, float EndOfScreenPosition)
        {
            base.Run(isMovingNegative, EndOfScreenPosition);
            UpdateCollisionCircle();

        }

        public override void CollidedWithOtherGameObject(GameObject gameObject = null)
        {
            if(gameObject is Player)
            {
                Player player = gameObject as Player;
                player.isDrifting = true;
                if (isOnLeft)
                {
                    player.isMovingLeft = true;
                }
                else
                {
                    player.isMovingLeft = false;
                }
            }
        }

        protected override void UpdateCollisionCircle()
        {
            this.collisionCircle.Centre = this.Position + new Vector2(this.ObjectTexture.Width / 2, this.ObjectTexture.Height / 2);
            this.pullPlayerCircle.Centre = this.Position + new Vector2(this.ObjectTexture.Width, this.ObjectTexture.Height);
        }
    }
}
