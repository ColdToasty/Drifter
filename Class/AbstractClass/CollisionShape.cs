using Drifter.Class.Tools.CollisionShapes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drifter.Class.AbstractClass
{
    internal abstract class CollisionShape
    {
        public abstract bool Intersects(CollisionShape other);

        public abstract void UpdateCentre(Vector2 ownerPosition);

        public abstract bool Contains(Vector2 point);
    }
}
