using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drifter.Class.Tools
{
    internal static class Score
    {
        //Score max 999,999,999,999
        private static long score = 0;
        public static long ScoreValue { get { return score; } }

        public static void IncreaseScore(long value = 1)
        {
            if(score + value <= 999999999999)
            {
                score += value;
            }
        }
        

        public static void Reset()
        {
            score = 0;
        }


        public static void RecordScore()
        {

        }

    }   
}
