using SpenSoft.BeamNG.VehicleObjects;

namespace SpenSoft.DanBeamNG.Services;


public interface DriveTrainDataService_Interface
{
    Task<List<DriveTrain>?> GetAllDriveTrain();
    Task<DriveTrain?> GetDriveTrainByID(Guid? driveTrainID);
    Task<DriveTrain?> GetDriveTrainByName(String? driveTrainID);
    Task<DriveTrain?> AddDriveTrain(DriveTrain? driveTrain);
    Task<Boolean> UpdateDriveTrain(DriveTrain? driveTrain);
    Task<Boolean> DeleteDriveTrain(Guid? driveTrainID);
}


#if false
public interface DriveTrainDataService_Interface
{
    Task<List<DriveTrain>> GetAllDriveTrain();
    Task<DriveTrain> GetDriveTrainById(Guid? dataTrainID);
    Task<DriveTrain> GetDriveTrainByName(String? dataTrainNameD);
    Task<DriveTrain?> AddDriveTrain(DriveTrain? driveTrain);
    Task UpdateDriveTrain(DriveTrain dataTrain);
    Task<Boolean> DeleteDriveTrain(Guid? dataTrainID);
}
#endif
