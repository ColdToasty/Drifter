﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Drifter.Class;

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

        private double interval;

        public Timer()
        {
            set = false;
        }



        //Checks if timer time has reached or passed
        public static bool CheckTimeReached(Timer timer)
        {
            if (!timer.set)
            {
                return false;
            }

            if (Globals.GameTime.TotalGameTime.TotalMilliseconds >= timer.EndTime)
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

        //Start the Timer
        public void SetStartTime()
        {
            if(Globals.GameTime is null)
            {
                this.startTime = 0;
            }
            else
            {
                this.startTime = Globals.GameTime.TotalGameTime.TotalMilliseconds;
            }

            set = true;
        }

        //Stops the timer
        public void StopTime(GameTime gameTime)
        {
            endTime = gameTime.TotalGameTime.TotalMilliseconds - startTime;
        }

        //Sets both the startTime and endTime
        public void SetStartTimeAndStopTime(int milliseconds)
        {
            SetStartTime();
            this.endTime = this.startTime + milliseconds;
            this.interval = milliseconds;
        }
        
        public void UpdateTimers()
        {
            this.startTime = Globals.GameTime.TotalGameTime.TotalMilliseconds;
            this.endTime = Globals.GameTime.TotalGameTime.TotalMilliseconds + this.interval;
        }

    }
}
