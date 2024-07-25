
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Drifter.Class.Factory;
using System;
using System.Collections.Generic;
using Drifter.Class.GameObjectClass;
using Drifter.Class.Tools;
using Drifter.Class.AbstractClass;
using Drifter.Class.Commands;
using Drifter.Class.GameObjectClass.ObstacleClass;


namespace Drifter
{
    public class Game1 : Game
    {
        private static GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public static int ScreenHeight { get; private set; }
        public static int ScreenWidth { get; private set; }

        private Random random;
        private Player player;
        private InputHandler inputHandler;

        private MoveLeftCommand moveLeftCommand;
        private MoveRightCommand moveRightCommand;
        private StopDriftCommand stopDriftCommand;

        private Texture2D playerTexture, projectileMissile, projectileLaser, obstacleAsteroid, ball, coin;

        private Vector2 playerPosition, textMiddlePoint;


        private SpriteFont spriteFont;

        private Timer shootTimer;
        private bool canShoot;


        private int previousTimeInSeconds;


        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            playerPosition = new Vector2(_graphics.PreferredBackBufferWidth /2, _graphics.PreferredBackBufferHeight - _graphics.PreferredBackBufferHeight /4);

            inputHandler = new InputHandler();
            moveLeftCommand = new MoveLeftCommand();
            moveRightCommand = new MoveRightCommand();
            stopDriftCommand = new StopDriftCommand();

            ScreenHeight = _graphics.PreferredBackBufferHeight;
            ScreenWidth = _graphics.PreferredBackBufferWidth;
            GameObjectSpawner.SetSpawnAxisRange(ScreenWidth);

            canShoot = true;
            previousTimeInSeconds = 0;
            shootTimer = new Timer();
            random = new Random();

            SpawnTypeSelector.Initialise();

        }


        //Initialize required services and load any non-graphic related content.
        protected override void Initialize()
        {
            base.Initialize();
        }

        //Load main game content
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            ball = Content.Load<Texture2D>("CollisionShape/ball");
            projectileMissile = Content.Load<Texture2D>("Projectile/Missile");
            obstacleAsteroid = Content.Load<Texture2D>("Obstacle/Asteroid/Asteroid");
            coin = Content.Load<Texture2D>("Item/Coin/Coin");

            spriteFont = Content.Load<SpriteFont>("Font/MainFont");
            textMiddlePoint = spriteFont.MeasureString(Score.ScoreValue.ToString("D10")) / 2;

            playerTexture = Content.Load<Texture2D>("Player/DefaultPlayer");
            player = new Player(playerTexture, playerPosition);
        }




        private void RunObjects(GameTime gameTime)
        {
            for (int i = GameObjectSpawner.projectiles.Count - 1; i >= 0; i--)
            {
                GameObjectSpawner.projectiles[i].Run(gameTime, GameObjectSpawner.projectiles[i].IsMovingNegative, ScreenHeight);
            }

            for (int i = GameObjectSpawner.obstacles.Count - 1; i >= 0; i--)
            {
                GameObjectSpawner.obstacles[i].Run(gameTime, GameObjectSpawner.obstacles[i].isMovingLeft, ScreenHeight);
            }

            for (int i = GameObjectSpawner.items.Count - 1; i >= 0; i--)
            {
                GameObjectSpawner.items[i].Run(gameTime, false, _graphics.PreferredBackBufferHeight);
            }

            for (int i = GameObjectSpawner.enemyProjectiles.Count - 1; i >= 0; i--)
            {
                GameObjectSpawner.enemyProjectiles[i].Run(gameTime, false, ScreenHeight);
            }
        }

        private void CheckCollision()
        {
            //checks player projectile collides with obstacles
            foreach (Obstacle o in GameObjectSpawner.obstacles)
            {
                if (player.collisionCircle.Intersects(o.collisionCircle))
                {
                    o.CollidedWithOtherGameObject();
                    //player.CollidedWithOtherGameObject();
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

            foreach(Item i in GameObjectSpawner.items)
            {
                if (i.collisionCircle.Intersects(player.collisionCircle))
                {
                    player.CollidedWithOtherGameObject(i);
                    i.CollidedWithOtherGameObject();
                }
            }

            foreach(Projectile enemyProjectile in GameObjectSpawner.enemyProjectiles)
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
                }
            }
        }





        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            if((int)gameTime.TotalGameTime.TotalSeconds - previousTimeInSeconds >= 1)
            {
                GameObjectSpawner.CreateObstacle(obstacleAsteroid);
                GameObjectSpawner.CreateItem(coin);
                previousTimeInSeconds = (int)gameTime.TotalGameTime.TotalSeconds;
            }

            Score.IncreaseScore(1);

            CheckPlayerInput(gameTime);

            RunObjects(gameTime);
            CheckCollision();
            GameObjectSpawner.DeleteGameObjects();

            //System.Diagnostics.Trace.WriteLine(obstacles.Count);
            //System.Diagnostics.Trace.WriteLine(projectiles.Count);
            
            base.Update(gameTime);
        }

        private void ShootProjectile()
        {
           GameObjectSpawner.CreateProjectile(projectileMissile, player.CurrentPosition, true);
        }


        private void CheckPlayerInput(GameTime gameTime)
        {
            var kstate = Keyboard.GetState();

            if (kstate.IsKeyDown(Keys.Left) || kstate.IsKeyDown(Keys.A))
            {
                inputHandler.Command = moveLeftCommand;
            }

            if (kstate.IsKeyDown(Keys.Right) || kstate.IsKeyDown(Keys.D))
            {

                inputHandler.Command = moveRightCommand;
            }


            if(kstate.IsKeyDown(Keys.Down) || kstate.IsKeyDown(Keys.S))
            {
                inputHandler.Command = stopDriftCommand;
            }

            if (kstate.IsKeyDown(Keys.Space))
            {
                if (shootTimer.Set)
                {
                    canShoot = Timer.CheckTimeReached(gameTime, shootTimer);
                    if (canShoot)
                    {
                        shootTimer.ResetTimer();
                    }
                }
                else
                {
                    ShootProjectile();
                    shootTimer.SetStartTimeAndStopTime(gameTime, 500);
                    canShoot = false;
                }
            }

            if (inputHandler.Command != null)
            {
                inputHandler.Command.Execute(gameTime, player);
            }

            inputHandler.Command = null;

            if (player.isDrifting)
            {
                player.Run(gameTime, player.isMovingLeft, ScreenHeight);
            }
        }

        //Draw sprites in game window
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();


            _spriteBatch.Draw(
            playerTexture,
            player.CurrentPosition,
            Color.White
            );

            _spriteBatch.Draw(
            ball,
            player.CurrentPosition + new Vector2(8, 8),
            Color.White);


            //Draw projectiles
            foreach (Projectile p in GameObjectSpawner.projectiles)
            {
                _spriteBatch.Draw(
                p.Texture,
                p.CurrentPosition,
                Color.White
                );

                _spriteBatch.Draw(
                ball,
                p.CurrentPosition,
                Color.White);
            };

            foreach (Projectile p in GameObjectSpawner.enemyProjectiles)
            {
                _spriteBatch.Draw(
                p.Texture,
                p.CurrentPosition,
                Color.White
                );

                _spriteBatch.Draw(
                ball,
                p.CurrentPosition,
                Color.White);
            };

            //Draw obstacles
            foreach (Obstacle o in GameObjectSpawner.obstacles)
            {
                _spriteBatch.Draw(
                o.Texture,
                o.CurrentPosition,
                Color.White
                );

                //draws their collisions shapes
                _spriteBatch.Draw(
                ball,
                o.CurrentPosition,
                null,
                Color.White,
                0,
                Vector2.Zero,
                //only 2.0f because ball texture is 16bits 
                2.0f,
                0,
                0
                );
            }

            //Draw obstacles
            foreach (Item i in GameObjectSpawner.items)
            {
                _spriteBatch.Draw(
                i.Texture,
                i.CurrentPosition,
                Color.White
                );

                //draws their collisions shapes
                _spriteBatch.Draw(
                ball,
                i.CurrentPosition,
                Color.White
                );
           
            }


            //Draw score
            Vector2 position = new Vector2(_graphics.PreferredBackBufferWidth / 2, _graphics.PreferredBackBufferHeight / 4 - _graphics.PreferredBackBufferHeight / 6);
            _spriteBatch.DrawString(spriteFont, Score.ScoreValue.ToString("D10"), position, Color.White, 0, textMiddlePoint, 1.0f, SpriteEffects.None, 0.5f);



            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
