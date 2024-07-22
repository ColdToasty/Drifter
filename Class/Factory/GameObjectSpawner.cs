using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Drifter.Class.GameObjectClass;
using Drifter.Class.GameObjectClass.ObstacleClass;
using Drifter;
using Drifter.Class.AbstractClass;
namespace Drifter.Class.Factory
{

    //Spawns gameObjects as well as responsible for deleting them
    internal static class GameObjectSpawner
    {
        public static int SpawnAxisRange { get; set; }

        private static Random random = new Random();

        public static List<Projectile> projectiles = new List<Projectile>();
        public static List<Obstacle> obstacles = new List<Obstacle>();
        public static List<Item> items = new List<Item>();
        private static List<GameObject> objectsToBeDeleted = new List<GameObject>();
        public static List<Projectile> enemyProjectiles = new List<Projectile>();

        public static void AddToList<TGameObject>(TGameObject gameObject) where TGameObject : GameObject
        {


            if (gameObject is Projectile)
            {
                if ((gameObject as Projectile).TypeOfProjectile != Projectile.ProjectileType.EnemyProjectile)
                {
                    projectiles.Add(gameObject as Projectile);
                }
                else
                {
                    enemyProjectiles.Add(gameObject as Projectile);
                }
            }

            else if (gameObject is Obstacle)
            {
                obstacles.Add(gameObject as Obstacle);
            }
            else if (gameObject is Item)
            {
                items.Add(gameObject as Item);

            }

            else
            {
                //System.Diagnostics.Trace.WriteLine("Null");
            }
        }



        public static void AddToDeleteList(GameObject gameObject)
        {
            objectsToBeDeleted.Add(gameObject);
        }


        //Deletes gameObjects by removing them from their respective list and setting them to null
        //Also resets the objectsToBeDeleted list at the end
        public static void DeleteGameObjects()
        {
            for (int i = 0; i < objectsToBeDeleted.Count; i++)
            {
                GameObject gameObject = objectsToBeDeleted[i];

                if (gameObject is Obstacle)
                {
                    obstacles.Remove((Obstacle)gameObject);
                }
                else if (gameObject is Projectile)
                {
                    projectiles.Remove((Projectile)gameObject);
                }
                else if (gameObject is Item)
                {
                    items.Remove((Item)gameObject);
                }
            }
            objectsToBeDeleted.Clear();
        }

        public static void SetSpawnAxisRange(int AxisRange)
        {
            SpawnAxisRange = AxisRange;
        }

        public static void CreateProjectile(Texture2D projectileTexture, Vector2 startPosition, bool isMovingNegative, Projectile.ProjectileType projectileType = Projectile.ProjectileType.Missle)
        {
            AddToList(new Projectile(projectileTexture, startPosition, isMovingNegative, projectileType));
        }


        public static void CreateEnemyProjectile(Texture2D projectileTexture, Vector2 startPosition, bool isMovingNegative, Projectile.ProjectileType projectileType = Projectile.ProjectileType.EnemyProjectile)
        {
            AddToList(new Projectile(projectileTexture, startPosition, isMovingNegative, projectileType));
        }


        public static void CreateObstacle(Texture2D texture, Obstacle.ObstacleType obstacleType = Obstacle.ObstacleType.Asteroid)
        {
            int spawnObstacle = random.Next(11);
            if (spawnObstacle <= 1)
            {
                int spawnXPosition = random.Next(2);
                if(spawnXPosition == 0)
                {
                    AddToList(new AlienSpaceship(texture, new Vector2(spawnXPosition, 64), obstacleType));
                }
                else
                {
                    AddToList(new AlienSpaceship(texture, new Vector2(SpawnAxisRange-32, 64), obstacleType));
                }
            }
            else
            {
                int spawnXPosition = random.Next(32, SpawnAxisRange - 32);
                AddToList(new Obstacle(texture, new Vector2(spawnXPosition, 0), obstacleType));
            }
        }

        public static void CreateItem(Texture2D texture, Item.ItemType itemType = Item.ItemType.Coin)
        {
            int spawnObstacle = random.Next(5);
            if (spawnObstacle <= 4)
            {
                int spawnXPosition = random.Next(32, SpawnAxisRange - 32);
                AddToList(new Item(texture, new Vector2(spawnXPosition, 0), itemType));
            }
        }   



            
            
    }
}