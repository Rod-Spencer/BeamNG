using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace SpenSoft.DanBeamNG.Components
{
    public partial class Input_Text
    {
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Parameters

        [Parameter]
        public String? Label { get; set; } = "";

        [Parameter]
        public String? Data { get; set; } = "";

        [Parameter]
        public String? LabelWidth { get; set; } = "40%";

        [Parameter]
        public String? InputWidth { get; set; } = "60%";

        [Parameter]
        public String? Placeholder { get; set; } = "Enter";

        #endregion Parameters
        ////////////////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Public Properties
        public String? InputValue
        {
            get => Data;
            set
            {
                if (Data != value)
                {
                    Data = value;
                }
            }
        }

        #endregion Public Properties
        ////////////////////////////////////////////////////////////////////////////////////////////////////


    }
}
