using Drifter.Class.Tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drifter.Class.AbstractClass
{
    internal abstract class GameObject
    {


        protected int travelSpeed { get; set; }

        protected Texture2D ObjectTexture { get; init; }

        protected Vector2 Position;

        public CollisionCircle collisionCircle;



        protected virtual void UpdateCollisionCircle()
        {
            this.collisionCircle.Centre = this.Position + new Vector2(8, 8);
        }

        public Vector2 CurrentPosition { get { return Position; } }

        //IsMovingNegative only used for player
        public virtual void Run(GameTime gameTime, bool IsMovingNegative)
        {
            UpdateCollisionCircle();
        }
        
        public int TravelSpeed { get {  return travelSpeed; } }


        public virtual void CollidedWithOtherGameObject() { }

        public virtual bool DidExitScreen(float EndOfScreenPosition)
        {
            if (Position.Y > EndOfScreenPosition + 32 || Position.Y < 0 - 33)
            {
                return true;
            }
            return false;
        }
    }
}
