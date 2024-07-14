using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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

        public Vector2 CurrentPosition { get { return Position; } }

        public abstract void Run(GameTime gameTime, bool IsMovingNegative);
        
        public int TravelSpeed { get {  return travelSpeed; } }
    }
}
