using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Drifter.Class.GameObjectClass;

namespace Drifter.Class.Factory
{
    internal class GameObjectSpawner
    {
        private int spawnAxisRange;
        private Random random;

        public GameObjectSpawner(int spawnAxisRange)
        {
            this.spawnAxisRange = spawnAxisRange;
            random = new Random();
        }

        public Projectile CreateProjectile(Texture2D projectileTexture, Vector2 startPosition,  Projectile.ProjectileType projectileType = Projectile.ProjectileType.Missle)
        {
            return new Projectile(projectileTexture, startPosition, projectileType);
        }

        public Obstacle CreateObstacle(Texture2D texture, Obstacle.ObstacleType obstacleType = Obstacle.ObstacleType.Asteroid)
        {
            int spawnXPosition = random.Next(32, spawnAxisRange - 32);
            return new Obstacle(texture, new Vector2(360, 0), obstacleType);
        }

        public Item? CreateItem(Texture2D texture, Item.ItemType itemType = Item.ItemType.Coin)
        {
            int spawnXPosition = random.Next(32, spawnAxisRange - 32);
            return new Item(texture, new Vector2(spawnXPosition, 0) , itemType);
        }


    }
}
