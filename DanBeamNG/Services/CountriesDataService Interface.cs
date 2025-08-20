using SpenSoft.BeamNG.VehicleObjects;

namespace SpenSoft.DanBeamNG.Services;


public interface CountriesDataService_Interface
{
    Task<List<Countries>?> GetAllCountries();
    Task<Countries?> GetCountriesByID(Guid? countryID);
    Task<Countries?> GetCountriesByName(String? countryID);
    Task<Countries?> AddCountries(Countries? country);
    Task<Boolean> UpdateCountries(Countries? country);
    Task<Boolean> DeleteCountries(Guid? countryID);
}
