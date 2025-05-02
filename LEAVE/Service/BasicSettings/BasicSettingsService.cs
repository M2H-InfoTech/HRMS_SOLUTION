using HRMS.EmployeeInformation.DTO.DTOs;
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
        public Task<List<GetEditbasicsettingsdto>> saveleavelinktable(int Masterid)
        {
            return _basicSettingsRepository.saveleavelinktable(Masterid);
        }
        public Task<int?> DeleteConfirm(int Basicsettingsid)
        {
            return _basicSettingsRepository.DeleteConfirm(Basicsettingsid);

        }
        public Task<int?> GetDeletebasics(int Basicsettingsid, int Masterid)
        {
            return _basicSettingsRepository.GetDeletebasics(Basicsettingsid, Masterid);
        }
        public async Task<object> Geteditdetails(string entitlement, int masterId, int? experienceId = null)
        {
            return await _basicSettingsRepository.Geteditdetails(entitlement, masterId, experienceId);
        }
    }
}
