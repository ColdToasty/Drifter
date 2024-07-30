using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Drifter.Class.GameObjectClass.ObstacleClass;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Drifter.Class.Tools
{
    internal class Globals
    {

        public static GameTime GameTime { get; private set; }
        public static ContentManager Content { get; set; }
        public static SpriteBatch SpriteBatch { get; set; }
        public static GraphicsDevice GraphicsDevice { get; set; }

        public static int ScreenHeight { get; private set; }
        public static int ScreenWidth { get; private set; }

        public static int GapSize { get; set; }
        public static void Update(GameTime gt)
        {
            GameTime = gt;
        }

        private static Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D>();

        public static Texture2D? GetTexture(string textureName)
        {
            return textures?[textureName];
        }

        public static Texture2D CurrentPlayerTexture { get; private set; }
        public static Texture2D CurrentPlayerProjectile { get; private set; }
        public static Texture2D CurrentCollisionBallShape { get; private set; }

        public static void SetScreenMeasurements(int height, int width)
        {
            ScreenHeight = height;
            ScreenWidth = width;
        }

        public static void LoadContent()
        {
            LoadPlayerTextures();
            LoadItemTextures();
            LoadObstacleTextures();
            LoadProjectileTextures();
            SetTexturesForGameObjects();
        }

        private static void LoadPlayerTextures()
        {
            textures.Add("player", Content.Load<Texture2D>("Player/DefaultPlayer"));
            GapSize = textures["player"].Width * 3;
        }

        private static void LoadProjectileTextures()
        {
            textures.Add("projectileMissile", Content.Load<Texture2D>("Projectile/Missile"));
            textures.Add("enemyProjectile", Content.Load<Texture2D>("Projectile/EnemyProjectile"));
            textures.Add("laser", Content.Load<Texture2D>("Projectile/Laser"));
        }

        private static void LoadItemTextures()
        {
            textures.Add("coin", Content.Load<Texture2D>("Item/Coin/Coin"));
        }

        private static void LoadObstacleTextures()
        {
            textures.Add("ball", Content.Load<Texture2D>("CollisionShape/ball"));
            textures.Add("asteroid", Content.Load<Texture2D>("Obstacle/Asteroid/Asteroid"));

            //SpacePipe Texture
            textures.Add("spacePipeBody", Content.Load<Texture2D>("Obstacle/SpacePipe/SpacePipeBody"));
            textures.Add("spacePipeHeadLeft", Content.Load<Texture2D>("Obstacle/SpacePipe/SpacePipeHeadLeft"));
            textures.Add("spacePipeHeadRight", Content.Load<Texture2D>("Obstacle/SpacePipe/spacePipeHeadRight"));

            textures.Add("purpleAlienSpaceship", Content.Load<Texture2D>("Obstacle/AlienSpaceship/PurpleAlienSpaceship"));
        }


        private static void SetTexturesForGameObjects()
        {
            SpacePipe.SetTextures(textures["spacePipeHeadLeft"], textures["spacePipeHeadRight"], textures["spacePipeBody"]);
        }



    }
}
