using System;
using System.Numerics;
using System.Threading.Tasks;
using Aptacode.Geometry.Primitives;
using Aptacode.Geometry.Primitives.Polygons;
using Microsoft.AspNetCore.Components;

namespace Aptacode.Physics.Blazor.Pages
{
    public class IndexBase : ComponentBase
    {
        #region Lifecycle

        protected override async Task OnInitializedAsync()
        {
            var componentBuilder = new PhysicsComponentBuilder();

            var wall1 = componentBuilder.SetPhysics(false).SetPrimitive(Rectangle.Create(0, -9, 100, 10)).Build();
            var wall2 = componentBuilder.SetPhysics(false).SetPrimitive(Rectangle.Create(99, 0, 10, 50)).Build();
            var wall3 = componentBuilder.SetPhysics(false).SetPrimitive(Rectangle.Create(0, 49, 100, 10)).Build();
            var wall4 = componentBuilder.SetPhysics(false).SetPrimitive(Rectangle.Create(-9, 0, 10, 50)).Build();

            var physicsSceneBuilder = new PhysicsSceneBuilder();
            physicsSceneBuilder.SetWidth(100).SetHeight(50);
            physicsSceneBuilder.AddComponent(wall1).AddComponent(wall2).AddComponent(wall3).AddComponent(wall4);

            for (var i = 0; i < 30; i++)
            {
                var radius = _rand.Next(5, 20);
                var primitiveN = componentBuilder.SetMass(radius * 150)
                    .SetVelocity(new Vector2(_rand.Next(0, 5), _rand.Next(0, 5)))
                    .SetPrimitive(Ellipse.Create(_rand.Next(0, 100), _rand.Next(0, 50), radius / 10.0f)).Build();
                physicsSceneBuilder.AddComponent(primitiveN);
            }

            SceneController = new PhysicsSceneController(physicsSceneBuilder.Build(), new Vector2(100, 50));

            await base.OnInitializedAsync();
        }

        #endregion

        #region Properties

        public PhysicsSceneController SceneController { get; set; }
        private readonly Random _rand = new();

        #endregion
    }
}