using AutoMapper;
using EMPLOYEE_INFORMATION.Data;
using Microsoft.EntityFrameworkCore;
using OFFICEKITCORELEAVE.OfficeKit.Leave.Data;
using OFFICEKITCORELEAVE.OfficeKit.Leave.DTO;
using OFFICEKITCORELEAVE.OfficeKit.Leave.MODELS;
using OFFICEKITCORELEAVE.OfficeKitHR.Leave.Interface.LeaveInterfaces;
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
            var Data = GetLeaveMasterById (leaveMasterId);
            if (Data == null)
            {
                return false;
            }
            else
            {
                _dbContext.SaveChanges ( );
                return true;
            }
        }
        public async Task<List<HrmLeaveMasterViewDto>> GetAllLeaveMasters (HrmLeaveMasterSearchDto sortDto)
        {
            var transaction = await _employeedbContext.TransactionMasters.FirstOrDefaultAsync (x => x.TransactionType == "Leave");
            int transactionId = transaction?.TransactionId ?? 0;
            var specialright = _employeedbContext.SpecialAccessRights.Where (x => x.RoleId == sortDto.RoleId);
            var boolr = _employeedbContext.EntityAccessRights02s.AsEnumerable ( ).SelectMany (s => SplitStrings_XML (s.LinkId, default).Select (item => new { s.RoleId, s.LinkLevel, Item = item })).Any (x => x.RoleId == sortDto.RoleId && x.LinkLevel == 15);

            if (boolr)
            {
                // Query leaveMaster from _dbContext
                var leaveMasters = _dbContext.HrmLeaveMasters.ToList ( );

                // Query employeemaster from _employeedbContext
                var adm_User_Masters = _employeedbContext.AdmUserMasters.ToList ( );

                var resultdata = (from leaveMaster in leaveMasters
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
                return resultdata;
            }


            else
            {
                var lnklev = 0;
                var empEntities = (from emp in _employeedbContext.HrEmpMasters
                                   where emp.EmpId == sortDto.employeeId
                                   select emp.EmpEntity).FirstOrDefault ( )?.Split (',').Select ((item, index) => new
                                   {
                                       Item = item,
                                       LinkLevelSelf = (int?)(index + 2)
                                   }).ToList ( );
                var data = empEntities;
                var entityapplicable00 = await _employeedbContext.EntityApplicable00s.Where (x => x.TransactionId == transactionId).ToListAsync ( );
                var entityaccessright02 = await _employeedbContext.EntityAccessRights02s.Where (x => x.RoleId == sortDto.RoleId).ToListAsync ( );
                //var applicableFinal = (
                //    from s in _employeedbContext.EntityAccessRights02s
                //    from f in SplitStrings_XML (s.LinkId,default).Select (item => new { Item = item })
                //    where s.RoleId == sortDto.RoleId
                //    select new { Item = f.Item, LinkLevel = s.LinkLevel }
                //)
                //.Union (
                //    from ct in empEntities
                //    where lnklev > 0 && ct.LinkLevelSelf >= lnklev
                //    select new { Item = ct.Item, LinkLevel = ct.LinkLevelSelf }
                //)
                //.ToList ( );
                var applicableFinal = (from s in _employeedbContext.EntityAccessRights02s.AsEnumerable ( ) // Load data into memory
                                       from f in SplitStrings_XML (s.LinkId, default).Select (item => new { Item = item }) // Perform split in-memory
                                       where s.RoleId == sortDto.RoleId
                                       select new { Item = f.Item, LinkLevel = s.LinkLevel })
                                       .Union (
                                       from ct in empEntities where lnklev > 0 && ct.LinkLevelSelf >= lnklev select new { Item = ct.Item, LinkLevel = ct.LinkLevelSelf }).ToList ( );
                var EntityApplicalble00Final = _employeedbContext.EntityApplicable00s.ToListAsync ( ).Result.Where (x => x.TransactionId == transactionId);

                //var ApplicableFinal02EMP = from employeeDetails in _employeedbContext.EmployeeDetails
                //                           join entityApplicable01 in _employeedbContext.EntityApplicable01s on employeeDetails.EmpId equals entityApplicable01.EmpId
                //                           where entityApplicable01.TransactionId == transactionId
                //                           join a in _employeedbContext.HighLevelViewTables on employeeDetails.LastEntity equals a.LastEntityId
                //                           join b in applicableFinal on true equals true // avoid hardcoding the 1-to-1 matching
                //                           where
                //                               (b.LinkLevel == 1 && a.LevelOneId == b.Item) ||
                //                               (b.LinkLevel == 2 && a.LevelTwoId == b.Item) ||
                //                               (b.LinkLevel == 3 && a.LevelThreeId == b.Item) ||
                //                               (b.LinkLevel == 4 && a.LevelFourId == b.Item) ||
                //                               (b.LinkLevel == 5 && a.LevelFiveId == b.Item) ||
                //                               (b.LinkLevel == 6 && a.LevelSixId == b.Item) ||
                //                               (b.LinkLevel == 7 && a.LevelSevenId == b.Item) ||
                //                               (b.LinkLevel == 8 && a.LevelEightId == b.Item) ||
                //                               (b.LinkLevel == 9 && a.LevelNineId == b.Item) ||
                //                               (b.LinkLevel == 10 && a.LevelTenId == b.Item) ||
                //                               (b.LinkLevel == 11 && a.LevelElevenId == b.Item) ||
                //                               (b.LinkLevel == 12 && a.LevelTwelveId == b.Item)
                //                           select e.MasterId).Distinct ( ).ToList ( );

                var result = applicableFinal.ToList ( );

                List<HrmLeaveMasterViewDto> hrmLeaveMasters1 = new List<HrmLeaveMasterViewDto> ( );
                return hrmLeaveMasters1;

                //// Define the second result set (EntityApplicable00Final)
                //var entityApplicable00Final = _employeedbContext.EntityApplicable00s
                //    .Where (e => e.TransactionId == transactionId)
                //    .Select (e => new { e.LinkId, e.LinkLevel, e.MasterId })
                //    .ToList ( );

            }





            //List<HrmLeaveMaster> hrmLeaveMasters = new List<HrmLeaveMaster> ( );
            //var LeaveMasterList = await _dbContext.HrmLeaveMasters.ToListAsync ( );
            //var LeaveMasterListToView = _mapper.Map<List<HrmLeaveMasterDTO>> (LeaveMasterList);
            //return LeaveMasterListToView;
        }
        //public async Task<List<HrmLeaveMasterViewDto>> GetLeaveMasterDataAsync (HrmLeaveMasterSearchDto sortDto)
        //    {
        //    var transaction = await _employeedbContext.TransactionMasters
        //        .FirstOrDefaultAsync (x => x.TransactionType == "Leave");

        //    int transactionId = transaction?.TransactionId ?? 0; // Handle null case safely


        //    bool hasAccess = await _employeedbContext.EntityAccessRights02s
        //        .AnyAsync (x => x.RoleId == sortDto.RoleId && x.LinkLevel == 15);

        //    if (!hasAccess)
        //        {
        //        return new List<HrmLeaveMasterViewDto> ( ); // Return empty list if no access
        //        }

        //    // Fetch leave master data efficiently
        //    var resultData = await (from leaveMaster in _dbContext.HrmLeaveMasters
        //                            join employee in _employeedbContext.AdmUserMasters
        //                                on leaveMaster.CreatedBy equals employee.UserId
        //                            select new HrmLeaveMasterViewDto
        //                                {
        //                                Username = employee.UserName,
        //                                LeavemasterId = leaveMaster.LeaveMasterId,
        //                                LeaveCode = leaveMaster.LeaveCode,
        //                                Description = leaveMaster.Description,
        //                                PayType = leaveMaster.PayType,
        //                                LeaveUnit = leaveMaster.LeaveUnit,
        //                                Active = leaveMaster.Active
        //                                })
        //                            .ToListAsync ( ); // Execute query

        //    return resultData;
        //    }

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
    }
}
