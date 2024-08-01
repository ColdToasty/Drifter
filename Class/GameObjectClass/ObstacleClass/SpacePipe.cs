using Drifter.Class.Tools;
using Drifter.Class.Tools.CollisionShapes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Drifter.Class.GameObjectClass.ObstacleClass
{
    internal class SpacePipe : Obstacle
    {
        //4 time size of player
        public static int GapSize { get; } = Globals.GapSize;


        public CollisionSquare leftPipeCollisionSquare, rightPipeCollisionSquare;

        public static Texture2D leftHeadTexture, rightHeadTexture, bodyTexture;

        private List<int> bodyTextureXPositions;

        private int leftHeadPosition, rightHeadPosition;

        private int leftPipeCollisionWidth, rightPipeCollisionWidth;

        public int LeftHeadPosition { get { return leftHeadPosition; } }
        public int RightHeadPosition { get { return rightHeadPosition; } }

        //start position refers to the gap that player has to pass through
        public SpacePipe(Vector2 startPosition, ObstacleType obstacleType = ObstacleType.SpacePipe) : base(startPosition, obstacleType)
        {

            this.Position = startPosition;
            this.leftHeadPosition = (int)this.Position.X - leftHeadTexture.Width;
            this.rightHeadPosition = (int)this.Position.X + GapSize;

            this.bodyTextureXPositions = new List<int>();

            this.ObjectTexture = leftHeadTexture;
            this.travelSpeed = 75;
            PipeBodyAmountToSpawn();

        }

        //calculates how many bodyTextures to spawn at specific positions on both left and right sides
        private void PipeBodyAmountToSpawn()
        {
            float remainingSpaceLeft = this.Position.X - leftHeadTexture.Width;
            float remaingSpaceRight = Globals.ScreenWidth - (this.Position.X + GapSize);

            leftPipeCollisionWidth = (int)remainingSpaceLeft;
            rightPipeCollisionWidth = (int)remaingSpaceRight;


            System.Diagnostics.Trace.WriteLine($"ScreenWidth - { Globals.ScreenWidth}");
            System.Diagnostics.Trace.WriteLine($"leftPipeWidth - {this.Position.X}");
            System.Diagnostics.Trace.WriteLine($"rightPipeWidth - {rightPipeCollisionWidth}");
            System.Diagnostics.Trace.WriteLine($"GapSize - {GapSize}");
            System.Diagnostics.Trace.WriteLine($"{Globals.ScreenWidth - (this.Position.X + GapSize)}");
            System.Diagnostics.Trace.WriteLine($"");

            int leftAmountOfPipeBodiesToSpawn = (int)MathF.Ceiling(remainingSpaceLeft / bodyTexture.Width);
            int rightAmountOfPipeBodiesToSpawn = (int)MathF.Ceiling(remaingSpaceRight / bodyTexture.Width);


            int leftValue = (int)remainingSpaceLeft - bodyTexture.Width;
            for (int i = 0; i < leftAmountOfPipeBodiesToSpawn; i++)
            {
                bodyTextureXPositions.Add(leftValue);
                leftValue -= bodyTexture.Width;
            }

            int rightValue = (int)this.Position.X + GapSize + rightHeadTexture.Width;
            for (int i = 0; i < rightAmountOfPipeBodiesToSpawn; i++)
            {
                bodyTextureXPositions.Add(rightValue);
                rightValue += bodyTexture.Width;
            }

            leftPipeCollisionSquare = new CollisionSquare(new Vector2(0, this.Position.Y), this.Position.X, bodyTexture.Height);
            rightPipeCollisionSquare = new CollisionSquare(new Vector2((int)this.Position.X + GapSize, this.Position.Y), remaingSpaceRight, bodyTexture.Height);

            System.Diagnostics.Trace.WriteLine($"{rightPipeCollisionSquare.rectangle.Width}");
        }

        public int[] GetXPositions()
        {
            return bodyTextureXPositions.ToArray();
        }


        public static void SetTextures(Texture2D leftHeadTexture, Texture2D rightHeadTexture, Texture2D bodyTexture)
        {
            SpacePipe.leftHeadTexture = leftHeadTexture;
            SpacePipe.rightHeadTexture = rightHeadTexture;
            SpacePipe.bodyTexture = bodyTexture;
        }

        public override void Run(bool isMovingNegative, float EndOfScreenPosition)
        {
            Position.Y += travelSpeed * (float)Globals.GameTime.ElapsedGameTime.TotalSeconds;


            leftPipeCollisionSquare.UpdateRectangle(Position);
            rightPipeCollisionSquare.UpdateRectangle(Position);


            //System.Diagnostics.Trace.WriteLine(rightPipeCollisionSquare.rectangle.Width);
            DidExitScreen(EndOfScreenPosition);
        }



    }
}
