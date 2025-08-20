using Microsoft.AspNetCore.Components;
using SpenSoft.DanBeamNG.Services;
using SpenSoft.BeamNG.VehicleObjects;

namespace SpenSoft.DanBeamNG.Pages.VehiclePages
{
    public partial class Vehicle_Random : ComponentBase
    {
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

        public VConfiguration? configuration { get; set; } = null;

        public Vehicle? vehicle { get; set; } = null;

        public String? vehicleName { get; set; } = null;

        public String? configurationName { get; set; } = null;

        public VehicleImage? vehicleImage { get; set; } = null;

        public String? classification { get; set; } = null;

        public String? country { get; set; } = null;

        public String? bodyStyle { get; set; } = null;

        public String? driveTrain { get; set; } = null;


        #endregion Public Properties
        ////////////////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Screen State Properties
        #endregion Screen State Properties
        ////////////////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Override Methods

        //protected override async Task OnParametersSetAsync()
        //{
        //    await OnBodySelected();
        //    return;
        //}

        //protected override async Task OnAfterRenderAsync(bool firstRender)
        //{
        //    if (firstRender == false)
        //    {
        //        await OnBodySelected();
        //    }
        //}

        protected override async Task OnInitializedAsync()
        {
            if (VehicleDataService != null)
            {
                List<Vehicle> allVehicles = (await VehicleDataService.GetAllVehicle()).ToList();
                Random random = new Random((int)(DateTime.Now.Ticks & int.MaxValue));
                vehicle = allVehicles[random.Next(allVehicles.Count)];
                if (vehicle != null)
                {
                    vehicleName = vehicle.Name;
                    configuration = vehicle.Configurations[random.Next(vehicle.Configurations.Count)];
                    if (configuration != null)
                    {
                        configurationName = configuration.Name;

                        if (imageDataService != null)
                        {
                            vehicleImage = await imageDataService.GetVehicleImageById(configuration.ImageID);
                        }

                        if (bodyStyleDataService != null)
                        {
                            var b = await bodyStyleDataService.GetBodyStyleById(configuration.BodyStyleID);
                            if (b != null) bodyStyle = b.Name;
                        }

                        if (classificationsDataService != null)
                        {
                            var c = await classificationsDataService.GetClassificationsById(configuration.ClassificationID);
                            if (c != null) classification = c.Name;
                        }

                        if (countriesDataService != null)
                        {
                            var c = await countriesDataService.GetCountriesByID(configuration.CountryID);
                            if (c != null) country = c.Name;
                        }

                        if (driveTrainDataService != null)
                        {
                            var d = await driveTrainDataService.GetDriveTrainByID(configuration.DriveTrainID);
                            if (d != null) driveTrain = d.Name;
                        }
                    }
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

        public async void GetNext()
        {
            await OnInitializedAsync();
        }

        #endregion
        ////////////////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Private Methods

        #endregion
        ////////////////////////////////////////////////////////////////////////////////////////////////////

    }
}
