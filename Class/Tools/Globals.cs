using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Drifter.Class.Factory;
using Drifter.Class.GameObjectClass;
using Drifter.Class.GameObjectClass.ItemClass;
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

        public static Random Random = new Random();
        public static void Update(GameTime gt, Player player = null)
        {
            GameTime = gt;

            if(player is not null)
            {
                if (player.IsAlive)
                {
                    //checks player projectile collides with obstacles
                    foreach (Obstacle o in GameObjectSpawner.obstacles)
                    {
                        if (o is SpacePipe)
                        {
                            SpacePipe sp = (SpacePipe)o;
                            if (sp.leftPipeCollisionSquare.Intersects(player.collisionCircle) || sp.rightPipeCollisionSquare.Intersects(player.collisionCircle))
                            {
                                player.CollidedWithOtherGameObject(sp);
                            }

                        }
                        else if (o is BlackHole)
                        {
                            BlackHole blackHole = (BlackHole)o;
                            if (blackHole.pullPlayerCircle.Intersects(player.collisionCircle))
                            {
                                blackHole.CollidedWithOtherGameObject(player);
                            }
                            if (blackHole.collisionCircle.Intersects(player.collisionCircle))
                            {
                                player.CollidedWithOtherGameObject(blackHole);
                            }
                        }
                        else if (player.collisionCircle.Intersects(o.collisionCircle))
                        {
                            o.CollidedWithOtherGameObject();
                            player.CollidedWithOtherGameObject(o);
                        }

                        foreach (Projectile p in GameObjectSpawner.projectiles)
                        {
                            if (o.collisionCircle.Intersects(p.collisionCircle))
                            {
                                p.CollidedWithOtherGameObject();
                                o.CollidedWithOtherGameObject(p);
                            }
                        }

                    }

                    foreach (Item i in GameObjectSpawner.items)
                    {
                        if (i.collisionCircle.Intersects(player.collisionCircle))
                        {
                            player.CollidedWithOtherGameObject(i);
                            i.CollidedWithOtherGameObject();
                        }
                    }

                    foreach (Projectile enemyProjectile in GameObjectSpawner.enemyProjectiles)
                    {
                        foreach (Projectile p in GameObjectSpawner.projectiles)
                        {
                            if (enemyProjectile.collisionCircle.Intersects(p.collisionCircle))
                            {
                                enemyProjectile.CollidedWithOtherGameObject();
                                p.CollidedWithOtherGameObject(enemyProjectile);
                            }
                        }

                        if (enemyProjectile.collisionCircle.Intersects(player.collisionCircle))
                        {
                            enemyProjectile.CollidedWithOtherGameObject(player);
                            player.CollidedWithOtherGameObject(enemyProjectile);
                        }
                    }
                }
            }
           
              
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
            GapSize = 32 * 3;
        }

        private static void LoadProjectileTextures()
        {
            textures.Add("projectileMissile", Content.Load<Texture2D>("Projectile/Missile"));
            textures.Add("enemyProjectile", Content.Load<Texture2D>("Projectile/EnemyProjectile"));
            textures.Add("laser", Content.Load<Texture2D>("Projectile/Laser"));
            textures.Add("laserBeam", Content.Load<Texture2D>("Projectile/Laser"));
        }

        private static void LoadItemTextures()
        {
            textures.Add("coin", Content.Load<Texture2D>("Item/Coin/Coin"));
            textures.Add("invincibility", Content.Load<Texture2D>("Item/Invincibility/Invincibility"));
            textures.Add("infiniteMissiles", Content.Load<Texture2D>("Item/Projectile/InfiniteMissiles"));
            textures.Add("laserItem", Content.Load<Texture2D>("Item/Projectile/Laser"));
            textures.Add("laserBeamItem", Content.Load<Texture2D>("Item/Projectile/LaserBeam"));
            textures.Add("superNova", Content.Load<Texture2D>("Item/Supernova/Supernova"));
        }

        private static void LoadObstacleTextures()
        {
            textures.Add("ball", Content.Load<Texture2D>("CollisionShape/ball"));
            textures.Add("asteroid", Content.Load<Texture2D>("Obstacle/Asteroid/Asteroid"));

            //SpacePipe Texture
            textures.Add("pipeBody", Content.Load<Texture2D>("Obstacle/SpacePipe/SpacePipeBody"));
            textures.Add("pipeHeadLeft", Content.Load<Texture2D>("Obstacle/SpacePipe/SpacePipeHeadLeft"));
            textures.Add("pipeHeadRight", Content.Load<Texture2D>("Obstacle/SpacePipe/spacePipeHeadRight"));
            textures.Add("blackHole", Content.Load<Texture2D>("Obstacle/BlackHole/BlackHole"));
            textures.Add("purpleAlienSpaceship", Content.Load<Texture2D>("Obstacle/AlienSpaceship/PurpleAlienSpaceship"));
        }


        private static void SetTexturesForGameObjects()
        {
        }



    }
}
