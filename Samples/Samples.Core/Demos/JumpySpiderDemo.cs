using Samples.Core.Demos.Prefabs;
using System.Numerics;

namespace Samples.Core.Demos
{
    public class JumpySpiderDemo : DemoScene
    {
        //private Agent _agent;
        private Border _border;
        private JumpySpider[] _spiders;


        public override string Name => "Jumpy Spider";

        public override void Reset()
        {
            World.ClearForces();
            World.Clear();

            World.Gravity = new Vector2(0f, 40f);

            Size = new Vector2(100, 60);
            _border = new Border(World, Size);

            _spiders = new JumpySpider[8];

            for (int i = 0; i < _spiders.Length; i++)
            {
                _spiders[i] = new JumpySpider(World, new Vector2(0f, 8f - (i + 1) * 2f));
            }
        }

        protected override void OnUpdate(float delta)
        {
            for (int i = 0; i < _spiders.Length; i++)
            {
                _spiders[i].Update(delta);
            }

            base.OnUpdate(delta);
        }
    }
}
