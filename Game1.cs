
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Drifter.Class;
using Drifter.Class.Factory;
using Drifter.Interface;
using System;
using System.Collections.Generic;
using static Drifter.Class.Obstacle;

namespace Drifter
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Texture2D playerTexture, projectileMissile, projectileLaser;
        private Texture2D obstacleAsteroid;

        private Vector2 playerPosition;

        private Player player;

        private ProjectileSpawner projectileSpawner;

        private ObstacleSpawner obstacleSpawner;

        private List<Projectile> projectiles = new List<Projectile>();
        private List<Obstacle> obstacles= new List<Obstacle>();
        private List<Item> items = new List<Item>();

        private static int BottomEndPosition, TopEndPosition;

        private Timer shootTimer;
        private bool canShoot;


        private int previousTimeInSeconds;

        private Random random;

        public static int ScreenWidth { get; private set; }

        private Texture2D ball;
        private Obstacle ob;
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            playerPosition = new Vector2(_graphics.PreferredBackBufferWidth /2, _graphics.PreferredBackBufferHeight - _graphics.PreferredBackBufferHeight /4);


            ScreenWidth = _graphics.PreferredBackBufferWidth;
            player = new Player("DefaultPlayer", playerPosition);
            projectileSpawner = new ProjectileSpawner();
            obstacleSpawner = new ObstacleSpawner(_graphics.PreferredBackBufferWidth);

            BottomEndPosition = _graphics.PreferredBackBufferHeight;
            TopEndPosition = 0;
            canShoot = true;
            previousTimeInSeconds = 0;
            shootTimer = new Timer();
            random = new Random();

            ob = new Obstacle(obstacleAsteroid , new Vector2(_graphics.PreferredBackBufferWidth / 2, _graphics.PreferredBackBufferHeight - _graphics.PreferredBackBufferHeight / 4 + 64));
        }


        //Initialize required services and load any non-graphic related content.
        protected override void Initialize()
        {
            base.Initialize();
        }

        //Load main game content
        //only used once
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            playerTexture = Content.Load<Texture2D>("DefaultPlayer");
            ball = Content.Load<Texture2D>("ball");
            projectileMissile = Content.Load<Texture2D>("Missile");
            obstacleAsteroid = Content.Load<Texture2D>("Asteroid");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            if((int)gameTime.TotalGameTime.TotalSeconds - previousTimeInSeconds >= 1)
            {
                int spawnObstacle = random.Next(11);
                if(spawnObstacle <= 4)
                {
                    AddToList(obstacleSpawner.CreateObstacle(obstacleAsteroid));
                }
                Score.IncreaseScore(1);
                previousTimeInSeconds = (int)gameTime.TotalGameTime.TotalSeconds;
            }


            var kstate = Keyboard.GetState();

            if (kstate.IsKeyDown(Keys.Left) || kstate.IsKeyDown(Keys.A))
            {
                player.Run(gameTime, true);
            }

            CheckPlayerAtTheEdge();

            if (kstate.IsKeyDown(Keys.Right) || kstate.IsKeyDown(Keys.D))
            {
                player.Run(gameTime, false);
            }

            
            if (kstate.IsKeyDown(Keys.Space))
            {
                if (shootTimer.Set)
                {
                    canShoot = Timer.CheckTimeReached(shootTimer, gameTime);
                    if (canShoot)
                    {
                        ShootProjectile();
                        shootTimer.ResetTimer();
                    }
                }
                else
                {
                    shootTimer.SetStartTimeAndStopTime(gameTime, 500);
                    ShootProjectile();
                }

            }


            RunObjects(gameTime);
            CheckCollision();
            base.Update(gameTime);
        }

        private void ShootProjectile()
        {
            if (!player.ItemPickedUp)
            {
                AddToList(projectileSpawner.CreateProjectile(projectileMissile, player.CurrentPosition));

            }
        }

        private void AddToList<TGameObject>(TGameObject gameObject) where TGameObject : GameObject
        {
            if(gameObject is Projectile)
            {
                projectiles.Add(gameObject as Projectile);
            }

            else if(gameObject is Obstacle)
            {
                obstacles.Add(gameObject as Obstacle);
            }
        }


        private void CheckPlayerAtTheEdge()
        {
            if (player.CurrentPosition.X < playerTexture.Width / 2)
            {
                player.SetPositionAtEdgeOfScreen(0 + playerTexture.Width /2);
            }

            else if (player.CurrentPosition.X > _graphics.PreferredBackBufferWidth - playerTexture.Width / 2)
            {
                player.SetPositionAtEdgeOfScreen(_graphics.PreferredBackBufferWidth - playerTexture.Width / 2);
            }
        }


        private void RunObjects(GameTime gameTime)
        {
            for(int i = projectiles.Count - 1; i >= 0; i--)
            {
                projectiles[i].Run(gameTime, false);
            }
            for(int i = obstacles.Count - 1; i >= 0;i--)
            {
                obstacles[i].Run(gameTime, false);
            }
        }

        private void CheckCollision()
        {
            foreach(Obstacle o in obstacles)
            {
                if (player.collisionCircle.Intersects(o.collisionCircle))
                {
                    System.Diagnostics.Trace.WriteLine("Yes");
                }
            }
        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();
            _spriteBatch.Draw(
                playerTexture,
                player.CurrentPosition,
                null,
                Color.White,
                0f,
                new Vector2(playerTexture.Width / 2, playerTexture.Height / 2),
                Vector2.One,
                SpriteEffects.None,
                0f
            );

            _spriteBatch.Draw(
                obstacleAsteroid,
                ob.CurrentPosition,
                null,
                Color.White,
                0f,
                new Vector2(playerTexture.Width / 2, playerTexture.Height / 2),
                Vector2.One,
                SpriteEffects.None,
                0f
            );

            _spriteBatch.Draw(
            ball,
            player.collisionCircle.Centre,
            null,
            Color.White,
            0f,
            new Vector2(playerTexture.Width / 2, playerTexture.Height / 2),
            Vector2.One,
            SpriteEffects.None,
            0f
        );

            foreach (Projectile p in  projectiles)
            {
                _spriteBatch.Draw(
                p.Texture,
                p.CurrentPosition,
                null,
                Color.White,
                0f,
                new Vector2(playerTexture.Width/ 2, playerTexture.Height),
                Vector2.One,
                SpriteEffects.None,
                0f
            );
            }

            
            foreach (Obstacle o in obstacles)
            {
                _spriteBatch.Draw(
                o.Texture,
                o.CurrentPosition,
                null,
                Color.White,
                0f,
                new Vector2(0, 0),
                Vector2.One,
                SpriteEffects.None,
                0f
            );

                //draws their collisions shapes
                _spriteBatch.Draw(
                ball,
                o.collisionCircle.Centre,
                Color.White
                );
            }

            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
