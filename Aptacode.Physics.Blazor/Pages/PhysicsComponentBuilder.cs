using System.Numerics;
using Aptacode.Geometry.Blazor.Components.ViewModels;

namespace Aptacode.Physics.Blazor.Pages
{
    public class PhysicsComponentBuilder
    {
        private Vector2 _acceleration = Vector2.Zero;
        private ComponentViewModel _component;
        private bool _hasCollisions = true;
        private bool _hasPhysics = true;
        private float _mass = 1.0f;
        private Vector2 _velocity = Vector2.Zero;

        public PhysicsComponentBuilder SetVelocity(Vector2 velocity)
        {
            _velocity = velocity;
            return this;
        }

        public PhysicsComponentBuilder SetCollision(bool collision)
        {
            _hasCollisions = collision;
            return this;
        }

        public PhysicsComponentBuilder SetAcceleration(Vector2 acceleration)
        {
            _acceleration = acceleration;
            return this;
        }

        public PhysicsComponentBuilder SetMass(float mass)
        {
            _mass = mass;
            return this;
        }

        public PhysicsComponentBuilder SetPhysics(bool hasPhysics)
        {
            _hasPhysics = hasPhysics;
            return this;
        }

        public PhysicsComponentBuilder SetComponent(ComponentViewModel component)
        {
            _component = component;
            return this;
        }

        public PhysicsComponent Build()
        {
            var component = new PhysicsComponent(_component)
            {
                Mass = _mass,
                Acceleration = _acceleration,
                Velocity = _velocity,
                HasPhysics = _hasPhysics,
                HasCollisions = _hasCollisions
            };

            Reset();
            return component;
        }

        public void Reset()
        {
            _mass = 1.0f;
            _acceleration = Vector2.Zero;
            _velocity = Vector2.Zero;
            _hasPhysics = true;
            _hasCollisions = true;
        }
    }
}