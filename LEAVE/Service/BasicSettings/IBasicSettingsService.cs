using HRMS.EmployeeInformation.DTO.DTOs;
using LEAVE.Dto;

namespace LEAVE.Service.BasicSettings
{
    public interface IBasicSettingsService
    {
        Task<List<FillvacationaccrualDto>> Fillvacationaccrual(int basicsettingsid);
        Task<List<GetEditbasicsettingsdto>> GetEditbasicsettings(int Masterid);
        Task<List<GetEditbasicsettingsdto>> saveleavelinktable(int Masterid);
        Task<int?> DeleteConfirm(int Basicsettingsid);
        Task<int?> GetDeletebasics(int Basicsettingsid, int Masterid);
        Task<object> Geteditdetails(string entitlement, int masterId, int? experienceId = null);
        Task<int> Createbasicsettings(CreatebasicsettingsDto CreatebasicsettingsDto);
        Task<List<LeaveDetailModelDto>> FillleavetypeListAsync(int SecondEntityId, int Empid);

    }
}
