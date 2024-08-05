using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace Drifter.Class.Tools.CollisionShapes 
{
    internal struct CollisionCircle
    {

        public Vector2 Centre;

        //Set size of circle
        public float Radius { get; init; }

        //centre = (half width of object texture and half height of object texture) + position of object
        //radius = half of width of texture

        public bool isDisabled;

        public CollisionCircle(Vector2 centre, float radius = 16)
        {
            Centre = centre;
            Radius = radius;
            isDisabled = false;
        }

        public bool Contains(Vector2 point)
        {
            return (Centre - point).LengthSquared() <= Radius * Radius;
        }

        public bool Intersects(CollisionCircle other)
        {
            float distanceBetweenCircleAndOther = (other.Centre - Centre).LengthSquared();
            float combindedRadius = Radius + other.Radius;
            return distanceBetweenCircleAndOther <= combindedRadius * combindedRadius;
        }

    }
}
