using EMPLOYEE_INFORMATION.Data;
using LEAVE.Dto;
using Microsoft.EntityFrameworkCore;

namespace LEAVE.Repository.LeaveBalance
{
    public class LeaveBalanceRepository : ILeaveBalanceRepository
    {

        private readonly EmployeeDBContext _context;
        private readonly HttpClient _httpClient;
        public LeaveBalanceRepository(EmployeeDBContext dbContext, HttpClient httpClient)
        {
            _context = dbContext;
            _httpClient = httpClient;
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
    }


}
