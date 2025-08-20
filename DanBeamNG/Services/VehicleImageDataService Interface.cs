using SpenSoft.BeamNG.VehicleObjects;

namespace SpenSoft.DanBeamNG.Services
{
    public interface VehicleImageDataService_Interface
    {
        Task<List<VehicleImage>?> GetAllVehicleImage();
        Task<List<VehicleImage>?> GetUnassignedImages();
        Task<List<VehicleImage>?> GetAllImagesByVehicle(int vid);
        Task<List<VehicleImage>?> GetAllVehicleDefaultImage();
        Task<VehicleImage?> GetVehicleImageById(Guid? imageID);
        Task<VehicleImage?> GetVehicleImageByName(String? vehicleName, String? configImageName);
        Task<VehicleImage?> AddVehicleImage(VehicleImage? image);
        Task UpdateVehicleImage(VehicleImage? image);
        Task<Boolean> DeleteVehicleImage(Guid? imageID);
        Task<VehicleImage?> GetVehicleImageByName(String? imageName);
    }
}
