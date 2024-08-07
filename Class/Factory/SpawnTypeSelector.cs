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

        public static void Initialise()
        {

            if (isInitialised){ 
                return; 
            }

            obstacleSpawnTimer = new Timer();
            itemSpawnTimer = new Timer();
            ScoreObstacleChanceValues = new Dictionary<int, List<int>>()
            {
                { 10000, new List<int> { 1, 2, 4, 5 }},
                { 20000, new List<int> { 1, 2, 4, 5 }},
                { 30000, new List<int> { 1, 2, 4, 5 }},
                { 40000, new List<int> { 1, 2, 4, 5 }},
                { 50000, new List<int> { 1, 2, 4, 5 }}
            };

                ScoreItemChanceValues = new Dictionary<int, List<int>>()
            {
                { 10000, new List<int> { 1, 2, 4, 5 }},
                { 20000, new List<int> { 1, 2, 4, 5 }},
                { 30000, new List<int> { 1, 2, 4, 5 }},
                { 40000, new List<int> { 1, 2, 4, 5 }},
                { 50000, new List<int> { 1, 2, 4, 5 }}
            };


            ObstacleSpawnTimes = new Dictionary<int, int>()
            {
                { 10000, 6},
                { 20000, 5},
                { 30000, 5},
                { 40000, 3},
                { 50000, 2}
            };

            ItemSpawnTimes = new Dictionary<int, int>()
            {
                { 10000, 5},
                { 20000, 7},
                { 30000, 9},
                { 40000, 11},
                { 50000, 15}
            };

            isInitialised = true;
            
        }


        public static Obstacle.ObstacleType? ChooseObstacleType()
        {
            return null;
        }



        public static Item.ItemType? ChooseItemType()
        {
            return null;
        }
    }
}
