using HRMS.EmployeeInformation.DTO.DTOs;

namespace LEAVE.Repository.BasicSettings
{
    public interface IBasicSettingsRepository
    {
        Task<List<FillvacationaccrualDto>> Fillvacationaccrual(int basicsettingsid);
        Task<List<GetEditbasicsettingsdto>> GetEditbasicsettings(int Masterid);
        Task<List<GetEditbasicsettingsdto>> saveleavelinktable(int Masterid);
        Task<int?> DeleteConfirm(int Basicsettingsid);
        Task<int?> GetDeletebasics(int Basicsettingsid, int Masterid);
        Task<object> Geteditdetails(string entitlement, int masterId, int? experienceId = null);
       

    }
}
    