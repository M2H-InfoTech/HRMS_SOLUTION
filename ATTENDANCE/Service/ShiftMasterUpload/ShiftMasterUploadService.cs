using ATTENDANCE.DTO;
using ATTENDANCE.Repository.ShiftMasterUpload;

namespace ATTENDANCE.Service.ShiftMasterUpload
{
    public class ShiftMasterUploadService(IShiftMasterUploadRepository _shiftmasterrepository) : IShiftMasterUploadService
    {
        public async Task<List<MasterShiftUploadDetailsDto>> GetMasterShiftUploadDetails()
        {
            return await _shiftmasterrepository.GetMasterShiftUploadDetails( );
        }
    }
}
