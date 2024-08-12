
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
using Drifter.Class.GameObjectClass.ItemClass;


namespace Drifter
{
    public class Game1 : Game
    {
        private static GraphicsDeviceManager _graphics;

        private Player player;


        private MoveLeftCommand moveLeftCommand;
        private MoveRightCommand moveRightCommand;
        private StopDriftCommand stopDriftCommand;
        private ShootCommand shootCommand;


        private Texture2D playerTexture, projectileMissile, projectileLaser, obstacleAsteroid, ball, coin;

        private Vector2 playerStartPosition, textMiddlePoint;

        //Out score counter font
        private SpriteFont spriteFont;




        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            playerStartPosition = new Vector2(_graphics.PreferredBackBufferWidth /2, _graphics.PreferredBackBufferHeight - _graphics.PreferredBackBufferHeight /4);

 
            moveLeftCommand = new MoveLeftCommand();
            moveRightCommand = new MoveRightCommand();
            stopDriftCommand = new StopDriftCommand();
            Globals.SetScreenMeasurements(_graphics.PreferredBackBufferHeight, _graphics.PreferredBackBufferWidth);
            GameObjectSpawner.SetSpawnXAxisRange(Globals.ScreenWidth);
            GameObjectSpawner.SetSpawnYAxisRange(Globals.ScreenHeight / 5 );


            Score.Reset();
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
            Globals.LoadContent();

            ball = Globals.GetTexture("ball");

            projectileMissile = Globals.GetTexture("projectileMissile");
            playerTexture = Globals.GetTexture("player");
            obstacleAsteroid = Globals.GetTexture("asteroid");
            coin = Globals.GetTexture("coin");


            spriteFont = Content.Load<SpriteFont>("Font/MainFont");
            textMiddlePoint = spriteFont.MeasureString(Score.ScoreValue.ToString("D10")) / 2;

            player = new Player(playerTexture, playerStartPosition);
            shootCommand = new ShootCommand(projectileMissile);

        }

        private void RunObjects()
        {
            for (int i = GameObjectSpawner.projectiles.Count - 1; i >= 0; i--)
            {
                GameObjectSpawner.projectiles[i].Run(GameObjectSpawner.projectiles[i].IsMovingNegative, Globals.ScreenHeight);
            }

            for (int i = GameObjectSpawner.obstacles.Count - 1; i >= 0; i--)
            {
                GameObjectSpawner.obstacles[i].Run(GameObjectSpawner.obstacles[i].isMovingLeft, Globals.ScreenHeight);
            }

            for (int i = GameObjectSpawner.items.Count - 1; i >= 0; i--)
            {
                GameObjectSpawner.items[i].Run(false, Globals.ScreenHeight);
            }

            for (int i = GameObjectSpawner.enemyProjectiles.Count - 1; i >= 0; i--)
            {
                GameObjectSpawner.enemyProjectiles[i].Run(false, Globals.ScreenHeight);
            }
        }



        protected override void Update(GameTime gameTime)
        {
            Globals.Update(gameTime, player);
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            if (player.IsAlive)
            {
                SpawnTypeSelector.CreateObstacleAndItem();
                Score.IncreaseScore();
                GameObjectSpawner.CreateCoin();
                CheckPlayerInput();
                RunObjects();

            }

            GameObjectSpawner.AddToListAfterLoop();
            GameObjectSpawner.DeleteGameObjects();

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


                if (kstate.IsKeyDown(Keys.Down) || kstate.IsKeyDown(Keys.S))
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
                    player.Run(player.isMovingLeft, Globals.ScreenHeight);
                }
            

        }



        //Draw sprites in game window
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);


            SpriteDrawer.DrawGameObjects();

            //Draw obstacles
            foreach (Obstacle o in GameObjectSpawner.obstacles)
            {
                if(o is SpacePipe)
                {
                    SpacePipe sp = (SpacePipe)o;
                    Globals.SpriteBatch.Draw(
                    sp.leftHeadTexture,
                    new Vector2(sp.LeftHeadPosition, sp.CurrentPosition.Y),
                    Color.White
                    );

                    Globals.SpriteBatch.Draw(
                    sp.rightHeadTexture,
                    new Vector2(sp.RightHeadPosition, sp.CurrentPosition.Y),
                    Color.White
                    );


                    foreach (int value in sp.GetXPositions())
                    {
                        Globals.SpriteBatch.Draw(
                        sp.bodyTexture,
                        new Vector2(value, sp.CurrentPosition.Y),
                        Color.White
                        );
                    }

                }
                else
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

            }



            //Draw obstacles
            foreach (Item i in GameObjectSpawner.items)
            {
                Globals.SpriteBatch.Draw(
                i.Texture,
                i.CurrentPosition,
                Color.White
                );
           
            }

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



            if (player.IsAlive)
            {
                Globals.SpriteBatch.Draw(
                playerTexture,
                player.CurrentPosition,
                Color.White
                );

                Globals.SpriteBatch.Draw(
                ball,
                player.CurrentPosition + new Vector2(8, 8),
                Color.White);
            }



            //Draw score
            Vector2 position = new Vector2(_graphics.PreferredBackBufferWidth / 2, _graphics.PreferredBackBufferHeight / 4 - _graphics.PreferredBackBufferHeight / 6);
            Globals.SpriteBatch.DrawString(spriteFont, Score.ScoreValue.ToString("D12"), position, Color.White, 0, textMiddlePoint, 1.0f, SpriteEffects.None, 0.5f);



            Globals.SpriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
