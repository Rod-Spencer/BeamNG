using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using SpenSoft.DanBeamNG.Services;
using SpenSoft.BeamNG.VehicleObjects;

namespace SpenSoft.DanBeamNG.Pages.ConfigurationPages
{
    public partial class Configuration_Add : ComponentBase
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

        #endregion Injections
        ////////////////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Public Properties

        public VConfiguration Configuration { get; set; } = new VConfiguration();

        public Vehicle? Vehicle { get; set; }

        public List<KeyValuePair<Guid, String>>? Classifications_List => Classifications?.Select(c => new KeyValuePair<Guid, String>(c.ID, c.Name)).ToList();

        public List<KeyValuePair<Guid, String>>? Countries_List => Countries?.Select(c => new KeyValuePair<Guid, String>(c.ID, c.Name)).ToList();

        public List<KeyValuePair<Guid, String>>? BodyStyles_List => BodyStyles?.Select(c => new KeyValuePair<Guid, String>(c.ID, c.Name)).ToList();

        public List<KeyValuePair<Guid, String>>? DriveTrains_List => DriveTrains?.Select(c => new KeyValuePair<Guid, String>(c.ID, c.Name)).ToList();

        public List<Classifications>? Classifications { get; set; }

        public List<Countries>? Countries { get; set; }

        public List<BodyStyles>? BodyStyles { get; set; }

        public List<DriveTrain>? DriveTrains { get; set; }


        public VehicleImage? vehicleImage { get; set; } = null;

        public ImagesData? imagesData { get; set; } = null;

        public Boolean isDisabled { get; set; } = true;

        public String? ImageName { get; set; }

        public List<VehicleImage>? Image_List { get; set; }

        #endregion Public Properties
        ////////////////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Screen State Properties

        protected String Message = String.Empty;
        protected String StatusClass = String.Empty;
        protected Boolean Saved = false;
        private System.Threading.Timer? timer = null;


        #endregion Screen State Properties
        ////////////////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Override Methods

        protected override async Task OnInitializedAsync()
        {
            Saved = false;
            int id = 0;
            int.TryParse(ID, out id);
            if (VehicleDataService != null) Vehicle = (await VehicleDataService.GetVehicleByID(id));
            Configuration = new VConfiguration() { VehicleID = id };
            if (bodyStyleDataService != null) BodyStyles = (await bodyStyleDataService.GetAllBodyStyle())?.ToList();
            if (classificationsDataService != null) Classifications = (await classificationsDataService.GetAllClassifications())?.ToList();
            if (countriesDataService != null) Countries = (await countriesDataService.GetAllCountries())?.ToList();
            if (driveTrainDataService != null) DriveTrains = (await driveTrainDataService.GetAllDriveTrain())?.ToList();
            if (imageDataService != null) Image_List = (await imageDataService.GetUnassignedImages()).OrderBy(x => x.ImageName).ToList();
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
            if (Configuration != null)
            {
                if (ConfigurationDataService != null)
                {
                    if (Configuration.ID == 0)
                    {
                        VConfiguration? addedConfiguration = await ConfigurationDataService.AddConfiguration(Configuration);
                        if (addedConfiguration != null)
                        {
                            if ((addedConfiguration.ImageID != null) && (addedConfiguration.ImageID != Guid.Empty))
                            {
                                if (imageDataService != null)
                                {
                                    VehicleImage? vi = await imageDataService.GetVehicleImageById(addedConfiguration.ImageID);
                                    if (vi == null)
                                    {
                                        vehicleImage = await imageDataService.AddVehicleImage(vehicleImage);
                                    }
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

                        if (imageDataService != null)
                        {
                            if (Configuration.ImageID.HasValue == true)
                            {
                                VehicleImage? vi = await imageDataService.GetVehicleImageById(Configuration.ImageID.Value);
                                if (vi == null)
                                {
                                    vehicleImage = await imageDataService.AddVehicleImage(vehicleImage);
                                }
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
            MemoryStream ms = new MemoryStream();
            await e.File.OpenReadStream().CopyToAsync(ms);
            if (imagesData == null) imagesData = new ImagesData { ImageID = Guid.NewGuid(), ImageData = ms.ToArray() };
            if (vehicleImage == null) vehicleImage = new VehicleImage();
            //vehicleImage.ImageData = ms.ToArray();
            vehicleImage.ImageName = e.File.Name;
            vehicleImage.ImageID = imagesData.ImageID;
            ImageName = e.File.Name;
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

        #endregion
        ////////////////////////////////////////////////////////////////////////////////////////////////////


    }
}
