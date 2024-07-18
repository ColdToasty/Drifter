
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Drifter.Class.Factory;
using System;
using System.Collections.Generic;
using Drifter.Class.GameObjectClass;
using Drifter.Class.Tools;
using Drifter.Class.AbstractClass;


namespace Drifter
{
    public class Game1 : Game
    {
        private static GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Texture2D playerTexture, projectileMissile, projectileLaser;
        private Texture2D obstacleAsteroid;

        private Vector2 playerPosition;

        private Player player;

        private GameObjectSpawner gameObjectSpawner;
        private SpriteFont spriteFont;

        private static List<Projectile> projectiles = new List<Projectile>();
        private static List<Obstacle> obstacles= new List<Obstacle>();
        private static List<Item> items = new List<Item>();

        private static List<GameObject> objectsToBeDeleted = new List<GameObject>();

        private static int BottomEndPosition, TopEndPosition;

        private Timer shootTimer;
        private bool canShoot;


        private int previousTimeInSeconds;

        private  Random random;

        public static int ScreenWidth { get; private set; }



        private Texture2D ball, coin;

        Vector2 textMiddlePoint;


        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            playerPosition = new Vector2(_graphics.PreferredBackBufferWidth /2, _graphics.PreferredBackBufferHeight - _graphics.PreferredBackBufferHeight /4);
            player = new Player(playerPosition);

            ScreenWidth = _graphics.PreferredBackBufferWidth;
            gameObjectSpawner = new GameObjectSpawner(_graphics.PreferredBackBufferWidth);

            BottomEndPosition = _graphics.PreferredBackBufferHeight;
            TopEndPosition = 0;
            canShoot = true;
            previousTimeInSeconds = 0;
            shootTimer = new Timer();
            random = new Random();
            

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
            spriteFont = Content.Load<SpriteFont>("Font/MainFont");
            playerTexture = Content.Load<Texture2D>("Player/DefaultPlayer");
            ball = Content.Load<Texture2D>("CollisionShape/ball");
            projectileMissile = Content.Load<Texture2D>("Projectile/Missile");
            obstacleAsteroid = Content.Load<Texture2D>("Obstacle/Asteroid/Asteroid");
            coin = Content.Load<Texture2D>("Item/Coin/Coin");
            textMiddlePoint = spriteFont.MeasureString(Score.ScoreValue.ToString("D10")) / 2;
        }


        //Deletes gameObjects by removing them from their respective list
        //Also resets the objectsToBeDeleted list at the end
        private static void DeleteGameObjects()
        {
            for (int i = 0; i < objectsToBeDeleted.Count; i++)
            {
                GameObject gameObject = objectsToBeDeleted[i];

                if (gameObject is Obstacle)
                {
                    obstacles.Remove((Obstacle)gameObject);
                }
                else if (gameObject is Projectile)
                {
                    projectiles.Remove((Projectile)gameObject);
                }
                else if (gameObject is Item)
                {
                    items.Remove((Item)gameObject);
                }
            }
            objectsToBeDeleted.Clear();
        }


        private void AddToList<TGameObject>(TGameObject gameObject) where TGameObject : GameObject
        {
            if (gameObject is Projectile)
            {
                projectiles.Add(gameObject as Projectile);
            }

            else if (gameObject is Obstacle)
            {
                obstacles.Add(gameObject as Obstacle);
            }
            else if(gameObject is Item)
            {
                items.Add(gameObject as Item);

            }
            else
            {
                //System.Diagnostics.Trace.WriteLine("Null");
            }
        }



        private void CheckPlayerAtTheEdge()
        {
            if (player.CurrentPosition.X < 0)
            {
                player.SetPositionAtEdgeOfScreen(0);
            }

            else if (player.CurrentPosition.X + playerTexture.Width/2 > _graphics.PreferredBackBufferWidth - playerTexture.Width / 2)
            {
                player.SetPositionAtEdgeOfScreen(_graphics.PreferredBackBufferWidth - playerTexture.Width);
            }
        }



        private void RunObjects(GameTime gameTime)
        {
            for (int i = projectiles.Count - 1; i >= 0; i--)
            {
                projectiles[i].Run(gameTime, false);
                if (projectiles[i].DidExitScreen(_graphics.PreferredBackBufferHeight))
                {
                    objectsToBeDeleted.Add(projectiles[i]);
                }
            }
            for (int i = obstacles.Count - 1; i >= 0; i--)
            {
                obstacles[i].Run(gameTime, false);
                if (obstacles[i].DidExitScreen(_graphics.PreferredBackBufferHeight))
                {
                    objectsToBeDeleted.Add(obstacles[i]);
                }
            }

            for (int i = items.Count - 1; i >= 0; i--)
            {
                items[i].Run(gameTime, false);
                if (items[i].DidExitScreen(_graphics.PreferredBackBufferHeight))
                {
                    objectsToBeDeleted.Add(items[i]);
                }
            }
        }


        private void CheckCollision()
        {
            foreach (Obstacle o in obstacles)
            {
                if (player.collisionCircle.Intersects(o.collisionCircle))
                {
                    o.CollidedWithOtherGameObject();
                    player.CollidedWithOtherGameObject();
                    objectsToBeDeleted.Add(o);
                }
                foreach (Projectile p in projectiles)
                {
                    if (o.collisionCircle.Intersects(p.collisionCircle))
                    {
                        o.CollidedWithOtherGameObject();
                        p.CollidedWithOtherGameObject();
                        objectsToBeDeleted.Add(o);
                        objectsToBeDeleted.Add(p);
                    }
                }
            }

            foreach(Item i in items)
            {
                if (i.collisionCircle.Intersects(player.collisionCircle))
                {
                    i.CollidedWithOtherGameObject();
                    player.CollidedWithOtherGameObject(i);
                    objectsToBeDeleted.Add(i);
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
                AddToList(gameObjectSpawner.CreateObstacle(obstacleAsteroid));

                AddToList(gameObjectSpawner.CreateItem(coin));

                previousTimeInSeconds = (int)gameTime.TotalGameTime.TotalSeconds;
            }
            Score.IncreaseScore(1);

            CheckPlayerInput(gameTime);

            RunObjects(gameTime);
            CheckCollision();
            DeleteGameObjects();

            //System.Diagnostics.Trace.WriteLine(obstacles.Count);
            //System.Diagnostics.Trace.WriteLine(projectiles.Count);
            
            base.Update(gameTime);
        }

        private void ShootProjectile()
        {
                AddToList(gameObjectSpawner.CreateProjectile(projectileMissile, player.CurrentPosition));
        }


        private void CheckPlayerInput(GameTime gameTime)
        {
            var kstate = Keyboard.GetState();

            if (kstate.IsKeyDown(Keys.Left) || kstate.IsKeyDown(Keys.A))
            {
                player.Run(gameTime, true);
                player.isDrifting = true;
                player.isMovingLeft = true; ;
            }

            CheckPlayerAtTheEdge();

            if (kstate.IsKeyDown(Keys.Right) || kstate.IsKeyDown(Keys.D))
            {
                player.Run(gameTime, false);
                player.isDrifting = true;
                player.isMovingLeft = false;
            }


            if (kstate.IsKeyDown(Keys.Space))
            {
                if (shootTimer.Set)
                {
                    canShoot = Timer.CheckTimeReached(shootTimer, gameTime);
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
            if (player.isDrifting)
            {
                player.Run(gameTime, player.isMovingLeft);
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
            player.CurrentPosition + new Vector2(8,12),
            Color.White
            );


            //Draw projectiles
            foreach (Projectile p in  projectiles)
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
            foreach (Obstacle o in obstacles)
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
            foreach (Item i in items)
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

            Vector2 position = new Vector2(_graphics.PreferredBackBufferWidth / 2, _graphics.PreferredBackBufferHeight / 4 - _graphics.PreferredBackBufferHeight / 8);
            _spriteBatch.DrawString(spriteFont, Score.ScoreValue.ToString("D10"), position, Color.White, 0, textMiddlePoint, 1.0f, SpriteEffects.None, 0.5f);



            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
