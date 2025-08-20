using Microsoft.AspNetCore.Components;
using SpenSoft.DanBeamNG.Services;
using SpenSoft.BeamNG.VehicleObjects;

namespace SpenSoft.DanBeamNG.Pages.ImagePages
{
    public partial class Image_Delete : ComponentBase
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
        public VehicleImageDataService_Interface? imageDataService { get; set; }

        [Inject]
        public ConfigurationDataService_Interface? ConfigurationDataService { get; set; }
        [Inject]
        public NavigationManager navigationManager { get; set; }

        #endregion Injections
        ////////////////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Public Properties

        public VehicleImage? vehicleImage { get; set; }

        public List<VConfiguration>? Configurations { get; set; }

        #endregion Public Properties
        ////////////////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Screen State Properties

        protected String Message = String.Empty;
        protected String StatusClass = String.Empty;
        protected Boolean Deleted = false;

        #endregion Screen State Properties
        ////////////////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Override Methods

        protected override async Task OnInitializedAsync()
        {
            Deleted = true;
            if (String.IsNullOrEmpty(ID) == false)
            {
                Guid id = Guid.Parse(ID);
                if (ConfigurationDataService == null)
                {
                    StatusClass = "alert-warning";
                    Message = $"The ConfigurationDataService is NULL";
                    return;
                }

                Configurations = (await ConfigurationDataService.GetAllConfiguration())
                    .Where(x => x.ImageID == id)
                    .ToList();
                if (Configurations == null)
                {
                    StatusClass = "alert-warning";
                    Message = $"The request for configurations returned a NULL";
                    return;
                }

                Configurations.ForEach(x =>
                {
                    x.ImageID = null;
                    ConfigurationDataService.UpdateConfiguration(x);
                });


                if (imageDataService == null)
                {
                    StatusClass = "alert-danger";
                    Message = $"The configurationDataService object is null";
                    return;
                }


                try
                {
                    vehicleImage = await imageDataService.GetVehicleImageById(id);
                    if (await imageDataService.DeleteVehicleImage(id) == true)
                    {
                        navigationManager.NavigateTo("/ListImages");
                        //StatusClass = "alert-success";
                        //Messages = $"The Image: {vehicleImage.ImageName} has been deleted";
                        //Deleted = false;
                    }
                    else
                    {
                        StatusClass = "alert-warning";
                        Message = $"The Image: {vehicleImage.ImageName} could not be found";
                    }
                }
                catch (Exception ex)
                {
                    StatusClass = "alert-danger";
                    Message = $"Deleting ImageID: {id} returned the following error: {ex.Message}";
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
