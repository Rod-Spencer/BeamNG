using Microsoft.AspNetCore.Components;
using NLog;
using Segway.Service.Helper.Except;
using Segway.Service.LoggerHelper;
using SpenSoft.BeamNG.VehicleObjects;
using SpenSoft.DanBeamNG.Services;

namespace SpenSoft.DanBeamNG.Pages.VehiclePages
{
    public partial class Vehicle_Edit : ComponentBase
    {
        private static Logger logger = Logger_Helper.GetCurrentLogger();
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Constructors
        #endregion
        ////////////////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Parameters

        [Parameter]
        public Int32? ID { get; set; }

        #endregion Parameters
        ////////////////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Injections

        [Inject]
        public VehicleDataService_Interface? VehicleDataService { get; set; }

        [Inject]
        public ClassificationsDataService_Interface? classificationsDataService { get; set; }

        [Inject]
        public BodyStyleDataService_Interface? bodyStyleDataService { get; set; }

        [Inject]
        public CountriesDataService_Interface? countriesDataService { get; set; }

        [Inject]
        public DriveTrainDataService_Interface? driveTrainDataService { get; set; }

        [Inject]
        public VehicleImageDataService_Interface? vehicleImageDataService { get; set; }

        [Inject]
        public ConfigurationDataService_Interface? configurationDataService { get; set; }

        #endregion Injections
        ////////////////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Public Properties

        private Vehicle? _Vehicle = null;
        public Vehicle? Vehicle
        {
            get
            {
                if (_Vehicle == null)
                {
                    _Vehicle = new Vehicle() { ID = 0, Configurations = new List<VConfiguration>() };
                }
                return _Vehicle;
            }
            set { _Vehicle = value; }
        }

        public List<VConfiguration?>? Configurations { get; set; } = new List<VConfiguration?>();

        public Guid? ImageID
        {
            get => Vehicle?.ImageID;
            set { if (Vehicle != null) Vehicle.ImageID = value; }
        }

        public List<Classifications>? Classifications_List { get; set; }

        public List<Countries>? Countries_List { get; set; }

        public List<BodyStyles>? BodyStyles_List { get; set; }

        public List<DriveTrain>? DriveTrains_List { get; set; }

        public List<VehicleImage>? Images_List { get; set; }

        public List<VehicleImage>? Unassigned_List { get; set; }

        public Boolean ShowPopup { get; set; }


        public String? _Filter = null;
        public String? Filter
        {
            get { return _Filter; }
            set
            {
                _Filter = value;
                ApplyConfigurationFilter(_Filter);
            }
        }

        #endregion Public Properties
        ////////////////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Private Properties

        private VConfiguration? _selectedConfiguration = null;

        //private System.Threading.Timer? timer = null;

        #endregion Private Properties
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
            try
            {
                logger.Info($"Entered - OnInitializedAsync", nameof(OnInitializedAsync));
                Saved = false;
                if ((ID != null) && (VehicleDataService != null))
                {
                    logger.Debug($"Retrieving vehicle ID: {ID}", nameof(OnInitializedAsync));
                    Vehicle = await VehicleDataService.GetVehicleByID(ID);
                    ImageID = Vehicle?.ImageID;
                    logger.Debug($"vehicle found: {Vehicle != null}", nameof(OnInitializedAsync));
                }

                logger.Debug($"Started loading configuration data", nameof(OnInitializedAsync));
                if (bodyStyleDataService != null) BodyStyles_List = (await bodyStyleDataService.GetAllBodyStyle())?.ToList();
                if (classificationsDataService != null) Classifications_List = (await classificationsDataService.GetAllClassifications())?.ToList();
                if (countriesDataService != null) Countries_List = (await countriesDataService.GetAllCountries())?.ToList();
                if (driveTrainDataService != null) DriveTrains_List = (await driveTrainDataService.GetAllDriveTrain())?.ToList();
                logger.Debug($"Finished loading configuration data", nameof(OnInitializedAsync));


                logger.Debug($"Images_List is null", nameof(OnInitializedAsync));
                if (vehicleImageDataService != null)
                {
                    logger.Debug($"Retrieving all images for vehicle: {Vehicle?.Name}", nameof(OnInitializedAsync));
                    if (Vehicle != null)
                    {
                        if (vehicleImageDataService != null)
                        {
                            Images_List = (await vehicleImageDataService.GetAllImagesByVehicle(Vehicle.ID))?.ToList();
                            if (Images_List != null) Images_List = Images_List.OrderBy(x => x?.ImageName).ToList();
                        }
                    }
                    logger.Debug($"Retrieved: {Images_List?.Count} images", nameof(OnInitializedAsync));
                }

                if (vehicleImageDataService != null)
                {
                    logger.Debug($"Retrieving all unassigned images", nameof(OnInitializedAsync));
                    Unassigned_List = (await vehicleImageDataService.GetUnassignedImages())?.OrderBy(x => x?.ImageName)?.ToList();
                    logger.Debug($"Retrieved: {Unassigned_List?.Count} images", nameof(OnInitializedAsync));
                }

                ApplyConfigurationFilter(Filter);

            }
            catch (Exception ex)
            {
                logger.Error(Exception_Helper.FormatExceptionString(ex), nameof(OnInitializedAsync));
            }
            finally
            {
                logger.Info($"Leaving - OnInitializedAsync", nameof(OnInitializedAsync));
            }
        }

        #endregion Override Methods
        ////////////////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Protected Methods

        protected void InvalidSubmitHandler()
        {
            try
            {
                StatusClass = "alert-danger";
                Message = "The vehicle data had validation errors.  Please correct and try again";
            }
            catch
            {
            }
        }

        protected async Task ValidSumitHandler()
        {
            if (Vehicle == null) return;
            //if (Vehicle.ID == 0)
            //{
            //    if (VehicleDataService != null)
            //    {
            //        Vehicle? addedVehicle = await VehicleDataService.AddVehicle(Vehicle);

            //        if (addedVehicle != null)
            //        {
            //            StatusClass = "alert-success";
            //            Message = "New vehicle added";
            //        }
            //        else
            //        {
            //            StatusClass = "alert-danger";
            //            Message = "The vehicle could not be added";
            //        }
            //    }
            //}
            //else if (VehicleDataService != null)
            if (VehicleDataService != null)
            {
                var v = await VehicleDataService.UpdateVehicle(Vehicle);
                if (v == true)
                {
                    StatusClass = "alert-success";
                    Message = "vehicle data updated";
                }
                else
                {
                    StatusClass = "alert-danger";
                    Message = "vehicle could not be updated";
                }
            }

            Saved = true;
            StateHasChanged();
            //timer = new System.Threading.Timer((object? stateInfo) =>
            //{
            //    timer?.Dispose();
            //    timer = null;

            //    Saved = false;
            //    StateHasChanged();
            //}, new System.Threading.AutoResetEvent(false), 5000, 10000);
        }

        #endregion
        ////////////////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Public Methods

        public void ShowImagePopup(VConfiguration selectedConfiguration)
        {
            _selectedConfiguration = selectedConfiguration;
            ShowPopup = _selectedConfiguration != null;
        }

        public void AssignVehicleImage(Guid? image)
        {
            ImageID = image;
            Vehicle = CreateNewVehicleFromOld();
        }

        public async Task ConfigurationImageAssigned(VConfiguration config)
        {
            ShowPopup = false;
            if (configurationDataService != null) await configurationDataService.UpdateConfiguration(config);
            if (vehicleImageDataService != null) Unassigned_List = (await vehicleImageDataService.GetUnassignedImages()).OrderBy(x => x?.ImageName).ToList();
            Vehicle = CreateNewVehicleFromOld();
            StateHasChanged();
        }

        public void RemoveImage()
        {
            ImageID = null;
            StateHasChanged();
        }

        #endregion
        ////////////////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Private Methods

        private String? ClassificationsName(Guid? id)
        {
            try
            {
                if ((id == null) || (id == Guid.Empty)) return "";
                Classifications? foundClass = Classifications_List?.FirstOrDefault(x => x.ID == id);
                return foundClass == null ? "" : foundClass.Name;
            }
            catch //(Exception ex)
            {
                return null;
            }
        }

        private String? BodyStyleName(Guid? id)
        {
            try
            {
                if ((id == null) || (id == Guid.Empty)) return "";
                BodyStyles? foundBody = BodyStyles_List?.FirstOrDefault(x => x.ID == id);
                return foundBody == null ? "" : foundBody.Name;
            }
            catch //(Exception ex)
            {
                return null;
            }
        }

        private String? CountriesName(Guid? id)
        {
            try
            {
                if ((id == null) || (id == Guid.Empty)) return "";
                Countries? foundCountry = Countries_List?.FirstOrDefault(x => x.ID == id);
                return foundCountry == null ? "" : foundCountry.Name;
            }
            catch //(Exception ex)
            {
                return null;
            }
        }

        private String? DriveTrainName(Guid? id)
        {
            try
            {
                if ((id == null) || (id == Guid.Empty)) return "";
                DriveTrain? foundDriveTrain = DriveTrains_List?.FirstOrDefault(x => x.ID == id);
                return foundDriveTrain == null ? "" : foundDriveTrain.Name;
            }
            catch //(Exception ex)
            {
                return null;
            }
        }

        private VehicleImage? GetImage(Guid? id)
        {
            try
            {
                if ((id == null) || (id == Guid.Empty)) return null;
                VehicleImage? vImage = Images_List?.FirstOrDefault(x => x?.ImageID == id);
                return vImage;
            }
            catch //(Exception ex)
            {
                return null;
            }
        }

        private Vehicle? CreateNewVehicleFromOld()
        {
            if (Vehicle != null)
            {
                var configs = new List<VConfiguration>(Vehicle.Configurations);
                var vehicle = new Vehicle()
                {
                    ImageID = Vehicle.ImageID,
                    Configurations = configs,
                    ID = Vehicle.ID,
                    Name = Vehicle.Name
                };
                Vehicle = vehicle;
            }
            return Vehicle;
        }

        private async void DeleteConfiguration(int configID)
        {
            VConfiguration? c = null;
            Vehicle? v = null;
            if (configurationDataService != null)
            {
                c = await configurationDataService.GetConfigurationByID(configID);
                await configurationDataService.DeleteConfiguration(configID);
                if (VehicleDataService != null) Vehicle = await VehicleDataService.GetVehicleByID(c.VehicleID);
                StateHasChanged();
            }
        }

        //private async void Filter_Changed(String filter)
        //{
        //    Filter = filter;
        //    await ApplyConfigurationFilter();
        //}

        private void ApplyConfigurationFilter(String? filter)
        {
            if ((String.IsNullOrEmpty(filter) == true) && (Vehicle != null)) Configurations = new List<VConfiguration?>(Vehicle.Configurations);
            else if ((String.IsNullOrEmpty(filter) == false) && (Vehicle != null))
            {
                Configurations = Vehicle?.Configurations?.Where(x => x.Name.Contains(filter, StringComparison.OrdinalIgnoreCase)).ToList();
            }
            else Configurations = new List<VConfiguration?>();
            Configurations = Configurations?.OrderBy(x => x?.Name).ToList();
        }


        #endregion
        ////////////////////////////////////////////////////////////////////////////////////////////////////

    }
}
