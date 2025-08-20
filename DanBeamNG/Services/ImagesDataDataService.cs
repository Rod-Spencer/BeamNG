using NLog;
using Segway.Service.LoggerHelper;
using SpenSoft.BeamNG.VehicleObjects;
using SpenSoft.EF.BeamNG;

namespace SpenSoft.DanBeamNG.Services;

public class ImagesDataDataService : ImagesDataDataService_Interface
{
    private static readonly Logger logger = Logger_Helper.GetCurrentLogger();
    static BeamNGContext? bngDB = null;

    public ImagesDataDataService(BeamNGContext _bngDB)
    {
        bngDB = _bngDB;
    }

    public async Task<List<ImagesData>?> GetAllImagesData()
    {
        try
        {
            List<ImagesData>? idList = null;
            logger.Info($"Entered {nameof(GetAllImagesData)}", nameof(GetAllImagesData));

            idList = bngDB?.Images_Data?.ToList();
            return idList;
        }
        catch (Exception ex)
        {
            logger.Error($"Exception: {ex.Message}", nameof(GetAllImagesData));
            throw;
        }
        finally
        {
            logger.Info($"Leaving {nameof(GetAllImagesData)}", nameof(GetAllImagesData));
        }

    }

    public async Task<ImagesData?> GetImagesDataByID(Guid? imagesDataID)
    {
        try
        {
            logger.Info($"Entered {nameof(GetImagesDataByID)}", nameof(GetImagesDataByID));
            var id = bngDB?.Images_Data?.FirstOrDefault<ImagesData>(x => x.ImageID == imagesDataID);
            return id;
        }
        catch (Exception ex)
        {
            logger.Error($"Exception: {ex.Message}", nameof(GetImagesDataByID));
            throw;
        }
        finally
        {
            logger.Info($"Leaving {nameof(GetImagesDataByID)}", nameof(GetImagesDataByID));
        }
    }

    public async Task<ImagesData?> AddImagesData(ImagesData? imagesData)
    {
        try
        {
            logger.Info($"Entered {nameof(AddImagesData)}", nameof(AddImagesData));
            if (imagesData == null) { return null; }
            bngDB?.Images_Data?.Add(imagesData);
            bngDB?.SaveChanges();
            logger.Debug($"Vehicle ID: {imagesData.ImageID}", nameof(AddImagesData));
            return imagesData;
        }
        catch (Exception ex)
        {
            logger.Error($"Exception: {ex.Message}", nameof(AddImagesData));
            throw;
        }
        finally
        {
            logger.Info($"Leaving {nameof(AddImagesData)}", nameof(AddImagesData));
        }
    }

    public async Task<Boolean> UpdateImagesData(ImagesData? imagesData)
    {
        try
        {
            logger.Info($"Entered {nameof(UpdateImagesData)}", nameof(UpdateImagesData));
            if (imagesData == null) { return false; }
            bngDB?.Images_Data?.Update(imagesData);
            bngDB?.SaveChanges();
            return true;
        }
        catch (Exception ex)
        {
            logger.Error($"Exception: {ex.Message}", nameof(UpdateImagesData));
            throw;
        }
        finally
        {
            logger.Info($"Leaving {nameof(UpdateImagesData)}", nameof(UpdateImagesData));
        }
    }

    public async Task<Boolean> DeleteImagesData(Guid? imagesDataID)
    {
        try
        {
            logger.Info($"Entered {nameof(UpdateImagesData)}", nameof(UpdateImagesData));
            if ((imagesDataID == null) || (imagesDataID == Guid.Empty)) { return false; }
            var imagesData = bngDB?.Images_Data?.FirstOrDefault(x => x.ImageID == imagesDataID);
            if (imagesData == null) { return false; }
            bngDB?.Images_Data?.Remove(imagesData);
            bngDB?.SaveChanges();
            return true;
        }
        catch (Exception ex)
        {
            logger.Error($"Exception: {ex.Message}", nameof(UpdateImagesData));
            throw;
        }
        finally
        {
            logger.Info($"Leaving {nameof(UpdateImagesData)}", nameof(UpdateImagesData));
        }
    }
}


#if false

public class ImagesDataDataService : ImagesDataDataService_Interface
{
    private static readonly Logger logger = Logger_Helper.GetCurrentLogger();
    static BeamNGContext? bngDB = null;

    public ImagesDataDataService(BeamNGContext _bngDB)
    {
        bngDB = _bngDB;
        logger = cl;
    }

    public async Task<List<ImagesData>> GetAllImagesData()
    {
        return await JsonSerializer.DeserializeAsync<List<ImagesData>>(await _httpClient.GetStreamAsync("ImagesData/All"),
            new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
    }

    public async Task<ImagesData?> GetImagesDataById(Guid? imageID)
    {
        ImagesData? image = await JsonSerializer.DeserializeAsync<ImagesData>(await _httpClient.GetStreamAsync($"ImagesData/ID/{imageID}"),
            new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        return image;
    }

    public async Task<ImagesData?> AddImagesData(ImagesData? image)
    {
        try
        {
            await logger.AddLoggingMessage($"Entered {nameof(AddImagesData)}");
            if (image == null) { return null; }
            bngDB?.Images_Data?.Add(image);
            await logger.AddLoggingMessage($"Image ID: {image.ImageID}");
            return image;
        }
        catch (Exception ex)
        {
            await logger.AddLoggingMessage($"Exception: {ex.Message}");
            throw;
        }
        finally
        {
            await logger.AddLoggingMessage($"Leaving {nameof(AddImagesData)}");
        }
    }

    public async Task UpdateImagesData(ImagesData image)
    {
        var imageJson = new StringContent(JsonSerializer.Serialize(image), Encoding.UTF8, "application/json");

        await _httpClient.PutAsync($"ImagesData/Update", imageJson);
    }

    public async Task<Boolean> DeleteImagesData(int imageID)
    {
        return (await _httpClient.DeleteAsync($"ImagesData/Delete/{imageID}")).IsSuccessStatusCode;
    }

    public async Task<List<ImagesData>> GetImagesDataByVehicleId(int vehicleID)
    {
        return await JsonSerializer.DeserializeAsync<List<ImagesData>>(await _httpClient.GetStreamAsync($"ImagesData/vehicle/{vehicleID}"),
            new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
    }
}
#endif
