using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Samples.Core.Demos.Prefabs;
using System;
using System.Numerics;

namespace Samples.Core.Demos
{
    public class SimplePlatformerDemo : DemoScene
    {
        public override string Name => "Simple Platformer";
        private Border _border;

        private Body _player;

        public override void Reset()
        {
            World.ClearForces();
            World.Clear();

            World.Gravity = new Vector2(0f, 30f);

            Size = new Vector2(50, 30);
            _border = new Border(World, Size);

            _player = BodyFactory.CreateEllipse(World, 0.5f, 1, 16, 1, new Vector2(0, 10));
            //FixtureFactory.AttachCircle(0.5f, 1, _player, new Vector2(0, 0.5f));
            //FixtureFactory.AttachCircle(0.5f, 1, _player, new Vector2(0, -0.5f));
            //FixtureFactory.AttachRectangle(1, 1, 1, new Vector2(0, 0), _player);

            BodyFactory.CreateCircle(World, 2, 1, new Vector2(4, 13), BodyType.Static).Friction = 0.001f;

            var edge2 = BodyFactory.CreateEdge(World, new Vector2(-10, 10), new Vector2(-5, 10));
            edge2.Friction = 0.05f;
            

            _player.FixedRotation = true;
            _player.IsStatic = false;
            _player.Awake = true;

            edge2.OnCollision += Edge2_OnCollision;
        }

        private bool Edge2_OnCollision(Fixture fixtureA, Fixture fixtureB, FarseerPhysics.Dynamics.Contacts.Contact contact)
        {
            if(fixtureB.Body.LinearVelocity.Y <= 0)
            {
                contact.Enabled = false;
                return false;
            }

            fixtureA.Body.GetTransform(out var transformA);
            fixtureA.Shape.ComputeAABB(out var platAabb, ref transformA, 0);

            fixtureB.Body.GetTransform(out var transformB);
            fixtureB.Shape.ComputeAABB(out var objAabb, ref transformB, 0);

            if(objAabb.UpperBound.Y - 0.1f > platAabb.UpperBound.Y)
            {
                contact.Enabled = false;
                return false;
            }

            return true;
        }

        protected override void OnUpdate(float delta)
        {
            if (_player == null) return;

            if (Input.A == Core.Input.BtnState.Pressed)
            {
                if (Input.Stick.Y > 0.1f)
                {
                    _player.CollidesWith = Category.None;
                    _player.Position = new Vector2(_player.Position.X, _player.Position.Y + 0.15f);
                    World.Step(0.00016f);
                    _player.CollidesWith = Category.All;
                }
                else
                {
                    var velocity = _player.LinearVelocity;
                    velocity.Y = -15;
                    _player.LinearVelocity = velocity;
                }
            }
        }

        protected override void OnFixedUpdate(float delta)
        {
            if (_player == null) return;
            if(Math.Abs(Input.Stick.X) > 0.1f)
            {
                _player.Friction = 0.0f;
            }
            else
            {
                _player.Friction = 2.0f;
            }
            var velocity = _player.LinearVelocity;
            
            velocity.X += Input.Stick.X * delta * 50;
            _player.ApplyForce(new Vector2(Input.Stick.X * _player.Mass, 0));
            
            velocity.X = Math.Min(20, Math.Abs(velocity.X)) * Math.Sign(velocity.X);

            _player.LinearVelocity = velocity;
            base.OnFixedUpdate(delta);
        }
    }
}
