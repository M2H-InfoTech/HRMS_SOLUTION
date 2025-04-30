using EMPLOYEE_INFORMATION.Data;
using HRMS.EmployeeInformation.Models.Models.Entity;
using LEAVE.Dto;
using LEAVE.Helpers.AccessMetadataService;
using Microsoft.EntityFrameworkCore;
using MPLOYEE_INFORMATION.DTO.DTOs;

namespace LEAVE.Repository.LeaveMaster
{
    public class LeaveMasterRepository : ILeaveMasterRepository
    {
        private readonly EmployeeDBContext _context;
        private readonly EmployeeSettings _employeeSettings;
        private IAccessMetadataService _accessMetadataService;
        public LeaveMasterRepository(EmployeeDBContext dbContext, IAccessMetadataService accessMetadataService)
        {
            _context = dbContext;
            _accessMetadataService = accessMetadataService;
        }

        private static IEnumerable<string> SplitStrings_XML(string list, char delimiter = ',') =>
            string.IsNullOrWhiteSpace(list)
                ? Enumerable.Empty<string>()
                : list.Split(delimiter)
                      .Select(item => item.Trim())
                      .Where(item => !string.IsNullOrEmpty(item));

        private async Task<List<LeaveDetailModelDto>> FillLeaveMasterAsyncNoAccessMode(int empId, int roleId, int? lnklev, int transid)
        {

            var newHigh = await _accessMetadataService.GetNewHighListAsync(empId, roleId, transid, lnklev);

            // Final Fetch
            return await _context.HrmLeaveMasters
                .Where(l => newHigh.Contains(l.LeaveMasterId))
                .GroupJoin(_context.AdmUserMasters,
                    l => l.CreatedBy,
                    u => u.UserId,
                    (l, users) => new { Leave = l, User = users.FirstOrDefault() })
                .Select(x => new LeaveDetailModelDto
                {
                    UserName = x.User.UserName,
                    LeaveMasterId = x.Leave.LeaveMasterId,
                    LeaveCode = x.Leave.LeaveCode,
                    Description = x.Leave.Description,
                    PayType = x.Leave.PayType,
                    LeaveUnit = x.Leave.LeaveUnit,
                    Active = x.Leave.Active,
                    CreatedDate = x.Leave.CreatedDate
                })
                .ToListAsync();
        }

        public async Task<List<LeaveDetailModelDto>> FillLeaveMasterAsync(int secondEntityId, int empId)
        {

            var accessMetadata = await _accessMetadataService.GetAccessMetadataAsync("Leave", secondEntityId, empId);
            if (accessMetadata.HasAccessRights)
            {
                // Fetch leave details if entity access rights are valid
                return await _context.HrmLeaveMasters
                    .GroupJoin(_context.AdmUserMasters,
                        l => l.CreatedBy,
                        u => u.UserId,
                        (l, users) => new { Leave = l, User = users.FirstOrDefault() })
                    .Select(x => new LeaveDetailModelDto
                    {
                        UserName = x.User.UserName,
                        LeaveMasterId = x.Leave.LeaveMasterId,
                        LeaveCode = x.Leave.LeaveCode,
                        Description = x.Leave.Description,
                        PayType = x.Leave.PayType,
                        LeaveUnit = x.Leave.LeaveUnit,
                        Active = x.Leave.Active,
                        CreatedDate = x.Leave.CreatedDate
                    })
                    .ToListAsync();
            }
            else
            {
                // If no entity access, fetch the result from dd method
                return await FillLeaveMasterAsyncNoAccessMode(empId, secondEntityId, accessMetadata.LinkLevel, accessMetadata.TransactionId);
            }
        }
        private static string FormatDate(DateTime? date, string format)
        {
            return date.HasValue ? date.Value.ToString(format) : string.Empty; // Or any other default value
        }
        public async Task<List<object>> FillvaluetypesAsync(string type)
        {
            if (type == "Entitlement")
            {
                var data = await _context.HrmValueTypes
                    .Where(v => v.Type == type && v.Code != "N")
                    .Select(v => new
                    {
                        v.Value,
                        v.Description
                    })
                    .ToListAsync();

                return data.Cast<object>().ToList();
            }
            else if (type == "ReasonMaster")
            {
                var data = await _context.GeneralCategories
                    .Select(g => new
                    {
                        g.Id,
                        g.Description,
                        g.Code
                    })
                    .ToListAsync();

                return data.Cast<object>().ToList();
            }
            else
            {
                var data = await _context.HrmValueTypes
                    .Where(v => v.Type == type)
                    .Select(v => new
                    {
                        v.Value,
                        v.Description
                    })
                    .ToListAsync();
                return data.Cast<object>().ToList();
            }
        }

        public async Task<int?> CreateMasterAsync(CreateMasterDto dto)
        {

            string camelCaseDescription = string.IsNullOrWhiteSpace(dto.Description) ? dto.Description : dto.Description.ToUpper();


            // If updating
            if (dto.MasterId != 0)
            {
                var existing = await _context.HrmLeaveMasters
                    .FirstOrDefaultAsync(x => x.LeaveMasterId == dto.MasterId);

                if (existing != null)
                {
                    existing.LeaveCode = dto.LeaveCode;
                    existing.Description = camelCaseDescription;
                    existing.PayType = dto.PayType;
                    existing.LeaveUnit = dto.LeaveUnit;
                    existing.Active = dto.Active;
                    existing.Colour = dto.Colour;

                    await _context.SaveChangesAsync();
                    return existing.LeaveMasterId;
                }
            }
            else
            {

                var exists = await _context.HrmLeaveMasters
                    .AnyAsync(x => x.LeaveCode == dto.LeaveCode);

                if (!exists)
                {
                    var newEntity = new HrmLeaveMaster
                    {
                        LeaveCode = dto.LeaveCode,
                        Description = camelCaseDescription,
                        PayType = dto.PayType,
                        LeaveUnit = dto.LeaveUnit,
                        Active = dto.Active,
                        CreatedBy = dto.CreatedBy,
                        Colour = dto.Colour,
                        CreatedDate = DateTime.UtcNow
                    };

                    _context.HrmLeaveMasters.Add(newEntity);
                    await _context.SaveChangesAsync();

                    return newEntity.LeaveMasterId;
                }
            }

            return null;
        }
        public async Task<List<object>> FillbasicsettingsAsync(int Masterid, int SecondEntityId, int EmpId)
        {
            if (Masterid == 0)
            {
                var accessMetadata = await _accessMetadataService.GetAccessMetadataAsync("Leave_BS", SecondEntityId, EmpId);
                if (accessMetadata.HasAccessRights)
                {
                    var result = await (from b in _context.HrmLeaveBasicSettings
                                        join a in _context.AdmUserMasters
                                            on b.CreatedBy equals a.UserId into gj
                                        from a in gj.DefaultIfEmpty()
                                        select new
                                        {
                                            UserName = a != null ? a.UserName : null,
                                            SettingsId = b.SettingsId,
                                            SettingsName = b.SettingsName,
                                            SettingsDescription = b.SettingsDescription,
                                            CreatedDate = b.CreatedDate.HasValue ? FormatDate(b.CreatedDate, _employeeSettings.DateFormat) : null
                                        })
                                        .Distinct()
                                        .ToListAsync<object>();

                    return result;
                }
                var newHigh = await _accessMetadataService.GetNewHighListAsync(EmpId, SecondEntityId, accessMetadata.TransactionId, accessMetadata.LinkLevel);
                var finalResult = await (from b in _context.HrmLeaveBasicSettings
                                         join a in _context.AdmUserMasters
                                             on b.CreatedBy equals a.UserId into gj
                                         from a in gj.DefaultIfEmpty()
                                         where newHigh.Contains(b.SettingsId)
                                         select new
                                         {
                                             UserName = a != null ? a.UserName : null,
                                             SettingsId = b.SettingsId,
                                             SettingsName = b.SettingsName,
                                             SettingsDescription = b.SettingsDescription,
                                             CreatedDate = b.CreatedDate.HasValue ? FormatDate(b.CreatedDate, _employeeSettings.DateFormat) : null
                                         })
                                         .Distinct()
                                         .ToListAsync<object>();

                return finalResult;
            }
            else
            {
                var finalResult = await (from lmsl in _context.HrmLeaveMasterandsettingsLinks
                                         join lm in _context.HrmLeaveMasters
                                             on lmsl.LeaveMasterId equals lm.LeaveMasterId
                                         join lbs in _context.HrmLeaveBasicSettings
                                             on lmsl.SettingsId equals lbs.SettingsId
                                         join um in _context.AdmUserMasters
                                             on lbs.CreatedBy equals um.UserId into userJoin
                                         from um in userJoin.DefaultIfEmpty()
                                         where lm.LeaveMasterId == Masterid
                                         select new
                                         {
                                             UserName = um != null ? um.UserName : null,
                                             SettingsId = lbs.SettingsId,
                                             SettingsName = lbs.SettingsName,
                                             SettingsDescription = lbs.SettingsDescription,
                                             CreatedDate = lbs.CreatedDate.HasValue ? lbs.CreatedDate.Value.ToString("dd/MM/yyyy") : null
                                         })
                                         .Distinct()
                                         .ToListAsync<object>();

                return finalResult;
            }
        }
    }

}
