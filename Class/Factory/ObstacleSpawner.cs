using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
namespace SpaceRider.Class.Factory
{
    internal class ObstacleSpawner
    {
        private int spawnAxisRange;
        private Random random;

        public ObstacleSpawner(int spawnAxisRange)
        {
            this.spawnAxisRange = spawnAxisRange;
            random = new Random();
        }

        public Obstacle CreateObstacle(Texture2D texture, Obstacle.ObstacleType obstacleType = Obstacle.ObstacleType.Asteroid)
        {
            int spawnXPosition = random.Next(32, spawnAxisRange - 32);
            return new Obstacle(texture, new Vector2(spawnXPosition, 0), obstacleType);
        }
    }
}
