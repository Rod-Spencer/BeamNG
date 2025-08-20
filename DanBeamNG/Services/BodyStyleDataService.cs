using NLog;
using Segway.Service.LoggerHelper;
using SpenSoft.BeamNG.VehicleObjects;
using SpenSoft.EF.BeamNG;

namespace SpenSoft.DanBeamNG.Services
{
    public class BodyStyleDataService : BodyStyleDataService_Interface
    {
        private static readonly Logger logger = Logger_Helper.GetCurrentLogger();
        static BeamNGContext? bngDB = null;

        public BodyStyleDataService(BeamNGContext _bngDB)
        {
            bngDB = _bngDB;
        }

        public async Task<List<BodyStyles>?> GetAllBodyStyle()
        {
            try
            {
                List<BodyStyles>? viList = null;
                logger.Info($"Entered {nameof(GetAllBodyStyle)}");

                viList = bngDB?.Body_Style?.ToList();
                return viList;
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"Exception: {ex.Message}");
                throw;
            }
            finally
            {
                logger.Info($"Leaving {nameof(GetAllBodyStyle)}");
            }
        }

        public async Task<BodyStyles?> GetBodyStyleById(Guid? bodyStyleID)
        {
            try
            {
                logger.Info($"Entered {nameof(GetBodyStyleById)}");
                var vi = bngDB?.Body_Style?.FirstOrDefault(x => x.ID == bodyStyleID);
                return vi;
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"Exception: {ex.Message}");
                throw;
            }
            finally
            {
                logger.Info($"Leaving {nameof(GetBodyStyleById)}");
            }
        }

        public async Task<BodyStyles?> GetBodyStyleByName(String? bodyStyleName)
        {
            try
            {
                logger.Info($"Entered {nameof(GetBodyStyleByName)}");
                var vi = bngDB?.Body_Style?.FirstOrDefault(x => x.Name == bodyStyleName);
                return vi;
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"Exception: {ex.Message}");
                throw;
            }
            finally
            {
                logger.Info($"Leaving {nameof(GetBodyStyleByName)}");
            }
        }

        public async Task<BodyStyles?> AddBodyStyle(BodyStyles? bodyStyle)
        {
            try
            {
                logger.Info($"Entered {nameof(AddBodyStyle)}");
                if (bodyStyle == null) { return null; }
                bngDB?.Body_Style?.Add(bodyStyle);
                bngDB?.SaveChanges();
                logger.Debug($"Vehicle ID: {bodyStyle.ID}");
                return bodyStyle;
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"Exception: {ex.Message}");
                throw;
            }
            finally
            {
                logger.Info($"Leaving {nameof(AddBodyStyle)}");
            }
        }

        public async Task<Boolean> UpdateBodyStyle(BodyStyles? bodyStyle)
        {
            try
            {
                logger.Info($"Entered {nameof(UpdateBodyStyle)}");
                if (bodyStyle == null) { return false; }
                bngDB?.Body_Style?.Update(bodyStyle);
                bngDB?.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"Exception: {ex.Message}");
                throw;
            }
            finally
            {
                logger.Info($"Leaving {nameof(UpdateBodyStyle)}");
            }
        }

        public async Task<Boolean> DeleteBodyStyle(Guid? bodyStyleID)
        {
            try
            {
                logger.Info($"Entered {nameof(DeleteBodyStyle)}");
                if ((bodyStyleID == null) || (bodyStyleID == Guid.Empty)) { return false; }
                var bodyStyle = bngDB?.Body_Style?.FirstOrDefault(x => x.ID == bodyStyleID);
                if (bodyStyle == null) { return false; }
                bngDB?.Body_Style?.Remove(bodyStyle);
                bngDB?.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"Exception: {ex.Message}");
                throw;
            }
            finally
            {
                logger.Info($"Leaving {nameof(DeleteBodyStyle)}");
            }
        }
    }

}
