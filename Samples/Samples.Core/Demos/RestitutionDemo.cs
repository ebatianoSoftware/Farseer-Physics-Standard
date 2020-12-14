using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Samples.Core.Demos.Prefabs;
using System.Numerics;

namespace Samples.Core.Demos
{
    public class RestitutionDemo: DemoScene
    {
        public override string Name => "Restitution";
        private Border _border;
        private Body[] _circle = new Body[6];

        public override void Reset()
        {
            World.ClearForces();
            World.Clear();

            World.Gravity = new Vector2(0f, 20f);

            Size = new Vector2(100, 60);
            _border = new Border(World, Size);

            Vector2 position = new Vector2(-15f, -8f);
            float restitution = 0f;

            for (int i = 0; i < 6; ++i)
            {
                _circle[i] = BodyFactory.CreateCircle(World, 1.5f, 1f, position);
                _circle[i].BodyType = BodyType.Dynamic;
                _circle[i].Restitution = restitution;
                position.X += 6f;
                restitution += 0.2f;
            }
        }
    }
}
