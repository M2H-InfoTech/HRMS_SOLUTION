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
    }
}
