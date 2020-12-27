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
            var width = 720;
            var height = 480;

            var wall1 = physicsComponentBuilder.SetPhysics(false)
                .SetComponent(componentBuilder.SetPrimitive(Rectangle.Create(0, 0, width, 10)).Build()).Build();
            var wall2 = physicsComponentBuilder.SetPhysics(false)
                .SetComponent(componentBuilder.SetPrimitive(Rectangle.Create(width - 10, 0, 10, height)).Build()).Build();
            var wall3 = physicsComponentBuilder.SetPhysics(false)
                .SetComponent(componentBuilder.SetPrimitive(Rectangle.Create(0, height - 10, width, 10)).Build()).Build();
            var wall4 = physicsComponentBuilder.SetPhysics(false)
                .SetComponent(componentBuilder.SetPrimitive(Rectangle.Create(10, 0, 10, height)).Build()).Build();

            var physicsSceneBuilder = new PhysicsSceneBuilder();
            physicsSceneBuilder.SetWidth(width).SetHeight(height);
            physicsSceneBuilder.AddComponent(wall1).AddComponent(wall2).AddComponent(wall3).AddComponent(wall4);

            var maxRadius = 50;
            var minRadius = 10;
            for (var i = 0; i < 10; i++)
            {
                var radius = _rand.Next(minRadius, maxRadius);
                var primitiveN = physicsComponentBuilder.SetMass(radius * 500)
                    .SetVelocity(new Vector2(_rand.Next(0, 40), _rand.Next(0, 40)))
                    .SetComponent(componentBuilder
                        .SetPrimitive(
                            Ellipse.Create(
                                _rand.Next(maxRadius, width - maxRadius), 
                                _rand.Next(maxRadius, height - maxRadius), 
                                radius, radius, 0.0f))
                        .SetFillColor(Color.Gray).Build()).Build();
                physicsSceneBuilder.AddComponent(primitiveN);
            }

            SceneController = new PhysicsSceneController(physicsSceneBuilder.Build(), new Vector2(width, height));

            await base.OnInitializedAsync();
        }

        #endregion

        #region Properties

        public PhysicsSceneController SceneController { get; set; }
        private readonly Random _rand = new();

        #endregion
    }
}