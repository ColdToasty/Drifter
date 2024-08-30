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
using Drifter.Class.GameObjectClass.ItemClass;
using System.Runtime.CompilerServices;



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

        public static void DeleteAllEnemies()
        {
            obstacles.Clear();
            enemyProjectiles.Clear();
        }


        
        public static int GameObjectStartTop { get; } = -80;

        private static Timer CoinSpawnTimer = new Timer();

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


        public static void CreateProjectile(Projectile.ProjectileType projectileType, Vector2 startPosition, bool isMovingNegative)
        {
            Texture2D projectileTexture;
            switch (projectileType)
            {
                case Projectile.ProjectileType.Missle:
                    projectileTexture = Globals.GetTexture("projectileMissile");
                    break;
                case Projectile.ProjectileType.Laser:
                    projectileTexture = Globals.GetTexture("laser");
                    break;
                case Projectile.ProjectileType.LaserBeam:
                    projectileTexture = Globals.GetTexture("laserBeam");
                    break;
                default:
                    projectileTexture = Globals.GetTexture("projectileMissile");
                    break;
            }
            AddToList(new Projectile(projectileTexture, startPosition, isMovingNegative, projectileType));
        }

        public static void CreateEnemyProjectile(Vector2 startPosition, bool isMovingNegative, Projectile.ProjectileType projectileType = Projectile.ProjectileType.EnemyProjectile)
        {
            AddToList(new Projectile(Globals.GetTexture("enemyProjectile"), startPosition, isMovingNegative, projectileType));
        }


        //used to create an obstacle based on probability
        public static void CreateObstacle(Obstacle.ObstacleType obstacleType = Obstacle.ObstacleType.Asteroid)
        {
            switch (obstacleType)
            {
                case Obstacle.ObstacleType.Asteroid:
                    CreateAsteroid(Globals.GetTexture("asteroid"), PickStartingPoint(obstacleType));
                    break;
                
                case Obstacle.ObstacleType.AngledAsteroid:
                    CreateAngledAsteroid(Globals.GetTexture("asteroid"), PickStartingPoint(obstacleType));
                    break;
                
                case Obstacle.ObstacleType.ShatteringAsteroid:
                    CreateShatteringAsteroid(Globals.GetTexture("asteroid"), PickStartingPoint(obstacleType));
                    break;
                
                case Obstacle.ObstacleType.AlienSpaceship:
                    CreateAlienSpaceship(Globals.GetTexture("purpleAlienSpaceship"), PickStartingPoint(obstacleType));
                    break;
                
                case Obstacle.ObstacleType.Blackhole:
                    CreateBlackHole(Globals.GetTexture("blackHole"), PickStartingPoint(obstacleType));
                    break;

                case Obstacle.ObstacleType.SpacePipe:
                    CreateSpacePipe(Globals.GetTexture("pipeBody"), Globals.GetTexture("pipeHeadLeft"), Globals.GetTexture("pipeHeadRight"));
                    break;

                case Obstacle.ObstacleType.SpaceWorm:
                    CreateSpaceWorm(Globals.GetTexture("asteroid"), PickStartingPoint(obstacleType));
                    break;
            }
        }

        private static Vector2 PickStartingPoint(Obstacle.ObstacleType obstacleType)
        {
            int x = 0;
            int y = 0;
            switch (obstacleType)
            {
                case Obstacle.ObstacleType.Asteroid:
                case Obstacle.ObstacleType.ShatteringAsteroid:
                case Obstacle.ObstacleType.SpaceWorm:
                    x = Globals.Random.Next(32, SpawnXAxisRange-32);
                    y = GameObjectStartTop;
                    break;

                case Obstacle.ObstacleType.AngledAsteroid:
                    x = ChooseEdge();
                    y = Globals.Random.Next(0, 64);
                    break;
                case Obstacle.ObstacleType.AlienSpaceship:
                    x = ChooseEdge();
                    y = 64;
                    break;

                case Obstacle.ObstacleType.Blackhole:
                    x = ChooseEdge();
                    y = GameObjectStartTop;
                    break;

            }
            return new Vector2(x, y);
        }

        //choose left or right edge
        private static int ChooseEdge()
        {
            int side = Globals.Random.Next(2);
            if (side == 0)
            {
                return 0;
            }
            else
            {
                return SpawnXAxisRange - 32;
            }
        }

        private static void CreateBlackHole(Texture2D texture, Vector2 spawnPosition)
        {
            AddToList(new BlackHole(texture, spawnPosition));
        }

        private static void CreateAsteroid(Texture2D texture, Vector2 spawnPosition)
        {
            AddToList(new Obstacle(texture, spawnPosition));
        }

        private static void CreateShatteringAsteroid(Texture2D texture, Vector2 spawnPosition)
        {
            AddToList(new ShatteringAsteroid(texture, spawnPosition));
        }

        public static void CreateAngledAsteroid(Texture2D texture, Vector2 spawnPosition, bool setCustomDirection = false, bool moveLeft = false)
        {
            AddToList(new AngledAsteroid(texture, spawnPosition, setCustomDirection, moveLeft));
        }

        private static void CreateSpacePipe(Texture2D pipeBody, Texture2D leftHead, Texture2D righthead)
        {
            int xSpawn = Globals.Random.Next(pipeBody.Width, Globals.ScreenWidth - (Globals.GapSize + pipeBody.Width));
            AddToList(new SpacePipe(pipeBody, leftHead, righthead, new Vector2(xSpawn, GameObjectStartTop)));
        }


        private static void CreateAlienSpaceship(Texture2D texture, Vector2 spawnPosition)
        {
            AddToList(new AlienSpaceship(texture, spawnPosition));
        }

        private static void CreateSpaceWorm(Texture2D texture, Vector2 spawnPosition)
        {
            AddToList(new SpaceWorm(texture, spawnPosition));
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


        public static void CreateItem(Item.ItemType itemType)
        {
            int spawnXPosition = Globals.Random.Next(32, SpawnXAxisRange - 32);
            AddToList(new Item(new Vector2(spawnXPosition, GameObjectStartTop), itemType));
        }


        public static void CreateCoin()
        {
            if (CoinSpawnTimer.Set)
            {
                if (Timer.CheckTimeReached(CoinSpawnTimer))
                    {
                        int spawnXPosition = Globals.Random.Next(32, SpawnXAxisRange - 32);
                        AddToList(new Item(new Vector2(spawnXPosition, GameObjectStartTop), Item.ItemType.Coin));
                        CoinSpawnTimer.ResetTimer();
                    }
            }
            else
            {
                int timeToSpawnCoin = Globals.Random.Next(1, 4);
                CoinSpawnTimer.SetStartTimeAndStopTime(timeToSpawnCoin * 1000);
            }
        }

    }
}