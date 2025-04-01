using OFFICEKITCORELEAVE.OfficeKit.Leave.DTO;
using OFFICEKITCORELEAVE.OfficeKit.Leave.MODELS;

namespace OFFICEKITCORELEAVE.OfficeKitHR.Leave.Interface.LeaveInterfaces
{
    public interface ILeaveMasterService
    {
        Task<int> SaveLeaveMaster (HrmLeaveMasterDTO dto);
        Task<bool> DeleteLeaveMaster (int leaveMasterId);
        Task<List<HrmLeaveMasterViewDto>> GetAllLeaveMasters (HrmLeaveMasterSearchDto sortDto);
        Task<HrmLeaveMasterDTO> GetLeaveMasterById (int leaveMasterId);
        Task<bool> Checkexistance (string leaveCode);

    }
}
