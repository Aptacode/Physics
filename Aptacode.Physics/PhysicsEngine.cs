using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Aptacode.Geometry.Collision;

namespace Aptacode.Physics
{
    public class PhysicsEngine
    {
        #region Constructor

        public PhysicsEngine(IEnumerable<PhysicsComponent> components, CollisionDetector collisionDetector)
        {
            CollisionDetector = collisionDetector;
            Components = components.ToList();
        }

        #endregion


        public void Start()
        {
            var lastTick = DateTime.Now;
            new TaskFactory().StartNew(async () =>
            {
                var delta = TimeSpan.Zero;
                Running = true;
                while (Running)
                {
                    await Task.Delay(15);
                    var currentTime = DateTime.Now;
                    delta = currentTime - lastTick;
                    ApplyPhysics(delta);
                    lastTick = currentTime;
                    var frameRate = 1.0f / delta.TotalSeconds;
                    Console.WriteLine($"{frameRate}fps");
                }
            });
        }


        public void ApplyPhysics(TimeSpan delta)
        {
            foreach (var C1 in Components.Where(c => c.HasPhysics))
            {
                C1.Start();
                //C1.ApplyForce(new Vector2(0,9f));


                foreach (var C2 in Components.Where(c => c.HasPhysics && C1 != c))
                {
                    var force = C2.Component.Primitive.BoundingCircle.Center -
                                C1.Component.Primitive.BoundingCircle.Center;
                    var d = force.LengthSquared();
                    force = Vector2.Normalize(force);
                    var strength = 0.5f * C1.Mass * C2.Mass / d;
                    force *= strength;
                    C1.ApplyForce(force);
                }

                //  C1.ApplyFriction();
                C1.ApplyAcceleration(delta);

                var distance = C1.ApplyVelocity(delta);
                var newPrimitive = C1.Component.Primitive.Translate(distance);
                if (Components.Any(c =>
                    C1 != c && c.HasCollisions && c.Component.Primitive.CollidesWith(newPrimitive, CollisionDetector)))
                {
                    C1.Velocity *= -1;
                }

                distance = C1.ApplyVelocity(delta);
                C1.Component.Translate(distance);
            }
        }

        #region Properties

        public List<PhysicsComponent> Components { get; set; }
        public CollisionDetector CollisionDetector { get; set; }
        public bool Running { get; set; }

        #endregion
    }
}