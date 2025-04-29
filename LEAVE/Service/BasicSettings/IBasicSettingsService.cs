using HRMS.EmployeeInformation.DTO.DTOs;

namespace LEAVE.Service.BasicSettings
{
    public interface IBasicSettingsService
    {
        Task<List<FillvacationaccrualDto>> Fillvacationaccrual(int basicsettingsid);
        Task<List<GetEditbasicsettingsdto>> GetEditbasicsettings(int Masterid);
        Task<List<GetEditbasicsettingsdto>> saveleavelinktable(int Masterid);
    }
}
