using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Samples.Core.Demos.Prefabs;
using System.Numerics;

namespace Samples.Core.Demos
{
    public class TheoJansenWalkerDemo : DemoScene
    {
        public override string Name => "Theo Jansen Walker";

        private Border _border;
        private TheoJansenWalker _walker;
        private Body[] _circles;

        public TheoJansenWalkerDemo()
        {
            Reset();
        }

        public override void Reset()
        {
            World.ClearForces();
            World.Clear();

            World.Gravity = new Vector2(0, 9.82f);

            Size = new Vector2(100, 60);
            _border = new Border(World, Size);

            CircleShape shape = new CircleShape(0.16f, 1);

            _circles = new Body[48];
            for (int i = 0; i < 48; i++)
            {
                _circles[i] = BodyFactory.CreateBody(World);
                _circles[i].BodyType = BodyType.Dynamic;
                _circles[i].Position = new Vector2(-24f + 1f * i, 10f);
                _circles[i].CreateFixture(shape);
            }

            _walker = new TheoJansenWalker(World, Vector2.Zero);
        }
    }
}
