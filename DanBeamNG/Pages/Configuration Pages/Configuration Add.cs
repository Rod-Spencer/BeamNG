using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using SpenSoft.DanBeamNG.Services;
using SpenSoft.BeamNG.VehicleObjects;

namespace SpenSoft.DanBeamNG.Pages.Configuration_Pages
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
        public String ID { get; set; }


        #endregion Parameters
        ////////////////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Injections

        [Inject]
        public ConfigurationDataService_Interface ConfigurationDataService { get; set; }

        [Inject]
        public VehicleDataService_Interface VehicleDataService { get; set; }

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

        public List<Classifications>? Classifications_List { get; set; }

        public List<Countries>? Countries_List { get; set; }

        public List<BodyStyles>? BodyStyles_List { get; set; }

        public List<DriveTrain>? DriveTrains_List { get; set; }

        public VehicleImage? vehicleImage { get; set; } = null;

        public Boolean isDisabled { get; set; } = true;

        public String ImageName { get; set; }

        #endregion Public Properties
        ////////////////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Screen State Properties

        protected String Message = String.Empty;
        protected String StatusClass = String.Empty;
        protected Boolean Saved;

        #endregion Screen State Properties
        ////////////////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Override Methods

        protected override async Task OnInitializedAsync()
        {
            Saved = false;
            int id = 0;
            int.TryParse(ID, out id);
            Vehicle = (await VehicleDataService.GetVehicleByID(id));
            Configuration = new VConfiguration() { VehicleID = id };
            BodyStyles_List = (await bodyStyleDataService.GetAllBodyStyle()).ToList();
            Classifications_List = (await classificationsDataService.GetAllClassifications()).ToList();
            Countries_List = (await countriesDataService.GetAllCountries()).ToList();
            DriveTrains_List = (await driveTrainDataService.GetAllDriveTrain()).ToList();
        }

        #endregion Override Methods
        ////////////////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Protected Methods

        protected void InvalidSubmitHandler()
        {
            StatusClass = "alert-danger";
            Message = "The Configuration data had validation errors.  Please correct and try again";
        }


        protected async Task ValidSumitHandler()
        {
            Saved = false;
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
                                VehicleImage? vi = await imageDataService?.GetVehicleImageByName(ImageName);
                                if (vi == null)
                                {
                                    vehicleImage = await imageDataService.AddVehicleImage(vehicleImage);
                                }
                            }
                            StatusClass = "alert-success";
                            Message = "New Configuration added";
                            Saved = true;
                        }
                        else
                        {
                            StatusClass = "alert-danger";
                            Message = "The new Configuration could not be dded";
                            Saved = false;
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
                        Message = "Configuration data updated";
                        Saved = true;
                    }
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
            if (vehicleImage == null) vehicleImage = new VehicleImage();
            //vehicleImage.ImageData = ms.ToArray();
            vehicleImage.ImageName = e.File.Name;
            vehicleImage.ImageID = Guid.NewGuid();
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
