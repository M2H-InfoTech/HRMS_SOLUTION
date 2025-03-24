using OFFICEKITCORELEAVE.OfficeKitHR.Leave.DTO;

namespace OFFICEKITCORELEAVE.OfficeKitHR.Leave.Interface.HrmLeaveBasicSettingInterface
    {
    public interface IHrmLeaveBasicSettingService
        {
        Task<int> HrmLeaveBasicSettingSave(HrmLeaveBasicSettingDto dto);
        Task<HrmLeaveBasicSettingDto> GetLeaveBasicSettingsById (int leavebasicSettingsId);
        Task<bool> DeleteBasicSettings(int leavebasicSettingsId);
        Task<List<HrmLeaveBasicSettingDto>> GetAllLeaveBasicSettings();
        }
    }
