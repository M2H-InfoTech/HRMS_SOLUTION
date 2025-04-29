using EMPLOYEE_INFORMATION.Data;
using HRMS.EmployeeInformation.DTO.DTOs;
using Microsoft.EntityFrameworkCore;

namespace LEAVE.Repository.BasicSettings
{
    public class BasicSettingsRepository : IBasicSettingsRepository
    {
        private readonly EmployeeDBContext _context;
        private readonly HttpClient _httpClient;
        public BasicSettingsRepository(EmployeeDBContext dbContext, HttpClient httpClient)
        {
            _context = dbContext;
            _httpClient = httpClient;
        }

        public async Task<List<FillvacationaccrualDto>> Fillvacationaccrual(int basicsettingsid)
        {
            var result = await (from a in _context.LeaveScheme00s
                                join b in _context.Leavescheme02s
                                    on a.LeaveSchemeId equals b.LeaveSchemeId
                                where b.Basicsettingsid == basicsettingsid
                                select new FillvacationaccrualDto
                                {
                                    VacationSchemeId = a.LeaveSchemeId,
                                    Scheme = a.SchemeDescription + " [ " + a.SchemeCode + " ]"
                                }).ToListAsync();

            return result;
        }


        public async Task<List<GetEditbasicsettingsdto>> GetEditbasicsettings(int basicsettingsid)
        {
            var result = from a in _context.HrmLeaveBasicSettings
                         join b in _context.HrmLeaveMasterandsettingsLinks
                             on a.SettingsId equals b.SettingsId
                         where a.SettingsId == basicsettingsid
                         select new GetEditbasicsettingsdto
                         {
                             SettingsName = a.SettingsName,
                             SettingsDescription = a.SettingsDescription,
                             LeaveMasterId = b.LeaveMasterId,
                             DaysOrHours = a.DaysOrHours
                         };

            return await result.ToListAsync();

        }

        public Task<List<GetEditbasicsettingsdto>> saveleavelinktable(int Masterid)
        {
            throw new NotImplementedException();
        }
    }
}
