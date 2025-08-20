using Microsoft.AspNetCore.Components;
using SpenSoft.BeamNG.VehicleObjects;
using SpenSoft.DanBeamNG.Services;

namespace SpenSoft.DanBeamNG.Pages.Vehicle_Pages
{
    public partial class Vehicle_Random_Class : ComponentBase
    {

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Constructors
        #endregion
        ////////////////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Parameters

        [Parameter]
        public String? ClassName { get; set; } = null;

        #endregion Parameters
        ////////////////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Injections

        [Inject]
        public ConfigurationDataService_Interface? ConfigurationDataService { get; set; }

        [Inject]
        public VehicleDataService_Interface? VehicleDataService { get; set; }

        [Inject]
        public VehicleImageDataService_Interface? vehicleImageDataService { get; set; }

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

        #endregion Injections
        ////////////////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Public Properties

        public VConfiguration? Configuration { get; set; }


        public Vehicle? Vehicle { get; set; }

        public int NextConfigID { get; set; }

        private Guid? _SelectedClass;

        public Guid? SelectedClass
        {
            get { return _SelectedClass; }
            set
            {
                _SelectedClass = value;
                OnClassSelected();
            }
        }

        public List<VConfiguration>? AllConfigurations { get; set; } = null;

        public List<Classifications>? Classifications_List { get; set; }

        public List<Countries>? Countries_List { get; set; }

        public List<BodyStyles>? BodyStyles_List { get; set; }

        public List<DriveTrain>? DriveTrains_List { get; set; }

        public VehicleImage? vehicleImage { get; set; }
        //public Byte[]? ImageData { get; set; } = null;

        //public String? ImageName { get; set; } = null;


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
            if (String.IsNullOrEmpty(ClassName) == false)
            {
                SelectedClass = Classifications_List?.FirstOrDefault(x => x.Name == ClassName)?.ID;
                if (SelectedClass != null)
                {
                    var selected = AllConfigurations?.Where(x => x.ClassificationID == SelectedClass).ToList();
                    OnClassSelected(selected);
                }
            }
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
            if ((bodyStyleDataService != null) && (BodyStyles_List == null))
            {
                BodyStyles_List = (await bodyStyleDataService.GetAllBodyStyle()).OrderBy(x => x.Name).ToList();
            }
            if ((classificationsDataService != null) && (Classifications_List == null))
            {
                Classifications_List = (await classificationsDataService.GetAllClassifications()).OrderBy(x => x.Name).ToList();
            }
            if ((countriesDataService != null) && (Countries_List == null))
            {
                Countries_List = (await countriesDataService.GetAllCountries()).OrderBy(x => x.Name).ToList();
            }
            if ((driveTrainDataService != null) && (DriveTrains_List == null))
            {
                DriveTrains_List = (await driveTrainDataService.GetAllDriveTrain()).OrderBy(x => x.Name).ToList();
            }

            if ((ConfigurationDataService != null) && (AllConfigurations == null))
            {
                List<VConfiguration> AllConfigurations = (await ConfigurationDataService.GetAllConfiguration()).ToList();
            }

            if (String.IsNullOrEmpty(ClassName) == false)
            {
                SelectedClass = Classifications_List.FirstOrDefault(x => x.Name == ClassName)?.ID;
                if (SelectedClass != null)
                {
                    var selected = AllConfigurations.Where(x => x.ClassificationID == SelectedClass).ToList();
                    OnClassSelected(selected);
                }
            }
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

        private async Task<List<VConfiguration>> GetVehicleAndConfiguration()
        {
            if ((bodyStyleDataService != null) && (BodyStyles_List == null))
            {
                BodyStyles_List = (await bodyStyleDataService.GetAllBodyStyle()).OrderBy(x => x.Name).ToList();
            }
            if ((classificationsDataService != null) && (Classifications_List == null))
            {
                Classifications_List = (await classificationsDataService.GetAllClassifications()).OrderBy(x => x.Name).ToList();
            }
            if ((countriesDataService != null) && (Countries_List == null))
            {
                Countries_List = (await countriesDataService.GetAllCountries()).OrderBy(x => x.Name).ToList();
            }
            if ((driveTrainDataService != null) && (DriveTrains_List == null))
            {
                DriveTrains_List = (await driveTrainDataService.GetAllDriveTrain()).OrderBy(x => x.Name).ToList();
            }

            if (ConfigurationDataService == null) return null;

            List<VConfiguration> configurations = (await ConfigurationDataService.GetAllConfiguration()).ToList();

            if (String.IsNullOrEmpty(ClassName) == false)
            {
                SelectedClass = Classifications_List.FirstOrDefault(x => x.Name == ClassName)?.ID;
                OnClassSelected(configurations);
            }
            return configurations;
        }

        protected async void OnClassSelected(List<VConfiguration>? configurations = null)
        {
            if (configurations == null) configurations = (await ConfigurationDataService.GetAllConfiguration()).ToList();
            configurations = configurations
                   .Where(x => x.ClassificationID == SelectedClass)
                   .ToList();

            ClassName = Classifications_List?.FirstOrDefault(x => x.ID == SelectedClass)?.Name;

            if ((configurations != null) && (configurations.Count > 0))
            {
                Random random = new Random((int)(DateTime.Now.Ticks & int.MaxValue));
                int configIndx = random.Next(configurations.Count);
                Configuration = configurations[configIndx];
                NextConfigID = Configuration.ID;

                if (VehicleDataService != null)
                {
                    Vehicle = await VehicleDataService.GetVehicleByID(Configuration.VehicleID);
                }

                if ((vehicleImageDataService != null) && (Configuration.ImageID.HasValue == true))
                {
                    vehicleImage = await vehicleImageDataService.GetVehicleImageById(Configuration.ImageID.Value);
                    //if (vi != null)
                    //{
                    //    //ImageData = vi.ImageData;
                    //    ImageName = vi.ImageName;
                    //}
                }
            }

            if (NavigationManager != null) NavigationManager.NavigateTo($"/RandomVehicle/Class/{ClassName}");
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

        public String GetClsName(Guid? id)
        {
            if ((id.HasValue == false) || (id == Guid.Empty)) return String.Empty;
            Classifications? bs = Classifications_List?.FirstOrDefault(x => x.ID == id);
            if (bs == null) return String.Empty;
            return bs.Name;
        }

        private void NavigateToClassName()
        {
            if (String.IsNullOrEmpty(ClassName) == true) return;
            if (NavigationManager != null)
            {
                NavigationManager.NavigateTo($"/RandomVehicle/Class/{ClassName}");
            }
        }

        #endregion
        ////////////////////////////////////////////////////////////////////////////////////////////////////


    }
}