using AutoMapper;
using EMPLOYEE_INFORMATION.Data;
using Microsoft.EntityFrameworkCore;
using MPLOYEE_INFORMATION.DTO.DTOs;
using OFFICEKITCORELEAVE.OfficeKit.Leave.Data;
using OFFICEKITCORELEAVE.OfficeKit.Leave.DTO;
using OFFICEKITCORELEAVE.OfficeKit.Leave.MODELS;
using OFFICEKITCORELEAVE.OfficeKitHR.Leave.Interface.LeaveInterfaces;

namespace OFFICEKITCORELEAVE.OfficeKitHR.Leave.SERVICE.LeaveServices
{
    public class LeaveMasterService : ILeaveMasterService
    {
        private readonly IMapper _mapper;
        private readonly LeaveDBContext _dbContext;
        private readonly EmployeeDBContext _employeedbContext;
        public LeaveMasterService (IMapper mapper_, LeaveDBContext dbContext_, EmployeeDBContext employeedbContext_)
        {
            _mapper = mapper_;
            _dbContext = dbContext_;
            _employeedbContext = employeedbContext_;
        }

        public async Task<int> SaveLeaveMaster (HrmLeaveMasterDTO dto)
        {
            var leaveMaster = _mapper.Map<HrmLeaveMaster> (dto);

            if (dto.LeaveMasterId != 0)
            {
                _dbContext.HrmLeaveMasters.Update (leaveMaster);
            }


            else
            {
                await _dbContext.HrmLeaveMasters.AddAsync (leaveMaster);
            }

            await _dbContext.SaveChangesAsync ( );
            return leaveMaster.LeaveMasterId;
        }

        public async Task<HrmLeaveMasterDTO> GetLeaveMasterById (int leaveMasterId)
        {
            try
            {
                var leaveMaster = await _dbContext.HrmLeaveMasters.FindAsync (leaveMasterId);
                if (leaveMaster == null)
                {
                    return null;
                }

                return _mapper.Map<HrmLeaveMasterDTO> (leaveMaster);
            }
            catch (Exception ex)
            {
                throw new Exception ("Failed to retrieve leave master", ex);
            }
        }

        public async Task<bool> DeleteLeaveMaster (int leaveMasterId)
        {
            var Data = _mapper.Map<HrmLeaveMaster> (await GetLeaveMasterById (leaveMasterId));
            if (Data == null)
            {
                return false;
            }
            _dbContext.HrmLeaveMasters.Remove (Data);
            _dbContext.SaveChanges ( );
            return true;
        }

        public async Task<bool> Checkexistance (string leaveCode)
        {
            var checkDuplication = await _dbContext.HrmLeaveMasters.FirstOrDefaultAsync (x => x.LeaveCode.Replace (" ", "") == leaveCode.Replace (" ", ""));

            return checkDuplication != null;
        }
        private static IEnumerable<string> SplitStrings_XML (string list, char delimiter = ',')
        {
            if (string.IsNullOrWhiteSpace (list))
                return Enumerable.Empty<string> ( );

            // Split the input string by the delimiter and return as IEnumerable
            return list.Split (delimiter)
                   .Select (item => item.Trim ( )) // Trim whitespace from each item
                   .Where (item => !string.IsNullOrEmpty (item)); // Exclude empty items
        }
        public async Task<List<HrmLeaveMasterViewDto>> GetAllLeaveMasters (HrmLeaveMasterSearchDto sortDto)
        {
            var transid = await _employeedbContext.TransactionMasters
               .Where (t => t.TransactionType == "Leave")
               .Select (t => t.TransactionId)
               .FirstOrDefaultAsync ( );

            int? lnklev = await _employeedbContext.SpecialAccessRights
                .Where (s => s.RoleId == sortDto.RoleId)
                .Select (s => s.LinkLevel)
                .FirstOrDefaultAsync ( );

            bool hasAccess = await _employeedbContext.EntityAccessRights02s
                .AnyAsync (s => s.RoleId == sortDto.RoleId && s.LinkLevel == 15);

            if (hasAccess)
            {
                var leaveMasters = _dbContext.HrmLeaveMasters.ToList ( );

                // Query employeemaster from _employeedbContext
                var adm_User_Masters = _employeedbContext.AdmUserMasters.ToList ( );

                var leavemastersdata = (from leaveMaster in leaveMasters
                                        join ADM_User_Master in adm_User_Masters
                                        on leaveMaster.CreatedBy equals ADM_User_Master.UserId
                                        select new HrmLeaveMasterViewDto
                                        {
                                            Username = ADM_User_Master.UserName,
                                            LeavemasterId = leaveMaster.LeaveMasterId,
                                            LeaveCode = leaveMaster.LeaveCode,
                                            Description = leaveMaster.Description,
                                            PayType = leaveMaster.PayType,
                                            LeaveUnit = leaveMaster.LeaveUnit,
                                            Active = leaveMaster.Active,
                                        }).ToList ( );
                return leavemastersdata;
            }

            // **Step 1: Compute `ctnew`**
            var empEntity = await _employeedbContext.HrEmpMasters
                .Where (h => h.EmpId == sortDto.employeeId)
                .Select (h => h.EmpEntity)
                .FirstOrDefaultAsync ( );

            var ctnew = SplitStrings_XML (empEntity, ',')
                .Select ((item, index) => new LinkItemDto { Item = item, LinkLevel = index + 2 })
                .Where (c => !string.IsNullOrEmpty (c.Item))
                .ToList ( );

            // **Step 2: Compute `applicableFinal`**
            var applicableFinal = _employeedbContext.EntityAccessRights02s
                .Where (s => s.RoleId == sortDto.RoleId).ToList ( )
                .SelectMany (s => SplitStrings_XML (s.LinkId, default),
                    (s, item) => new LinkItemDto { Item = item, LinkLevel = s.LinkLevel })
                ;

            var applicablefinall = applicableFinal.ToList ( );

            if (lnklev > 0)
            {
                applicablefinall.AddRange (
                    ctnew.Where (c => c.LinkLevel >= lnklev)
                         .Select (c => new LinkItemDto { Item = c.Item, LinkLevel = c.LinkLevel })
                );
            }

            // Convert `applicableFinal` to HashSet for fast lookup
            //var applicableFinalSet = applicableFinal.Select(a => a.Item).ToHashSet();
            var applicableFinalSetLong = applicableFinal.Select (a => (long?)Convert.ToInt64 (a.Item)).ToHashSet ( );

            // **Step 3: Fetch `EntityApplicable00Final`**
            var entityApplicable00Final = await _employeedbContext.EntityApplicable00s
                .Where (e => e.TransactionId == transid)
                .Select (e => new { e.LinkId, e.LinkLevel, e.MasterId })
                .ToListAsync ( );

            // **Step 4: Compute `applicableFinal02`**
            var applicableFinal02 = applicableFinal.ToList ( ); // Already computed

            // **Step 5: Compute `applicableFinal02Emp`**
            var employeedetailsen = await _employeedbContext.EmployeeDetails.ToListAsync ( );
            var access2 = await _employeedbContext.EntityApplicable01s.ToListAsync ( );
            var high = await _employeedbContext.HighLevelViewTables.ToListAsync ( );


            var applicableFinal02Emp = (
            from emp in employeedetailsen
            join ea in access2 on emp.EmpId equals ea.EmpId
            join hlv in high on emp.LastEntity equals hlv.LastEntityId
            join af2 in applicableFinal02.ToList ( )  // Convert to List before join
            on hlv.LevelOneId.ToString ( ) equals af2.Item into af2LevelOne
            from af2L1 in af2LevelOne.DefaultIfEmpty ( )
            where ea.TransactionId == transid
            select ea.MasterId).Distinct ( ).ToList ( );



            // **Step 6: Compute `newhigh`**
            var newhigh = entityApplicable00Final
                .Where (e => applicableFinalSetLong.Contains (e.LinkId) || e.LinkLevel == 15)
                .Select (e => e.MasterId)
                .Union (applicableFinal02Emp)
                .Distinct ( )
                .ToList ( );

            var hrmleavemasters =await _dbContext.HrmLeaveMasters.ToListAsync();
            var admusermasters =await _employeedbContext.AdmUserMasters.ToListAsync();

            return  (from leaveMaster in hrmleavemasters
                          where newhigh.Contains (leaveMaster.LeaveMasterId)
                          join ADM_User_Master in admusermasters
                          on leaveMaster.CreatedBy equals ADM_User_Master.UserId
                          select new HrmLeaveMasterViewDto
                          {
                              Username = ADM_User_Master.UserName,
                              LeavemasterId = leaveMaster.LeaveMasterId,
                              LeaveCode = leaveMaster.LeaveCode,
                              Description = leaveMaster.Description,
                              PayType = leaveMaster.PayType,
                              LeaveUnit = leaveMaster.LeaveUnit,
                              Active = leaveMaster.Active
                          }).ToList( );
        }
    }
}
