using NLog;
using Segway.Service.LoggerHelper;
using SpenSoft.BeamNG.VehicleObjects;
using SpenSoft.EF.BeamNG;

namespace SpenSoft.DanBeamNG.Services;

public class VehicleDataService : VehicleDataService_Interface
{
    private static readonly Logger logger = Logger_Helper.GetCurrentLogger();
    static BeamNGContext? bngDB = null;

    public VehicleDataService(BeamNGContext _bngDB)
    {
        bngDB = _bngDB;
    }

    public async Task<List<Vehicle>?> GetAllVehicle()
    {
        try
        {
            List<Vehicle>? vList = null;
            logger.Info($"Entered {nameof(GetAllVehicle)}", nameof(GetAllVehicle));

            vList = bngDB?.Vehicles?.ToList();

            var cList = bngDB?.Configurations?.ToList();
            vList?.ForEach(v =>
            {
                var cl = cList?.Where(c => c.VehicleID == v.ID).ToList();
                if (cl != null) v.Configurations?.AddRange(cl);
            });
            return vList;
        }
        catch (Exception ex)
        {
            logger.Error($"Exception: {ex.Message}", nameof(GetAllVehicle));
            throw;
        }
        finally
        {
            logger.Info($"Leaving {nameof(GetAllVehicle)}", nameof(GetAllVehicle));
        }

    }

    public async Task<Vehicle?> GetVehicleByID(int? vehicleID)
    {
        try
        {
            logger.Info($"Entered {nameof(GetVehicleByID)}", nameof(GetVehicleByID));
            var v = bngDB?.Vehicles?.FirstOrDefault<Vehicle>(x => x.ID == vehicleID);
            if (v == null) { return null; }
            var cList = bngDB?.Configurations?.Where(c => c.VehicleID == vehicleID).ToList();
            if (cList != null)
            {
                v.Configurations = cList;
            }
            return v;
        }
        catch (Exception ex)
        {
            logger.Error($"Exception: {ex.Message}", nameof(GetVehicleByID));
            throw;
        }
        finally
        {
            logger.Info($"Leaving {nameof(GetVehicleByID)}", nameof(GetVehicleByID));
        }
    }

    public async Task<Vehicle?> GetVehicleByName(String? vehicleName)
    {
        try
        {
            logger.Info($"Entered {nameof(GetVehicleByName)}", nameof(GetVehicleByName));
            var v = bngDB?.Vehicles?.FirstOrDefault<Vehicle>(x => x.Name == vehicleName);
            return v;
        }
        catch (Exception ex)
        {
            logger.Error($"Exception: {ex.Message}", nameof(GetVehicleByName));
            throw;
        }
        finally
        {
            logger.Info($"Leaving {nameof(GetVehicleByName)}", nameof(GetVehicleByName));
        }
    }

    public async Task<Vehicle?> AddVehicle(Vehicle? vehicle)
    {
        try
        {
            logger.Info($"Entered {nameof(AddVehicle)}", nameof(AddVehicle));
            if (vehicle == null) { return null; }
            bngDB?.Vehicles?.Add(vehicle);
            bngDB?.SaveChanges();
            logger.Debug($"Vehicle ID: {vehicle.ID}", nameof(AddVehicle));
            return vehicle;
        }
        catch (Exception ex)
        {
            logger.Error($"Exception: {ex.Message}", nameof(AddVehicle));
            throw;
        }
        finally
        {
            logger.Info($"Leaving {nameof(AddVehicle)}", nameof(AddVehicle));
        }
    }

    public async Task<Boolean> UpdateVehicle(Vehicle? vehicle)
    {
        try
        {
            logger.Info($"Entered {nameof(UpdateVehicle)}", nameof(UpdateVehicle));
            if (vehicle == null) { return false; }
            bngDB?.Vehicles?.Update(vehicle);
            bngDB?.SaveChanges();
            return true;
        }
        catch (Exception ex)
        {
            logger.Error($"Exception: {ex.Message}", nameof(UpdateVehicle));
            throw;
        }
        finally
        {
            logger.Info($"Leaving {nameof(UpdateVehicle)}", nameof(UpdateVehicle));
        }
    }

    public async Task<Boolean> DeleteVehicle(int? vehicleID)
    {
        try
        {
            logger.Info($"Entered {nameof(UpdateVehicle)}", nameof(UpdateVehicle));
            if (vehicleID == null) { return false; }
            var vehicle = bngDB?.Vehicles?.FirstOrDefault(x => x.ID == vehicleID);
            if (vehicle == null) { return false; }
            bngDB?.Vehicles?.Remove(vehicle);
            bngDB?.SaveChanges();
            return true;
        }
        catch (Exception ex)
        {
            logger.Error($"Exception: {ex.Message}", nameof(UpdateVehicle));
            throw;
        }
        finally
        {
            logger.Info($"Leaving {nameof(UpdateVehicle)}", nameof(UpdateVehicle));
        }
    }
}


#if false
public class VehicleDataService : VehicleDataService_Interface
{
    private static readonly Logger logger = Logger_Helper.GetCurrentLogger();
    static BeamNGContext? bngDB = null;

    public VehicleDataService(BeamNGContext _bngDB)
    {
        bngDB = _bngDB;
        logger = cl;
    }

    public async Task<Vehicle?> AddVehicle(Vehicle? vehicle)
    {
        try
        {
            await logger.AddLoggingMessage($"Entered {nameof(AddVehicle)}");
            if (vehicle == null) { return null; }
            bngDB?.Vehicles?.Add(vehicle);
            await logger.AddLoggingMessage($"Vehicle ID: {vehicle.ID}");
            return vehicle;
        }
        catch (Exception ex)
        {
            await logger.AddLoggingMessage($"Exception: {ex.Message}");
            throw;
            //return null;
        }
        finally
        {
            await logger.AddLoggingMessage($"Entered {nameof(AddVehicle)}");
        }
    }

    public async Task<Boolean> DeleteVehicle(int id)
    {
        if (bngDB?.Vehicles == null) { return false; }
        var vehicle = bngDB.Vehicles.FirstOrDefault(v => v.ID == id);
        if (vehicle != null)
        {
            bngDB.Vehicles.Remove(vehicle);
            await bngDB.SaveChangesAsync();
            return true;
        }
        return false;
    }

    public async Task<List<Vehicle>?> GetAllVehicles()
    {
        try
        {
            List<Vehicle>? viList = null;
            if (logger != null)
            {
                await logger.AddLoggingMessage($"Entered {nameof(GetAllVehicles)}");
            }

            viList = bngDB?.Vehicles?.ToList();
            return viList;
        }
        catch (Exception ex)
        {
            if (logger != null) await logger.AddLoggingMessage($"Exception: {ex.Message}");
            throw;
        }
        finally
        {
            if (logger != null) await logger.AddLoggingMessage($"Leaving {nameof(GetAllVehicles)}");
        }

    }

    public async Task<Vehicle?> GetVehicleByName(String name)
    {
        var result = await JsonSerializer.DeserializeAsync<Vehicle?>(await _httpClient.GetStreamAsync($"vehicle/Name/{name}"),
            new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        return result;
    }

    public async Task<Vehicle?> GetVehicleById(int id)
    {
        return await JsonSerializer.DeserializeAsync<Vehicle?>(await _httpClient.GetStreamAsync($"vehicle/ID/{id}"),
            new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
    }

    public async Task<Vehicle?> UpdateVehicle(Vehicle? vehicle)
    {
        var vehicleJson = new StringContent(JsonSerializer.Serialize(vehicle), Encoding.UTF8, "application/json");

        var response = await _httpClient.PutAsync("vehicle/update", vehicleJson);

        if (response.IsSuccessStatusCode)
        {
            Vehicle? v = await JsonSerializer.DeserializeAsync<Vehicle?>(await response.Content.ReadAsStreamAsync());
            //Vehicle? v = (await GetVehicleByName(vehicle.Name));
            return v;
        }
        return null;
    }
}
#endif
