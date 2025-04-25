using OFFICEKITCORELEAVE.OfficeKit.Leave.DTO;
using OFFICEKITCORELEAVE.OfficeKitHR.Leave.DTO;

namespace OFFICEKITCORELEAVE.OfficeKitHR.Leave.Services
{
    public class LeaveService : ILeaveService
    {
        public Task<bool> Checkexistance(string leaveCode)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteLeaveBasicSetting(int leaveSettingId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteLeaveMaster(int leaveMasterId)
        {
            throw new NotImplementedException();
        }

        public Task<int> EntityApplicableSave(SaveApplicableParameters dto)
        {
            throw new NotImplementedException();
        }

        public Task<List<HrmLeaveBasicSettingDto>> GetAllLeaveBasicSettings()
        {
            throw new NotImplementedException();
        }

        public Task<List<HrmLeaveMasterViewDto>> GetAllLeaveMasters(HrmLeaveMasterSearchDto sortDto)
        {
            throw new NotImplementedException();
        }

        public Task<HrmLeaveBasicSettingDto> GetLeaveBasicSettingById(int leaveSettingId)
        {
            throw new NotImplementedException();
        }

        public Task<HrmLeaveMasterDTO> GetLeaveMasterById(int leaveMasterId)
        {
            throw new NotImplementedException();
        }

        public Task<int> SaveLeaveBasicSetting(HrmLeaveBasicSettingDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<int> SaveLeaveBasicSettingsDetails(HrmLeaveBasicsettingsDetailDto LeaveBasicSettingsDetails)
        {
            throw new NotImplementedException();
        }

        public Task<int> SaveLeaveMaster(HrmLeaveMasterDTO dto)
        {
            throw new NotImplementedException();
        }
    }
}
