using Microsoft.AspNetCore.Components;
using Newtonsoft.Json.Linq;
using SpenSoft.DanBeamNG.Services;
using SpenSoft.BeamNG.VehicleObjects;
using System.Net;

namespace SpenSoft.DanBeamNG.Pages.ClassificationPages
{
    public partial class Classification_Delete : ComponentBase
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
        public ClassificationsDataService_Interface? classificationDataService { get; set; }

        [Inject]
        public ConfigurationDataService_Interface? configurationDataService { get; set; }

        [Inject]
        public VehicleDataService_Interface? vehicleDataService { get; set; }

        #endregion Injections
        ////////////////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Public Properties

        public Classifications? classification { get; set; } = null;
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

                if (classificationDataService == null)
                {
                    StatusClass = "alert-danger";
                    Message = $"The classificationDataService object is null";
                    return;
                }

                if (configurationDataService == null)
                {
                    StatusClass = "alert-warning";
                    Message = $"The ConfigurationDataService is NULL";
                    return;
                }

                classification = await classificationDataService.GetClassificationsById(id);
                if (classification == null)
                {
                    StatusClass = "alert-warning";
                    Message = $"The Classification with ID: {id} could not be found";
                    return;
                }

                var v = await vehicleDataService.GetAllVehicle();
                var configList = (await configurationDataService.GetAllConfiguration())
                      .Where(x => x.ClassificationID == classification.ID)
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
                    Message = $"The Classification: {classification.Name} can not be deleted because it's in use by the following vehicle configurations:";
                    return;
                }


                try
                {
                    if (await classificationDataService.DeleteClassifications(id) == true)
                    {
                        StatusClass = "alert-success";
                        Message = $"The Classification: {classification.Name} has been deleted";
                        Deleted = false;
                    }
                    else
                    {
                        StatusClass = "alert-warning";
                        Message = $"The Classification ID: {id} could not be found";
                    }
                }
                catch (Exception ex)
                {
                    StatusClass = "alert-danger";
                    Message = $"Deleting Classification with ID: {id} returned the following error: {ex.Message}";
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
