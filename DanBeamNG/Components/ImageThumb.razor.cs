using Microsoft.AspNetCore.Components;
using SpenSoft.BeamNG.VehicleObjects;
using SpenSoft.DanBeamNG.Services;

namespace SpenSoft.DanBeamNG.Components
{
    public partial class ImageThumb : ComponentBase
    {
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Constructors
        #endregion
        ////////////////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Parameters

        [Parameter]
        public Guid? imageID { get; set; }

        [Parameter]
        public String? imageName { get; set; }

        [Parameter]
        public VConfiguration? config { get; set; }

        [Parameter]
        public EventCallback<Guid?> imageCallBack { get; set; }

        #endregion Parameters
        ////////////////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Injections

        [Inject]
        public ConfigurationDataService_Interface? configurationDataService { get; set; }

        [Inject]
        public NavigationManager? navigationManager { get; set; }


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
        #endregion
        ////////////////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Private Methods

        private String? GetImage() => $"/Images/{imageID}.jpg";

        private async void Assign() => await imageCallBack.InvokeAsync(imageID);

#if false

        {
            try
            {
                //if (clientLogger == null)
                //{
                //    return;
                //}

                if (configurationDataService == null)
                {
                    clientLogger?.AddLoggingMessage("configurationDataService == null", "warn");
                    return;
                }

                if (navigationManager == null)
                {
                    clientLogger?.AddLoggingMessage("navigationManager == null", "warn");
                    return;
                }

                if (config == null)
                {
                    clientLogger?.AddLoggingMessage("config == null", "error");
                    return;
                }


                if (imageID != null)
                {
                    clientLogger?.AddLoggingMessage("image (vehicleImage) != null");
                    config.ImageID = imageID;
                    String msg = $"vConfiguration: {JsonConvert.SerializeObject(config)}";
                    clientLogger?.AddLoggingMessage(msg);

                    clientLogger?.AddLoggingMessage("Updating Configuration");
                    configurationDataService.UpdateConfiguration(config);
                    clientLogger?.AddLoggingMessage("Updated Configuration");

                    String url = $"EditVehicle/{config.VehicleID}";
                    clientLogger?.AddLoggingMessage($"Navigating to: {url}");
                    navigationManager.NavigateTo(url, true);
                }
                else
                {
                    clientLogger?.AddLoggingMessage("image (vehicleImage) == null", "error");
                }
            }
            catch (Exception e)
            {
                clientLogger?.AddLoggingMessage(e.Message, "Error");
            }
            finally
            {
                config = null;
                StateHasChanged();
            }
        }
#endif

        #endregion
        ////////////////////////////////////////////////////////////////////////////////////////////////////

    }
}
