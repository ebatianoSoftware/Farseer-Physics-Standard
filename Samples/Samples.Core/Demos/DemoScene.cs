using FarseerPhysics;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Common;
using FarseerPhysics.Dynamics;
using Samples.Core.Input;
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

        protected Vector2 Size { get; set; } = new Vector2(100, 100);

        public ISimpleInput Input { get; set; }

        protected DemoScene()
        {
            World = new World(Vector2.Zero);
        }

        public virtual void Render(IRenderCanvas renderCanvas)
        {
            renderCanvas.Clear(Color.Black);

            renderCanvas.Scale = MathHelper.Min((renderCanvas.Size.Width - 10) / Size.X, (renderCanvas.Size.Height - 10) / Size.Y);
            renderCanvas.Center = Vector2.Zero;

            for (var idx = 0; idx <  World.BodyList.Count; ++idx)
            {
                var body = World.BodyList[idx];

                PrepareDraw(renderCanvas, body);
                DrawBodyFixtures(renderCanvas, body.FixtureList);
            }
        }

        private void PrepareDraw(IRenderCanvas renderCanvas, Body body)
        {
            renderCanvas.StrokeThickness = 1f;

            if (body.IsStatic)
            {
                renderCanvas.StrokeColor = Color.Yellow;    
                renderCanvas.FillColor = Color.FromArgb(128, Color.YellowGreen);
            }
            else if (body.BodyType == BodyType.Kinematic)
            {
                renderCanvas.StrokeColor = Color.White;
                renderCanvas.FillColor = Color.FromArgb(128, Color.White);
            }
            else if(body.Awake)
            {
                renderCanvas.StrokeColor = Color.Red;
                renderCanvas.FillColor = Color.FromArgb(128, Color.OrangeRed);
            }
            else
            {
                renderCanvas.StrokeColor = Color.Green;
                renderCanvas.FillColor = Color.FromArgb(128, Color.SeaGreen);
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

            OnUpdate(delta);
            while (timeAccumulated >= frameTime)
            {
                OnFixedUpdate(frameTime);
                timeAccumulated -= frameTime;
            }
        }

        protected virtual void OnUpdate(float delta) { }

        protected virtual void OnFixedUpdate(float delta)
        {
            World.Step(delta);
        }
    }
}
