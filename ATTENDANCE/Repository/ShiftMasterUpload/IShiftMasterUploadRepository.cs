using ATTENDANCE.DTO;

namespace ATTENDANCE.Repository.ShiftMasterUpload
{
    public interface IShiftMasterUploadRepository
    {
        Task<List<MasterShiftUploadDetailsDto>> GetMasterShiftUploadDetails();
    }
}
