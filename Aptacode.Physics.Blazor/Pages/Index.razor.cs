using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Aptacode.Geometry.Blazor.Components.ViewModels.Components.Primitives;
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
            var components = new List<PhysicsComponent>();

            var wall1 = physicsComponentBuilder.SetPhysics(false)
                .SetComponent(Rectangle.Create(0, 0, width, 10).ToViewModel()).Build();
            var wall2 = physicsComponentBuilder.SetPhysics(false)
                .SetComponent(Rectangle.Create(width - 10, 0, 10, height).ToViewModel()).Build();
            var wall3 = physicsComponentBuilder.SetPhysics(false)
                .SetComponent(Rectangle.Create(width - 10, 0, 10, height).ToViewModel()).Build();
            var wall4 = physicsComponentBuilder.SetPhysics(false)
                .SetComponent(Rectangle.Create(width - 10, 0, 10, height).ToViewModel()).Build();

            var physicsSceneBuilder = new PhysicsSceneBuilder();
            physicsSceneBuilder.SetWidth(width).SetHeight(height);
            components.Add(wall1);
            components.Add(wall2);
            components.Add(wall3);
            components.Add(wall4);

            var maxRadius = 20;
            var minRadius = 5;
            for (var i = 0; i < 20; i++)
            {
                var radius = _rand.Next(minRadius, maxRadius);
                var primitiveN = physicsComponentBuilder.SetMass(radius * 500)
                    .SetVelocity(new Vector2(_rand.Next(0, 40), _rand.Next(0, 40)))
                    .SetComponent(componentBuilder
                        .SetBase(
                            Ellipse.Create(
                                _rand.Next(maxRadius, width - maxRadius), 
                                _rand.Next(maxRadius, height - maxRadius), 
                                radius, radius, 0.0f).ToViewModel())
                        .SetFillColor(Color.Gray).Build()).Build();
                components.Add(primitiveN);
            }

            SceneController = new PhysicsSceneController(physicsSceneBuilder.Build(), new Vector2(width, height));

            SceneController.PhysicsEngine.Components.AddRange(components);
            SceneController.Scene.Components.AddRange(components.Select(c => c.Component).ToList());

            await base.OnInitializedAsync();
        }

        #endregion

        #region Properties

        public PhysicsSceneController SceneController { get; set; }
        private readonly Random _rand = new();

        #endregion
    }
}