using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Aptacode.Geometry.Collision;
using Aptacode.Geometry.Collision.Rectangles;

namespace Aptacode.Physics
{
    public class PhysicsEngine
    {
        #region Constructor

        public PhysicsEngine(IEnumerable<PhysicsComponent> components, CollisionDetector collisionDetector)
        {
            CollisionDetector = collisionDetector;
            Components = components.ToList();
            physicsComponents = components.Where(c => c.HasPhysics).ToArray();
        }

        #endregion

        public void ApplyPhysics(TimeSpan delta)
        {
            foreach (var C1 in physicsComponents)
            {
                C1.Start();
                //C1.ApplyForce(new Vector2(0,9f));


                foreach (var C2 in physicsComponents)
                {
                    if (C1 == C2)
                    {
                        continue;
                    }

                    var force = C2.Component.BoundingRectangle.Center -
                                C1.Component.BoundingRectangle.Center;
                    var d = force.LengthSquared();
                    force = Vector2.Normalize(force);
                    var strength = 0.5f * C1.Mass * C2.Mass / d;
                    force *= strength;
                    C1.ApplyForce(force);
                }

                C1.ApplyFriction();
                C1.ApplyAcceleration(delta);

                var distance = C1.ApplyVelocity(delta);
                var newPrimitive = C1.Component.BoundingRectangle.Translate(distance);
                if (Components.Any(c =>
                    C1 != c && c.HasCollisions && c.Component.BoundingRectangle.CollidesWith(newPrimitive)))
                {
                    C1.Velocity *= -0.9f;
                }

                distance = C1.ApplyVelocity(delta);
                C1.Component.Translate(distance);
            }
        }

        #region Properties

        private PhysicsComponent[] physicsComponents { get; }
        public List<PhysicsComponent> Components { get; set; }
        public CollisionDetector CollisionDetector { get; set; }

        #endregion
    }
}