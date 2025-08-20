using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using SpenSoft.DanBeamNG.Services;
using SpenSoft.BeamNG.VehicleObjects;

namespace SpenSoft.DanBeamNG.Pages.Vehicle_Pages
{
    public partial class Vehicle_Random : ComponentBase
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

        [Inject]
        public NavigationManager? NavigationManager { get; set; }

        //[Inject]
        //public NavigationManager? NavigationManager { get; set; }

        #endregion Injections
        ////////////////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Public Properties

        public VConfiguration Configuration { get; set; }

        public Vehicle Vehicle { get; set; }

        public int NextConfigID { get; set; }

        public VehicleImage? vehicleImage { get; set; } = null;

        public Boolean isDisabled { get; set; } = true;

        public String ImageName { get; set; }


        public List<Classifications>? Classifications_List { get; set; }

        public List<Countries>? Countries_List { get; set; }

        public List<BodyStyles>? BodyStyles_List { get; set; }

        public List<DriveTrain>? DriveTrains_List { get; set; }


        public Byte[]? ImageData { get; set; }


        #endregion Public Properties
        ////////////////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Screen State Properties
        #endregion Screen State Properties
        ////////////////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Override Methods

        protected override async Task OnParametersSetAsync()
        {
            await GetVehicleAndConfiguration();
            return;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender == false)
            {
                await GetVehicleAndConfiguration();
            }
        }

        protected override async Task OnInitializedAsync()
        {
            await GetVehicleAndConfiguration();
        }

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

        private async Task GetVehicleAndConfiguration()
        {
            if (ConfigurationDataService != null)
            {
                List<VConfiguration> configurations = (await ConfigurationDataService.GetAllConfiguration()).ToList();

                if ((configurations != null) && (configurations.Count > 0))
                {
                    Random random = new Random((int)(DateTime.Now.Ticks & int.MaxValue));
                    int configIndx = random.Next(configurations.Count);
                    Configuration = configurations[configIndx];

                    if (VehicleDataService != null)
                    {
                        Vehicle = await VehicleDataService.GetVehicleByID(Configuration.VehicleID);
                        NextConfigID = Configuration.VehicleID;
                    }

                    if (bodyStyleDataService != null) BodyStyles_List = (await bodyStyleDataService.GetAllBodyStyle()).ToList();
                    if (classificationsDataService != null) Classifications_List = (await classificationsDataService.GetAllClassifications()).ToList();
                    if (countriesDataService != null) Countries_List = (await countriesDataService.GetAllCountries()).ToList();
                    if (driveTrainDataService != null) DriveTrains_List = (await driveTrainDataService.GetAllDriveTrain()).ToList();

                }
            }
        }

        private async void LoadImage(InputFileChangeEventArgs e)
        {
            MemoryStream ms = new MemoryStream();
            await e.File.OpenReadStream().CopyToAsync(ms);
            if (vehicleImage == null) vehicleImage = new VehicleImage();
            //vehicleImage.ImageData = ms.ToArray();
            vehicleImage.ImageName = e.File.Name;
            //vehicleImage.ImageKey = Guid.NewGuid();
            ImageName = e.File.Name;
        }


        private async void NameChanged(String name)
        {
            if ((vehicleImage == null) || (vehicleImage.ImageID == Guid.Empty))
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


        private String ClassificationsName(Guid? id)
        {
            if ((id == null) || (id == Guid.Empty)) return "";
            Classifications? foundClass = Classifications_List?.FirstOrDefault(x => x.ID == id);
            return foundClass == null ? "" : foundClass.Name;
        }

        private String BodyStyleName(Guid? id)
        {
            if ((id == null) || (id == Guid.Empty)) return "";
            BodyStyles? foundBody = BodyStyles_List?.FirstOrDefault(x => x.ID == id);
            return foundBody == null ? "" : foundBody.Name;
        }

        private String CountriesName(Guid? id)
        {
            if ((id == null) || (id == Guid.Empty)) return "";
            Countries? foundCountry = Countries_List?.FirstOrDefault(x => x.ID == id);
            return foundCountry == null ? "" : foundCountry.Name;
        }

        private String DriveTrainName(Guid? id)
        {
            if ((id == null) || (id == Guid.Empty)) return "";
            DriveTrain? foundDriveTrain = DriveTrains_List?.FirstOrDefault(x => x.ID == id);
            return foundDriveTrain == null ? "" : foundDriveTrain.Name;
        }



        #endregion
        ////////////////////////////////////////////////////////////////////////////////////////////////////




    }
}
