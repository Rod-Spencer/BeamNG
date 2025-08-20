using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using SpenSoft.BeamNG.VehicleObjects;
using SpenSoft.DanBeamNG.Services;

namespace SpenSoft.DanBeamNG.Pages.ConfigurationPages
{
    public partial class Configuration_Edit : ComponentBase
    {

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Constructors
        #endregion
        ////////////////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Parameters

        [Parameter]
        public String? ID { get; set; }

        //[Parameter]
        public String? ImageID { get; set; }

        #endregion Parameters
        ////////////////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Injections

        [Inject]
        public ConfigurationDataService_Interface? ConfigurationDataService { get; set; }

        [Inject]
        public VehicleDataService_Interface? VehicleDataService { get; set; }

        [Inject]
        public VehicleImageDataService_Interface? imageDataService { get; set; }

        [Inject]
        public ClassificationsDataService_Interface? classificationsDataService { get; set; }

        [Inject]
        public BodyStyleDataService_Interface? bodyStyleDataService { get; set; }

        [Inject]
        public CountriesDataService_Interface? countriesDataService { get; set; }

        [Inject]
        public DriveTrainDataService_Interface? driveTrainDataService { get; set; }

        [Inject]
        public NavigationManager? NavigationManager { get; set; }

        [Inject]
        public ImagesDataDataService_Interface? imagesDataDataService { get; set; }

        #endregion Injections
        ////////////////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Public Properties

        public VConfiguration? Configuration { get; set; }

        public Vehicle? Vehicle { get; set; }

        public List<KeyValuePair<Guid, String>>? Classifications_List => Classifications?.Select(c => new KeyValuePair<Guid, String>(c.ID, c.Name)).ToList();

        public List<KeyValuePair<Guid, String>>? Countries_List => Countries?.Select(c => new KeyValuePair<Guid, String>(c.ID, c.Name)).ToList();

        public List<KeyValuePair<Guid, String>>? BodyStyles_List => BodyStyles?.Select(c => new KeyValuePair<Guid, String>(c.ID, c.Name)).ToList();

        public List<KeyValuePair<Guid, String>>? DriveTrains_List => DriveTrains?.Select(c => new KeyValuePair<Guid, String>(c.ID, c.Name)).ToList();

        public List<Classifications>? Classifications { get; set; }

        public List<Countries>? Countries { get; set; }

        public List<BodyStyles>? BodyStyles { get; set; }

        public List<DriveTrain>? DriveTrains { get; set; }

        public List<VehicleImage>? Image_List { get; set; } = null;

        public VehicleImage? vehicleImage { get; set; } = null;

        //public ImagesData? imagesData { get; set; } = null;

        public Boolean isDisabled { get; set; } = true;

        public String? ImageName { get; set; }

        #endregion Public Properties
        ////////////////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Screen State Properties

        protected String Message = String.Empty;
        protected String StatusClass = String.Empty;
        protected Boolean Saved;
        private System.Threading.Timer? timer = null;

        #endregion Screen State Properties
        ////////////////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Override Methods




#if true

        protected override async Task OnInitializedAsync()
        {
            Saved = false;
            //imagesData = null;

            if (String.IsNullOrEmpty(ID) == false)
            {
                int id = int.Parse(ID);
                if (ConfigurationDataService != null) Configuration = (await ConfigurationDataService.GetConfigurationByID(id));
                {
                    if (Configuration != null)
                    {
                        if (VehicleDataService != null) Vehicle = (await VehicleDataService.GetVehicleByID(Configuration.VehicleID));
                        if (Configuration.ImageID.HasValue == true)
                        {

                            if (imageDataService != null)
                            //if (Vehicle != null) Image_List = await imageDataService?.getGetAllImagesByVehicle(Vehicle.ID);
                            {
                                VehicleImage? vi = await imageDataService.GetVehicleImageById(Configuration.ImageID);
                                if ( vi != null)
                                {
                                    ImageName = vi.ImageName;
                                    ImageID = $"{vi.ImageID}";
                                }

                                //if ((Image_List != null) && (vehicleImage != null))
                                //{
                                //    vehicleImage = Image_List.FirstOrDefault(x => x.ImageID == Configuration.ImageID.Value);
                                //    if (vehicleImage != null)
                                //    {
                                //        ImageName = vehicleImage.ImageName;
                                //        ImageID = $"{vehicleImage.ImageID}";
                                //        //if (imagesDataDataService != null) imagesData = await imagesDataDataService.GetImagesDataById(vehicleImage.ImageID);
                                //    }
                                //}
                            }
                        }
                    }
                }
                if (bodyStyleDataService != null) BodyStyles = (await bodyStyleDataService.GetAllBodyStyle())?.ToList();
                if (classificationsDataService != null) Classifications = (await classificationsDataService.GetAllClassifications())?.ToList();
                if (countriesDataService != null) Countries = (await countriesDataService.GetAllCountries())?.ToList();
                if (driveTrainDataService != null) DriveTrains = (await driveTrainDataService.GetAllDriveTrain())?.ToList();
            }
        }
#endif


        #endregion Override Methods
        ////////////////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Protected Methods

        protected void InvalidSubmitHandler()
        {
            StatusClass = "alert-danger";
            Message = "The configuration data had validation errors.  Please correct and try again";
        }

        protected async Task ValidSumitHandler()
        {
            Saved = true;
            if (Configuration != null)
            {
                if (ConfigurationDataService != null)
                {
                    if (Configuration.ID == 0)
                    {
                        VConfiguration? addedConfiguration = await ConfigurationDataService.AddConfiguration(Configuration);
                        if (addedConfiguration != null)
                        {
                            if (imageDataService != null)
                            {
                                VehicleImage? vi = await imageDataService.GetVehicleImageById(addedConfiguration.ImageID);
                                if (vi == null)
                                {
                                    vehicleImage = await imageDataService.AddVehicleImage(vehicleImage);
                                }
                            }
                            StatusClass = "alert-success";
                            Message = "New configuration added";
                        }
                        else
                        {
                            StatusClass = "alert-danger";
                            Message = "The new configuration could not be dded";
                        }
                    }
                    else
                    {
                        await ConfigurationDataService.UpdateConfiguration(Configuration);

                        if ((imageDataService != null) && (Configuration.ImageID.HasValue == true))
                        {
                            VehicleImage? vi = await imageDataService.GetVehicleImageById(Configuration.ImageID);
                            if (vi == null)
                            {
                                vehicleImage = await imageDataService.AddVehicleImage(vehicleImage);
                                ImageID = $"{vehicleImage?.ImageID}";
                                ImageName = vehicleImage?.ImageName;
                            }
                            else
                            {
                                ImageID = $"{vi?.ImageID}";
                                ImageName = vi?.ImageName;
                            }
                        }

                        StatusClass = "alert-success";
                        Message = "configuration data updated";
                    }
                }
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

        #endregion
        ////////////////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Public Methods

        #endregion
        ////////////////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Private Methods

        private static List<T> GetCodes<T>()
        {
            List<T> codes = new List<T>();
            foreach (T t in Enum.GetValues(typeof(T))) { codes.Add(t); }
            return codes;
        }


        private async void LoadImage(InputFileChangeEventArgs e)
        {
            try
            {
                ImageName = e.File.Name;
                if (Configuration?.ImageID != null)
                {
                    if (imageDataService != null) vehicleImage = await imageDataService.GetVehicleImageById(Configuration?.ImageID);
                }
                if (vehicleImage != null)
                {
                    ImageName = vehicleImage.ImageName;
                    ImageID = vehicleImage.ImageID.ToString();
                    if (Configuration != null) Configuration.ImageID = vehicleImage.ImageID;
                    if (ConfigurationDataService != null)
                    {
                        await ConfigurationDataService.UpdateConfiguration(Configuration);
                        //NavigationManager?.NavigateTo($"EditConfiguration/{ID}/{Configuration.ImageID}");
                        Saved = true;
                        Message = $"Configuration: {Configuration?.Name} has been updated successfully";
                        StatusClass = "alert-success";
                    }

                    return;
                }

                ImagesData imagesData = new ImagesData() { ImageID = Guid.NewGuid() };

                MemoryStream ms = new MemoryStream();
                await e.File.OpenReadStream().CopyToAsync(ms);
                imagesData.ImageData = ms.ToArray();

                if (imagesDataDataService != null)
                {
                    var id = await imagesDataDataService.AddImagesData(imagesData);
                    if (id == null)
                    {
                        Saved = true;
                        Message = $"A new Image-Data record could not be created";
                        StatusClass = "alert-danger";
                        return;
                    }
                    imagesData = id;
                }

                vehicleImage = new VehicleImage() { ImageID = imagesData.ImageID, ImageName = this.ImageName };
                if (imageDataService != null)
                {
                    var vi = await imageDataService.AddVehicleImage(vehicleImage);
                    if (vi == null)
                    {
                        Saved = true;
                        Message = $"A new VehicleImage record could not be created";
                        StatusClass = "alert-danger";
                        return;
                    }
                    vehicleImage = vi;
                }


                if (Configuration != null) Configuration.ImageID = vehicleImage.ImageID;
                if (ConfigurationDataService != null)
                {
                    if (await ConfigurationDataService.UpdateConfiguration(Configuration) == true)
                    {
                        //NavigationManager?.NavigateTo($"EditConfiguration/{ID}/{Configuration.ImageID}");
                        Saved = true;
                        Message = $"Configuration: {Configuration?.Name} has been updated successfully";
                        StatusClass = "alert-success";
                    }
                    else
                    {
                        Saved = true;
                        Message = $"Configuration: {Configuration?.Name} could not be updated";
                        StatusClass = "alert-danger";
                    }
                }
            }
            finally
            {
                StateHasChanged();
                timer = new Timer((object? stateInfo) =>
                {
                    timer?.Dispose();
                    timer = null;

                    Saved = false;
                    StateHasChanged();
                }, new AutoResetEvent(false), 5000, 10000);

            }
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
                    ImageID = vehicleImage.ImageID.ToString();
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
                    ImageID = String.Empty;
                }
            }
            else
            {
                ImageName = null;
                ImageID = String.Empty;
            }
        }

        private async void Image_Selected(IBrowserFile f)
        {
            if (imagesDataDataService == null) return;
            if (imageDataService == null) return;

            ImageName = f.Name;
            Guid ImageIDGuid = Guid.NewGuid();
            ImageID = $"{ImageIDGuid}";
            List<VehicleImage>? images = null;
            if (Vehicle != null) images = await imageDataService.GetAllImagesByVehicle(Vehicle.ID);
            if (images == null) return;
            var vi = images?.FirstOrDefault(x => x.ImageName == ImageName);
            if (vi == null)
            {
                vi = new VehicleImage() { ImageEntered = DateTime.Now, ImageName = ImageName, ImageID = ImageIDGuid };
                vi = await imageDataService.AddVehicleImage(vi);
                if ((vi != null) && (vi.ImageID != Guid.Empty))
                {
                    var imagesData = await imagesDataDataService.GetImagesDataByID(vi.ImageID);
                    if (imagesData != null) return;
                    ImagesData id = new ImagesData() { ImageID = ImageIDGuid };

                    MemoryStream ms = new MemoryStream();
                    await f.OpenReadStream().CopyToAsync(ms);
                    id.ImageData = ms.ToArray();
                    imagesDataDataService?.AddImagesData(id);
                }
            }
        }

#if true
        private async void ImageName_Changed(String name)
        {
            if (vehicleImage == null)
            {
                isDisabled = true;
                return;
            }
            if (imageDataService != null)
            {
                VehicleImage vi = await imageDataService.GetVehicleImageById(vehicleImage.ImageID);
                if (vi != null)
                {
                    vi.ImageName = name;
                    vehicleImage.ImageName = name;
                    await imageDataService.UpdateVehicleImage(vehicleImage);
                }
                ImageName = name;
            }

            if (String.IsNullOrEmpty(ImageName) == true) isDisabled = true;
            else if (vehicleImage == null) isDisabled = true;
            else isDisabled = false;
        }
#else

        private void ImageName_Changed(String? imageName)
        {
            ImageName = imageName;
            if (imageDataService != null)
            {
                VehicleImage? vi = imageDataService.GetVehicleImageById(Configuration?.ImageID).Result;
                if (vi != null)
                {
                    vi.ImageName = imageName;
                    imageDataService.UpdateVehicleImage(vi);
                }
            }
        }
#endif

        #endregion
        ////////////////////////////////////////////////////////////////////////////////////////////////////

    }
}
