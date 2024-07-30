using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Drifter.Class.GameObjectClass;
using Drifter.Class.GameObjectClass.ObstacleClass;
using Drifter.Class.AbstractClass;
using Drifter.Class.Tools;



namespace Drifter.Class.Factory
{

    //Spawns gameObjects as well as responsible for deleting them
    internal static class GameObjectSpawner
    {
        public static int SpawnXAxisRange { get; private set; }
        public static int SpawnYAxisRange { get; private set; }


        public static List<Projectile> projectiles = new List<Projectile>();
        public static List<Obstacle> obstacles = new List<Obstacle>();
        public static List<Item> items = new List<Item>();
        private static List<GameObject> objectsToBeDeleted = new List<GameObject>();
        public static List<Projectile> enemyProjectiles = new List<Projectile>();
        private static List<GameObject> objectsToBeAddedAfterLoop = new List<GameObject>();

        private static Timer addToListTimer = new Timer();

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
                    enemyProjectiles.Remove((Projectile)gameObject);
                    projectiles.Remove((Projectile)gameObject);
                }
                else if (gameObject is Item)
                {
                    items.Remove((Item)gameObject);
                }

            }
            objectsToBeDeleted.Clear();
        }


        public static void SetSpawnXAxisRange(int AxisRange)
        {
            SpawnXAxisRange = AxisRange;
        }

        public static void SetSpawnYAxisRange(int axisRange)
        {
            SpawnYAxisRange = axisRange;
        }


        public static void CreateProjectile(Texture2D projectileTexture, Vector2 startPosition, bool isMovingNegative, Projectile.ProjectileType projectileType = Projectile.ProjectileType.Missle)
        {
            AddToList(new Projectile(projectileTexture, startPosition, isMovingNegative, projectileType));
        }


        public static void CreateEnemyProjectile(Texture2D projectileTexture, Vector2 startPosition, bool isMovingNegative, Projectile.ProjectileType projectileType = Projectile.ProjectileType.EnemyProjectile)
        {
            AddToList(new Projectile(Globals.GetTexture("enemyProjectile"), startPosition, isMovingNegative, projectileType));
        }




        //used to create an obstacle based on probability
        public static void CreateObstacle(Texture2D texture, Obstacle.ObstacleType obstacleType = Obstacle.ObstacleType.Asteroid)
        {
            int spawnObstacle = Game1.Random.Next(11);
            if (spawnObstacle <=4)
            {
                //dictate which side of the screen to spawn
                int spawnXPosition = Game1.Random.Next(2);

                //if 0 then spawn left
                //if 1 then spawn right
                if (spawnXPosition != 0)
                {
                    //spawns right end of the screen
                    spawnXPosition = SpawnXAxisRange - 32;

                }
                //if number is less than equal to 1 then
                if (spawnObstacle == 0)
                {
                    CreateAlienSpaceship(texture, new Vector2(spawnXPosition, 64));
                }
                else
                {
                    //pick y spawn range
                    int spawnYPosition = Game1.Random.Next(SpawnYAxisRange);
                    CreateAngledAsteroid(texture, new Vector2(spawnXPosition, spawnYPosition));
                }

            }
            else
            {
                int spawnXPosition = Game1.Random.Next(32, SpawnXAxisRange - 32);
                AddToList(new ShatteringAsteroid(texture, new Vector2(spawnXPosition, 0), obstacleType));
            }
        }


        public static void CreateShatteringAsteroid(Texture2D texture, Vector2 spawnPosition)
        {
            AddToList(new ShatteringAsteroid(texture, spawnPosition));
        }

        public static void CreateSpacePipe()
        {
            int xSpawn = Game1.Random.Next(SpacePipe.leftHeadTexture.Width, Globals.ScreenWidth - (Globals.GapSize + SpacePipe.leftHeadTexture.Width));
            AddToList(new SpacePipe(new Vector2(xSpawn, -80)));
        }

        public static void CreateAlienSpaceship(Texture2D texture, Vector2 spawnPosition)
        {
            AddToList(new AlienSpaceship(Globals.GetTexture("purpleAlienSpaceship"), spawnPosition));
        }

        public static void CreateAngledAsteroid(Texture2D texture, Vector2 spawnPosition, bool setCustomDirection = false, bool moveLeft = false)
        {
            AddToList(new AngledAsteroid(texture, spawnPosition, setCustomDirection, moveLeft));
        }

        //Used if objects need to be added during a run time loop
        public static void AddToListAfterLoop(GameObject gameObject = null)
        {
            if (addToListTimer.Set)
            {
                if(Timer.CheckTimeReached(addToListTimer))
                {
                    foreach(GameObject o in objectsToBeAddedAfterLoop)
                    {
                        AddToList(o);
                    }
                    addToListTimer.ResetTimer();
                    objectsToBeAddedAfterLoop.Clear();
                }
            }
            else
            {
                addToListTimer.SetStartTimeAndStopTime(10);
                if(gameObject != null)
                {
                    if (!objectsToBeAddedAfterLoop.Contains(gameObject))
                    {
                        objectsToBeAddedAfterLoop.Add(gameObject);
                    }
                    
                }
            }
        }




        public static void CreateItem(Texture2D texture, Item.ItemType itemType = Item.ItemType.Coin)
        {
            int spawnObstacle = Game1.Random.Next(5);
            if (spawnObstacle <= 4)
            {
                int spawnXPosition = Game1.Random.Next(32, SpawnXAxisRange - 32);
                AddToList(new Item(texture, new Vector2(spawnXPosition, 0), itemType));
            }
        }   



            
            
    }
}