using Drifter.Class.GameObjectClass.ObstacleClass;
using System.Collections.Generic;
using Drifter.Class.GameObjectClass.ItemClass;
using Drifter.Class.Tools;
using System.Buffers.Text;
using System.ComponentModel.Design;
using Microsoft.Xna.Framework;

namespace Drifter.Class.Factory
{

    internal static class SpawnTypeSelector
    {

        private static bool isInitialised = false;

        private static Timer spacePipeSpawnCooldownTimer = new Timer();
        private static Timer blackHoleSpawnCooldownTimer = new Timer();
        private static Timer alienSpaceshipSpawnCooldownTimer = new Timer();

        private static int alienSpaceshipCooldownTime = 3000;
        private static int spacePipeCooldownTime = 8000;
        private static int blackHoleCooldownTime = 5000;
        
        private static int maxProbability = 1000;
        private static int previousTimeInSecondsForObstacleSpawn = 0; 
        private static int previousTimeInSecondsForItemSpawn = 0;


        private static List<int> obstacleSpawnChances =  new List<int> { 200, 400, 600, 750, 875 };



        //Invincibility, InfiniteMissiles, Laser, LaserBeam, SuperNova
        private static List<int> itemSpawnChances = new List<int> {100, 350, 550, 750, 800};


        //Used to load variables after game starts
        public static void Initialise()
        {

            if (isInitialised){ 
                return; 
            }

            isInitialised = true;
        
        }


        private static void SetSpawnChanceList()
        {
            // 000 000 000 000 - max score
            if(Score.ScoreValue < 000000025000)
            {

            }
            
            else if(Score.ScoreValue < 000000070000)
            {

            }

            else if(Score.ScoreValue < 000000115000)
            {

            }
            else if (Score.ScoreValue < 000000150000)
            {

            }
            else if(Score.ScoreValue < 000000200000)
            {

            }
            else if (Score.ScoreValue < 000000275000)
            {

            }
            else if(Score.ScoreValue < 000000350000)
            {

            }
        }

        public static Obstacle.ObstacleType ChooseObstacleType()
        {
            
            int obstacleIndex = 0;

            int randomValue = Globals.Random.Next(1, maxProbability + 1);

            Obstacle.ObstacleType typeToReturn; 
            do
            {
                //increases index if randomValue is greater than current index
                if (randomValue > obstacleSpawnChances[obstacleIndex])
                {

                    obstacleIndex++;
                }
                else
                {
                    break;
                }
            } while (obstacleIndex < obstacleSpawnChances.Count);

            if(obstacleIndex >= obstacleSpawnChances.Count)
            {
                obstacleIndex--;
            }

            switch (obstacleIndex)
            {
                //default never runs lol
                default:
                    typeToReturn = Obstacle.ObstacleType.Asteroid;
                    break;

                case 0:
                    typeToReturn = Obstacle.ObstacleType.AngledAsteroid;
                    break;

                case 1:
                    typeToReturn = Obstacle.ObstacleType.ShatteringAsteroid;
                    break;

                case 2:
                    typeToReturn = CheckCooldownTimeReached(alienSpaceshipSpawnCooldownTimer, Obstacle.ObstacleType.AlienSpaceship, alienSpaceshipCooldownTime);
                    break;

                case 3:
                    typeToReturn = CheckCooldownTimeReached(blackHoleSpawnCooldownTimer, Obstacle.ObstacleType.Blackhole, blackHoleCooldownTime);
                    break;

                case 4:
                    typeToReturn = CheckCooldownTimeReached(spacePipeSpawnCooldownTimer, Obstacle.ObstacleType.SpacePipe, spacePipeCooldownTime);
                    break;
            }

            return typeToReturn;
        }

        //Use to choose an asteroid if spacePipe || blackHole timer(s) are active
        private static Obstacle.ObstacleType ChooseRandomAsteroid()
        {
            int asteroidChoice = Globals.Random.Next(3);
            switch (asteroidChoice)
            {
                default:
                    return Obstacle.ObstacleType.Asteroid;
                case 1:
                    return Obstacle.ObstacleType.AngledAsteroid;
                case 2:
                    return Obstacle.ObstacleType.ShatteringAsteroid;
            }

        }

        private static Obstacle.ObstacleType CheckCooldownTimeReached(Timer timer,  Obstacle.ObstacleType obType,  int timeInMilliSeconds)
        {
            if (timer.Set)
            {
                if (Timer.CheckTimeReached(timer))
                {
                    timer.ResetTimer();
                    return obType;
                }
                else
                {
                    return ChooseRandomAsteroid();
                }
            }
            else
            {
                timer.SetStartTimeAndStopTime(timeInMilliSeconds);
                return ChooseRandomAsteroid();
            }
        }

        public static Item.ItemType ChooseItemType()
        {

            int itemIndex  = 0;
            int randomValue = Globals.Random.Next(1, maxProbability + 1);

            Item.ItemType typeToReturn;
            do
            {
                //increases index if randomValue is greater than current index
                if (randomValue > itemSpawnChances[itemIndex])
                {
                    itemIndex++;
                }
                else
                {
                    break;
                }
            } while (itemIndex < itemSpawnChances.Count);

            //if value > max value in itemSpawnChances then set it to spawn coin
            if (itemIndex >= itemSpawnChances.Count)
            {
                itemIndex = -1;
            }

            switch (itemIndex)
            {
                default:
                    typeToReturn = Item.ItemType.Coin;
                    break;
                case 0:
                    typeToReturn = Item.ItemType.Invincibility;
                    break;
                case 1:
                    typeToReturn = Item.ItemType.InfiniteMissiles;
                    break;
                case 2:
                    typeToReturn = Item.ItemType.Laser;
                    break;
                case 3:
                    typeToReturn = Item.ItemType.LaserBeam;
                    break;
                case 4:
                    typeToReturn = Item.ItemType.SuperNova;
                    break;
            }

            return typeToReturn;
        }


        //Creates the item and obstacle
        public static void CreateObstacleAndItem()
        {
            if ((int)Globals.GameTime.TotalGameTime.TotalSeconds - previousTimeInSecondsForObstacleSpawn >= 1)
            {
                Obstacle.ObstacleType ob = SpawnTypeSelector.ChooseObstacleType();
                GameObjectSpawner.CreateObstacle(ob);
                GameObjectSpawner.CreateObstacle(Obstacle.ObstacleType.Asteroid);
                previousTimeInSecondsForObstacleSpawn = (int)Globals.GameTime.TotalGameTime.TotalSeconds;
            }

            if ((int)Globals.GameTime.TotalGameTime.TotalSeconds - previousTimeInSecondsForItemSpawn >= 8)
            {
                Item.ItemType itemType = SpawnTypeSelector.ChooseItemType();
                GameObjectSpawner.CreateItem(itemType);
                previousTimeInSecondsForItemSpawn = (int)Globals.GameTime.TotalGameTime.TotalSeconds;
            }
        }

    }
}
