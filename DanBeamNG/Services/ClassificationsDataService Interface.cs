using SpenSoft.BeamNG.VehicleObjects;

namespace SpenSoft.DanBeamNG.Services;


//public interface ClassificationsDataService_Interface
//{
//    Task<Classifications?> AddClassifications(Classifications? classification);
//    Task<List<Classifications>> GetAllClassifications();
//    Task<Classifications> GetClassificationsById(Guid? classificationID);
//    Task<Classifications> GetClassificationsByName(String? className);
//    Task UpdateClassifications(Classifications classification);
//    Task<Boolean> DeleteClassifications(Guid? classificationID);
//}



public interface ClassificationsDataService_Interface
{
    Task<List<Classifications>?> GetAllClassifications();
    Task<Classifications?> GetClassificationsById(Guid? classificationID);
    Task<Classifications?> GetClassificationsByName(String? classificationID);
    Task<Classifications?> AddClassifications(Classifications? classification);
    Task<Boolean> UpdateClassifications(Classifications? classification);
    Task<Boolean> DeleteClassifications(Guid? classificationID);
}