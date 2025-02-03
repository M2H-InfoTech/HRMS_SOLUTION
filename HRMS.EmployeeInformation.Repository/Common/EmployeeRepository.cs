using System.ComponentModel.Design;
using AutoMapper;
using EMPLOYEE_INFORMATION.Data;
using EMPLOYEE_INFORMATION.DTO.DTOs;
using EMPLOYEE_INFORMATION.Models;
using EMPLOYEE_INFORMATION.Models.Entity;
using EMPLOYEE_INFORMATION.Models.EnumFolder;
using HRMS.EmployeeInformation.DTO.DTOs;
using HRMS.EmployeeInformation.DTO.DTOs.Documents;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using MPLOYEE_INFORMATION.DTO.DTOs;


namespace HRMS.EmployeeInformation.Repository.Common
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly EmployeeDBContext _context;
        //private IStringLocalizer _stringLocalizer;
        private readonly IMemoryCache _memoryCache;
        private readonly EmployeeSettings _employeeSettings;
        private int paramDynVal;

        private readonly IMapper _mapper;
        public EmployeeRepository(EmployeeDBContext dbContext, EmployeeSettings employeeSettings, IMemoryCache memoryCache, IMapper mapper)
        {
            _context = dbContext;
            _employeeSettings = employeeSettings;
            _memoryCache = memoryCache;
            _mapper = mapper;

        }
        public async Task<string> GetDefaultCompanyParameter(int employeeId, string parameterCode, string type)
        {

            string? defaultValue = byte.MinValue.ToString();
            if (employeeId == 0)
            {
                return byte.MinValue.ToString();
            }


            var paramControlData = (from a in _context.CompanyParameters
                                    join b in _context.ParameterControlTypes
                                    on a.ControlType equals b.ParamControlId
                                    where a.ParameterCode == parameterCode && a.Type == type
                                    select new
                                    {

                                        b.ParamControlDesc,
                                        IsMultiple = b.IsMultiple ?? 0 // Handle null with ISNULL equivalent
                                    }).FirstOrDefault();

            if (paramControlData == null)
            {

                return byte.MinValue.ToString();
            }

            var paramControlDesc = paramControlData.ParamControlDesc;
            var isMultiple = paramControlData.IsMultiple != 0;
            var queryResultParent = (from a in _context.CompanyParameters02s
                                     join b in _context.CompanyParameters on a.ParamId equals b.Id
                                     where a.EmpId == employeeId && b.ParameterCode == parameterCode && b.Type == type
                                     select new
                                     {
                                         a.Value,
                                         a.Text,
                                         a.Data,
                                         b.MultipleValues
                                     }).FirstOrDefault();

            if (queryResultParent != null)
            {
                defaultValue = queryResultParent switch
                {
                    _ when paramControlDesc == "Number" => queryResultParent.Value?.ToString(),
                    _ when paramControlDesc == "Text" => queryResultParent.Text,
                    _ when paramControlDesc == "Image" => queryResultParent.Data,
                    _ => isMultiple ? queryResultParent.MultipleValues?.ToString() : queryResultParent.Value?.ToString()
                };
            }
            else
            {
                defaultValue = null; // Or set a default value as needed
            }
            // Check Entity level parameters if DefaultValue is still null
            if (string.IsNullOrEmpty(defaultValue) || defaultValue.Equals(byte.MinValue))
            {
                var entity = _context.EmployeeDetails.Where(e => e.EmpId == employeeId).Select(e => e.EmpEntity).FirstOrDefault();

                if (!string.IsNullOrEmpty(entity))
                {
                    var entityList = entity.Split(',').Select(int.Parse).ToList();

                    var queryResultEntity = (from a in _context.CompanyParameters01s
                                             join b in _context.CompanyParameters
                                                 on a.ParamId equals b.Id
                                             where a.LevelId != 1 // Exclude LevelID == 1
                                                && entityList.Contains((int)a.LinkId) // LinkID match
                                                && b.ParameterCode == parameterCode
                                                && b.Type == type
                                             orderby a.LevelId descending // Order by LevelID desc
                                             select new
                                             {
                                                 a.Value,
                                                 a.Text,
                                                 a.Data,
                                                 a.MultipleValues
                                             }).FirstOrDefault();
                    if (queryResultEntity != null)
                    {
                        // Step 3: Use Switch Expression to Get the Default Value
                        defaultValue = queryResultEntity switch
                        {
                            _ when paramControlDesc == "Number" => queryResultEntity?.Value?.ToString(),
                            _ when paramControlDesc == "Text" => queryResultEntity?.Text,
                            _ when paramControlDesc == "Image" => queryResultEntity?.Data,
                            _ => isMultiple ? queryResultEntity?.MultipleValues?.ToString() : queryResultEntity?.Value?.ToString()
                        };
                    }
                    else
                    {
                        defaultValue = null;
                    }
                }
            }

            // Check First Entity level parameters if DefaultValue is still null
            if (string.IsNullOrEmpty(defaultValue) || defaultValue.Equals(byte.MinValue))
            {
                var firstEntity = _context.EmployeeDetails
                    .Where(e => e.EmpId == employeeId)
                    .Select(e => e.EmpFirstEntity)
                    .FirstOrDefault();

                if (firstEntity != null)
                {
                    var queryResultFirstEntity = (from a in _context.CompanyParameters01s
                                                  join b in _context.CompanyParameters
                                                      on a.ParamId equals b.Id
                                                  where a.LevelId != 0
                                                     && b.ParameterCode == parameterCode
                                                     && b.Type == type
                                                     && a.LevelId == 1
                                                  orderby a.LevelId descending // Order by LevelID desc
                                                  select new
                                                  {
                                                      a.Value,
                                                      a.Text,
                                                      a.Data,
                                                      a.MultipleValues
                                                  }).FirstOrDefault();
                    if (queryResultFirstEntity != null)
                    {
                        defaultValue = queryResultFirstEntity switch
                        {
                            _ when paramControlDesc == "Number" => queryResultFirstEntity?.Value?.ToString(),
                            _ when paramControlDesc == "Text" => queryResultFirstEntity?.Text,
                            _ when paramControlDesc == "Image" => queryResultFirstEntity?.Data,
                            _ => isMultiple ? queryResultFirstEntity?.MultipleValues?.ToString() : queryResultFirstEntity?.Value?.ToString()
                        };
                    }
                    else
                    {
                        defaultValue = null;
                    }
                }
            }

            // Check Company level parameters if DefaultValue is still null
            if (string.IsNullOrEmpty(defaultValue) || defaultValue == "0")
            {

                var queryResultCompanyLevel = (from a in _context.CompanyParameters
                                               where a.ParameterCode == parameterCode
                                               && a.Type == type
                                               select new
                                               {
                                                   a.Value,
                                                   a.Text,
                                                   a.Data,
                                                   a.MultipleValues
                                               }).FirstOrDefault();
                if (queryResultCompanyLevel != null)
                {
                    defaultValue = queryResultCompanyLevel switch
                    {
                        _ when paramControlDesc == "Number" => queryResultCompanyLevel?.Value?.ToString(),
                        _ when paramControlDesc == "Text" => queryResultCompanyLevel?.Text,
                        _ when paramControlDesc == "Image" => queryResultCompanyLevel?.Data,
                        _ => isMultiple ? queryResultCompanyLevel?.MultipleValues?.ToString() : queryResultCompanyLevel?.Value?.ToString()
                    };
                }
                else
                {
                    defaultValue = null;
                }

            }

            return defaultValue ?? string.Empty;
        }


        private bool IsLinkLevelExists(int? roleId)
        {
            var exists = _context.EntityAccessRights02s.Where(s => s.RoleId == roleId && s.LinkLevel == 15).Select(x => x.LinkLevel).First();
            return exists > 0;
        }
        public async Task<EmployeeStatusResultDto> EmployeeStatus(int employeeId, string parameterCode, string type)
        {

            var extendedExcludedStatuses = _employeeSettings.Extended;

            // Determine excluded statuses based on the info format
            var infoFormat = await GetDefaultCompanyParameter(employeeId, parameterCode, type);

            var activeStatusesTask = await _context.HrmValueTypes
                    .Where(u => u.Type == _employeeSettings.UserSettings && _employeeSettings.ActiveStatusCodes.Contains(u.Code))
                    .Select(u => new ActiveStatusDto { Value = u.Value, Description = u.Description })
                    .ToListAsync();



            activeStatusesTask.Insert(0, new ActiveStatusDto { Value = 0, Description = _employeeSettings.Statuses });

            var employeeStatusesTask = await (from status in _context.HrEmpStatusSettings
                                              where status.Active == _employeeSettings.ActiveStatus
                                              select new EmployeeStatusDto
                                              {
                                                  StatusId = status.StatusId,
                                                  StatusDesc = status.StatusDesc,
                                                  Status = _employeeSettings.Extended.Contains(status.Status) ? _employeeSettings.Sep : _employeeSettings.EmpStatus
                                              }).ToListAsync();


            var systemStatusesTask = await _context.EmployeeCurrentStatuses
                     .Select(b => new SystemStatusDto { Status = b.Status, SortOrder = b.SortOrder, StatusDesc = b.StatusDesc })
                     .ToListAsync();


            var companyParameterCodes = from c in _context.CompanyParameters join h in _context.HrmValueTypes on c.Value equals h.Value where h.Type == _employeeSettings.EmployeeReportingType && c.ParameterCode == _employeeSettings.companyParameterCodes && c.Type == _employeeSettings.companyParameterCodesType select h.Code;



            employeeStatusesTask.Insert(0, new EmployeeStatusDto { StatusId = 0, StatusDesc = _employeeSettings.Statuses, Status = _employeeSettings.Statuses });

            systemStatusesTask.Insert(0, new SystemStatusDto { Status = 0, SortOrder = 0, StatusDesc = _employeeSettings.Statuses });

            var result = new EmployeeStatusResultDto
            {
                EmployeeStatuses = employeeStatusesTask,
                SystemStatuses = systemStatusesTask,
                CompanyParameterCodes = companyParameterCodes,
                ActiveStatus = activeStatusesTask
            };
            return await Task.FromResult(result);
        }

        public async Task<PaginatedResult<EmployeeResultDto>> GetEmpData(EmployeeInformationParameters employeeInformationParameters)
        {

            string infoFormatCacheKey = $"InfoFormat_{employeeInformationParameters.empId}";
            string linkLevelCacheKey = $"LinkLevel_{employeeInformationParameters.roleId}";
            string currentStatusCacheKey = $"CurrentStatusDesc";
            string ageFormatCacheKey = $"AgeFormat_{_employeeSettings.ParameterCode}";
            string existsEmployeeCacheKey = "ExistsEmployee";


            var infoFormat = await _memoryCache.GetOrCreateAsync(infoFormatCacheKey, async entry =>
            {
                entry.SetSlidingExpiration(TimeSpan.FromMinutes(5)); // Cache expires 5 minutes after the last access
                entry.SetAbsoluteExpiration(TimeSpan.FromHours(1));  // Cache expires 1 hour after creation
                return await GetDefaultCompanyParameter(employeeInformationParameters.empId, _employeeSettings.CompanyParameterEmpInfoFormat, _employeeSettings.CompanyParameterType);
            });


            //var infoFormat = await GetDefaultCompanyParameter(employeeInformationParameters.empId, _employeeSettings.CompanyParameterEmpInfoFormat, _employeeSettings.CompanyParameterType);
            int format = Convert.ToInt32(infoFormat);

            var linkLevelExists = _memoryCache.GetOrCreate(linkLevelCacheKey, entry =>
            {
                entry.SetSlidingExpiration(TimeSpan.FromMinutes(5));
                entry.SetAbsoluteExpiration(TimeSpan.FromHours(1));
                return IsLinkLevelExists(employeeInformationParameters.roleId);
            });


            //var linkLevelExists = IsLinkLevelExists(employeeInformationParameters.roleId);

            var CurrentStatusDesc = _memoryCache.GetOrCreate(currentStatusCacheKey, entry =>
            {
                entry.SetSlidingExpiration(TimeSpan.FromMinutes(5));
                entry.SetAbsoluteExpiration(TimeSpan.FromHours(1));
                return (from ec in _context.EmployeeCurrentStatuses
                        where ec.StatusDesc == _employeeSettings.OnNotice
                        select ec.Status).FirstOrDefault();
            });


            //var CurrentStatusDesc = (from ec in _context.EmployeeCurrentStatuses
            //                         where ec.StatusDesc == _employeeSettings.OnNotice
            //                         select ec.Status).FirstOrDefault();


            string? ageFormat = await _memoryCache.GetOrCreateAsync(ageFormatCacheKey, async entry =>
            {
                entry.SetSlidingExpiration(TimeSpan.FromMinutes(5));
                entry.SetAbsoluteExpiration(TimeSpan.FromHours(1));
                return await (from cp in _context.CompanyParameters
                              join vt in _context.HrmValueTypes on cp.Value equals vt.Value
                              where vt.Type == _employeeSettings.ValueType && cp.ParameterCode == _employeeSettings.ParameterCode
                              select vt.Code).FirstOrDefaultAsync();
            });

            //string? ageFormat = await (from cp in _context.CompanyParameters
            //                           join vt in _context.HrmValueTypes on cp.Value equals vt.Value
            //                           where vt.Type == _employeeSettings.ValueType && cp.ParameterCode == _employeeSettings.ParameterCode
            //                           select vt.Code).FirstOrDefaultAsync();

            //bool existsEmployee =  _context.HrEmpMasters.Any(emp => (emp.IsSave ?? 0) == 1);



            bool existsEmployee = await _memoryCache.GetOrCreateAsync(existsEmployeeCacheKey, async entry =>
            {
                entry.SetSlidingExpiration(TimeSpan.FromMinutes(5));
                entry.SetAbsoluteExpiration(TimeSpan.FromHours(1));
                return await _context.HrEmpMasters.AnyAsync(emp => (emp.IsSave ?? 0) == 1);
            });
            //bool existsEmployee = await _context.HrEmpMasters.AnyAsync(emp => (emp.IsSave ?? 0) == 1);

            //return format switch
            //{
            //    0 or 1 => await HandleFormatZeroOrOne(employeeInformationParameters, linkLevelExists, ageFormat, CurrentStatusDesc, existsEmployee),
            //    2 => await HandleFormatTwo(employeeInformationParameters, linkLevelExists, ageFormat, CurrentStatusDesc, existsEmployee),
            //    3 => await HandleFormatThree(employeeInformationParameters, linkLevelExists, ageFormat, CurrentStatusDesc, existsEmployee),
            //    4 => await HandleFormatFour(employeeInformationParameters, linkLevelExists, ageFormat, CurrentStatusDesc, existsEmployee),
            //};
            string cacheKey = $"HandleFormat_{format}_{employeeInformationParameters.empId}_{employeeInformationParameters.roleId}_{employeeInformationParameters.empStatus}";
            return await _memoryCache.GetOrCreateAsync(cacheKey, async entry =>
            {
                entry.SetSlidingExpiration(TimeSpan.FromMinutes(5));
                entry.SetAbsoluteExpiration(TimeSpan.FromHours(1));

                return format switch
                {
                    0 or 1 => await HandleFormatZeroOrOne(employeeInformationParameters, linkLevelExists, ageFormat, CurrentStatusDesc, existsEmployee),
                    2 => await HandleFormatTwo(employeeInformationParameters, linkLevelExists, ageFormat, CurrentStatusDesc, existsEmployee),
                    3 => await HandleFormatThree(employeeInformationParameters, linkLevelExists, ageFormat, CurrentStatusDesc, existsEmployee),
                    4 => await HandleFormatFour(employeeInformationParameters, linkLevelExists, ageFormat, CurrentStatusDesc, existsEmployee),
                    _ => throw new InvalidOperationException("Invalid format value.")
                };
            });




        }
        private async Task<PaginatedResult<EmployeeResultDto>> HandleFormatZeroOrOne(EmployeeInformationParameters employeeInformationParameters, bool linkLevelExists, string? ageFormat, int? currentStatusDesc, bool existsEmployee)
        {
            if (linkLevelExists)
            {
                return await InfoFormatOneOrZeroLinkLevelExist(
                    employeeInformationParameters.empStatus, employeeInformationParameters.systemStatus, employeeInformationParameters.empIds, employeeInformationParameters.filterType, employeeInformationParameters.durationFrom, employeeInformationParameters.durationTo, employeeInformationParameters.probationStatus, currentStatusDesc.ToString(), ageFormat, employeeInformationParameters.pageNumber, employeeInformationParameters.pageSize);
            }

            if (employeeInformationParameters.empIds == byte.MinValue.ToString())
            {
                var empidnews = await InfoFormatZeroOrOneLinkLevelNotExistAndZeroEmpIds(employeeInformationParameters.roleId, employeeInformationParameters.empId, linkLevelExists);

                var empIdList = empidnews?.Select(id => id.ToString().Trim()).ToList();




                if (existsEmployee)
                {
                    return await InfoFormatZeroOrOneEmpIdZeroEmployeeExists(employeeInformationParameters.pageNumber, employeeInformationParameters.pageSize, ageFormat, employeeInformationParameters.systemStatus, currentStatusDesc.ToString());
                }
                else
                {
                    return await InforFormatOneOrZeroNotExistLinkSelect(employeeInformationParameters.durationFrom, employeeInformationParameters.durationTo, employeeInformationParameters.empStatus, empIdList, employeeInformationParameters.pageNumber, employeeInformationParameters.pageSize, ageFormat);
                }
            }

            return new PaginatedResult<EmployeeResultDto>();
        }

        private async Task<PaginatedResult<EmployeeResultDto>> HandleFormatTwo(EmployeeInformationParameters employeeInformationParameters, bool linkLevelExists, string? ageFormat, int? currentStatusDesc, bool existsEmployee)
        {
            if (linkLevelExists)
            {
                return await InfoFormatTwoLinkLevelExists(employeeInformationParameters.empStatus, employeeInformationParameters.systemStatus, employeeInformationParameters.empIds, employeeInformationParameters.filterType, employeeInformationParameters.durationFrom, employeeInformationParameters.durationTo, employeeInformationParameters.probationStatus, currentStatusDesc.ToString(), ageFormat, employeeInformationParameters.pageNumber, employeeInformationParameters.pageSize);
            }

            else
            {
                if (employeeInformationParameters.empIds == byte.MinValue.ToString())
                {
                    var empidnews = await InfoFormatTwoLinkLevelNotExistAndZeroEmpIds(employeeInformationParameters.roleId, employeeInformationParameters.empId, linkLevelExists);

                    var empIdList = empidnews?.Select(id => id.ToString().Trim()).ToList();

                    //bool existsEmployee = _context.HrEmpMasters.Any(emp => (emp.IsSave ?? 0) == 1);


                    if (existsEmployee)
                    {
                        return await InfoFormatTwoEmpIdZeroEmployeeExists(employeeInformationParameters.pageNumber, employeeInformationParameters.pageSize, ageFormat, employeeInformationParameters.systemStatus, currentStatusDesc.ToString());
                    }
                    else
                    {
                        return await InforFormatTwoNotExistLinkSelect(employeeInformationParameters.durationFrom, employeeInformationParameters.durationTo, employeeInformationParameters.empStatus, empIdList, employeeInformationParameters.pageNumber, employeeInformationParameters.pageSize, ageFormat);

                    }

                }
            }
            return new PaginatedResult<EmployeeResultDto>();
        }


        private async Task<PaginatedResult<EmployeeResultDto>> HandleFormatThree(EmployeeInformationParameters employeeInformationParameters, bool linkLevelExists, string? ageFormat, int? currentStatusDesc, bool existsEmployee)
        {
            if (linkLevelExists)
            {
                return await InfoFormatThreeLinkLevelExists(employeeInformationParameters.empStatus, employeeInformationParameters.systemStatus, employeeInformationParameters.empIds, employeeInformationParameters.filterType, employeeInformationParameters.durationFrom, employeeInformationParameters.durationTo, employeeInformationParameters.probationStatus, currentStatusDesc.ToString(), ageFormat, employeeInformationParameters.pageNumber, employeeInformationParameters.pageSize);
            }
            else
            {

                if (employeeInformationParameters.empIds == byte.MinValue.ToString())
                {
                    var empidnews = await InfoFormatThreeLinkLevelNotExistAndZeroEmpIds(employeeInformationParameters.roleId, employeeInformationParameters.empId, linkLevelExists);

                    var empIdList = empidnews?.Select(id => id.ToString().Trim()).ToList();

                    //bool existsEmployee = _context.HrEmpMasters.Any(emp => (emp.IsSave ?? 0) == 1);


                    if (existsEmployee)
                    {
                        return await InfoFormatThreeEmpIdZeroEmployeeExists(employeeInformationParameters.pageNumber, employeeInformationParameters.pageSize, ageFormat, employeeInformationParameters.systemStatus, currentStatusDesc.ToString());
                    }
                    else
                    {
                        return await InforFormatThreeNotExistLinkSelect(employeeInformationParameters.durationFrom, employeeInformationParameters.durationTo, employeeInformationParameters.empStatus, empIdList, employeeInformationParameters.pageNumber, employeeInformationParameters.pageSize, ageFormat);

                    }

                }

            }
            return new PaginatedResult<EmployeeResultDto>();
        }


        private async Task<PaginatedResult<EmployeeResultDto>> HandleFormatFour(EmployeeInformationParameters employeeInformationParameters, bool linkLevelExists, string? ageFormat, int? currentStatusDesc, bool existsEmployee)
        {
            if (linkLevelExists)
            {
                return await InfoFormatFourLinkLevelExists(employeeInformationParameters.empStatus, employeeInformationParameters.systemStatus, employeeInformationParameters.empIds, employeeInformationParameters.filterType, employeeInformationParameters.durationFrom, employeeInformationParameters.durationTo, employeeInformationParameters.probationStatus, currentStatusDesc.ToString(), ageFormat, employeeInformationParameters.pageNumber, employeeInformationParameters.pageSize);
            }
            else
            {
                if (employeeInformationParameters.empIds == byte.MinValue.ToString())
                {
                    var empidnews = await InfoFormatFourLinkLevelNotExistAndZeroEmpIds(employeeInformationParameters.roleId, employeeInformationParameters.empId, linkLevelExists);

                    var empIdList = empidnews?.Select(id => id.ToString().Trim()).ToList();

                    //bool existsEmployee = _context.HrEmpMasters.Any(emp => (emp.IsSave ?? 0) == 1);


                    if (existsEmployee)
                    {
                        return await InfoFormatFourEmpIdZeroEmployeeExists(employeeInformationParameters.pageNumber, employeeInformationParameters.pageSize, ageFormat, employeeInformationParameters.systemStatus, currentStatusDesc.ToString());
                    }
                    else
                    {
                        return await InforFormatFourNotExistLinkSelect(employeeInformationParameters.durationFrom, employeeInformationParameters.durationTo, employeeInformationParameters.empStatus, empIdList, employeeInformationParameters.pageNumber, employeeInformationParameters.pageSize, ageFormat);

                    }

                }
            }
            return new PaginatedResult<EmployeeResultDto>();

        }

        public async Task<PaginatedResult<EmployeeResultDto>> InfoFormatOneOrZeroLinkLevelExist(string? empStatus, string? empSystemStatus, string? empIds, string? filterType, DateTime? durationFrom, DateTime? durationTo, int probationStatus, string? currentStatusDesc, string? ageFormat, int pageNumber, int pageSize)
        {
            // Parse and process the status list
            var statusList = empStatus?.Split(',')
                                       .Select(s => int.Parse(s.Trim()))
                                       .ToList();

            var filteredStatuses = await _context.HrEmpStatusSettings
                                                 .Where(s => s.IsResignation.Equals(false))
                                                 .Select(s => s.StatusId)
                                                 .ToListAsync();

            var excludedStatuses = await _context.HrEmpStatusSettings
                                                 .Where(s => s.IsResignation.Equals(true) && !statusList.Contains(s.StatusId))
                                                 .Select(s => s.StatusId)
                                                 .ToListAsync();

            var result = statusList?.Where(item => filteredStatuses.Contains(item)).ToList();



            var finalQuery = await (
    from emp in (
        from emp in _context.HrEmpMasters
        join res in _context.Resignations
               .Where(r =>
r.CurrentRequest == 1 &&
!new[] { "D", "R" }.Contains(r.ApprovalStatus) &&
r.ApprovalStatus == (empSystemStatus == currentStatusDesc ? "P" : "A") &&
r.RejoinStatus == "P")
        on emp.EmpId equals res.EmpId into resGroup
        from res in resGroup.DefaultIfEmpty()
        where
            (durationFrom == null || durationTo == null ||
             emp.JoinDt >= durationFrom && emp.JoinDt <= durationTo ||
             emp.ProbationDt >= durationFrom && emp.ProbationDt <= durationTo ||
             emp.RelievingDate >= durationFrom && emp.RelievingDate <= durationTo)
            && (emp.CurrentStatus == Convert.ToInt32(empSystemStatus) ||
                empSystemStatus.Equals(byte.MinValue.ToString()) ||
                empSystemStatus == currentStatusDesc && res.ResignationId != null && res.RelievingDate >= DateTime.UtcNow)
            && result.Contains(emp.EmpStatus.GetValueOrDefault())
            && !excludedStatuses.Contains(emp.SeperationStatus.GetValueOrDefault())
            && (probationStatus == 2 && emp.IsProbation == true ||
                probationStatus == 3 && emp.IsProbation == false ||
                probationStatus == 1 && (emp.IsProbation == true || emp.IsProbation == false))
            && emp.IsDelete.Equals(false)
        select new
        {
            EmpId = emp.EmpId,
            EmpCode = emp.EmpCode,
            Name = $"{emp.FirstName} {emp.MiddleName} {emp.LastName}",
            GuardiansName = emp.GuardiansName,
            DateOfBirth = FormatDate(emp.DateOfBirth, _employeeSettings.DateFormat),
            JoinDate = emp.JoinDt.ToString(),
            DataDate = emp.JoinDt.ToString(),
            SeperationStatus = emp.SeperationStatus,
            Gender = emp.Gender,
            WorkingStatus = emp.SeperationStatus == (int)SeparationStatus.Live ? nameof(SeparationStatus.Live) : nameof(SeparationStatus.Resigned),
            Age = CalculateAge(emp.DateOfBirth, ageFormat),
            ProbationDt = FormatDate(emp.ProbationDt, _employeeSettings.DateFormat),
            Probation = emp.IsProbation == false ? ProbationStatus.CONFIRMED : ProbationStatus.PROBATION,
            LastEntity = emp.LastEntity,
            CurrentStatus = emp.CurrentStatus,
            EmpStatus = emp.EmpStatus.ToString(),
            IsSave = emp.IsSave,
            EmpFileNumber = emp.EmpFileNumber,
            DailyRateTypeId = emp.DailyRateTypeId,
            PayrollMode = emp.PayrollMode,
            ResignationReason = res.Reason,
            ResignationDate = res.ResignationDate.ToString(),
            RelievingDate = res.RelievingDate.ToString()
        }
    )
    join addr in _context.HrEmpAddresses on emp.EmpId equals addr.EmpId into addrGroup
    from addr in addrGroup.DefaultIfEmpty()
    join pers in _context.HrEmpPersonals on emp.EmpId equals pers.EmpId into persGroup
    from pers in persGroup.DefaultIfEmpty()
    join rep in _context.HrEmpReportings on emp.EmpId equals rep.EmpId into repGroup
    from rep in repGroup.DefaultIfEmpty()
    join highView in _context.HighLevelViewTables on emp.LastEntity equals highView.LastEntityId into highViewGroup
    from highView in highViewGroup.DefaultIfEmpty()
    join img in _context.HrEmpImages on emp.EmpId equals img.EmpId into imgGroup
    from img in imgGroup.DefaultIfEmpty()
    join currStatus in _context.EmployeeCurrentStatuses on emp.CurrentStatus equals currStatus.Status into currStatusGroup
    from currStatus in currStatusGroup.DefaultIfEmpty()
    join empStatusSettings in _context.HrEmpStatusSettings on Convert.ToInt32(emp.EmpStatus) equals empStatusSettings.StatusId into empStatusGroup
    from empStatusSettings in empStatusGroup.DefaultIfEmpty()
    join reason in _context.ReasonMasters on Convert.ToInt32(emp.ResignationReason) equals reason.ReasonId into reasonGroup
    from reason in reasonGroup.DefaultIfEmpty()
    join country in _context.AdmCountryMasters on pers.Nationality equals country.CountryId into countryGroup
    from country in countryGroup.DefaultIfEmpty()
    select new EmployeeResultDto
    {
        EmpId = emp.EmpId,
        ImageUrl = img.ImageUrl,
        EmpCode = emp.EmpCode,
        Name = emp.Name,
        JoinDate = emp.JoinDate.ToString(),
        DataDate = emp.DataDate.ToString(),
        EmpStatusDesc = currStatus.StatusDesc.ToString(),
        EmpStatus = empStatusSettings.StatusDesc,
        Gender = emp.Gender.ToString(),
        SeperationStatus = emp.SeperationStatus,
        OfficialEmail = addr.OfficialEmail,
        PersonalEmail = addr.PersonalEmail,
        Phone = addr.Phone,
        MaritalStatus = pers.MaritalStatus,
        Age = emp.Age,
        ProbationDt = emp.ProbationDt.ToString(),
        LevelOneDescription = highView.LevelOneDescription,
        LevelTwoDescription = highView.LevelTwoDescription,
        ProbationStatus = emp.Probation.ToString(),
        Nationality = country.Nationality,
        IsSave = emp.IsSave,
        EmpFileNumber = emp.EmpFileNumber,
        CurrentStatus = emp.CurrentStatus
    }).ToListAsync();


            // Pagination
            var totalRecords = finalQuery.Count();
            var paginatedResult = finalQuery
                .OrderBy(x => x.EmpCode)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new PaginatedResult<EmployeeResultDto>
            {
                TotalRecords = totalRecords,
                PageNumber = pageNumber,
                PageSize = pageSize,
                Records = paginatedResult
            };
        }



        public async Task<PaginatedResult<EmployeeResultDto>> InfoFormatTwoLinkLevelExists(string? empStatus, string? empSystemStatus, string? empIds, string? filterType, DateTime? durationFrom,
DateTime? durationTo, int probationStatus, string? currentStatusDesc, string? ageFormat, int pageNumber, int pageSize)
        {


            var statusList = empStatus?.Split(',')
                                      .Select(s => int.Parse(s.Trim()))
                                      .ToList();

            var filteredStatuses = await _context.HrEmpStatusSettings
                                                 .Where(s => s.IsResignation.Equals(false))
                                                 .Select(s => s.StatusId)
                                                 .ToListAsync();

            var excludedStatuses = await _context.HrEmpStatusSettings
                                                 .Where(s => s.IsResignation.Equals(true) && !statusList.Contains(s.StatusId))
                                                 .Select(s => s.StatusId)
                                                 .ToListAsync();

            var result = statusList?.Where(item => filteredStatuses.Contains(item)).ToList();



            var finalQuery = await (
    from emp in (
        from emp in _context.HrEmpMasters
        join res in _context.Resignations
               .Where(r =>
r.CurrentRequest == 1 &&
!new[] { "D", "R" }.Contains(r.ApprovalStatus) &&
r.ApprovalStatus == (empSystemStatus == currentStatusDesc ? "P" : "A") &&
r.RejoinStatus == "P")
        on emp.EmpId equals res.EmpId into resGroup
        from res in resGroup.DefaultIfEmpty()
        where
            (durationFrom == null || durationTo == null ||
             emp.JoinDt >= durationFrom && emp.JoinDt <= durationTo ||
             emp.ProbationDt >= durationFrom && emp.ProbationDt <= durationTo ||
             emp.RelievingDate >= durationFrom && emp.RelievingDate <= durationTo)
            && (emp.CurrentStatus == Convert.ToInt32(empSystemStatus) ||
                empSystemStatus.Equals(byte.MinValue.ToString()) ||
                empSystemStatus == currentStatusDesc && res.ResignationId != null && res.RelievingDate >= DateTime.UtcNow)
            && result.Contains(emp.EmpStatus.GetValueOrDefault())
            && !excludedStatuses.Contains(emp.SeperationStatus.GetValueOrDefault())
            && (probationStatus == 2 && emp.IsProbation == true ||
                probationStatus == 3 && emp.IsProbation == false ||
                probationStatus == 1 && (emp.IsProbation == true || emp.IsProbation == false))
            && emp.IsDelete.Equals(false)
        select new
        {
            EmpId = emp.EmpId,
            EmpCode = emp.EmpCode,
            Name = $"{emp.FirstName} {emp.MiddleName} {emp.LastName}",
            GuardiansName = emp.GuardiansName,
            DateOfBirth = FormatDate(emp.DateOfBirth, _employeeSettings.DateFormat),
            JoinDate = emp.JoinDt.ToString(),
            DataDate = emp.JoinDt.ToString(),
            SeperationStatus = emp.SeperationStatus,
            Gender = emp.Gender,
            WorkingStatus = emp.SeperationStatus == (int)SeparationStatus.Live ? nameof(SeparationStatus.Live) : nameof(SeparationStatus.Resigned),
            Age = CalculateAge(emp.DateOfBirth, ageFormat),
            ProbationDt = FormatDate(emp.ProbationDt, _employeeSettings.DateFormat),
            Probation = emp.IsProbation == false ? ProbationStatus.CONFIRMED : ProbationStatus.PROBATION,
            LastEntity = emp.LastEntity,
            CurrentStatus = emp.CurrentStatus,
            EmpStatus = emp.EmpStatus.ToString(),
            IsSave = emp.IsSave,
            EmpFileNumber = emp.EmpFileNumber,
            DailyRateTypeId = emp.DailyRateTypeId,
            PayrollMode = emp.PayrollMode,
            ResignationReason = res.Reason,
            ResignationDate = res.ResignationDate.ToString(),
            RelievingDate = res.RelievingDate.ToString()
        }
    )
    join addr in _context.HrEmpAddresses on emp.EmpId equals addr.EmpId into addrGroup
    from addr in addrGroup.DefaultIfEmpty()
    join pers in _context.HrEmpPersonals on emp.EmpId equals pers.EmpId into persGroup
    from pers in persGroup.DefaultIfEmpty()
    join rep in _context.HrEmpReportings on emp.EmpId equals rep.EmpId into repGroup
    from rep in repGroup.DefaultIfEmpty()
    join highView in _context.HighLevelViewTables on emp.LastEntity equals highView.LastEntityId into highViewGroup
    from highView in highViewGroup.DefaultIfEmpty()
    join img in _context.HrEmpImages on emp.EmpId equals img.EmpId into imgGroup
    from img in imgGroup.DefaultIfEmpty()
    join currStatus in _context.EmployeeCurrentStatuses on emp.CurrentStatus equals currStatus.Status into currStatusGroup
    from currStatus in currStatusGroup.DefaultIfEmpty()
    join empStatusSettings in _context.HrEmpStatusSettings on Convert.ToInt32(emp.EmpStatus) equals empStatusSettings.StatusId into empStatusGroup
    from empStatusSettings in empStatusGroup.DefaultIfEmpty()
    join reason in _context.ReasonMasters on Convert.ToInt32(emp.ResignationReason) equals reason.ReasonId into reasonGroup
    from reason in reasonGroup.DefaultIfEmpty()
    join country in _context.AdmCountryMasters on pers.Nationality equals country.CountryId into countryGroup
    from country in countryGroup.DefaultIfEmpty()
    select new EmployeeResultDto
    {
        EmpId = emp.EmpId,
        ImageUrl = img.ImageUrl,
        EmpCode = emp.EmpCode,
        Name = emp.Name,
        JoinDate = emp.JoinDate.ToString(),
        DataDate = emp.DataDate.ToString(),
        EmpStatusDesc = currStatus.StatusDesc.ToString(),
        EmpStatus = empStatusSettings.StatusDesc,
        Gender = emp.Gender.ToString(),
        SeperationStatus = emp.SeperationStatus,
        OfficialEmail = addr.OfficialEmail,
        PersonalEmail = addr.PersonalEmail,
        Phone = addr.Phone,
        MaritalStatus = pers.MaritalStatus,
        Age = emp.Age,
        ProbationDt = emp.ProbationDt.ToString(),
        LevelOneDescription = highView.LevelOneDescription,
        LevelTwoDescription = highView.LevelTwoDescription,
        ProbationStatus = emp.Probation.ToString(),
        Nationality = country.Nationality,
        IsSave = emp.IsSave,
        EmpFileNumber = emp.EmpFileNumber,
        CurrentStatus = emp.CurrentStatus
    }).ToListAsync();


            // Pagination
            var totalRecords = finalQuery.Count();
            var paginatedResult = finalQuery
                .OrderBy(x => x.EmpCode)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new PaginatedResult<EmployeeResultDto>
            {
                TotalRecords = totalRecords,
                PageNumber = pageNumber,
                PageSize = pageSize,
                Records = paginatedResult
            };

        }



        public async Task<PaginatedResult<EmployeeResultDto>> InfoFormatThreeLinkLevelExists(string? empStatus, string? empSystemStatus, string? empIds, string? filterType, DateTime? durationFrom,
DateTime? durationTo, int probationStatus, string? currentStatusDesc, string? ageFormat, int pageNumber, int pageSize)
        {

            var statusList = empStatus?.Split(',')
                         .Select(s => int.Parse(s.Trim()))
                         .ToList();

            var filteredStatuses = await _context.HrEmpStatusSettings
                                                 .Where(s => s.IsResignation.Equals(false))
                                                 .Select(s => s.StatusId)
                                                 .ToListAsync();

            var excludedStatuses = await _context.HrEmpStatusSettings
                                                 .Where(s => s.IsResignation.Equals(true) && !statusList.Contains(s.StatusId))
                                                 .Select(s => s.StatusId)
                                                 .ToListAsync();

            var result = statusList?.Where(item => filteredStatuses.Contains(item)).ToList();



            var finalQuery = await (
    from emp in (
        from emp in _context.HrEmpMasters
        join res in _context.Resignations
               .Where(r =>
r.CurrentRequest == 1 &&
!new[] { "D", "R" }.Contains(r.ApprovalStatus) &&
r.ApprovalStatus == (empSystemStatus == currentStatusDesc ? "P" : "A") &&
r.RejoinStatus == "P")
        on emp.EmpId equals res.EmpId into resGroup
        from res in resGroup.DefaultIfEmpty()
        where
            (durationFrom == null || durationTo == null ||
             emp.JoinDt >= durationFrom && emp.JoinDt <= durationTo ||
             emp.ProbationDt >= durationFrom && emp.ProbationDt <= durationTo ||
             emp.RelievingDate >= durationFrom && emp.RelievingDate <= durationTo)
            && (emp.CurrentStatus == Convert.ToInt32(empSystemStatus) ||
                empSystemStatus.Equals(byte.MinValue.ToString()) ||
                empSystemStatus == currentStatusDesc && res.ResignationId != null && res.RelievingDate >= DateTime.UtcNow)
            && result.Contains(emp.EmpStatus.GetValueOrDefault())
            && !excludedStatuses.Contains(emp.SeperationStatus.GetValueOrDefault())
            && (probationStatus == 2 && emp.IsProbation == true ||
                probationStatus == 3 && emp.IsProbation == false ||
                probationStatus == 1 && (emp.IsProbation == true || emp.IsProbation == false))
            && emp.IsDelete.Equals(false)
        select new
        {
            EmpId = emp.EmpId,
            EmpCode = emp.EmpCode,
            Name = $"{emp.FirstName} {emp.MiddleName} {emp.LastName}",
            GuardiansName = emp.GuardiansName,
            DateOfBirth = FormatDate(emp.DateOfBirth, _employeeSettings.DateFormat),
            JoinDate = emp.JoinDt.ToString(),
            DataDate = emp.JoinDt.ToString(),
            SeperationStatus = emp.SeperationStatus,
            Gender = emp.Gender,
            WorkingStatus = emp.SeperationStatus == (int)SeparationStatus.Live ? nameof(SeparationStatus.Live) : nameof(SeparationStatus.Resigned),
            Age = CalculateAge(emp.DateOfBirth, ageFormat),
            ProbationDt = FormatDate(emp.ProbationDt, _employeeSettings.DateFormat),
            Probation = emp.IsProbation == false ? ProbationStatus.CONFIRMED : ProbationStatus.PROBATION,
            LastEntity = emp.LastEntity,
            CurrentStatus = emp.CurrentStatus,
            EmpStatus = emp.EmpStatus.ToString(),
            IsSave = emp.IsSave,
            EmpFileNumber = emp.EmpFileNumber,
            DailyRateTypeId = emp.DailyRateTypeId,
            PayrollMode = emp.PayrollMode,
            ResignationReason = res.Reason,
            ResignationDate = res.ResignationDate.ToString(),
            RelievingDate = res.RelievingDate.ToString()
        }
    )
    join addr in _context.HrEmpAddresses on emp.EmpId equals addr.EmpId into addrGroup
    from addr in addrGroup.DefaultIfEmpty()
    join pers in _context.HrEmpPersonals on emp.EmpId equals pers.EmpId into persGroup
    from pers in persGroup.DefaultIfEmpty()
    join rep in _context.HrEmpReportings on emp.EmpId equals rep.EmpId into repGroup
    from rep in repGroup.DefaultIfEmpty()
    join highView in _context.HighLevelViewTables on emp.LastEntity equals highView.LastEntityId into highViewGroup
    from highView in highViewGroup.DefaultIfEmpty()
    join img in _context.HrEmpImages on emp.EmpId equals img.EmpId into imgGroup
    from img in imgGroup.DefaultIfEmpty()
    join currStatus in _context.EmployeeCurrentStatuses on emp.CurrentStatus equals currStatus.Status into currStatusGroup
    from currStatus in currStatusGroup.DefaultIfEmpty()
    join empStatusSettings in _context.HrEmpStatusSettings on Convert.ToInt32(emp.EmpStatus) equals empStatusSettings.StatusId into empStatusGroup
    from empStatusSettings in empStatusGroup.DefaultIfEmpty()
    join reason in _context.ReasonMasters on Convert.ToInt32(emp.ResignationReason) equals reason.ReasonId into reasonGroup
    from reason in reasonGroup.DefaultIfEmpty()
    join country in _context.AdmCountryMasters on pers.Nationality equals country.CountryId into countryGroup
    from country in countryGroup.DefaultIfEmpty()
    select new EmployeeResultDto
    {
        EmpId = emp.EmpId,
        ImageUrl = img.ImageUrl,
        EmpCode = emp.EmpCode,
        Name = emp.Name,
        JoinDate = emp.JoinDate.ToString(),
        DataDate = emp.DataDate.ToString(),
        EmpStatusDesc = currStatus.StatusDesc.ToString(),
        EmpStatus = empStatusSettings.StatusDesc,
        Gender = emp.Gender.ToString(),
        SeperationStatus = emp.SeperationStatus,
        OfficialEmail = addr.OfficialEmail,
        PersonalEmail = addr.PersonalEmail,
        Phone = addr.Phone,
        MaritalStatus = pers.MaritalStatus,
        Age = emp.Age,
        ProbationDt = emp.ProbationDt.ToString(),
        LevelOneDescription = highView.LevelOneDescription,
        LevelTwoDescription = highView.LevelTwoDescription,
        ProbationStatus = emp.Probation.ToString(),
        Nationality = country.Nationality,
        IsSave = emp.IsSave,
        EmpFileNumber = emp.EmpFileNumber,
        CurrentStatus = emp.CurrentStatus
    }).ToListAsync();


            // Pagination
            var totalRecords = finalQuery.Count();
            var paginatedResult = finalQuery
                .OrderBy(x => x.EmpCode)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new PaginatedResult<EmployeeResultDto>
            {
                TotalRecords = totalRecords,
                PageNumber = pageNumber,
                PageSize = pageSize,
                Records = paginatedResult
            };
        }

        public async Task<PaginatedResult<EmployeeResultDto>> InfoFormatFourLinkLevelExists(string? empStatus, string? empSystemStatus, string? empIds, string? filterType, DateTime? durationFrom,
DateTime? durationTo, int probationStatus, string? currentStatusDesc, string? ageFormat, int pageNumber, int pageSize)
        {

            var result = await (
              from emp in _context.EmployeeDetails
              join docApp in _context.HrmsEmpdocumentsApproved00s on emp.EmpId equals docApp.EmpId
              join docDetail in _context.HrmsEmpdocumentsApproved01s on docApp.DetailId equals docDetail.DetailId
              join docField in _context.HrmsDocumentField00s on docDetail.DocFields equals Convert.ToInt32(docField.DocFieldId) into docFieldGroup
              from docField in docFieldGroup.DefaultIfEmpty()
              join doc in _context.HrmsDocument00s on docField.DocId equals Convert.ToInt32(doc.DocId)
              where (doc.DocType == 5 || doc.DocType == 3) && docApp.Status != "D"
              select new
              {
                  emp.EmpId,
                  emp.EmpCode,
                  EmployeeName = emp.Name,
                  docField.DocDescription,
                  docDetail.DocValues,

              }).ToListAsync();

            var pivotedResult = result
                .GroupBy(r => new { r.EmpId, r.EmpCode, r.EmployeeName, })
                .Select(g => new
                {
                    g.Key.EmpId,
                    g.Key.EmpCode,
                    g.Key.EmployeeName,
                    //AccountNumber = g.FirstOrDefault(x => x.DocDescription == "ACCOUNT NUMBER")?.DocValues,
                    //BankName = g.FirstOrDefault(x => x.DocDescription == "BANK NAME")?.DocValues,
                    //IFSCCode = g.FirstOrDefault(x => x.DocDescription == "IFSC CODE")?.DocValues,
                    //IsActive = g.FirstOrDefault(x => x.DocDescription == "IsActive")?.DocValues,
                    //Name = g.FirstOrDefault(x => x.DocDescription == "NAME")?.DocValues,
                    //PaymentType = g.FirstOrDefault(x => x.DocDescription == "PAYMENT TYPE")?.DocValues,
                    //AdharCardNumber = g.FirstOrDefault(x => x.DocDescription == "Adhar Card Number")?.DocValues,
                    //PANNumber = g.FirstOrDefault(x => x.DocDescription == "PAN Number")?.DocValues,
                    //UANNumber = g.FirstOrDefault(x => x.DocDescription == "UAN Number")?.DocValues,
                    //PFAccountNumber = g.FirstOrDefault(x => x.DocDescription == "PF ACCOUNT NUMBER")?.DocValues,
                    //ACCOUNT_NUMBER = g.de.ContainsKey("ACCOUNT NUMBER") ? r.Details["ACCOUNT NUMBER"] : null,
                    //BANK_NAME = r.Details.ContainsKey("BANK NAME") ? r.Details["BANK NAME"] : null,
                    //IFSC_CODE = r.Details.ContainsKey("IFSC CODE") ? r.Details["IFSC CODE"] : null,
                    //IsActive = r.Details.ContainsKey("IsActive") ? r.Details["IsActive"] : null,
                    //NAME = r.Details.ContainsKey("NAME") ? r.Details["NAME"] : null,
                    //PAYMENT_TYPE = r.Details.ContainsKey("PAYMENT TYPE") ? r.Details["PAYMENT TYPE"] : null,
                    //Adhar_Card_Number = r.Details.ContainsKey("Adhar Card Number") ? r.Details["Adhar Card Number"] : null,
                    //PAN_Number = r.Details.ContainsKey("PAN Number") ? r.Details["PAN Number"] : null,
                    //UAN_Number = r.Details.ContainsKey("UAN Number") ? r.Details["UAN Number"] : null,
                    //PF_ACCOUNT_NUMBER = r.Details.ContainsKey("PF ACCOUNT NUMBER") ? r.Details["PF ACCOUNT NUMBER"] : null
                })
                .ToList();







            var statusList = empStatus.Split(',').Select(s => int.Parse(s.Trim())).ToList();

            var filteredStatuses = await _context.HrEmpStatusSettings
                                .Where(s => s.IsResignation != true)
                                .Select(s => s.StatusId)
                                .ToListAsync();

            var excludedStatuses = await _context.HrEmpStatusSettings
                                .Where(s => s.IsResignation == true && !statusList.Contains(s.StatusId))
                                .Select(s => s.StatusId)
                                .ToListAsync();

            var result12 = statusList
                                .Where(item => filteredStatuses.Contains(item))
                                .ToList();



            var cteEmployeeDetails = await (
                from emp in _context.HrEmpMasters
                join res in _context.Resignations
                on emp.EmpId equals res.EmpId into resGroup
                from res in resGroup.DefaultIfEmpty()
                where
(durationFrom == null || durationTo == null || emp.JoinDt >= durationFrom && emp.JoinDt <= durationTo || durationFrom == null || durationTo == null || emp.ProbationDt >= durationFrom && emp.ProbationDt <= durationTo || durationFrom == null || durationTo == null || emp.RelievingDate >= durationFrom && emp.RelievingDate <= durationTo)
                        && (emp.CurrentStatus == Convert.ToInt32(empSystemStatus) || empSystemStatus == "0"
                        || empSystemStatus == currentStatusDesc && res.ResignationId != null && res.RelievingDate >= DateTime.UtcNow)
                        && result12.Contains(emp.EmpStatus.GetValueOrDefault())
                        && !excludedStatuses.Contains(emp.SeperationStatus.GetValueOrDefault())
                        && (probationStatus == 2 && emp.IsProbation == true ||
                            probationStatus == 3 && emp.IsProbation == false ||
                            probationStatus == 1 && (emp.IsProbation == true || emp.IsProbation == false))
                        && emp.IsDelete == false
                select new
                {
                    emp.EmpId,
                    emp.EmpCode,
                    Name = $"{emp.FirstName} {emp.MiddleName} {emp.LastName}",
                    emp.GuardiansName,
                    //DateOfBirth = emp.DateOfBirth.ToString("dd/MM/yyyy"),
                    //JoinDate = emp.Join_Dt.ToString("dd/MM/yyyy"),
                    //DataDate = emp.Join_Dt.ToString("yyyyMMdd"),
                    emp.DateOfBirth,
                    JoinDate = emp.JoinDt,
                    DataDate = emp.JoinDt,
                    emp.SeperationStatus,
                    //Gender = emp.Gender == "M" ? "Male" : emp.Gender == "F" ? "Female" : emp.Gender == "O" ? "Other" : "NA",
                    Gender = GetGender(emp.Gender),
                    //WorkingStatus = emp.SeperationStatus == 0 ? "Live" : "Resigned",
                    WorkingStatus = emp.SeperationStatus == (int)SeparationStatus.Live ? nameof(SeparationStatus.Live) : nameof(SeparationStatus.Resigned),
                    Age = CalculateAge(emp.DateOfBirth, ageFormat),
                    //Probation_Dt = emp.Probation_Dt.ToString("dd/MM/yyyy"),
                    Probation_Dt = emp.ProbationDt,
                    // Probation = emp.IsProbation == false ? "CONFIRMED" : "PROBATION",
                    Probation = emp.IsProbation == false ? ProbationStatus.CONFIRMED : ProbationStatus.PROBATION,
                    emp.LastEntity,
                    emp.CurrentStatus,
                    emp.EmpStatus,
                    emp.IsSave,
                    emp.EmpFileNumber,
                    emp.DailyRateTypeId,
                    emp.PayrollMode,
                    res.Reason,
                    res.ResignationDate,
                    res.RelievingDate,
                    res.RelievingType,
                    res.ResignationType,


                }).ToListAsync();

            #region ctePayscale


            var ctePayscale =
                from pay in _context.Payscale00s
                where pay.EffectiveFrom <= DateTime.UtcNow
                group pay by pay.EmployeeId into payGroup
                select new
                {
                    EmployeeId = payGroup.Key,
                    payGroup.OrderByDescending(p => p.EffectiveFrom).First().TotalPay
                };
            #endregion

            #region finalQuery



            var baseDetailsQuery =
                    from emp in cteEmployeeDetails
                    join img in _context.HrEmpImages on emp.EmpId equals img.EmpId into imgGroup
                    from img in imgGroup.DefaultIfEmpty()
                    join currStatus in _context.EmployeeCurrentStatuses on emp.CurrentStatus equals currStatus.Status into currStatusGroup
                    from currStatus in currStatusGroup.DefaultIfEmpty()
                    join empStatusSettings in _context.HrEmpStatusSettings on emp.EmpStatus equals empStatusSettings.StatusId into empStatusGroup
                    from empStatusSettings in empStatusGroup.DefaultIfEmpty()
                    select new
                    {
                        emp.EmpId,
                        emp.EmpCode,
                        emp.Name,
                        emp.CurrentStatus,
                        emp.ResignationDate,
                        emp.JoinDate,
                        emp.Gender,
                        emp.IsSave,
                        img.ImageUrl,
                        currStatus.StatusDesc,
                        emp.GuardiansName
                        //empStatusSettings.StatusDesc
                    };
            var addressDetailsQuery =
                        from emp in cteEmployeeDetails
                        join addr in _context.HrEmpAddresses on emp.EmpId equals addr.EmpId into addrGroup
                        from addr in addrGroup.DefaultIfEmpty()
                        join address01 in _context.HrEmpAddress01s on emp.EmpId equals address01.EmpId into AddressGroup01
                        from address01 in AddressGroup01.DefaultIfEmpty()
                        select new
                        {
                            emp.EmpId,
                            addr.OfficialEmail,
                            addr.PersonalEmail,
                            addr.Phone
                        };

            var reportingQuery =
                        from emp in cteEmployeeDetails
                        join rep in _context.HrEmpReportings on emp.EmpId equals rep.EmpId into repGroup
                        from rep in repGroup.DefaultIfEmpty()
                        join repDet in _context.EmployeeDetails on rep.ReprotToWhome equals repDet.EmpId into repDetGroup
                        from repDet in repDetGroup.DefaultIfEmpty()
                        join highView in _context.HighLevelViewTables on emp.LastEntity equals highView.LastEntityId into highViewGroup
                        from highView in highViewGroup.DefaultIfEmpty()
                        select new
                        {
                            emp.EmpId,
                            repDet.EmpCode,
                            repDet.Name,
                            highView.LevelOneDescription,
                            highView.LevelTwoDescription,
                            highView.LevelThreeDescription
                        };


            var resultFour = from baseDetails in baseDetailsQuery
                             join addressDetails in addressDetailsQuery on baseDetails.EmpId equals addressDetails.EmpId into addressGroup
                             from addressDetails in addressGroup.DefaultIfEmpty()
                             join reportingDetails in reportingQuery on baseDetails.EmpId equals reportingDetails.EmpId into reportingGroup
                             from reportingDetails in reportingGroup.DefaultIfEmpty()
                             select new EmployeeResultDto
                             {
                                 EmpId = baseDetails.EmpId,
                                 ImageUrl = baseDetails.ImageUrl,
                                 EmpCode = baseDetails.EmpCode,
                                 Name = baseDetails.Name,
                                 GuardiansName = baseDetails.GuardiansName,
                                 Gender = baseDetails.Gender.ToString(),
                                 OfficialEmail = addressDetails.OfficialEmail,
                                 PersonalEmail = addressDetails.PersonalEmail,
                                 Phone = addressDetails.Phone,
                                 ReportingEmployeeCode = reportingDetails.EmpCode,
                                 ReportingEmployeeName = reportingDetails.Name,
                                 LevelOneDescription = reportingDetails.LevelOneDescription,
                                 //        EmpId = emp.EmpId,
                                 //        ImageUrl = img.ImageUrl,
                                 //        EmpCode = emp.EmpCode,
                                 //        Name = emp.Name,
                                 //        GuardiansName = emp.GuardiansName,
                                 //        DateOfBirth = emp.DateOfBirth,
                                 //        JoinDate = emp.JoinDate,
                                 //        DataDate = emp.DataDate,
                                 //        EmpStatusDesc = currStatus.StatusDesc,
                                 //        EmpStatus = empStatusSettings.StatusDesc,
                                 //        Gender = emp.Gender,
                                 //        ResignationDate = emp.ResignationDate,

                                 //        SeperationStatus = emp.SeperationStatus,
                                 //        OfficialEmail = addr.OfficialEmail,
                                 //        PersonalEmail = addr.PersonalEmail,
                                 //        Phone = addr.Phone,
                                 //        MaritalStatus = pers.MaritalStatus,
                                 //        Age = emp.Age,
                                 //        ProbationDt = emp.Probation_Dt,
                                 //        LevelOneDescription = highView.LevelOneDescription,
                                 //        LevelTwoDescription = highView.LevelTwoDescription,
                                 //        LevelThreeDescription = highView.LevelThreeDescription,
                                 //        LevelFourDescription = highView.LevelFourDescription,
                                 //        LevelFiveDescription = highView.LevelFiveDescription,
                                 //        LevelSixDescription = highView.LevelSixDescription,
                                 //        LevelSevenDescription = highView.LevelSevenDescription,
                                 //        LevelEightDescription = highView.LevelEightDescription,
                                 //        LevelNineDescription = highView.LevelNineDescription,
                                 //        LevelTenDescription = highView.LevelTenDescription,
                                 //        LevelElevenDescription = highView.LevelElevenDescription,
                                 //        Description = rm.Description,
                                 //        ResignationType = emp.ResignationType,
                                 //        ProbationStatus = emp.Probation,
                                 //        Nationality = country.Nationality,
                                 //        IsSave = emp.IsSave,
                                 //        EmpFileNumber = emp.EmpFileNumber,
                                 //        ReligionName = religion.ReligionName,
                                 //        BloodGroup = pers.BloodGrp,
                                 //        ReportingEmployeeCode = repDet.EmpCode,
                                 //        ReportingEmployeeName = repDet.Name,
                                 //        WorkingStatus = emp.WorkingStatus,
                                 //        RelievingDate = emp.RelievingDate,
                                 //        //SalaryType = emp.PayrollMode == 1 || emp.PayrollMode == 0 ? "Monthly Wage" : "Daily Wage",
                                 //        //GrossPay = pay != null ? pay.TotalPay : 0,
                                 //        CurrentStatus = emp.CurrentStatus
                                 // Add other fields as needed
                             };




            #endregion


            // Apply pagination
            var totalRecords = resultFour.Count();
            var paginatedResult = resultFour
                .OrderBy(x => x.EmpCode)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // Return paginated result
            return new PaginatedResult<EmployeeResultDto>
            {
                TotalRecords = totalRecords,
                PageNumber = pageNumber,
                PageSize = pageSize,
                Records = paginatedResult
            };

        }




        private async Task<PaginatedResult<EmployeeResultDto>> InforFormatOneOrZeroNotExistLinkSelect(DateTime? durationFrom, DateTime? durationTo, string EmpStatus, List<string>? empIdList, int pageNumber, int pageSize, string? ageFormat)
        {
            var statusList = EmpStatus.Split(',').Select(s => int.Parse(s.Trim())).ToList();

            var filteredStatuses = await _context.HrEmpStatusSettings
         .Where(s => s.IsResignation != true)
         .Select(s => s.StatusId)
         .ToListAsync();


            var result = statusList
                                .Where(item => filteredStatuses.Contains(item))
                                .ToList();
            HashSet<int> empIdSet = empIdList.Select(int.Parse).ToHashSet();
            var finalQuery = await (
            from a in _context.HrEmpMasters
            where
                   empIdSet.Contains(a.EmpId) &&
                  a.IsDelete == false
                  && (durationFrom == null || durationTo == null ||
                      a.JoinDt >= durationFrom && a.JoinDt <= durationTo ||
                      a.ProbationDt >= durationFrom && a.ProbationDt <= durationTo ||
                      a.RelievingDate >= durationFrom && a.RelievingDate <= durationTo)
            join b in _context.HrEmpAddresses on a.EmpId equals b.EmpId into addrGroup
            from b in addrGroup.DefaultIfEmpty()
            join c in _context.HrEmpPersonals on a.EmpId equals c.EmpId into persGroup
            from c in persGroup.DefaultIfEmpty()
            join rep in _context.HrEmpReportings on a.EmpId equals rep.EmpId into repGroup
            from rep in repGroup.DefaultIfEmpty()
            join repDet in _context.EmployeeDetails on rep.ReprotToWhome equals repDet.EmpId into repDetGroup
            from repDet in repDetGroup.DefaultIfEmpty()
            join f in _context.HighLevelViewTables on a.LastEntity equals f.LastEntityId into highViewGroup
            from f in highViewGroup.DefaultIfEmpty()
            join img in _context.HrEmpImages on a.EmpId equals img.EmpId into imgGroup
            from img in imgGroup.DefaultIfEmpty()
            join j in _context.EmployeeCurrentStatuses on a.CurrentStatus equals j.Status into currStatusGroup
            from j in currStatusGroup.DefaultIfEmpty()
            join s in _context.HrEmpStatusSettings on a.EmpStatus equals s.StatusId into empStatusGroup
            from s in empStatusGroup.DefaultIfEmpty()
            join t in _context.Resignations.Where(x => x.CurrentRequest == 1 && !new[] { "D", "R" }.Contains(x.ApprovalStatus)) on a.EmpId equals t.EmpId into tempResignations
            from t in tempResignations.DefaultIfEmpty()
            join reason in _context.ReasonMasters.Where(r => r.Type == _employeeSettings.ReasonType) on Convert.ToInt32(t.Reason) equals reason.ReasonId into reasonGroup
            from rm in reasonGroup.DefaultIfEmpty()
            join cm in _context.AdmCountryMasters on c.Nationality equals cm.CountryId into countryGroup
            from cm in countryGroup.DefaultIfEmpty()
            join religion in _context.AdmReligionMasters on c.Religion equals religion.ReligionId into religionGroup
            from religion in religionGroup.DefaultIfEmpty()
            where result.Contains(a.EmpStatus.GetValueOrDefault())

            select new EmployeeResultDto
            {
                EmpId = a.EmpId,
                EmpCode = a.EmpCode,
                Name = $"{a.FirstName} {a.MiddleName} {a.LastName}",
                GuardiansName = a.GuardiansName,
                DateOfBirth = FormatDate(a.DateOfBirth, _employeeSettings.DateFormat),
                JoinDate = a.JoinDt.ToString(),
                WeddingDate = c.WeddingDate,

                //WeddingDate = c?.WeddingDate.ToString("dd/MM/yyyy"),

                DataDate = a.JoinDt.ToString(),

                StatusDesc = j.StatusDesc,
                EmpStatusDesc = s.StatusDesc,
                Gender = a.Gender.ToString(),
                //Resignation_Date = a?.ResignationDate.ToString("dd/MM/yyyy"),
                ResignationDate = FormatDate(t.ResignationDate, _employeeSettings.DateFormat),
                //RelievingDate = a?.RelievingDate.ToString("dd/MM/yyyy"),
                RelievingDate = FormatDate(a.RelievingDate, _employeeSettings.DateFormat),
                SeperationStatus = a.SeperationStatus,
                OfficialEmail = b.OfficialEmail ?? "NA",
                PersonalEmail = b.PersonalEmail,
                Phone = b.Phone,
                MaritalStatus = c.MaritalStatus == "S" ? "Single" :
                                      c.MaritalStatus == "M" ? "Married" :
                                      c.MaritalStatus == "W" ? "Widowed" :
                                      c.MaritalStatus == "X" ? "Separated" :
                                      c.MaritalStatus == "D" ? "Divorcee" :
                                      "NA",
                Age = CalculateAge(a.DateOfBirth, ageFormat),
                ProbationDt = a.ProbationDt.ToString(),
                LevelOneDescription = f.LevelOneDescription,
                LevelTwoDescription = f.LevelTwoDescription,
                LevelThreeDescription = f.LevelThreeDescription,
                LevelFourDescription = f.LevelFourDescription,
                LevelFiveDescription = f.LevelFiveDescription,
                LevelSixDescription = f.LevelSixDescription,
                LevelSevenDescription = f.LevelSevenDescription,
                LevelEightDescription = f.LevelEightDescription,
                LevelNineDescription = f.LevelNineDescription,
                LevelTenDescription = f.LevelTenDescription,
                LevelElevenDescription = f.LevelElevenDescription,
                ResignationReason = rm.Description,
                ResignationType = t.ResignationType,// a.ResignationType.ToString(),
                ProbationStatus = a.IsProbation.ToString(),//  == false ? ProbationStatus.CONFIRMED : ProbationStatus.PROBATION,

                Nationality = cm.CountryName,
                IsSave = a.IsSave,
                EmpFileNumber = a.EmpFileNumber,
                CurrentStatus = a.CurrentStatus,


            }).ToListAsync();

            var totalRecords = finalQuery.Count();
            var paginatedResult = finalQuery
                .OrderBy(x => x.EmpCode)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // Return paginated result
            return new PaginatedResult<EmployeeResultDto>
            {
                TotalRecords = totalRecords,
                PageNumber = pageNumber,
                PageSize = pageSize,
                Records = paginatedResult
            };
        }

        private async Task<PaginatedResult<EmployeeResultDto>> InforFormatTwoNotExistLinkSelect(DateTime? durationFrom, DateTime? durationTo, string? EmpStatus, List<string>? empIdList, int pageNumber, int pageSize, string? ageFormat)
        {
            var statusList = EmpStatus.Split(',').Select(s => int.Parse(s.Trim())).ToList();

            var filteredStatuses = await _context.HrEmpStatusSettings
         .Where(s => s.IsResignation != true)
         .Select(s => s.StatusId)
         .ToListAsync();


            var result = statusList
                                .Where(item => filteredStatuses.Contains(item))
                                .ToList();
            HashSet<int> empIdSet = empIdList.Select(int.Parse).ToHashSet();
            var finalQuery = await (
            from a in _context.HrEmpMasters
            where
                   empIdSet.Contains(a.EmpId) &&
                  a.IsDelete == false
                  && (durationFrom == null || durationTo == null ||
                      a.JoinDt >= durationFrom && a.JoinDt <= durationTo ||
                      a.ProbationDt >= durationFrom && a.ProbationDt <= durationTo ||
                      a.RelievingDate >= durationFrom && a.RelievingDate <= durationTo)
            join b in _context.HrEmpAddresses on a.EmpId equals b.EmpId into addrGroup
            from b in addrGroup.DefaultIfEmpty()
            join c in _context.HrEmpPersonals on a.EmpId equals c.EmpId into persGroup
            from c in persGroup.DefaultIfEmpty()
            join rep in _context.HrEmpReportings on a.EmpId equals rep.EmpId into repGroup
            from rep in repGroup.DefaultIfEmpty()
            join repDet in _context.EmployeeDetails on rep.ReprotToWhome equals repDet.EmpId into repDetGroup
            from repDet in repDetGroup.DefaultIfEmpty()
            join f in _context.HighLevelViewTables on a.LastEntity equals f.LastEntityId into highViewGroup
            from f in highViewGroup.DefaultIfEmpty()
            join img in _context.HrEmpImages on a.EmpId equals img.EmpId into imgGroup
            from img in imgGroup.DefaultIfEmpty()
            join j in _context.EmployeeCurrentStatuses on a.CurrentStatus equals j.Status into currStatusGroup
            from j in currStatusGroup.DefaultIfEmpty()
            join s in _context.HrEmpStatusSettings on a.EmpStatus equals s.StatusId into empStatusGroup
            from s in empStatusGroup.DefaultIfEmpty()
            join t in _context.Resignations.Where(x => x.CurrentRequest == 1 && !new[] { "D", "R" }.Contains(x.ApprovalStatus)) on a.EmpId equals t.EmpId into tempResignations
            from t in tempResignations.DefaultIfEmpty()
            join reason in _context.ReasonMasters.Where(r => r.Type == _employeeSettings.ReasonType) on Convert.ToInt32(t.Reason) equals reason.ReasonId into reasonGroup
            from rm in reasonGroup.DefaultIfEmpty()
            join cm in _context.AdmCountryMasters on c.Nationality equals cm.CountryId into countryGroup
            from cm in countryGroup.DefaultIfEmpty()
            join religion in _context.AdmReligionMasters on c.Religion equals religion.ReligionId into religionGroup
            from religion in religionGroup.DefaultIfEmpty()
            where result.Contains(a.EmpStatus.GetValueOrDefault())

            select new EmployeeResultDto
            {
                EmpId = a.EmpId,
                EmpCode = a.EmpCode,
                Name = $"{a.FirstName} {a.MiddleName} {a.LastName}",
                GuardiansName = a.GuardiansName,
                DateOfBirth = FormatDate(a.DateOfBirth, _employeeSettings.DateFormat),
                JoinDate = a.JoinDt.ToString(),
                WeddingDate = c.WeddingDate,

                //WeddingDate = c?.WeddingDate.ToString("dd/MM/yyyy"),

                DataDate = a.JoinDt.ToString(),

                StatusDesc = j.StatusDesc,
                EmpStatusDesc = s.StatusDesc,
                Gender = a.Gender.ToString(),
                //Resignation_Date = a?.ResignationDate.ToString("dd/MM/yyyy"),
                ResignationDate = FormatDate(t.ResignationDate, _employeeSettings.DateFormat),
                //RelievingDate = a?.RelievingDate.ToString("dd/MM/yyyy"),
                RelievingDate = FormatDate(a.RelievingDate, _employeeSettings.DateFormat),
                SeperationStatus = a.SeperationStatus,
                OfficialEmail = b.OfficialEmail ?? "NA",
                PersonalEmail = b.PersonalEmail,
                Phone = b.Phone,
                MaritalStatus = c.MaritalStatus == "S" ? "Single" :
                                      c.MaritalStatus == "M" ? "Married" :
                                      c.MaritalStatus == "W" ? "Widowed" :
                                      c.MaritalStatus == "X" ? "Separated" :
                                      c.MaritalStatus == "D" ? "Divorcee" :
                                      "NA",
                Age = CalculateAge(a.DateOfBirth, ageFormat),
                ProbationDt = a.ProbationDt.ToString(),
                LevelOneDescription = f.LevelOneDescription,
                LevelTwoDescription = f.LevelTwoDescription,
                LevelThreeDescription = f.LevelThreeDescription,
                LevelFourDescription = f.LevelFourDescription,
                LevelFiveDescription = f.LevelFiveDescription,
                LevelSixDescription = f.LevelSixDescription,
                LevelSevenDescription = f.LevelSevenDescription,
                LevelEightDescription = f.LevelEightDescription,
                LevelNineDescription = f.LevelNineDescription,
                LevelTenDescription = f.LevelTenDescription,
                LevelElevenDescription = f.LevelElevenDescription,
                ResignationReason = rm.Description,
                ResignationType = t.ResignationType,// a.ResignationType.ToString(),
                ProbationStatus = a.IsProbation.ToString(),//  == false ? ProbationStatus.CONFIRMED : ProbationStatus.PROBATION,

                Nationality = cm.CountryName,
                IsSave = a.IsSave,
                EmpFileNumber = a.EmpFileNumber,
                CurrentStatus = a.CurrentStatus,


            }).ToListAsync();

            var totalRecords = finalQuery.Count();
            var paginatedResult = finalQuery
                .OrderBy(x => x.EmpCode)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // Return paginated result
            return new PaginatedResult<EmployeeResultDto>
            {
                TotalRecords = totalRecords,
                PageNumber = pageNumber,
                PageSize = pageSize,
                Records = paginatedResult
            };
        }

        private async Task<PaginatedResult<EmployeeResultDto>> InforFormatThreeNotExistLinkSelect(DateTime? durationFrom, DateTime? durationTo, string? EmpStatus, List<string>? empIdList, int pageNumber, int pageSize, string? ageFormat)
        {
            var statusList = EmpStatus.Split(',').Select(s => int.Parse(s.Trim())).ToList();

            var filteredStatuses = await _context.HrEmpStatusSettings
         .Where(s => s.IsResignation != true)
         .Select(s => s.StatusId)
         .ToListAsync();


            var result = statusList
                                .Where(item => filteredStatuses.Contains(item))
                                .ToList();
            HashSet<int> empIdSet = empIdList.Select(int.Parse).ToHashSet();
            var finalQuery = await (
            from a in _context.HrEmpMasters
            where
                   empIdSet.Contains(a.EmpId) &&
                  a.IsDelete == false
                  && (durationFrom == null || durationTo == null ||
                      a.JoinDt >= durationFrom && a.JoinDt <= durationTo ||
                      a.ProbationDt >= durationFrom && a.ProbationDt <= durationTo ||
                      a.RelievingDate >= durationFrom && a.RelievingDate <= durationTo)
            join b in _context.HrEmpAddresses on a.EmpId equals b.EmpId into addrGroup
            from b in addrGroup.DefaultIfEmpty()
            join c in _context.HrEmpPersonals on a.EmpId equals c.EmpId into persGroup
            from c in persGroup.DefaultIfEmpty()
            join rep in _context.HrEmpReportings on a.EmpId equals rep.EmpId into repGroup
            from rep in repGroup.DefaultIfEmpty()
            join repDet in _context.EmployeeDetails on rep.ReprotToWhome equals repDet.EmpId into repDetGroup
            from repDet in repDetGroup.DefaultIfEmpty()
            join f in _context.HighLevelViewTables on a.LastEntity equals f.LastEntityId into highViewGroup
            from f in highViewGroup.DefaultIfEmpty()
            join img in _context.HrEmpImages on a.EmpId equals img.EmpId into imgGroup
            from img in imgGroup.DefaultIfEmpty()
            join j in _context.EmployeeCurrentStatuses on a.CurrentStatus equals j.Status into currStatusGroup
            from j in currStatusGroup.DefaultIfEmpty()
            join s in _context.HrEmpStatusSettings on a.EmpStatus equals s.StatusId into empStatusGroup
            from s in empStatusGroup.DefaultIfEmpty()
            join t in _context.Resignations.Where(x => x.CurrentRequest == 1 && !new[] { "D", "R" }.Contains(x.ApprovalStatus)) on a.EmpId equals t.EmpId into tempResignations
            from t in tempResignations.DefaultIfEmpty()
            join reason in _context.ReasonMasters.Where(r => r.Type == _employeeSettings.ReasonType) on Convert.ToInt32(t.Reason) equals reason.ReasonId into reasonGroup
            from rm in reasonGroup.DefaultIfEmpty()
            join cm in _context.AdmCountryMasters on c.Nationality equals cm.CountryId into countryGroup
            from cm in countryGroup.DefaultIfEmpty()
            join religion in _context.AdmReligionMasters on c.Religion equals religion.ReligionId into religionGroup
            from religion in religionGroup.DefaultIfEmpty()
            where result.Contains(a.EmpStatus.GetValueOrDefault())

            select new EmployeeResultDto
            {
                EmpId = a.EmpId,
                EmpCode = a.EmpCode,
                Name = $"{a.FirstName} {a.MiddleName} {a.LastName}",
                GuardiansName = a.GuardiansName,
                DateOfBirth = FormatDate(a.DateOfBirth, _employeeSettings.DateFormat),
                JoinDate = a.JoinDt.ToString(),
                WeddingDate = c.WeddingDate,

                //WeddingDate = c?.WeddingDate.ToString("dd/MM/yyyy"),

                DataDate = a.JoinDt.ToString(),

                StatusDesc = j.StatusDesc,
                EmpStatusDesc = s.StatusDesc,
                Gender = a.Gender.ToString(),
                //Resignation_Date = a?.ResignationDate.ToString("dd/MM/yyyy"),
                ResignationDate = FormatDate(t.ResignationDate, _employeeSettings.DateFormat),
                //RelievingDate = a?.RelievingDate.ToString("dd/MM/yyyy"),
                RelievingDate = FormatDate(a.RelievingDate, _employeeSettings.DateFormat),
                SeperationStatus = a.SeperationStatus,
                OfficialEmail = b.OfficialEmail ?? "NA",
                PersonalEmail = b.PersonalEmail,
                Phone = b.Phone,
                MaritalStatus = c.MaritalStatus == "S" ? "Single" :
                                      c.MaritalStatus == "M" ? "Married" :
                                      c.MaritalStatus == "W" ? "Widowed" :
                                      c.MaritalStatus == "X" ? "Separated" :
                                      c.MaritalStatus == "D" ? "Divorcee" :
                                      "NA",
                Age = CalculateAge(a.DateOfBirth, ageFormat),
                ProbationDt = a.ProbationDt.ToString(),
                LevelOneDescription = f.LevelOneDescription,
                LevelTwoDescription = f.LevelTwoDescription,
                LevelThreeDescription = f.LevelThreeDescription,
                LevelFourDescription = f.LevelFourDescription,
                LevelFiveDescription = f.LevelFiveDescription,
                LevelSixDescription = f.LevelSixDescription,
                LevelSevenDescription = f.LevelSevenDescription,
                LevelEightDescription = f.LevelEightDescription,
                LevelNineDescription = f.LevelNineDescription,
                LevelTenDescription = f.LevelTenDescription,
                LevelElevenDescription = f.LevelElevenDescription,
                ResignationReason = rm.Description,
                ResignationType = t.ResignationType,// a.ResignationType.ToString(),
                ProbationStatus = a.IsProbation.ToString(),//  == false ? ProbationStatus.CONFIRMED : ProbationStatus.PROBATION,

                Nationality = cm.CountryName,
                IsSave = a.IsSave,
                EmpFileNumber = a.EmpFileNumber,
                CurrentStatus = a.CurrentStatus,


            }).ToListAsync();

            var totalRecords = finalQuery.Count();
            var paginatedResult = finalQuery
                .OrderBy(x => x.EmpCode)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // Return paginated result
            return new PaginatedResult<EmployeeResultDto>
            {
                TotalRecords = totalRecords,
                PageNumber = pageNumber,
                PageSize = pageSize,
                Records = paginatedResult
            };
        }

        private async Task<PaginatedResult<EmployeeResultDto>> InforFormatFourNotExistLinkSelect(DateTime? durationFrom, DateTime? durationTo, string? EmpStatus, List<string>? empIdList, int pageNumber, int pageSize, string? ageFormat)
        {
            var statusList = EmpStatus.Split(',').Select(s => int.Parse(s.Trim())).ToList();

            var filteredStatuses = await _context.HrEmpStatusSettings
         .Where(s => s.IsResignation != true)
         .Select(s => s.StatusId)
         .ToListAsync();


            var result = statusList
                                .Where(item => filteredStatuses.Contains(item))
                                .ToList();
            HashSet<int> empIdSet = empIdList.Select(int.Parse).ToHashSet();
            var finalQuery = await (
            from a in _context.HrEmpMasters
            where
                   empIdSet.Contains(a.EmpId) &&
                  a.IsDelete == false
                  && (durationFrom == null || durationTo == null ||
                      a.JoinDt >= durationFrom && a.JoinDt <= durationTo ||
                      a.ProbationDt >= durationFrom && a.ProbationDt <= durationTo ||
                      a.RelievingDate >= durationFrom && a.RelievingDate <= durationTo)
            join b in _context.HrEmpAddresses on a.EmpId equals b.EmpId into addrGroup
            from b in addrGroup.DefaultIfEmpty()
            join c in _context.HrEmpPersonals on a.EmpId equals c.EmpId into persGroup
            from c in persGroup.DefaultIfEmpty()
            join rep in _context.HrEmpReportings on a.EmpId equals rep.EmpId into repGroup
            from rep in repGroup.DefaultIfEmpty()
            join repDet in _context.EmployeeDetails on rep.ReprotToWhome equals repDet.EmpId into repDetGroup
            from repDet in repDetGroup.DefaultIfEmpty()
            join f in _context.HighLevelViewTables on a.LastEntity equals f.LastEntityId into highViewGroup
            from f in highViewGroup.DefaultIfEmpty()
            join img in _context.HrEmpImages on a.EmpId equals img.EmpId into imgGroup
            from img in imgGroup.DefaultIfEmpty()
            join j in _context.EmployeeCurrentStatuses on a.CurrentStatus equals j.Status into currStatusGroup
            from j in currStatusGroup.DefaultIfEmpty()
            join s in _context.HrEmpStatusSettings on a.EmpStatus equals s.StatusId into empStatusGroup
            from s in empStatusGroup.DefaultIfEmpty()
            join t in _context.Resignations.Where(x => x.CurrentRequest == 1 && !new[] { "D", "R" }.Contains(x.ApprovalStatus)) on a.EmpId equals t.EmpId into tempResignations
            from t in tempResignations.DefaultIfEmpty()
            join reason in _context.ReasonMasters.Where(r => r.Type == _employeeSettings.ReasonType) on Convert.ToInt32(t.Reason) equals reason.ReasonId into reasonGroup
            from rm in reasonGroup.DefaultIfEmpty()
            join cm in _context.AdmCountryMasters on c.Nationality equals cm.CountryId into countryGroup
            from cm in countryGroup.DefaultIfEmpty()
            join religion in _context.AdmReligionMasters on c.Religion equals religion.ReligionId into religionGroup
            from religion in religionGroup.DefaultIfEmpty()
            where result.Contains(a.EmpStatus.GetValueOrDefault())

            select new EmployeeResultDto
            {
                EmpId = a.EmpId,
                EmpCode = a.EmpCode,
                Name = $"{a.FirstName} {a.MiddleName} {a.LastName}",
                GuardiansName = a.GuardiansName,
                DateOfBirth = FormatDate(a.DateOfBirth, _employeeSettings.DateFormat),
                JoinDate = a.JoinDt.ToString(),
                WeddingDate = c.WeddingDate,

                //WeddingDate = c?.WeddingDate.ToString("dd/MM/yyyy"),

                DataDate = a.JoinDt.ToString(),

                StatusDesc = j.StatusDesc,
                EmpStatusDesc = s.StatusDesc,
                Gender = a.Gender.ToString(),
                //Resignation_Date = a?.ResignationDate.ToString("dd/MM/yyyy"),
                ResignationDate = FormatDate(t.ResignationDate, _employeeSettings.DateFormat),
                //RelievingDate = a?.RelievingDate.ToString("dd/MM/yyyy"),
                RelievingDate = FormatDate(a.RelievingDate, _employeeSettings.DateFormat),
                SeperationStatus = a.SeperationStatus,
                OfficialEmail = b.OfficialEmail ?? "NA",
                PersonalEmail = b.PersonalEmail,
                Phone = b.Phone,
                MaritalStatus = c.MaritalStatus == "S" ? "Single" :
                                      c.MaritalStatus == "M" ? "Married" :
                                      c.MaritalStatus == "W" ? "Widowed" :
                                      c.MaritalStatus == "X" ? "Separated" :
                                      c.MaritalStatus == "D" ? "Divorcee" :
                                      "NA",
                Age = CalculateAge(a.DateOfBirth, ageFormat),
                ProbationDt = a.ProbationDt.ToString(),
                LevelOneDescription = f.LevelOneDescription,
                LevelTwoDescription = f.LevelTwoDescription,
                LevelThreeDescription = f.LevelThreeDescription,
                LevelFourDescription = f.LevelFourDescription,
                LevelFiveDescription = f.LevelFiveDescription,
                LevelSixDescription = f.LevelSixDescription,
                LevelSevenDescription = f.LevelSevenDescription,
                LevelEightDescription = f.LevelEightDescription,
                LevelNineDescription = f.LevelNineDescription,
                LevelTenDescription = f.LevelTenDescription,
                LevelElevenDescription = f.LevelElevenDescription,
                ResignationReason = rm.Description,
                ResignationType = t.ResignationType,// a.ResignationType.ToString(),
                ProbationStatus = a.IsProbation.ToString(),//  == false ? ProbationStatus.CONFIRMED : ProbationStatus.PROBATION,

                Nationality = cm.CountryName,
                IsSave = a.IsSave,
                EmpFileNumber = a.EmpFileNumber,
                CurrentStatus = a.CurrentStatus,


            }).ToListAsync();

            var totalRecords = finalQuery.Count();
            var paginatedResult = finalQuery
                .OrderBy(x => x.EmpCode)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // Return paginated result
            return new PaginatedResult<EmployeeResultDto>
            {
                TotalRecords = totalRecords,
                PageNumber = pageNumber,
                PageSize = pageSize,
                Records = paginatedResult
            };
        }


        private async Task<PaginatedResult<EmployeeResultDto>> InfoFormatZeroOrOneEmpIdZeroEmployeeExists(int pageNumber, int pageSize, string? ageFormat, string? systemStatus, string? currentStatusDesc)
        {

            var resultThreeInfoFormat = await (
                     from a in _context.HrEmpMasters
                     join b in _context.HrEmpAddresses on a.EmpId equals b.EmpId into tempAddress
                     from b in tempAddress.DefaultIfEmpty()
                     join c in _context.HrEmpPersonals on a.EmpId equals c.EmpId into tempPersonal
                     from c in tempPersonal.DefaultIfEmpty()
                     join f in _context.HighLevelViewTables on a.LastEntity equals f.LastEntityId into tempHighLevel
                     from f in tempHighLevel.DefaultIfEmpty()
                     join i in _context.HrEmpImages on a.EmpId equals i.EmpId into tempImages
                     from i in tempImages.DefaultIfEmpty()
                     join t in _context.Resignations.Where(x => x.CurrentRequest == 1 && !new[] { "D", "R" }.Contains(x.ApprovalStatus)) on a.EmpId equals t.EmpId into tempResignations
                     from t in tempResignations.DefaultIfEmpty()
                     join j in _context.EmployeeCurrentStatuses on a.CurrentStatus equals j.Status
                     join s in _context.HrEmpStatusSettings on a.EmpStatus equals s.StatusId
                     join reason in _context.ReasonMasters.Where(r => r.Type == _employeeSettings.ReasonType) on Convert.ToInt32(t.Reason) equals reason.ReasonId into reasonGroup
                     from rm in reasonGroup.DefaultIfEmpty()
                     join cm in _context.AdmCountryMasters on c.Nationality equals cm.CountryId into tempCountry
                     from cm in tempCountry.DefaultIfEmpty()
                     where a.CurrentStatus == Convert.ToInt32(systemStatus) || systemStatus == "0" || systemStatus == currentStatusDesc && t.RelievingDate >= DateTime.UtcNow
                     orderby a.EmpCode
                     select new EmployeeResultDto
                     {
                         EmpId = a.EmpId,
                         ImageUrl = i.ImageUrl,
                         EmpCode = a.EmpCode,
                         Name = $"{a.FirstName} {a.MiddleName} {a.LastName}",

                         GuardiansName = a.GuardiansName,
                         DateOfBirth = FormatDate(a.DateOfBirth, _employeeSettings.DateFormat),
                         JoinDate = a.JoinDt.ToString(),
                         WeddingDate = c.WeddingDate,

                         //WeddingDate = c?.WeddingDate.ToString("dd/MM/yyyy"),

                         DataDate = a.JoinDt.ToString(),

                         StatusDesc = j.StatusDesc,
                         EmpStatusDesc = s.StatusDesc,
                         Gender = a.Gender.ToString(),
                         //Resignation_Date = a?.ResignationDate.ToString("dd/MM/yyyy"),
                         ResignationDate = FormatDate(t.ResignationDate, _employeeSettings.DateFormat),
                         //RelievingDate = a?.RelievingDate.ToString("dd/MM/yyyy"),
                         RelievingDate = FormatDate(a.RelievingDate, _employeeSettings.DateFormat),
                         SeperationStatus = a.SeperationStatus,
                         OfficialEmail = b.OfficialEmail ?? "NA",
                         PersonalEmail = b.PersonalEmail,
                         Phone = b.Phone,
                         MaritalStatus = c.MaritalStatus == "S" ? "Single" :
                                      c.MaritalStatus == "M" ? "Married" :
                                      c.MaritalStatus == "W" ? "Widowed" :
                                      c.MaritalStatus == "X" ? "Separated" :
                                      c.MaritalStatus == "D" ? "Divorcee" :
                                      "NA",
                         Age = CalculateAge(a.DateOfBirth, ageFormat),
                         ProbationDt = a.ProbationDt.ToString(),
                         LevelOneDescription = f.LevelOneDescription,
                         LevelTwoDescription = f.LevelTwoDescription,
                         LevelThreeDescription = f.LevelThreeDescription,
                         LevelFourDescription = f.LevelFourDescription,
                         LevelFiveDescription = f.LevelFiveDescription,
                         LevelSixDescription = f.LevelSixDescription,
                         LevelSevenDescription = f.LevelSevenDescription,
                         LevelEightDescription = f.LevelEightDescription,
                         LevelNineDescription = f.LevelNineDescription,
                         LevelTenDescription = f.LevelTenDescription,
                         LevelElevenDescription = f.LevelElevenDescription,
                         ResignationReason = rm.Description,
                         ResignationType = t.ResignationType,// a.ResignationType.ToString(),
                         ProbationStatus = a.IsProbation.ToString(),//  == false ? ProbationStatus.CONFIRMED : ProbationStatus.PROBATION,

                         Nationality = cm.CountryName,
                         IsSave = a.IsSave,
                         EmpFileNumber = a.EmpFileNumber,
                         CurrentStatus = a.CurrentStatus,
                     }).ToListAsync();
            var totalRecords = resultThreeInfoFormat.Count();
            var paginatedResult = resultThreeInfoFormat
                .OrderBy(x => x.EmpCode)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // Return paginated result
            return new PaginatedResult<EmployeeResultDto>
            {
                TotalRecords = totalRecords,
                PageNumber = pageNumber,
                PageSize = pageSize,
                Records = paginatedResult
            };
        }

        private async Task<PaginatedResult<EmployeeResultDto>> InfoFormatTwoEmpIdZeroEmployeeExists(int pageNumber, int pageSize, string? ageFormat, string? systemStatus, string? currentStatusDesc)
        {

            var resultThreeInfoFormat = await (
                     from a in _context.HrEmpMasters
                     join b in _context.HrEmpAddresses on a.EmpId equals b.EmpId into tempAddress
                     from b in tempAddress.DefaultIfEmpty()
                     join c in _context.HrEmpPersonals on a.EmpId equals c.EmpId into tempPersonal
                     from c in tempPersonal.DefaultIfEmpty()
                     join f in _context.HighLevelViewTables on a.LastEntity equals f.LastEntityId into tempHighLevel
                     from f in tempHighLevel.DefaultIfEmpty()
                     join i in _context.HrEmpImages on a.EmpId equals i.EmpId into tempImages
                     from i in tempImages.DefaultIfEmpty()
                     join t in _context.Resignations.Where(x => x.CurrentRequest == 1 && !new[] { "D", "R" }.Contains(x.ApprovalStatus)) on a.EmpId equals t.EmpId into tempResignations
                     from t in tempResignations.DefaultIfEmpty()
                     join j in _context.EmployeeCurrentStatuses on a.CurrentStatus equals j.Status
                     join s in _context.HrEmpStatusSettings on a.EmpStatus equals s.StatusId
                     join reason in _context.ReasonMasters.Where(r => r.Type == _employeeSettings.ReasonType) on Convert.ToInt32(t.Reason) equals reason.ReasonId into reasonGroup
                     from rm in reasonGroup.DefaultIfEmpty()
                     join cm in _context.AdmCountryMasters on c.Nationality equals cm.CountryId into tempCountry
                     from cm in tempCountry.DefaultIfEmpty()
                     where a.CurrentStatus == Convert.ToInt32(systemStatus) || systemStatus == "0" || systemStatus == currentStatusDesc && t.RelievingDate >= DateTime.UtcNow
                     orderby a.EmpCode
                     select new EmployeeResultDto
                     {
                         EmpId = a.EmpId,
                         ImageUrl = i.ImageUrl,
                         EmpCode = a.EmpCode,
                         Name = $"{a.FirstName} {a.MiddleName} {a.LastName}",

                         GuardiansName = a.GuardiansName,
                         DateOfBirth = FormatDate(a.DateOfBirth, _employeeSettings.DateFormat),
                         JoinDate = a.JoinDt.ToString(),
                         WeddingDate = c.WeddingDate,

                         //WeddingDate = c?.WeddingDate.ToString("dd/MM/yyyy"),

                         DataDate = a.JoinDt.ToString(),

                         StatusDesc = j.StatusDesc,
                         EmpStatusDesc = s.StatusDesc,
                         Gender = a.Gender.ToString(),
                         //Resignation_Date = a?.ResignationDate.ToString("dd/MM/yyyy"),
                         ResignationDate = FormatDate(t.ResignationDate, _employeeSettings.DateFormat),
                         //RelievingDate = a?.RelievingDate.ToString("dd/MM/yyyy"),
                         RelievingDate = FormatDate(a.RelievingDate, _employeeSettings.DateFormat),
                         SeperationStatus = a.SeperationStatus,
                         OfficialEmail = b.OfficialEmail ?? "NA",
                         PersonalEmail = b.PersonalEmail,
                         Phone = b.Phone,
                         MaritalStatus = c.MaritalStatus == "S" ? "Single" :
                                      c.MaritalStatus == "M" ? "Married" :
                                      c.MaritalStatus == "W" ? "Widowed" :
                                      c.MaritalStatus == "X" ? "Separated" :
                                      c.MaritalStatus == "D" ? "Divorcee" :
                                      "NA",
                         Age = CalculateAge(a.DateOfBirth, ageFormat),
                         ProbationDt = a.ProbationDt.ToString(),
                         LevelOneDescription = f.LevelOneDescription,
                         LevelTwoDescription = f.LevelTwoDescription,
                         LevelThreeDescription = f.LevelThreeDescription,
                         LevelFourDescription = f.LevelFourDescription,
                         LevelFiveDescription = f.LevelFiveDescription,
                         LevelSixDescription = f.LevelSixDescription,
                         LevelSevenDescription = f.LevelSevenDescription,
                         LevelEightDescription = f.LevelEightDescription,
                         LevelNineDescription = f.LevelNineDescription,
                         LevelTenDescription = f.LevelTenDescription,
                         LevelElevenDescription = f.LevelElevenDescription,
                         ResignationReason = rm.Description,
                         ResignationType = t.ResignationType,// a.ResignationType.ToString(),
                         ProbationStatus = a.IsProbation.ToString(),//  == false ? ProbationStatus.CONFIRMED : ProbationStatus.PROBATION,

                         Nationality = cm.CountryName,
                         IsSave = a.IsSave,
                         EmpFileNumber = a.EmpFileNumber,
                         CurrentStatus = a.CurrentStatus,
                     }).ToListAsync();
            var totalRecords = resultThreeInfoFormat.Count();
            var paginatedResult = resultThreeInfoFormat
                .OrderBy(x => x.EmpCode)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // Return paginated result
            return new PaginatedResult<EmployeeResultDto>
            {
                TotalRecords = totalRecords,
                PageNumber = pageNumber,
                PageSize = pageSize,
                Records = paginatedResult
            };
        }

        private async Task<PaginatedResult<EmployeeResultDto>> InfoFormatThreeEmpIdZeroEmployeeExists(int pageNumber, int pageSize, string? ageFormat, string? systemStatus, string? currentStatusDesc)
        {

            var resultThreeInfoFormat = await (
                     from a in _context.HrEmpMasters
                     join b in _context.HrEmpAddresses on a.EmpId equals b.EmpId into tempAddress
                     from b in tempAddress.DefaultIfEmpty()
                     join c in _context.HrEmpPersonals on a.EmpId equals c.EmpId into tempPersonal
                     from c in tempPersonal.DefaultIfEmpty()
                     join f in _context.HighLevelViewTables on a.LastEntity equals f.LastEntityId into tempHighLevel
                     from f in tempHighLevel.DefaultIfEmpty()
                     join i in _context.HrEmpImages on a.EmpId equals i.EmpId into tempImages
                     from i in tempImages.DefaultIfEmpty()
                     join t in _context.Resignations.Where(x => x.CurrentRequest == 1 && !new[] { "D", "R" }.Contains(x.ApprovalStatus)) on a.EmpId equals t.EmpId into tempResignations
                     from t in tempResignations.DefaultIfEmpty()
                     join j in _context.EmployeeCurrentStatuses on a.CurrentStatus equals j.Status
                     join s in _context.HrEmpStatusSettings on a.EmpStatus equals s.StatusId
                     join reason in _context.ReasonMasters.Where(r => r.Type == _employeeSettings.ReasonType) on Convert.ToInt32(t.Reason) equals reason.ReasonId into reasonGroup
                     from rm in reasonGroup.DefaultIfEmpty()
                     join cm in _context.AdmCountryMasters on c.Nationality equals cm.CountryId into tempCountry
                     from cm in tempCountry.DefaultIfEmpty()
                     where a.CurrentStatus == Convert.ToInt32(systemStatus) || systemStatus == "0" || systemStatus == currentStatusDesc && t.RelievingDate >= DateTime.UtcNow
                     orderby a.EmpCode
                     select new EmployeeResultDto
                     {
                         EmpId = a.EmpId,
                         ImageUrl = i.ImageUrl,
                         EmpCode = a.EmpCode,
                         Name = $"{a.FirstName} {a.MiddleName} {a.LastName}",

                         GuardiansName = a.GuardiansName,
                         DateOfBirth = FormatDate(a.DateOfBirth, _employeeSettings.DateFormat),
                         JoinDate = a.JoinDt.ToString(),
                         WeddingDate = c.WeddingDate,

                         //WeddingDate = c?.WeddingDate.ToString("dd/MM/yyyy"),

                         DataDate = a.JoinDt.ToString(),

                         StatusDesc = j.StatusDesc,
                         EmpStatusDesc = s.StatusDesc,
                         Gender = a.Gender.ToString(),
                         //Resignation_Date = a?.ResignationDate.ToString("dd/MM/yyyy"),
                         ResignationDate = FormatDate(t.ResignationDate, _employeeSettings.DateFormat),
                         //RelievingDate = a?.RelievingDate.ToString("dd/MM/yyyy"),
                         RelievingDate = FormatDate(a.RelievingDate, _employeeSettings.DateFormat),
                         SeperationStatus = a.SeperationStatus,
                         OfficialEmail = b.OfficialEmail ?? "NA",
                         PersonalEmail = b.PersonalEmail,
                         Phone = b.Phone,
                         MaritalStatus = c.MaritalStatus == "S" ? "Single" :
                                      c.MaritalStatus == "M" ? "Married" :
                                      c.MaritalStatus == "W" ? "Widowed" :
                                      c.MaritalStatus == "X" ? "Separated" :
                                      c.MaritalStatus == "D" ? "Divorcee" :
                                      "NA",
                         Age = CalculateAge(a.DateOfBirth, ageFormat),
                         ProbationDt = a.ProbationDt.ToString(),
                         LevelOneDescription = f.LevelOneDescription,
                         LevelTwoDescription = f.LevelTwoDescription,
                         LevelThreeDescription = f.LevelThreeDescription,
                         LevelFourDescription = f.LevelFourDescription,
                         LevelFiveDescription = f.LevelFiveDescription,
                         LevelSixDescription = f.LevelSixDescription,
                         LevelSevenDescription = f.LevelSevenDescription,
                         LevelEightDescription = f.LevelEightDescription,
                         LevelNineDescription = f.LevelNineDescription,
                         LevelTenDescription = f.LevelTenDescription,
                         LevelElevenDescription = f.LevelElevenDescription,
                         ResignationReason = rm.Description,
                         ResignationType = t.ResignationType,// a.ResignationType.ToString(),
                         ProbationStatus = a.IsProbation.ToString(),//  == false ? ProbationStatus.CONFIRMED : ProbationStatus.PROBATION,

                         Nationality = cm.CountryName,
                         IsSave = a.IsSave,
                         EmpFileNumber = a.EmpFileNumber,
                         CurrentStatus = a.CurrentStatus,
                     }).ToListAsync();
            var totalRecords = resultThreeInfoFormat.Count();
            var paginatedResult = resultThreeInfoFormat
                .OrderBy(x => x.EmpCode)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // Return paginated result
            return new PaginatedResult<EmployeeResultDto>
            {
                TotalRecords = totalRecords,
                PageNumber = pageNumber,
                PageSize = pageSize,
                Records = paginatedResult
            };
        }

        private async Task<PaginatedResult<EmployeeResultDto>> InfoFormatFourEmpIdZeroEmployeeExists(int pageNumber, int pageSize, string? ageFormat, string? systemStatus, string? currentStatusDesc)
        {

            var resultThreeInfoFormat = await (
                     from a in _context.HrEmpMasters
                     join b in _context.HrEmpAddresses on a.EmpId equals b.EmpId into tempAddress
                     from b in tempAddress.DefaultIfEmpty()
                     join c in _context.HrEmpPersonals on a.EmpId equals c.EmpId into tempPersonal
                     from c in tempPersonal.DefaultIfEmpty()
                     join f in _context.HighLevelViewTables on a.LastEntity equals f.LastEntityId into tempHighLevel
                     from f in tempHighLevel.DefaultIfEmpty()
                     join i in _context.HrEmpImages on a.EmpId equals i.EmpId into tempImages
                     from i in tempImages.DefaultIfEmpty()
                     join t in _context.Resignations.Where(x => x.CurrentRequest == 1 && !new[] { "D", "R" }.Contains(x.ApprovalStatus)) on a.EmpId equals t.EmpId into tempResignations
                     from t in tempResignations.DefaultIfEmpty()
                     join j in _context.EmployeeCurrentStatuses on a.CurrentStatus equals j.Status
                     join s in _context.HrEmpStatusSettings on a.EmpStatus equals s.StatusId
                     join reason in _context.ReasonMasters.Where(r => r.Type == _employeeSettings.ReasonType) on Convert.ToInt32(t.Reason) equals reason.ReasonId into reasonGroup
                     from rm in reasonGroup.DefaultIfEmpty()
                     join cm in _context.AdmCountryMasters on c.Nationality equals cm.CountryId into tempCountry
                     from cm in tempCountry.DefaultIfEmpty()
                     where a.CurrentStatus == Convert.ToInt32(systemStatus) || systemStatus == "0" || systemStatus == currentStatusDesc && t.RelievingDate >= DateTime.UtcNow
                     orderby a.EmpCode
                     select new EmployeeResultDto
                     {
                         EmpId = a.EmpId,
                         ImageUrl = i.ImageUrl,
                         EmpCode = a.EmpCode,
                         Name = $"{a.FirstName} {a.MiddleName} {a.LastName}",

                         GuardiansName = a.GuardiansName,
                         DateOfBirth = FormatDate(a.DateOfBirth, _employeeSettings.DateFormat),
                         JoinDate = a.JoinDt.ToString(),
                         WeddingDate = c.WeddingDate,

                         //WeddingDate = c?.WeddingDate.ToString("dd/MM/yyyy"),

                         DataDate = a.JoinDt.ToString(),

                         StatusDesc = j.StatusDesc,
                         EmpStatusDesc = s.StatusDesc,
                         Gender = a.Gender.ToString(),
                         //Resignation_Date = a?.ResignationDate.ToString("dd/MM/yyyy"),
                         ResignationDate = FormatDate(t.ResignationDate, _employeeSettings.DateFormat),
                         //RelievingDate = a?.RelievingDate.ToString("dd/MM/yyyy"),
                         RelievingDate = FormatDate(a.RelievingDate, _employeeSettings.DateFormat),
                         SeperationStatus = a.SeperationStatus,
                         OfficialEmail = b.OfficialEmail ?? "NA",
                         PersonalEmail = b.PersonalEmail,
                         Phone = b.Phone,
                         MaritalStatus = c.MaritalStatus == "S" ? "Single" :
                                      c.MaritalStatus == "M" ? "Married" :
                                      c.MaritalStatus == "W" ? "Widowed" :
                                      c.MaritalStatus == "X" ? "Separated" :
                                      c.MaritalStatus == "D" ? "Divorcee" :
                                      "NA",
                         Age = CalculateAge(a.DateOfBirth, ageFormat),
                         ProbationDt = a.ProbationDt.ToString(),
                         LevelOneDescription = f.LevelOneDescription,
                         LevelTwoDescription = f.LevelTwoDescription,
                         LevelThreeDescription = f.LevelThreeDescription,
                         LevelFourDescription = f.LevelFourDescription,
                         LevelFiveDescription = f.LevelFiveDescription,
                         LevelSixDescription = f.LevelSixDescription,
                         LevelSevenDescription = f.LevelSevenDescription,
                         LevelEightDescription = f.LevelEightDescription,
                         LevelNineDescription = f.LevelNineDescription,
                         LevelTenDescription = f.LevelTenDescription,
                         LevelElevenDescription = f.LevelElevenDescription,
                         ResignationReason = rm.Description,
                         ResignationType = t.ResignationType,// a.ResignationType.ToString(),
                         ProbationStatus = a.IsProbation.ToString(),//  == false ? ProbationStatus.CONFIRMED : ProbationStatus.PROBATION,

                         Nationality = cm.CountryName,
                         IsSave = a.IsSave,
                         EmpFileNumber = a.EmpFileNumber,
                         CurrentStatus = a.CurrentStatus,
                     }).ToListAsync();
            var totalRecords = resultThreeInfoFormat.Count();
            var paginatedResult = resultThreeInfoFormat
                .OrderBy(x => x.EmpCode)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // Return paginated result
            return new PaginatedResult<EmployeeResultDto>
            {
                TotalRecords = totalRecords,
                PageNumber = pageNumber,
                PageSize = pageSize,
                Records = paginatedResult
            };
        }


        public async Task<List<int>> InfoFormatZeroOrOneLinkLevelNotExistAndZeroEmpIds(int roleId, int empId, bool exists)
        {

            int lnkLev = 0;
            //int roleId = 3;
            //int empId = 72;
            //string empIDs = string.Empty;
            int? linkSelect = await _context.SpecialAccessRights.Where(s => s.RoleId == roleId).Select(s => s.LinkLevel).FirstOrDefaultAsync();
            if (linkSelect != null)
            {
                linkSelect = await _context.SpecialAccessRights
                    .Where(e => e.RoleId == roleId)
                    .OrderBy(e => e.LinkLevel)
                    .Select(e => e.LinkLevel)
                    .FirstOrDefaultAsync();//------------Have to get 0 in case roleId 3

            }
            else
            {
                linkSelect = await (from e in _context.EntityAccessRights02s
                                    where e.RoleId == roleId
                                    orderby e.LinkLevel
                                    select e.LinkLevel).FirstOrDefaultAsync();
            }
            var ctnew = SplitStrings_XML(_context.HrEmpMasters
                     .Where(h => h.EmpId == empId)
                     .Select(h => h.EmpEntity).FirstOrDefault(), ',')
             .Select((item, index) => new LinkItemDto
             {
                 Item = item,
                 LinkLevel = index + 2
             }).Where(c => !string.IsNullOrEmpty(c.Item));


            //var entityAccessRights = _context.EntityAccessRights02s.Where(s => s.RoleId == roleId).ToList();

            var entityAccessRights = await _context.EntityAccessRights02s.Where(s => s.RoleId == roleId && s.LinkLevel == linkSelect).ToListAsync();

            var part1 = entityAccessRights
                .Where(s => !string.IsNullOrEmpty(s.LinkId)) // Ensure LinkId is not null or empty
                .SelectMany(
                s => SplitStrings_XML(s.LinkId), // Split LinkId
                (s, item) => new LinkItemDto
                {
                    Item = item,
                    LinkLevel = entityAccessRights.FirstOrDefault().LinkLevel // LinkLevel as per SQL logic
                })
                .Where(f => !string.IsNullOrEmpty(f.Item)) // Exclude empty items
                .ToList();



            var part2 = ctnew
                   .Where(ct => ct.LinkLevel >= lnkLev && !string.IsNullOrEmpty(ct.Item))
                   .Select(ct => new LinkItemDto
                   {
                       Item = ct.Item,
                       LinkLevel = ct.LinkLevel
                   });

            var applicableFinalNew = part1.Concat(part2).Distinct().ToList();

            var empIDs = await (from d in _context.HrEmpMasters
                                where exists ||
                                      (from hlv in _context.HighLevelViewTables
                                       join b in applicableFinalNew on hlv.LastEntityId equals d.LastEntity into joined
                                       where d.IsDelete == false && joined.Any()
                                       select d.EmpId).Contains(d.EmpId)
                                select d.EmpId)
                  .ToListAsync();
            return empIDs;
        }

        public async Task<List<int>> InfoFormatTwoLinkLevelNotExistAndZeroEmpIds(int roleId, int empId, bool exists)
        {

            int lnkLev = 0;
            //int roleId = 3;
            //int empId = 72;
            //string empIDs = string.Empty;
            int? linkSelect = await _context.SpecialAccessRights.Where(s => s.RoleId == roleId).Select(s => s.LinkLevel).FirstOrDefaultAsync();
            if (linkSelect != null)
            {
                linkSelect = await _context.SpecialAccessRights
                    .Where(e => e.RoleId == roleId)
                    .OrderBy(e => e.LinkLevel)
                    .Select(e => e.LinkLevel)
                    .FirstOrDefaultAsync();//------------Have to get 0 in case roleId 3

            }
            else
            {
                linkSelect = await (from e in _context.EntityAccessRights02s
                                    where e.RoleId == roleId
                                    orderby e.LinkLevel
                                    select e.LinkLevel).FirstOrDefaultAsync();
            }
            var ctnew = SplitStrings_XML(_context.HrEmpMasters
                     .Where(h => h.EmpId == empId)
                     .Select(h => h.EmpEntity).FirstOrDefault(), ',')
             .Select((item, index) => new LinkItemDto
             {
                 Item = item,
                 LinkLevel = index + 2
             }).Where(c => !string.IsNullOrEmpty(c.Item));


            //var entityAccessRights = _context.EntityAccessRights02s.Where(s => s.RoleId == roleId).ToList();

            var entityAccessRights = await _context.EntityAccessRights02s.Where(s => s.RoleId == roleId && s.LinkLevel == linkSelect).ToListAsync();

            var part1 = entityAccessRights
                .Where(s => !string.IsNullOrEmpty(s.LinkId)) // Ensure LinkId is not null or empty
                .SelectMany(
                s => SplitStrings_XML(s.LinkId), // Split LinkId
                (s, item) => new LinkItemDto
                {
                    Item = item,
                    LinkLevel = entityAccessRights.FirstOrDefault().LinkLevel // LinkLevel as per SQL logic
                })
                .Where(f => !string.IsNullOrEmpty(f.Item)) // Exclude empty items
                .ToList();



            var part2 = ctnew
                   .Where(ct => ct.LinkLevel >= lnkLev && !string.IsNullOrEmpty(ct.Item))
                   .Select(ct => new LinkItemDto
                   {
                       Item = ct.Item,
                       LinkLevel = ct.LinkLevel
                   });

            var applicableFinalNew = part1.Concat(part2).Distinct().ToList();

            var empIDs = await (from d in _context.HrEmpMasters
                                where exists ||
                                      (from hlv in _context.HighLevelViewTables
                                       join b in applicableFinalNew on hlv.LastEntityId equals d.LastEntity into joined
                                       where d.IsDelete == false && joined.Any()
                                       select d.EmpId).Contains(d.EmpId)
                                select d.EmpId)
                  .ToListAsync();
            return empIDs;
        }

        public async Task<List<int>> InfoFormatThreeLinkLevelNotExistAndZeroEmpIds(int roleId, int empId, bool exists)
        {

            int lnkLev = 0;
            //int roleId = 3;
            //int empId = 72;
            //string empIDs = string.Empty;
            int? linkSelect = await _context.SpecialAccessRights.Where(s => s.RoleId == roleId).Select(s => s.LinkLevel).FirstOrDefaultAsync();
            if (linkSelect != null)
            {
                linkSelect = await _context.SpecialAccessRights
                    .Where(e => e.RoleId == roleId)
                    .OrderBy(e => e.LinkLevel)
                    .Select(e => e.LinkLevel)
                    .FirstOrDefaultAsync();//------------Have to get 0 in case roleId 3

            }
            else
            {
                linkSelect = await (from e in _context.EntityAccessRights02s
                                    where e.RoleId == roleId
                                    orderby e.LinkLevel
                                    select e.LinkLevel).FirstOrDefaultAsync();
            }
            var ctnew = SplitStrings_XML(_context.HrEmpMasters
                     .Where(h => h.EmpId == empId)
                     .Select(h => h.EmpEntity).FirstOrDefault(), ',')
             .Select((item, index) => new LinkItemDto
             {
                 Item = item,
                 LinkLevel = index + 2
             }).Where(c => !string.IsNullOrEmpty(c.Item));


            //var entityAccessRights = _context.EntityAccessRights02s.Where(s => s.RoleId == roleId).ToList();

            var entityAccessRights = await _context.EntityAccessRights02s.Where(s => s.RoleId == roleId && s.LinkLevel == linkSelect).ToListAsync();

            var part1 = entityAccessRights
                .Where(s => !string.IsNullOrEmpty(s.LinkId)) // Ensure LinkId is not null or empty
                .SelectMany(
                s => SplitStrings_XML(s.LinkId), // Split LinkId
                (s, item) => new LinkItemDto
                {
                    Item = item,
                    LinkLevel = entityAccessRights.FirstOrDefault().LinkLevel // LinkLevel as per SQL logic
                })
                .Where(f => !string.IsNullOrEmpty(f.Item)) // Exclude empty items
                .ToList();



            var part2 = ctnew
                   .Where(ct => ct.LinkLevel >= lnkLev && !string.IsNullOrEmpty(ct.Item))
                   .Select(ct => new LinkItemDto
                   {
                       Item = ct.Item,
                       LinkLevel = ct.LinkLevel
                   });

            var applicableFinalNew = part1.Concat(part2).Distinct().ToList();

            var empIDs = await (from d in _context.HrEmpMasters
                                where exists ||
                                      (from hlv in _context.HighLevelViewTables
                                       join b in applicableFinalNew on hlv.LastEntityId equals d.LastEntity into joined
                                       where d.IsDelete == false && joined.Any()
                                       select d.EmpId).Contains(d.EmpId)
                                select d.EmpId)
                  .ToListAsync();
            return empIDs;
        }

        public async Task<List<int>> InfoFormatFourLinkLevelNotExistAndZeroEmpIds(int roleId, int empId, bool exists)
        {

            int lnkLev = 0;
            //int roleId = 3;
            //int empId = 72;
            //string empIDs = string.Empty;
            int? linkSelect = await _context.SpecialAccessRights.Where(s => s.RoleId == roleId).Select(s => s.LinkLevel).FirstOrDefaultAsync();
            if (linkSelect != null)
            {
                linkSelect = await _context.SpecialAccessRights
                    .Where(e => e.RoleId == roleId)
                    .OrderBy(e => e.LinkLevel)
                    .Select(e => e.LinkLevel)
                    .FirstOrDefaultAsync();//------------Have to get 0 in case roleId 3

            }
            else
            {
                linkSelect = await (from e in _context.EntityAccessRights02s
                                    where e.RoleId == roleId
                                    orderby e.LinkLevel
                                    select e.LinkLevel).FirstOrDefaultAsync();
            }
            var ctnew = SplitStrings_XML(_context.HrEmpMasters
                     .Where(h => h.EmpId == empId)
                     .Select(h => h.EmpEntity).FirstOrDefault(), ',')
             .Select((item, index) => new LinkItemDto
             {
                 Item = item,
                 LinkLevel = index + 2
             }).Where(c => !string.IsNullOrEmpty(c.Item));


            //var entityAccessRights = _context.EntityAccessRights02s.Where(s => s.RoleId == roleId).ToList();

            var entityAccessRights = await _context.EntityAccessRights02s.Where(s => s.RoleId == roleId && s.LinkLevel == linkSelect).ToListAsync();

            var part1 = entityAccessRights
                .Where(s => !string.IsNullOrEmpty(s.LinkId)) // Ensure LinkId is not null or empty
                .SelectMany(
                s => SplitStrings_XML(s.LinkId), // Split LinkId
                (s, item) => new LinkItemDto
                {
                    Item = item,
                    LinkLevel = entityAccessRights.FirstOrDefault().LinkLevel // LinkLevel as per SQL logic
                })
                .Where(f => !string.IsNullOrEmpty(f.Item)) // Exclude empty items
                .ToList();



            var part2 = ctnew
                   .Where(ct => ct.LinkLevel >= lnkLev && !string.IsNullOrEmpty(ct.Item))
                   .Select(ct => new LinkItemDto
                   {
                       Item = ct.Item,
                       LinkLevel = ct.LinkLevel
                   });

            var applicableFinalNew = part1.Concat(part2).Distinct().ToList();

            var empIDs = await (from d in _context.HrEmpMasters
                                where exists ||
                                      (from hlv in _context.HighLevelViewTables
                                       join b in applicableFinalNew on hlv.LastEntityId equals d.LastEntity into joined
                                       where d.IsDelete == false && joined.Any()
                                       select d.EmpId).Contains(d.EmpId)
                                select d.EmpId)
                  .ToListAsync();
            return empIDs;
        }
        public static IEnumerable<string> SplitStrings_XML(string list, char delimiter = ',')
        {
            if (string.IsNullOrWhiteSpace(list))
                return Enumerable.Empty<string>();

            // Split the input string by the delimiter and return as IEnumerable
            return list.Split(delimiter)
                       .Select(item => item.Trim()) // Trim whitespace from each item
                       .Where(item => !string.IsNullOrEmpty(item)); // Exclude empty items
        }
        public static IEnumerable<string> SplitStrings_XML(string input)
        {
            return input?.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries) ?? Enumerable.Empty<string>();
        }

        public static string CalculateAge(DateTime? givenDate, string ageFormat)
        {
            if (!givenDate.HasValue)
                return "NA";
            if (givenDate.Value > DateTime.UtcNow)
                return "0 years";

            DateTime currentDate = DateTime.UtcNow;
            DateTime birthDate = givenDate.Value;

            // Calculate years
            int years = currentDate.Year - birthDate.Year;
            if (birthDate.AddYears(years) > currentDate)
                years--;

            // Calculate months
            DateTime yearAdjustedDate = birthDate.AddYears(years);
            int months = (currentDate.Year - yearAdjustedDate.Year) * 12 + currentDate.Month - yearAdjustedDate.Month;
            if (yearAdjustedDate.AddMonths(months) > currentDate)
                months--;

            // Calculate days
            DateTime monthAdjustedDate = yearAdjustedDate.AddMonths(months);
            int days = (currentDate - monthAdjustedDate).Days;

            // Calculate total days
            int totalDays = (currentDate - birthDate).Days;

            // Return based on the format
            return ageFormat switch
            {
                "INYR" => $"{years} years",
                "INYRDY" => $"{years} years {totalDays} days",
                "INYRMN" => $"{years} years {months} months",
                "INYRMND" => $"{years} years {months} months {days} days",
                _ => $"{years} years"
            };
        }

        public static Gender GetGender(string genderCode)
        {
            return genderCode switch
            {
                "M" => Gender.Male,
                "F" => Gender.Female,
                "O" => Gender.Other,
                _ => Gender.UnKnown
            };
        }



        public static string FormatDate(DateTime? date, string format)
        {
            return date.HasValue ? date.Value.ToString(format) : string.Empty; // Or any other default value
        }

        public async Task<List<LanguageSkillResultDto>> LanguageSkill(int employeeId)
        {

            return await (from emp in _context.EmployeeLanguageSkills
                          join empdet in _context.EmployeeDetails
                          on emp.EmpId equals empdet.EmpId
                          join hrm in _context.HrEmpLanguagemasters on Convert.ToInt32(emp.LanguageId) equals hrm.LanguageId
                          where emp.EmpId == employeeId && emp.Status == _employeeSettings.EmployeeStatus
                          select new LanguageSkillResultDto
                          {
                              Emp_LangId = emp.EmpLangId,
                              Name = empdet.Name,
                              LanguageId = (byte)(emp.LanguageId == "1" ? 1 : 0),
                              Code = hrm.Code,
                              Description = hrm.Description,
                              Read = (byte)(emp.Read == true ? 1 : 0),
                              Write = (byte)(emp.Write == true ? 1 : 0),
                              Speak = (byte)(emp.Speak == true ? 1 : 0),
                              Comprehend = (byte)(emp.Comprehend == true ? 1 : 0),
                              MotherTongue = (byte)(emp.MotherTongue == true ? 1 : 0)

                          }).ToListAsync();


        }

        public async Task<CommunicationResultDto> Communication(int employeeId)
        {
            var communationtable = await (from a in _context.HrEmpAddresses
                                          join b in _context.AdmCountryMasters on a.Country equals b.CountryId into admGroup
                                          from b in admGroup.DefaultIfEmpty()
                                          where a.EmpId == employeeId
                                          select new CommunicationTable1Dto
                                          {
                                              Inst_Id = a.InstId,
                                              Add_Id = a.AddId,
                                              Emp_Id = a.EmpId,
                                              Add1 = a.Add1,
                                              Add2 = a.Add2,
                                              PBNo = a.Pbno,
                                              Country_ID = b.CountryId,
                                              Country_Name = b.CountryName,
                                              Phone = a.Phone,
                                              Mobile = a.Mobile,
                                              OfficePhone = a.OfficePhone,
                                              Extension = a.Extension,
                                              EMail = a.OfficialEmail,
                                              PersonalEMail = a.PersonalEmail,
                                              Status =  "A"


                                          }).ToListAsync();

            var communationtable1 = await (from a in _context.HrEmpAddressApprls
                                           join b in _context.AdmCountryMasters on a.Country equals b.CountryId into admGroup
                                           from b in admGroup.DefaultIfEmpty()
                                           join c in _context.CommunicationRequestWorkFlowstatuses on a.AddId equals c.RequestId
                                           where a.EmpId == employeeId
                                           select new CommunicationTable1Dto
                                           {
                                               Inst_Id = a.InstId,
                                               Add_Id = a.AddId,
                                               Emp_Id = a.EmpId,
                                               Add1 = a.Add1,
                                               Add2 = a.Add2,
                                               PBNo = a.Pbno,
                                               Country_ID = b.CountryId,
                                               Country_Name = b.CountryName,
                                               Phone = a.Phone,
                                               Mobile = a.Mobile,
                                               OfficePhone = a.OfficePhone,
                                               Extension = a.Extension,
                                               EMail = a.Email,
                                               PersonalEMail = a.PersonalEmail,
                                               Status = a.Status

                                           }).ToListAsync();


            var result = new CommunicationResultDto
            {
                CommunicationTable = communationtable,
                CommunicationTable1 = communationtable1,

            };
            return await Task.FromResult(result);
        }

        public async Task<CommunicationExtraDto> CommunicationExtra(int employeeId)
        {


            var query1 = await (
                from a in _context.HrEmpAddress01Apprls
                join b in _context.AdmCountryMasters on a.Addr1Country equals b.CountryId into countryGroup1
                from b in countryGroup1.DefaultIfEmpty()
                join c in _context.AdmCountryMasters on a.Addr2Country equals c.CountryId into countryGroup2
                from c in countryGroup2.DefaultIfEmpty()
                join d in _context.CommunicationAdditionalRequestWorkFlowstatuses
                    on a.AddrId equals d.RequestId
                where a.EmpId == employeeId && d.ApprovalStatus == "P"
                select new AddressDto
                {
                    AddrID = a.AddrId,
                    EmpID = a.EmpId,
                    PermanentAddr = a.PermanentAddr ?? "NA",
                    ContactAddr = a.ContactAddr ?? "NA",
                    PinNo1 = a.PinNo1 ?? "NA",
                    PinNo2 = a.PinNo2 ?? "NA",
                    CountryID1 = b.CountryId == null ? 0 : b.CountryId,
                    Country1 = b.CountryName ?? "NA",
                    CountryID2 = c.CountryId == null ? 0 : c.CountryId,
                    Country2 = c.CountryName ?? "NA",
                    Status = d.ApprovalStatus,
                    PhoneNo = a.PhoneNo ?? "NA",
                    AlterPhoneNo = a.AlterPhoneNo ?? "NA",
                    MobileNo = a.MobileNo ?? "NA"
                }).ToListAsync();

            var query2 = await (
                        from a in _context.HrEmpAddress01s
                        join b in _context.AdmCountryMasters on a.Addr1Country equals b.CountryId into countryGroup1
                        from b in countryGroup1.DefaultIfEmpty()
                        join c in _context.AdmCountryMasters on a.Addr2Country equals c.CountryId into countryGroup2
                        from c in countryGroup2.DefaultIfEmpty()
                        where a.EmpId == employeeId &&
                              (a.ApprlId == null || _context.HrEmpAddress01Apprls.Any(h => h.AddrId == a.ApprlId && h.EmpId == employeeId))
                        select new AddressDto
                        {
                            AddrID = a.AddrId,
                            EmpID = a.EmpId,
                            PermanentAddr = a.PermanentAddr ?? "NA",
                            ContactAddr = a.ContactAddr ?? "NA",
                            PinNo1 = a.PinNo1 ?? "NA",
                            PinNo2 = a.PinNo2 ?? "NA",
                            CountryID1 = b.CountryId == null ? 0 : b.CountryId,
                            Country1 = b.CountryName ?? "NA",
                            CountryID2 = c.CountryId == null ? 0 : c.CountryId,
                            Country2 = c.CountryName ?? "NA",
                            Status = "A",
                            PhoneNo = a.PhoneNo ?? "NA",
                            AlterPhoneNo = a.AlterPhoneNo ?? "NA",
                            MobileNo = a.MobileNo ?? "NA"
                        }).ToListAsync();

            var finalQuery = query1.Concat(query2);

            var result = finalQuery.ToList();



            return new CommunicationExtraDto
            {
                Addresses = result.Select(r => new AddressDto
                {
                    AddrID = r.AddrID,
                    EmpID = r.EmpID,
                    PermanentAddr = r.PermanentAddr,
                    ContactAddr = r.ContactAddr,
                    PinNo1 = r.PinNo1,
                    PinNo2 = r.PinNo2,
                    CountryID1 = r.CountryID1,
                    Country1 = r.Country1,
                    CountryID2 = r.CountryID2,
                    Country2 = r.Country2,
                    Status = r.Status,
                    PhoneNo = r.PhoneNo,
                    AlterPhoneNo = r.AlterPhoneNo,
                    MobileNo = r.MobileNo
                }).ToList()
            };
        }

        public async Task<List<CommunicationEmergencyDto>> CommunicationEmergency(int employeeId)
        {
            var result = await (from a in _context.HrEmpEmergaddresses
                                join b in _context.AdmCountryMasters on a.Country equals b.CountryId into Emergency
                                from b in Emergency.DefaultIfEmpty()
                                where a.EmpId == employeeId
                                select new CommunicationEmergencyDto
                                {
                                    AddrID = a.AddrId,
                                    EmpID = Convert.ToInt32(a.EmpId),
                                    Address = a.Address,
                                    PinNo = a.PinNo,
                                    Country = Convert.ToInt32(a.Country),
                                    Country_Name = b.CountryName,
                                    PhoneNo = a.PhoneNo,
                                    AlterPhoneNo = a.AlterPhoneNo,
                                    MobileNo = a.MobileNo,
                                    Status = "A",
                                    EmerName = a.EmerName,
                                    EmerRelation = a.EmerRelation
                                }).ToListAsync();

            return result;
        }

        public async Task<List<string>> HobbiesData(int employeeId)
        {
            var Hobbies = await (from a in _context.GeneralCategories
                                 join b in _context.ReasonMasters on a.Id equals b.Value
                                 join c in _context.EmployeeHobbies on b.ReasonId equals Convert.ToInt32(c.HobbieId)
                                 where a.Description == "Hobbies" && c.EmployeeId == employeeId
                                 select b.Description
                                     )
                                     .ToListAsync();

            return Hobbies;
        }

        public async Task<List<RewardAndRecognitionDto>> RewardAndRecognitions(int employeeId)
        {
            var RewardAndRecognitionResult = await (from a in _context.EmpRewards
                                                    join b in _context.AchievementMasters on a.AchievementId equals b.AchievementId
                                                    join c in _context.HrmValueTypes on a.RewardType equals c.Id
                                                    where a.EmpId == employeeId && a.Status == "A"
                                                    select new RewardAndRecognitionDto
                                                    {
                                                        RewardID = a.RewardId,
                                                        Emp_id = a.EmpId,
                                                        Achievement = b.Description,
                                                        RewardType = c.Description,
                                                        RewardValue = c.Id,
                                                        Rewarddate = a.RewardDate.HasValue ? a.RewardDate.Value.ToString("dd/MM/yyyy") : null,
                                                        Reason = a.Reason,
                                                        Amount = a.Amount,
                                                        rewardidtype = a.RewardType

                                                    }
                ).ToListAsync();
            return RewardAndRecognitionResult;
        }

        public async Task<List<QualificationDto>> Qualification(int employeeId)
        {
            var apprlResults = (
            from a in _context.HrEmpQualificationApprls
            join b in _context.HrEmpMasters on a.EmpId equals b.EmpId
            join c in _context.QualificationRequestWorkFlowstatuses on a.QlfId equals c.RequestId
            where a.EmpId == employeeId && c.ApprovalStatus == "P"
            select new QualificationTableDto
            {
                Qlf_id = a.QlfId,
                Emp_Id = a.EmpId,
                Course = a.Course ?? "NA",
                University = a.University ?? "NA",
                Inst_Name = a.InstName ?? "NA",
                Dur_Frm = a.DurFrm.HasValue ? a.DurFrm.Value.ToString("dd/MM/yyyy") : "NA",
                Dur_To = a.DurTo.HasValue ? a.DurTo.Value.ToString("dd/MM/yyyy") : "NA",
                Year_Pass = a.YearPass ?? "NA",
                Mark_Per = a.MarkPer ?? "NA",
                Class = a.Class ?? "NA",
                Subjects = a.Subjects ?? "NA",
                Status = a.Status ?? "NA"
            }).ToList();

            var qualificationResults = (
                from a in _context.HrEmpQualifications
                join b in _context.HrEmpMasters on a.EmpId equals b.EmpId
                where a.EmpId == employeeId &&
                      (_context.HrEmpQualificationApprls.Any(q => q.EmpId == employeeId && q.QlfId == a.ApprlId) || a.ApprlId == null)
                select new QualificationTableDto
                {
                    Qlf_id = a.QlfId,
                    Emp_Id = a.EmpId,
                    Course = a.Course ?? "NA",
                    University = a.University ?? "NA",
                    Inst_Name = a.InstName ?? "NA",
                    Dur_Frm = a.DurFrm.HasValue ? a.DurFrm.Value.ToString("dd/MM/yyyy") : "NA",
                    Dur_To = a.DurTo.HasValue ? a.DurTo.Value.ToString("dd/MM/yyyy") : "NA",
                    Year_Pass = a.YearPass ?? "NA",
                    Mark_Per = a.MarkPer ?? "NA",
                    Class = a.Class ?? "NA",
                    Subjects = a.Subjects ?? "NA",
                    Status = "A"
                }
            ).ToList();

            var combinedResults = apprlResults.Concat(qualificationResults).ToList();

            var groupedResults = combinedResults
                .GroupBy(q => new
                {
                    q.Qlf_id,
                    q.Emp_Id,
                    q.Course,
                    q.University,
                    q.Inst_Name,
                    q.Dur_Frm,
                    q.Dur_To,
                    q.Year_Pass,
                    q.Mark_Per,
                    q.Class,
                    q.Subjects,
                    q.Status
                })
                .OrderByDescending(g => DateTime.TryParse(g.Key.Dur_To, out var dateResult) ? dateResult : DateTime.MinValue)
                .Select(g => new QualificationTableDto
                {
                    Qlf_id = g.Key.Qlf_id,
                    Emp_Id = g.Key.Emp_Id,
                    Course = g.Key.Course,
                    University = g.Key.University,
                    Inst_Name = g.Key.Inst_Name,
                    Dur_Frm = g.Key.Dur_Frm,
                    Dur_To = g.Key.Dur_To,
                    Year_Pass = g.Key.Year_Pass,
                    Mark_Per = g.Key.Mark_Per,
                    Class = g.Key.Class,
                    Subjects = g.Key.Subjects,
                    Status = g.Key.Status
                })
                .ToList();

            var attachments = _context.QualificationAttachments
                .Where(a => a.EmpId == employeeId && a.DocStatus == "A")
                .Select(a => new QualificationFileDto
                {
                    QualAttachId = a.QualAttachId,
                    QualificationId = a.QualificationId,
                    QualFileName = a.QualFileName,
                    DocStatus = a.DocStatus
                })
                .ToList();

            return await Task.FromResult(new List<QualificationDto>
            {
                new QualificationDto
                {
                    QualificationTable = groupedResults,
                    QualificationFile = attachments
                }
            });
        }

        public async Task<List<SkillSetDto>> SkillSets(int employeeId)
        {
            var apprlResults = (
                from a in _context.HrEmpTechnicalApprls
                join b in _context.SkillSetRequestWorkFlowstatuses on a.TechId equals b.RequestId
                where a.EmpId == employeeId && b.ApprovalStatus == "P"
                select new SkillSetDto
                {
                    Tech_id = a.TechId,
                    Emp_Id = a.EmpId,
                    Course = a.Course ?? "NA",
                    Course_Dtls = a.CourseDtls ?? "NA",
                    Inst_Name = a.InstName ?? "NA",
                    Dur_Frm = a.DurFrm.HasValue ? a.DurFrm.Value.ToString("dd/MM/yyyy") : "NA",
                    Dur_To = a.DurTo.HasValue ? a.DurTo.Value.ToString("dd/MM/yyyy") : "NA",
                    Year = a.Year ?? "NA",
                    Mark_Per = a.MarkPer ?? "NA",
                    langSkills = a.LangSkills ?? "NA",
                    Status = "P"
                }).ToList();

            var technicalResults = (
                from a in _context.HrEmpTechnicals
                where a.EmpId == employeeId &&
                      (_context.HrEmpTechnicalApprls.Any(ta => ta.EmpId == employeeId && ta.TechId == a.ApprlId) || a.ApprlId == null)
                select new SkillSetDto
                {
                    Tech_id = a.TechId,
                    Emp_Id = a.EmpId,
                    Course = a.Course ?? "NA",
                    Course_Dtls = a.CourseDtls ?? "NA",
                    Inst_Name = a.InstName ?? "NA",
                    Dur_Frm = a.DurFrm.HasValue ? a.DurFrm.Value.ToString("dd/MM/yyyy") : "NA",
                    Dur_To = a.DurTo.HasValue ? a.DurTo.Value.ToString("dd/MM/yyyy") : "NA",
                    Year = a.Year ?? "NA",
                    Mark_Per = a.MarkPer ?? "NA",
                    langSkills = a.LangSkills ?? "NA",
                    Status = "A"
                }).ToList();

            // Combine both results (approval results and technical results)
            var combinedResults = apprlResults.Concat(technicalResults).ToList();

            // Group the results by all required fields and sort them by Dur_To
            var groupedResults = combinedResults
                .GroupBy(r => new
                {
                    r.Tech_id,
                    r.Emp_Id,
                    r.Course,
                    r.Course_Dtls,
                    r.Inst_Name,
                    r.Dur_Frm,
                    r.Dur_To,
                    r.Year,
                    r.Mark_Per,
                    r.langSkills,
                    r.Status
                })
                .OrderByDescending(g => DateTime.TryParse(g.Key.Dur_To, out var dateResult) ? dateResult : DateTime.MinValue)
                .Select(g => new SkillSetDto
                {
                    Tech_id = g.Key.Tech_id,
                    Emp_Id = g.Key.Emp_Id,
                    Course = g.Key.Course,
                    Course_Dtls = g.Key.Course_Dtls,
                    Inst_Name = g.Key.Inst_Name,
                    Dur_Frm = g.Key.Dur_Frm,
                    Dur_To = g.Key.Dur_To,
                    Year = g.Key.Year,
                    Mark_Per = g.Key.Mark_Per,
                    langSkills = g.Key.langSkills,
                    Status = g.Key.Status
                })
                .ToList();

            // Assuming you want to return a list of SkillSetDto
            return groupedResults;

        }

        public async Task<List<DocumentsDto>> Documents(int employeeId)
        {
            //var result = await (from vt in _context.HrmValueTypes
            //              where vt.Type.Equals("EmployeeReporting")
            //                 && vt.Value.Equals(GetDefaultCompanyParameter(employeeId, "EMPEDITBUTTN", "EMP1"))
            //              select vt.Code).FirstOrDefaultAsync();

            //var r=from a in _context.EmployeeDetails

            var result = await (from vt in _context.HrmValueTypes
                                where vt.Type == "EmployeeReporting"
                                select vt.Code).FirstOrDefaultAsync();

            var ageFormat = await (from cp in _context.CompanyParameters
                                   join vt in _context.HrmValueTypes on cp.Value equals vt.Value
                                   where vt.Type == _employeeSettings.ValueType && cp.ParameterCode == _employeeSettings.ParameterCode
                                   select vt.Code).FirstOrDefaultAsync();


            var cteEmployeeDetails = await (
            from emp in _context.HrEmpMasters
            where emp.EmpId == employeeId
            select new
            {
                emp.InstId,
                emp.EmpId,
                emp.EmpCode,
                Name = $"{emp.FirstName} {emp.MiddleName} {emp.LastName}",
                emp.DateOfBirth,
                emp.BranchId,
                emp.DepId,
                emp.DesigId,
                emp.LastEntity,
                JoinDate = FormatDate(emp.JoinDt, _employeeSettings.DateFormat),
                Probation_Dt = FormatDate(emp.ProbationDt, _employeeSettings.DateFormat),
                emp.GuardiansName,
                Gender = GetGender(emp.Gender),
                emp.EmpStatus,
                emp.CurrentStatus,
                emp.EmpFirstEntity,
                emp.EmpEntity,
                emp.SeperationStatus,
                emp.IsExperienced,
                emp.NoticePeriod,
                emp.ProbationNoticePeriod,
                emp.GratuityStrtDate,
                emp.FirstEntryDate,
                emp.Ishra,
                emp.IsExpat,
                emp.CompanyConveyance,
                emp.CompanyVehicle,
                emp.IsDelete,
                emp.DeletedBy,
                emp.DeletedDate,
                emp.IsSave,

            }).ToListAsync();




            return new List<DocumentsDto>();
        }

        public async Task<List<DependentDto>> Dependent(int employeeId)
        {
            var result = await (from a in _context.Dependent00s
                                join b in _context.DependentMasters on a.RelationshipId equals b.DependentId
                                where a.DepEmpId == employeeId
                                select new DependentDto
                                {
                                    DepId = a.DepId,
                                    Description = b.Description,
                                    DateOfBirth = a.DateOfBirth.HasValue ? a.DateOfBirth.Value.ToString("dd/MM/yyyy") : "0",
                                    InterEmpID = a.InterEmpId,
                                    Type = a.Type,
                                    Phone = a.Description, // Assuming "Phone" is mapped to "Description"
                                    Gender = a.Gender == "M" ? "Male" : (a.Gender == "O" ? "Others" : "Female")
                                }).ToListAsync();

            return result;

        }

        public async Task<List<CertificationDto>> Certification(int employeeId)
        {
            var ctename = from a in _context.ReasonMasters
                          join b in _context.GeneralCategories on a.Value equals b.Id
                          where b.Code == "CERNAME"
                          select new
                          {
                              a.ReasonId,
                              a.Description
                          };

            // CTE for Certificate Field
            var ctefield = from a in _context.ReasonMasters
                           join b in _context.GeneralCategories on a.Value equals b.Id
                           where b.Code == "CERTFIELD"
                           select new
                           {
                               a.ReasonId,
                               a.Description
                           };

            // CTE for Issuing Authority
            var cteIssue = from a in _context.ReasonMasters
                           join b in _context.GeneralCategories on a.Value equals b.Id
                           where b.Code == "ISSUAUTH"
                           select new
                           {
                               a.ReasonId,
                               a.Description
                           };

            // CTE for Year of Completion
            var cteYear = from a in _context.ReasonMasters
                          join b in _context.GeneralCategories on a.Value equals b.Id
                          where b.Code == "YRCMP"
                          select new
                          {
                              a.ReasonId,
                              a.Description
                          };

            // Now, join the CTEs with the EmployeeCertifications table
            var result = await (from a in _context.EmployeeCertifications
                                join b in ctefield on a.CertificationField equals b.ReasonId
                                join d in cteIssue on a.IssuingAuthority equals d.ReasonId
                                join c in ctename on a.CertificationName equals c.ReasonId
                                join e in cteYear on a.YearofCompletion equals e.ReasonId
                                where a.EmpId == employeeId && a.Status != "D"
                                select new CertificationDto
                                {
                                    Emp_Id = a.EmpId,
                                    CertificationID = a.CertificationId,
                                    Certificate_Name = c.Description,
                                    Certificate_Field = b.Description,
                                    Issuing_Authority = d.Description,
                                    Year_Of_Completion = e.Description
                                }).ToListAsync();

            // Execute the query and get the results

            return result;

        }

        public async Task<List<DisciplinaryActionsDto>> DisciplinaryActions(int employeeId)
        {
            var result = await (from a in _context.AssignedLetterTypes
                                join b in _context.LetterMaster01s on a.LetterSubType equals b.ModuleSubId
                                join c in _context.LetterMaster00s on b.LetterTypeId equals c.LetterTypeId
                                where a.EmpId == employeeId &&
                                      c.LetterCode == "WRNG" &&
                                      (a.ApprovalStatus == "RE" || a.ApprovalStatus == "A")
                                orderby b.LetterSubName, a.ReleaseDate
                                select new DisciplinaryActionsDto
                                {
                                    EmpID = a.EmpId,
                                    LetterName = b.LetterSubName,
                                    Reason = a.Remark,
                                    LetterSubName = b.LetterSubName,
                                    IsLetterAttached = Convert.ToInt32(a.IsLetterAttached),
                                    ReleaseDate = a.ReleaseDate == null ? "NA" : a.ReleaseDate.Value.ToString("dd/MM/yyyy"),
                                    LetterReqID = Convert.ToInt32(a.LetterReqId),
                                    ApprovalStatus = "E",
                                    UploadFileName = a.UploadFileName,
                                    IssueDate = a.IssueDate == null ? "NA" : a.IssueDate.Value.ToString("dd/MM/yyyy"),
                                    Template = string.IsNullOrEmpty(a.TemplateStyle) ? 0 : 1
                                }).ToListAsync();
            return result;
        }
        public async Task<List<LetterDto>> Letter(int employeeId)
        {
            // Get the LetterJoinDate
            DateTime? letterJoinDate = _context.HrEmpMasters
                .Where(emp => emp.EmpId == employeeId)
                .Select(emp => emp.JoinDt)
                .FirstOrDefault();

            // Query 1: Convert to LetterDto before Concat
            var query1 = from a in _context.AssignedLetterTypes
                         join b in _context.LetterMaster01s on a.LetterSubType equals b.ModuleSubId
                         join c in _context.LetterMaster00s on b.LetterTypeId equals c.LetterTypeId
                         where (a.ApprovalStatus == "A" || a.ApprovalStatus == "RE")
                               && a.EmpId == employeeId
                               && (b.AppointmentLetter ?? 0) == 0
                         group a by new { a.LetterReqId, b.LetterSubName, a.EmpId, a.ReleaseDate, b.AppointmentLetter } into grouped
                         select new LetterDto
                         {
                             LetterReqID = Convert.ToInt32(grouped.Key.LetterReqId),
                             LetterSubName = grouped.Key.LetterSubName,
                             EmpID = grouped.Key.EmpId,
                             ReleaseDate = grouped.Key.ReleaseDate.HasValue ? grouped.Key.ReleaseDate.Value.ToString("dd/MM/yyyy") : null,
                             AppointmentLetter = grouped.Key.AppointmentLetter ?? 0
                         };

            // Query 2: Convert to LetterDto before Concat
            var query2 = from b in _context.LetterMaster01s
                         join c in _context.LetterMaster00s on b.LetterTypeId equals c.LetterTypeId
                         where c.LetterCode == "OTYPE" && (b.AppointmentLetter ?? 0) == 1
                         select new LetterDto
                         {
                             LetterReqID = 0,
                             LetterSubName = b.LetterSubName,
                             EmpID = employeeId,
                             ReleaseDate = letterJoinDate.HasValue ? letterJoinDate.Value.ToString("dd/MM/yyyy") : null,
                             AppointmentLetter = b.AppointmentLetter ?? 0
                         };

            // Query 3: Convert to LetterDto before Concat
            var query3 = (from a in _context.TaxDeclarationFileUploads
                          where a.EmpId == employeeId
                          orderby a.CreatedDate descending
                          select new LetterDto
                          {
                              LetterReqID = a.InvstmntFileId,
                              LetterSubName = "Form 12B",
                              EmpID = employeeId,
                              ReleaseDate = a.CreatedDate.HasValue ? a.CreatedDate.Value.ToString("dd/MM/yyyy") : null,
                              AppointmentLetter = -1
                          }).Take(1);

            // Materialize all queries to in-memory lists (ToList())
            var query1List = await query1.ToListAsync();
            var query2List = await query2.ToListAsync();
            var query3List = await query3.ToListAsync();

            // Concatenate the results
            var concatResult = query1List.Concat(query2List).Concat(query3List).OrderByDescending(r => r.ReleaseDate);

            // Return the final result as a list of LetterDto
            return concatResult.ToList();
        }

        public async Task<List<ReferenceDto>> Reference(int employeeId)
        {
            var result = await (from a in _context.HrEmpreferences
                                join b in _context.ConsultantDetails on a.ConsultantId equals b.Id into consultantDetails
                                from bDetail in consultantDetails.DefaultIfEmpty()
                                join c in _context.EmployeeDetails on a.RefEmpId equals c.EmpId into employeeDetails
                                from cDetail in employeeDetails.DefaultIfEmpty()
                                where a.EmpId == employeeId && a.Status != "D" // Filtering is applied only to HrEmpreferences
                                select new ReferenceDto
                                {
                                    RefID = a.RefId,
                                    RefTypedet = a.RefType == "I" ? "Internal" : a.RefType == "E" ? "External" : null,
                                    RefType = a.RefType,
                                    RefMethoddet = a.RefMethod == "C" ? "Consultant" :
                                                   a.RefMethod == "O" ? "Others" :
                                                   a.RefMethod == "D" ? "Direct" : "NA",
                                    RefMethod = a.RefMethod,
                                    Id = bDetail.Id,
                                    ConsultantName = string.IsNullOrEmpty(bDetail.ConsultantName) ? "NA" : bDetail.ConsultantName,
                                    RefEmpName = cDetail.Name == null ? "NA" : cDetail.Name,
                                    Emp_Code = cDetail.EmpCode == null ? "NA" : cDetail.EmpCode,
                                    RefEmpID = a.RefEmpId == null ? 0 : a.RefEmpId,
                                    RefName = string.IsNullOrEmpty(a.RefName) ? "NA" : a.RefName,
                                    PhoneNo = string.IsNullOrEmpty(a.PhoneNo) ? "NA" : a.PhoneNo,
                                    Address = string.IsNullOrEmpty(a.Address) ? "NA" : a.Address
                                }).ToListAsync();

            return result;
        }

        private async Task<string> GetEmployeeExperienceLength(int empId, int expDays)
        {
            DateTime? joinDate = _context.EmployeeDetails
                .Where(e => e.EmpId == empId)
                .Select(e => e.FirstEntryDate ?? e.JoinDt)
                .FirstOrDefault();

            if (!joinDate.HasValue)
                return "0Y: 0M: 0D";

            DateTime lastDate = joinDate.Value.AddDays(expDays);
            DateTime fromDate = joinDate.Value;
            DateTime toDate = lastDate;

            if (fromDate > toDate) // Future Date
            {
                return "0Y: 0M: 0D";
            }

            int years = toDate.Year - fromDate.Year;
            if (toDate.Month < fromDate.Month || (toDate.Month == fromDate.Month && toDate.Day < fromDate.Day))
                years--;

            DateTime tempDate = fromDate.AddYears(years);

            //int months = toDate.Month - tempDate.Month;
            int months = (toDate.Year - tempDate.Year) * 12 + (toDate.Month - tempDate.Month);

            if (toDate.Day < tempDate.Day)
                months--;

            if (months < 0)
            {
                months += 12;
                years--;
            }

              tempDate = tempDate.AddMonths(months);

            int days = (toDate - tempDate).Days;

            return $"{years}Y: {months}M: {days}D";
        }


        public async Task<List<ProfessionalDto>> Professional(int employeeId)
        {
            var query = await (
                from a in _context.HrEmpProfdtls
                join b in _context.HrEmpMasters on a.EmpId equals b.EmpId into empGroup
                from b in empGroup.DefaultIfEmpty()
                join c in _context.CurrencyMasters on a.CurrencyId equals c.CurrencyId into currencyGroup
                from c in currencyGroup.DefaultIfEmpty()
                join d in _context.ProfessionalRequestWorkFlowstatuses on a.ProfId equals d.RequestId into workflowGroup
                from d in workflowGroup.DefaultIfEmpty()
                where a.EmpId == employeeId &&
                      (d.ApprovalStatus == "P" ||
                       _context.HrEmpProfdtlsApprls.Any(x => x.ProfId == a.ProfId && x.EmpId == employeeId) ||
                       a.ApprlId == null)
                select new
                {
                    a.ProfId,
                    a.EmpId,
                    a.JoinDt,
                    a.LeavingDt,
                    a.LeaveReason,
                    a.Designation,
                    a.CompName,
                    a.CompAddress,
                    a.Pbno,
                    a.JobDesc,
                    a.ContactPer,
                    a.ContactNo,
                    CurrencyId = c.CurrencyId,
                    Currency = c.Currency,
                    a.Ctc,
                    Status = d != null && d.ApprovalStatus == "P" ? d.ApprovalStatus : "A"
                }
            ).ToListAsync();

            var result = new List<ProfessionalDto>();

            foreach (var item in query)
            {
                var years = (item.JoinDt.HasValue && item.LeavingDt.HasValue)
                    ? ((item.LeavingDt.Value.Year - item.JoinDt.Value.Year) -
                       ((item.LeavingDt.Value.Month < item.JoinDt.Value.Month ||
                         (item.LeavingDt.Value.Month == item.JoinDt.Value.Month && item.LeavingDt.Value.Day < item.JoinDt.Value.Day)) ? 1 : 0))
                    : 0;

                var months = (item.JoinDt.HasValue && item.LeavingDt.HasValue)
                    ? ((item.LeavingDt.Value.Month - item.JoinDt.Value.Month) +
                       ((item.LeavingDt.Value.Day < item.JoinDt.Value.Day) ? -1 : 0) + 12) % 12
                    : 0;

                var days = (item.JoinDt.HasValue && item.LeavingDt.HasValue)
                    ? (item.LeavingDt.Value - item.JoinDt.Value.AddMonths((item.LeavingDt.Value.Month - item.JoinDt.Value.Month) +
                                                                          ((item.LeavingDt.Value.Day < item.JoinDt.Value.Day) ? -1 : 0))).Days
                    : 0;

                var lengthOfService = item.JoinDt.HasValue && item.LeavingDt.HasValue
                    ? await GetEmployeeExperienceLength(item.EmpId, (item.LeavingDt.Value - item.JoinDt.Value).Days)
                    : "0Y: 0M: 0D";

                result.Add(new ProfessionalDto
                {
                    Prof_Id = item.ProfId,
                    Emp_Id = item.EmpId,
                    Join_Dt = item.JoinDt.HasValue ? item.JoinDt.Value.ToString("dd/MM/yyyy") : "NA",
                    Leaving_Dt = item.LeavingDt.HasValue ? item.LeavingDt.Value.ToString("dd/MM/yyyy") : "NA",
                    Leave_Reason = item.LeaveReason ?? "NA",
                    Designation = item.Designation ?? "NA",
                    Comp_Name = item.CompName ?? "NA",
                    Comp_Address = item.CompAddress ?? "NA",
                    PBno = item.Pbno ?? "NA",
                    Job_Desc = item.JobDesc ?? "NA",
                    Contact_Per = item.ContactPer ?? "NA",
                    Contact_No = item.ContactNo ?? "NA",
                    Currency_Id = item.CurrencyId,
                    Currency = item.Currency ?? "NA",
                    Ctc = item.Ctc ?? "NA",
                    Status = item.Status,
                    Years = years,
                    Months = months,
                    Days = days,
                    LengthOfService = lengthOfService
                });
            }

            return result;
        }

        public async Task<List<AssetDto>> Asset()
        {
            var result = await (
                 from a in _context.CompanyParameters
                 where a.ParameterCode == "ASTDYN" && a.Type == "COM"
                 select new AssetDto
                 {
                     DynamicAsset = a.Value == 0 ? 0 : a.Value
                 }).ToListAsync();

            return result;
        }

        public async Task<List<AssetDetailsDto>> AssetDetails(int employeeId)
        {


            var paramVal01 = (from a in _context.CompanyParameters
                              where a.ParameterCode == "AST" && a.Type == "COM"
                              select new
                              {
                                  a.Value
                              }).FirstOrDefault();
            int paramVal = paramVal01?.Value ?? 0;

            var paramDynVal01 = (from a in _context.CompanyParameters
                                 where a.ParameterCode == "ASTDYN" && a.Type == "COM"
                                 select new
                                 {
                                     a.Value
                                 }).FirstOrDefault();
            int paramDynVal = paramDynVal01?.Value ?? 0;


            if (paramDynVal == 1)
            {
                var result = await (from a in _context.EmployeesAssetsAssigns
                                    join b in _context.EmployeeDetails on a.EmpId equals b.EmpId
                                    join c in _context.GeneralCategories on Convert.ToInt32(a.AssetGroup) equals c.Id
                                    join f in _context.GeneralCategoryFields on c.Id equals f.GeneralCategoryId into fg
                                    from f in fg.DefaultIfEmpty()
                                    join d in _context.ReasonMasters on Convert.ToInt32(a.Asset) equals d.ReasonId into dg
                                    from d in dg.DefaultIfEmpty()
                                    join e in _context.ReasonMasterFieldValues on new { ReasonId = (int?)d.ReasonId, CategoryFieldId = (int?)f.CategoryFieldId } equals new { e.ReasonId, e.CategoryFieldId } into eg
                                    from e in eg.DefaultIfEmpty()
                                    where a.EmpId == employeeId && a.IsActive == true
                                    select new AssetDetailsDto
                                    {
                                        AssetId = c.Id,
                                        AssetGroup = c.Description,
                                        FieldDescription = f.FieldDescription,
                                        Asset = d.Description,
                                        AssetModel = a.AssetModel ?? "NA",
                                        Monitor = a.Monitor ?? "NA",
                                        IWDate = a.InWarranty.HasValue ? a.InWarranty.Value.ToString("dd/MM/yyyy") : "NA",
                                        OWDate = a.OutOfWarranty.HasValue ? a.OutOfWarranty.Value.ToString("dd/MM/yyyy") : "NA",
                                        FieldValues = e.FieldValues,
                                        ReceivedDate = a.ReceivedDate.HasValue ? a.ReceivedDate.Value.ToString("dd/MM/yyyy") : "N/A",
                                        Status = a.Status,
                                        ParamVal = paramDynVal,
                                        ParamDynVal = paramDynVal,
                                        AssignID = a.AssignId,
                                        Remarks = a.Remarks ?? "",
                                        ExpiryDate = a.ExpiryDate.HasValue ? a.ExpiryDate.Value.ToString("dd/MM/yyyy") : "NA",
                                        ReturnDate = a.ReturnDate.HasValue ? a.ReturnDate.Value.ToString("dd/MM/yyyy") : "NA",
                                        EmplinkID = c.EmplinkId ?? 0

                                    }).ToListAsync();
                return result;
            }
            else if (paramVal == 1)
            {

                var result = await (from a in _context.EmployeesAssetsAssigns
                                    join c in _context.GeneralCategories on Convert.ToInt32(a.AssetGroup) equals c.Id
                                    join d in _context.EmployeeDetails on a.EmpId equals d.EmpId
                                    join b in _context.ReasonMasters on Convert.ToInt32(a.Asset) equals b.ReasonId into rm
                                    from b in rm.DefaultIfEmpty()
                                    where a.EmpId == employeeId && a.IsActive == true
                                    select new AssetDetailsDto
                                    {
                                        AssetGroup = c.Description,
                                        Asset = b.Description,
                                        AssetNo = a.AssetNo,
                                        AssetModel = a.AssetModel == null ? "NA" : a.AssetModel,
                                        Monitor = a.Monitor == null ? "NA" : a.Monitor,
                                        InWarranty = a.InWarranty.HasValue ? a.InWarranty.Value.ToString("dd/MM/yyyy") : "NA",
                                        OutOfWarranty = a.OutOfWarranty.HasValue ? a.OutOfWarranty.Value.ToString("dd/MM/yyyy") : "NA",
                                        ReceivedDate = a.ReceivedDate.HasValue ? a.ReceivedDate.Value.ToString("dd/MM/yyyy") : "NA",
                                        Status = a.Status,
                                        ParamVal = paramVal,
                                        ParamDynVal = 0,
                                        AssignID = a.AssignId,
                                        Remarks = a.Remarks == null ? "NA" : a.Remarks,
                                        ExpiryDate = a.ExpiryDate.HasValue ? a.ExpiryDate.Value.ToString("dd/MM/yyyy") : "NA",
                                        ReturnDate = a.ReturnDate.HasValue ? a.ReturnDate.Value.ToString("dd/MM/yyyy") : "NA"





                                    }).ToListAsync();
                return result;

            }
            else
            {
                var result = await (from aa in _context.AssignedAssets
                                    join hcf in _context.HrmsCommonField01s on Convert.ToInt32(aa.AssestId) equals hcf.ComMasId
                                    join cf in _context.CommonFields on hcf.ComFieldId equals Convert.ToInt32(cf.ComFieldId)
                                    join rl in _context.HrEmployeeUserRelations on aa.CreatedBy equals rl.UserId
                                    join empd in _context.EmployeeDetails on rl.EmpId equals empd.EmpId
                                    join efd1 in _context.EmployeeDetails on aa.EmpId equals efd1.EmpId
                                    where cf.ComId == cf.ComId && aa.EmpId == employeeId
                                    select new AssetDetailsDto
                                    {
                                        AssestRequestID = aa.AssestRequestId,
                                        AssignID = aa.AssignId,
                                        Employee = efd1.Name,
                                        AssestType = aa.AssestType,
                                        Asset = hcf.CommonVal,
                                        AssignedBy = empd.Name,
                                        Status = aa.Status,
                                        AssignedDate = aa.CreatedDate.HasValue ? aa.CreatedDate.Value.ToString("dd-MM-yyyy") : null,
                                        ComMasId = hcf.ComMasId,
                                        ParamVal = paramVal,

                                    }).OrderByDescending(x => x.AssignedDate).ToListAsync();


                return result;
            }



        }



        public async Task<List<CurrencyDropdown_ProfessionalDto>> CurrencyDropdown_Professional()
        {
            var result = await (
                 from a in _context.CurrencyMasters
                 select new CurrencyDropdown_ProfessionalDto
                 {
                     Currency_Id = a.CurrencyId,
                     Currency = a.Currency
                 }).ToListAsync();

            return result;
        }
        public string GetSequence(int employeeId, int mainMasterId, string entity = "", int firstEntity = 0)
        {
            string sequence = null;
            int? codeId = null;

            // Check if 'levels' == '17'
            //bool isLevel17 = _context.LevelSettingsAccess00s
            //    .Any(ls => _context.TransactionMasters
            //        .Where(tm => tm.TransactionType == "Seq_Gen")
            //        .Select(tm => tm.TransactionId)
            //        .Contains(ls.TransactionId) && ls.Levels == "17");

            bool isLevel17 = (from ls in _context.LevelSettingsAccess00s
                              join tm in _context.TransactionMasters
                              on ls.TransactionId equals tm.TransactionId
                              where tm.TransactionType == "Seq_Gen" && ls.Levels == "17"
                              select ls).Any();

            if (isLevel17)
            {
                if (!string.IsNullOrEmpty(entity))
                {
                    // Query for entity-based logic
                    //var entityQuery = _context.EntityApplicable00s
                    //    .Join(_context.AdmCodegenerationmasters,
                    //        a => a.MasterId,
                    //        c => c.CodeId,
                    //        (a, c) => new { a, c })
                    //    .Where(x => _context.TransactionMasters
                    //            .Where(tm => tm.TransactionType == "Seq_Gen")
                    //            .Select(tm => tm.TransactionId)
                    //            .Contains(x.a.TransactionId) &&
                    //        x.a.MainMasterId == mainMasterId &&
                    //        x.a.LinkLevel != 1 &&
                    //        SplitStrings_XML(entity, ',').Contains(x.a.LinkId.ToString()))
                    //    .OrderBy(x => x.a.LinkLevel)
                    //    .FirstOrDefault();
                    var entityQuery = (from a in _context.EntityApplicable00s
                                       join c in _context.AdmCodegenerationmasters
                                       on a.MasterId equals c.CodeId
                                       join tm in _context.TransactionMasters
                                       on a.TransactionId equals tm.TransactionId
                                       where tm.TransactionType == "Seq_Gen" &&
                                             a.MainMasterId == mainMasterId &&
                                             a.LinkLevel != 1 &&
                                             SplitStrings_XML(entity, ',').Contains(a.LinkId.ToString())
                                       orderby a.LinkLevel
                                       select new { a, c })
                  .FirstOrDefault();

                    if (entityQuery != null)
                    {
                        //sequence = entityQuery.a.ToString;
                        codeId = entityQuery.c.CodeId;
                    }

                    if (sequence == null && firstEntity != 0)
                    {
                        var firstEntityQuery = (from a in _context.EntityApplicable00s
                                                join c in _context.AdmCodegenerationmasters
                                                on a.MasterId equals c.CodeId
                                                join tm in _context.TransactionMasters
                                                on a.TransactionId equals tm.TransactionId
                                                where tm.TransactionType == "Seq_Gen" &&
                                                      a.MainMasterId == mainMasterId &&
                                                      a.LinkLevel == 1 &&
                                                      a.LinkId == firstEntity
                                                orderby a.LinkLevel
                                                select new { a, c })
                        .FirstOrDefault();

                        if (firstEntityQuery != null)
                        {
                            //sequence = firstEntityQuery.a.LastSequence;
                            codeId = firstEntityQuery.c.CodeId;
                        }
                    }

                    if (sequence == null)
                    {
                        var defaultQuery = (from a in _context.EntityApplicable00s
                                            join c in _context.AdmCodegenerationmasters
                                            on a.MasterId equals c.CodeId
                                            join tm in _context.TransactionMasters
                                            on a.TransactionId equals tm.TransactionId
                                            where tm.TransactionType == "Seq_Gen" &&
                                                  a.MainMasterId == mainMasterId &&
                                                  a.LinkLevel == 15
                                            select new { a, c })
                       .FirstOrDefault();

                        if (defaultQuery != null)
                        {
                            //sequence = defaultQuery.a.LastSequence;
                            codeId = defaultQuery.c.CodeId;
                        }
                    }
                }
                else
                {
                    // Entity is empty, handle alternate logic
                    var empQuery = (from emp in _context.HrEmpMasters
                                    where emp.EmpId == employeeId
                                    from e in SplitStrings_XML(emp.EmpEntity, ',')
                                    join ea in _context.EntityApplicable00s
                                    on e equals ea.LinkId.ToString()
                                    join adm in _context.AdmCodegenerationmasters
                                    on ea.MasterId equals adm.CodeId
                                    join tm in _context.TransactionMasters
                                    on ea.TransactionId equals tm.TransactionId
                                    where tm.TransactionType == "Seq_Gen" &&
                                          ea.MainMasterId == mainMasterId &&
                                          ea.LinkLevel != 1
                                    orderby ea.LinkLevel
                                    select new { emp, ea, adm })
                  .FirstOrDefault();

                    if (empQuery != null)
                    {
                        //sequence = empQuery.ea.LastSequence;
                        codeId = empQuery.adm.CodeId;
                    }

                    if (sequence == null)
                    {
                        var fallbackQuery = (from emp in _context.HrEmpMasters
                                             where emp.EmpId == employeeId
                                             join ea in _context.EntityApplicable00s
                                             on Convert.ToInt64(emp.EmpFirstEntity) equals ea.LinkId
                                             join adm in _context.AdmCodegenerationmasters
                                             on ea.MasterId equals adm.CodeId
                                             join tm in _context.TransactionMasters
                                             on ea.TransactionId equals tm.TransactionId
                                             where tm.TransactionType == "Seq_Gen" &&
                                                   ea.MainMasterId == mainMasterId &&
                                                   ea.LinkLevel == 1
                                             orderby ea.LinkLevel
                                             select new { ea, adm })
                    .FirstOrDefault();

                        if (fallbackQuery != null)
                        {
                            //sequence = fallbackQuery.ea.LastSequence;
                            codeId = fallbackQuery.adm.CodeId;
                        }
                    }
                }
            }
            else
            {
                // Handle 'level == 16' and other cases
                // Add similar LINQ logic here based on the SQL conditions
            }

            return codeId?.ToString();
        }
        public async Task<HrEmpProfdtlsApprlDto> InsertOrUpdateProfessionalData(HrEmpProfdtlsApprlDto profdtlsApprlDto)
        {
            var existingEntity = await _context.HrEmpProfdtlsApprls
                                  .FirstOrDefaultAsync(e => e.EmpId == profdtlsApprlDto.EmpId &&
                                                            e.JoinDt.HasValue && e.JoinDt.Value.Date == profdtlsApprlDto.JoinDt &&
                                                            e.LeavingDt.HasValue && e.LeavingDt.Value.Date == profdtlsApprlDto.LeavingDt);
            if (existingEntity == null)
            {

                var workFlowNeed = await (from a in _context.CompanyParameters
                                          join b in _context.HrmValueTypes
                                          on a.Value equals b.Value
                                          where b.Type == "EmployeeReporting"
                                                && a.ParameterCode == "WRKFLO"
                                          select b.Code).FirstOrDefaultAsync();

                if (workFlowNeed.Equals("Yes"))
                {
                    var transactionID = await (from a in _context.TransactionMasters where a.TransactionType == "Professional" select a.TransactionId).FirstOrDefaultAsync();

                    var codeId = GetSequence(profdtlsApprlDto.EmpId, transactionID, "", 0);
                    if (codeId != null)
                    {
                        var requestID = await (from a in _context.AdmCodegenerationmasters where a.Code == codeId select a.LastSequence).FirstOrDefaultAsync();
                        var hrEmpProfdtlsApprl = _mapper.Map<HrEmpProfdtlsApprl>(profdtlsApprlDto);
                        hrEmpProfdtlsApprl.RequestId = requestID;
                        await _context.HrEmpProfdtlsApprls.AddAsync(hrEmpProfdtlsApprl);

                        var codeGen = await _context.AdmCodegenerationmasters.Where(c => c.CodeId == Convert.ToInt32(codeId))
                                            .Select(c => new
                                            {
                                                c.CodeId,
                                                c.CurrentCodeValue,
                                                c.NumberFormat,
                                                c.Code
                                            })
                                            .FirstOrDefaultAsync();
                        if (codeGen != null)
                        {
                            // Calculate new CurrentCodeValue and LastSequence
                            var currentCodeValue = (codeGen.CurrentCodeValue ?? 0) + 1;

                            var lastSequence = codeGen.Code +
                                               codeGen.NumberFormat.Substring(0, codeGen.NumberFormat.Length - currentCodeValue.ToString().Length) +
                                               currentCodeValue;

                            // Update the entity
                            var codeGenEntity = await _context.AdmCodegenerationmasters
                                .FirstOrDefaultAsync(c => c.CodeId == Convert.ToInt32(codeId));

                            if (codeGenEntity != null)
                            {
                                codeGenEntity.CurrentCodeValue = currentCodeValue;
                                codeGenEntity.LastSequence = lastSequence;

                                // Save changes to the database
                                await _context.SaveChangesAsync();
                            }
                        }
                    }
                }
                else
                {
                    var hrEmpProfdtlsApprl = _mapper.Map<HrEmpProfdtlsApprl>(profdtlsApprlDto);
                    await _context.HrEmpProfdtlsApprls.AddAsync(hrEmpProfdtlsApprl);

                    var profdtlsApprlDtow = await _context.HrEmpProfdtlsApprls
                                            .Where(x => x.EmpId == profdtlsApprlDto.EmpId)
                                            .Select(x => new HrEmpProfdtlsApprl
                                            {

                                                InstId = x.InstId,
                                                EmpId = x.EmpId,
                                                CompName = x.CompName,
                                                Designation = x.Designation,
                                                CompAddress = x.CompAddress,
                                                Pbno = x.Pbno,
                                                ContactPer = x.ContactPer,
                                                ContactNo = x.ContactNo,
                                                JobDesc = x.JobDesc,
                                                JoinDt = x.JoinDt,
                                                LeavingDt = x.LeavingDt,
                                                LeaveReason = x.LeaveReason,
                                                Ctc = x.Ctc,
                                                CurrencyId = x.CurrencyId,
                                                EntryBy = x.EntryBy,
                                                EntryDt = x.EntryDt
                                            })
                                            .FirstOrDefaultAsync();


                    var hrEmpProfdtl = _mapper.Map<HrEmpProfdtl>(profdtlsApprlDtow);
                    hrEmpProfdtl.EntryDt = DateTime.UtcNow;
                    await _context.HrEmpProfdtls.AddAsync(hrEmpProfdtl);

                    await _context.SaveChangesAsync();

                    var employee = await _context.HrEmpMasters.FirstOrDefaultAsync(e => e.EmpId == profdtlsApprlDto.EmpId);

                    if (employee != null)
                    {
                        // Update the ModifiedDate property
                        employee.ModifiedDate = DateTime.UtcNow;
                    }

                    // Save changes in a single transaction
                    await _context.SaveChangesAsync();
                }
            }
            return _mapper.Map<HrEmpProfdtlsApprlDto>(profdtlsApprlDto);
        }
        public async Task<List<HrEmpProfdtlsApprlDto>> GetProfessionalByIdAsync(string updateType, int detailID, int empID)
        {
            List<HrEmpProfdtlsApprlDto> result = new();

            if (updateType == "Pending")
            {
                result = await (from a in _context.HrEmpProfdtlsApprls
                                join b in _context.CurrencyMasters
                                on a.CurrencyId equals b.CurrencyId into currencyGroup
                                from b in currencyGroup.DefaultIfEmpty()
                                where a.ProfId == detailID && a.EmpId == empID
                                select new HrEmpProfdtlsApprlDto
                                {
                                    ProfId = a.ProfId,
                                    EmpId = a.EmpId,
                                    CompName = a.CompName,
                                    Designation = a.Designation,
                                    CompAddress = a.CompAddress,
                                    Pbno = a.Pbno,
                                    ContactPer = a.ContactPer,
                                    ContactNo = a.ContactNo,
                                    JobDesc = a.JobDesc,
                                    JoinDt = a.JoinDt,
                                    LeavingDt = a.LeavingDt,
                                    LeaveReason = a.LeaveReason,
                                    Ctc = a.Ctc,
                                    CurrencyId = b.CurrencyId,
                                }).ToListAsync();
            }
            else if (updateType == "Approved")
            {
                result = await (from a in _context.HrEmpProfdtls
                                join b in _context.CurrencyMasters
                                on a.CurrencyId equals b.CurrencyId into currencyGroup
                                from b in currencyGroup.DefaultIfEmpty()
                                where a.ProfId == detailID && a.EmpId == empID
                                select new HrEmpProfdtlsApprlDto
                                {
                                    ProfId = a.ProfId,
                                    EmpId = a.EmpId,
                                    CompName = a.CompName,
                                    Designation = a.Designation,
                                    CompAddress = a.CompAddress,
                                    Pbno = a.Pbno,
                                    ContactPer = a.ContactPer,
                                    ContactNo = a.ContactNo,
                                    JobDesc = a.JobDesc,
                                    JoinDt = a.JoinDt,
                                    LeavingDt = a.LeavingDt,
                                    LeaveReason = a.LeaveReason,
                                    Ctc = a.Ctc,
                                    CurrencyId = b.CurrencyId,
                                }).ToListAsync();
            }

            return result;
        }

        public Task<PersonalDetailsHistoryDto> InsertOrUpdatePersonalData(PersonalDetailsHistoryDto persnldtlsApprlDto)
        {
            throw new NotImplementedException();
        }


        public async Task<List<AllDocumentsDto>> Documents(int employeeId,List<string> excludedDocTypes)
        {

            var tempDocumentFill = from a in _context.ReasonMasterFieldValues
                                   join b in _context.GeneralCategoryFields on a.CategoryFieldId equals b.CategoryFieldId
                                   join c in _context.HrmsDatatypes on b.DataTypeId equals c.DataTypeId into dataTypeJoin
                                   from c in dataTypeJoin.DefaultIfEmpty()
                                   join d in _context.AdmCountryMasters on a.FieldValues equals d.CountryId.ToString() into countryJoin
                                   from d in countryJoin.DefaultIfEmpty()
                                   join e in _context.ReasonMasters on a.FieldValues equals e.ReasonId.ToString() into reasonJoin
                                   from e in reasonJoin.DefaultIfEmpty()
                                   select new DocumentFillDto
                                   {
                                       ReasonId = a.ReasonId,
                                       CategoryFieldId = a.CategoryFieldId,
                                       FieldValues = null,
                                       FieldDescription = b.FieldDescription,
                                       DataTypeId = b.DataTypeId
                                   };
            var tempDocumentFillList = await tempDocumentFill.ToListAsync();
            var tempDocumentFillDict = tempDocumentFillList.ToDictionary(x => x.ReasonId, x => x);

            // Step 2: Retrieve Approved Documents


            var ApprovedDos = await (
                                        from t1 in _context.HrmsEmpdocumentsApproved00s
                                        join t2 in _context.HrmsEmpdocumentsApproved01s on t1.DetailId equals t2.DetailId
                                        join t3 in _context.HrmsDocumentField00s on Convert.ToInt32(t2.DocFields) equals t3.DocFieldId into docFieldJoin
                                        from t3 in docFieldJoin.DefaultIfEmpty()
                                        join t4 in _context.HrmsDocument00s on Convert.ToInt32(t1.DocId) equals t4.DocId
                                        join t5 in _context.HrmsDocTypeMasters on Convert.ToInt32(t4.DocType) equals t5.DocTypeId
                                        join t6 in _context.HrmsDatatypes on t3.DataTypeId equals t6.DataTypeId into dataTypeJoin
                                        from t6 in dataTypeJoin.DefaultIfEmpty()
                                        join t7 in _context.AdmCountryMasters on t2.DocValues equals t7.CountryId.ToString() into countryJoin
                                        from t7 in countryJoin.DefaultIfEmpty()
                                        join t8 in _context.ReasonMasters on t2.DocValues equals t8.ReasonId.ToString() into reasonJoin
                                        from t8 in reasonJoin.DefaultIfEmpty()
                                        where t1.EmpId == employeeId
                                                && t1.Status == "A"
                                                && !excludedDocTypes.Contains(t5.DocType)
                                                && t3.DocDescription != "IsActive"

                                        select new DocumentsDto
                                        {
                                            DetailID = t1.DetailId,
                                            DocID = t1.DocId,
                                            EmpID = t1.EmpId,
                                            DocFieldID = t3.DocFieldId,
                                            DocDescription = t3.DocDescription,
                                            DocValues = t2.DocValues,
                                            IsGeneralCategory = t6.IsGeneralCategory,
                                            DataType = t6.DataType,
                                            DocName = t4.DocName.ToUpper(),
                                            IsDate = t6.IsDate,
                                            repeatrank = (
                                                from innerT2 in _context.HrmsEmpdocumentsApproved01s
                                                join innerT3 in _context.HrmsDocumentField00s on Convert.ToInt32(innerT2.DocFields) equals innerT3.DocFieldId
                                                where innerT2.DetailId == t1.DetailId
                                                orderby innerT3.DocFieldId descending
                                                select innerT3.DocFieldId
                                            ).Count()
                                        }
                                    ).OrderBy(x => x.DocID).ToListAsync();


            

            var docList = await (from a in _context.HrmsDocument00s
                                 join b in _context.HrmsDocTypeMasters on (a.DocType.HasValue ? (long)a.DocType.Value : -1L) equals b.DocTypeId
                                 //join c in _context.EmpDocumentAccesses  on new { DocID = (long)a.DocId , EmpID = employeeId } equals new { c.DocId, c.EmpId }
                                 join c in _context.EmpDocumentAccesses
                                on new { DocID = (long)a.DocId, EmpID = (long)employeeId }
                                equals new { DocID = c.DocId ?? -1L, EmpID = c.EmpId ?? -1L }
                                 where !(
                                     from e in _context.HrmsEmpdocuments00s
                                     join f in _context.HrmsEmpdocuments01s on e.DetailId equals f.DetailId
                                     where e.EmpId == employeeId && e.Status != "R" && e.Status != "D" && e.DocId == a.DocId
                                     select e.DocId
                                 ).Any()
                                 && (!excludedDocTypes.Contains(b.DocType) ||
                                     (from d in _context.HrmsDocument00s
                                      join e in _context.HrmsDocTypeMasters on (a.DocType.HasValue ? (long)a.DocType.Value : -1L) equals e.DocTypeId
                                      where d.IsAllowMultiple == 1 && !excludedDocTypes.Contains(e.DocType)
                                      select d.DocId).Contains(a.DocId))
                                 select new DocumentListDto
                                 {
                                     DocID = a.DocId,
                                     DocName = a.DocName.ToUpper(),
                                     DocDescription = "Submit Document"
                                 }).Distinct().ToListAsync();


            // Step 3: Retrieve Pending Documents


            var pendingDocuments = await (from t1 in _context.HrmsEmpdocuments00s
                                          join t2 in _context.HrmsEmpdocuments01s on t1.DetailId equals t2.DetailId
                                          join t3 in _context.HrmsDocumentField00s on Convert.ToInt32(t2.DocFields) equals t3.DocFieldId into docFieldJoin
                                          from t3 in docFieldJoin.DefaultIfEmpty()
                                          join t4 in _context.HrmsDocument00s on Convert.ToInt32(t1.DocId) equals t4.DocId
                                          join t5 in _context.HrmsDocTypeMasters on Convert.ToInt32(t4.DocType) equals t5.DocTypeId
                                          join t6 in _context.HrmsDatatypes on t3.DataTypeId equals t6.DataTypeId into dataTypeJoin
                                          from t6 in dataTypeJoin.DefaultIfEmpty()
                                          join t7 in _context.AdmCountryMasters on t2.DocValues equals t7.CountryId.ToString() into countryJoin
                                          from t7 in countryJoin.DefaultIfEmpty()
                                          join t8 in _context.ReasonMasters on t2.DocValues equals t8.ReasonId.ToString() into reasonJoin
                                          from t8 in reasonJoin.DefaultIfEmpty()
                                          where t1.EmpId == employeeId &&
                                                !_context.HrmsEmpdocumentsApproved00s.Any(ap => ap.DetailId == t1.DetailId && ap.EmpId == employeeId && ap.Status == "A") &&
                                                t1.Status == "P" && t3.DocDescription != "IsActive" && !excludedDocTypes.Contains(t5.DocType)
                                          select new DocumentsDto
                                          {
                                              DetailID = t1.DetailId,
                                              DocID = t1.DocId,
                                              EmpID = t1.EmpId,
                                              DocFieldID = Convert.ToInt32(t3.DocFieldId),
                                              DocDescription = t3.DocDescription,

                                              IsGeneralCategory = t6.IsGeneralCategory,
                                              DataType = t6.DataType,
                                              DocName = t4.DocName.ToUpper(),
                                              IsDate = t6.IsDate,
                                              repeatrank = (from innerT1 in _context.HrmsEmpdocuments00s
                                                            join innerT2 in _context.HrmsEmpdocuments01s on innerT1.DetailId equals innerT2.DetailId
                                                            where innerT1.DetailId == t1.DetailId
                                                            orderby t3.DocFieldId descending
                                                            select innerT1).Count(),

                                              MatchedDoc = tempDocumentFillDict.ContainsKey(t8.ReasonId) ? tempDocumentFillDict[t8.ReasonId] : null
                                          }).ToListAsync();



            //--Pending files
            var files = from a in _context.HrmsEmpdocuments02s
                        join b in _context.HrmsEmpdocuments00s on a.DetailId equals b.DetailId
                        join c in _context.HrmsDocument00s on b.DocId equals Convert.ToInt16(c.DocId)
                        where b.EmpId == employeeId
                        select new FilesDto
                        {
                            DocID = b.DocId,
                            DetailID = a.DetailId,
                            FileName = c.FolderName + a.FileName,
                            Status = b.Status
                        };
            var pendingFiles = files.Where(f => f.Status == "P").ToList();
            var approvedFiles = files.Where(f => f.Status == "A").ToList();

            return new List<AllDocumentsDto>
     {
                 new AllDocumentsDto
                 {
                     TempDocumentFill = tempDocumentFillList,
                     ApprovedDocuments = ApprovedDos,
                     DocumentList = docList,
                     PendingDocumentsDto = pendingDocuments,
                     PendingFiles = pendingFiles,
                     ApprovedFiles = approvedFiles
                 }
      };

        }

        public Task<List<PersonalDetailsDto>> GetPersonalDetailsById(int employeeid)
        {
            throw new NotImplementedException();
        }

        public Task<List<TrainingDto>> Training(int employeeid)
        {
            throw new NotImplementedException();
        }
        public async Task<List<CareerHistoryDto>> CareerHistory(int employeeid)
        {

            var previousExpData = _context.HrEmpProfdtls
                .Where(empProf => empProf.EmpId == employeeid)
                .Select(empProf => new
                {
                    DaysWorked = EF.Functions.DateDiffDay(empProf.JoinDt, empProf.LeavingDt) ?? 0
                })
                .ToList(); 

            var previousExp = new
            {
                Category = "Previous Experience",
                Relevant = 0.00,
                NonRelevent = previousExpData.Sum(e => e.DaysWorked),
                Total = previousExpData.Sum(e => e.DaysWorked)
            };

            var companyExp = _context.HrEmpMasters
                .Where(empMaster => empMaster.EmpId == employeeid)
                .Select(empMaster => new
                {
                    RelevantDays = EF.Functions.DateDiffDay(empMaster.FirstEntryDate, DateTime.UtcNow) ?? 0
                })
                .FirstOrDefault(); 

            var companyExpResult = new
            {
                Category = "Company Experience (First Entry Date)",
                Relevant = companyExp?.RelevantDays ?? 0,
                NonRelevent = 0.00,
                Total = companyExp?.RelevantDays ?? 0
            };
            var tempSummary = new List<dynamic> { previousExp, companyExpResult };

            //--------------------------Finish #tempSummary--------------------------------------------

            var totalSummary = new
            {
                Category = "Total",
                Relevant = tempSummary.Sum(x => (double)x.Relevant),
                NonRelevent = tempSummary.Sum(x => (double)x.NonRelevent),
                Total = tempSummary.Sum(x => (double)x.Total)
            };

            var detailedSummary = tempSummary.Select(x => new
            {
                Category = x.Category,
                Relevant = x.Relevant ?? 0.00,
                NonRelevent = x.NonRelevent ?? 0.00,
                Total = x.Total ?? 0.00
            }).ToList();

            var tempExperienceDetails = new List<dynamic> { totalSummary }.Concat(detailedSummary).ToList();

            //--------------------------Finish #TempExperienceDetails--------------------------------------------

            var result1 = await Task.WhenAll(tempExperienceDetails.Select(async a =>
            {
                var relevant = a.Relevant == 0.00
                                ? "0Y: 0M: 0D"
                                : await GetEmployeeExperienceLength(employeeid, (int)a.Relevant);
                var nonRelevent = a.NonRelevent == 0.00
                                ? "0Y: 0M: 0D"
                                : await GetEmployeeExperienceLength(employeeid, (int)a.NonRelevent);
                var total = a.Total == 0.00
                                ? "0Y: 0M: 0D"
                                : await GetEmployeeExperienceLength(employeeid, (int)a.Total);

                return new CareerHistoryDto
                {
                    Category = a.Category,
                    Relevant = relevant,
                    NonRelevent = nonRelevent,
                    Total = total
                };
            }));

            return result1.ToList();

        }

       public async Task<List<object>> BiometricDetails(int employeeId)
        {
            var result = await (from a in _context.BiometricsDtls
                                join b in _context.HrEmpMasters on a.EmployeeId equals b.EmpId into bio
                                from b in bio.DefaultIfEmpty()
                                where a.EmployeeId == employeeId
                                select new 
                                {
                                    CompanyID = a.CompanyId.ToString(),
                                    EmployeeID= a.EmployeeId.ToString(),
                                    DeviceID=a.DeviceId.ToString(),
                                    UserID= a.UserId,
                                    AttMarkId = b != null && b.IsMarkAttn.HasValue
                                    ? (b.IsMarkAttn.Value ? "1" : "2"):""
                                }).ToListAsync();
            return result.Cast<object>().ToList();
       }
    }
}

