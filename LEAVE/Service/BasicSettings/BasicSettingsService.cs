using HRMS.EmployeeInformation.DTO.DTOs;
using LEAVE.Dto;
using LEAVE.Repository.BasicSettings;

namespace LEAVE.Service.BasicSettings
{
    public class BasicSettingsService : IBasicSettingsService
    {
        private readonly IBasicSettingsRepository _basicSettingsRepository;
        public BasicSettingsService(IBasicSettingsRepository basicSettingsRepository)
        {

            _basicSettingsRepository = basicSettingsRepository;
        }

        public Task<List<FillvacationaccrualDto>> Fillvacationaccrual(int basicsettingsid)
        {
            return _basicSettingsRepository.Fillvacationaccrual(basicsettingsid);
        }
        public Task<List<GetEditbasicsettingsdto>> GetEditbasicsettings(int Masterid)
        {
            return _basicSettingsRepository.GetEditbasicsettings(Masterid);
        }
        public Task<List<HrmLeaveMasterandsettingsLinksDto>> saveleavelinktable(int masterId, int basicSettingsId, int createdBy)
        {
            return _basicSettingsRepository.saveleavelinktable(masterId, basicSettingsId, createdBy);
        }
        public Task<int?> DeleteConfirm(int Basicsettingsid)
        {
            return _basicSettingsRepository.DeleteConfirm(Basicsettingsid);

        }
        public Task<int?> GetDeletebasics(int Basicsettingsid, int Masterid, string transactionType)
        {
            return _basicSettingsRepository.GetDeletebasics(Basicsettingsid, Masterid, transactionType);
        }
        public async Task<object> Geteditdetails(string entitlement, int masterId, int? experienceId = null)
        {
            return await _basicSettingsRepository.Geteditdetails(entitlement, masterId, experienceId);
        }
        public async Task<int> Createbasicsettings(CreatebasicsettingsDto CreatebasicsettingsDto)
        {
            return await _basicSettingsRepository.Createbasicsettings(CreatebasicsettingsDto);
        }
        public Task<List<LeaveDetailModelDto>> FillleavetypeListAsync(int SecondEntityId, int Empid)
        {
            return _basicSettingsRepository.FillleavetypeListAsync(SecondEntityId, Empid);
        }
        public Task<List<BasicSettingDto>> GetEditbasicsettingsAsync(int masterid)
        {
            return _basicSettingsRepository.GetEditbasicsettingsAsync(masterid);
        }
    }
}
