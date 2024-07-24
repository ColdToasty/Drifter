using Drifter.Class.GameObjectClass;
using Drifter.Class.GameObjectClass.ObstacleClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Drifter.Class;


namespace Drifter.Class.Factory
{

    internal static class SpawnTypeSelector
    {
        private static Random random = new Random();

        private static Dictionary<int, List<int>> ScoreObstacleChanceValues;

        private static Dictionary<int, List<int>> ScoreItemChanceValues;

        private static bool isInitialised = false;

        public static void Initialise()
        {

            if (isInitialised){ 
                return; 
            }


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
