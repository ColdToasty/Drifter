using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Drifter.Class.Tools;
using Drifter.Class.Factory;
using Drifter.Class.Commands;


namespace Drifter.Class.GameObjectClass.ObstacleClass
{
    internal class AlienSpaceship : Obstacle
    {
        //will keep track on when to choose the next command
        private Timer commandTimer;
        //Will track how long a command was last
        private Timer movementTimer;

        //Will place intervals on when projectile will spawn
        private Timer shootTimer; 

        public AlienSpaceship(Texture2D texture, Vector2 startPosition, ObstacleType obstacleType = ObstacleType.AlienSpaceship) : base(texture, startPosition, obstacleType)
        {
            this.travelSpeed = 200;
            this.commandTimer = new Timer();
            this.movementTimer = new Timer();
            this.shootTimer = new Timer();


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
            

            CheckObjectAtEdge();
            this.collisionCircle.Centre = this.Position + new Vector2(16, 16);

            //GameObjectSpawner.CreateEnemyProjectile(this.ObjectTexture, CurrentPosition, false);
        }

        private void CheckTimer()
        {

        }



        public override void CheckObjectAtEdge()
        {

            if (CurrentPosition.X < 0)
            {
                SetPositionAtEdgeOfScreen(0);
                isMovingLeft = false;
            }

            else if (CurrentPosition.X + Texture.Width / 2 > Game1.ScreenWidth - Texture.Width / 2)
            {
                SetPositionAtEdgeOfScreen(Game1.ScreenWidth - Texture.Width);
                isMovingLeft = true;
            }
        }



    }
}
