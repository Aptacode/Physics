﻿using System.Numerics;
using Aptacode.Geometry.Blazor.Utilities;
using Aptacode.Geometry.Primitives;

namespace Aptacode.Physics.Blazor.Pages
{
    public class PhysicsComponentBuilder
    {
        private Primitive _primitive = new Point(Vector2.Zero);
        private readonly ViewModelFactory _viewModelFactory = new();
        private Vector2 _velocity = Vector2.Zero;
        private Vector2 _acceleration = Vector2.Zero;
        private float _mass = 1.0f;
        private bool _hasPhysics = true;

        public PhysicsComponentBuilder SetVelocity(Vector2 velocity)
        {
            _velocity = velocity;
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

        public PhysicsComponentBuilder SetPrimitive(Primitive primitive)
        {
            _primitive = primitive;
            return this;
        }

        public PhysicsComponent Build()
        {
            var component = new PhysicsComponent(_viewModelFactory.ToViewModel(_primitive))
            {
                Mass = _mass,
                Acceleration = _acceleration,
                Velocity = _velocity,
                HasPhysics = _hasPhysics
            };

            Reset();
            return component;
        }

        public void Reset()
        {
            _primitive = new Point(Vector2.Zero);
            _mass = 1.0f;
            _acceleration = Vector2.Zero;
            _velocity = Vector2.Zero;
            _hasPhysics = true;
        }
    }
}