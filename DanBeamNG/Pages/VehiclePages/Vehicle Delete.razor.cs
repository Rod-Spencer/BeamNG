using Microsoft.AspNetCore.Components;
using SpenSoft.BeamNG.VehicleObjects;
using SpenSoft.DanBeamNG.Services;

namespace SpenSoft.DanBeamNG.Pages.VehiclePages
{
    public partial class Vehicle_Delete : ComponentBase
    {
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Constructors
        #endregion
        ////////////////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Parameters

        [Parameter]
        public String? ID { get; set; }

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

        public Vehicle? Vehicle { get; set; }

        #endregion Public Properties
        ////////////////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Screen State Properties

        protected String Message = String.Empty;
        protected String StatusClass = String.Empty;
        protected Boolean Deleted;

        #endregion Screen State Properties
        ////////////////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Override Methods
        #endregion Override Methods
        ////////////////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Protected Methods

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
                            //var v = Vehicles.AllVehicles?.FirstOrDefault(x => x.ID == vehicle.ID);
                            //Vehicles.AllVehicles.Remove(v);

                            StatusClass = "alert-success";
                            Message = $"The vehicle: {Vehicle.Name} has been deleted";
                        }
                        else
                        {
                            StatusClass = "alert-warning";
                            Message = $"The vehicle with ID: {ID} could not be found";
                        }
                    }
                    else
                    {
                        StatusClass = "alert-warning";
                        Message = $"The vehicle with ID: {ID} could not be found";
                        Deleted = false;
                    }
                }
                else
                {
                    StatusClass = "alert-danger";
                    Message = $"The configurationDataService object is null";
                }
            }
        }

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
