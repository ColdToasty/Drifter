using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Drifter.Class.Tools.CollisionShapes
{
    internal struct CollisionSquare
    {
        public Vector2 Centre;

        public Rectangle rectangle;

        public bool isDisabled;

        private float width, height;

        //startPosition being the top left corner
        public CollisionSquare(Vector2 startPosition, float width, float height)
        {
            rectangle = new Rectangle((int) startPosition.X, (int) startPosition.Y, (int) width, (int) height);
            Centre = new Vector2(rectangle.Center.X, startPosition.Y + rectangle.Center.Y);
            isDisabled = false;
            this.width = width;
            this.height = height;
        }
        
        public void UpdateRectangle(Vector2 position)
        {
            rectangle.Y = (int)position.Y;
            Centre = new Vector2(rectangle.Center.X, rectangle.Center.Y);
            
        }
       

        public bool Intersects(CollisionCircle circle)
        {
            if (rectangle.Contains(circle.Centre))
            {
                return true;
            }

            float closestX = Math.Max(rectangle.X, Math.Min(circle.Centre.X, width));
            float closestY = Math.Max(rectangle.Y, Math.Min(circle.Centre.Y, rectangle.Y + height));

            // Calculate the distance from the circle's center to this closest point
            float distanceX = circle.Centre.X - closestX;
            float distanceY = circle.Centre.Y - closestY;

            // If the distance is less than or equal to the radius, they overlap
            float distanceSquared = distanceX * distanceX + distanceY * distanceY;
            return distanceSquared <= circle.Radius * circle.Radius;
        }


        //Currently dont need to check for square
        private bool Intersects(CollisionSquare other)
        {
            throw new NotImplementedException();
        }
    }
}
