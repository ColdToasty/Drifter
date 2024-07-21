using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace Drifter.Class.Tools

{
    //Only conserned with keeping track of time in milliseconds
    internal class Timer
    {

        private double? startTime;
        private double? endTime;

        public double? StartTime { get { return startTime;  } }
        public double? EndTime { get { return endTime; } }

        private bool set;

        public bool Set { get { return set; } } 

        public Timer()
        {
            set = false;
        }

        //Checks if timer time has reached or passed
        public static bool CheckTimeReached(GameTime gameTime, Timer timer)
        {
            
            if (gameTime.TotalGameTime.TotalMilliseconds >= timer.EndTime)
            {
                return true;
            }

            return false;
        }

        public void ResetTimer()
        {
            startTime = null;
            endTime = null;
            set = false;
        }

        //Set the startTime only
        public void SetStartTime(GameTime gameTime)
        {
            this.startTime = gameTime.TotalGameTime.TotalMilliseconds;
            set = true;
        }

        //Sets both the startTime and endTime
        public void SetStartTimeAndStopTime(GameTime gameTime, int milliseconds)
        {

            SetStartTime(gameTime);
            this.endTime = this.startTime + milliseconds;;
        }
        

    }
}
