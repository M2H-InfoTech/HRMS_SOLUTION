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
            return await _externalApiService.GetTransactionIdByTransactionTypeAsync(transactionType);
        }
        public async Task<int> GetEmployeeParameterSettingsAsync(int employeeId, string drpType = "", string parameterCode = "", string parameterType = "")
        {
            return await _externalApiService.EmployeeParameterSettings(employeeId, drpType, parameterCode, parameterType);
        }
        public async Task<AccessMetadataDto> GetAccessMetadataAsync(string transactionType, int roleId, int empId)
        {
            var transactionIdTask = _externalApiService.GetTransactionIdByTransactionTypeAsync(transactionType);
            var linkLevelTask = _externalApiService.GetLinkLevelByRoleIdAsync(roleId);

            await Task.WhenAll(transactionIdTask, linkLevelTask);

            var transactionId = transactionIdTask.Result;
            var linkLevel = linkLevelTask.Result;

            var accessTask = _externalApiService.GetEntityAccessRightsAsync(roleId, linkLevel);
            var accessDetailsTask = _externalApiService.AccessLevelDetailsAndEmpList(empId, transactionType, roleId);

            await Task.WhenAll(accessTask, accessDetailsTask);

            return new AccessMetadataDto
            {
                TransactionId = transactionId,
                LinkLevel = linkLevel,
                HasAccessRights = accessTask.Result,
                accessCheckResultDto = accessDetailsTask.Result
            };
        }

        public async Task<List<long?>> GetNewHighListAsync(int empId, int roleId, long transId, int? linkLevel)
        {
            // Get employee's entity string
            var empEntityStr = await _context.HrEmpMasters
                .Where(h => h.EmpId == empId)
                .Select(h => h.EmpEntity)
                .FirstOrDefaultAsync();

            var empEntityLinks = SplitStrings_XML(empEntityStr)
                .Select((item, index) => new LinkItemDto { Item = item, LinkLevel = index + 2 });

            var accessRights = await _context.EntityAccessRights02s
                .Where(s => s.RoleId == roleId && !string.IsNullOrEmpty(s.LinkId))
                .ToListAsync();

            var accessLinkItems = accessRights
                .SelectMany(s => SplitStrings_XML(s.LinkId)
                    .Select(item => new LinkItemDto { Item = item, LinkLevel = s.LinkLevel }));

            var applicableLinks = accessLinkItems.ToList();

            if (linkLevel > 0)
            {
                applicableLinks.AddRange(empEntityLinks.Where(c => c.LinkLevel >= linkLevel));
            }

            var applicableSet = new HashSet<long?>(applicableLinks
                .Select(a => long.TryParse(a.Item, out long val) ? (long?)val : null)
                .Where(id => id.HasValue));

            var applicableFinalItems = applicableLinks.Select(a => a.Item).ToHashSet();

            // Fetch applicable employee master IDs from EntityApplicable01
            var applicableEmpMasterIds = await (
                from emp in _context.EmployeeDetails
                join ea in _context.EntityApplicable01s on emp.EmpId equals ea.EmpId
                join hlv in _context.HighLevelViewTables on emp.LastEntity equals hlv.LastEntityId
                where ea.TransactionId == transId
                let levelOneId = hlv.LevelOneId.ToString()
                where applicableFinalItems.Contains(levelOneId)
                select ea.MasterId
            ).Distinct().ToListAsync();

            // Union and return final list
            applicableSet.UnionWith(applicableEmpMasterIds.Select(x => (long?)x));
            return applicableSet.ToList();
        }

        private List<string> SplitStrings_XML(string input)
        {
            return string.IsNullOrWhiteSpace(input)
                ? new List<string>()
                : input.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToList();
        }
        public async Task<int> EmployeeParameterSettings(int employeeId, string drpType, string parameterCode, string parameterType)
        {
            return await _externalApiService.EmployeeParameterSettings(employeeId, drpType, parameterCode, parameterType);
        }
    }
}
