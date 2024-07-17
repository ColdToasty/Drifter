using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drifter.Class.Tools
{
    internal static class Score
    {
        private static int score = 0;

        public static int ScoreValue { get { return score; } }

        public static void IncreaseScore(int value)
        {
            score += value;
        }
        
        public static void Reset()
        {
            score = 0;
        }


    }
}
