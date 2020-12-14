using FarseerPhysics;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Common;
using FarseerPhysics.Dynamics;
using Samples.Core.Rendering;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Numerics;

namespace Samples.Core.Demos
{
    public abstract class DemoScene: IWindowRenderer
    {
        public abstract string Name { get; }

        protected World World { get; }

        private float timeAccumulated = 0;

        private Vector2[] tempVertices = new Vector2[1024];

        protected DemoScene(World world)
        {
            World = world;
        }

        public virtual void Render(IRenderCanvas renderCanvas)
        {
            renderCanvas.Clear(Color.Black);

            renderCanvas.Scale = 10;
            renderCanvas.Center = Vector2.Zero;

            renderCanvas.StrokeColor = Color.Green;
            renderCanvas.StrokeThickness = 0.25f;
            renderCanvas.FillColor = Color.Yellow;

            for (var idx = 0; idx <  World.BodyList.Count; ++idx)
            {
                var body = World.BodyList[idx];
                DrawBodyFixtures(renderCanvas, body.FixtureList);
            }
        }

        public virtual void Reset()
        {
        }

        private void DrawBodyFixtures(IRenderCanvas renderCanvas, List<Fixture> fixtureList)
        {
            Transform transform;
            for(var idx =0; idx < fixtureList.Count; ++idx)
            {
                var fixture = fixtureList[idx];

                fixture.Body.GetTransform(out transform);
                DrawShape(renderCanvas, fixture.Shape, transform);
            }
        }

        private void DrawShape(IRenderCanvas renderCanvas, Shape shape, Transform xf)
        {
            switch (shape.ShapeType)
            {
                case ShapeType.Circle:
                    {
                        CircleShape circle = (CircleShape)shape;
                        Vector2 center = MathUtils.Mul(ref xf, circle.Position);
                        float radius = circle.Radius;
                        renderCanvas.DrawCircle(center, radius);
                    }
                    break;

                case ShapeType.Polygon:
                    {
                        PolygonShape poly = (PolygonShape)shape;
                        int vertexCount = poly.Vertices.Count;
                        Debug.Assert(vertexCount <= Settings.MaxPolygonVertices);

                        for (int i = 0; i < vertexCount; ++i)
                        {
                            tempVertices[i] = MathUtils.Mul(ref xf, poly.Vertices[i]);
                        }

                        renderCanvas.DrawPolygon(tempVertices, vertexCount);
                    }
                    break;


                case ShapeType.Edge:
                    {
                        EdgeShape edge = (EdgeShape)shape;
                        Vector2 v1 = MathUtils.Mul(ref xf, edge.Vertex1);
                        Vector2 v2 = MathUtils.Mul(ref xf, edge.Vertex2);
                        renderCanvas.DrawLine(v1, v2);
                    }
                    break;

                case ShapeType.Chain:
                    {
                        ChainShape chain = (ChainShape)shape;

                        for (int i = 0; i < chain.Vertices.Count - 1; ++i)
                        {
                            Vector2 v1 = MathUtils.Mul(ref xf, chain.Vertices[i]);
                            Vector2 v2 = MathUtils.Mul(ref xf, chain.Vertices[i + 1]);
                            renderCanvas.DrawLine(v1, v2);
                        }
                    }
                    break;
            }

        }

        public void Update(float delta)
        {
            const float frameTime = 0.01666666666666666666666666666667f;
            timeAccumulated += delta;

            while (timeAccumulated >= frameTime)
            {
                World.Step(frameTime);
                timeAccumulated -= frameTime;
            }
        }
    }
}
