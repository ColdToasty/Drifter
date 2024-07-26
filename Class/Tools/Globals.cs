using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Drifter.Class.Tools
{
    internal class Globals
    {

        public static GameTime GameTime { get; private set; }
        public static ContentManager Content { get; set; }
        public static SpriteBatch SpriteBatch { get; set; }
        public static GraphicsDevice GraphicsDevice { get; set; }


        public static void Update(GameTime gt)
        {
            GameTime = gt;
        }


        public static void LoadContent()
        {

        }
    }
}
