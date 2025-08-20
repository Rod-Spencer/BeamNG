using Microsoft.AspNetCore.Components;
using SpenSoft.DanBeamNG.Services;
using SpenSoft.BeamNG.VehicleObjects;

namespace SpenSoft.DanBeamNG.Pages.DriveTrainPages
{
    public partial class DriveTrain_Add : ComponentBase
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
            driveTrain = new DriveTrain();
            Saved = true;
        }

        #endregion Override Methods
        ////////////////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Protected Methods

        protected async Task InvalidSubmitHandler()
        {
            StatusClass = "alert-danger";
            Message = "The drive train data had validation errors.  Please correct and try again";
            Saved = false;
        }

        protected async Task ValidSumitHandler()
        {
            Saved = false;
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
                            Message = $"A DriveTrain record already exists for: {driveTrain.Name}";
                        }
                        else
                        {
                            driveTrain.ID = Guid.NewGuid();
                            DriveTrain? addedDrive = await driveTrainDataService.AddDriveTrain(driveTrain);
                            if (addedDrive != null)
                            {
                                StatusClass = "alert-success";
                                Message = $"New Drive Train: {driveTrain.Name} added";
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
                    await driveTrainDataService.UpdateDriveTrain(driveTrain);
                    StatusClass = "alert-success";
                    Message = "Drive Train data updated";
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
