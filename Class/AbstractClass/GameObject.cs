using Drifter.Class.Factory;
using Drifter.Class.Tools;
using Drifter.Class.Tools.CollisionShapes;
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
    internal class GameObject
    {
        protected int travelSpeed { get; set; }

        protected Texture2D ObjectTexture { get; init; }

        public Texture2D Texture { get { return ObjectTexture; } }

        protected Vector2 Position;

        public CollisionCircle collisionCircle;

        public bool isMovingLeft = false;

        protected virtual void UpdateCollisionCircle()
        {
            this.collisionCircle.Centre = this.Position + new Vector2(8, 8);
        }

        public Vector2 CurrentPosition { get { return Position; } }

        //IsMovingNegative only used for player
        public virtual void Run(bool IsMovingNegative, float EndOfScreenPosition)
        {
            UpdateCollisionCircle();
            DidExitScreen(EndOfScreenPosition);
        }
        
        public int TravelSpeed { get {  return travelSpeed; } }


        public virtual void CollidedWithOtherGameObject(GameObject gameObject = null) { 
            DestroyMyself();
        }



        protected virtual void DidExitScreen(float EndOfScreenPosition)
        {
            if (Position.Y > EndOfScreenPosition + 128 || Position.Y < 0 - 128)
            {
                DestroyMyself();
            }
        }

        protected void DestroyMyself()
        {
            GameObjectSpawner.AddToDeleteList(this);
        }


        protected virtual void CheckObjectAtEdge()
        {

            if (CurrentPosition.X < 0)
            {
                SetPositionAtEdgeOfScreen(0);
            }

            else if (CurrentPosition.X + Texture.Width / 2 > Globals.ScreenWidth - Texture.Width / 2)
            {
                SetPositionAtEdgeOfScreen(Globals.ScreenWidth - Texture.Width);
            }
        }


        public void SetPositionAtEdgeOfScreen(float edgePosition)
        {
            this.Position.X = edgePosition;
        }
    }
}
