using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Drifter.Class.Tools.CollisionShapes
{
    internal struct CollisionSquare
    {
        public Vector2 Centre;

        //centre = (half width of object texture and half height of object texture) + position of object
        //radius = half of width of texture

        public bool isDisabled;

        private float width, height;

        public CollisionSquare(float width, float height)
        {
            Centre = new Vector2(width/2, height /2);
            isDisabled = false;
            this.width = width;
            this.height = height;
        }
        
        public void UpdateCentre(Vector2 ownerPosition)
        {
            Centre = ownerPosition + new Vector2(this.width/2, this.height/2);
        }


        public bool Contains(Vector2 point)
        {
            return false;
        }

        public bool Intersects(CollisionCircle other)
        {
            Vector2 circleDistance;
            //get distance between x values
            circleDistance.X = Math.Abs(other.Centre.X - Centre.X);
            //get distance between y values
            circleDistance.Y = Math.Abs(other.Centre.X - Centre.Y);

            //if x disance is greater that circle radius and x width then won't overlap
            if (circleDistance.X > (this.width / 2 + other.Radius)) { return false; }
            //if y disance is greater that circle radius and y width then won't overlap
            if (circleDistance.Y > (this.height / 2 + other.Radius)) { return false; }


            if (circleDistance.X <= (this.width / 2)) { return true; }
            if (circleDistance.Y <= (this.height / 2)) { return true; }


            float cornerDistance_sq = (circleDistance.X - this.width / 2) * (circleDistance.X - this.width / 2) +
                                 (circleDistance.Y - this.height / 2) * (circleDistance.Y - this.height / 2);

            return (cornerDistance_sq <= (other.Radius * other.Radius));
        }

        public bool Intersects(CollisionSquare other)
        {
            throw new NotImplementedException();
        }
    }
}
