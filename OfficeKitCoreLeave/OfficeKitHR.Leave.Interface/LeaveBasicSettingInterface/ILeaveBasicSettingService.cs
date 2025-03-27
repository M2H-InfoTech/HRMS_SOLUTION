using OFFICEKITCORELEAVE.OfficeKitHR.Leave.DTO;

namespace OFFICEKITCORELEAVE.OfficeKitHR.Leave.Interface.HrmLeaveBasicSettingInterface
    {
    public interface ILeaveBasicSettingService
        {
        Task<int> SaveLeaveBasicSetting (HrmLeaveBasicSettingDto dto);
        Task<HrmLeaveBasicSettingDto> GetLeaveBasicSettingById (int leaveSettingId);
        Task<bool> DeleteLeaveBasicSetting (int leaveSettingId);
        Task<List<HrmLeaveBasicSettingDto>> GetAllLeaveBasicSettings();
        }
    }
