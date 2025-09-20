using Microsoft.AspNetCore.Components;
using SpenSoft.DanBeamNG.Services;
using SpenSoft.BeamNG.VehicleObjects;
using System.Reflection;

namespace SpenSoft.DanBeamNG.Pages.VehiclePages
{
    public partial class Vehicle_Random_Class : ComponentBase
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

        //[Inject]
        //public VehicleImageDataService_Interface? vehicleImageDataService { get; set; }

        [Inject]
        public ClassificationsDataService_Interface? classificationsDataService { get; set; }

        [Inject]
        public BodyStyleDataService_Interface? bodyStyleDataService { get; set; }

        [Inject]
        public CountriesDataService_Interface? countriesDataService { get; set; }

        [Inject]
        public DriveTrainDataService_Interface? driveTrainDataService { get; set; }

        //[Inject]
        //public NavigationManager? NavigationManager { get; set; }

        //[Inject]
        //public ImagesDataDataService_Interface? imagesDataService { get; set; }

        #endregion Injections
        ////////////////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Public Properties


        public Vehicle? Vehicle { get; set; }

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

        public List<BodyStyles>? BodyStyles_List { get; set; }

        public List<Classifications>? Classifications_List { get; set; }

        public List<Countries>? Country_List { get; set; }

        public List<DriveTrain>? DriveTrain_List { get; set; }

        public Configuration_Info? CI { get; set; } = null;

        public int ConfigurationCount { get; set; } = 0;

        public String? BodyStyleName { get; set; }
        public String? ClassificationName { get; set; }
        public String? CountryName { get; set; }
        public String? DriveTrainName { get; set; }
        public String? VehicleName { get; set; }
        public String? VehicleImageID { get; set; }
        public String? ConfigurationName { get; set; }
        public Guid? ConfigurationImageID { get; set; }

        public List<KeyValuePair<Guid, String?>> Classifications
        {
            get
            {
                var list = new List<KeyValuePair<Guid, String?>>();
                list.Add(new KeyValuePair<Guid, String?>(Guid.Empty, null));
                if (Classifications_List != null)
                {
                    foreach (var cls in Classifications_List)
                    {
                        list.Add(new KeyValuePair<Guid, String?>(cls.ID, cls.Name));

                    }
                }
                return list;
            }
        }


        #endregion Public Properties
        ////////////////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Screen State Properties
        #endregion Screen State Properties
        ////////////////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Override Methods


        protected override async Task OnInitializedAsync()
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

        protected async Task OnClassSelected()
        {
            var configurations = AllConfigurations.Where(x => x.ClassificationID == SelectedClass).ToList();
            if ((configurations != null) && (configurations.Count > 0))
            {
                String? className = Classifications_List?.FirstOrDefault(x => x.ID == SelectedClass)?.Name;

                Random random = new Random((int)(DateTime.Now.Ticks & int.MaxValue));
                int configIndx = random.Next(configurations.Count);
                var configuration = configurations[configIndx];

                if (VehicleDataService != null)
                {
                    Vehicle = await VehicleDataService.GetVehicleByID(configuration.VehicleID);
                }

                //CI = new Configuration_Info()
                //{
                BodyStyleName = BodyStyles_List?.FirstOrDefault(x => x.ID == configuration.BodyStyleID)?.Name;
                ClassificationName = Classifications_List?.FirstOrDefault(x => x.ID == configuration.ClassificationID)?.Name;
                CountryName = Country_List?.FirstOrDefault(x => x.ID == configuration.CountryID)?.Name;
                DriveTrainName = DriveTrain_List?.FirstOrDefault(x => x.ID == configuration.DriveTrainID)?.Name;
                VehicleName = Vehicle?.Name;
                //VehicleImageID = vehicle?.ImageID;
                ConfigurationName = configuration?.Name;
                ConfigurationImageID = configuration?.ImageID;
                //};
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
                Console.WriteLine($"{MethodBase.GetCurrentMethod()?.Name} - SelectedClass: {SelectedClass}");
                if ((SelectedClass.HasValue == true) && (SelectedClass != Guid.Empty))
                {
                    await OnClassSelected();
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
                SelectedClass = v;
                await OnClassSelected();
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