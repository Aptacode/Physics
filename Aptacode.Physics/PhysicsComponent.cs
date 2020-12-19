using System;
using System.Drawing;
using System.Numerics;
using Aptacode.Geometry.Blazor.Components.ViewModels;

namespace Aptacode.Physics
{
    public class PhysicsComponent
    {
        public static readonly Vector2 Gravity = new(0, 0.9f);

        private readonly float _frictionCoefficient = 0.05f;
        private readonly float _minVelocity = 0.001f;

        public PhysicsComponent(ComponentViewModel component)
        {
            Component = component;
            Color = Color.Gray;
            Velocity = new Vector2(4f, 1f);
            Mass = 1.0f;
            HasPhysics = true;
            HasCollisions = true;
        }

        public ComponentViewModel Component { get; set; }
        public Color Color { get; set; }
        public Vector2 Velocity { get; set; }
        public Vector2 Acceleration { get; set; }
        public float Mass { get; set; }
        public bool HasPhysics { get; set; }

        public bool HasCollisions
        {
            get => Component.CollisionDetectionEnabled;
            set => Component.CollisionDetectionEnabled = value;
        }

        public void Start()
        {
            Acceleration = new Vector2(0, 0);
        }

        public void ApplyFriction()
        {
            if (Math.Abs(Velocity.X) <= Constants.Tolerance && Math.Abs(Velocity.Y) <= Constants.Tolerance)
            {
                return;
            }

            var friction = Velocity * -1.0f;
            friction = Vector2.Normalize(friction) * _frictionCoefficient;
            ApplyForce(friction);
        }

        public void ApplyForce(Vector2 force)
        {
            Acceleration += force / Mass;
        }

        public void ApplyAcceleration(TimeSpan delta)
        {
            Velocity += Acceleration * (float) delta.TotalSeconds;
        }

        public Vector2 ApplyVelocity(TimeSpan delta)
        {
            if (Math.Abs(Velocity.X) <= _minVelocity)
            {
                Velocity = new Vector2(0.0f, Velocity.Y);
            }

            if (Math.Abs(Velocity.Y) <= _minVelocity)
            {
                Velocity = new Vector2(Velocity.X, 0.0f);
            }

            return Velocity * (float) delta.TotalSeconds;
        }
    }
}