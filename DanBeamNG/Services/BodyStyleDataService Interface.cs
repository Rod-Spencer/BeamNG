using SpenSoft.BeamNG.VehicleObjects;

namespace SpenSoft.DanBeamNG.Services
{
    public interface BodyStyleDataService_Interface
    {
        Task<List<BodyStyles>?> GetAllBodyStyle();
        Task<BodyStyles?> GetBodyStyleById(Guid? bodyStyleID);
        Task<BodyStyles?> GetBodyStyleByName(String? bodyStyleName);
        Task<BodyStyles?> AddBodyStyle(BodyStyles? bodyStyle);
        Task<Boolean> UpdateBodyStyle(BodyStyles? bodyStyle);
        Task<Boolean> DeleteBodyStyle(Guid? bodyStyleID);
    }
}
