using Microsoft.AspNetCore.Components;

namespace SpenSoft.DanBeamNG.Components
{
    public partial class RenderComponent : ComponentBase
    {
        [Parameter]
        public RenderFragment? ChildContent { get; set; }
    }
}
