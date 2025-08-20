using Microsoft.AspNetCore.Components;

namespace SpenSoft.DanBeamNG.Components
{
    public partial class Display_Text
    {
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Parameters

        [Parameter]
        public String? Label { get; set; }

        [Parameter]
        public String? Data { get; set; }

        [Parameter]
        public String? LabelWidth { get; set; } = "40%";

        [Parameter]
        public String? DataWidth { get; set; } = "60%";

        #endregion Parameters
        ////////////////////////////////////////////////////////////////////////////////////////////////////


    }
}
