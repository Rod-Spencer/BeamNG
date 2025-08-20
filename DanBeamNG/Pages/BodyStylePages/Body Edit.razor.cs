using Microsoft.AspNetCore.Components;
using SpenSoft.DanBeamNG.Services;
using SpenSoft.BeamNG.VehicleObjects;
using static System.Net.Mime.MediaTypeNames;

namespace SpenSoft.DanBeamNG.Pages.BodyStylePages
{
    public partial class Body_Edit : ComponentBase
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
        public BodyStyleDataService_Interface? bodyStyleDataService { get; set; }

        #endregion Injections
        ////////////////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Public Properties

        public BodyStyles? bodyStyle { get; set; } = null;

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
            if (String.IsNullOrEmpty(ID) == false)
            {
                Guid id = Guid.Parse(ID);

                if (bodyStyleDataService == null)
                {
                    StatusClass = "alert-danger";
                    Message = $"The bodyStyleDataService object is null";
                    Saved = false;
                    return;
                }
                bodyStyle = await bodyStyleDataService.GetBodyStyleById(id);
                Saved = true;
            }
        }

        #endregion Override Methods
        ////////////////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Protected Methods

        protected async Task InvalidSubmitHandler()
        {
            StatusClass = "alert-danger";
            Message = "The Body Style data had validation errors.  Please correct and try again";
            Saved = false;
        }

        protected async Task ValidSumitHandler()
        {
            Saved = true;
            if (bodyStyle != null)
            {
                if (bodyStyle.ID == Guid.Empty)
                {
                    if (bodyStyleDataService != null)
                    {
                        BodyStyles? testcountry = await bodyStyleDataService.GetBodyStyleByName(bodyStyle.Name);
                        if (testcountry != null)
                        {
                            StatusClass = "alert-danger";
                            Message = $"A Countries record already exists for: {bodyStyle.Name}";
                        }
                        else
                        {
                            bodyStyle.ID = Guid.NewGuid();
                            BodyStyles? addedVehicle = await bodyStyleDataService.AddBodyStyle(bodyStyle);
                            if (addedVehicle != null)
                            {
                                StatusClass = "alert-success";
                                Message = $"New Body Style: {bodyStyle.Name} added";
                                Saved = false;
                            }
                            else
                            {
                                StatusClass = "alert-danger";
                                Message = "The new Body Style could not be added";
                            }
                        }
                    }
                    else
                    {
                        StatusClass = "alert-danger";
                        Message = "The bodyStyleDataService is null";
                    }
                }
                else if (bodyStyleDataService != null)
                {
                    BodyStyles? testcountry = await bodyStyleDataService.GetBodyStyleByName(bodyStyle.Name);
                    if (testcountry != null)
                    {
                        StatusClass = "alert-danger";
                        Message = $"A Countries record already exists for: {bodyStyle.Name}";
                    }
                    else
                    {
                        await bodyStyleDataService.UpdateBodyStyle(bodyStyle);
                        StatusClass = "alert-success";
                        Message = "Body Style data updated";
                        Saved = false;
                    }
                }
                else
                {
                    StatusClass = "alert-danger";
                    Message = "The bodyStyleDataService is null";
                }
            }
            else
            {
                StatusClass = "alert-danger";
                Message = "The Body Style object is null";
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
        #endregion
        ////////////////////////////////////////////////////////////////////////////////////////////////////

    }
}
