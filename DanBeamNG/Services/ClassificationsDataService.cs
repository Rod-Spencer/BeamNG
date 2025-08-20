using NLog;
using Segway.Service.LoggerHelper;
using SpenSoft.BeamNG.VehicleObjects;
using SpenSoft.EF.BeamNG;

namespace SpenSoft.DanBeamNG.Services;

public class ClassificationsDataService : ClassificationsDataService_Interface
{
    private static readonly Logger logger = Logger_Helper.GetCurrentLogger();
    static BeamNGContext? bngDB = null;

    public ClassificationsDataService(BeamNGContext _bngDB)
    {
        bngDB = _bngDB;
    }

    public async Task<List<Classifications>?> GetAllClassifications()
    {
        try
        {
            List<Classifications>? clsList = null;
            logger.Info($"Entered {nameof(GetAllClassifications)}");

            clsList = bngDB?.Classification?.ToList();
            return clsList;
        }
        catch (Exception ex)
        {
            logger.Info($"Exception: {ex.Message}");
            throw;
        }
        finally
        {
            logger.Info($"Leaving {nameof(GetAllClassifications)}");
            await Task.CompletedTask;
        }

    }

    public async Task<Classifications?> GetClassificationsById(Guid? classificationID)
    {
        try
        {
            logger.Info($"Entered {nameof(GetClassificationsById)}", nameof(GetClassificationsById));
            var cls = bngDB?.Classification?.FirstOrDefault<Classifications>(x => x.ID == classificationID);
            return cls;
        }
        catch (Exception ex)
        {
            logger.Error($"Exception: {ex.Message}", nameof(GetClassificationsById));
            throw;
        }
        finally
        {
            logger.Info($"Leaving {nameof(GetClassificationsById)}", nameof(GetClassificationsById));
        }
    }

    public async Task<Classifications?> GetClassificationsByName(String? classificationName)
    {
        try
        {
            logger.Info($"Entered {nameof(GetClassificationsByName)}", nameof(GetClassificationsByName));
            var cls = bngDB?.Classification?.FirstOrDefault<Classifications>(x => x.Name == classificationName);
            return cls;
        }
        catch (Exception ex)
        {
            logger.Error($"Exception: {ex.Message}", nameof(GetClassificationsByName));
            throw;
        }
        finally
        {
            logger.Info($"Leaving {nameof(GetClassificationsByName)}", nameof(GetClassificationsByName));
        }
    }

    public async Task<Classifications?> AddClassifications(Classifications? classification)
    {
        try
        {
            logger.Info($"Entered {nameof(AddClassifications)}", nameof(AddClassifications));
            if (classification == null) { return null; }
            bngDB?.Classification?.Add(classification);
            bngDB?.SaveChanges();
            logger.Debug($"Vehicle ID: {classification.ID}");
            return classification;
        }
        catch (Exception ex)
        {
            logger.Error($"Exception: {ex.Message}", nameof(AddClassifications));
            throw;
        }
        finally
        {
            logger.Info($"Leaving {nameof(AddClassifications)}", nameof(AddClassifications));
        }
    }

    public async Task<Boolean> UpdateClassifications(Classifications? classification)
    {
        try
        {
            logger.Info($"Entered {nameof(UpdateClassifications)}", nameof(UpdateClassifications));
            if (classification == null) { return false; }
            bngDB?.Classification?.Update(classification);
            bngDB?.SaveChanges();
            return true;
        }
        catch (Exception ex)
        {
            logger.Error($"Exception: {ex.Message}", nameof(UpdateClassifications));
            throw;
        }
        finally
        {
            logger.Info($"Leaving {nameof(UpdateClassifications)}", nameof(UpdateClassifications));
        }
    }

    public async Task<Boolean> DeleteClassifications(Guid? classificationID)
    {
        try
        {
            logger.Info($"Entered {nameof(UpdateClassifications)}", nameof(UpdateClassifications));
            if ((classificationID == null) || (classificationID == Guid.Empty)) { return false; }
            var classification = bngDB?.Classification?.FirstOrDefault(x => x.ID == classificationID);
            if (classification == null) { return false; }
            bngDB?.Classification?.Remove(classification);
            bngDB?.SaveChanges();
            return true;
        }
        catch (Exception ex)
        {
            logger.Error($"Exception: {ex.Message}", nameof(UpdateClassifications));
            throw;
        }
        finally
        {
            logger.Info($"Leaving {nameof(UpdateClassifications)}", nameof(UpdateClassifications));
        }
    }
}
