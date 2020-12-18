using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Aptacode.Physics.Blazor.Pages
{
    public class IndexBase : ComponentBase
    {
        public PhysicsEngine Engine { get; set; }
        
        protected override async Task OnInitializedAsync()
        {
            Engine = new PhysicsEngine();
            Engine.OnRedraw += EngineOnOnRedraw;
            Engine.Start();


            await base.OnInitializedAsync();
        }

        private void EngineOnOnRedraw(object? sender, PhysicsScene e)
        {
            InvokeAsync(StateHasChanged);
        }
    }
}
