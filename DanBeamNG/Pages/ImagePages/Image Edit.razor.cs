using Microsoft.AspNetCore.Components;
using SpenSoft.DanBeamNG.Services;
using SpenSoft.BeamNG.VehicleObjects;

namespace SpenSoft.DanBeamNG.Pages.ImagePages
{
    public partial class Image_Edit : ComponentBase
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
        public NavigationManager? NavigationManager { get; set; }

        [Inject]
        public ImagesDataDataService_Interface? imagesDataService { get; set; }

        #endregion Injections
        ////////////////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Public Properties

        public List<VConfiguration>? Configurations { get; set; }

        public VehicleImage? vehicleImage { get; set; } = null;

        public ImagesData? imagesData { get; set; }

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

                vehicleImage = null;
                if (imageDataService == null)
                {
                    StatusClass = "alert-danger";
                    Message = $"The configurationDataService object is null";
                    Saved = false;
                    return;
                }

                vehicleImage = await imageDataService.GetVehicleImageById(id);

                imagesData = null;
                if (imagesDataService != null) imagesData = await imagesDataService.GetImagesDataByID(id);
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

#if false

        private async void LoadImage(InputFileChangeEventArgs e)
        {
            ImageName = e.File.Name;
            vehicleImage = await imageDataService.GetVehicleImageByName(ImageName);
            if (vehicleImage != null)
            {
                ImageName = vehicleImage.ImageName;
                ImageIDString = vehicleImage.ImageID.ToString();
                Configuration.ImageID = vehicleImage.ImageID;
                if (ConfigurationDataService != null)
                {
                    await ConfigurationDataService.UpdateConfiguration(Configuration);
                    NavigationManager?.NavigateTo($"EditConfiguration/{ID}/{Configuration.ImageID}");
                }

                return;
            }
            vehicleImage = new VehicleImage() { ImageID = Guid.NewGuid(), ImageName = this.ImageName };
            MemoryStream ms = new MemoryStream();
            await e.File.OpenReadStream().CopyToAsync(ms);
            vehicleImage.ImageData = ms.ToArray();
            await imageDataService.AddVehicleImage(vehicleImage);

            Configuration.ImageID = vehicleImage.ImageID;
            if (ConfigurationDataService != null)
            {
                await ConfigurationDataService.UpdateConfiguration(Configuration);
                NavigationManager?.NavigateTo($"EditConfiguration/{ID}/{Configuration.ImageID}");
            }
        }


        private async void NameChanged(String name)
        {
            if (vehicleImage == null)
            {
                isDisabled = true;
                return;
            }
            VehicleImage vi = await imageDataService.GetVehicleImageById(vehicleImage.ImageID);
            if (vi != null)
            {
                vi.ImageName = name;
                vehicleImage.ImageName = name;
                await imageDataService.UpdateVehicleImage(vehicleImage);
            }
            ImageName = name;

            if (String.IsNullOrEmpty(ImageName) == true) isDisabled = true;
            else if (vehicleImage == null) isDisabled = true;
            else isDisabled = false;
        }

        private async void ImageID_Changed(String imageID)
        {
            if (String.IsNullOrEmpty(imageID) == true) return;
            Guid guid = Guid.Empty;
            Guid.TryParse(imageID, out guid);
            if (guid == Guid.Empty) return;
            if (imageDataService != null)
            {
                vehicleImage = await imageDataService.GetVehicleImageById(guid);
                if (vehicleImage != null)
                {
                    ImageName = vehicleImage.ImageName;
                    ImageIDString = vehicleImage.ImageID.ToString();
                    Configuration.ImageID = guid;
                    if (ConfigurationDataService != null)
                    {
                        await ConfigurationDataService.UpdateConfiguration(Configuration);
                        NavigationManager?.NavigateTo($"EditConfiguration/{ID}/{Configuration.ImageID}");
                    }

                }
                else
                {
                    ImageName = null;
                    ImageIDString = Guid.Empty.ToString();
                }
            }
            else
            {
                ImageName = null;
                ImageIDString = Guid.Empty.ToString();
            }

        }
#endif

    }
}
