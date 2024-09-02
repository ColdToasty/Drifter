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
using Microsoft.Xna.Framework.Audio;
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


        private static Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D>();

        public static Texture2D GetTexture(string textureName)
        {
            if (textures.ContainsKey(textureName))
            {
                return textures[textureName];
            }
            return null;
        }

        private static Dictionary<string, SoundEffect> soundEffects = new Dictionary<string, SoundEffect>();

        public static SoundEffect GetSoundEffect(string soundEffectName)
        {
            if (soundEffects.ContainsKey(soundEffectName))
            {
                return soundEffects[soundEffectName];
            }

            return null;
            
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
            LoadBackgroundTextures();
            LoadSoundEffects();

            //SetTexturesForGameObjects();
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


        private static void LoadBackgroundTextures()
        {
            textures.Add("background", Content.Load<Texture2D>("Background/Background1"));

            textures.Add("planet1", Content.Load<Texture2D>("Background/PointOfInterest/Planet1"));
            textures.Add("planet2", Content.Load<Texture2D>("Background/PointOfInterest/Planet2"));
            textures.Add("planet3", Content.Load<Texture2D>("Background/PointOfInterest/Planet3"));
            textures.Add("planet4", Content.Load<Texture2D>("Background/PointOfInterest/Planet4"));
        }

        private static void LoadSoundEffects()
        {
            soundEffects.Add("coin", Content.Load<SoundEffect>("SoundEffect/Item/PickUpCoin"));

            soundEffects.Add("powerUp1", Content.Load<SoundEffect>("SoundEffect/Item/PowerUp1"));
            soundEffects.Add("powerUp2", Content.Load<SoundEffect>("SoundEffect/Item/PowerUp2"));

            soundEffects.Add("explosion1", Content.Load<SoundEffect>("SoundEffect/Explosion/Explosion1"));
            soundEffects.Add("playerExplosion", Content.Load<SoundEffect>("SoundEffect/Explosion/PlayerExplosion"));

            soundEffects.Add("enemyShoot", Content.Load<SoundEffect>("SoundEffect/Shoot/EnemyShoot"));
            soundEffects.Add("playerShoot", Content.Load<SoundEffect>("SoundEffect/Shoot/PlayerShoot"));

            soundEffects.Add("blackHole1", Content.Load<SoundEffect>("SoundEffect/Obstacle/BlackHole/BlackHole1"));
        }

        private static void SetTexturesForGameObjects()
        {

        }

        private static void PlayExplosion()
        {
            GetSoundEffect("explosion1")?.Play();
        }

        public static void Update(GameTime gt, Player player = null)
        {
            GameTime = gt;

            if (player is not null)
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
                                PlayExplosion();
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
                                PlayExplosion();
                            }
                        }
                        else if (player.collisionCircle.Intersects(o.collisionCircle))
                        {
                            o.CollidedWithOtherGameObject();
                            player.CollidedWithOtherGameObject(o);
                            PlayExplosion();
                        }

                        foreach (Projectile p in GameObjectSpawner.projectiles)
                        {
                            if (o.collisionCircle.Intersects(p.collisionCircle))
                            {
                                p.CollidedWithOtherGameObject();
                                o.CollidedWithOtherGameObject(p);
                                PlayExplosion();
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
                                PlayExplosion();
                            }
                        }

                        if (enemyProjectile.collisionCircle.Intersects(player.collisionCircle))
                        {
                            enemyProjectile.CollidedWithOtherGameObject(player);
                            player.CollidedWithOtherGameObject(enemyProjectile);
                            PlayExplosion();
                        }
                    }
                }
            }


        }



    }
}
