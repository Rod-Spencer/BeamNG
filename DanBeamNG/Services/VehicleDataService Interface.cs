using SpenSoft.BeamNG.VehicleObjects;
using System.Threading.Tasks;

namespace SpenSoft.DanBeamNG.Services;


public interface VehicleDataService_Interface
{
    Task<List<Vehicle>?> GetAllVehicle();
    Task<Vehicle?> GetVehicleByID(int? vehicleID);
    Task<Vehicle?> GetVehicleByName(String? vehicleID);
    Task<Vehicle?> AddVehicle(Vehicle? vehicle);
    Task<Boolean> UpdateVehicle(Vehicle? vehicle);
    Task<Boolean> DeleteVehicle(int? vehicleID);
}

#if false
public interface VehicleDataService_Interface
{
    Task<List<Vehicle>?> GetAllVehicles();
    Task<Vehicle?> GetVehicleById(int vehicleId);
    Task<Vehicle?> GetVehicleByName(String name);
    Task<Vehicle?> AddVehicle(Vehicle? vehicle);
    Task<Vehicle?> UpdateVehicle(Vehicle? vehicle);
    Task<Boolean> DeleteVehicle(int vehicleId);
}
#endif
