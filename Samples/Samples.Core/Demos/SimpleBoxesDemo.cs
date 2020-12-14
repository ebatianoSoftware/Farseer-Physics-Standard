using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Samples.Core.Demos.Prefabs;
using System.Numerics;

namespace Samples.Core.Demos
{
    public class SimpleBoxesDemo : DemoScene
    {
        public override string Name => "Simple Boxes";

        private Body rectangle;
        private Body rectangle2;
        private Body rectangle3;
        private Border _border;

        public override void Reset()
        {
            World.ClearForces();
            World.Clear();

            World.Gravity = new Vector2(0, 9.82f);

            rectangle = BodyFactory.CreateRectangle(World, 5f, 5f, 1f, new Vector2(0, 20));
            rectangle.BodyType = BodyType.Dynamic;

            rectangle2 = BodyFactory.CreateRectangle(World, 5f, 5f, 1f, new Vector2(3, 0));
            rectangle2.BodyType = BodyType.Dynamic;

            rectangle3 = BodyFactory.CreateRectangle(World, 5f, 5f, 1f, new Vector2(-2.6f, -20));
            rectangle3.BodyType = BodyType.Dynamic;

            Size = new Vector2(100, 60);
            _border = new Border(World, Size);

            World.Step(0);
        }
    }
}
