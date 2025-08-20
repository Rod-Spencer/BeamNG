using Microsoft.AspNetCore.Components;
using SpenSoft.DanBeamNG.Services;
using SpenSoft.BeamNG.VehicleObjects;

namespace SpenSoft.DanBeamNG.Pages.DriveTrainPages
{
    public partial class DriveTrain_Edit : ComponentBase
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

        #endregion Injections
        ////////////////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Public Properties

        public DriveTrain? driveTrain { get; set; } = null;

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

                if (driveTrainDataService == null)
                {
                    StatusClass = "alert-danger";
                    Message = $"The driveTrainDataService object is null";
                    Saved = false;
                    return;
                }
                driveTrain = await driveTrainDataService.GetDriveTrainByID(id);
                Saved= true;
            }
        }

        #endregion Override Methods
        ////////////////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Protected Methods

        protected async Task InvalidSubmitHandler()
        {
            StatusClass = "alert-danger";
            Message = "The DriveTrain data had validation errors.  Please correct and try again";
            Saved = false;
        }

        protected async Task ValidSumitHandler()
        {
            Saved = true;
            if (driveTrain != null)
            {
                if (driveTrain.ID == Guid.Empty)
                {
                    if (driveTrainDataService != null)
                    {
                        DriveTrain? testDrive = await driveTrainDataService.GetDriveTrainByName(driveTrain.Name);
                        if (testDrive != null)
                        {
                            StatusClass = "alert-danger";
                            Message = $"A Drive Train record already exists for: {driveTrain.Name}";
                        }
                        else
                        {
                            driveTrain.ID = Guid.NewGuid();
                            DriveTrain? addedDrive = await driveTrainDataService.AddDriveTrain(driveTrain);
                            if (addedDrive != null)
                            {
                                StatusClass = "alert-success";
                                Message = $"New Drive Train: {driveTrain.Name} added";
                                Saved = false;
                            }
                            else
                            {
                                StatusClass = "alert-danger";
                                Message = "The new Drive Train could not be added";
                            }
                        }
                    }
                    else
                    {
                        StatusClass = "alert-danger";
                        Message = "The driveTrainDataService is null";
                    }
                }
                else if (driveTrainDataService != null)
                {
                    DriveTrain? testDrive = await driveTrainDataService.GetDriveTrainByID(driveTrain.ID);
                    if (testDrive != null)
                    {
                        StatusClass = "alert-danger";
                        Message = $"A Drive Train record already exists for: {driveTrain.Name}";
                    }
                    else
                    {
                        await driveTrainDataService.UpdateDriveTrain(driveTrain);
                        StatusClass = "alert-success";
                        Message = "Drive Train data updated";
                        Saved = false;
                    }
                }
                else
                {
                    StatusClass = "alert-danger";
                    Message = "The driveTrainDataService is null";
                }
            }
            else
            {
                StatusClass = "alert-danger";
                Message = "The DriveTrain object is null";
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
