using NLog;
using Segway.Service.LoggerHelper;
using SpenSoft.BeamNG.VehicleObjects;
using SpenSoft.EF.BeamNG;

namespace SpenSoft.DanBeamNG.Services;

public class ConfigurationDataService : ConfigurationDataService_Interface
{
    private static readonly Logger logger = Logger_Helper.GetCurrentLogger();
    static BeamNGContext? bngDB = null;

    public ConfigurationDataService(BeamNGContext _bngDB)
    {
        bngDB = _bngDB;
    }

    public async Task<List<VConfiguration>?> GetAllConfiguration()
    {
        try
        {
            List<VConfiguration>? cnfList = null;
            logger.Info($"Entered {nameof(GetAllConfiguration)}", nameof(GetAllConfiguration));

            cnfList = bngDB?.Configurations?.ToList();
            return cnfList;
        }
        catch (Exception ex)
        {
            logger.Error($"Exception: {ex.Message}", nameof(GetAllConfiguration));
            throw;
        }
        finally
        {
            logger.Info($"Leaving {nameof(GetAllConfiguration)}", nameof(GetAllConfiguration));
        }

    }

    public async Task<VConfiguration?> GetConfigurationByID(int? configID)
    {
        try
        {
            logger.Info($"Entered {nameof(GetConfigurationByID)}", nameof(GetConfigurationByID));
            var cnf = bngDB?.Configurations?.FirstOrDefault<VConfiguration>(x => x.ID == configID);
            return cnf;
        }
        catch (Exception ex)
        {
            logger.Error($"Exception: {ex.Message}", nameof(GetConfigurationByID));
            throw;
        }
        finally
        {
            logger.Info($"Leaving {nameof(GetConfigurationByID)}", nameof(GetConfigurationByID));
        }
    }

    public async Task<VConfiguration?> GetConfigurationByName(String? configName)
    {
        try
        {
            logger.Info($"Entered {nameof(GetConfigurationByName)}", nameof(GetConfigurationByName));
            var cnf = bngDB?.Configurations?.FirstOrDefault<VConfiguration>(x => x.Name == configName);
            return cnf;
        }
        catch (Exception ex)
        {
            logger.Error($"Exception: {ex.Message}", nameof(GetConfigurationByName));
            throw;
        }
        finally
        {
            logger.Info($"Leaving {nameof(GetConfigurationByName)}", nameof(GetConfigurationByName));
        }
    }

    public async Task<VConfiguration?> AddConfiguration(VConfiguration? config)
    {
        try
        {
            logger.Info($"Entered {nameof(AddConfiguration)}", nameof(AddConfiguration));
            if (config == null) { return null; }
            bngDB?.Configurations?.Add(config);
            bngDB?.SaveChanges();
            logger.Debug($"Vehicle ID: {config.ID}", nameof(AddConfiguration));
            return config;
        }
        catch (Exception ex)
        {
            logger.Error($"Exception: {ex.Message}", nameof(AddConfiguration));
            throw;
        }
        finally
        {
            logger.Info($"Leaving {nameof(AddConfiguration)}", nameof(AddConfiguration));
        }
    }

    public async Task<Boolean> UpdateConfiguration(VConfiguration? config)
    {
        try
        {
            logger.Info($"Entered {nameof(UpdateConfiguration)}", nameof(UpdateConfiguration));
            if (config == null) { return false; }
            bngDB?.Configurations?.Update(config);
            bngDB?.SaveChanges();
            return true;
        }
        catch (Exception ex)
        {
            logger.Error($"Exception: {ex.Message}", nameof(UpdateConfiguration));
            throw;
        }
        finally
        {
            logger.Info($"Leaving {nameof(UpdateConfiguration)}", nameof(UpdateConfiguration));
        }
    }

    public async Task<Boolean> DeleteConfiguration(int? configID)
    {
        try
        {
            logger.Info($"Entered {nameof(UpdateConfiguration)}", nameof(UpdateConfiguration));
            if (configID == null) { return false; }
            var config = bngDB?.Configurations?.FirstOrDefault(x => x.ID == configID);
            if (config == null) { return false; }
            bngDB?.Configurations?.Remove(config);
            bngDB?.SaveChanges();
            return true;
        }
        catch (Exception ex)
        {
            logger.Error($"Exception: {ex.Message}", nameof(UpdateConfiguration));
            throw;
        }
        finally
        {
            logger.Info($"Leaving {nameof(UpdateConfiguration)}", nameof(UpdateConfiguration));
        }
    }
}


#if false
public class ConfigurationDataService : ConfigurationDataService_Interface
{
    private static JsonSerializerOptions jsOpt = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
    private static readonly Logger logger = Logger_Helper.GetCurrentLogger();
    static BeamNGContext? bngDB = null;

    public ConfigurationDataService(BeamNGContext _bngDB)
    {
        bngDB = _bngDB;
        logger = cl;
    }


    public async Task<VConfiguration?>? AddConfiguration(VConfiguration? config)
    {
        try
        {
            await logger.AddLoggingMessage($"Entered {nameof(AddConfiguration)}");
            if (config == null) { return null; }
            bngDB?.Configurations?.Add(config);
            await logger.AddLoggingMessage($"Vehicle ID: {config.ID}");
            return config;
        }
        catch (Exception ex)
        {
            await logger.AddLoggingMessage($"Exception: {ex.Message}");
            throw;
        }
        finally
        {
            await logger.AddLoggingMessage($"Entered {nameof(AddConfiguration)}");
        }
    }

    public async Task<Boolean> DeleteConfiguration(int id)
    {
        return (await _httpClient.DeleteAsync($"Config/Delete/{id}")).IsSuccessStatusCode;
    }

    public async Task<List<VConfiguration?>> GetAllConfigurations()
    {
        return await JsonSerializer.DeserializeAsync<List<VConfiguration?>>(await _httpClient?.GetStreamAsync($"Config/All"), jsOpt);
    }

    public async Task<List<VConfiguration?>> GetAllVehicleConfigurations(int vehicleID)
    {
        return await JsonSerializer.DeserializeAsync<List<VConfiguration?>>(await _httpClient.GetStreamAsync($"Config/vehicle/{vehicleID}"), jsOpt);
    }

    public async Task<VConfiguration?> GetConfigurationById(int id)
    {
        return await JsonSerializer.DeserializeAsync<VConfiguration?>(await _httpClient.GetStreamAsync($"Config/ID/{id}"), jsOpt);
    }

    public async Task<List<VConfiguration?>> GetConfigurationByClass(String? className)
    {
        var resp = await _httpClient.GetStreamAsync($"Config/Class/{className}");
        if (resp != null) return await JsonSerializer.DeserializeAsync<List<VConfiguration?>>((Stream)resp, jsOpt);
        return null;
    }

    public async Task<Boolean> UpdateConfiguration(VConfiguration? config)
    {
        try
        {
            if (config != null)
            {
                String configJson = Newtonsoft.Json.JsonConvert.SerializeObject(config); // JsonSerializer.Serialize(config);
                var configurationJson = new StringContent(configJson, Encoding.UTF8, "application/json");

                var resp = await _httpClient.PutAsync($"Config/Update", configurationJson);
                if (resp != null) return JsonSerializer.Deserialize<Boolean>(resp.Content.ReadAsStream(), jsOpt);
            }
            return false;
        }
        catch
        {
            return false;
        }
    }
}
#endif
