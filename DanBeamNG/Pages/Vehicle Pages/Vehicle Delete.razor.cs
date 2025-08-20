using Microsoft.AspNetCore.Components;
using SpenSoft.DanBeamNG.Services;
using SpenSoft.BeamNG.VehicleObjects;

namespace SpenSoft.DanBeamNG.Pages.Vehicle_Pages
{
    public partial class Vehicle_Delete : ComponentBase
    {
        [Parameter]
        public String? ID { get; set; }

        public Vehicle? Vehicle { get; set; }

        ////////////////////////////////////////////////////////////////////
        // Store Screen State
        protected String Message = String.Empty;
        protected String StatusClass = String.Empty;
        protected Boolean Deleted;
        // 
        ////////////////////////////////////////////////////////////////////

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

                    if (Vehicle != null)
                    {
                        Deleted = await VehicleDataService.DeleteVehicle(id);
                        if (Deleted == true)
                        {
                            StatusClass = "alert-success";
                            Message = $"The Vehicle: {Vehicle.Name} has been deleted";
                        }
                        else
                        {
                            StatusClass = "alert-warning";
                            Message = $"The Vehicle with ID: {ID} could not be found";
                        }
                    }
                    else
                    {
                        StatusClass = "alert-warning";
                        Message = $"The Vehicle with ID: {ID} could not be found";
                        Deleted = false;
                    }
                }
                else
                {
                    StatusClass = "alert-danger";
                    Message = $"The VehicleDataService object is null";
                }
            }
        }
    }
}
