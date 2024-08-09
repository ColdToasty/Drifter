using Drifter.Class.GameObjectClass.ObstacleClass;
using System.Collections.Generic;
using Drifter.Class.GameObjectClass.ItemClass;
using Drifter.Class.Tools;
using System.Buffers.Text;

namespace Drifter.Class.Factory
{

    internal static class SpawnTypeSelector
    {

        private static Dictionary<int, List<int>> ScoreObstacleChanceValues;

        private static Dictionary<int, List<int>> ScoreItemChanceValues;

        private static Dictionary<int, int> ObstacleSpawnTimes;

        private static Dictionary<int, int> ItemSpawnTimes;

        private static bool isInitialised = false;

        private static Timer obstacleSpawnTimer = new Timer();
        private static Timer itemSpawnTimer = new Timer();
        private static Timer spacePipeSpawnCooldownTimer = new Timer();
        private static Timer blackHoleSpawnCooldownTimer = new Timer();
        private static Timer alienSpaceshipSpawnCooldownTimer = new Timer();

        private static int alienSpaceshipCooldownTime = 3000;
        private static int spacePipeCooldownTime = 8000;
        private static int blackHoleCooldownTime = 5000;
        
        private static int maxProbability = 1000;


        private static List<int> obstacleSpawnChances =  new List<int> { 200, 400, 600, 750, 875 };


        //Used to load variables after game starts
        public static void Initialise()
        {

            if (isInitialised){ 
                return; 
            }

            isInitialised = true;
            
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

            switch (obstacleSpawnChances[obstacleIndex])
            {
                //default never runs lol
                default:
                    typeToReturn = Obstacle.ObstacleType.Asteroid;
                    break;

                case 200:
                    typeToReturn = Obstacle.ObstacleType.AngledAsteroid;
                    break;

                case 400:
                    typeToReturn = Obstacle.ObstacleType.ShatteringAsteroid;
                    break;

                case 600:
                    typeToReturn = CheckCooldownTimeReached(alienSpaceshipSpawnCooldownTimer, Obstacle.ObstacleType.AlienSpaceship, alienSpaceshipCooldownTime);
                    break;

                case 750:
                    typeToReturn = CheckCooldownTimeReached(blackHoleSpawnCooldownTimer, Obstacle.ObstacleType.Blackhole, blackHoleCooldownTime);
                    break;

                case 875:
                    typeToReturn = CheckCooldownTimeReached(spacePipeSpawnCooldownTimer, Obstacle.ObstacleType.SpacePipe, spacePipeCooldownTime);
                    break;
            }
            return typeToReturn;
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
                return obType;
            }
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


        public static Item.ItemType? ChooseItemType()
        {
            return null;
        }



    }
}
