using System.Numerics;
using System.Threading.Tasks;
using Aptacode.Geometry.Primitives;
using Aptacode.Geometry.Primitives.Polygons;
using Microsoft.AspNetCore.Components;

namespace Aptacode.Physics.Blazor.Pages
{
    public class IndexBase : ComponentBase
    {
        public PhysicsSceneController SceneController { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var componentBuilder = new PhysicsComponentBuilder();

            var wall1 = componentBuilder.SetPhysics(false).SetPrimitive(Rectangle.Create(0, 0, 100, 1)).Build();
            var wall2 = componentBuilder.SetPhysics(false).SetPrimitive(Rectangle.Create(99, 0, 1, 50)).Build();
            var wall3 = componentBuilder.SetPhysics(false).SetPrimitive(Rectangle.Create(0, 49, 100, 1)).Build();
            var wall4 = componentBuilder.SetPhysics(false).SetPrimitive(Rectangle.Create(0, 0, 1, 50)).Build();

            var primitive1 = componentBuilder.SetMass(10).SetVelocity(new Vector2(1,1)).SetPrimitive(Ellipse.Create(20, 20, 1.0f)).Build();
            var primitive2 = componentBuilder.SetMass(40).SetVelocity(new Vector2(-10,1)).SetPrimitive(Ellipse.Create(30, 20, 4.0f)).Build();
            var primitive3 = componentBuilder.SetMass(1000).SetVelocity(new Vector2(5,1)).SetPrimitive(Ellipse.Create(35, 35, 10.0f)).Build();

            var physicsSceneBuilder = new PhysicsSceneBuilder();
            physicsSceneBuilder.SetWidth(100).SetHeight(50);
            physicsSceneBuilder.AddComponent(wall1).AddComponent(wall2).AddComponent(wall3).AddComponent(wall4);
            physicsSceneBuilder.AddComponent(primitive1).AddComponent(primitive2).AddComponent(primitive3);

            SceneController = new PhysicsSceneController(physicsSceneBuilder.Build(), new Vector2(100, 50));

            await base.OnInitializedAsync();
        }
    }
}
