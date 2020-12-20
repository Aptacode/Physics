using System;
using System.Drawing;
using System.Numerics;
using System.Threading.Tasks;
using Aptacode.Geometry.Blazor.Utilities;
using Aptacode.Geometry.Primitives;
using Microsoft.AspNetCore.Components;
using Rectangle = Aptacode.Geometry.Primitives.Polygons.Rectangle;

namespace Aptacode.Physics.Blazor.Pages
{
    public class IndexBase : ComponentBase
    {
        #region Lifecycle

        protected override async Task OnInitializedAsync()
        {
            var physicsComponentBuilder = new PhysicsComponentBuilder();
            var componentBuilder = new ComponentBuilder();

            var wall1 = physicsComponentBuilder.SetPhysics(false)
                .SetComponent(componentBuilder.SetPrimitive(Rectangle.Create(0, -9, 100, 10)).Build()).Build();
            var wall2 = physicsComponentBuilder.SetPhysics(false)
                .SetComponent(componentBuilder.SetPrimitive(Rectangle.Create(99, 0, 10, 50)).Build()).Build();
            var wall3 = physicsComponentBuilder.SetPhysics(false)
                .SetComponent(componentBuilder.SetPrimitive(Rectangle.Create(0, 49, 100, 10)).Build()).Build();
            var wall4 = physicsComponentBuilder.SetPhysics(false)
                .SetComponent(componentBuilder.SetPrimitive(Rectangle.Create(-9, 0, 10, 50)).Build()).Build();

            var physicsSceneBuilder = new PhysicsSceneBuilder();
            physicsSceneBuilder.SetWidth(100).SetHeight(50);
            physicsSceneBuilder.AddComponent(wall1).AddComponent(wall2).AddComponent(wall3).AddComponent(wall4);

            for (var i = 0; i < 20; i++)
            {
                var radius = _rand.Next(5, 20);
                var primitiveN = physicsComponentBuilder.SetMass(radius * 100)
                    .SetVelocity(new Vector2(_rand.Next(0, 5), _rand.Next(0, 5)))
                    .SetComponent(componentBuilder
                        .SetPrimitive(Ellipse.Create(_rand.Next(10, 90), _rand.Next(10, 40), radius / 10.0f))
                        .SetFillColor(Color.Gray).Build()).Build();
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