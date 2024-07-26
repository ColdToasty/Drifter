
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

        public static int ScreenHeight { get; private set; }
        public static int ScreenWidth { get; private set; }

        public static Random Random = new Random();
        private Player player;


        private MoveLeftCommand moveLeftCommand;
        private MoveRightCommand moveRightCommand;
        private StopDriftCommand stopDriftCommand;
        private ShootCommand shootCommand;




        private Texture2D playerTexture, projectileMissile, projectileLaser, obstacleAsteroid, ball, coin;

        private Vector2 playerStartPosition, textMiddlePoint;

        //Out score counter font
        private SpriteFont spriteFont;


        private int previousTimeInSeconds;


        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            playerStartPosition = new Vector2(_graphics.PreferredBackBufferWidth /2, _graphics.PreferredBackBufferHeight - _graphics.PreferredBackBufferHeight /4);

 
            moveLeftCommand = new MoveLeftCommand();
            moveRightCommand = new MoveRightCommand();
            stopDriftCommand = new StopDriftCommand();

            ScreenHeight = _graphics.PreferredBackBufferHeight;
            ScreenWidth = _graphics.PreferredBackBufferWidth;
            GameObjectSpawner.SetSpawnXAxisRange(ScreenWidth);
            GameObjectSpawner.SetSpawnYAxisRange(ScreenHeight / 5 );

            previousTimeInSeconds = 0;


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
            Globals.SpriteBatch = new SpriteBatch(GraphicsDevice);
            Globals.Content = Content;
            Globals.GraphicsDevice = GraphicsDevice;

            ball = Content.Load<Texture2D>("CollisionShape/ball");
            projectileMissile = Content.Load<Texture2D>("Projectile/Missile");
            obstacleAsteroid = Content.Load<Texture2D>("Obstacle/Asteroid/Asteroid");
            coin = Content.Load<Texture2D>("Item/Coin/Coin");

            spriteFont = Content.Load<SpriteFont>("Font/MainFont");
            textMiddlePoint = spriteFont.MeasureString(Score.ScoreValue.ToString("D10")) / 2;

            playerTexture = Content.Load<Texture2D>("Player/DefaultPlayer");
            player = new Player(playerTexture, playerStartPosition);
            shootCommand = new ShootCommand(projectileMissile);

            Globals.LoadContent();
        }




        private void RunObjects()
        {
            for (int i = GameObjectSpawner.projectiles.Count - 1; i >= 0; i--)
            {
                GameObjectSpawner.projectiles[i].Run(GameObjectSpawner.projectiles[i].IsMovingNegative, ScreenHeight);
            }

            for (int i = GameObjectSpawner.obstacles.Count - 1; i >= 0; i--)
            {
                GameObjectSpawner.obstacles[i].Run(GameObjectSpawner.obstacles[i].isMovingLeft, ScreenHeight);
            }

            for (int i = GameObjectSpawner.items.Count - 1; i >= 0; i--)
            {
                GameObjectSpawner.items[i].Run(false, _graphics.PreferredBackBufferHeight);
            }

            for (int i = GameObjectSpawner.enemyProjectiles.Count - 1; i >= 0; i--)
            {
                GameObjectSpawner.enemyProjectiles[i].Run(false, ScreenHeight);
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
            Globals.Update(gameTime);
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

            CheckPlayerInput();

            RunObjects();
            CheckCollision();
            GameObjectSpawner.AddToListAfterLoop();
            GameObjectSpawner.DeleteGameObjects();

            //System.Diagnostics.Trace.WriteLine(obstacles.Count);
            //System.Diagnostics.Trace.WriteLine(projectiles.Count);

            base.Update(gameTime);
        }



        private void CheckPlayerInput()
        {
            var kstate = Keyboard.GetState();

            if (kstate.IsKeyDown(Keys.Left) || kstate.IsKeyDown(Keys.A))
            {
                InputHandler.Command = moveLeftCommand;
            }

            if (kstate.IsKeyDown(Keys.Right) || kstate.IsKeyDown(Keys.D))
            {

                InputHandler.Command = moveRightCommand;
            }


            if(kstate.IsKeyDown(Keys.Down) || kstate.IsKeyDown(Keys.S))
            {
                InputHandler.Command = stopDriftCommand;
            }

            if (kstate.IsKeyDown(Keys.Space))
            {
                shootCommand.Execute(player);
            }

            if (InputHandler.Command != null)
            {
                InputHandler.Command.Execute(player);
            }

            InputHandler.Command = null;

            if (player.isDrifting)
            {
                player.Run(player.isMovingLeft, ScreenHeight);
            }
        }

        //Draw sprites in game window
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            Globals.SpriteBatch.Begin();


            //Draw projectiles
            foreach (Projectile p in GameObjectSpawner.projectiles)
            {
                Globals.SpriteBatch.Draw(
                p.Texture,
                p.CurrentPosition,
                Color.White
                );

                Globals.SpriteBatch.Draw(
                ball,
                p.CurrentPosition,
                Color.White);
            };

            foreach (Projectile p in GameObjectSpawner.enemyProjectiles)
            {
                Globals.SpriteBatch.Draw(
                p.Texture,
                p.CurrentPosition,
                Color.White
                );

                Globals.SpriteBatch.Draw(
                ball,
                p.CurrentPosition,
                Color.White);
            };

            //Draw obstacles
            foreach (Obstacle o in GameObjectSpawner.obstacles)
            {
                Globals.SpriteBatch.Draw(
                o.Texture,
                o.CurrentPosition,
                Color.White
                );

                //draws their collisions shapes
                Globals.SpriteBatch.Draw(
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
                Globals.SpriteBatch.Draw(
                i.Texture,
                i.CurrentPosition,
                Color.White
                );

                //draws their collisions shapes
                Globals.SpriteBatch.Draw(
                ball,
                i.CurrentPosition,
                Color.White
                );
           
            }


            Globals.SpriteBatch.Draw(
            playerTexture,
            player.CurrentPosition,
            Color.White
            );

            Globals.SpriteBatch.Draw(
            ball,
            player.CurrentPosition + new Vector2(8, 8),
            Color.White);


            //Draw score
            Vector2 position = new Vector2(_graphics.PreferredBackBufferWidth / 2, _graphics.PreferredBackBufferHeight / 4 - _graphics.PreferredBackBufferHeight / 6);
            Globals.SpriteBatch.DrawString(spriteFont, Score.ScoreValue.ToString("D10"), position, Color.White, 0, textMiddlePoint, 1.0f, SpriteEffects.None, 0.5f);



            Globals.SpriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
