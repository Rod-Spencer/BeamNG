using Microsoft.AspNetCore.Components;
using SpenSoft.DanBeamNG.Services;
using SpenSoft.BeamNG.VehicleObjects;

namespace SpenSoft.DanBeamNG.Pages.Vehicle_Pages
{
    public partial class Vehicle_List : ComponentBase
    {
        [Parameter]
        public string ID { get; set; }

        public IEnumerable<Vehicle>? VehicleList { get; set; }

        public Vehicle? Vehicle { get; set; } = new Vehicle();

        [Inject]
        public VehicleDataService_Interface VehicleDataService { get; set; }

        ////////////////////////////////////////////////////////////////////
        // Store Screen State
        protected String Message = String.Empty;
        protected String StatusClass = String.Empty;
        protected Boolean Saved;
        // 
        ////////////////////////////////////////////////////////////////////


        protected override async Task OnInitializedAsync()
        {
            VehicleList = (await VehicleDataService.GetAllVehicle()).ToList();

            //int.TryParse(ID, out var id);

            //if (id != 0)
            //{
            //    Vehicle = await VehicleDataService.GetVehicleByID(id);
            //}

        }

        protected async Task DeleteVehicle()
        {
            await VehicleDataService.DeleteVehicle(Vehicle.ID);

            StatusClass = "alert-success";
            Message = "Deleted successfully";

            Saved = true;
        }
    }
}
