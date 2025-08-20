using Microsoft.AspNetCore.Components;
using SpenSoft.DanBeamNG.Services;
using SpenSoft.BeamNG.VehicleObjects;

namespace SpenSoft.DanBeamNG.Pages.Vehicle_Pages
{
    public partial class Vehicle_Edit : ComponentBase
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
        public VehicleDataService_Interface? VehicleDataService { get; set; }

        [Inject]
        public ClassificationsDataService_Interface? classificationsDataService { get; set; }

        [Inject]
        public BodyStyleDataService_Interface? bodyStyleDataService { get; set; }

        [Inject]
        public CountriesDataService_Interface? countriesDataService { get; set; }

        [Inject]
        public DriveTrainDataService_Interface? driveTrainDataService { get; set; }

        //[Inject]
        //public NavigationManager NavigationManager { get; set; }

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
                    _Vehicle = new Vehicle() { ID = 1, Configurations = new List<VConfiguration>() };
                }
                return _Vehicle;
            }
            set { _Vehicle = value; }
        }

        public List<Classifications>? Classifications_List { get; set; }

        public List<Countries>? Countries_List { get; set; }

        public List<BodyStyles>? BodyStyles_List { get; set; }

        public List<DriveTrain>? DriveTrains_List { get; set; }


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
            if (String.IsNullOrEmpty(ID) == false)
            {
                int id = int.Parse(ID);
                if (VehicleDataService != null) Vehicle = (await VehicleDataService.GetVehicleByID(id));
            }

            if (bodyStyleDataService != null) BodyStyles_List = (await bodyStyleDataService.GetAllBodyStyle()).ToList();
            if (classificationsDataService != null) Classifications_List = (await classificationsDataService.GetAllClassifications()).ToList();
            if (countriesDataService != null) Countries_List = (await countriesDataService.GetAllCountries()).ToList();
            if (driveTrainDataService != null) DriveTrains_List = (await driveTrainDataService.GetAllDriveTrain()).ToList();
        }

        #endregion Override Methods
        ////////////////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Protected Methods

        protected async Task InvalidSubmitHandler()
        {
            StatusClass = "alert-danger";
            Message = "The Vehicle data had validation errors.  Please correct and try again";
        }

        protected async Task ValidSumitHandler()
        {
            Saved = false;

            if (Vehicle == null) return;
            if (Vehicle.ID == 0)
            {
                if (VehicleDataService != null)
                {
                    Vehicle? addedVehicle = await VehicleDataService.AddVehicle(Vehicle);

                    if (addedVehicle != null)
                    {
                        StatusClass = "alert-success";
                        Message = "New Vehicle added";
                        Saved = true;
                    }
                    else
                    {
                        StatusClass = "alert-danger";
                        Message = "The new Vehicle could not be dded";
                        Saved = false;
                    }
                }
            }
            else if (VehicleDataService != null)
            {
                await VehicleDataService.UpdateVehicle(Vehicle);
                StatusClass = "alert-success";
                Message = "Vehicle data updated";
                Saved = true;
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
