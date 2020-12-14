using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using System.Numerics;

namespace Samples.Core.Demos
{
    public class SingleFixtureDemo : DemoScene
    {
        public override string Name => "Single Fixture";

        private Body rectangle;
        private Body ground;

        public SingleFixtureDemo(): base(new World(new Vector2(0,100)))
        {
            Reset();
        }

        public override void Reset()
        {
            World.ClearForces();
            World.Clear();
            rectangle = BodyFactory.CreateRectangle(World, 5f, 5f, 1f, new Vector2(0, -10));
            rectangle.BodyType = BodyType.Dynamic;
            ground = BodyFactory.CreateRectangle(World, 20, 1, 1000f, new Vector2(0, 10), 0, BodyType.Static);
            World.Step(0);
        }
    }
}
