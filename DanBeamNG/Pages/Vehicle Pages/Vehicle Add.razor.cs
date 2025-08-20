using Microsoft.AspNetCore.Components;
using SpenSoft.DanBeamNG.Services;
using SpenSoft.BeamNG.VehicleObjects;

namespace SpenSoft.DanBeamNG.Pages.Vehicle_Pages
{
    public partial class Vehicle_Add : ComponentBase
    {
        public Vehicle? Vehicle { get; set; } = new Vehicle();

        [Inject]
        public VehicleDataService_Interface? VehicleDataService { get; set; }



        ////////////////////////////////////////////////////////////////////
        // Store Screen State
        protected String Message = String.Empty;
        protected String StatusClass = String.Empty;
        protected Boolean Saved;
        // 
        ////////////////////////////////////////////////////////////////////


        protected override async Task OnInitializedAsync()
        {
            Saved = false;
        }

        protected async Task InvalidSubmitHandler()
        {
            StatusClass = "alert-danger";
            Message = "The Vehicle data had validation errors.  Please correct and try again";
        }

        protected async Task ValidSumitHandler()
        {
            Saved = false;
            if (Vehicle != null)
            {
                if (Vehicle.ID == 0)
                {
                    if (VehicleDataService != null)
                    {
                        Vehicle? addedVehicle = await VehicleDataService.AddVehicle(Vehicle);
                        if (addedVehicle != null)
                        {
                            StatusClass = "alert-success";
                            Message = "New Vehicle added";
                            Saved = true;
                        }
                        else
                        {
                            StatusClass = "alert-danger";
                            Message = "The new Vehicle could not be added";
                            Saved = false;
                        }
                    }
                    else
                    {
                        StatusClass = "alert-danger";
                        Message = "The VehicleDataService is null";
                        Saved = false;
                    }
                }
                else if (VehicleDataService != null)
                {
                    await VehicleDataService.UpdateVehicle(Vehicle);
                    StatusClass = "alert-success";
                    Message = "Vehicle data updated";
                    Saved = true;
                }
                else
                {
                    StatusClass = "alert-danger";
                    Message = "The VehicleDataService is null";
                    Saved = false;
                }
            }
            else
            {
                StatusClass = "alert-danger";
                Message = "The Vehicle object is null";
                Saved = false;
            }
        }
    }
}
