using NLog;
using Segway.Service.LoggerHelper;
using SpenSoft.BeamNG.VehicleObjects;
using SpenSoft.EF.BeamNG;

namespace SpenSoft.DanBeamNG.Services;

public class DriveTrainDataService : DriveTrainDataService_Interface
{
    private static readonly Logger logger = Logger_Helper.GetCurrentLogger();
    static BeamNGContext? bngDB = null;

    public DriveTrainDataService(BeamNGContext _bngDB)
    {
        bngDB = _bngDB;
    }

    public async Task<List<DriveTrain>?> GetAllDriveTrain()
    {
        try
        {
            List<DriveTrain>? dtList = null;
            logger.Info($"Entered {nameof(GetAllDriveTrain)}", nameof(GetAllDriveTrain));

            dtList = bngDB?.Drive_Train?.ToList();
            return dtList;
        }
        catch (Exception ex)
        {
            logger.Error($"Exception: {ex.Message}", nameof(GetAllDriveTrain));
            throw;
        }
        finally
        {
            logger.Info($"Leaving {nameof(GetAllDriveTrain)}", nameof(GetAllDriveTrain));
        }

    }

    public async Task<DriveTrain?> GetDriveTrainByID(Guid? driveTrainID)
    {
        try
        {
            logger.Info($"Entered {nameof(GetDriveTrainByID)}", nameof(GetDriveTrainByID));
            var dt = bngDB?.Drive_Train?.FirstOrDefault<DriveTrain>(x => x.ID == driveTrainID);
            return dt;
        }
        catch (Exception ex)
        {
            logger.Error($"Exception: {ex.Message}", nameof(GetDriveTrainByID));
            throw;
        }
        finally
        {
            logger.Info($"Leaving {nameof(GetDriveTrainByID)}", nameof(GetDriveTrainByID));
        }
    }

    public async Task<DriveTrain?> GetDriveTrainByName(String? driveTrainName)
    {
        try
        {
            logger.Info($"Entered {nameof(GetDriveTrainByName)}", nameof(GetDriveTrainByName));
            var dt = bngDB?.Drive_Train?.FirstOrDefault<DriveTrain>(x => x.Name == driveTrainName);
            return dt;
        }
        catch (Exception ex)
        {
            logger.Error($"Exception: {ex.Message}", nameof(GetDriveTrainByName));
            throw;
        }
        finally
        {
            logger.Info($"Leaving {nameof(GetDriveTrainByName)}", nameof(GetDriveTrainByName));
        }
    }

    public async Task<DriveTrain?> AddDriveTrain(DriveTrain? driveTrain)
    {
        try
        {
            logger.Info($"Entered {nameof(AddDriveTrain)}", nameof(AddDriveTrain));
            if (driveTrain == null) { return null; }
            bngDB?.Drive_Train?.Add(driveTrain);
            bngDB?.SaveChanges();
            logger.Debug($"Vehicle ID: {driveTrain.ID}", nameof(AddDriveTrain));
            return driveTrain;
        }
        catch (Exception ex)
        {
            logger.Error($"Exception: {ex.Message}", nameof(AddDriveTrain));
            throw;
        }
        finally
        {
            logger.Info($"Leaving {nameof(AddDriveTrain)}", nameof(AddDriveTrain));
        }
    }

    public async Task<Boolean> UpdateDriveTrain(DriveTrain? driveTrain)
    {
        try
        {
            logger.Info($"Entered {nameof(UpdateDriveTrain)}", nameof(UpdateDriveTrain));
            if (driveTrain == null) { return false; }
            bngDB?.Drive_Train?.Update(driveTrain);
            bngDB?.SaveChanges();
            return true;
        }
        catch (Exception ex)
        {
            logger.Error($"Exception: {ex.Message}", nameof(UpdateDriveTrain));
            throw;
        }
        finally
        {
            logger.Info($"Leaving {nameof(UpdateDriveTrain)}", nameof(UpdateDriveTrain));
        }
    }

    public async Task<Boolean> DeleteDriveTrain(Guid? driveTrainID)
    {
        try
        {
            logger.Info($"Entered {nameof(UpdateDriveTrain)}", nameof(UpdateDriveTrain));
            if ((driveTrainID == null) || (driveTrainID == Guid.Empty)) { return false; }
            var driveTrain = bngDB?.Drive_Train?.FirstOrDefault(x => x.ID == driveTrainID);
            if (driveTrain == null) { return false; }
            bngDB?.Drive_Train?.Remove(driveTrain);
            bngDB?.SaveChanges();
            return true;
        }
        catch (Exception ex)
        {
            logger.Error($"Exception: {ex.Message}", nameof(UpdateDriveTrain));
            throw;
        }
        finally
        {
            logger.Info($"Leaving {nameof(UpdateDriveTrain)}", nameof(UpdateDriveTrain));
        }
    }
}



#if false
public class DriveTrainDataService : DriveTrainDataService_Interface
{
    private static readonly Logger logger = Logger_Helper.GetCurrentLogger();
    static BeamNGContext? bngDB = null;

    public DriveTrainDataService(BeamNGContext _bngDB)
    {
        bngDB = _bngDB;
        logger = cl;
    }

    public async Task<List<DriveTrain>> GetAllDriveTrain()
    {
        return await JsonSerializer.DeserializeAsync<List<DriveTrain>>(await _httpClient.GetStreamAsync("DriveTrain/All"),
            new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
    }

    public async Task<DriveTrain> GetDriveTrainById(Guid? driveTrainID)
    {
        return await JsonSerializer.DeserializeAsync<DriveTrain>(await _httpClient.GetStreamAsync($"DriveTrain/ID/{driveTrainID}"),
            new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
    }

    public async Task<DriveTrain> GetDriveTrainByName(String? driveTrainName)
    {
        return await JsonSerializer.DeserializeAsync<DriveTrain>(await _httpClient.GetStreamAsync($"DriveTrain/Name/{driveTrainName}"),
            new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
    }

    public async Task<DriveTrain?> AddDriveTrain(DriveTrain? driveTrain)
    {
        try
        {
            await logger.AddLoggingMessage($"Entered {nameof(AddDriveTrain)}");
            if (driveTrain == null) { return null; }
            bngDB?.Drive_Train?.Add(driveTrain);
            await logger.AddLoggingMessage($"Drive Train ID: {driveTrain.ID}");
            return driveTrain;
        }
        catch (Exception ex)
        {
            await logger.AddLoggingMessage($"Exception: {ex.Message}");
            throw;
        }
        finally
        {
            await logger.AddLoggingMessage($"Leaving {nameof(AddDriveTrain)}");
        }
    }

    public async Task UpdateDriveTrain(DriveTrain driveTrain)
    {
        var driveTrainJson = new StringContent(JsonSerializer.Serialize(driveTrain), Encoding.UTF8, "application/json");

        await _httpClient.PutAsync($"DriveTrain/Update", driveTrainJson);
    }

    public async Task<Boolean> DeleteDriveTrain(Guid? driveTrainID)
    {
        return (await _httpClient.DeleteAsync($"DriveTrain/Delete/{driveTrainID}")).IsSuccessStatusCode;
    }
}
#endif
