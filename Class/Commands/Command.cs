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
    internal abstract class Command
    {
        public abstract void Execute<T>(T gameObject);


    }
}
