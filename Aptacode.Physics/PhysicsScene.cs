using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Timers;
using Aptacode.Geometry.Collision;
using Aptacode.Geometry.Primitives;
using Aptacode.Geometry.Vertices;
using Rectangle = Aptacode.Geometry.Primitives.Polygons.Rectangle;

namespace Aptacode.Physics
{
    public static class Constants
    {
        public static readonly float Tolerance = 0.001f;
    }
    public abstract class PhysicsComponent
    {
        public Primitive Primitive { get; set; }
        public Color Color { get; set; }
        public Vector2 Velocity { get; set; }
        public Vector2 Acceleration { get; set; }
        public float Mass { get; set; }

        public static readonly Vector2 Gravity = new Vector2(0, 0.9f);

        protected PhysicsComponent(Primitive primitive)
        {
            Primitive = primitive;
            Color = Color.Gray;
            Velocity = new Vector2(2,1);
            Mass = 1.0f;
        }
               
        public void Start()
        {
            Acceleration = new Vector2(0, 0);
        }

        private readonly float _frictionCoefficient = 0.05f;
        private readonly float _minVelocity = 0.000001f;
        public void ApplyFriction()
        {

            var friction = Velocity * -1.0f;
            friction = Vector2.Normalize(friction) * _frictionCoefficient;
            ApplyForce(friction);
        }

        public void ApplyForce(Vector2 force)
        {
            Acceleration += force / Mass;
        }

        public void ApplyAcceleration()
        {
            Velocity += Acceleration;
        }

        public void ApplyVelocity()
        {
            if (Velocity.X <= _minVelocity)
            {
                Velocity = new Vector2(0.0f, Velocity.Y);
            }
            if (Velocity.Y <= _minVelocity)
            {
                Velocity = new Vector2(Velocity.X, 0.0f);
            }
        }

        public void Update()
        {
            Primitive = Primitive.Translate(Velocity);
        }
    }

    public class CircleComponent : PhysicsComponent
    {
        public Circle Circle => (Circle) Primitive;
        
        public CircleComponent(Vector2 center, float radius) : base(new Geometry.Primitives.Circle(center, radius))
        {
            
        }
    }

    public class RectangleComponent : PhysicsComponent
    {
        public Rectangle Rectangle => (Rectangle)Primitive;

        public RectangleComponent(Vector2 position, Vector2 size) : base(Rectangle.Create(position, size))
        {

        }
    }

    public class PhysicsScene
    {
        public Vector2 Size { get; set; }
        public List<PhysicsComponent> Components { get; set; }
        public List<PhysicsComponent> Obstacles { get; set; }
        public readonly PolyLine TopWall, RightWall, BottomWall, LeftWall;


        public PhysicsScene()
        {
            Size = new Vector2(100, 100);
            TopWall = new PolyLine(VertexArray.Create(new Vector2(0, 0), new Vector2(Size.X, 0)));
            RightWall = new PolyLine(VertexArray.Create(new Vector2(Size.X, 0), new Vector2(Size.X, Size.Y)));
            BottomWall = new PolyLine(VertexArray.Create(new Vector2(Size.X, Size.Y), new Vector2(0, Size.Y)));
            LeftWall = new PolyLine(VertexArray.Create(new Vector2(0, Size.Y), new Vector2(0, 0)));
            
            Components = new List<PhysicsComponent>();
            Obstacles = new List<PhysicsComponent>();
            Components.Add(new CircleComponent(new Vector2(60,20), 10));
         //   Obstacles.Add(new RectangleComponent(new Vector2(20,20), new Vector2(20, 20)));
        }
    }

    public class PhysicsEngine
    {
        public PhysicsScene Scene { get; set; }

        public PhysicsEngine()
        {
            Scene = new PhysicsScene();
        }

        public void Start()
        {
            _timer = new Timer(10);
            _timer.Elapsed += Tick;
            _timer.Start();
        }

        private Timer _timer;
        private DateTime _lastTick;
        private readonly CollisionDetector collisionDetector = new HybridCollisionDetector();

        private void Tick(object sender, ElapsedEventArgs e)
        {
            var currentTime = DateTime.Now;
            var delta = currentTime - _lastTick;
            _lastTick = currentTime;

            foreach (var component in Scene.Components)
            {
                component.Start();
                
                component.ApplyFriction();
                if (!Scene.BottomWall.CollidesWith(component.Primitive, collisionDetector))
                {
                    component.ApplyForce(new Vector2(0.0f, 0.9f));
                }
                
                component.ApplyAcceleration();
                if (Scene.Obstacles.Any(o => component.Primitive.CollidesWith(o.Primitive,
                    collisionDetector)))
                {
                    component.Velocity *= -1.0f;
                }

                if (Scene.TopWall.CollidesWith(component.Primitive, collisionDetector) || Scene.BottomWall.CollidesWith(component.Primitive, collisionDetector))
                {
                    component.Velocity *= new Vector2(1.0f, -1.0f);
                }

                if (Scene.RightWall.CollidesWith(component.Primitive, collisionDetector) || Scene.LeftWall.CollidesWith(component.Primitive, collisionDetector))
                {
                    component.Velocity *= new Vector2(-1.0f, 1.0f);
                }
                
                component.ApplyVelocity();
                component.Update();
            }

            Redraw();
        }

        public void Redraw()
        {
            OnRedraw?.Invoke(this, Scene);
        }

        public event EventHandler<PhysicsScene> OnRedraw;
    }
}
