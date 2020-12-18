using FarseerPhysics.Common;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using System.Numerics;

namespace Samples.Core.Demos.Prefabs
{
    public class Border
    {
        private Body _anchor;

        public Border(World world, Vector2 size)
        {
            // Physics
            float halfWidth = size.X / 2f - 0.75f;
            float halfHeight = size.Y / 2f - 0.75f;

            Vertices borders = new Vertices(4);
            borders.Add(new Vector2(-halfWidth, halfHeight));  // Lower left
            borders.Add(new Vector2(halfWidth, halfHeight));   // Lower right
            borders.Add(new Vector2(halfWidth, -halfHeight));  // Upper right
            borders.Add(new Vector2(-halfWidth, -halfHeight)); // Upper left

            _anchor = BodyFactory.CreateLoopShape(world, borders);
            _anchor.CollisionCategories = Category.All;
            _anchor.CollidesWith = Category.All;
            _anchor.Friction = 1;
        }
    }
}