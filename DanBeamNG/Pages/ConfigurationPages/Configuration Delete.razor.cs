using Microsoft.AspNetCore.Components;
using SpenSoft.BeamNG.VehicleObjects;
using SpenSoft.DanBeamNG.Services;

namespace SpenSoft.DanBeamNG.Pages.ConfigurationPages
{
    public partial class Configuration_Delete : ComponentBase
    {
        [Parameter]
        public String? ID { get; set; }


        public VConfiguration? Configuration { get; set; }

        [Inject]
        public ConfigurationDataService_Interface? ConfigurationDataService { get; set; }

        [Inject]
        public VehicleDataService_Interface? VehicleDataService { get; set; }


        public Vehicle? Vehicle { get; set; }


        ////////////////////////////////////////////////////////////////////
        // Store Screen State
        protected String Message = String.Empty;
        protected String StatusClass = String.Empty;
        protected Boolean Deleted;
        // 
        ////////////////////////////////////////////////////////////////////


        protected override async Task OnInitializedAsync()
        {
            if (String.IsNullOrEmpty(ID) == false)
            {
                int id = int.Parse(ID);
                if (ConfigurationDataService != null) Configuration = await ConfigurationDataService.GetConfigurationByID(id);
                if (Configuration != null)
                {
                    if (VehicleDataService != null) Vehicle = await VehicleDataService.GetVehicleByID(Configuration.VehicleID);
                    if (Vehicle == null)
                    {
                        StatusClass = "alert-warning";
                        Message = $"The vehicle with ID: {Configuration.VehicleID} could not be found";
                        Deleted = false;
                        return;
                    }

                    if (ConfigurationDataService != null) Deleted = await ConfigurationDataService.DeleteConfiguration(id);
                    if (Deleted == true)
                    {
                        StatusClass = "alert-success";
                        Message = $"The configuration: {Configuration.Name} has been deleted";
                    }
                    else
                    {
                        StatusClass = "alert-danger";
                        Message = $"The configuration: {Configuration.Name} could not be deleted";
                        Deleted = false;
                    }
                }
                else
                {
                    StatusClass = "alert-danger";
                    Message = $"The configuration with ID: {id} could not be found";
                    Deleted = false;
                }
            }
        }
    }
}
