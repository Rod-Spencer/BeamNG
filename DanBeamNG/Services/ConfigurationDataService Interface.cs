using Microsoft.Extensions.Configuration;
using SpenSoft.BeamNG.VehicleObjects;

namespace SpenSoft.DanBeamNG.Services;


public interface ConfigurationDataService_Interface
{
    Task<List<VConfiguration>?> GetAllConfiguration();
    Task<VConfiguration?> GetConfigurationByID(int? parameterID);
    Task<VConfiguration?> GetConfigurationByName(String? parameterID);
    Task<VConfiguration?> AddConfiguration(VConfiguration? parameter);
    Task<Boolean> UpdateConfiguration(VConfiguration? parameter);
    Task<Boolean> DeleteConfiguration(int? parameterID);
}