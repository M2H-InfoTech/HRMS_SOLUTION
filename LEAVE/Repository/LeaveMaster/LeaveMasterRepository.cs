using EMPLOYEE_INFORMATION.Data;
using Microsoft.EntityFrameworkCore;
using MPLOYEE_INFORMATION.DTO.DTOs;

namespace LEAVE.Repository.LeaveMaster
{
    public class LeaveMasterRepository : ILeaveMasterRepository
    {
        private readonly EmployeeDBContext _context;
        public LeaveMasterRepository(EmployeeDBContext dbContext)
        {
            _context = dbContext;
        }
        private async Task<int> GetTransactionIdByTransactionType(string transactionType)
        {
            return await _context.TransactionMasters.Where(a => a.TransactionType == transactionType).Select(a => a.TransactionId).FirstOrDefaultAsync();
        }
        private async Task<int?> GetLinkLevelByRoleId(int roleId)
        {
            return await _context.EntityAccessRights02s
            .Where(e => e.RoleId == roleId)
            .OrderBy(e => e.LinkLevel)
            .Select(e => e.LinkLevel)
            .FirstOrDefaultAsync();

        }
        private static IEnumerable<string> SplitStrings_XML(string list, char delimiter = ',')
        {
            if (string.IsNullOrWhiteSpace(list))
                return Enumerable.Empty<string>();

            // Split the input string by the delimiter and return as IEnumerable
            return list.Split(delimiter)
            .Select(item => item.Trim()) // Trim whitespace from each item
            .Where(item => !string.IsNullOrEmpty(item)); // Exclude empty items
        }
        private async Task<List<LinkItemDto>> GetEntityAccessRights(int roleId, int? linkSelect)
        {
            var entityAccessRights = await _context.EntityAccessRights02s
            .Where(s => s.RoleId == roleId && s.LinkLevel == linkSelect)
            .ToListAsync();
            return entityAccessRights
            .Where(s => !string.IsNullOrEmpty(s.LinkId))
            .SelectMany(s => SplitStrings_XML(s.LinkId, default), (s, item) => new LinkItemDto
            {
                Item = item,
                LinkLevel = s.LinkLevel
            })
            .Where(f => !string.IsNullOrEmpty(f.Item))
            .ToList();
        }
        public async Task<int?> FillLeaveMasterAsync(int SecondEntityId, int EmpId)
        {
            var transId = await GetTransactionIdByTransactionType("Leave");
            var linkLevel = await GetLinkLevelByRoleId(SecondEntityId);
            var entityAccessRights = await GetEntityAccessRights(SecondEntityId, linkLevel);
            if (entityAccessRights != null)
            {
                var leaveDetails = await (from l in _context.HrmLeaveMasters
                                          join u in _context.AdmUserMasters
                                          on l.CreatedBy equals u.UserId into userJoin
                                          from u in userJoin.DefaultIfEmpty()
                                          select new
                                          {
                                              u.UserName,
                                              l.LeaveMasterId,
                                              l.LeaveCode,
                                              l.Description,
                                              l.PayType,
                                              l.LeaveUnit,
                                              l.Active,
                                              CreatedDate = l.CreatedDate
                                          }).ToListAsync();

            }
            return 1;
        }
    }

}
