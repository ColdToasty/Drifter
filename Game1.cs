
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
using Microsoft.Xna.Framework.Audio;
using System.Text.Json.Serialization;
using Drifter.Class.Tools.Background;


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


        private BackgroundManager backgroundManger;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            playerStartPosition = new Vector2(_graphics.PreferredBackBufferWidth /2, _graphics.PreferredBackBufferHeight - _graphics.PreferredBackBufferHeight /4);

 
            moveLeftCommand = new MoveLeftCommand();
            moveRightCommand = new MoveRightCommand();
            stopDriftCommand = new StopDriftCommand();
            shootCommand = new ShootCommand();
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

            backgroundManger = new BackgroundManager(Globals.GetTexture("background"), Globals.ScreenWidth, Globals.ScreenHeight);

        }

        private void RunObjects()
        {
            for (int i = GameObjectSpawner.projectiles.Count - 1; i >= 0; i--)
            {
                GameObjectSpawner.projectiles[i].Run(GameObjectSpawner.projectiles[i].IsMovingNegative, Globals.ScreenHeight);
                GameObjectSpawner.projectiles[i].PlayAnimation();
            }

            for (int i = GameObjectSpawner.obstacles.Count - 1; i >= 0; i--)
            {
                GameObjectSpawner.obstacles[i].Run(GameObjectSpawner.obstacles[i].isMovingLeft, Globals.ScreenHeight);
                GameObjectSpawner.obstacles[i].PlayAnimation();
            }

            for (int i = GameObjectSpawner.items.Count - 1; i >= 0; i--)
            {
                GameObjectSpawner.items[i].Run(false, Globals.ScreenHeight);
                GameObjectSpawner.items[i].PlayAnimation();
            }

            for (int i = GameObjectSpawner.enemyProjectiles.Count - 1; i >= 0; i--)
            {
                GameObjectSpawner.enemyProjectiles[i].Run(false, Globals.ScreenHeight);
                GameObjectSpawner.enemyProjectiles[i].PlayAnimation();
            }

            backgroundManger.Run();
        }


        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            if (player.IsAlive)
            {
                Globals.Update(gameTime, player);
                SpawnTypeSelector.CreateObstacleAndItem();
                Score.IncreaseScore();
                GameObjectSpawner.CreateCoin();
                RunObjects();
                player.PlayAnimation();
                CheckPlayerInput();
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


            if (InputHandler.Command != null)
            {
                InputHandler.Command.Execute(player);
            }

            InputHandler.Command = null;


            if (player.isDrifting)
            {
                player.Drift();
            }




            if (!player.InfiniteProjectile)
            {
                if (kstate.IsKeyDown(Keys.Space))
                {
                    shootCommand.Execute(player);
                }
            }
            else
            {
                shootCommand.Execute(player);

            }


        }



        //Draw sprites in game window
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            SpriteDrawer.DrawGameObjects();

            Globals.SpriteBatch.Draw(
            backgroundManger.BackgroundTexture,
            backgroundManger.BackgroundTextureOnePosition,
            Color.White
            );

            Globals.SpriteBatch.Draw(
            backgroundManger.BackgroundTexture,
            backgroundManger.BackgroundTextureTwoPosition,
            Color.White
            );


            if (backgroundManger.PlanetSpawned)
            {
                Globals.SpriteBatch.Draw(
                backgroundManger.BackgroundPlanet,
                backgroundManger.BackgroundPlanetPosition,
                Color.White
                ) ;
            }

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
                p.CurrentAnimationRectangle,
                Color.White
                );


            };

            foreach (Projectile p in GameObjectSpawner.enemyProjectiles)
            {
                Globals.SpriteBatch.Draw(
                p.Texture,
                p.CurrentPosition,
                p.CurrentAnimationRectangle,
                Color.White
                );

            };



            if (player.IsAlive)
            {
                Globals.SpriteBatch.Draw(
                playerTexture,
                player.CurrentPosition,
                player.CurrentAnimationRectangle,
                Color.White
                );

            }



            //Draw score
            Vector2 position = new Vector2(_graphics.PreferredBackBufferWidth / 2, _graphics.PreferredBackBufferHeight / 4 - _graphics.PreferredBackBufferHeight / 6);
            Globals.SpriteBatch.DrawString(spriteFont, Score.ScoreValue.ToString("D12"), position, Color.White, 0, textMiddlePoint, 1.0f, SpriteEffects.None, 0.5f);



            Globals.SpriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
