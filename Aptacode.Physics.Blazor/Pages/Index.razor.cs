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
                .SetComponent(componentBuilder.SetPrimitive(Rectangle.Create(0, -9, 200, 10)).Build()).Build();
            var wall2 = physicsComponentBuilder.SetPhysics(false)
                .SetComponent(componentBuilder.SetPrimitive(Rectangle.Create(199, 0, 10, 100)).Build()).Build();
            var wall3 = physicsComponentBuilder.SetPhysics(false)
                .SetComponent(componentBuilder.SetPrimitive(Rectangle.Create(0, 100, 200, 10)).Build()).Build();
            var wall4 = physicsComponentBuilder.SetPhysics(false)
                .SetComponent(componentBuilder.SetPrimitive(Rectangle.Create(-9, 0, 10, 100)).Build()).Build();

            var physicsSceneBuilder = new PhysicsSceneBuilder();
            physicsSceneBuilder.SetWidth(200).SetHeight(100);
            physicsSceneBuilder.AddComponent(wall1).AddComponent(wall2).AddComponent(wall3).AddComponent(wall4);

            for (var i = 0; i < 40; i++)
            {
                var radius = _rand.Next(1, 5);
                var primitiveN = physicsComponentBuilder.SetMass(radius * 100)
                    .SetVelocity(new Vector2(_rand.Next(0, 5), _rand.Next(0, 5)))
                    .SetComponent(componentBuilder
                        .SetPrimitive(Ellipse.Create(_rand.Next(50, 150), _rand.Next(25, 75), radius, radius, 0.0f))
                        .SetFillColor(Color.Gray).Build()).Build();
                physicsSceneBuilder.AddComponent(primitiveN);
            }

            SceneController = new PhysicsSceneController(physicsSceneBuilder.Build(), new Vector2(200, 100));

            await base.OnInitializedAsync();
        }

        #endregion

        #region Properties

        public PhysicsSceneController SceneController { get; set; }
        private readonly Random _rand = new();

        #endregion
    }
}