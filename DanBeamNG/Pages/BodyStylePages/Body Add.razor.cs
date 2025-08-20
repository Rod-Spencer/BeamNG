using Microsoft.AspNetCore.Components;
using SpenSoft.BeamNG.VehicleObjects;
using SpenSoft.DanBeamNG.Services;

namespace SpenSoft.DanBeamNG.Pages.BodyStylePages
{
    public partial class Body_Add : ComponentBase
    {
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
            bodyStyle = new BodyStyles();
            Saved = true;
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
            Saved = false;
            if (bodyStyle != null)
            {
                if (bodyStyle.ID == Guid.Empty)
                {
                    if (bodyStyleDataService != null)
                    {
                        BodyStyles? testBodyStyle = await bodyStyleDataService.GetBodyStyleByName(bodyStyle.Name);
                        if (testBodyStyle != null)
                        {
                            StatusClass = "alert-danger";
                            Message = $"A Body Style already exists for: {bodyStyle.Name}";
                        }
                        else
                        {
                            bodyStyle.ID = Guid.NewGuid();
                            BodyStyles? addedVehicle = await bodyStyleDataService.AddBodyStyle(bodyStyle);
                            if (addedVehicle != null)
                            {
                                StatusClass = "alert-success";
                                Message = $"New Body Style: {bodyStyle.Name} added";
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
                    await bodyStyleDataService.UpdateBodyStyle(bodyStyle);
                    StatusClass = "alert-success";
                    Message = "Body Style data updated";
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
