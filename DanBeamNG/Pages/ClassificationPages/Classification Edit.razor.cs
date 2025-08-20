using Microsoft.AspNetCore.Components;
using SpenSoft.DanBeamNG.Services;
using SpenSoft.BeamNG.VehicleObjects;
using static System.Net.Mime.MediaTypeNames;

namespace SpenSoft.DanBeamNG.Pages.ClassificationPages
{
    public partial class Classification_Edit : ComponentBase
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

        #endregion Injections
        ////////////////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Public Properties

        public Classifications? classification { get; set; } = null;

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

                if (classificationDataService == null)
                {
                    StatusClass = "alert-danger";
                    Message = $"The classificationDataService object is null";
                    Saved = false;
                    return;
                }
                classification = await classificationDataService.GetClassificationsById(id);
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
            Message = "The Classification data had validation errors.  Please correct and try again";
            Saved = false;
        }

        protected async Task ValidSumitHandler()
        {
            Saved = true;
            if (classification != null)
            {
                if (classification.ID == Guid.Empty)
                {
                    if (classificationDataService != null)
                    {
                        Classifications? testClass = await classificationDataService.GetClassificationsByName(classification.Name);
                        if (testClass != null)
                        {
                            StatusClass = "alert-danger";
                            Message = $"A Countries record already exists for: {classification.Name}";
                        }
                        else
                        {
                            classification.ID = Guid.NewGuid();
                            Classifications? addedClass = await classificationDataService.AddClassifications(classification);
                            if (addedClass != null)
                            {
                                StatusClass = "alert-success";
                                Message = $"New Classification: {classification.Name} added";
                                Saved = false;
                            }
                            else
                            {
                                StatusClass = "alert-danger";
                                Message = "The new Classification could not be added";
                            }
                        }
                    }
                    else
                    {
                        StatusClass = "alert-danger";
                        Message = "The classificationDataService is null";
                    }
                }
                else if (classificationDataService != null)
                {
                    Classifications? testClass = await classificationDataService.GetClassificationsByName(classification.Name);
                    if (testClass != null)
                    {
                        StatusClass = "alert-danger";
                        Message = $"A Countries record already exists for: {classification.Name}";
                    }
                    else
                    {
                        await classificationDataService.UpdateClassifications(classification);
                        StatusClass = "alert-success";
                        Message = "Classification data updated";
                        Saved = false;
                    }
                }
                else
                {
                    StatusClass = "alert-danger";
                    Message = "The classificationDataService is null";
                }
            }
            else
            {
                StatusClass = "alert-danger";
                Message = "The Classification object is null";
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
