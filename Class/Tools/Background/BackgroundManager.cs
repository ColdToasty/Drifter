using Drifter.Class.Factory;
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
        public Texture2D BackgroundTexture { get; init; }
        public Texture2D BackgroundPlanet { get; private set;}

        //used to rectangle size
        private int screenWidth;
        private int screenHeight;

        public Vector2 BackgroundTextureOnePosition { get { return backgroundTextureOnePosition; } }
        public Vector2 BackgroundTextureTwoPosition { get { return backgroundTextureTwoPosition; } }

        public Vector2 BackgroundPlanetPosition { get { return backgroundPlanetPosition; } }


        private Vector2 backgroundTextureOnePosition, backgroundTextureTwoPosition, backgroundPlanetPosition;
        public enum RectangleToUse { RectangleOne, Both }

        public RectangleToUse rectangleToUse { get; private set; }

        private int backgroundTravelSpeed = 50;
        private int backgroundPlanetTravelSpeed = 30;
        private int chooseXPositionOfTexture;


        public bool PlanetSpawned { get; private set; }


        public BackgroundManager(Texture2D texture, int screenWidth, int screenHeight)
        {
            this.screenWidth = screenWidth;
            this.screenHeight = screenHeight;

            BackgroundTexture = texture;

            backgroundTextureOnePosition = new Vector2(chooseXPositionOfTexture, 0 - BackgroundTexture.Height + screenHeight);
            backgroundTextureTwoPosition = new Vector2(chooseXPositionOfTexture, backgroundTextureOnePosition.Y - BackgroundTexture.Height);

            PlanetSpawned = false;
        }

        private void SetTexturePosition(ref Vector2 texturePosition, ref Vector2 otherTexturePosition)
        {
            texturePosition.Y = otherTexturePosition.Y - BackgroundTexture.Height;
            texturePosition.X = chooseXPositionOfTexture;
        }

        private void ResetBackgroundPlanetPosition()
        {
            backgroundPlanetPosition = new Vector2(Globals.Random.Next(-20, screenWidth-5), GameObjectSpawner.GameObjectStartTop);
        }

        private void SelectPlanetTexture()
        {
            int planetValue = Globals.Random.Next(101);

            //nice earth like planet
            if(planetValue < 10)
            {
                BackgroundPlanet = Globals.GetTexture("planet1");
            }
            // green like planet
            else if(planetValue < 20)
            {
                BackgroundPlanet = Globals.GetTexture("planet2");
            }
            //ice planet
            else if (planetValue < 40)
            {
                BackgroundPlanet = Globals.GetTexture("planet3");
            }
            //saturn like planet
            else //if(planetValue < 60)
            {
                BackgroundPlanet = Globals.GetTexture("planet4");
            }
            //sun
     


        }

        public void Run()
        {
            if (PlanetSpawned)
            {
                backgroundPlanetPosition.Y += backgroundPlanetTravelSpeed * (float)Globals.GameTime.ElapsedGameTime.TotalSeconds;
            }

            if(backgroundPlanetPosition.Y > Globals.ScreenHeight)
            {
                ResetBackgroundPlanetPosition();
                PlanetSpawned = false;
            }

            //add timer
            if (Globals.Random.Next(101) <= 10 && !PlanetSpawned)
            {
                SelectPlanetTexture();
                ResetBackgroundPlanetPosition();
                PlanetSpawned = true;
            }


            backgroundTextureTwoPosition.Y += backgroundTravelSpeed * (float)Globals.GameTime.ElapsedGameTime.TotalSeconds;
            backgroundTextureOnePosition.Y += backgroundTravelSpeed * (float)Globals.GameTime.ElapsedGameTime.TotalSeconds;

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
