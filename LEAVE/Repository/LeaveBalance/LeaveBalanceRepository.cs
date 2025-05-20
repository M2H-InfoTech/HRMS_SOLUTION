using EMPLOYEE_INFORMATION.Data;
using LEAVE.Dto;
using LEAVE.Helpers;
using Microsoft.EntityFrameworkCore;

namespace LEAVE.Repository.LeaveBalance
{
    public class LeaveBalanceRepository : ILeaveBalanceRepository
    {

        private readonly EmployeeDBContext _context;
        private readonly HttpClient _httpClient;
        private readonly ExternalApiService _externalApiService;
        public LeaveBalanceRepository(EmployeeDBContext dbContext, HttpClient httpClient, ExternalApiService externalApiServic)
        {
            _context = dbContext;
            _httpClient = httpClient;
            _externalApiService = externalApiServic;
        }


        public async Task<List<RetrieveBranchDetailsDto>> RetrieveBranchDetails(int instID, int branchID)
        {
            var topBranches = await (
                from mb in _context.MasterBranchDetails
                join hvt in _context.HighLevelViewTables on mb.SubBranchId equals hvt.LevelSevenId
                where mb.InstId == instID && (mb.BranchId == branchID || branchID == 0)
                group mb by mb.BranchName into g
                select g.OrderByDescending(x => x.BranchId).FirstOrDefault()
            ).ToListAsync();

            var result = topBranches.Select(topBranch => new RetrieveBranchDetailsDto
            {
                InstId = topBranch.InstId,
                BranchId = topBranch.BranchId,
                BranchCode = topBranch.BranchCode,
                SubBranchId = topBranch.SubBranchId,
                BranchName = topBranch.BranchName,
                Location = topBranch.Location,
                Address = topBranch.Address,
                DateOfBranchOpen = topBranch.DateOfBranchOpen,
                Currency = topBranch.Currency,
                Latitude = topBranch.Latitude,
                Longitude = topBranch.Longitude,
                Circumference = topBranch.Circumference,
                Email = topBranch.Email,
                ExtensionCode = topBranch.ExtensionCode,
                Phone = topBranch.Phone,
                Logo = topBranch.Logo,
                TimeOffSetId = topBranch.TimeOffSetId
            }).ToList();

            return result;
        }


        public async Task<List<LeaveApplicationDto>> GetLeaveApplicationsAsync(int employeeId, int leaveId, string approvalStatus, string? flowStatus, DateTime? leaveFrom, DateTime? leaveTo)
        {
            IQueryable<LeaveApplicationDto> query;
            IQueryable<LeaveApplicationDto> groupedQuery;
            var baseLeaveData =
                from a in _context.LeaveApplication00s
                join d in _context.LeaveApplication02s on a.LeaveApplicationId equals d.LeaveApplicationId into adGroup
                from d in adGroup.DefaultIfEmpty()
                join b in _context.HrmLeaveMasters on a.LeaveId equals b.LeaveMasterId into abGroup
                from b in abGroup.DefaultIfEmpty()
                join c in _context.EmployeeDetails on a.EmployeeId equals c.EmpId into acGroup
                from c in acGroup.DefaultIfEmpty()
                select new
                {
                    a.LeaveApplicationId,
                    a.LeaveFrom,
                    a.LeaveTo,
                    a.NoOfLeaveDays,
                    a.Reason,
                    a.TimeMode,
                    a.ReturnDate,
                    a.IdproxyLeave,
                    a.ApprovalStatus,
                    a.Cancelstatus,
                    a.EmployeeId,
                    a.LeaveId,
                    a.RequestId,
                    a.FlowStatus,
                    LeaveCode = b != null ? b.LeaveCode : null,
                    Description = b != null ? b.Description : null,
                    c.Name,
                    c.EmpId,
                    LeaveDate = d != null ? d.LeaveDate : (DateTime?)null
                };

            groupedQuery =
                           from bl in baseLeaveData
                           join lc in _context.Leavecancel00s on bl.LeaveApplicationId equals lc.LeaveApplicationId into lcGroup
                           from lc in lcGroup.DefaultIfEmpty()
                           where bl.EmployeeId == employeeId
                               && bl.LeaveId == leaveId
                               && (
                                   (approvalStatus == "C" || approvalStatus == "PC")
                                   && (bl.Cancelstatus == approvalStatus || (bl.Cancelstatus != null && !bl.Cancelstatus.Equals("C")))
                                   || (!new[] { "C", "PC" }.Contains(approvalStatus)
                                       && (bl.ApprovalStatus == approvalStatus || string.IsNullOrEmpty(approvalStatus))
                                       && (bl.Cancelstatus ?? "P").Equals("P"))
                               )
                               && !bl.ApprovalStatus.Equals("D")
                               && (string.IsNullOrEmpty(flowStatus) || bl.FlowStatus == flowStatus)
                               && (
                                   (bl.LeaveDate >= leaveFrom && bl.LeaveDate <= leaveTo)
                                   || (leaveFrom == null && leaveTo == null)
                               )
                           group new { bl, lc } by new
                           {
                               bl.LeaveCode,
                               bl.Description,
                               bl.LeaveApplicationId,
                               Lcfromdate = lc != null ? lc.Lcfromdate : (DateTime?)null,
                               Lctodate = lc != null ? lc.Lctodate : (DateTime?)null,
                               bl.LeaveFrom,
                               bl.LeaveTo,
                               bl.NoOfLeaveDays,
                               bl.Reason,
                               bl.ReturnDate,
                               bl.IdproxyLeave,
                               DayTypeVal = lc != null ? lc.Lcdaytype : bl.TimeMode,
                               bl.TimeMode,
                               bl.Name,
                               bl.EmpId,
                               bl.Cancelstatus,
                               bl.ApprovalStatus,
                               bl.RequestId,
                               Lcdays = lc != null ? lc.Lcdays : (decimal?)null,
                               LeaveCancelId = lc != null ? lc.LeavecancelId : bl.LeaveApplicationId
                           } into g
                           orderby g.Key.LeaveApplicationId descending
                           select new LeaveApplicationDto
                           {
                               CaseCode = "R",
                               LeaveCode = g.Key.LeaveCode,
                               Description = g.Key.Description,
                               LeaveApplicationId = g.Key.LeaveApplicationId,
                               LeaveFrom = g.Key.Lcfromdate ?? g.Key.LeaveFrom,
                               LeaveTo = g.Key.Lctodate ?? g.Key.LeaveTo,
                               NoOfLeaveDays = g.Key.NoOfLeaveDays ?? 0,
                               Reason = g.Key.Reason,
                               TimeMode = g.Key.DayTypeVal,
                               Daytype = g.Key.DayTypeVal == 1 ? "Full Day" :
                                         g.Key.DayTypeVal == 2 ? "Half Day" : "NA",
                               ReturnDate = g.Key.ReturnDate,
                               IDProxyLeave = g.Key.IdproxyLeave,
                               ApprovalStatus = g.Key.ApprovalStatus,
                               Name = g.Key.Name,
                               EmployeeId = g.Key.EmpId,
                               ApprovalStatusDesc =
                                   g.Key.Cancelstatus == "C" ? "Cancelled" :
                                   g.Key.Cancelstatus == "PC" ? "Partially Cancelled" :
                                   g.Key.ApprovalStatus == "P" ? "Pending" :
                                   g.Key.ApprovalStatus == "R" ? "Rejected" :
                                   g.Key.ApprovalStatus == "A" ? "Approved" :
                                   g.Key.ApprovalStatus == "D" ? "Deleted" : "NA",
                               RequestId = g.Key.RequestId,
                               FileName = "",
                               CancelledDays = g.Key.Lcdays ?? 0,
                               LeaveCancelId = g.Key.LeaveCancelId
                           };



            query = groupedQuery;

            return await query.ToListAsync();
        }


    }


}
