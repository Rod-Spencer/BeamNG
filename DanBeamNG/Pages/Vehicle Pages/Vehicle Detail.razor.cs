using Microsoft.AspNetCore.Components;
using SpenSoft.DanBeamNG.Services;
using SpenSoft.BeamNG.VehicleObjects;

namespace SpenSoft.DanBeamNG.Pages.Vehicle_Pages
{
    public partial class Vehicle_Detail : ComponentBase
    {
        [Parameter]
        public String? ID { get; set; }


        public Vehicle? Vehicle { get; set; }

        [Inject]
        public VehicleDataService_Interface? VehicleDataService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            if (String.IsNullOrEmpty(ID) == false)
            {
                int id = int.Parse(ID);
                if (VehicleDataService != null)
                {
                    Vehicle = (await VehicleDataService.GetVehicleByID(id));
                }
            }
        }

    }
}
