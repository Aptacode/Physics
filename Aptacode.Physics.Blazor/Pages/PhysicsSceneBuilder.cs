using System.Collections.Generic;
using System.Linq;
using Aptacode.Geometry.Blazor.Utilities;
using Aptacode.Geometry.Collision;

namespace Aptacode.Physics.Blazor.Pages
{
    public class PhysicsSceneBuilder
    {
        private readonly List<PhysicsComponent> _components = new();
        private readonly ViewModelFactory _viewModelFactory = new();
        private float _height;
        private float _width;
        private CollisionDetector _collisionDetector = new HybridCollisionDetector();


        public PhysicsSceneBuilder SetCollisionDetector(CollisionDetector collisionDetector)
        {
            _collisionDetector = collisionDetector;
            return this;
        }
        
        public PhysicsSceneBuilder SetWidth(float width)
        {
            _width = width;
            return this;
        }

        public PhysicsSceneBuilder SetHeight(float height)
        {
            _height = height;
            return this;
        }

        public PhysicsSceneBuilder AddComponent(PhysicsComponent component)
        {
            _components.Add(component);
            return this;
        }

        public PhysicsEngine Build()
        {
            var engine = new PhysicsEngine(_components, _collisionDetector)
            {
                Components = _components.ToList()
            };
            
            Reset();
            return engine;
        }

        public void Reset()
        {
            _width = 0.0f;
            _height = 0.0f;
            _components.Clear();
        }
    }
}