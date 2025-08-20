using Microsoft.AspNetCore.Components;
using SpenSoft.BeamNG.VehicleObjects;
using SpenSoft.DanBeamNG.Services;
using System.Reflection;

namespace SpenSoft.DanBeamNG.Pages.VehiclePages
{
    public partial class Vehicle_Random_Country : ComponentBase
    {
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Constructors
        #endregion
        ////////////////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Parameters
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

        public VConfiguration? configuration { get; set; } = null;

        public Vehicle? vehicle { get; set; } = null;

        public Configuration_Info? CI { get; set; } = null;

        public List<VConfiguration>? AllConfigurations { get; set; } = null;

        public List<BodyStyles>? BodyStyles_List { get; set; }

        public List<Classifications>? Classifications_List { get; set; }

        public List<Countries>? Country_List { get; set; }

        public List<DriveTrain>? DriveTrain_List { get; set; }


        private Guid? _SelectedCountry = null;
        public Guid? SelectedCountry
        {
            get { return _SelectedCountry; }
            set
            {
                _SelectedCountry = value;
                OnCountrySelected();
            }
        }

        public int ConfigurationCount { get; set; } = 0;


        public String? BodyStyleName { get; set; }
        public String? ClassificationName { get; set; }
        public String? CountryName { get; set; }
        public String? DriveTrainName { get; set; }
        public String? VehicleName { get; set; }
        public String? VehicleImageID { get; set; }
        public String? ConfigurationName { get; set; }
        public Guid? ConfigurationImageID { get; set; }

        #endregion Public Properties
        ////////////////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Private Properties
        #endregion Private Properties
        ////////////////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Screen State Properties
        #endregion Screen State Properties
        ////////////////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Override Methods

        protected override async Task OnInitializedAsync()
        {
            Console.WriteLine($"Entered {MethodBase.GetCurrentMethod()?.Name}");

            if (ConfigurationDataService != null)
            {
                AllConfigurations = (await ConfigurationDataService.GetAllConfiguration()).ToList();
            }

            if (classificationsDataService != null)
            {
                Classifications_List = (await classificationsDataService.GetAllClassifications()).OrderBy(x => x.Name).ToList();
            }

            if (bodyStyleDataService != null)
            {
                BodyStyles_List = (await bodyStyleDataService.GetAllBodyStyle()).OrderBy(x => x.Name).ToList();
            }

            if (countriesDataService != null)
            {
                Country_List = (await countriesDataService.GetAllCountries()).OrderBy(x => x.Name).ToList();
            }

            if (driveTrainDataService != null)
            {
                DriveTrain_List = (await driveTrainDataService.GetAllDriveTrain()).OrderBy(x => x.Name).ToList();
            }

            Console.WriteLine($"Leaving {MethodBase.GetCurrentMethod()?.Name}");
        }

        protected override async Task OnParametersSetAsync()
        {
            if (ConfigurationDataService != null)
            {
                AllConfigurations = (await ConfigurationDataService.GetAllConfiguration()).ToList();
            }

            if (classificationsDataService != null)
            {
                Classifications_List = (await classificationsDataService.GetAllClassifications()).OrderBy(x => x.Name).ToList();
            }

            if (bodyStyleDataService != null)
            {
                BodyStyles_List = (await bodyStyleDataService.GetAllBodyStyle()).OrderBy(x => x.Name).ToList();
            }

            if (countriesDataService != null)
            {
                Country_List = (await countriesDataService.GetAllCountries()).OrderBy(x => x.Name).ToList();
            }

            if (driveTrainDataService != null)
            {
                DriveTrain_List = (await driveTrainDataService.GetAllDriveTrain()).OrderBy(x => x.Name).ToList();
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

        protected async Task OnCountrySelected()
        {
            var configurations = AllConfigurations
            .Where(x => x.CountryID == SelectedCountry)
            .ToList();
            if ((configurations != null) && (configurations.Count > 0))
            {
                Random random = new Random((int)(DateTime.Now.Ticks & int.MaxValue));
                int configIndx = random.Next(configurations.Count);
                var configuration = configurations[configIndx];

                Vehicle vehicle = null;
                if (VehicleDataService != null)
                {
                    vehicle = await VehicleDataService.GetVehicleByID(configuration.VehicleID);
                }

                BodyStyleName = BodyStyles_List?.FirstOrDefault(x => x.ID == configuration.BodyStyleID)?.Name;
                ClassificationName = Classifications_List?.FirstOrDefault(x => x.ID == configuration.ClassificationID)?.Name;
                CountryName = Country_List?.FirstOrDefault(x => x.ID == configuration.CountryID)?.Name;
                DriveTrainName = DriveTrain_List?.FirstOrDefault(x => x.ID == configuration.DriveTrainID)?.Name;
                VehicleName = vehicle?.Name;
                ConfigurationName = configuration?.Name;
                ConfigurationImageID = configuration?.ImageID;
            }
        }


        private void Reset()
        {
            ConfigurationCount = 0;
            CI = null;
        }

        private async Task GetNext()
        {
            Console.WriteLine($"Entered {MethodBase.GetCurrentMethod()?.Name}");
            try
            {
                Console.WriteLine($"{MethodBase.GetCurrentMethod()?.Name} - SelectedCountry: {SelectedCountry}");
                if ((SelectedCountry.HasValue == true) && (SelectedCountry != Guid.Empty))
                {
                    await OnCountrySelected();
                }
            }
            finally
            {
                Console.WriteLine($"Leaving {MethodBase.GetCurrentMethod()?.Name}");
            }
        }

        private async Task WhenValueChanges(Guid? v)
        {
            Console.WriteLine($"Entered {MethodBase.GetCurrentMethod()?.Name}");
            try
            {
                SelectedCountry = v;
                await OnCountrySelected();
            }
            finally
            {
                Console.WriteLine($"Leaving {MethodBase.GetCurrentMethod()?.Name}");
            }
        }

        #endregion
        ////////////////////////////////////////////////////////////////////////////////////////////////////


    }
}