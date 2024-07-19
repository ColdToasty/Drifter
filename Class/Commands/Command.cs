using Drifter.Class.GameObjectClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Drifter.Class.AbstractClass;

namespace Drifter.Class.Commands
{
    internal class Command
    {
        public virtual void Execute<T>(GameTime gameTime, T gameObject) { }


    }
}
