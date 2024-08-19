using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Drifter.Class.Tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Drifter.Class.Tools
{
    internal class AnimationPlayer
    {
        private Timer timer;

        //how long until next frame in milliseconds
        private int frameThreshold = 200;

        private int previousFrameIndex;
        private int currentFrameIndex = -1;

        public Rectangle CurrentRectangleLocation { get; private set; }

        private Texture2D texture { get; init; }
        public Texture2D Texture2D { get { return texture; } }

        private int framesPerRow;
        private int framesPerColumn;

        private int frameWidth;
        private int frameHeight;
        //keeps track of the start of each animation
        private Dictionary<string, int> spriteAnimationRowLocation = new Dictionary<string, int>();

        private bool initialised = false;

        private int startOfFrameLoop, endOfFrameLoop;

        public AnimationPlayer(Texture2D texture, int framesPerColumn, int framesPerRow)
        {
            this.texture = texture;
            this.timer = new Timer();
            //startX startY, width, height
            this.framesPerRow = framesPerRow;
            this.framesPerColumn = framesPerColumn;

            this.frameWidth = texture.Width / framesPerRow;
            this.frameHeight = texture.Height / framesPerColumn;
            timer.SetStartTimeAndStopTime(frameThreshold);
        }



        //If spriteSheet has empty frames
        public void SetAnimationFramesRowLocations(string frameAnimationName, int row)
        {
            spriteAnimationRowLocation.Add(frameAnimationName, row);
        }

        public void UpdateFrameIndex(bool loop)
        {
            if (Timer.CheckTimeReached(timer))
            {
                if (loop)
                {
                    if (currentFrameIndex == framesPerRow - 1 || currentFrameIndex == -1)
                    {
                        currentFrameIndex = 0;
                    }
                    else
                    {
                        currentFrameIndex++;
                    }
                }
                else
                {
                    if (currentFrameIndex == framesPerRow - 1)
                    {
                        return;
                    }
                    else
                    {
                        currentFrameIndex++;
                    }
                }
                timer.UpdateTimers();
            }
        }


        public void SetCustomLoopFrame(int startOfFrameLoop, int endOfFrameLoop = -1)
        {
            this.startOfFrameLoop = startOfFrameLoop;

            if(endOfFrameLoop != -1)
            {
                this.endOfFrameLoop = endOfFrameLoop;
            }
        }


        public void SetFrameThreshHold(int newTimeInMilliseconds)
        {
            timer.SetStartTimeAndStopTime(newTimeInMilliseconds);
        }


        public void Play(string animationName, bool loop = false)
        {
            if(animationName == "")
            {
                return;
            }
            UpdateFrameIndex(loop);
            CurrentRectangleLocation = new Rectangle((int)currentFrameIndex * frameWidth, spriteAnimationRowLocation[animationName] * frameHeight, frameWidth, frameHeight);

        }

    }
}
