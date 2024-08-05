using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Drifter.Class.Tools;
using Drifter.Class.Factory;
using Drifter.Class.GameObjectClass;
using Drifter.Class.AbstractClass;
using Microsoft.Xna.Framework.Graphics;
using Drifter.Class.GameObjectClass.ObstacleClass;

namespace Drifter.Class.Commands
{
    internal class ShootCommand : Command
    {

        private static Timer shootTimer = new Timer();
        private static bool canShoot = false;
        private Texture2D projectileMissile;
        public ShootCommand(Texture2D projectileMissile)
        {
            this.projectileMissile = projectileMissile;
        }

     public override void Execute<T>(T gameObject)
        {
           if(shootTimer.Set)
        {
                canShoot = Timer.CheckTimeReached(shootTimer);
                if (canShoot)
                {
                    shootTimer.ResetTimer();
                    GameObjectSpawner.CreateBlackHole();
                }
            }
        else
            {
                ShootProjectile(((gameObject as GameObject).CurrentPosition));
                shootTimer.SetStartTimeAndStopTime(500);
                canShoot = false;
            }
        }
    
    private void ShootProjectile(Vector2 playerPosition)
    {
        GameObjectSpawner.CreateProjectile(projectileMissile, playerPosition + new Vector2(0, 16), true);
    }

    }

}
