using Microsoft.AspNetCore.Components;
using SpenSoft.DanBeamNG.Services;
using SpenSoft.BeamNG.VehicleObjects;

namespace SpenSoft.DanBeamNG.Pages.ImagePages
{
    public partial class Image_View : ComponentBase
    {
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Constructors
        #endregion
        ////////////////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Parameters

        [Parameter]
        public String? ImageID { get; set; }

        #endregion Parameters
        ////////////////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Injections

        [Inject]
        public VehicleImageDataService_Interface? imageDataService { get; set; }

        [Inject]
        public ImagesDataDataService_Interface? imagesDataDataService { get; set; }

        #endregion Injections
        ////////////////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Public Properties

        public VehicleImage? vehicleImage { get; set; } = null;

        public ImagesData? imagesData { get; set; } = null;

        #endregion Public Properties
        ////////////////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Screen State Properties

        protected String Message = String.Empty;
        protected String StatusClass = String.Empty;
        protected Boolean Saved = true;

        #endregion Screen State Properties
        ////////////////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Override Methods

        protected override async Task OnInitializedAsync()
        {
            if (String.IsNullOrEmpty(ImageID) == false)
            {
                Guid id = Guid.Parse(ImageID);

                if (id != Guid.Empty)
                {
                    if (imageDataService == null)
                    {
                        StatusClass = "alert-danger";
                        Message = $"The configurationDataService object is null";
                        Saved = false;
                        return;
                    }

                    vehicleImage = await imageDataService.GetVehicleImageById(id);

                    if (imagesDataDataService == null)
                    {
                        StatusClass = "alert-danger";
                        Message = $"The imagesDataDataService object is null";
                        Saved = false;
                        return;
                    }
                    imagesData = await imagesDataDataService.GetImagesDataByID(id);
                }
            }
        }

        #endregion Override Methods
        ////////////////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Protected Methods

        protected async Task InvalidSubmitHandler()
        {
            StatusClass = "alert-danger";
            Message = "The configuration data had validation errors.  Please correct and try again";
        }

        protected async Task ValidSumitHandler()
        {
            Saved = false;
            if (vehicleImage == null)
            {
                StatusClass = "alert-danger";
                Message = "The new configuration could not be dded";
                return;
            }

            if (imageDataService == null)
            {
                StatusClass = "alert-danger";
                Message = $"The configurationDataService object is null";
                return;
            }

            await imageDataService.UpdateVehicleImage(vehicleImage);

            StatusClass = "alert-success";
            Message = "vehicle Image has been updated";
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
