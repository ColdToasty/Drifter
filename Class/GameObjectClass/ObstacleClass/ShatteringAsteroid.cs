using Drifter.Class.AbstractClass;
using Drifter.Class.Factory;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drifter.Class.GameObjectClass.ObstacleClass
{
    internal class ShatteringAsteroid : Obstacle
    {
        public ShatteringAsteroid(Texture2D texture, Vector2 startPosition, ObstacleType obstacleType = ObstacleType.ShatteringAsteroid) : base(texture, startPosition, obstacleType)
        {

        }


        public override void CollidedWithOtherGameObject(GameObject gameObject = null)
        {
            GameObjectSpawner.CreateAngledAsteroid(this.Texture, CurrentPosition, true, true);
            GameObjectSpawner.CreateAngledAsteroid(this.Texture, CurrentPosition, true, false);
            base.CollidedWithOtherGameObject();
        }

    }
}
