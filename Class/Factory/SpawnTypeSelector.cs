using Drifter.Class.GameObjectClass;
using Drifter.Class.GameObjectClass.ObstacleClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace Drifter.Class.Factory
{

    internal static class SpawnTypeSelector
    {
        private static Random random = new Random();

        private static int[] obstacleTypeChanceValues = { };
        private static int[] itemTypeSpawnChanceValues = { };



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
