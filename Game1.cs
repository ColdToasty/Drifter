
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Drifter.Class.Factory;
using System;
using System.Collections.Generic;
using Drifter.Class.GameObjectClass;
using Drifter.Class.Tools;
using Drifter.Class.AbstractClass;
using static System.Net.Mime.MediaTypeNames;

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

        private Random random;

        public static int ScreenWidth { get; private set; }



        private Texture2D ball;




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
                    AddToList(gameObjectSpawner.CreateObstacle(obstacleAsteroid));
                }
                Score.IncreaseScore(1);
                previousTimeInSeconds = (int)gameTime.TotalGameTime.TotalSeconds;
            }


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
            if (!player.ItemPickedUp)
            {
                AddToList(gameObjectSpawner.CreateProjectile(projectileMissile, player.CurrentPosition));
            }
        }


        private void CheckPlayerInput(GameTime gameTime)
        {
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
        }



        //Deletes gameObjects by removing them from their respective list
        //Also resets the objectsToBeDeleted list at the end
        private static void DeleteGameObjects()
        {
            for(int i = 0; i < objectsToBeDeleted.Count; i++)
            {
                GameObject gameObject = objectsToBeDeleted[i];

                if(gameObject is Obstacle)
                {
                    obstacles.Remove((Obstacle)gameObject);
                }
                else if(gameObject is Projectile)
                {
                    projectiles.Remove((Projectile)gameObject);
                }
            }
            objectsToBeDeleted.Clear();
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
                if (projectiles[i].DidExitScreen(_graphics.PreferredBackBufferHeight))
                {
                    objectsToBeDeleted.Add(projectiles[i]);
                }
            }
            for(int i = obstacles.Count - 1; i >= 0;i--)
            {
                obstacles[i].Run(gameTime, false);
                if (obstacles[i].DidExitScreen(_graphics.PreferredBackBufferHeight))
                {
                    objectsToBeDeleted.Add(obstacles[i]);
                }
            }
        }

        private void CheckCollision()
        {
            foreach(Obstacle o in obstacles)
            {
                if (player.collisionCircle.Intersects(o.collisionCircle))
                {
                    o.CollidedWithOtherGameObject();
                    player.CollidedWithOtherGameObject();
                    objectsToBeDeleted.Add(o);
                }
                foreach(Projectile p in projectiles)
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
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();



            Vector2 position = new Vector2(_graphics.PreferredBackBufferWidth / 2, _graphics.PreferredBackBufferHeight /4 - _graphics.PreferredBackBufferHeight / 8);
            Vector2 textMiddlePoint = spriteFont.MeasureString("MonoGame Font Test") / 2;
            _spriteBatch.DrawString(spriteFont, "MonoGame Font Test", position, Color.White, 0, textMiddlePoint, 1.0f, SpriteEffects.None, 0.5f);

            _spriteBatch.Draw(
                playerTexture,
                player.CurrentPosition,
                Color.White
            );



            _spriteBatch.Draw(
            ball,
            player.collisionCircle.Centre,
            Color.White
            );


            _spriteBatch.Draw(
            ball,
            new Vector2(32, 32),
            Color.White
            );

            _spriteBatch.Draw(
            ball,
            new Vector2(32, 32),
            Color.White
            );


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
                o.CurrentPosition + new Vector2(8, 8),
                Color.White
                );
            }




            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
