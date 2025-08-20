using NLog;
using Segway.Service.LoggerHelper;
using SpenSoft.BeamNG.VehicleObjects;
using SpenSoft.EF.BeamNG;

namespace SpenSoft.DanBeamNG.Services;


public class CountriesDataService : CountriesDataService_Interface
{
    private static readonly Logger logger = Logger_Helper.GetCurrentLogger();
    static BeamNGContext? bngDB = null;

    public CountriesDataService(BeamNGContext _bngDB)
    {
        bngDB = _bngDB;
    }

    public async Task<List<Countries>?> GetAllCountries()
    {
        try
        {
            List<Countries>? cntryList = null;
            logger.Info($"Entered {nameof(GetAllCountries)}", nameof(GetAllCountries));

            cntryList = bngDB?.Country?.ToList();
            return cntryList;
        }
        catch (Exception ex)
        {
            logger.Error($"Exception: {ex.Message}", nameof(GetAllCountries));
            throw;
        }
        finally
        {
            logger.Info($"Leaving {nameof(GetAllCountries)}", nameof(GetAllCountries));
        }

    }

    public async Task<Countries?> GetCountriesByID(Guid? countryID)
    {
        try
        {
            logger.Info($"Entered {nameof(GetCountriesByID)}", nameof(GetCountriesByID));
            var cntry = bngDB?.Country?.FirstOrDefault<Countries>(x => x.ID == countryID);
            return cntry;
        }
        catch (Exception ex)
        {
            logger.Error($"Exception: {ex.Message}", nameof(GetCountriesByID));
            throw;
        }
        finally
        {
            logger.Info($"Leaving {nameof(GetCountriesByID)}", nameof(GetCountriesByID));
        }
    }

    public async Task<Countries?> GetCountriesByName(String? countryName)
    {
        try
        {
            logger.Info($"Entered {nameof(GetCountriesByName)}", nameof(GetCountriesByName));
            var cntry = bngDB?.Country?.FirstOrDefault<Countries>(x => x.Name == countryName);
            return cntry;
        }
        catch (Exception ex)
        {
            logger.Error($"Exception: {ex.Message}", nameof(GetCountriesByName));
            throw;
        }
        finally
        {
            logger.Info($"Leaving {nameof(GetCountriesByName)}", nameof(GetCountriesByName));
        }
    }

    public async Task<Countries?> AddCountries(Countries? country)
    {
        try
        {
            logger.Info($"Entered {nameof(AddCountries)}", nameof(AddCountries));
            if (country == null) { return null; }
            bngDB?.Country?.Add(country);
            bngDB?.SaveChanges();
            logger.Debug($"Vehicle ID: {country.ID}", nameof(AddCountries));
            return country;
        }
        catch (Exception ex)
        {
            logger.Error($"Exception: {ex.Message}", nameof(AddCountries));
            throw;
        }
        finally
        {
            logger.Info($"Leaving {nameof(AddCountries)}", nameof(AddCountries));
        }
    }

    public async Task<Boolean> UpdateCountries(Countries? country)
    {
        try
        {
            logger.Info($"Entered {nameof(UpdateCountries)}", nameof(UpdateCountries));
            if (country == null) { return false; }
            bngDB?.Country?.Update(country);
            bngDB?.SaveChanges();
            return true;
        }
        catch (Exception ex)
        {
            logger.Error($"Exception: {ex.Message}", nameof(UpdateCountries));
            throw;
        }
        finally
        {
            logger.Info($"Leaving {nameof(UpdateCountries)}", nameof(UpdateCountries));
        }
    }

    public async Task<Boolean> DeleteCountries(Guid? countryID)
    {
        try
        {
            logger.Info($"Entered {nameof(UpdateCountries)}", nameof(UpdateCountries));
            if ((countryID == null) || (countryID == Guid.Empty)) { return false; }
            var country = bngDB?.Country?.FirstOrDefault(x => x.ID == countryID);
            if (country == null) { return false; }
            bngDB?.Country?.Remove(country);
            bngDB?.SaveChanges();
            return true;
        }
        catch (Exception ex)
        {
            logger.Error($"Exception: {ex.Message}", nameof(UpdateCountries));
            throw;
        }
        finally
        {
            logger.Info($"Leaving {nameof(UpdateCountries)}", nameof(UpdateCountries));
        }
    }
}



#if false
public class CountriesDataService : CountriesDataService_Interface
{
    private static readonly Logger logger = Logger_Helper.GetCurrentLogger();
    static BeamNGContext? bngDB = null;

    public CountriesDataService(BeamNGContext _bngDB)
    {
        bngDB = _bngDB;
        logger = cl;
    }

    public async Task<List<Countries>> GetAllCountries()
    {
        return await JsonSerializer.DeserializeAsync<List<Countries>>(await _httpClient.GetStreamAsync("Countries/All"),
            new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
    }

    public async Task<Countries> GetCountriesById(Guid? countryID)
    {
        return await JsonSerializer.DeserializeAsync<Countries>(await _httpClient.GetStreamAsync($"Countries/ID/{countryID}"),
            new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
    }

    public async Task<Countries> GetCountryByName(String? countryName)
    {
        return await JsonSerializer.DeserializeAsync<Countries>(await _httpClient.GetStreamAsync($"Countries/Name/{countryName}"),
            new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
    }

    public async Task<Countries?> AddCountries(Countries? country)
    {
        try
        {
            await logger.AddLoggingMessage($"Entered {nameof(AddCountries)}");
            if (country == null) { return null; }
            bngDB?.Country?.Add(country);
            await logger.AddLoggingMessage($"Vehicle ID: {country.ID}");
            return country;
        }
        catch (Exception ex)
        {
            await logger.AddLoggingMessage($"Exception: {ex.Message}");
            throw;
        }
        finally
        {
            await logger.AddLoggingMessage($"Entered {nameof(AddCountries)}");
        }
    }

    public async Task UpdateCountries(Countries country)
    {
        var countryJson = new StringContent(JsonSerializer.Serialize(country), Encoding.UTF8, "application/json");

        await _httpClient.PutAsync($"Countries/Update", countryJson);
    }

    public async Task<Boolean> DeleteCountries(Guid? countryID)
    {
        return (await _httpClient.DeleteAsync($"Countries/Delete/{countryID}")).IsSuccessStatusCode;
    }
}
#endif
