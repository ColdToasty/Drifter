using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpaceRider.Class;
using SpaceRider.Class.Factory;
using SpaceRider.Interface;
using System;
using System.Collections.Generic;

namespace SpaceRider
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Texture2D playerTexture, projectileMissile, projectileLaser;


        private Vector2 playerPosition;

        private Player player;

        private ProjectileSpawner projectileSpawner;

        private List<Projectile> projectiles = new List<Projectile>();
        private List<Obstacle> obstacles= new List<Obstacle>();

        private static int BottomEndPosition, TopEndPosition;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            playerPosition = new Vector2(_graphics.PreferredBackBufferWidth /2, _graphics.PreferredBackBufferHeight - _graphics.PreferredBackBufferHeight /4);

            player = new Player("DefaultPlayer", playerPosition);
            projectileSpawner = new ProjectileSpawner();

            BottomEndPosition = _graphics.PreferredBackBufferHeight;
            TopEndPosition = 0;
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
            playerTexture = Content.Load<Texture2D>(player.PlayerTexture);
            projectileMissile = Content.Load<Texture2D>("Missile");

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
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
                if (!player.ItemPickedUp)
                {
                    AddToList(projectileSpawner.CreateProjectile(projectileMissile, player.CurrentPosition));
                }
            }

            RunObjects(gameTime);
            base.Update(gameTime);
        }

        private void AddToList<TGameObject>(TGameObject gameObject) where TGameObject : GameObject
        {
            if(gameObject is Projectile)
            {
                projectiles.Add(gameObject as Projectile);
            }

            if(gameObject is Obstacle)
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

            foreach(Projectile p in  projectiles)
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





            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
