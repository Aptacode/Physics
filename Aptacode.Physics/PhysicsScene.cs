using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Reflection.Metadata;
using System.Timers;
using Aptacode.Geometry.Blazor.Components.ViewModels;
using Aptacode.Geometry.Collision;

namespace Aptacode.Physics
{
    public static class Constants
    {
        public static readonly float Tolerance = 0.001f;
    }
    public class PhysicsComponent
    {
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

        public static readonly Vector2 Gravity = new Vector2(0, 0.9f);

        public PhysicsComponent(ComponentViewModel component)
        {
            Component = component;
            Color = Color.Gray;
            Velocity = new Vector2(4f,1f);
            Mass = 1.0f;
            HasPhysics = true;
            HasCollisions = true;
        }
               
        public void Start()
        {
            Acceleration = new Vector2(0, 0);
        }

        private readonly float _frictionCoefficient = 0.05f;
        private readonly float _minVelocity = 0.001f;
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
            Velocity += Acceleration * (float)delta.TotalSeconds;
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

            return Velocity * (float)delta.TotalSeconds;
        }
    }

    public class PhysicsEngine
    {
        public List<PhysicsComponent> Components { get; set; }
        public CollisionDetector CollisionDetector { get; set; }
        
        public PhysicsEngine(IEnumerable<PhysicsComponent> components, CollisionDetector collisionDetector)
        {
            CollisionDetector = collisionDetector;
            Components = components.ToList();
        }

        public void Start()
        {
            _timer = new Timer(1.0f/30.0f);
            _timer.Elapsed += Tick;
            _timer.Start();
            _lastTick = DateTime.Now;
        }

        private Timer _timer;
        private DateTime _lastTick;
        private void Tick(object sender, ElapsedEventArgs e)
        {
            var currentTime = DateTime.Now;
            var delta = currentTime - _lastTick;
            ApplyPhysics(delta);
            _lastTick = currentTime;
        }

        public void ApplyPhysics(TimeSpan delta)
        {
            foreach (var C1 in Components.Where(c => c.HasPhysics))
            {
                C1.Start();
                //C1.ApplyForce(new Vector2(0,9f));

                
                foreach (var C2 in Components.Where(c => c.HasPhysics && C1 != c))
                {
                    var force = C2.Component.Primitive.BoundingCircle.Center - C1.Component.Primitive.BoundingCircle.Center;
                    var d = force.Length();
                    force = Vector2.Normalize(force);
                    var strength = (0.4f * C1.Mass * C2.Mass) / (d * d);
                    force *= (float)strength;
                    C1.ApplyForce(force);
                }

                C1.ApplyFriction();
                C1.ApplyAcceleration(delta);

                var distance = C1.ApplyVelocity(delta);
                var newPrimitive = C1.Component.Primitive.Translate(distance);
                if (Components.Any(c => C1 != c && c.HasCollisions && c.Component.Primitive.CollidesWith(newPrimitive, CollisionDetector)))
                {
                    C1.Velocity *= -1;
                }

                distance = C1.ApplyVelocity(delta);
                C1.Component.Translate(distance);
            }
        }
    }
}
