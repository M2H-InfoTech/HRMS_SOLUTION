using ATTENDANCE.DTO;

namespace ATTENDANCE.Service.ShiftMasterUpload
{
    public interface IShiftMasterUploadService
    {
        Task<List<MasterShiftUploadDetailsDto>> GetMasterShiftUploadDetails();
    }
}
