using Microsoft.AspNetCore.Components;
using SpenSoft.DanBeamNG.Services;
using SpenSoft.BeamNG.VehicleObjects;

namespace SpenSoft.DanBeamNG.Components
{
    public partial class ConfigurationCard : ComponentBase
    {
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Constructors
        #endregion
        ////////////////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Parameters

        [Parameter]
        public VConfiguration? config { get; set; } = default;

        [Parameter]
        public String? body { get; set; }

        [Parameter]
        public String? cls { get; set; }

        [Parameter]
        public String? country { get; set; }

        [Parameter]
        public String? drive { get; set; }

        [Parameter]
        public EventCallback<VConfiguration> ImageLookup_Clicked { get; set; }

        [Parameter]
        public Boolean AllowEdit { get; init; }

        [Parameter]
        public EventCallback<Guid?> VehicleImageAssigned { get; set; }

        [Parameter]
        public EventCallback<int> DeleteConfigHandler { get; set; }

        #endregion Parameters
        ////////////////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Injections

        [Inject]
        public VehicleDataService_Interface? vehicleDataService { get; set; }

        #endregion Injections
        ////////////////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Public Properties
        #endregion Public Properties
        ////////////////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Screen State Properties
        #endregion Screen State Properties
        ////////////////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Override Methods
        #endregion Override Methods
        ////////////////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Protected Methods
        #endregion
        ////////////////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Public Methods

        public void PopupHasClosed()
        {
            StateHasChanged();
        }

        #endregion
        ////////////////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Private Methods


#if false
        private async void AssignToVehicle()
        {
            if (vehicleDataService != null)
            {
                if (config != null)
                {
                    if ((config.ImageID.HasValue == true) && (config.ImageID.Value != Guid.Empty))
                    {
                        Vehicle v = await vehicleDataService.GetVehicleByID(config.VehicleID);
                        v.ImageID = config.ImageID;
                        await vehicleDataService.UpdateVehicle(v);
                        StateHasChanged();
                    }
                }
            }
        }
#endif

        private async void AssignImageToConfig() => await ImageLookup_Clicked.InvokeAsync(config);

        private async void AssignImageToVehicle() => await VehicleImageAssigned.InvokeAsync(config?.ImageID);

        private async void AssignImageToConfiguration()
        {
            StateHasChanged();
            await ImageLookup_Clicked.InvokeAsync(config);
        }

        private async void DeleteConfigClick() => await DeleteConfigHandler.InvokeAsync(config.ID);

        #endregion
        ////////////////////////////////////////////////////////////////////////////////////////////////////

    }
}
