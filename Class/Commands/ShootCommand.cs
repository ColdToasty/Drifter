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

     public override void Execute<T>(T gameObject)
        {
           if(shootTimer.Set)
        {
                canShoot = Timer.CheckTimeReached(shootTimer);
                if (canShoot)
                {
                    shootTimer.ResetTimer();
                }
            }
        else
            {
                ShootProjectile(((gameObject as Player)));
                shootTimer.SetStartTimeAndStopTime(500);
                canShoot = false;
            }
        }
    
    private void ShootProjectile(Player player)
    {
        GameObjectSpawner.CreateProjectile(player.ProjectileType, player.CurrentPosition + new Vector2(0, 64), true);
    }

    }

}
