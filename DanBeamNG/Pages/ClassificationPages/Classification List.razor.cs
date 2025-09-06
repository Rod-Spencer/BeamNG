using Microsoft.AspNetCore.Components;
using SpenSoft.DanBeamNG.Services;
using SpenSoft.BeamNG.VehicleObjects;

namespace SpenSoft.DanBeamNG.Pages.ClassificationPages
{
    public partial class Classification_List : ComponentBase
    {
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Constructors
        #endregion
        ////////////////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Parameters

        //[Parameter]
        //public String? ID { get; set; }

        #endregion Parameters
        ////////////////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Injections

        [Inject]
        public ClassificationsDataService_Interface? classificationDataService { get; set; }

        #endregion Injections
        ////////////////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Private Properties

        //private Logger logger = Logger_Helper.GetCurrentLogger();

        #endregion Private Properties
        ////////////////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Public Properties

        public List<Classifications>? classificationList { get; set; } = null;

        public Classifications? classification { get; set; }

        public int itemHeight { get; } = 50;

        #endregion Public Properties
        ////////////////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Screen State Properties

        protected String Message1 = String.Empty;
        protected String Message2 = String.Empty;
        protected String StatusClass = String.Empty;
        protected Boolean Error = false;

        #endregion Screen State Properties
        ////////////////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Override Methods

        protected override async Task OnInitializedAsync()
        {
            Error = false;
            try
            {
                classificationList = null;
                classificationList = (await classificationDataService.GetAllClassifications()).OrderBy(x => x.Name).ToList();
            }
            catch (Exception ex)
            {
                //logger.Error(Exception_Helper.FormatExceptionString(ex));
                StatusClass = "alert-danger"; Message1 = $"The following Exception error was thrown while trying to retrieve the image list: {ex.Message}";
                Message2 = "Please see runtime log for more details";
                Error = true;
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

        public async Task Delete_Classification_Handler(Classifications? cls)
        {
            if (cls == null) return;
            Error = false;
            try
            {
                if (classificationDataService != null)
                {
                    await classificationDataService.DeleteClassifications(cls.ID);
                    var clsList = classificationDataService.GetAllClassifications();
                    if (clsList != null)
                    {
                        classificationList = clsList?.Result?.OrderBy(x => x.Name).ToList();
                    }

                }
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                //logger.Error(Exception_Helper.FormatExceptionString(ex));
                StatusClass = "alert-danger"; Message1 = $"The following Exception error was thrown while trying to delete the classification: {ex.Message}";
                Message2 = "Please see runtime log for more details";
                Error = true;
            }
        }

        #endregion
        ////////////////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Private Methods
        #endregion
        ////////////////////////////////////////////////////////////////////////////////////////////////////
    }
}
