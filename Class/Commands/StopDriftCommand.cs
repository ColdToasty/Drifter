using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Drifter.Class.AbstractClass;
using Drifter.Class.Commands;
using Drifter.Class.GameObjectClass;
using Microsoft.Xna.Framework;
using Drifter.Class.Tools;

namespace Drifter.Class.Commands
{
    internal class StopDriftCommand : Command
    {

        private Timer driftTimer = new Timer();

        public override void Execute<T>(GameTime gameTime, T gameObject)
        {
            if (driftTimer.Set)
            {
                if(Timer.CheckTimeReached(driftTimer, gameTime))
                {
                    StopDrift(gameObject as Player);
                    driftTimer.ResetTimer();
                }
            }
            else
            {
                StopDrift(gameObject as Player);
                driftTimer.SetStartTimeAndStopTime(gameTime, 5000);

            }
        }

        private void StopDrift(Player player)
        {
            player.isDrifting = false;
        }
    }
}
