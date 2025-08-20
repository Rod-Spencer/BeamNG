using Microsoft.EntityFrameworkCore;
using NLog;
using Segway.Service.LoggerHelper;
using SpenSoft.BeamNG.VehicleObjects;
using SpenSoft.EF.BeamNG;

namespace SpenSoft.DanBeamNG.Services
{
    public class VehicleImageDataService : VehicleImageDataService_Interface
    {
        private static readonly Logger logger = Logger_Helper.GetCurrentLogger();
        static BeamNGContext? bngDB = null;

        public VehicleImageDataService(BeamNGContext _bngDB)
        {
            bngDB = _bngDB;
        }

        public async Task<List<VehicleImage>?> GetAllVehicleImage()
        {
            try
            {
                List<VehicleImage>? viList = null;
                if (logger != null)
                {
                    logger.Info($"Entered {nameof(GetAllVehicleImage)}", nameof(GetAllVehicleImage));
                }

                viList = bngDB?.Images?.ToList();
                return viList;
            }
            catch (Exception ex)
            {
                if (logger != null) logger.Error($"Exception: {ex.Message}", nameof(GetAllVehicleImage));
                throw;
            }
            finally
            {
                if (logger != null) logger.Info($"Leaving {nameof(GetAllVehicleImage)}", nameof(GetAllVehicleImage));
            }

        }


        public async Task<List<VehicleImage>?> GetUnassignedImages()
        {
            logger.Info($"Entered {nameof(GetUnassignedImages)}", nameof(GetUnassignedImages));
            var viList = bngDB?.Database.SqlQuery<VehicleImage>($"EXEC [dbo].[GetUnassignedImages]").ToList();
            await Task.Yield();
            return viList;
        }

        public async Task<List<VehicleImage>?> GetAllImagesByVehicle(int vid)
        {
            var viList = bngDB?.Database.SqlQuery<VehicleImage>($"EXEC [dbo].[GetImagesByVehicle] @vehicleID={vid}").ToList();
            await Task.Yield();
            return viList;
        }

        public async Task<List<VehicleImage>?> GetAllVehicleDefaultImage()
        {
            var viList = bngDB?.Database.SqlQuery<VehicleImage>($"EXEC [dbo].[GetAllVehicleDefaultImage]").ToList();
            await Task.Yield();
            return viList;
        }
        public async Task<VehicleImage?> GetVehicleImageById(Guid? imageID)
        {
            var vi = bngDB?.Images?.FirstOrDefault<VehicleImage>(x => x.ImageID == imageID);
            await Task.Yield();
            return vi;
        }

        public async Task<VehicleImage?> GetVehicleImageByName(String? vehicleName, String? configImageName)
        {
            var vi = (from i in bngDB?.Images
                      join c in bngDB?.Configurations on i.ImageID equals c.ImageID
                      join v in bngDB?.Vehicles on c.VehicleID equals v.ID
                      where v.Name == vehicleName && c.Name == configImageName
                      select i).FirstOrDefault();

            await Task.Yield();
            return vi;
        }


        public async Task<VehicleImage?> AddVehicleImage(VehicleImage? image)
        {
            try
            {
                logger.Info($"Entered {nameof(AddVehicleImage)}", nameof(AddVehicleImage));
                if (image == null) return null;
                bngDB?.Images?.Add(image);
                bngDB?.SaveChanges();
                await Task.Yield();
                return image;
            }
            catch (Exception ex)
            {
                logger.Error($"Exception: {ex.Message}", nameof(AddVehicleImage));
                throw;
            }
            finally
            {
                logger.Info($"Leaving {nameof(AddVehicleImage)}", nameof(AddVehicleImage));
            }
        }

        public async Task UpdateVehicleImage(VehicleImage? image)
        {
            try
            {
                logger.Info($"Entered {nameof(UpdateVehicleImage)}");
                if (image == null) return;
                bngDB?.Images?.Update(image);
                bngDB?.SaveChanges();
            }
            catch (Exception ex)
            {
                logger.Error($"Exception: {ex.Message}", nameof(UpdateVehicleImage));
                throw;
            }
            finally
            {
                logger.Info($"Leaving {nameof(UpdateVehicleImage)}", nameof(UpdateVehicleImage));
            }
        }

        public async Task<Boolean> DeleteVehicleImage(Guid? imageID)
        {
            var image = bngDB?.Images?.FirstOrDefault<VehicleImage>(x => x.ImageID == imageID);
            if (image == null) return false;
            bngDB?.Images?.Remove(image);
            bngDB?.SaveChanges();
            await Task.Yield();
            return true;
        }

        public async Task<VehicleImage?> GetVehicleImageByName(string? imageName)
        {
            var vi = bngDB?.Images?.FirstOrDefault<VehicleImage>(x => x.ImageName == imageName);
            await Task.Yield();
            return vi;

        }

    }

}
