using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Drifter.Class.AbstractClass;
using Drifter.Class.GameObjectClass;
using Microsoft.Xna.Framework;


namespace Drifter.Class.Commands
{
    internal class MoveLeftCommand : Command
    {

        public override void Execute<T>(GameTime gameTime, T gameObject)
        {
            Player player = (gameObject as Player);
            player.Run(gameTime, true);
            player.isDrifting = true;
            player.isMovingLeft = true; ;
        }
    }
}
