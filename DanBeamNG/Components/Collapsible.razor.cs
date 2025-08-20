using Microsoft.AspNetCore.Components;

namespace SpenSoft.DanBeamNG.Components
{
    public partial class Collapsible
    {
        [Parameter]
        public RenderFragment? ChildContent { get; set; }
    }
}
