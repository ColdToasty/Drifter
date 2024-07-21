﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Drifter.Class.AbstractClass;
using Drifter.Class.GameObjectClass;
using Drifter.Class.GameObjectClass.ObstacleClass;
using Microsoft.Xna.Framework;


namespace Drifter.Class.Commands
{
    internal class MoveLeftCommand : Command
    {

        public override void Execute<T>(GameTime gameTime, T gameObject)
        {
            GameObject GameObject = gameObject as GameObject;
            GameObject.Run(gameTime, true);
            GameObject.isMovingLeft = true;

            if (gameObject is Player)
            {
                Player player = (GameObject as Player);
                player.isDrifting = true;
            }
            else if(gameObject is AlienSpaceship)
            {
                AlienSpaceship spaceship = GameObject as AlienSpaceship;
            }

        }
    }
}
