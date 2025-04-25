using OFFICEKITCORELEAVE.OfficeKit.Leave.DTO;
using OFFICEKITCORELEAVE.OfficeKitHR.Leave.DTO;

namespace OFFICEKITCORELEAVE.OfficeKitHR.Leave.Services
{
    public interface ILeaveService
    {
        Task<int> SaveLeaveBasicSettingsDetails(HrmLeaveBasicsettingsDetailDto LeaveBasicSettingsDetails);
        Task<int> SaveLeaveBasicSetting(HrmLeaveBasicSettingDto dto);
        Task<HrmLeaveBasicSettingDto> GetLeaveBasicSettingById(int leaveSettingId);
        Task<bool> DeleteLeaveBasicSetting(int leaveSettingId);
        Task<List<HrmLeaveBasicSettingDto>> GetAllLeaveBasicSettings();
        Task<int> SaveLeaveMaster(HrmLeaveMasterDTO dto);
        Task<bool> DeleteLeaveMaster(int leaveMasterId);
        Task<List<HrmLeaveMasterViewDto>> GetAllLeaveMasters(HrmLeaveMasterSearchDto sortDto);
        Task<HrmLeaveMasterDTO> GetLeaveMasterById(int leaveMasterId);
        Task<bool> Checkexistance(string leaveCode);
        Task<int> EntityApplicableSave(SaveApplicableParameters dto);
    }
}
