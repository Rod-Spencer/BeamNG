using SpenSoft.BeamNG.VehicleObjects;

namespace SpenSoft.DanBeamNG.Services;


public interface ImagesDataDataService_Interface
{
    Task<List<ImagesData>?> GetAllImagesData();
    Task<ImagesData?> GetImagesDataByID(Guid? imageID);
    Task<ImagesData?> AddImagesData(ImagesData? image);
    Task<Boolean> UpdateImagesData(ImagesData? image);
    Task<Boolean> DeleteImagesData(Guid? imageID);
}

#if false

public interface ImagesDataDataService_Interface
{
    Task<List<ImagesData>?> GetAllImagesData();
    Task<ImagesData?> GetImagesDataById(Guid? imageID);
    Task<List<ImagesData>?> GetImagesDataByVehicleId(int vehicleID);
    Task<ImagesData?> AddImagesData(ImagesData? image);
    Task UpdateImagesData(ImagesData? image);
    Task<Boolean> DeleteImagesData(int imageID);
}
#endif
