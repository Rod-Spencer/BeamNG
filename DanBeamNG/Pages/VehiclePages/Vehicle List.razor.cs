using Microsoft.AspNetCore.Components;
using NLog;
using Segway.Service.Helper.Except;
using Segway.Service.LoggerHelper;
using SpenSoft.BeamNG.VehicleObjects;
using SpenSoft.DanBeamNG.Services;

namespace SpenSoft.DanBeamNG.Pages.VehiclePages
{
    public partial class Vehicle_List : ComponentBase
    {
        private static Logger logger = Logger_Helper.GetCurrentLogger();
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
        public VehicleDataService_Interface? VehicleDataService { get; set; }

        [Inject]
        public VehicleImageDataService_Interface? imageDataService { get; set; }

        [Inject]
        public Browser_Service? browserService { get; set; }


        #endregion Injections
        ////////////////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Public Properties

        public List<Vehicle?>? VehicleList { get; set; }

        public Vehicle? vehicle { get; set; } = new Vehicle();

        public List<VehicleImage>? Images_List { get; set; }

        public int itemHeight { get; } = 50;

        public int Height { get; set; }

        public int Width { get; set; }



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
        #endregion Override Methods
        ////////////////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Protected Methods

        protected override async Task OnInitializedAsync()
        {
            try
            {
                if (VehicleDataService != null) VehicleList = (await VehicleDataService.GetAllVehicle()).OrderBy((Vehicle? x) => x?.Name).ToList();
                if (imageDataService != null) Images_List = (await imageDataService.GetAllVehicleDefaultImage()).ToList();
            }
            catch (Exception ex)
            {
                logger.Error(Exception_Helper.FormatExceptionString(ex), nameof(OnInitializedAsync), nameof(OnInitializedAsync));
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

        //private VehicleImage? GetImage(Guid? id)
        //{
        //    try
        //    {
        //        if ((id == null) || (id == Guid.Empty)) return null;
        //        var i = Images_List?.FirstOrDefault(x => x.ImageID == id);
        //        return i;
        //    }
        //    catch
        //    {
        //        return null;
        //    }
        //}

        private async Task deleteEventHandler(int vehicleID)
        {
            if (VehicleDataService == null) return;
            Boolean deleted = await VehicleDataService.DeleteVehicle(vehicleID);
            if (deleted == true)
            {
                VehicleList = (await VehicleDataService.GetAllVehicle()).OrderBy((Vehicle? x) => x?.Name).ToList();
            }
        }

        #endregion
        ////////////////////////////////////////////////////////////////////////////////////////////////////

    }
}
