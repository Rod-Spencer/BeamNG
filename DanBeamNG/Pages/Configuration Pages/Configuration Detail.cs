using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using SpenSoft.DanBeamNG.Services;
using SpenSoft.BeamNG.VehicleObjects;

namespace SpenSoft.DanBeamNG.Pages.Configuration_Pages
{
    public partial class Configuration_Detail : ComponentBase
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
        public ConfigurationDataService_Interface? ConfigurationDataService { get; set; }

        [Inject]
        public VehicleDataService_Interface? VehicleDataService { get; set; }

        [Inject]
        public VehicleImageDataService_Interface? ImageDataService { get; set; }

        [Inject]
        public NavigationManager? NavigationManager { get; set; }

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

        public VConfiguration? Configuration { get; set; }

        public Vehicle? Vehicle { get; set; }

        public int NextConfigID { get; set; }

        public int PrevConfigID { get; set; }

        //public Byte[]? ImageData { get; set; }

        public String ImageName { get; set; }

        public List<Classifications>? Classifications_List { get; set; }

        public List<Countries>? Countries_List { get; set; }

        public List<BodyStyles>? BodyStyles_List { get; set; }

        public List<DriveTrain>? DriveTrains_List { get; set; }

        public VehicleImage? vehicleImage { get; set; }

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

        protected override async Task OnInitializedAsync()
        {
            await GetVehicleAndConfiguration();
            return;
        }

        #endregion Override Methods
        ////////////////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Protected Methods

        protected void GetPrevConfiguration()
        {
            if (String.IsNullOrEmpty(ID) == false)
            {
                int id = int.Parse(ID);

                if ((id == Configuration.ID) && (Vehicle.ID == Configuration.VehicleID))
                {
                    for (int i = 0; i < Vehicle.Configurations.Count; i++)
                    {
                        if (Vehicle.Configurations[i].ID == id)
                        {
                            if (i > 0)
                            {
                                id = Vehicle.Configurations[i - 1].ID;
                            }
                            break;
                        }
                    }
                }
                if (NavigationManager != null)
                {
                    NavigationManager.NavigateTo($"/ViewConfiguration/{id}");
                }
            }
        }


        protected void GetNextConfiguration()
        {
            if (String.IsNullOrEmpty(ID) == false)
            {
                int id = int.Parse(ID);

                if ((id == Configuration.ID) && (Vehicle.ID == Configuration.VehicleID))
                {
                    for (int i = 0; i < Vehicle.Configurations.Count; i++)
                    {
                        if (Vehicle.Configurations[i].ID == id)
                        {
                            if (i < Vehicle.Configurations.Count)
                            {
                                id = Vehicle.Configurations[i + 1].ID;
                            }
                            break;
                        }
                    }
                }
                if (NavigationManager != null)
                {
                    NavigationManager.NavigateTo($"/ViewConfiguration/{id}");
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

        private async Task GetVehicleAndConfiguration()
        {
            int id = int.Parse(ID.ToString());
            if (ConfigurationDataService != null)
            {
                Configuration = await ConfigurationDataService.GetConfigurationByID(id);

                if (Configuration != null)
                {
                    if (Vehicle == null)
                    {
                        if (VehicleDataService != null) Vehicle = (await VehicleDataService.GetVehicleByID(Configuration.VehicleID));
                    }
                    else if (Configuration.VehicleID != Vehicle.ID)
                    {
                        if (VehicleDataService != null) Vehicle = (await VehicleDataService.GetVehicleByID(Configuration.VehicleID));
                    }

                    if (ImageDataService != null)
                    {
                        if ((Configuration == null) || (Configuration.ImageID.HasValue == false))
                        {
                            //ImageData = null;
                            return;
                        }
                        vehicleImage = await ImageDataService.GetVehicleImageById(Configuration.ImageID.Value);
                        if (vehicleImage != null)
                        {
                            //ImageData = vehicleImage.ImageData;
                            ImageName = vehicleImage.ImageName;
                        }
                        else
                        {
                            //ImageData = null;
                            ImageName = null;
                        }
                    }
                }
            }
            if (bodyStyleDataService != null) BodyStyles_List = (await bodyStyleDataService.GetAllBodyStyle()).ToList();
            if (classificationsDataService != null) Classifications_List = (await classificationsDataService.GetAllClassifications()).ToList();
            if (countriesDataService != null) Countries_List = (await countriesDataService.GetAllCountries()).ToList();
            if (driveTrainDataService != null) DriveTrains_List = (await driveTrainDataService.GetAllDriveTrain()).ToList();
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
