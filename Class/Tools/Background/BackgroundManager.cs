using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drifter.Class.Tools.Background
{
    internal class BackgroundManager
    {
        public Texture2D backgroundTexture { get; init; }

        //used to rectangle size
        private int screenWidth;
        private int screenHeight;


        public Vector2 BackgroundTextureOnePosition { get { return backgroundTextureOnePosition; } }
        public Vector2 BackgroundTextureTwoPosition { get { return backgroundTextureTwoPosition; } }


        private Vector2 backgroundTextureOnePosition, backgroundTextureTwoPosition;
        public enum RectangleToUse { RectangleOne, Both }

        public RectangleToUse rectangleToUse { get; private set; }


        private int travelSpeed = 50;

        private int chooseXPositionOfTexture;

        public BackgroundManager(Texture2D texture, int screenWidth, int screenHeight)
        {
            this.screenWidth = screenWidth;
            this.screenHeight = screenHeight;

            backgroundTexture = texture;


            backgroundTextureOnePosition = new Vector2(chooseXPositionOfTexture, 0 - backgroundTexture.Height + screenHeight);
            backgroundTextureTwoPosition = new Vector2(chooseXPositionOfTexture, backgroundTextureOnePosition.Y - backgroundTexture.Height);

        }

        private void SetTexturePosition(ref Vector2 texturePosition, ref Vector2 otherTexturePosition)
        {
            texturePosition.Y = otherTexturePosition.Y - backgroundTexture.Height;
            texturePosition.X = chooseXPositionOfTexture;
        }

        public void Run()
        {
            backgroundTextureTwoPosition.Y += travelSpeed * (float)Globals.GameTime.ElapsedGameTime.TotalSeconds;
            backgroundTextureOnePosition.Y += travelSpeed * (float)Globals.GameTime.ElapsedGameTime.TotalSeconds;

            if (backgroundTextureOnePosition.Y > screenHeight)
            {
                SetTexturePosition(ref backgroundTextureOnePosition, ref backgroundTextureTwoPosition);
            }
            else if (backgroundTextureTwoPosition.Y > screenHeight)
            {
                SetTexturePosition(ref backgroundTextureTwoPosition, ref backgroundTextureOnePosition);
            }
        }
    }
}
