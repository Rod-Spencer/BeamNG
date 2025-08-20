using Microsoft.AspNetCore.Components;
using SpenSoft.BeamNG.VehicleObjects;

namespace SpenSoft.DanBeamNG.Components
{
    public partial class VehicleNameEdit : ComponentBase
    {
        [Parameter]
        public Vehicle? vehicle { get; set; }

        [Parameter]
        public Boolean showImage { get; set; } = true;

        [Parameter]
        public Boolean viewOnly { get; set; } = false;
    }
}
