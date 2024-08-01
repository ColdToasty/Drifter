using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drifter.Class.Tools
{
    internal static class CoinTracker
    {
        private static int coinsCollected = 0;

        public static int CoinsCollected { get { return CoinsCollected; } set { coinsCollected += value; } }



    }
}
