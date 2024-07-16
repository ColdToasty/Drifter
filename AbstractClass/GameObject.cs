using Drifter.Class;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceRider.Interface
{
    internal abstract class GameObject
    {

        protected int travelSpeed { get; set; }

        protected Texture2D ObjectTexture { get; init; }

        protected Vector2 Position;

        public CollisionCircle collisionCircle;

        protected virtual void UpdateCollisionCircle()
        {
            Vector2 collisionCircleCentre = this.Position + new Vector2(8, 8);
            this.collisionCircle.Centre = collisionCircleCentre;

        }

        public Vector2 CurrentPosition { get { return Position; } }

        //IsMovingNegative only used for player
        public abstract void Run(GameTime gameTime, bool IsMovingNegative);
        
        public int TravelSpeed { get {  return travelSpeed; } }
    }
}
