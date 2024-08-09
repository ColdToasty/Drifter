using Drifter.Class.GameObjectClass.ObstacleClass;
using System.Collections.Generic;
using Drifter.Class.GameObjectClass.ItemClass;
using Drifter.Class.Tools;

namespace Drifter.Class.Factory
{

    internal static class SpawnTypeSelector
    {

        private static Dictionary<int, List<int>> ScoreObstacleChanceValues;

        private static Dictionary<int, List<int>> ScoreItemChanceValues;

        private static Dictionary<int, int> ObstacleSpawnTimes;

        private static Dictionary<int, int> ItemSpawnTimes;

        private static bool isInitialised = false;

        private static Timer obstacleSpawnTimer;
        private static Timer itemSpawnTimer;
        private static Timer coolDownTimer;

        private static int maxProbability = 1000;

        private static List<int> obstacleSpawnChances;
        public static void Initialise()
        {

            if (isInitialised){ 
                return; 
            }

            obstacleSpawnTimer = new Timer();
            itemSpawnTimer = new Timer();
            coolDownTimer = new Timer();

            obstacleSpawnChances = new List<int> { 300, 600, 700, 900, 950 };

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
                case 300:
                    typeToReturn = Obstacle.ObstacleType.AngledAsteroid;
                    break;
                case 600:
                    typeToReturn = Obstacle.ObstacleType.ShatteringAsteroid;
                    break;
                case 700:
                    typeToReturn =  Obstacle.ObstacleType.AlienSpaceship;
                    break;
                case 900:
                    typeToReturn = Obstacle.ObstacleType.Blackhole;
                    break;
                case 950:
                    typeToReturn =  Obstacle.ObstacleType.SpacePipe;
                    break;
                default:
                    typeToReturn = Obstacle.ObstacleType.Asteroid;
                    break;
            }
            return typeToReturn;
        }



        public static Item.ItemType? ChooseItemType()
        {
            return null;
        }



    }
}
