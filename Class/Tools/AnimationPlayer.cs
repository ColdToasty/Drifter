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

        private int columns;
        private int rows;

        private int frameWidth;
        private int frameHeight;
        //keeps track of the start of each animation
        private Dictionary<string, int> spriteAnimationRowLocation = new Dictionary<string, int>();

        private bool initialised = false;
        private bool animationFinished = false;
        public bool AnimationFinished { get {return animationFinished; } }
        private int startOfFrameLoop, endOfFrameLoop;

        public AnimationPlayer(Texture2D texture, int rows, int columns)
        {
            this.texture = texture;
            this.timer = new Timer();
            //startX startY, width, height
            this.columns = columns;
            this.rows = rows;

            this.frameWidth = texture.Width / this.columns;
            this.frameHeight = texture.Height / this.rows;
            CurrentRectangleLocation = new Rectangle(0, 0, frameWidth, frameHeight);
            timer.SetStartTimeAndStopTime(frameThreshold);
        }



        //If spriteSheet has empty frames
        public void SetAnimationFramesRowLocations(string frameAnimationName, int row)
        {
            spriteAnimationRowLocation.Add(frameAnimationName, row);
        }

        public void SetAnimationFinished()
        {
            animationFinished = false;
        }

        public void UpdateFrameIndex(bool loop)
        {
            if (Timer.CheckTimeReached(timer))
            {
                if (loop)
                {
                    if (currentFrameIndex == columns - 1 || currentFrameIndex == -1)
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
                    if (currentFrameIndex == columns - 1)
                    {
                        animationFinished = true;
                        return;
                    }
                    else
                    {
                        if (currentFrameIndex == -1)
                        {
                            currentFrameIndex = 0;
                        }
                        else
                        {
                            currentFrameIndex++;
                        }
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

        public void SetRectangle(int xIndex, int yIndex)
        {
            CurrentRectangleLocation = new Rectangle(xIndex * frameWidth, yIndex * frameHeight, frameWidth, frameHeight);
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
