using Microsoft.AspNetCore.Components;
using SpenSoft.BeamNG.VehicleObjects;
using SpenSoft.DanBeamNG.Services;
using System.Timers;

namespace SpenSoft.DanBeamNG.Pages.VehiclePages
{
    public partial class Vehicle_Add : ComponentBase
    {
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Constructors
        #endregion
        ////////////////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Parameters
        #endregion Parameters
        ////////////////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Injections

        [Inject]
        public VehicleDataService_Interface? VehicleDataService { get; set; }

        #endregion Injections
        ////////////////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Public Properties

        public Vehicle? Vehicle { get; set; } = null;

        public Boolean Saved = false;

        #endregion Public Properties
        ////////////////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Screen State Properties

        protected String Message = String.Empty;
        protected String StatusClass = String.Empty;

        private System.Threading.Timer? timer = null;
        private static Vehicle? vehicle = null;

        #endregion Screen State Properties
        ////////////////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Override Methods

        protected override Task OnInitializedAsync()
        {
            Vehicle = new Vehicle();
            return Task.CompletedTask;
        }

        #endregion Override Methods
        ////////////////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Protected Methods

        protected void InvalidSubmitHandler()
        {
            StatusClass = "alert-danger";
            Message = "The vehicle data had validation errors.  Please correct and try again";
        }

        protected async Task ValidSumitHandler()
        {
            if (Vehicle != null)
            {
                if (Vehicle.ID != 0) Vehicle.ID = 0;

                if (VehicleDataService != null)
                {
                    Vehicle? testVehicle = await VehicleDataService.GetVehicleByName(Vehicle.Name);
                    if (testVehicle != null)
                    {
                        StatusClass = "alert-danger";
                        Message = $"A vehicle with the name of {testVehicle.Name} already exists";
                    }
                    else
                    {
                        Vehicle? addedVehicle = await VehicleDataService.AddVehicle(Vehicle);
                        if (addedVehicle != null)
                        {
                            StatusClass = "alert-success";
                            Message = "New vehicle added";
                            Vehicle = addedVehicle;
                            //vehicle = (await VehicleDataService.GetAllVehicles()).FirstOrDefault(x => x.Name == vehicle.Name);
                        }
                        else
                        {
                            StatusClass = "alert-danger";
                            Message = "The new vehicle could not be added";
                        }
                    }
                }
                else
                {
                    StatusClass = "alert-danger";
                    Message = "The configurationDataService is null";
                }
                //}
                //else if (VehicleDataService != null)
                //{
                //    await VehicleDataService.UpdateVehicle(vehicle);

                //    StatusClass = "alert-success";
                //    Message = "vehicle data updated";
                //}
                //else
                //{
                //    StatusClass = "alert-danger";
                //    Message = "The configurationDataService is null";
                //}
            }
            else
            {
                StatusClass = "alert-danger";
                Message = "The vehicle object is null";
            }
            Saved = true;
            StateHasChanged();
            timer = new System.Threading.Timer((object? stateInfo) =>
            {
                timer?.Dispose();
                timer = null;

                Saved = false;
                StateHasChanged();
            }, new System.Threading.AutoResetEvent(false), 5000, 10000);

        }

        //private void TimerCallBack(object source, ElapsedEventArgs e)
        //{
        //    //timer.Elapsed -= TimerCallBack;
        //    timer?.Dispose();
        //    timer = null;

        //    Saved = false;
        //    StateHasChanged();
        //}

        #endregion
        ////////////////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Public Methods
        #endregion
        ////////////////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Private Methods
        #endregion
        ////////////////////////////////////////////////////////////////////////////////////////////////////
    }
}
