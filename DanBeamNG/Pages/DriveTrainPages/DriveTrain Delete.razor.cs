using Microsoft.AspNetCore.Components;
using SpenSoft.DanBeamNG.Services;
using SpenSoft.BeamNG.VehicleObjects;

namespace SpenSoft.DanBeamNG.Pages.DriveTrainPages
{
    public partial class DriveTrain_Delete : ComponentBase
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
        public DriveTrainDataService_Interface? driveTrainDataService { get; set; }

        [Inject]
        public VehicleDataService_Interface? vehicleDataService { get; set; }

        [Inject]
        public ConfigurationDataService_Interface? configurationDataService { get; set; }

        #endregion Injections
        ////////////////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Public Properties

        public DriveTrain? driveTrain { get; set; } = null;
        public List<InUseConfig> inUseConfigs { get; set; } = null;

        #endregion Public Properties
        ////////////////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Screen State Properties

        protected String Message = String.Empty;
        protected String StatusClass = String.Empty;
        protected Boolean Deleted;

        #endregion Screen State Properties
        ////////////////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Override Methods

        protected override async Task OnInitializedAsync()
        {
            Deleted = false;
            inUseConfigs = null;
            if (String.IsNullOrEmpty(ID) == false)
            {
                Guid id = Guid.Parse(ID);

                if (configurationDataService == null)
                {
                    StatusClass = "alert-warning";
                    Message = $"The ConfigurationDataService is NULL";
                    return;
                }

                if (driveTrainDataService == null)
                {
                    StatusClass = "alert-danger";
                    Message = $"The driveTrainDataService object is null";
                    return;
                }

                driveTrain = await driveTrainDataService.GetDriveTrainByID(id);
                if (driveTrain == null)
                {
                    StatusClass = "alert-warning";
                    Message = $"The Country with ID: {id} could not be found";
                    return;
                }

                var v = await vehicleDataService.GetAllVehicle();
                var configList = (await configurationDataService.GetAllConfiguration())
                      .Where(x => x.DriveTrainID == id)
                      .ToList();

                if (configList.Count > 0)
                {
                    List<InUseConfig> inUse = new List<InUseConfig>();
                    configList.ForEach(x =>
                    {
                        var iuc = new InUseConfig() { ConfigurationID = x.ID, ConfigurationName = x.Name, VehicleID = x.VehicleID };
                        inUse.Add(iuc);
                        iuc.VehicleName = v.First(x => x.ID == iuc.VehicleID).Name;
                    });
                    inUseConfigs = inUse;
                    StatusClass = "alert-warning";
                    Message = $"The Drive Train: {driveTrain.Name} can not be deleted because it's in use by the following vehicle configurations:";
                    return;
                }


                try
                {
                    if (await driveTrainDataService.DeleteDriveTrain(id) == true)
                    {
                        StatusClass = "alert-success";
                        Message = $"The Country: {driveTrain.Name} has been deleted";
                        Deleted = false;
                    }
                    else
                    {
                        StatusClass = "alert-warning";
                        Message = $"The Country ID: {id} could not be found";
                    }
                }
                catch (Exception ex)
                {
                    StatusClass = "alert-danger";
                    Message = $"Deleting Country with ID: {id} returned the following error: {ex.Message}";
                    return;
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
        #endregion
        ////////////////////////////////////////////////////////////////////////////////////////////////////

    }
}
