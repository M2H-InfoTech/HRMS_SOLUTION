using HRMS.EmployeeInformation.DTO.DTOs;
using LEAVE.Dto;

namespace LEAVE.Service.BasicSettings
{
    public interface IBasicSettingsService
    {
        Task<List<FillvacationaccrualDto>> Fillvacationaccrual(int basicsettingsid);
        Task<List<GetEditbasicsettingsdto>> GetEditbasicsettings(int Masterid);
        Task<List<HrmLeaveMasterandsettingsLinksDto>> saveleavelinktable(int masterId, int basicSettingsId, int createdBy);
        Task<int?> DeleteConfirm(int Basicsettingsid);
        Task<int?> GetDeletebasics(int Basicsettingsid, int Masterid, string transactionType);
        Task<object> Geteditdetails(string entitlement, int masterId, int? experienceId = null);
        Task<int> Createbasicsettings(CreatebasicsettingsDto CreatebasicsettingsDto);
        Task<List<LeaveDetailModelDto>> FillleavetypeListAsync(int SecondEntityId, int Empid);
        Task<List<BasicSettingDto>> GetEditbasicsettingsAsync(int masterid);
        //Task<long?> UpdatetLeaveMasterAndSettingsLinkAsync(int masterId, int basicSettingsId, int createdBy);
        Task<long?> UpsertLeaveMasterAndSettingsLinkAsync(LeaveEntitlementDto leaveEntitlementDto);
    }
}
