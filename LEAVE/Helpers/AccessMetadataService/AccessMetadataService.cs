using EMPLOYEE_INFORMATION.Data;
using LEAVE.Dto;
using Microsoft.EntityFrameworkCore;
using MPLOYEE_INFORMATION.DTO.DTOs;

namespace LEAVE.Helpers.AccessMetadataService
{
    public class AccessMetadataService : IAccessMetadataService
    {
        private readonly ExternalApiService _externalApiService;
        private readonly EmployeeDBContext _context;
        public AccessMetadataService(ExternalApiService externalApiService, EmployeeDBContext context)
        {
            _externalApiService = externalApiService;
            _context = context;
        }
        public async Task<int?> GetTransactionIdByTransactionTypeAsync(string transactionType)
        {
            var response = await _externalApiService.GetTransactionIdByTransactionTypeAsync(transactionType);
            return response;
        }
        public async Task<AccessMetadataDto> GetAccessMetadataAsync(string transactionType, int roleId, int empId)
        {
            var transactionId = await _externalApiService.GetTransactionIdByTransactionTypeAsync(transactionType);
            var linkLevel = await _externalApiService.GetLinkLevelByRoleIdAsync(roleId);
            var hasAccess = await _externalApiService.GetEntityAccessRightsAsync(roleId, linkLevel);
            var accessDetails = await _externalApiService.AccessLevelDetailsAndEmpList(empId, transactionType, roleId);
            return new AccessMetadataDto
            {
                TransactionId = transactionId,
                LinkLevel = linkLevel,
                HasAccessRights = hasAccess,
                accessCheckResultDto = accessDetails
            };
        }
        public async Task<List<long?>> GetNewHighListAsync(int empId, int roleId, long transid, int? lnklev)
        {
            var empEntity = await _context.HrEmpMasters
                .Where(h => h.EmpId == empId)
                .Select(h => h.EmpEntity)
                .FirstOrDefaultAsync();

            var ctnew = SplitStrings_XML(empEntity)
                .Select((item, index) => new LinkItemDto { Item = item, LinkLevel = index + 2 })
                .ToList();

            var accessRights = await _context.EntityAccessRights02s
                .Where(s => s.RoleId == roleId)
                .ToListAsync();

            var applicableFinal = accessRights
                .Where(s => !string.IsNullOrEmpty(s.LinkId))
                .SelectMany(s => SplitStrings_XML(s.LinkId),
                    (s, item) => new LinkItemDto { Item = item, LinkLevel = s.LinkLevel })
                .ToList();

            if (lnklev > 0)
            {
                applicableFinal.AddRange(ctnew.Where(c => c.LinkLevel >= lnklev));
            }

            var applicableFinalSetLong = applicableFinal
                .Select(a => (long?)Convert.ToInt64(a.Item))
                .ToHashSet();

            var entityApplicable00Final = await _context.EntityApplicable00s
                .Where(e => e.TransactionId == transid)
                .Select(e => new { e.LinkId, e.LinkLevel, e.MasterId })
                .ToListAsync();

            var applicableFinalItems = applicableFinal.Select(a => a.Item).ToList();

            var applicableFinal02Emp = await (
                from emp in _context.EmployeeDetails
                join ea in _context.EntityApplicable01s on emp.EmpId equals ea.EmpId
                join hlv in _context.HighLevelViewTables on emp.LastEntity equals hlv.LastEntityId
                where ea.TransactionId == transid
                let levelOneId = hlv.LevelOneId.ToString()
                where applicableFinalItems.Contains(levelOneId)
                select ea.MasterId
            ).Distinct().ToListAsync();

            var newhigh = applicableFinalSetLong
                .Union(applicableFinal02Emp.Select(emp => (long?)emp))
                .ToList();

            return newhigh;
        }
        private List<string> SplitStrings_XML(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return new List<string>();

            return input.Split(',', StringSplitOptions.RemoveEmptyEntries)
                        .Select(x => x.Trim())
                        .ToList();
        }
    }
}
