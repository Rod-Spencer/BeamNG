using Microsoft.AspNetCore.Components;
using SpenSoft.BeamNG.VehicleObjects;
using SpenSoft.DanBeamNG.Services;

namespace SpenSoft.DanBeamNG.Pages.VehiclePages
{
    public partial class Vehicle_View : ComponentBase
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

        [Inject]
        public VehicleImageDataService_Interface? vehicleImageDataService { get; set; }

        #endregion Injections
        ////////////////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Public Properties

        public Vehicle? vehicle { get; set; }

        public List<Classifications>? Classifications_List { get; set; }

        public List<Countries>? Countries_List { get; set; }

        public List<BodyStyles>? BodyStyles_List { get; set; }

        public List<DriveTrain>? DriveTrains_List { get; set; }

        public List<VehicleImage>? Images_List { get; set; }


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
            int id = 0;
            try
            {
                if (String.IsNullOrEmpty(ID) == true)
                {
                    vehicle = null;
                    return;
                }

                id = int.Parse(ID);
                if (vehicle == null)
                {
                    if (VehicleDataService != null) vehicle = (await VehicleDataService.GetVehicleByID(id));
                    if (vehicle == null) return;
                }

                if (bodyStyleDataService != null) BodyStyles_List = (await bodyStyleDataService.GetAllBodyStyle()).ToList();
                if (classificationsDataService != null) Classifications_List = (await classificationsDataService.GetAllClassifications()).ToList();
                if (countriesDataService != null) Countries_List = (await countriesDataService.GetAllCountries()).ToList();
                if (driveTrainDataService != null) DriveTrains_List = (await driveTrainDataService.GetAllDriveTrain()).ToList();
                if (vehicleImageDataService != null) Images_List = (await vehicleImageDataService.GetAllVehicleImage()).OrderBy(x => x.ImageName).ToList();
                //Images_List = Vehicles.AllImages;
                //if ((Images_List == null) || (Images_List.Count > 0))
                //{
                //    if (vehicleImageDataService != null)
                //    {
                //        Images_List = (await vehicleImageDataService.GetAllVehicleImage()).OrderBy(x => x.ImageName).ToList();
                //        Vehicles.AllImages = Images_List;
                //    }
                //}

            }
            catch /*(Exception ex)*/
            {
                return;
            }



            //if (String.IsNullOrEmpty(ID) == false)
            //{
            //    int id = int.Parse(ID);
            //    if (VehicleDataService != null)
            //    {
            //        vehicle = (await VehicleDataService.GetVehicleByID(id));
            //    }
            //}
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
            catch // (Exception ex)
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
            catch // (Exception ex)
            {
                return null;
            }
        }

        //private VehicleImage? GetImage(Guid? id)
        //{
        //    try
        //    {
        //        if ((id == null) || (id == Guid.Empty)) return null;
        //        VehicleImage? vImage = Images_List?.FirstOrDefault(x => x.ImageID == id);
        //        return vImage;
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}

        #endregion
        ////////////////////////////////////////////////////////////////////////////////////////////////////

    }
}
