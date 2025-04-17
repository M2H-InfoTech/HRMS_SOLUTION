using AutoMapper;
using EMPLOYEE_INFORMATION.Data;
using EMPLOYEE_INFORMATION.DTO.DTOs;
using EMPLOYEE_INFORMATION.HRMS.EmployeeInformation.Models.Models.Entity;
using EMPLOYEE_INFORMATION.Models;
using EMPLOYEE_INFORMATION.Models.Entity;
using EMPLOYEE_INFORMATION.Models.EnumFolder;
using HRMS.EmployeeInformation.DTO.DTOs;
using HRMS.EmployeeInformation.DTO.DTOs.Documents;
using HRMS.EmployeeInformation.DTO.DTOs.PayScale;
using HRMS.EmployeeInformation.Models;
using HRMS.EmployeeInformation.Models.Models.Entity;
using HRMS.EmployeeInformation.Models.Models.EnumFolder;
using HRMS.EmployeeInformation.Repository.Common.RepositoryC;
using HRMS.EmployeeInformation.Repository.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using MPLOYEE_INFORMATION.DTO.DTOs;


namespace HRMS.EmployeeInformation.Repository.Common
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly EmployeeDBContext _context;
        //private IStringLocalizer _stringLocalizer;
        private readonly IMemoryCache _memoryCache;
        private readonly EmployeeSettings _employeeSettings;
        private readonly IMapper _mapper;
        private readonly IRepository<QualificationAttachment> _qualificationAttachmentRepository;
        private readonly IRepository<AssignedLetterType> _assignedLetterTypeRepository;
        private readonly ILogger<EmployeeRepository> _logger;
        public EmployeeRepository(EmployeeDBContext dbContext, EmployeeSettings employeeSettings, IMemoryCache memoryCache, IMapper mapper, IWebHostEnvironment env, IRepositoryC employeeRepositoryC, IRepository<QualificationAttachment> qualificationAttachmentRepository, IRepository<AssignedLetterType> assignedLetterTypeRepository, ILogger<EmployeeRepository> logger)
        {
            _context = dbContext;
            _employeeSettings = employeeSettings;
            _memoryCache = memoryCache;
            _mapper = mapper;
            _qualificationAttachmentRepository = qualificationAttachmentRepository;
            _assignedLetterTypeRepository = assignedLetterTypeRepository;
            _logger = logger;

        }
        public async Task<string> GetDefaultCompanyParameter(int employeeId, string parameterCode, string type)
        {
            string? defaultValue = byte.MinValue.ToString();
            if (employeeId == 0)
            {
                return "0";// byte.MinValue.ToString();
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
            if (string.IsNullOrEmpty(defaultValue) || defaultValue == _employeeSettings.empSystemStatus)
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
        //private bool IsLinkLevelExists(int? roleId)
        //{
        //    var exists = _context.EntityAccessRights02s.Where(s => s.RoleId == roleId && s.LinkLevel == _employeeSettings.LinkLevel).Select(x => x.LinkLevel).First();
        //    return exists > byte.MinValue;
        //}
        private bool IsLinkLevelExists(int? roleId)
        {
            var exists = _context.EntityAccessRights02s
                .Where(s => s.RoleId == roleId && s.LinkLevel == _employeeSettings.LinkLevel)
                .Select(x => x.LinkLevel)
                .FirstOrDefault(); // Returns default value if no data is found

            return exists > byte.MinValue; // `exists` will be 0 if not found
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
            try
            {
                var infoFormat = await GetDefaultCompanyParameter(employeeInformationParameters.empId, _employeeSettings.CompanyParameterEmpInfoFormat, _employeeSettings.CompanyParameterType);
                int format = Convert.ToInt32(infoFormat);

                if (infoFormat != null)
                {

                    var linkLevelExists = IsLinkLevelExists(employeeInformationParameters.roleId);

                    var CurrentStatusDesc = (from ec in _context.EmployeeCurrentStatuses
                                             where ec.StatusDesc == _employeeSettings.OnNotice
                                             select ec.Status).FirstOrDefault();

                    string? ageFormat = await (from cp in _context.CompanyParameters
                                               join vt in _context.HrmValueTypes on cp.Value equals vt.Value
                                               where vt.Type == _employeeSettings.ValueType && cp.ParameterCode == _employeeSettings.ParameterCode
                                               select vt.Code).FirstOrDefaultAsync();

                    bool existsEmployee = _context.HrEmpMasters.Any(emp => (emp.IsSave ?? 0) == 1);
                    return format switch
                    {
                        0 or 1 => await HandleFormatZeroOrOne(employeeInformationParameters, linkLevelExists, ageFormat, CurrentStatusDesc, existsEmployee),
                        2 => await HandleFormatTwo(employeeInformationParameters, linkLevelExists, ageFormat, CurrentStatusDesc, existsEmployee),
                        3 => await HandleFormatThree(employeeInformationParameters, linkLevelExists, ageFormat, CurrentStatusDesc, existsEmployee),
                        4 => await HandleFormatFour(employeeInformationParameters, linkLevelExists, ageFormat, CurrentStatusDesc, existsEmployee),
                        _ => throw new InvalidOperationException("Invalid format value.")
                    };

                }
                return new PaginatedResult<EmployeeResultDto>();
            }
            catch (Exception ex)
            {
                _logger.LogWarning("{Timestamp} | Method: {MethodName} | Exception: {ExceptionType}",
                   DateTime.Now,
                   nameof(GetEmpData),
                   ex.GetType().Name);

                return new PaginatedResult<EmployeeResultDto>();

            }

        }
        private async Task<PaginatedResult<EmployeeResultDto>> HandleFormatZeroOrOne(EmployeeInformationParameters employeeInformationParameters, bool linkLevelExists, string? ageFormat, int? currentStatusDesc, bool existsEmployee)
        {
            if (linkLevelExists)
            {
                return await InfoFormatOneOrZeroLinkLevelExist(employeeInformationParameters.empStatus, employeeInformationParameters.systemStatus, employeeInformationParameters.empIds, employeeInformationParameters.filterType, employeeInformationParameters.durationFrom, employeeInformationParameters.durationTo, employeeInformationParameters.probationStatus, currentStatusDesc.ToString(), ageFormat, employeeInformationParameters.pageNumber, employeeInformationParameters.pageSize, employeeInformationParameters.draw, employeeInformationParameters.searchValue, employeeInformationParameters.IsActive);
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
                    return await InforFormatOneOrZeroNotExistLinkSelect(employeeInformationParameters.durationFrom, employeeInformationParameters.durationTo, employeeInformationParameters.empStatus, empIdList, employeeInformationParameters.pageNumber, employeeInformationParameters.pageSize, ageFormat, employeeInformationParameters.draw, employeeInformationParameters.searchValue);
                }
            }

            return new PaginatedResult<EmployeeResultDto>();
        }
        private async Task<PaginatedResult<EmployeeResultDto>> HandleFormatTwo(EmployeeInformationParameters employeeInformationParameters, bool linkLevelExists, string? ageFormat, int? currentStatusDesc, bool existsEmployee)
        {
            if (linkLevelExists)
            {
                return await InfoFormatTwoLinkLevelExists(employeeInformationParameters.empStatus, employeeInformationParameters.systemStatus, employeeInformationParameters.empIds, employeeInformationParameters.filterType, employeeInformationParameters.durationFrom, employeeInformationParameters.durationTo, employeeInformationParameters.probationStatus, currentStatusDesc.ToString(), ageFormat, employeeInformationParameters.pageNumber, employeeInformationParameters.pageSize, employeeInformationParameters.draw, employeeInformationParameters.searchValue, employeeInformationParameters.IsActive);
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
                        return await InforFormatTwoNotExistLinkSelect(employeeInformationParameters.durationFrom, employeeInformationParameters.durationTo, employeeInformationParameters.empStatus, empIdList, employeeInformationParameters.pageNumber, employeeInformationParameters.pageSize, ageFormat, employeeInformationParameters.draw, employeeInformationParameters.searchValue);

                    }

                }
            }
            return new PaginatedResult<EmployeeResultDto>();
        }
        private async Task<PaginatedResult<EmployeeResultDto>> HandleFormatThree(EmployeeInformationParameters employeeInformationParameters, bool linkLevelExists, string? ageFormat, int? currentStatusDesc, bool existsEmployee)
        {
            if (linkLevelExists)
            {
                return await InfoFormatThreeLinkLevelExists(employeeInformationParameters.empStatus, employeeInformationParameters.systemStatus, employeeInformationParameters.empIds, employeeInformationParameters.filterType, employeeInformationParameters.durationFrom, employeeInformationParameters.durationTo, employeeInformationParameters.probationStatus, currentStatusDesc.ToString(), ageFormat, employeeInformationParameters.pageNumber, employeeInformationParameters.pageSize, employeeInformationParameters.draw, employeeInformationParameters.searchValue, employeeInformationParameters.IsActive);
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
                        return await InforFormatThreeNotExistLinkSelect(employeeInformationParameters.durationFrom, employeeInformationParameters.durationTo, employeeInformationParameters.empStatus, empIdList, employeeInformationParameters.pageNumber, employeeInformationParameters.pageSize, ageFormat, employeeInformationParameters.draw, employeeInformationParameters.searchValue);

                    }

                }

            }
            return new PaginatedResult<EmployeeResultDto>();
        }
        private async Task<PaginatedResult<EmployeeResultDto>> HandleFormatFour(EmployeeInformationParameters employeeInformationParameters, bool linkLevelExists, string? ageFormat, int? currentStatusDesc, bool existsEmployee)
        {
            if (linkLevelExists)
            {
                return await InfoFormatFourLinkLevelExists(employeeInformationParameters.empStatus, employeeInformationParameters.systemStatus, employeeInformationParameters.empIds, employeeInformationParameters.filterType, employeeInformationParameters.durationFrom, employeeInformationParameters.durationTo, employeeInformationParameters.probationStatus, currentStatusDesc.ToString(), ageFormat, employeeInformationParameters.pageNumber, employeeInformationParameters.pageSize, employeeInformationParameters.draw);
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
                        return await InforFormatFourNotExistLinkSelect(employeeInformationParameters.durationFrom, employeeInformationParameters.durationTo, employeeInformationParameters.empStatus, empIdList, employeeInformationParameters.pageNumber, employeeInformationParameters.pageSize, ageFormat, employeeInformationParameters.draw, employeeInformationParameters.searchValue);

                    }

                }
            }
            return new PaginatedResult<EmployeeResultDto>();

        }
        //public async Task<List<int?>> GetFilteredEmployeesAsync(HashSet<int> statusSet)
        //{
        //    var nonResignationStatuses = await GetStatusSetAsync(false);
        //    var resignationStatuses = await GetStatusSetAsync(true);
        //    var excludedStatuses = await GetStatusSetAsync(true, statusSet);

        //    var employeeList = await _context.HrEmpMasters.Where(a => (nonResignationStatuses.Contains((int)a.EmpStatus) && resignationStatuses.Any(r => statusSet.Contains(r))) && !excludedStatuses.Contains((int)a.SeperationStatus) || ((nonResignationStatuses.Contains((int)a.EmpStatus) || statusSet.Contains((int)a.EmpStatus)) && resignationStatuses.Any(r => statusSet.Contains(r)) && excludedStatuses.Contains((int)a.SeperationStatus))).Select(a => a.EmpStatus).ToListAsync();





        //    return employeeList;
        //}
        public async Task<List<int?>> GetFilteredEmployeesAsync(HashSet<int> statusSet)
        {


            var nonResignationStatuses = await GetStatusSetAsync(false);
            var resignationStatuses = await GetStatusSetAsync(true);

            var selectedNonResignationStatuses = statusSet.Intersect(nonResignationStatuses).ToHashSet();
            var selectedResignationStatuses = statusSet.Intersect(resignationStatuses).ToHashSet();

            var employeeList = await _context.HrEmpMasters
                .Where(a =>
                    // Active employees: EmpStatus matches and SeperationStatus is not resigned (0 or 1)
                    (selectedNonResignationStatuses.Contains((int)a.EmpStatus)
                     && (!a.SeperationStatus.HasValue || !resignationStatuses.Contains(a.SeperationStatus.Value)))

                    ||

                    // Resigned employees: SeperationStatus is 4/5/8/9
                    (a.SeperationStatus.HasValue && selectedResignationStatuses.Contains(a.SeperationStatus.Value))
                )
                .Select(a => a.SeperationStatus)
                .ToListAsync();

            return employeeList;
        }
        private async Task<PaginatedResult<EmployeeResultDto>> GetEmployeeLinkLevelExistDataAsync(DateTime? durationFrom, DateTime? durationTo, string empSystemStatus, string currentStatusDesc,
        List<int> result, HashSet<int> excludedStatuses, int probationStatus,
        int pageNumber, int pageSize, int draw, string searchValue, bool isActive)
        {
            var empList = await GetFilteredEmployeesAsync(result.ToHashSet());
            var empStatusSet = empList.Where(e => e.HasValue).Select(e => e.Value).ToHashSet();

            // Common filters applied on HR employee data
            var baseQuery = from emp in _context.HrEmpMasters
                            join res in _context.Resignations
                                .Where(r => r.CurrentRequest == 1 &&
                                    r.RejoinStatus == ApprovalStatus.Pending.ToString() &&
                                    !new[] { ApprovalStatus.Deleted.ToString(), ApprovalStatus.Rejceted.ToString() }
                                        .Contains(r.ApprovalStatus) &&
                                    r.ApprovalStatus == (empSystemStatus == currentStatusDesc
                                        ? ApprovalStatus.Pending.ToString()
                                        : ApprovalStatus.Approved.ToString()))
                                on emp.EmpId equals res.EmpId into resGroup
                            from res in resGroup.DefaultIfEmpty()
                            where
                                (durationFrom == null || durationTo == null ||
                                 (emp.JoinDt >= durationFrom && emp.JoinDt <= durationTo) ||
                                 (emp.ProbationDt >= durationFrom && emp.ProbationDt <= durationTo) ||
                                 (emp.RelievingDate >= durationFrom && emp.RelievingDate <= durationTo))
                            && (emp.CurrentStatus == Convert.ToInt32(empSystemStatus)
                                || empSystemStatus == byte.MinValue.ToString()
                                || (empSystemStatus == currentStatusDesc &&
                                    res.ResignationId != null &&
                                    res.RelievingDate >= DateTime.UtcNow))
                            && emp.EmpStatus.HasValue
                            && emp.IsDelete == false
                            && (probationStatus == 1 ||
                                (probationStatus == 2 && emp.IsProbation == true) ||
                                (probationStatus == 3 && emp.IsProbation == false))
                            && (
                                (!isActive && empList.Contains(emp.SeperationStatus)) ||
                                (isActive && !excludedStatuses.Contains(emp.SeperationStatus.Value))
                            )
                            select new
                            {
                                emp.EmpId,
                                emp.EmpCode,
                                //Name = $"{emp.FirstName} {emp.MiddleName} {emp.LastName}",
                                Name = (emp.FirstName ?? "") + " " + (emp.MiddleName ?? "") + " " + (emp.LastName ?? ""),
                                emp.GuardiansName,
                                DateOfBirth = FormatDate(emp.DateOfBirth, _employeeSettings.DateFormat),
                                JoinDate = FormatDate(emp.JoinDt, _employeeSettings.DateFormat),
                                DataDate = FormatDate(emp.JoinDt, _employeeSettings.DateFormat),
                                emp.SeperationStatus,
                                emp.Gender,
                                WorkingStatus = emp.SeperationStatus == (int)SeparationStatus.Live ? "Live" : "Resigned",
                                Age = CalculateAge(emp.DateOfBirth, "Years"),
                                ProbationDt = FormatDate(emp.ProbationDt, _employeeSettings.DateFormat),
                                Probation = emp.IsProbation == false ? "CONFIRMED" : "PROBATION",
                                emp.LastEntity,
                                emp.CurrentStatus,
                                EmpStatus = emp.EmpStatus.ToString(),
                                emp.IsSave,
                                emp.EmpFileNumber,
                                emp.DailyRateTypeId,
                                emp.PayrollMode,
                                ResignationReason = res.Reason,
                                ResignationDate = res.ResignationDate.ToString(),
                                RelievingDate = res.RelievingDate.ToString()
                            };

            // Apply joins and map to DTO
            var finalQuery = from emp in baseQuery
                             join addr in _context.HrEmpAddresses on emp.EmpId equals addr.EmpId into addrGroup
                             from addr in addrGroup.DefaultIfEmpty()
                             join pers in _context.HrEmpPersonals on emp.EmpId equals pers.EmpId into persGroup
                             from pers in persGroup.DefaultIfEmpty()
                             join rep in _context.HrEmpReportings on emp.EmpId equals rep.EmpId into repGroup
                             from rep in repGroup.DefaultIfEmpty()
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
                             join empDetails in _context.EmployeeDetails on rep.ReprotToWhome equals empDetails.EmpId into empDetailsGroup
                             from empDetails in empDetailsGroup.DefaultIfEmpty()
                             join highView in _context.HighLevelViewTables on emp.LastEntity equals highView.LastEntityId into highViewGroup
                             from highView in highViewGroup.DefaultIfEmpty()
                             select new EmployeeResultDto
                             {
                                 EmpId = emp.EmpId,
                                 ImageUrl = img.ImageUrl,
                                 EmpCode = emp.EmpCode,
                                 Name = emp.Name,
                                 GuardiansName = emp.GuardiansName,
                                 JoinDate = emp.JoinDate,
                                 DataDate = emp.DataDate,
                                 EmpStatusDesc = currStatus.StatusDesc,
                                 EmpStatus = empStatusSettings.StatusDesc,
                                 Gender = GetGender(emp.Gender).ToString(),
                                 SeperationStatus = emp.SeperationStatus,
                                 OfficialEmail = addr.OfficialEmail,
                                 PersonalEmail = addr.PersonalEmail,
                                 Phone = addr.Phone,
                                 MaritalStatus = GetMaritalStatus(pers.MaritalStatus).ToString(),
                                 Age = emp.Age,
                                 ProbationDt = emp.ProbationDt,
                                 LevelOneDescription = highView.LevelOneDescription,
                                 LevelTwoDescription = highView.LevelTwoDescription,
                                 ProbationStatus = emp.Probation,
                                 Nationality = country.Nationality,
                                 IsSave = emp.IsSave,
                                 EmpFileNumber = emp.EmpFileNumber,
                                 CurrentStatus = emp.CurrentStatus,
                                 LevelThreeDescription = highView.LevelThreeDescription,
                                 LevelFourDescription = highView.LevelFourDescription,
                                 LevelFiveDescription = highView.LevelFiveDescription,
                                 LevelSixDescription = highView.LevelSixDescription,
                                 LevelSevenDescription = highView.LevelSevenDescription,
                                 LevelEightDescription = highView.LevelEightDescription,
                                 LevelNineDescription = highView.LevelNineDescription,
                                 LevelTenDescription = highView.LevelTenDescription,
                                 LevelElevenDescription = highView.LevelElevenDescription,
                                 LevelTwelveDescription = highView.LevelTwelveDescription,
                                 ReportingEmployeeCode = empDetails.EmpCode,
                                 ReportingEmployeeName = empDetails.Name,
                                 WorkingStatus = emp.WorkingStatus,
                                 RelievingDate = emp.RelievingDate
                             };

            // Apply search filtering
            if (!string.IsNullOrWhiteSpace(searchValue))
            {
                finalQuery = finalQuery.Where(emp =>
                emp.Name.Contains(searchValue) ||
                    emp.GuardiansName.Contains(searchValue) ||
                    emp.EmpCode.Contains(searchValue) ||
                    emp.EmpFileNumber.Contains(searchValue) ||
                    emp.PersonalEmail.Contains(searchValue) ||
                    emp.OfficialEmail.Contains(searchValue) ||
                    emp.Phone.Contains(searchValue) ||
                    emp.ReportingEmployeeName.Contains(searchValue) ||
                    emp.EmpStatus.Contains(searchValue) ||
                    emp.LevelOneDescription.Contains(searchValue) ||
                    emp.LevelTwoDescription.Contains(searchValue) ||
                    emp.LevelThreeDescription.Contains(searchValue) ||
                    emp.LevelFourDescription.Contains(searchValue) ||
                    emp.LevelFiveDescription.Contains(searchValue) ||
                    emp.LevelSixDescription.Contains(searchValue) ||
                    emp.LevelSevenDescription.Contains(searchValue) ||
                    emp.LevelEightDescription.Contains(searchValue) ||
                    emp.LevelNineDescription.Contains(searchValue) ||
                    emp.LevelTenDescription.Contains(searchValue) ||
                    emp.LevelElevenDescription.Contains(searchValue) ||
                    emp.LevelTwelveDescription.Contains(searchValue));
            }

            var totalRecords = await finalQuery.CountAsync();

            var paginatedData = await finalQuery
                .OrderBy(x => x.EmpCode)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginatedResult<EmployeeResultDto>
            {
                draw = draw,
                recordsTotal = totalRecords,
                recordsFiltered = totalRecords,
                PageNumber = pageNumber,
                PageSize = pageSize,
                data = paginatedData
            };
        }
        //---------------Below code is a backup for above function. it also retrieve data from database but with not that much speed----------------

        #region GetEmployeeLinkLevelExistDataAsync Backup(16 April 2025)
        //private async Task<PaginatedResult<EmployeeResultDto>> GetEmployeeLinkLevelExistDataAsync(
        //   DateTime? durationFrom, DateTime? durationTo, string empSystemStatus, string currentStatusDesc,
        //   List<int> result, HashSet<int> excludedStatuses, int probationStatus, int pageNumber, int pageSize, int draw, string searchValue, bool isActive)
        //{
        //    var empList = await GetFilteredEmployeesAsync(result.ToHashSet());

        //    var empStatusSet = empList
        //        .Where(e => e.HasValue) // Filter null values
        //        .Select(e => e.Value) // Convert to non-nullable int
        //        .ToHashSet();

        //    IQueryable<EmployeeResultDto> finalQuery = null;


        //    if (!isActive)
        //    {
        //        finalQuery =
        //            from emp in (
        //                from emp in _context.HrEmpMasters
        //                join res in _context.Resignations
        //                       .Where(r => r.CurrentRequest == 1 &&
        //                           !new[] { ApprovalStatus.Deleted.ToString(), ApprovalStatus.Rejceted.ToString() }
        //                           .Contains(r.ApprovalStatus) &&
        //                           r.ApprovalStatus == (empSystemStatus == currentStatusDesc ?
        //                               ApprovalStatus.Pending.ToString() : ApprovalStatus.Approved.ToString()) &&
        //                           r.RejoinStatus == ApprovalStatus.Pending.ToString())
        //                on emp.EmpId equals res.EmpId into resGroup
        //                from res in resGroup.DefaultIfEmpty()
        //                where
        //                    (durationFrom == null || durationTo == null ||
        //                     emp.JoinDt >= durationFrom && emp.JoinDt <= durationTo ||
        //                     emp.ProbationDt >= durationFrom && emp.ProbationDt <= durationTo ||
        //                     emp.RelievingDate >= durationFrom && emp.RelievingDate <= durationTo)
        //                && (emp.CurrentStatus == Convert.ToInt32(empSystemStatus) ||
        //                    empSystemStatus.Equals(byte.MinValue.ToString()) ||
        //                    empSystemStatus == currentStatusDesc && res.ResignationId != null && res.RelievingDate >= DateTime.UtcNow)
        //                && emp.EmpStatus.HasValue
        //                && empList.Contains(emp.SeperationStatus)
        //                && (probationStatus == 2 && emp.IsProbation == true ||
        //                    probationStatus == 3 && emp.IsProbation == false ||
        //                    probationStatus == 1 && (emp.IsProbation == true || emp.IsProbation == false))
        //                && emp.IsDelete.Equals(false)
        //                select new
        //                {
        //                    EmpId = emp.EmpId,
        //                    EmpCode = emp.EmpCode,
        //                    Name = $"{emp.FirstName} {emp.MiddleName} {emp.LastName}",
        //                    GuardiansName = emp.GuardiansName,
        //                    DateOfBirth = FormatDate(emp.DateOfBirth, _employeeSettings.DateFormat),
        //                    JoinDate = FormatDate(emp.JoinDt, _employeeSettings.DateFormat),
        //                    DataDate = FormatDate(emp.JoinDt, _employeeSettings.DateFormat),
        //                    SeperationStatus = emp.SeperationStatus,
        //                    Gender = emp.Gender,
        //                    WorkingStatus = emp.SeperationStatus == (int)SeparationStatus.Live ? nameof(SeparationStatus.Live) : nameof(SeparationStatus.Resigned),
        //                    Age = CalculateAge(emp.DateOfBirth, "Years"),
        //                    ProbationDt = FormatDate(emp.ProbationDt, _employeeSettings.DateFormat),
        //                    Probation = emp.IsProbation == false ? ProbationStatus.CONFIRMED : ProbationStatus.PROBATION,
        //                    LastEntity = emp.LastEntity,
        //                    CurrentStatus = emp.CurrentStatus,
        //                    EmpStatus = emp.EmpStatus.ToString(),
        //                    IsSave = emp.IsSave,
        //                    EmpFileNumber = emp.EmpFileNumber,
        //                    DailyRateTypeId = emp.DailyRateTypeId,
        //                    PayrollMode = emp.PayrollMode,
        //                    ResignationReason = res.Reason,
        //                    ResignationDate = res.ResignationDate.ToString(),
        //                    RelievingDate = res.RelievingDate.ToString()
        //                }
        //            )
        //            join addr in _context.HrEmpAddresses on emp.EmpId equals addr.EmpId into addrGroup
        //            from addr in addrGroup.DefaultIfEmpty()
        //            join pers in _context.HrEmpPersonals on emp.EmpId equals pers.EmpId into persGroup
        //            from pers in persGroup.DefaultIfEmpty()
        //            join rep in _context.HrEmpReportings on emp.EmpId equals rep.EmpId into repGroup
        //            from rep in repGroup.DefaultIfEmpty()
        //            join highView in _context.HighLevelViewTables on emp.LastEntity equals highView.LastEntityId into highViewGroup
        //            from highView in highViewGroup.DefaultIfEmpty()
        //            join img in _context.HrEmpImages on emp.EmpId equals img.EmpId into imgGroup
        //            from img in imgGroup.DefaultIfEmpty()
        //            join currStatus in _context.EmployeeCurrentStatuses on emp.CurrentStatus equals currStatus.Status into currStatusGroup
        //            from currStatus in currStatusGroup.DefaultIfEmpty()
        //            join empStatusSettings in _context.HrEmpStatusSettings on Convert.ToInt32(emp.EmpStatus) equals empStatusSettings.StatusId into empStatusGroup
        //            from empStatusSettings in empStatusGroup.DefaultIfEmpty()
        //            join reason in _context.ReasonMasters on Convert.ToInt32(emp.ResignationReason) equals reason.ReasonId into reasonGroup
        //            from reason in reasonGroup.DefaultIfEmpty()
        //            join country in _context.AdmCountryMasters on pers.Nationality equals country.CountryId into countryGroup
        //            from country in countryGroup.DefaultIfEmpty()
        //            join empDetails in _context.EmployeeDetails on rep.ReprotToWhome equals empDetails.EmpId into empDetailsGroup
        //            from empDetails in empDetailsGroup.DefaultIfEmpty()
        //            select new EmployeeResultDto
        //            {
        //                EmpId = emp.EmpId,
        //                ImageUrl = img.ImageUrl,
        //                EmpCode = emp.EmpCode,
        //                Name = emp.Name,
        //                GuardiansName = emp.GuardiansName,
        //                JoinDate = emp.JoinDate,
        //                DataDate = emp.DataDate,
        //                EmpStatusDesc = currStatus.StatusDesc,
        //                EmpStatus = empStatusSettings.StatusDesc,
        //                Gender = GetGender(emp.Gender).ToString(),
        //                SeperationStatus = emp.SeperationStatus,
        //                OfficialEmail = addr.OfficialEmail,
        //                PersonalEmail = addr.PersonalEmail,
        //                Phone = addr.Phone,
        //                MaritalStatus = GetMaritalStatus(pers.MaritalStatus).ToString(),
        //                Age = emp.Age,
        //                ProbationDt = emp.ProbationDt,
        //                LevelOneDescription = highView.LevelOneDescription,
        //                LevelTwoDescription = highView.LevelTwoDescription,
        //                ProbationStatus = emp.Probation.ToString(),
        //                Nationality = country.Nationality,
        //                IsSave = emp.IsSave,
        //                EmpFileNumber = emp.EmpFileNumber,
        //                CurrentStatus = emp.CurrentStatus,
        //                LevelThreeDescription = highView.LevelThreeDescription,
        //                LevelFourDescription = highView.LevelFourDescription,
        //                LevelFiveDescription = highView.LevelFiveDescription,
        //                LevelSixDescription = highView.LevelSixDescription,
        //                LevelSevenDescription = highView.LevelSevenDescription,
        //                LevelEightDescription = highView.LevelEightDescription,
        //                LevelNineDescription = highView.LevelNineDescription,
        //                LevelTenDescription = highView.LevelTenDescription,
        //                LevelElevenDescription = highView.LevelElevenDescription,
        //                LevelTwelveDescription = highView.LevelTwelveDescription,
        //                ReportingEmployeeCode = empDetails.EmpCode,
        //                ReportingEmployeeName = empDetails.Name,
        //                WorkingStatus = emp.WorkingStatus,
        //                RelievingDate = emp.RelievingDate

        //            };
        //    }
        //    else
        //    {
        //        finalQuery =
        //         from emp in (
        //             from emp in _context.HrEmpMasters
        //             join res in _context.Resignations
        //                    .Where(r => r.CurrentRequest == 1 &&
        //                        !new[] { ApprovalStatus.Deleted.ToString(), ApprovalStatus.Rejceted.ToString() }
        //                        .Contains(r.ApprovalStatus) &&
        //                        r.ApprovalStatus == (empSystemStatus == currentStatusDesc ?
        //                            ApprovalStatus.Pending.ToString() : ApprovalStatus.Approved.ToString()) &&
        //                        r.RejoinStatus == ApprovalStatus.Pending.ToString())
        //             on emp.EmpId equals res.EmpId into resGroup
        //             from res in resGroup.DefaultIfEmpty()
        //             where
        //                 (durationFrom == null || durationTo == null ||
        //                  emp.JoinDt >= durationFrom && emp.JoinDt <= durationTo ||
        //                  emp.ProbationDt >= durationFrom && emp.ProbationDt <= durationTo ||
        //                  emp.RelievingDate >= durationFrom && emp.RelievingDate <= durationTo)
        //             && (emp.CurrentStatus == Convert.ToInt32(empSystemStatus) ||
        //                 empSystemStatus.Equals(byte.MinValue.ToString()) ||
        //                 empSystemStatus == currentStatusDesc && res.ResignationId != null && res.RelievingDate >= DateTime.UtcNow)
        //             && emp.EmpStatus.HasValue // Ensure it's not null
        //                                       // && empStatusSet.Contains(emp.EmpStatus.Value) // Use Value instead of GetValueOrDefault()
        //             && !excludedStatuses.Contains(emp.SeperationStatus.Value)
        //             && (probationStatus == 2 && emp.IsProbation == true ||
        //                 probationStatus == 3 && emp.IsProbation == false ||
        //                 probationStatus == 1 && (emp.IsProbation == true || emp.IsProbation == false))
        //             && emp.IsDelete.Equals(false)
        //             select new
        //             {
        //                 EmpId = emp.EmpId,
        //                 EmpCode = emp.EmpCode,
        //                 Name = $"{emp.FirstName} {emp.MiddleName} {emp.LastName}",
        //                 GuardiansName = emp.GuardiansName,
        //                 DateOfBirth = FormatDate(emp.DateOfBirth, _employeeSettings.DateFormat),
        //                 JoinDate = FormatDate(emp.JoinDt, _employeeSettings.DateFormat),
        //                 DataDate = FormatDate(emp.JoinDt, _employeeSettings.DateFormat),
        //                 SeperationStatus = emp.SeperationStatus,
        //                 Gender = emp.Gender,
        //                 WorkingStatus = emp.SeperationStatus == (int)SeparationStatus.Live ? nameof(SeparationStatus.Live) : nameof(SeparationStatus.Resigned),
        //                 Age = CalculateAge(emp.DateOfBirth, "Years"),
        //                 ProbationDt = FormatDate(emp.ProbationDt, _employeeSettings.DateFormat),
        //                 Probation = emp.IsProbation == false ? ProbationStatus.CONFIRMED : ProbationStatus.PROBATION,
        //                 LastEntity = emp.LastEntity,
        //                 CurrentStatus = emp.CurrentStatus,
        //                 EmpStatus = emp.EmpStatus.ToString(),
        //                 IsSave = emp.IsSave,
        //                 EmpFileNumber = emp.EmpFileNumber,
        //                 DailyRateTypeId = emp.DailyRateTypeId,
        //                 PayrollMode = emp.PayrollMode,
        //                 ResignationReason = res.Reason,
        //                 ResignationDate = res.ResignationDate.ToString(),
        //                 RelievingDate = res.RelievingDate.ToString()
        //             }
        //         )
        //         join addr in _context.HrEmpAddresses on emp.EmpId equals addr.EmpId into addrGroup
        //         from addr in addrGroup.DefaultIfEmpty()
        //         join pers in _context.HrEmpPersonals on emp.EmpId equals pers.EmpId into persGroup
        //         from pers in persGroup.DefaultIfEmpty()
        //         join rep in _context.HrEmpReportings on emp.EmpId equals rep.EmpId into repGroup
        //         from rep in repGroup.DefaultIfEmpty()
        //         join highView in _context.HighLevelViewTables on emp.LastEntity equals highView.LastEntityId into highViewGroup
        //         from highView in highViewGroup.DefaultIfEmpty()
        //         join img in _context.HrEmpImages on emp.EmpId equals img.EmpId into imgGroup
        //         from img in imgGroup.DefaultIfEmpty()
        //         join currStatus in _context.EmployeeCurrentStatuses on emp.CurrentStatus equals currStatus.Status into currStatusGroup
        //         from currStatus in currStatusGroup.DefaultIfEmpty()
        //         join empStatusSettings in _context.HrEmpStatusSettings on Convert.ToInt32(emp.EmpStatus) equals empStatusSettings.StatusId into empStatusGroup
        //         from empStatusSettings in empStatusGroup.DefaultIfEmpty()
        //         join reason in _context.ReasonMasters on Convert.ToInt32(emp.ResignationReason) equals reason.ReasonId into reasonGroup
        //         from reason in reasonGroup.DefaultIfEmpty()
        //         join country in _context.AdmCountryMasters on pers.Nationality equals country.CountryId into countryGroup
        //         from country in countryGroup.DefaultIfEmpty()
        //         join empDetails in _context.EmployeeDetails on rep.ReprotToWhome equals empDetails.EmpId into empDetailsGroup
        //         from empDetails in empDetailsGroup.DefaultIfEmpty()
        //         select new EmployeeResultDto
        //         {
        //             EmpId = emp.EmpId,
        //             ImageUrl = img.ImageUrl,
        //             EmpCode = emp.EmpCode,
        //             Name = emp.Name,
        //             GuardiansName = emp.GuardiansName,
        //             JoinDate = emp.JoinDate,
        //             DataDate = emp.DataDate,
        //             EmpStatusDesc = currStatus.StatusDesc,
        //             EmpStatus = empStatusSettings.StatusDesc,
        //             Gender = GetGender(emp.Gender).ToString(),
        //             SeperationStatus = emp.SeperationStatus,
        //             OfficialEmail = addr.OfficialEmail,
        //             PersonalEmail = addr.PersonalEmail,
        //             Phone = addr.Phone,
        //             MaritalStatus = GetMaritalStatus(pers.MaritalStatus).ToString(),
        //             Age = emp.Age,
        //             ProbationDt = emp.ProbationDt,
        //             LevelOneDescription = highView.LevelOneDescription,
        //             LevelTwoDescription = highView.LevelTwoDescription,
        //             ProbationStatus = emp.Probation.ToString(),
        //             Nationality = country.Nationality,
        //             IsSave = emp.IsSave,
        //             EmpFileNumber = emp.EmpFileNumber,
        //             CurrentStatus = emp.CurrentStatus,
        //             LevelThreeDescription = highView.LevelThreeDescription,
        //             LevelFourDescription = highView.LevelFourDescription,
        //             LevelFiveDescription = highView.LevelFiveDescription,
        //             LevelSixDescription = highView.LevelSixDescription,
        //             LevelSevenDescription = highView.LevelSevenDescription,
        //             LevelEightDescription = highView.LevelEightDescription,
        //             LevelNineDescription = highView.LevelNineDescription,
        //             LevelTenDescription = highView.LevelTenDescription,
        //             LevelElevenDescription = highView.LevelElevenDescription,
        //             LevelTwelveDescription = highView.LevelTwelveDescription,
        //             ReportingEmployeeCode = empDetails.EmpCode,
        //             ReportingEmployeeName = empDetails.Name,
        //             WorkingStatus = emp.WorkingStatus,
        //             RelievingDate = emp.RelievingDate

        //         };

        //    }

        //    var filteredQuery = finalQuery
        //       .Where(emp =>
        //           string.IsNullOrEmpty(searchValue) || emp.GuardiansName.Contains(searchValue) || emp.EmpCode.Contains(searchValue) || emp.EmpFileNumber.Contains(searchValue) || emp.PersonalEmail.Contains(searchValue) || emp.OfficialEmail.Contains(searchValue) || emp.Phone.Contains(searchValue) || emp.ReportingEmployeeName.Contains(searchValue) || emp.EmpStatus.Contains(searchValue) || emp.LevelOneDescription.Contains(searchValue) || emp.LevelTwoDescription.Contains(searchValue) || emp.LevelThreeDescription.Contains(searchValue) || emp.LevelFourDescription.Contains(searchValue) || emp.LevelFiveDescription.Contains(searchValue) || emp.LevelSixDescription.Contains(searchValue) || emp.LevelSevenDescription.Contains(searchValue) || emp.LevelEightDescription.Contains(searchValue) || emp.LevelNineDescription.Contains(searchValue) || emp.LevelTenDescription.Contains(searchValue) || emp.LevelElevenDescription.Contains(searchValue) || emp.LevelTwelveDescription.Contains(searchValue)
        //       );

        //    var totalRecords = await filteredQuery.CountAsync();


        //    var paginatedResult = await filteredQuery
        //                        .OrderBy(x => x.EmpCode)
        //                        .Skip((pageNumber - 1) * pageSize)
        //                        .Take(pageSize)
        //                        .ToListAsync();


        //    return new PaginatedResult<EmployeeResultDto>
        //    {
        //        draw = draw,
        //        recordsTotal = totalRecords,
        //        recordsFiltered = totalRecords,
        //        PageNumber = pageNumber,
        //        PageSize = pageSize,
        //        data = paginatedResult
        //    };



        //}

        #endregion


        private async Task<PaginatedResult<EmployeeResultDto>> InfoFormatOneOrZeroLinkLevelExist(string? empStatus, string? empSystemStatus, string? empIds, string? filterType, DateTime? durationFrom, DateTime? durationTo, int probationStatus, string? currentStatusDesc, string? ageFormat, int pageNumber, int pageSize, int draw, string searchValue, bool isActive)
        {

            var statusSet = ParseStatusList(empStatus);//4,5,8,9 is showing here

            var filteredStatuses = await GetStatusSetAsync(false);//1,2,3,7,8,9,10
            var excludedStatuses = await GetStatusSetAsync(true, statusSet);


            List<int> result = new List<int>();

            if (isActive)
            {
                result = statusSet.Where(filteredStatuses.Contains).ToList();
            }
            else
            {
                result = statusSet.Where(s => !filteredStatuses.Contains(s)).ToList();
            }




            return await GetEmployeeLinkLevelExistDataAsync(durationFrom, durationTo, empSystemStatus,
        currentStatusDesc, result, excludedStatuses, probationStatus, pageNumber, pageSize, draw, searchValue, isActive);

        }
        private async Task<HashSet<int>> GetStatusSetAsync(bool isResignation, HashSet<int>? excludedSet = null)
        {
            return (await _context.HrEmpStatusSettings
                                  .Where(s => s.IsResignation == isResignation &&
                                              (excludedSet == null || !excludedSet.Contains(s.StatusId)))
                                  .Select(s => s.StatusId)
                                  .ToListAsync())
                                  .ToHashSet();
        }

        private HashSet<int> ParseStatusList(string? empStatus)
        {
            return string.IsNullOrWhiteSpace(empStatus)
                ? new HashSet<int>()
                : empStatus.Split(',')
                           .Select(s => int.Parse(s.Trim()))
                           .ToHashSet();
        }
        private async Task<PaginatedResult<EmployeeResultDto>> InfoFormatTwoLinkLevelExists(string? empStatus, string? empSystemStatus, string? empIds, string? filterType, DateTime? durationFrom,
        DateTime? durationTo, int probationStatus, string? currentStatusDesc, string? ageFormat, int pageNumber, int pageSize, int draw, string searchValue, bool isActive)
        {

            var statusSet = ParseStatusList(empStatus);
            var filteredStatuses = await GetStatusSetAsync(false);
            var excludedStatuses = await GetStatusSetAsync(true, statusSet);
            // Filter results
            var result = statusSet.Where(filteredStatuses.Contains).ToList();


            return await GetEmployeeLinkLevelExistDataAsync(durationFrom, durationTo, empSystemStatus,
        currentStatusDesc, result, excludedStatuses, probationStatus, pageNumber, pageSize, draw, searchValue, isActive);

        }
        private async Task<PaginatedResult<EmployeeResultDto>> InfoFormatThreeLinkLevelExists(string? empStatus, string? empSystemStatus, string? empIds, string? filterType, DateTime? durationFrom,
        DateTime? durationTo, int probationStatus, string? currentStatusDesc, string? ageFormat, int pageNumber, int pageSize, int draw, string searchValue, bool isActive)
        {

            var statusSet = ParseStatusList(empStatus);
            var filteredStatuses = await GetStatusSetAsync(false);
            var excludedStatuses = await GetStatusSetAsync(true, statusSet);

            // Filter results
            var result = statusSet.Where(filteredStatuses.Contains).ToList();


            return await GetEmployeeLinkLevelExistDataAsync(durationFrom, durationTo, empSystemStatus,
        currentStatusDesc, result, excludedStatuses, probationStatus, pageNumber, pageSize, draw, searchValue, isActive);


        }
        private async Task<PaginatedResult<EmployeeResultDto>> InfoFormatFourLinkLevelExists(string? empStatus, string? empSystemStatus, string? empIds, string? filterType, DateTime? durationFrom,
        DateTime? durationTo, int probationStatus, string? currentStatusDesc, string? ageFormat, int pageNumber, int pageSize, int draw)
        {

            var result = await (
              from emp in _context.EmployeeDetails
              join docApp in _context.HrmsEmpdocumentsApproved00s on emp.EmpId equals docApp.EmpId
              join docDetail in _context.HrmsEmpdocumentsApproved01s on docApp.DetailId equals docDetail.DetailId
              join docField in _context.HrmsDocumentField00s on docDetail.DocFields equals Convert.ToInt32(docField.DocFieldId) into docFieldGroup
              from docField in docFieldGroup.DefaultIfEmpty()
              join doc in _context.HrmsDocument00s on docField.DocId equals Convert.ToInt32(doc.DocId)
              where (doc.DocType == 5 || doc.DocType == 3) && docApp.Status != ApprovalStatus.Deleted.ToString()
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



            var statusSet = ParseStatusList(empStatus);
            var filteredStatuses = await GetStatusSetAsync(false);
            var excludedStatuses = await GetStatusSetAsync(true, statusSet);

            var result12 = statusSet
                                .Where(item => filteredStatuses.Contains(item))
                                .ToList();


            var cteEmployeeDetails = await (
                from emp in _context.HrEmpMasters
                join res in _context.Resignations
                on emp.EmpId equals res.EmpId into resGroup
                from res in resGroup.DefaultIfEmpty()
                where
                (durationFrom == null || durationTo == null || emp.JoinDt >= durationFrom && emp.JoinDt <= durationTo || durationFrom == null || durationTo == null || emp.ProbationDt >= durationFrom && emp.ProbationDt <= durationTo || durationFrom == null || durationTo == null || emp.RelievingDate >= durationFrom && emp.RelievingDate <= durationTo)
                                        && (emp.CurrentStatus == Convert.ToInt32(empSystemStatus) || empSystemStatus == _employeeSettings.empSystemStatus
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
                    Name = (emp.FirstName ?? "") + " " + (emp.MiddleName ?? "") + " " + (emp.LastName ?? ""),
                    emp.GuardiansName,
                    //DateOfBirth = emp.DateOfBirth.ToString(_employeeSettings.DateFormat),
                    //JoinDate = emp.Join_Dt.ToString(_employeeSettings.DateFormat),
                    //DataDate = emp.Join_Dt.ToString("yyyyMMdd"),
                    emp.DateOfBirth,
                    JoinDate = emp.JoinDt,
                    DataDate = emp.JoinDt,
                    emp.SeperationStatus,
                    //Gender = emp.Gender == "M" ? "Male" : emp.Gender == "F" ? "Female" : emp.Gender == "O" ? "Other" : _employeeSettings.NotAvailable,
                    Gender = GetGender(emp.Gender),
                    //WorkingStatus = emp.SeperationStatus == 0 ? "Live" : "Resigned",
                    WorkingStatus = emp.SeperationStatus == (int)SeparationStatus.Live ? nameof(SeparationStatus.Live) : nameof(SeparationStatus.Resigned),
                    Age = CalculateAge(emp.DateOfBirth, ageFormat),
                    //Probation_Dt = emp.Probation_Dt.ToString(_employeeSettings.DateFormat),
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
                        emp.GuardiansName,
                        emp.DateOfBirth,
                        emp.SeperationStatus,

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
                            EmpCode = repDet?.EmpCode ?? string.Empty,  // Handle null by assigning empty string
                            Name = repDet?.Name ?? string.Empty,        // Handle null safely
                            LevelOneDescription = highView?.LevelOneDescription ?? string.Empty,
                            LevelTwoDescription = highView?.LevelTwoDescription ?? string.Empty,
                            LevelThreeDescription = highView?.LevelThreeDescription ?? string.Empty,
                            LevelFourDescription = highView?.LevelFourDescription ?? string.Empty,
                            LevelFiveDescription = highView?.LevelFiveDescription ?? string.Empty,
                            LevelSixDescription = highView?.LevelSixDescription ?? string.Empty,
                            LevelSevenDescription = highView?.LevelSevenDescription ?? string.Empty,
                            LevelEightDescription = highView?.LevelEightDescription ?? string.Empty,
                            LevelNineDescription = highView?.LevelNineDescription ?? string.Empty,
                            LevelTenDescription = highView?.LevelTenDescription ?? string.Empty,
                            LevelElevenDescription = highView?.LevelElevenDescription ?? string.Empty,
                            LevelTwelveDescription = highView?.LevelTwelveDescription ?? string.Empty,
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
                                 LevelTwoDescription = reportingDetails.LevelTwoDescription,
                                 LevelThreeDescription = reportingDetails.LevelThreeDescription,
                                 LevelFourDescription = reportingDetails.LevelFourDescription,
                                 LevelFiveDescription = reportingDetails.LevelFiveDescription,
                                 LevelSixDescription = reportingDetails.LevelSixDescription,
                                 LevelSevenDescription = reportingDetails.LevelSevenDescription,
                                 LevelEightDescription = reportingDetails.LevelEightDescription,
                                 LevelNineDescription = reportingDetails.LevelNineDescription,
                                 LevelTenDescription = reportingDetails.LevelTenDescription,
                                 LevelElevenDescription = reportingDetails.LevelElevenDescription,
                                 LevelTwelveDescription = reportingDetails.LevelTwelveDescription,

                                 DateOfBirth = baseDetails.DateOfBirth.ToString(),
                                 JoinDate = baseDetails.JoinDate.ToString(),
                                 //DataDate = baseDetails.DataDate,
                                 //EmpStatusDesc = currStatus.StatusDesc,
                                 //EmpStatus = empStatusSettings.StatusDesc,
                                 //Gen = baseDetails.Gender.ToString(),

                                 //ResignationDate = emp.ResignationDate,

                                 SeperationStatus = baseDetails.SeperationStatus,


                                 //MaritalStatus = pers.MaritalStatus,
                                 //Age = emp.Age,
                                 //ProbationDt = emp.Probation_Dt,

                                 //Description = baseDetails.Description,
                                 //ResignationType = emp.ResignationType,
                                 //ProbationStatus = emp.Probation,
                                 //Nationality = country.Nationality,
                                 //IsSave = emp.IsSave,
                                 //EmpFileNumber = emp.EmpFileNumber,
                                 //ReligionName = religion.ReligionName,
                                 //BloodGroup = pers.BloodGrp,
                                 //ReportingEmployeeCode = repDet.EmpCode,
                                 //ReportingEmployeeName = repDet.Name,
                                 //WorkingStatus = emp.WorkingStatus,
                                 //RelievingDate = emp.RelievingDate,
                                 //SalaryType = emp.PayrollMode == 1 || emp.PayrollMode == 0 ? "Monthly Wage" : "Daily Wage",
                                 //GrossPay = pay != null ? pay.TotalPay : 0,
                                 //CurrentStatus = emp.CurrentStatus,
                                 // Add other fields as needed
                             };




            #endregion




            //var totalRecords = resultFour.Count();
            //var maxPages = (int)Math.Ceiling((double)totalRecords / pageSize);
            //if (pageNumber > maxPages) pageNumber = maxPages;
            //if (pageNumber < 1) pageNumber = 1;

            //var paginatedResult = resultFour
            //    .OrderBy(x => x.EmpCode)
            //    .Skip((pageNumber - 1) * pageSize)
            //    .Take(pageSize)
            //    .ToList();


            var totalRecords = resultFour.Count();
            var paginatedResult = resultFour
                .OrderBy(x => x.EmpCode) // Sorting logic
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();



            return new PaginatedResult<EmployeeResultDto>
            {
                draw = draw,
                recordsTotal = totalRecords,
                recordsFiltered = totalRecords,
                PageNumber = pageNumber,
                PageSize = pageSize,
                data = paginatedResult
            };




        }
        private async Task<PaginatedResult<EmployeeResultDto>> GetPaginatedNotExistLinkSelectEmployeeDataAsync(
        DateTime? durationFrom, DateTime? durationTo, string empStatus, List<string>? empIdList,
        int pageNumber, int pageSize, string? ageFormat, int draw, string searchValue)
        {


            var statusSet = ParseStatusList(empStatus);
            var filteredStatuses = await GetStatusSetAsync(false);



            var result = statusSet.Where(item => filteredStatuses.Contains(item)).ToList();
            HashSet<int> empIdSet = empIdList.Select(int.Parse).ToHashSet();

            var query = await (from a in _context.HrEmpMasters
                               where empIdSet.Contains(a.EmpId) && a.IsDelete == false &&
                                     (durationFrom == null || durationTo == null ||
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
                               join t in _context.Resignations.Where(x => x.CurrentRequest == 1 &&
                                           !new[] { ApprovalStatus.Deleted.ToString(), ApprovalStatus.Rejceted.ToString() }.Contains(x.ApprovalStatus))
                                   on a.EmpId equals t.EmpId into tempResignations
                               from t in tempResignations.DefaultIfEmpty()
                               join reason in _context.ReasonMasters.Where(r => r.Type == _employeeSettings.ReasonType)
                                   on Convert.ToInt32(t.Reason) equals reason.ReasonId into reasonGroup
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
                                   Name = (a.FirstName ?? "") + " " + (a.MiddleName ?? "") + " " + (a.LastName ?? ""),
                                   GuardiansName = a.GuardiansName,
                                   DateOfBirth = FormatDate(a.DateOfBirth, _employeeSettings.DateFormat),
                                   JoinDate = a.JoinDt.ToString(),
                                   WeddingDate = c.WeddingDate,
                                   DataDate = a.JoinDt.ToString(),
                                   StatusDesc = j.StatusDesc,
                                   EmpStatusDesc = s.StatusDesc,
                                   Gender = GetGender(a.Gender).ToString(),
                                   ResignationDate = FormatDate(t.ResignationDate, _employeeSettings.DateFormat),
                                   RelievingDate = FormatDate(a.RelievingDate, _employeeSettings.DateFormat),
                                   SeperationStatus = a.SeperationStatus,
                                   OfficialEmail = b.OfficialEmail ?? _employeeSettings.NotAvailable,
                                   PersonalEmail = b.PersonalEmail,
                                   Phone = b.Phone,
                                   MaritalStatus = GetMaritalStatus(c.MaritalStatus).ToString(),
                                   Age = CalculateAge(a.DateOfBirth, ageFormat),
                                   ProbationDt = a.ProbationDt.ToString(),
                                   LevelOneDescription = f.LevelOneDescription,
                                   LevelTwoDescription = f.LevelTwoDescription,
                                   LevelThreeDescription = f.LevelThreeDescription,
                                   ResignationReason = rm.Description,
                                   ResignationType = t.ResignationType,
                                   ProbationStatus = a.IsProbation.ToString(),
                                   Nationality = cm.CountryName,
                                   IsSave = a.IsSave,
                                   EmpFileNumber = a.EmpFileNumber,
                                   CurrentStatus = a.CurrentStatus
                               }).ToListAsync();




            var filteredQuery = query
                                .Where(emp =>
                                string.IsNullOrEmpty(searchValue) || emp.Name.Contains(searchValue) || emp.GuardiansName.Contains(searchValue) || emp.EmpCode.Contains(searchValue) || emp.EmpFileNumber.Contains(searchValue) || emp.PersonalEmail.Contains(searchValue) || emp.OfficialEmail.Contains(searchValue) || emp.Phone.Contains(searchValue) || emp.ReportingEmployeeName.Contains(searchValue) || emp.EmpStatus.Contains(searchValue) || emp.LevelOneDescription.Contains(searchValue) || emp.LevelTwoDescription.Contains(searchValue) || emp.LevelThreeDescription.Contains(searchValue) || emp.LevelFourDescription.Contains(searchValue) || emp.LevelFiveDescription.Contains(searchValue) || emp.LevelSixDescription.Contains(searchValue) || emp.LevelSevenDescription.Contains(searchValue) || emp.LevelEightDescription.Contains(searchValue) || emp.LevelNineDescription.Contains(searchValue) || emp.LevelTenDescription.Contains(searchValue) || emp.LevelElevenDescription.Contains(searchValue) || emp.LevelTwelveDescription.Contains(searchValue)
                                );
            var totalRecords = filteredQuery.Count();
            var paginatedResult = filteredQuery
                                    .OrderBy(x => x.EmpCode)
                                    .Skip((pageNumber - 1) * pageSize)
                                    .Take(pageSize)
                                    .ToList();
            return new PaginatedResult<EmployeeResultDto>
            {
                draw = draw,
                recordsTotal = totalRecords,
                recordsFiltered = totalRecords,
                PageNumber = pageNumber,
                PageSize = pageSize,
                data = paginatedResult
            };




            //var totalRecords = query.Count();
            //var maxPages = (int)Math.Ceiling((double)totalRecords / pageSize);
            //if (pageNumber > maxPages) pageNumber = maxPages;
            //if (pageNumber < 1) pageNumber = 1;

            //var paginatedResult = query
            //    .OrderBy(x => x.EmpCode)
            //    .Skip((pageNumber - 1) * pageSize)
            //    .Take(pageSize)
            //    .ToList();


            //var totalRecords = query.Count();
            //var paginatedResult = query
            //.OrderBy(x => x.EmpCode) // Sorting logic
            //.Skip((pageNumber - 1) * pageSize)
            //.Take(pageSize)
            //.ToList();
            //return new PaginatedResult<EmployeeResultDto>
            //{
            //    draw = draw,
            //    recordsTotal = totalRecords,
            //    recordsFiltered = totalRecords,
            //    PageNumber = pageNumber,
            //    PageSize = pageSize,
            //    data = paginatedResult
            //};
        }
        private async Task<PaginatedResult<EmployeeResultDto>> InforFormatOneOrZeroNotExistLinkSelect(DateTime? durationFrom, DateTime? durationTo, string EmpStatus, List<string>? empIdList, int pageNumber, int pageSize, string? ageFormat, int draw, string searchValue)
        {
            return await GetPaginatedNotExistLinkSelectEmployeeDataAsync(durationFrom, durationTo, EmpStatus, empIdList, pageNumber, pageSize, ageFormat, draw, searchValue);
        }
        private async Task<PaginatedResult<EmployeeResultDto>> InforFormatTwoNotExistLinkSelect(DateTime? durationFrom, DateTime? durationTo, string? EmpStatus, List<string>? empIdList, int pageNumber, int pageSize, string? ageFormat, int draw, string searchValue)
        {
            return await GetPaginatedNotExistLinkSelectEmployeeDataAsync(durationFrom, durationTo, EmpStatus, empIdList, pageNumber, pageSize, ageFormat, draw, searchValue);
        }
        private async Task<PaginatedResult<EmployeeResultDto>> InforFormatThreeNotExistLinkSelect(DateTime? durationFrom, DateTime? durationTo, string? EmpStatus, List<string>? empIdList, int pageNumber, int pageSize, string? ageFormat, int draw, string searchValue)
        {
            return await GetPaginatedNotExistLinkSelectEmployeeDataAsync(durationFrom, durationTo, EmpStatus, empIdList, pageNumber, pageSize, ageFormat, draw, searchValue);
        }
        private async Task<PaginatedResult<EmployeeResultDto>> InforFormatFourNotExistLinkSelect(DateTime? durationFrom, DateTime? durationTo, string? EmpStatus, List<string>? empIdList, int pageNumber, int pageSize, string? ageFormat, int draw, string searchValue)
        {
            return await GetPaginatedNotExistLinkSelectEmployeeDataAsync(durationFrom, durationTo, EmpStatus, empIdList, pageNumber, pageSize, ageFormat, draw, searchValue);
        }
        private async Task<List<EmployeeResultDto>> GetEmployeeResultEmpIdZeroEmployeeExistsAsync(string systemStatus, string currentStatusDesc, string ageFormat)
        {
            return await (
            from a in _context.HrEmpMasters
            join b in _context.HrEmpAddresses on a.EmpId equals b.EmpId into tempAddress
            from b in tempAddress.DefaultIfEmpty()
            join c in _context.HrEmpPersonals on a.EmpId equals c.EmpId into tempPersonal
            from c in tempPersonal.DefaultIfEmpty()
            join f in _context.HighLevelViewTables on a.LastEntity equals f.LastEntityId into tempHighLevel
            from f in tempHighLevel.DefaultIfEmpty()
            join i in _context.HrEmpImages on a.EmpId equals i.EmpId into tempImages
            from i in tempImages.DefaultIfEmpty()
            join t in _context.Resignations
            .Where(x => x.CurrentRequest == 1 && !new[] { ApprovalStatus.Deleted.ToString(), ApprovalStatus.Rejceted.ToString() }.Contains(x.ApprovalStatus))
            on a.EmpId equals t.EmpId into tempResignations
            from t in tempResignations.DefaultIfEmpty()
            join j in _context.EmployeeCurrentStatuses on a.CurrentStatus equals j.Status
            join s in _context.HrEmpStatusSettings on a.EmpStatus equals s.StatusId
            join reason in _context.ReasonMasters
            .Where(r => r.Type == _employeeSettings.ReasonType)
            on Convert.ToInt32(t.Reason) equals reason.ReasonId into reasonGroup
            from rm in reasonGroup.DefaultIfEmpty()
            join cm in _context.AdmCountryMasters on c.Nationality equals cm.CountryId into tempCountry
            from cm in tempCountry.DefaultIfEmpty()
            where a.CurrentStatus == Convert.ToInt32(systemStatus)
            || systemStatus == _employeeSettings.empSystemStatus
            || systemStatus == currentStatusDesc && t.RelievingDate >= DateTime.UtcNow
            orderby a.EmpCode
            select new EmployeeResultDto
            {
                EmpId = a.EmpId,
                ImageUrl = i.ImageUrl,
                EmpCode = a.EmpCode,
                //Name = $"{a.FirstName} {a.MiddleName} {a.LastName}",
                Name = (a.FirstName ?? "") + " " + (a.MiddleName ?? "") + " " + (a.LastName ?? ""),
                GuardiansName = a.GuardiansName,
                DateOfBirth = FormatDate(a.DateOfBirth, _employeeSettings.DateFormat),
                JoinDate = a.JoinDt.ToString(),
                WeddingDate = c.WeddingDate,
                DataDate = a.JoinDt.ToString(),
                StatusDesc = j.StatusDesc,
                EmpStatusDesc = s.StatusDesc,
                Gender = GetGender(a.Gender).ToString(),
                ResignationDate = FormatDate(t.ResignationDate, _employeeSettings.DateFormat),
                RelievingDate = FormatDate(a.RelievingDate, _employeeSettings.DateFormat),
                SeperationStatus = a.SeperationStatus,
                OfficialEmail = b.OfficialEmail ?? _employeeSettings.NotAvailable,
                PersonalEmail = b.PersonalEmail,
                Phone = b.Phone,
                MaritalStatus = GetMaritalStatus(c.MaritalStatus).ToString(),
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
                ResignationType = t.ResignationType,
                ProbationStatus = a.IsProbation.ToString(),
                Nationality = cm.CountryName,
                IsSave = a.IsSave,
                EmpFileNumber = a.EmpFileNumber,
                CurrentStatus = a.CurrentStatus,
            }).ToListAsync();
        }
        private async Task<PaginatedResult<EmployeeResultDto>> GetPaginatedEmployeeResultAsync(int pageNumber, int pageSize, string? ageFormat, string? systemStatus, string? currentStatusDesc)
        {
            var employeeResults = await GetEmployeeResultEmpIdZeroEmployeeExistsAsync(systemStatus, currentStatusDesc, ageFormat);

            //var totalRecords = employeeResults.Count();
            //var maxPages = (int)Math.Ceiling((double)totalRecords / pageSize);
            //if (pageNumber > maxPages) pageNumber = maxPages;
            //if (pageNumber < 1) pageNumber = 1;

            //var paginatedResult = employeeResults
            //    .OrderBy(x => x.EmpCode)
            //    .Skip((pageNumber - 1) * pageSize)
            //    .Take(pageSize)
            //    .ToList();

            var totalRecords = employeeResults.Count();
            var paginatedResult = employeeResults
            .OrderBy(x => x.EmpCode) // Sorting logic
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();
            return new PaginatedResult<EmployeeResultDto>
            {
                recordsTotal = totalRecords,
                recordsFiltered = totalRecords,
                PageNumber = pageNumber,
                PageSize = pageSize,
                data = paginatedResult
            };
        }
        private async Task<PaginatedResult<EmployeeResultDto>> InfoFormatZeroOrOneEmpIdZeroEmployeeExists(int pageNumber, int pageSize, string? ageFormat, string? systemStatus, string? currentStatusDesc)
        {
            return await GetPaginatedEmployeeResultAsync(pageNumber, pageSize, ageFormat, systemStatus, currentStatusDesc);
        }
        private async Task<PaginatedResult<EmployeeResultDto>> InfoFormatTwoEmpIdZeroEmployeeExists(int pageNumber, int pageSize, string? ageFormat, string? systemStatus, string? currentStatusDesc)
        {
            return await GetPaginatedEmployeeResultAsync(pageNumber, pageSize, ageFormat, systemStatus, currentStatusDesc);
        }
        private async Task<PaginatedResult<EmployeeResultDto>> InfoFormatThreeEmpIdZeroEmployeeExists(int pageNumber, int pageSize, string? ageFormat, string? systemStatus, string? currentStatusDesc)
        {
            return await GetPaginatedEmployeeResultAsync(pageNumber, pageSize, ageFormat, systemStatus, currentStatusDesc);
        }
        private async Task<PaginatedResult<EmployeeResultDto>> InfoFormatFourEmpIdZeroEmployeeExists(int pageNumber, int pageSize, string? ageFormat, string? systemStatus, string? currentStatusDesc)
        {
            return await GetPaginatedEmployeeResultAsync(pageNumber, pageSize, ageFormat, systemStatus, currentStatusDesc);
        }
        //-----------------------------Start---------------------------------------------------------
        private async Task<int?> GetLinkLevelByRoleId(int roleId)
        {
            //int? linkLevel = await _context.SpecialAccessRights
            //    .Where(s => s.RoleId == roleId)
            //    .OrderBy(s => s.LinkLevel)
            //    .Select(s => s.LinkLevel)
            //    .FirstOrDefaultAsync();

            return await _context.EntityAccessRights02s
            .Where(e => e.RoleId == roleId)
            .OrderBy(e => e.LinkLevel)
            .Select(e => e.LinkLevel)
            .FirstOrDefaultAsync();
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
        private async Task<List<LinkItemDto>> GetEmployeeEntityLinks(int empId)
        {
            var empEntity = await _context.HrEmpMasters
            .Where(h => h.EmpId == empId)
            .Select(h => h.EmpEntity)
            .FirstOrDefaultAsync();
            return SplitStrings_XML(empEntity, ',')
            .Select((item, index) => new LinkItemDto
            {
                Item = item,
                LinkLevel = index + 2
            })
            .Where(c => !string.IsNullOrEmpty(c.Item))
            .ToList();
        }
        private async Task<List<int>> GetFilteredEmployeeIds(List<LinkItemDto> applicableFinalNew, bool exists)
        {
            var applicableEntityIds = applicableFinalNew.Select(x => x.Item).ToList();
            return await (from d in _context.HrEmpMasters
                          where exists ||
                          (d.IsDelete == false &&
                          _context.HighLevelViewTables
                          .Where(hlv => applicableEntityIds.Contains(hlv.LastEntityId.ToString()))
                          .Select(hlv => hlv.LastEntityId)
                          .Contains(d.LastEntity))
                          select d.EmpId)
            .ToListAsync();
        }
        public async Task<List<int>> GetEmpIdsByRoleAndEntityAccess(int roleId, int empId, bool exists)
        {
            int? linkSelect = await GetLinkLevelByRoleId(roleId);
            var entityAccessRights = await GetEntityAccessRights(roleId, linkSelect);
            var empEntityLinks = await GetEmployeeEntityLinks(empId);
            var applicableFinalNew = entityAccessRights.Concat(empEntityLinks).Distinct().ToList();
            return await GetFilteredEmployeeIds(applicableFinalNew, exists);
        }

        //-----------------------------End---------------------------------------------------------
        private async Task<List<int>> InfoFormatZeroOrOneLinkLevelNotExistAndZeroEmpIds(int roleId, int empId, bool exists)
        {
            return await GetEmpIdsByRoleAndEntityAccess(roleId, empId, exists);
        }
        private async Task<List<int>> InfoFormatTwoLinkLevelNotExistAndZeroEmpIds(int roleId, int empId, bool exists)
        {
            return await GetEmpIdsByRoleAndEntityAccess(roleId, empId, exists);
        }
        private async Task<List<int>> InfoFormatThreeLinkLevelNotExistAndZeroEmpIds(int roleId, int empId, bool exists)
        {
            return await GetEmpIdsByRoleAndEntityAccess(roleId, empId, exists);
        }
        private async Task<List<int>> InfoFormatFourLinkLevelNotExistAndZeroEmpIds(int roleId, int empId, bool exists)
        {
            return await GetEmpIdsByRoleAndEntityAccess(roleId, empId, exists);
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
        private static List<int> SplitAndParseEntityIds(string list, char delimiter = ',')
        {
            if (string.IsNullOrWhiteSpace(list)) return new List<int>();

            return list.Split(delimiter)
                       .Select(s => s.Trim())
                       .Where(s => int.TryParse(s, out _))
                       .Select(int.Parse)
                       .ToList();
        }
        private static string CalculateAge(DateTime? givenDate, string ageFormat)
        {
            if (!givenDate.HasValue)
                return "dd/MM/yyyy";
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
        private static Gender GetGender(string genderCode)
        {
            return genderCode switch
            {
                "M" => Gender.Male,
                "F" => Gender.Female,
                "O" => Gender.Other,
                null or _ => Gender.UnKnown
            };
        }
        private static MaritalStatus GetMaritalStatus(string genderCode)
        {
            return genderCode switch
            {
                "S" => MaritalStatus.Single,
                "M" => MaritalStatus.Married,
                "W" => MaritalStatus.Widowed,
                "X" => MaritalStatus.Separated,
                "D" => MaritalStatus.Divorcee,
                "U" => MaritalStatus.Unknown,
                null or _ => MaritalStatus.Unknown // Handle null or any unexpected values
            };
        }
        private static string FormatDate(DateTime? date, string format)
        {
            return date.HasValue ? date.Value.ToString(format) : string.Empty; // Or any other default value
        }
        private static string FormatString(string value, string defaultValue = "")
        {
            return string.IsNullOrEmpty(value) ? defaultValue : value;
        }
        public async Task<List<LanguageSkillResultDto>> LanguageSkillAsync(int employeeId)
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
                              LanguageId = emp.LanguageId,
                              Code = hrm.Code,
                              Description = hrm.Description,
                              Read = (byte)(emp.Read == true ? 1 : 0),
                              Write = (byte)(emp.Write == true ? 1 : 0),
                              Speak = (byte)(emp.Speak == true ? 1 : 0),
                              Comprehend = (byte)(emp.Comprehend == true ? 1 : 0),
                              MotherTongue = (byte)(emp.MotherTongue == true ? 1 : 0)
                          }).AsNoTracking().ToListAsync();
        }
        public async Task<CommunicationResultDto> CommunicationAsync(int employeeId)
        {
            var communicationTask = await (from a in _context.HrEmpAddresses
                                           join b in _context.AdmCountryMasters
                                           on a.Country equals b.CountryId into admGroup
                                           from b in admGroup.DefaultIfEmpty()
                                           where a.EmpId == employeeId
                                           select new CommunicationTableDto
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
                                               Status = _employeeSettings.EmployeeStatus
                                           }).AsNoTracking().ToListAsync();

            var communicationApprTask = await (from a in _context.HrEmpAddressApprls
                                               join b in _context.AdmCountryMasters
                                                   on a.Country equals b.CountryId into admGroup
                                               from b in admGroup.DefaultIfEmpty()
                                               join c in _context.CommunicationRequestWorkFlowstatuses
                                                   on a.AddId equals c.RequestId
                                               where a.EmpId == employeeId
                                               select new CommunicationTableDto
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
                                               }).AsNoTracking().ToListAsync();

            return new CommunicationResultDto
            {
                CommunicationTable = communicationTask,
                CommunicationTable1 = communicationApprTask
            };
        }
        public async Task<List<CommunicationExtraDto>> CommunicationExtraAsync(int employeeId)
        {
            var query1 = await (
                from a in _context.HrEmpAddress01Apprls
                join b in _context.AdmCountryMasters on a.Addr1Country equals b.CountryId into countryGroup1
                from b in countryGroup1.DefaultIfEmpty()
                join c in _context.AdmCountryMasters on a.Addr2Country equals c.CountryId into countryGroup2
                from c in countryGroup2.DefaultIfEmpty()
                join d in _context.CommunicationAdditionalRequestWorkFlowstatuses
                    on a.AddrId equals d.RequestId
                where a.EmpId == employeeId && d.ApprovalStatus == ApprovalStatus.Pending.ToString()
                select new AddressDto
                {
                    AddrID = a.AddrId,
                    EmpID = a.EmpId,
                    PermanentAddr = a.PermanentAddr ?? _employeeSettings.NotAvailable,
                    ContactAddr = a.ContactAddr ?? _employeeSettings.NotAvailable,
                    PinNo1 = a.PinNo1 ?? _employeeSettings.NotAvailable,
                    PinNo2 = a.PinNo2 ?? _employeeSettings.NotAvailable,
                    CountryID1 = b.CountryId == null ? 0 : b.CountryId,
                    Country1 = b.CountryName ?? _employeeSettings.NotAvailable,
                    CountryID2 = c.CountryId == null ? 0 : c.CountryId,
                    Country2 = c.CountryName ?? _employeeSettings.NotAvailable,
                    Status = d.ApprovalStatus,
                    PhoneNo = a.PhoneNo ?? _employeeSettings.NotAvailable,
                    AlterPhoneNo = a.AlterPhoneNo ?? _employeeSettings.NotAvailable,
                    MobileNo = a.MobileNo ?? _employeeSettings.NotAvailable
                }).AsNoTracking().ToListAsync();

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
                    PermanentAddr = a.PermanentAddr ?? _employeeSettings.NotAvailable,
                    ContactAddr = a.ContactAddr ?? _employeeSettings.NotAvailable,
                    PinNo1 = a.PinNo1 ?? _employeeSettings.NotAvailable,
                    PinNo2 = a.PinNo2 ?? _employeeSettings.NotAvailable,
                    CountryID1 = b.CountryId == null ? 0 : b.CountryId,
                    Country1 = b.CountryName ?? _employeeSettings.NotAvailable,
                    CountryID2 = c.CountryId == null ? 0 : c.CountryId,
                    Country2 = c.CountryName ?? _employeeSettings.NotAvailable,
                    Status = _employeeSettings.EmployeeStatus,// "A",
                    PhoneNo = a.PhoneNo ?? _employeeSettings.NotAvailable,
                    AlterPhoneNo = a.AlterPhoneNo ?? _employeeSettings.NotAvailable,
                    MobileNo = a.MobileNo ?? _employeeSettings.NotAvailable
                }).AsNoTracking().ToListAsync();

            var finalQuery = query1.Concat(query2).ToList();

            return new List<CommunicationExtraDto>
            {
                         new CommunicationExtraDto
                         {
                                Addresses = finalQuery.Select(r => new AddressDto
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
                         }
            };
        }
        public async Task<List<CommunicationEmergencyDto>> CommunicationEmergencyAsync(int employeeId)
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
                                    Status = _employeeSettings.EmployeeStatus,// "A",
                                    EmerName = a.EmerName,
                                    EmerRelation = a.EmerRelation
                                }).AsNoTracking().ToListAsync();

            return result;
        }
        public async Task<List<string>> HobbiesDataAsync(int employeeId)
        {
            return await (from a in _context.GeneralCategories
                          join b in _context.ReasonMasters on a.Id equals b.Value
                          join c in _context.EmployeeHobbies on b.ReasonId equals Convert.ToInt32(c.HobbieId)
                          where a.Description == _employeeSettings.Hobbies && c.EmployeeId == employeeId
                          select b.Description
                                     ).AsNoTracking().ToListAsync();

        }
        public async Task<List<RewardAndRecognitionDto>> RewardAndRecognitionsAsync(int employeeId)
        {
            return await (from a in _context.EmpRewards
                          join b in _context.AchievementMasters on a.AchievementId equals b.AchievementId
                          join c in _context.HrmValueTypes on a.RewardType equals c.Id
                          where a.EmpId == employeeId && a.Status == _employeeSettings.EmployeeStatus// "A"
                          select new RewardAndRecognitionDto
                          {
                              RewardID = a.RewardId,
                              Emp_id = a.EmpId,
                              Achievement = b.Description,
                              RewardType = c.Description,
                              RewardValue = c.Id,
                              Rewarddate = a.RewardDate.HasValue ? a.RewardDate.Value.ToString(_employeeSettings.DateFormat) : null,
                              Reason = a.Reason,
                              Amount = a.Amount,
                              rewardidtype = a.RewardType

                          }).AsNoTracking().ToListAsync();

        }
        public async Task<List<QualificationDto>> QualificationAsync(int employeeId)
        {
            var apprlResults = await (
                from a in _context.HrEmpQualificationApprls
                join c in _context.QualificationRequestWorkFlowstatuses on a.QlfId equals c.RequestId
                where a.EmpId == employeeId && c.ApprovalStatus == ApprovalStatus.Pending.ToString()
                select new QualificationTableDto
                {
                    Qlficationid = a.QlfId,
                    EmpId = a.EmpId,
                    Course = a.Course ?? _employeeSettings.NotAvailable,
                    University = a.University ?? _employeeSettings.NotAvailable,
                    InstName = a.InstName ?? _employeeSettings.NotAvailable,
                    DurFrm = a.DurFrm.HasValue ? a.DurFrm.Value.ToString(_employeeSettings.DateFormat) : _employeeSettings.NotAvailable,
                    DurTo = a.DurTo.HasValue ? a.DurTo.Value.ToString(_employeeSettings.DateFormat) : _employeeSettings.NotAvailable,
                    YearPass = a.YearPass ?? _employeeSettings.NotAvailable,
                    MarkPer = a.MarkPer ?? _employeeSettings.NotAvailable,
                    Class = a.Class ?? _employeeSettings.NotAvailable,
                    Subjects = a.Subjects ?? _employeeSettings.NotAvailable,
                    Status = a.Status ?? _employeeSettings.NotAvailable
                }).AsNoTracking().ToListAsync();

            var qualificationResults = await (
                from a in _context.HrEmpQualifications
                where a.EmpId == employeeId &&
                      !_context.HrEmpQualificationApprls.Any(q => q.EmpId == employeeId && q.QlfId == a.ApprlId)
                select new QualificationTableDto
                {
                    Qlficationid = a.QlfId,
                    EmpId = a.EmpId,
                    Course = a.Course ?? _employeeSettings.NotAvailable,
                    University = a.University ?? _employeeSettings.NotAvailable,
                    InstName = a.InstName ?? _employeeSettings.NotAvailable,
                    DurFrm = a.DurFrm.HasValue ? a.DurFrm.Value.ToString(_employeeSettings.DateFormat) : _employeeSettings.NotAvailable,
                    DurTo = a.DurTo.HasValue ? a.DurTo.Value.ToString(_employeeSettings.DateFormat) : _employeeSettings.NotAvailable,
                    YearPass = a.YearPass ?? _employeeSettings.NotAvailable,
                    MarkPer = a.MarkPer ?? _employeeSettings.NotAvailable,
                    Class = a.Class ?? _employeeSettings.NotAvailable,
                    Subjects = a.Subjects ?? _employeeSettings.NotAvailable,
                    Status = _employeeSettings.EmployeeStatus// "A"
                }).AsNoTracking().ToListAsync();

            var combinedResults = apprlResults.Concat(qualificationResults);

            var groupedResults = combinedResults
                .GroupBy(q => new
                {
                    q.Qlficationid,
                    q.EmpId,
                    q.Course,
                    q.University,
                    q.InstName,
                    q.DurFrm,
                    q.DurTo,
                    q.YearPass,
                    q.MarkPer,
                    q.Class,
                    q.Subjects,
                    q.Status
                })
                .OrderByDescending(g => DateTime.TryParse(g.Key.DurTo, out var dateResult) ? dateResult : DateTime.MinValue)
                .Select(g => g.First())
                .ToList();

            var attachments = await _context.QualificationAttachments
                .Where(a => a.EmpId == employeeId && a.DocStatus == ApprovalStatus.Approved.ToString())
                .Select(a => new QualificationFileDto
                {
                    QualAttachId = a.QualAttachId,
                    QualificationId = a.QualificationId,
                    QualFileName = a.QualFileName,
                    DocStatus = a.DocStatus
                }).AsNoTracking().ToListAsync();

            return new List<QualificationDto>
            {
                    new QualificationDto
                    {
                        QualificationTable = groupedResults,
                        QualificationFile = attachments
                    }
            };
        }
        public async Task<List<SkillSetDto>> SkillSetsAsync(int employeeId)
        {
            var apprlResultsQuery =
                from a in _context.HrEmpTechnicalApprls
                join b in _context.SkillSetRequestWorkFlowstatuses
                    on a.TechId equals b.RequestId
                where a.EmpId == employeeId && b.ApprovalStatus == ApprovalStatus.Pending.ToString()
                select new SkillSetDto
                {
                    Tech_id = a.TechId,
                    Emp_Id = a.EmpId,
                    Course = a.Course ?? _employeeSettings.NotAvailable,
                    Course_Dtls = a.CourseDtls ?? _employeeSettings.NotAvailable,
                    Inst_Name = a.InstName ?? _employeeSettings.NotAvailable,
                    Dur_Frm = a.DurFrm.HasValue ? a.DurFrm.Value.ToString(_employeeSettings.DateFormat) : _employeeSettings.NotAvailable,
                    Dur_To = a.DurTo.HasValue ? a.DurTo.Value.ToString(_employeeSettings.DateFormat) : _employeeSettings.NotAvailable,
                    Year = a.Year ?? _employeeSettings.NotAvailable,
                    Mark_Per = a.MarkPer ?? _employeeSettings.NotAvailable,
                    langSkills = a.LangSkills ?? _employeeSettings.NotAvailable,
                    Status = ApprovalStatus.Pending.ToString()
                };

            var technicalResultsQuery =
                from a in _context.HrEmpTechnicals
                where a.EmpId == employeeId &&
                      (!_context.HrEmpTechnicalApprls.Any(ta => ta.EmpId == employeeId && ta.TechId == a.ApprlId) || a.ApprlId == null)
                select new SkillSetDto
                {
                    Tech_id = a.TechId,
                    Emp_Id = a.EmpId,
                    Course = a.Course ?? _employeeSettings.NotAvailable,
                    Course_Dtls = a.CourseDtls ?? _employeeSettings.NotAvailable,
                    Inst_Name = a.InstName ?? _employeeSettings.NotAvailable,
                    Dur_Frm = a.DurFrm.HasValue ? a.DurFrm.Value.ToString(_employeeSettings.DateFormat) : _employeeSettings.NotAvailable,
                    Dur_To = a.DurTo.HasValue ? a.DurTo.Value.ToString(_employeeSettings.DateFormat) : _employeeSettings.NotAvailable,
                    Year = a.Year ?? _employeeSettings.NotAvailable,
                    Mark_Per = a.MarkPer ?? _employeeSettings.NotAvailable,
                    langSkills = a.LangSkills ?? _employeeSettings.NotAvailable,
                    Status = _employeeSettings.EmployeeStatus// "A"
                };

            // Execute queries asynchronously
            var apprlResults = await apprlResultsQuery.AsNoTracking().ToListAsync();
            var technicalResults = await technicalResultsQuery.AsNoTracking().ToListAsync();

            // Combine and order results
            var combinedResults = apprlResults.Concat(technicalResults)
                .OrderByDescending(r => DateTime.TryParse(r.Dur_To, out var dateResult) ? dateResult : DateTime.MinValue)
                .ToList();

            return combinedResults;
        }
        public async Task<List<DependentDto>> DependentAsync(int employeeId)
        {
            var result = await (from a in _context.Dependent00s
                                join b in _context.DependentMasters on a.RelationshipId equals b.DependentId
                                where a.DepEmpId == employeeId
                                select new DependentDto
                                {
                                    DepId = a.DepId,
                                    Description = b.Description,
                                    DateOfBirth = a.DateOfBirth.HasValue ? a.DateOfBirth.Value.ToString(_employeeSettings.DateFormat) : _employeeSettings.empSystemStatus,
                                    InterEmpID = a.InterEmpId,
                                    Type = a.Type,
                                    Phone = a.Description, // Assuming "Phone" is mapped to "Description"
                                    Gender = GetGender(a.Gender).ToString()
                                }).AsNoTracking().ToListAsync();

            return result;

        }
        public async Task<List<CertificationDto>> CertificationAsync(int employeeId)
        {
            // Fetch all lookup values at once to avoid multiple queries
            var lookupValues = await _context.ReasonMasters
                .Join(_context.GeneralCategories, rm => rm.Value, gc => gc.Id, (rm, gc) => new { rm.ReasonId, rm.Description, gc.Code })
                .Where(gc => new[] { "CERNAME", "CERTFIELD", "ISSUAUTH", "YRCMP" }.Contains(gc.Code))
              .AsNoTracking().ToListAsync();

            // Convert to dictionaries for quick lookup
            var certNames = lookupValues.Where(x => x.Code == "CERNAME").ToDictionary(x => x.ReasonId, x => x.Description);
            var certFields = lookupValues.Where(x => x.Code == "CERTFIELD").ToDictionary(x => x.ReasonId, x => x.Description);
            var issueAuths = lookupValues.Where(x => x.Code == "ISSUAUTH").ToDictionary(x => x.ReasonId, x => x.Description);
            var yearCompletions = lookupValues.Where(x => x.Code == "YRCMP").ToDictionary(x => x.ReasonId, x => x.Description);

            // Fetch employee certifications
            var certifications = await _context.EmployeeCertifications
                .Where(ec => ec.EmpId == employeeId && ec.Status != _employeeSettings.LetterD)
                .Select(ec => new CertificationDto
                {
                    empId = ec.EmpId,
                    certificationId = ec.CertificationId,
                    certificateName = certNames.ContainsKey((int)ec.CertificationName) ? certNames[(int)ec.CertificationName] : null,
                    certificateField = certFields.ContainsKey((int)ec.CertificationField) ? certFields[(int)ec.CertificationField] : null,
                    issuingAuthority = issueAuths.ContainsKey((int)ec.IssuingAuthority) ? issueAuths[(int)ec.IssuingAuthority] : null,
                    yearOfCompletion = yearCompletions.ContainsKey((int)ec.YearofCompletion) ? yearCompletions[(int)ec.YearofCompletion] : null
                }).AsNoTracking().ToListAsync();

            return certifications;
        }
        public async Task<List<DisciplinaryActionsDto>> DisciplinaryActionsAsync(int employeeId)
        {
            var result = await (from a in _context.AssignedLetterTypes
                                join b in _context.LetterMaster01s on a.LetterSubType equals b.ModuleSubId
                                join c in _context.LetterMaster00s on b.LetterTypeId equals c.LetterTypeId
                                where a.EmpId == employeeId &&
                                      c.LetterCode == "WRNG" &&
                                      (a.ApprovalStatus == _employeeSettings.Re || a.ApprovalStatus == _employeeSettings.EmployeeStatus)
                                orderby b.LetterSubName, a.ReleaseDate
                                select new DisciplinaryActionsDto
                                {
                                    EmpID = a.EmpId,
                                    LetterName = b.LetterSubName,
                                    Reason = a.Remark,
                                    LetterSubName = b.LetterSubName,
                                    IsLetterAttached = Convert.ToInt32(a.IsLetterAttached),
                                    ReleaseDate = a.ReleaseDate == null ? _employeeSettings.NotAvailable : a.ReleaseDate.Value.ToString(_employeeSettings.DateFormat),
                                    LetterReqID = Convert.ToInt32(a.LetterReqId),
                                    ApprovalStatus = _employeeSettings.E,
                                    UploadFileName = a.UploadFileName,
                                    IssueDate = a.IssueDate == null ? _employeeSettings.NotAvailable : a.IssueDate.Value.ToString(_employeeSettings.DateFormat),
                                    Template = string.IsNullOrEmpty(a.TemplateStyle) ? 0 : 1
                                }).AsNoTracking().ToListAsync();
            return result;
        }
        public async Task<List<LetterDto>> LetterAsync(int employeeId)
        {
            // Get the LetterJoinDate
            DateTime? letterJoinDate = await (_context.HrEmpMasters
                .Where(emp => emp.EmpId == employeeId)
                .Select(emp => emp.JoinDt)
                 .FirstOrDefaultAsync());

            // Query 1: Convert to LetterDto before Concat
            var query1 = from a in _context.AssignedLetterTypes
                         join b in _context.LetterMaster01s on a.LetterSubType equals b.ModuleSubId
                         join c in _context.LetterMaster00s on b.LetterTypeId equals c.LetterTypeId
                         where (a.ApprovalStatus == _employeeSettings.EmployeeStatus || a.ApprovalStatus == _employeeSettings.Re)
                               && a.EmpId == employeeId
                               && (b.AppointmentLetter ?? 0) == 0
                         group a by new { a.LetterReqId, b.LetterSubName, a.EmpId, a.ReleaseDate, b.AppointmentLetter } into grouped
                         select new LetterDto
                         {
                             LetterReqID = Convert.ToInt32(grouped.Key.LetterReqId),
                             LetterSubName = grouped.Key.LetterSubName,
                             EmpID = grouped.Key.EmpId,
                             ReleaseDate = grouped.Key.ReleaseDate.HasValue ? grouped.Key.ReleaseDate.Value.ToString(_employeeSettings.DateFormat) : null,
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
                             ReleaseDate = letterJoinDate.HasValue ? letterJoinDate.Value.ToString(_employeeSettings.DateFormat) : null,
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
                              ReleaseDate = a.CreatedDate.HasValue ? a.CreatedDate.Value.ToString(_employeeSettings.DateFormat) : null,
                              AppointmentLetter = -1
                          }).Take(1);

            // Materialize all queries to in-memory lists (ToList())
            var query1List = await query1.AsNoTracking().ToListAsync();
            var query2List = await query2.AsNoTracking().ToListAsync();
            var query3List = await query3.AsNoTracking().ToListAsync();

            // Concatenate the results
            var concatResult = query1List.Concat(query2List).Concat(query3List).OrderByDescending(r => r.ReleaseDate);

            // Return the final result as a list of LetterDto
            return concatResult.ToList();
        }
        public async Task<List<ReferenceDto>> ReferenceAsync(int employeeId)
        {
            var result = await (from a in _context.HrEmpreferences
                                join b in _context.ConsultantDetails on a.ConsultantId equals b.Id into consultantDetails
                                from bDetail in consultantDetails.DefaultIfEmpty()
                                join c in _context.EmployeeDetails on a.RefEmpId equals c.EmpId into employeeDetails
                                from cDetail in employeeDetails.DefaultIfEmpty()
                                where a.EmpId == employeeId && a.Status != ApprovalStatus.Deleted.ToString() // Filtering is applied only to HrEmpreferences
                                select new ReferenceDto
                                {
                                    RefID = a.RefId,
                                    RefTypedet = a.RefType == "I" ? "Internal" : a.RefType == _employeeSettings.E ? "External" : null,
                                    RefType = a.RefType,
                                    RefMethoddet = a.RefMethod == "C" ? "Consultant" :
                                                   a.RefMethod == "O" ? "Others" :
                                                   a.RefMethod == "D" ? "Direct" : _employeeSettings.NotAvailable,
                                    RefMethod = a.RefMethod,
                                    Id = bDetail.Id,
                                    ConsultantName = string.IsNullOrEmpty(bDetail.ConsultantName) ? _employeeSettings.NotAvailable : bDetail.ConsultantName,
                                    RefEmpName = cDetail.Name == null ? _employeeSettings.NotAvailable : cDetail.Name,
                                    Emp_Code = cDetail.EmpCode == null ? _employeeSettings.NotAvailable : cDetail.EmpCode,
                                    RefEmpID = a.RefEmpId == null ? 0 : a.RefEmpId,
                                    RefName = string.IsNullOrEmpty(a.RefName) ? _employeeSettings.NotAvailable : a.RefName,
                                    PhoneNo = string.IsNullOrEmpty(a.PhoneNo) ? _employeeSettings.NotAvailable : a.PhoneNo,
                                    Address = string.IsNullOrEmpty(a.Address) ? _employeeSettings.NotAvailable : a.Address
                                }).AsNoTracking().ToListAsync();

            return result;
        }
        private async Task<string> GetEmployeeExperienceLengthAsync(int empId, int expDays)
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
        public async Task<List<ProfessionalDto>> ProfessionalAsync(int employeeId)
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
                      (d.ApprovalStatus == ApprovalStatus.Pending.ToString() ||
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
                    Status = d != null && d.ApprovalStatus == ApprovalStatus.Pending.ToString() ? d.ApprovalStatus : _employeeSettings.EmployeeStatus
                }
            ).AsNoTracking().ToListAsync();

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
                    ? await GetEmployeeExperienceLengthAsync(item.EmpId, (item.LeavingDt.Value - item.JoinDt.Value).Days)
                    : "0Y: 0M: 0D";

                result.Add(new ProfessionalDto
                {
                    Prof_Id = item.ProfId,
                    Emp_Id = item.EmpId,
                    Join_Dt = item.JoinDt.HasValue ? item.JoinDt.Value.ToString(_employeeSettings.DateFormat) : _employeeSettings.NotAvailable,
                    Leaving_Dt = item.LeavingDt.HasValue ? item.LeavingDt.Value.ToString(_employeeSettings.DateFormat) : _employeeSettings.NotAvailable,
                    Leave_Reason = item.LeaveReason ?? _employeeSettings.NotAvailable,
                    Designation = item.Designation ?? _employeeSettings.NotAvailable,
                    Comp_Name = item.CompName ?? _employeeSettings.NotAvailable,
                    Comp_Address = item.CompAddress ?? _employeeSettings.NotAvailable,
                    PBno = item.Pbno ?? _employeeSettings.NotAvailable,
                    Job_Desc = item.JobDesc ?? _employeeSettings.NotAvailable,
                    Contact_Per = item.ContactPer ?? _employeeSettings.NotAvailable,
                    Contact_No = item.ContactNo ?? _employeeSettings.NotAvailable,
                    Currency_Id = item.CurrencyId,
                    Currency = item.Currency ?? _employeeSettings.NotAvailable,
                    Ctc = item.Ctc ?? _employeeSettings.NotAvailable,
                    Status = item.Status,
                    Years = years,
                    Months = months,
                    Days = days,
                    LengthOfService = lengthOfService
                });
            }

            return result;
        }
        public async Task<List<AssetDto>> AsseAsync()
        {
            var result = await (
                 from a in _context.CompanyParameters
                 where a.ParameterCode == "ASTDYN" && a.Type == _employeeSettings.CompanyParameterType //"COM"
                 select new AssetDto
                 {
                     DynamicAsset = a.Value == 0 ? 0 : a.Value
                 }).AsNoTracking().ToListAsync();

            return result;
        }
        public async Task<List<AssetDetailsDto>> AssetDetailsAsync(int employeeId)
        {


            var paramVal01 = await (from a in _context.CompanyParameters
                                    where a.ParameterCode == "AST" && a.Type == _employeeSettings.CompanyParameterType// "COM"
                                    select new
                                    {
                                        a.Value
                                    }).FirstOrDefaultAsync();
            int paramVal = paramVal01?.Value ?? 0;

            var paramDynVal01 = await (from a in _context.CompanyParameters
                                       where a.ParameterCode == "ASTDYN" && a.Type == _employeeSettings.CompanyParameterType// "COM"
                                       select new
                                       {
                                           a.Value
                                       }).FirstOrDefaultAsync();
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
                                        AssetModel = a.AssetModel ?? _employeeSettings.NotAvailable,
                                        Monitor = a.Monitor ?? _employeeSettings.NotAvailable,
                                        IWDate = a.InWarranty.HasValue ? a.InWarranty.Value.ToString(_employeeSettings.DateFormat) : _employeeSettings.NotAvailable,
                                        OWDate = a.OutOfWarranty.HasValue ? a.OutOfWarranty.Value.ToString(_employeeSettings.DateFormat) : _employeeSettings.NotAvailable,
                                        FieldValues = e.FieldValues,
                                        ReceivedDate = a.ReceivedDate.HasValue ? a.ReceivedDate.Value.ToString(_employeeSettings.DateFormat) : "N/A",
                                        Status = a.Status,
                                        ParamVal = paramDynVal,
                                        ParamDynVal = paramDynVal,
                                        AssignID = a.AssignId,
                                        Remarks = a.Remarks ?? "",
                                        ExpiryDate = a.ExpiryDate.HasValue ? a.ExpiryDate.Value.ToString(_employeeSettings.DateFormat) : _employeeSettings.NotAvailable,
                                        ReturnDate = a.ReturnDate.HasValue ? a.ReturnDate.Value.ToString(_employeeSettings.DateFormat) : _employeeSettings.NotAvailable,
                                        EmplinkID = c.EmplinkId ?? 0

                                    }).AsNoTracking().ToListAsync();
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
                                        AssetModel = a.AssetModel == null ? _employeeSettings.NotAvailable : a.AssetModel,
                                        Monitor = a.Monitor == null ? _employeeSettings.NotAvailable : a.Monitor,
                                        InWarranty = a.InWarranty.HasValue ? a.InWarranty.Value.ToString(_employeeSettings.DateFormat) : _employeeSettings.NotAvailable,
                                        OutOfWarranty = a.OutOfWarranty.HasValue ? a.OutOfWarranty.Value.ToString(_employeeSettings.DateFormat) : _employeeSettings.NotAvailable,
                                        ReceivedDate = a.ReceivedDate.HasValue ? a.ReceivedDate.Value.ToString(_employeeSettings.DateFormat) : _employeeSettings.NotAvailable,
                                        Status = a.Status,
                                        ParamVal = paramVal,
                                        ParamDynVal = 0,
                                        AssignID = a.AssignId,
                                        Remarks = a.Remarks == null ? _employeeSettings.NotAvailable : a.Remarks,
                                        ExpiryDate = a.ExpiryDate.HasValue ? a.ExpiryDate.Value.ToString(_employeeSettings.DateFormat) : _employeeSettings.NotAvailable,
                                        ReturnDate = a.ReturnDate.HasValue ? a.ReturnDate.Value.ToString(_employeeSettings.DateFormat) : _employeeSettings.NotAvailable





                                    }).AsNoTracking().ToListAsync();
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

                                    }).OrderByDescending(x => x.AssignedDate).AsNoTracking().ToListAsync();


                return result;
            }



        }
        public async Task<List<CurrencyDropdown_ProfessionalDto>> CurrencyDropdownProfessionalAsync()
        {
            var result = await (
                 from a in _context.CurrencyMasters
                 select new CurrencyDropdown_ProfessionalDto
                 {
                     Currency_Id = a.CurrencyId,
                     Currency = a.Currency
                 }).AsNoTracking().ToListAsync();

            return result;
        }
        private string GetSequence(int employeeId, int mainMasterId, string entity = "", int firstEntity = 0)
        {
            string sequence = null;
            int? codeId = null;
            bool isLevel17 = (from ls in _context.LevelSettingsAccess00s
                              join tm in _context.TransactionMasters
                              on ls.TransactionId equals tm.TransactionId
                              where tm.TransactionType == _employeeSettings.SeqGen && ls.Levels == "17"
                              select ls).Any();

            if (isLevel17)
            {
                if (!string.IsNullOrEmpty(entity))
                {

                    var entityQuery = (from a in _context.EntityApplicable00s
                                       join c in _context.AdmCodegenerationmasters
                                       on a.MasterId equals c.CodeId
                                       join tm in _context.TransactionMasters
                                       on a.TransactionId equals tm.TransactionId
                                       where tm.TransactionType == _employeeSettings.SeqGen &&
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
                                                where tm.TransactionType == _employeeSettings.SeqGen &&
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
                                            where tm.TransactionType == _employeeSettings.SeqGen &&
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
                                    where tm.TransactionType == _employeeSettings.SeqGen &&
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
                                             where tm.TransactionType == _employeeSettings.SeqGen &&
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
        //    public async Task<(int ErrorID, string ErrorMessage)> UpdateProfessionalDetailsAsync(
        //string updateType, int empID, int ProfId, string companyName, string designation,
        //string companyAddress, string postboxNo, string contactPerson, long? contactNo,
        //string jobDescription, DateTime? joiningDate, DateTime? leavingDate, string leaveReason,
        //int? ctc, int? currencyID, int entryBy, DateTime? entryDate)
        //    {
        //        try
        //        {

        //            if (updateType == "Pending")
        //            {
        //                var record = await _context.HrEmpProfdtlsApprls
        //                    .FirstOrDefaultAsync(x => x.ProfId == ProfId && x.EmpId == empID);

        //                if (record != null)
        //                {
        //                    record.CompName = companyName;
        //                    record.Designation = designation;
        //                    record.CompAddress = companyAddress;
        //                    record.Pbno = postboxNo;
        //                    record.ContactPer = contactPerson;
        //                    record.ContactNo = contactNo.ToString();
        //                    record.JobDesc = jobDescription;
        //                    record.JoinDt = joiningDate;
        //                    record.LeavingDt = leavingDate;
        //                    record.LeaveReason = leaveReason;
        //                    record.Ctc = ctc.ToString();
        //                    record.CurrencyId = currencyID;
        //                    record.EntryBy = entryBy;
        //                    record.EntryDt = DateTime.UtcNow;

        //                    await _context.SaveChangesAsync();
        //                }
        //            }
        //            else if (updateType == "Approved")
        //            {

        //                var workflowNeed = await IsWorkflowNeeded();

        //                string? codeId = await GenerateRequestId(empID);
        //                var requestID = await _context.AdmCodegenerationmasters
        //                       .Where(c => c.CodeId == Convert.ToInt32(codeId))
        //                       .Select(c => c.LastSequence)
        //                       .FirstOrDefaultAsync();
        //                if (workflowNeed.Equals(true))
        //                {
        //                    var transactionID = await _context.TransactionMasters
        //                        .Where(t => t.TransactionType == "Professional")
        //                        .Select(t => t.TransactionId)
        //                        .FirstOrDefaultAsync();

        //                    //var codeID = await context.GetSequence(empID, transactionID, null, null);

        //                    if (codeId == null)
        //                    {
        //                        return (0, "NoSequence");
        //                    }



        //                    var newApproval = new HrEmpProfdtlsApprl
        //                    {
        //                        EmpId = empID,
        //                        CompName = companyName,
        //                        Designation = designation,
        //                        CompAddress = companyAddress,
        //                        Pbno = postboxNo,
        //                        ContactPer = contactPerson,
        //                        ContactNo = contactNo.ToString(),
        //                        JobDesc = jobDescription,
        //                        JoinDt = joiningDate,
        //                        LeavingDt = leavingDate,
        //                        LeaveReason = leaveReason,
        //                        Ctc = ctc.ToString(),
        //                        CurrencyId = currencyID,
        //                        Status = "P",
        //                        FlowStatus = "E",
        //                        EntryBy = entryBy,
        //                        EntryDt = entryDate ?? DateTime.UtcNow,
        //                        RequestId = requestID,
        //                        MasterId = ProfId
        //                    };

        //                    await _context.HrEmpProfdtlsApprls.AddAsync(newApproval);
        //                    await _context.SaveChangesAsync();

        //                    var newErrorID = newApproval.ProfId;

        //                    //await context.WorkFlowActivityFlow(empID, "Professional", newErrorID, entryBy);

        //                    await ExecuteWorkFlowActivityFlow(empID, "Professional", newErrorID.ToString(), entryBy.ToString());

        //                    return (newErrorID, "Successfully Updated");
        //                }
        //                else if (workflowNeed.Equals(false))
        //                {
        //                    var record = await _context.HrEmpProfdtls
        //                        .FirstOrDefaultAsync(x => x.ProfId == ProfId && x.EmpId == empID);

        //                    if (record != null)
        //                    {
        //                        record.CompName = companyName;
        //                        record.Designation = designation;
        //                        record.CompAddress = companyAddress;
        //                        record.Pbno = postboxNo;
        //                        record.ContactPer = contactPerson;
        //                        record.ContactNo = contactNo.ToString();
        //                        record.JobDesc = jobDescription;
        //                        record.JoinDt = joiningDate;
        //                        record.LeavingDt = leavingDate;
        //                        record.LeaveReason = leaveReason;
        //                        record.Ctc = ctc.ToString();
        //                        record.CurrencyId = currencyID;
        //                        record.EntryBy = entryBy;
        //                        record.EntryDt = DateTime.UtcNow;
        //                        await _context.SaveChangesAsync();

        //                    }

        //                    var approval = new HrEmpProfdtlsApprl
        //                    {
        //                        EmpId = empID,
        //                        CompName = companyName,
        //                        Designation = designation,
        //                        CompAddress = companyAddress,
        //                        Pbno = postboxNo,
        //                        ContactPer = contactPerson,
        //                        ContactNo = contactNo.ToString(),
        //                        JobDesc = jobDescription,
        //                        JoinDt = joiningDate,
        //                        LeavingDt = leavingDate,
        //                        LeaveReason = leaveReason,
        //                        Ctc = ctc.ToString(),
        //                        CurrencyId = currencyID,
        //                        Status = "A",
        //                        FlowStatus = "E",
        //                        EntryBy = entryBy,
        //                        EntryDt = entryDate ?? DateTime.UtcNow,
        //                        RequestId = requestID,
        //                        MasterId = ProfId
        //                    };

        //                    await _context.HrEmpProfdtlsApprls.AddAsync(approval);
        //                    await _context.SaveChangesAsync();

        //                    var empRecord = await _context.HrEmpMasters.FirstOrDefaultAsync(x => x.EmpId == empID);
        //                    if (empRecord != null)
        //                    {
        //                        empRecord.ModifiedDate = DateTime.UtcNow;
        //                        await _context.SaveChangesAsync();
        //                    }
        //                }


        //                return (0, "Successfully Updated");
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            return (-1, ex.Message);
        //        }
        //        return (0, "Successfully Updated");
        //    }


        public async Task<(int ErrorID, string ErrorMessage)> UpdateProfessionalDetailsAsync(HrEmpProfdtlsApprlDto dto)
        {
            try
            {
                var rr = await GetEmployeeAndSystemStatusesAsync(dto.EmpId);


                if (dto.updateType == "Pending")
                {
                    var record = await _context.HrEmpProfdtlsApprls
                        .FirstOrDefaultAsync(x => x.ProfId == dto.ProfId && x.EmpId == dto.EmpId);

                    if (record != null)
                    {
                        record.CompName = dto.CompName;
                        record.Designation = dto.Designation;
                        record.CompAddress = dto.CompAddress;
                        record.Pbno = dto.Pbno;
                        record.ContactPer = dto.ContactPer;
                        record.ContactNo = dto.ContactNo;
                        record.JobDesc = dto.JobDesc;
                        record.JoinDt = dto.JoinDt;
                        record.LeavingDt = dto.LeavingDt;
                        record.LeaveReason = dto.LeaveReason;
                        record.Ctc = dto.Ctc;
                        record.CurrencyId = dto.CurrencyId;
                        record.EntryBy = dto.EntryBy;
                        record.EntryDt = DateTime.UtcNow;

                        await _context.SaveChangesAsync();
                    }
                }
                else if (dto.updateType == "Approved")
                {
                    var workflowNeed = await IsWorkflowNeeded();
                    string? codeId = await GenerateRequestId(dto.EmpId);
                    var requestID = await _context.AdmCodegenerationmasters
                        .Where(c => c.CodeId == Convert.ToInt32(codeId))
                        .Select(c => c.LastSequence)
                        .FirstOrDefaultAsync();

                    if (workflowNeed)
                    {
                        if (codeId == null)
                        {
                            return (0, "NoSequence");
                        }

                        var newApproval = new HrEmpProfdtlsApprl
                        {
                            EmpId = dto.EmpId,
                            CompName = dto.CompName,
                            Designation = dto.Designation,
                            CompAddress = dto.CompAddress,
                            Pbno = dto.Pbno,
                            ContactPer = dto.ContactPer,
                            ContactNo = dto.ContactNo,
                            JobDesc = dto.JobDesc,
                            JoinDt = dto.JoinDt,
                            LeavingDt = dto.LeavingDt,
                            LeaveReason = dto.LeaveReason,
                            Ctc = dto.Ctc,
                            CurrencyId = dto.CurrencyId,
                            Status = "P",
                            FlowStatus = "E",
                            EntryBy = dto.EntryBy,
                            EntryDt = dto.EntryDt,
                            RequestId = requestID,
                            MasterId = dto.ProfId
                        };

                        await _context.HrEmpProfdtlsApprls.AddAsync(newApproval);
                        await _context.SaveChangesAsync();

                        var newErrorID = newApproval.ProfId;
                        await ExecuteWorkFlowActivityFlow(dto.EmpId, "Professional", newErrorID.ToString(), dto.EntryBy.ToString());

                        return (newErrorID, "Successfully Updated");
                    }
                    else
                    {
                        var record = await _context.HrEmpProfdtls
                            .FirstOrDefaultAsync(x => x.ProfId == dto.ProfId && x.EmpId == dto.EmpId);

                        if (record != null)
                        {
                            record.CompName = dto.CompName;
                            record.Designation = dto.Designation;
                            record.CompAddress = dto.CompAddress;
                            record.Pbno = dto.Pbno;
                            record.ContactPer = dto.ContactPer;
                            record.ContactNo = dto.ContactNo;
                            record.JobDesc = dto.JobDesc;
                            record.JoinDt = dto.JoinDt;
                            record.LeavingDt = dto.LeavingDt;
                            record.LeaveReason = dto.LeaveReason;
                            record.Ctc = dto.Ctc;
                            record.CurrencyId = dto.CurrencyId;
                            record.EntryBy = dto.EntryBy;
                            record.EntryDt = DateTime.UtcNow;

                            await _context.SaveChangesAsync();
                        }

                        var approval = new HrEmpProfdtlsApprl
                        {
                            EmpId = dto.EmpId,
                            CompName = dto.CompName,
                            Designation = dto.Designation,
                            CompAddress = dto.CompAddress,
                            Pbno = dto.Pbno,
                            ContactPer = dto.ContactPer,
                            ContactNo = dto.ContactNo,
                            JobDesc = dto.JobDesc,
                            JoinDt = dto.JoinDt,
                            LeavingDt = dto.LeavingDt,
                            LeaveReason = dto.LeaveReason,
                            Ctc = dto.Ctc,
                            CurrencyId = dto.CurrencyId,
                            Status = "A",
                            FlowStatus = "E",
                            EntryBy = dto.EntryBy,
                            EntryDt = dto.EntryDt,
                            RequestId = requestID,
                            MasterId = dto.ProfId
                        };

                        await _context.HrEmpProfdtlsApprls.AddAsync(approval);
                        await _context.SaveChangesAsync();

                        var empRecord = await _context.HrEmpMasters.FirstOrDefaultAsync(x => x.EmpId == dto.EmpId);
                        if (empRecord != null)
                        {
                            empRecord.ModifiedDate = DateTime.UtcNow;
                            await _context.SaveChangesAsync();
                        }
                    }
                }

                return (0, "Successfully Updated");
            }
            catch (Exception ex)
            {
                return (-1, ex.Message);
            }
        }

        public async Task<string?> InsertOrUpdateProfessionalData(HrEmpProfdtlsApprlDto profdtlsApprlDto)
        {
            var existingEntity = await _context.HrEmpProfdtlsApprls
                .FirstOrDefaultAsync(e => e.EmpId == profdtlsApprlDto.EmpId &&
                                          e.JoinDt.HasValue && e.JoinDt.Value.Date == profdtlsApprlDto.JoinDt &&
                                          e.LeavingDt.HasValue && e.LeavingDt.Value.Date == profdtlsApprlDto.LeavingDt);
            if (existingEntity != null)
            {
                return _employeeSettings.DataInsertFailedStatus;
            }

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                bool isWorkflowNeeded = await IsWorkflowNeeded();
                var hrEmpProfdtlsApprl = _mapper.Map<HrEmpProfdtlsApprl>(profdtlsApprlDto);
                hrEmpProfdtlsApprl.EntryBy = profdtlsApprlDto.EntryBy;
                hrEmpProfdtlsApprl.EntryDt = DateTime.UtcNow; // Ensure Entry Date is set

                if (isWorkflowNeeded)
                {
                    string? codeId = await GenerateRequestId(profdtlsApprlDto.EmpId);
                    if (codeId == null)
                    {
                        await transaction.RollbackAsync();
                        return "NoSequence";  // Match SQL logic
                    }

                    hrEmpProfdtlsApprl.RequestId = await GetLastSequence(codeId);
                    await _context.HrEmpProfdtlsApprls.AddAsync(hrEmpProfdtlsApprl);
                    await UpdateCodeGeneration(codeId);

                    // Call Workflow Activity Flow - Assuming a method exists
                    await ExecuteWorkFlowActivityFlow(profdtlsApprlDto.EmpId, "Professional", hrEmpProfdtlsApprl.RequestId, profdtlsApprlDto.EntryBy.ToString());
                }
                else
                {

                    var hrEmpProfdtl = new HrEmpProfdtl
                    {
                        InstId = profdtlsApprlDto.InstId,
                        ProfId = profdtlsApprlDto.ProfId,
                        EmpId = profdtlsApprlDto.EmpId,
                        CompName = profdtlsApprlDto.CompName,
                        Designation = profdtlsApprlDto.Designation,
                        CompAddress = profdtlsApprlDto.CompAddress,
                        Pbno = profdtlsApprlDto.Pbno,
                        ContactPer = profdtlsApprlDto.ContactPer,
                        ContactNo = profdtlsApprlDto.ContactNo,
                        JobDesc = profdtlsApprlDto.JobDesc,
                        JoinDt = profdtlsApprlDto.JoinDt,
                        LeavingDt = profdtlsApprlDto.LeavingDt,
                        LeaveReason = profdtlsApprlDto.LeaveReason,
                        Ctc = profdtlsApprlDto.Ctc,
                        CurrencyId = profdtlsApprlDto.CurrencyId,
                        CompSite = profdtlsApprlDto.CompSite,
                        EntryBy = profdtlsApprlDto.EntryBy,
                        EntryDt = profdtlsApprlDto.EntryDt,
                    };
                    await _context.HrEmpProfdtls.AddAsync(hrEmpProfdtl);

                    //await _context.SaveChangesAsync();

                    await InsertProfessionalDetails(profdtlsApprlDto.EmpId);
                }

                // Update HR_EMP_MASTER.ModifiedDate
                var empMaster = await _context.HrEmpMasters.FirstOrDefaultAsync(e => e.EmpId == profdtlsApprlDto.EmpId);
                if (empMaster != null)
                {
                    empMaster.ModifiedDate = DateTime.UtcNow;
                    await _context.SaveChangesAsync();
                }

                int result = await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return result > 0 ? _employeeSettings.DataInsertSuccessStatus : _employeeSettings.DataInsertFailedStatus;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        // Fixes in GetLastSequence
        public async Task<string?> GetLastSequence(string codeId)
        {
            return await _context.AdmCodegenerationmasters
                .Where(a => a.CodeId.ToString() == codeId) // Fix condition
                .Select(a => a.LastSequence)
                .FirstOrDefaultAsync();
        }

        // Fixes in UpdateCodeGeneration
        public async Task UpdateCodeGeneration(string codeId)
        {
            var codeGenEntity = await _context.AdmCodegenerationmasters.FirstOrDefaultAsync(c => c.CodeId.ToString() == codeId);
            if (codeGenEntity != null)
            {
                codeGenEntity.CurrentCodeValue = (codeGenEntity.CurrentCodeValue ?? 0) + 1;

                // Validate substring length before applying it
                int length = codeGenEntity.NumberFormat.Length - codeGenEntity.CurrentCodeValue.ToString().Length;
                string sequence = length > 0 ? codeGenEntity.NumberFormat[..length] : "";

                codeGenEntity.LastSequence = $"{codeGenEntity.Code}{sequence}{codeGenEntity.CurrentCodeValue}";
                await _context.SaveChangesAsync();
            }
        }

        // New Method: Execute WorkFlowActivityFlow
        private async Task ExecuteWorkFlowActivityFlow(int empId, string transactionType, string? requestId, string entryBy)
        {
            // Assuming there’s a stored procedure call or business logic
            await _context.Database.ExecuteSqlRawAsync(
                "EXEC WorkFlowActivityFlow @p0, @p1, @p2, @p3",
                empId, transactionType, requestId, entryBy
            );
        }


        //public async Task<string?> InsertOrUpdateProfessionalData(HrEmpProfdtlsApprlDto profdtlsApprlDto)
        //{
        //    var existingEntity = await _context.HrEmpProfdtlsApprls
        //        .FirstOrDefaultAsync(e => e.EmpId == profdtlsApprlDto.EmpId &&
        //                                  e.JoinDt.HasValue && e.JoinDt.Value.Date == profdtlsApprlDto.JoinDt &&
        //                                  e.LeavingDt.HasValue && e.LeavingDt.Value.Date == profdtlsApprlDto.LeavingDt);
        //    if (existingEntity != null)
        //    {
        //        return _employeeSettings.DataInsertFailedStatus;
        //    }

        //    using var transaction = await _context.Database.BeginTransactionAsync();
        //    try
        //    {
        //        bool isWorkflowNeeded = await IsWorkflowNeeded();
        //        var hrEmpProfdtlsApprl = _mapper.Map<HrEmpProfdtlsApprl>(profdtlsApprlDto);

        //        if (isWorkflowNeeded)
        //        {
        //            string? codeId = await GenerateRequestId(profdtlsApprlDto.EmpId);
        //            if (codeId != null)
        //            {
        //                hrEmpProfdtlsApprl.RequestId = await GetLastSequence(codeId);
        //                await _context.HrEmpProfdtlsApprls.AddAsync(hrEmpProfdtlsApprl);
        //                await UpdateCodeGeneration(codeId);
        //            }
        //        }
        //        else
        //        {

        //            var hrEmpProfdtlsApprlDto = _mapper.Map<HrEmpProfdtlsApprl>(profdtlsApprlDto);

        //            await _context.HrEmpProfdtlsApprls.AddAsync(hrEmpProfdtlsApprlDto);
        //            await _context.SaveChangesAsync();
        //            await InsertProfessionalDetails(profdtlsApprlDto.EmpId);

        //        }
        //        var empMaster = await _context.HrEmpMasters.FirstOrDefaultAsync(e => e.EmpId == profdtlsApprlDto.EmpId);
        //        if (empMaster != null)
        //        {
        //            empMaster.ModifiedDate = DateTime.UtcNow;
        //            await _context.SaveChangesAsync();
        //        }
        //        int result = await _context.SaveChangesAsync();
        //        await transaction.CommitAsync();
        //        string? strMessage = result > 0 ? _employeeSettings.DataInsertSuccessStatus : _employeeSettings.DataInsertFailedStatus;
        //        return strMessage;
        //    }
        //    catch
        //    {
        //        await transaction.RollbackAsync();
        //        throw;
        //    }
        //}
        public async Task<bool> IsWorkflowNeeded()
        {
            return await _context.CompanyParameters
                .Join(_context.HrmValueTypes, a => a.Value, b => b.Value, (a, b) => new { a, b })
                .Where(x => x.b.Type == "EmployeeReporting" && x.a.ParameterCode == "WRKFLO")
                .Select(x => x.b.Code)
                .FirstOrDefaultAsync() == "Yes";
        }
        public async Task<string?> GenerateRequestId(int empId)
        {
            var transactionID = await _context.TransactionMasters
                .Where(a => a.TransactionType == "Professional")
                .Select(a => a.TransactionId)
                .FirstOrDefaultAsync();

            return GetSequence(empId, transactionID, "", 0);
        }
        //public async Task<string?> GetLastSequence(string codeId)
        //{
        //    return await _context.AdmCodegenerationmasters
        //        .Where(a => a.Code == codeId)
        //        .Select(a => a.LastSequence)
        //        .FirstOrDefaultAsync();
        //}
        //public async Task UpdateCodeGeneration(string codeId)
        //{
        //    var codeGenEntity = await _context.AdmCodegenerationmasters.FirstOrDefaultAsync(c => c.CodeId == Convert.ToInt32(codeId));

        //    if (codeGenEntity != null)
        //    {
        //        codeGenEntity.CurrentCodeValue = (codeGenEntity.CurrentCodeValue ?? 0) + 1;
        //        codeGenEntity.LastSequence = $"{codeGenEntity.Code}{codeGenEntity.NumberFormat[..^codeGenEntity.CurrentCodeValue.ToString().Length]}{codeGenEntity.CurrentCodeValue}";
        //        await _context.SaveChangesAsync();
        //    }
        //}
        private async Task InsertProfessionalDetails(int empId)
        {
            var profdtlsApprlDto = await _context.HrEmpProfdtlsApprls
                .Where(x => x.EmpId == empId)
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

            if (profdtlsApprlDto != null)
            {
                var hrEmpProfdtl = _mapper.Map<HrEmpProfdtl>(profdtlsApprlDto);
                hrEmpProfdtl.EntryDt = DateTime.UtcNow;
                await _context.HrEmpProfdtls.AddAsync(hrEmpProfdtl);
            }

            var employee = await _context.HrEmpMasters.FirstOrDefaultAsync(e => e.EmpId == empId);
            if (employee != null)
            {
                employee.ModifiedDate = DateTime.UtcNow;
            }
        }
        public async Task<List<HrEmpProfdtlsApprlDto>> GetProfessionalByIdAsync(string updateType, int detailID, int empID)
        {
            var query = (updateType == "Pending")
                ? _context.HrEmpProfdtlsApprls
                    .Select(a => new { a.ProfId, a.EmpId, a.CompName, a.Designation, a.CompAddress, a.Pbno, a.ContactPer, a.ContactNo, a.JobDesc, a.JoinDt, a.LeavingDt, a.LeaveReason, a.Ctc, a.CurrencyId })
                : _context.HrEmpProfdtls
                    .Select(a => new { a.ProfId, a.EmpId, a.CompName, a.Designation, a.CompAddress, a.Pbno, a.ContactPer, a.ContactNo, a.JobDesc, a.JoinDt, a.LeavingDt, a.LeaveReason, a.Ctc, a.CurrencyId });

            return await (from a in query
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
                          }).AsNoTracking().ToListAsync();
        }
        public async Task<List<AllDocumentsDto>> DocumentsAsync(int employeeId, List<string> excludedDocTypes)
        {

            var tempDocumentFill = await (from a in _context.ReasonMasterFieldValues
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
                                          }).AsNoTracking().ToListAsync();
            var tempDocumentFillList = tempDocumentFill;
            //var tempDocumentFillDict = tempDocumentFillList.ToDictionary(x => x.ReasonId, x => x);
            var tempDocumentFillDict = tempDocumentFillList
    .DistinctBy(x => x.ReasonId)
    .ToDictionary(x => x.ReasonId, x => x);

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
                                                && t1.Status == _employeeSettings.EmployeeStatus
                                                && !excludedDocTypes.Contains(t5.DocType)
                                                && t3.DocDescription != _employeeSettings.Active// "IsActive"

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
                                    ).OrderBy(x => x.DocID).AsNoTracking().ToListAsync();




            var docList = await (from a in _context.HrmsDocument00s
                                 join b in _context.HrmsDocTypeMasters on (a.DocType.HasValue ? (long)a.DocType.Value : -1L) equals b.DocTypeId
                                 //join c in _context.EmpDocumentAccesses  on new { DocID = (long)a.DocId , EmpID = employeeId } equals new { c.DocId, c.EmpId }
                                 join c in _context.EmpDocumentAccesses
                                on new { DocID = (long)a.DocId, EmpID = (long)employeeId }
                                equals new { DocID = c.DocId ?? -1L, EmpID = c.EmpId ?? -1L }
                                 where !(
                                     from e in _context.HrmsEmpdocuments00s
                                     join f in _context.HrmsEmpdocuments01s on e.DetailId equals f.DetailId
                                     //where e.EmpId == employeeId && e.Status != "R" && e.Status != "D" && e.DocId == a.DocId
                                     where e.EmpId == employeeId && e.Status != ApprovalStatus.Rejceted.ToString() && e.Status != ApprovalStatus.Deleted.ToString() && e.DocId == a.DocId
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
                                 }).Distinct().AsNoTracking().ToListAsync();


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
                                       !_context.HrmsEmpdocumentsApproved00s.Any(ap => ap.DetailId == t1.DetailId && ap.EmpId == employeeId && ap.Status == ApprovalStatus.Approved.ToString()) &&
                                       t1.Status == ApprovalStatus.Pending.ToString() && t3.DocDescription != _employeeSettings.Active && !excludedDocTypes.Contains(t5.DocType)
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
                                          }).AsNoTracking().ToListAsync();



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
            //var pendingFiles = files.Where(f => f.Status == ApprovalStatus.Pending.ToString()).ToList();
            //var approvedFiles = files.Where(f => f.Status == ApprovalStatus.Approved.ToString()).ToList();

            var pendingFiles = files.Where(f => f.Status == _employeeSettings.Status02).ToList();
            var approvedFiles = files.Where(f => f.Status == _employeeSettings.EmployeeStatus).ToList();

            return new List<AllDocumentsDto>
            {
                 new AllDocumentsDto
                 {
                     //TempDocumentFill = tempDocumentFillList,
                     ApprovedDocuments = ApprovedDos,
                     DocumentList = docList,
                     PendingDocumentsDto = pendingDocuments,
                     PendingFiles = pendingFiles,
                     ApprovedFiles = approvedFiles
                 }
            };

        }
        public async Task<List<PersonalDetailsDto>> GetPersonalDetailsByIdAsync(int employeeid)
        {
            var enableWeddingDate = await GetDefaultCompanyParameter(employeeid, "ENABLEWEDDINGDATE", _employeeSettings.companyParameterCodesType);// "EMP1"

            var employeeDetails = await (from a in _context.HrEmpMasters
                                         join c in _context.HrEmpPersonals on a.EmpId equals c.EmpId
                                         join b in _context.HrEmpAddresses on a.EmpId equals b.EmpId into personalEmailGroup
                                         from b in personalEmailGroup.DefaultIfEmpty()
                                         join d in _context.AdmCountryMasters on c.Country equals d.CountryId into countryGroup
                                         from d in countryGroup.DefaultIfEmpty()
                                         join e in _context.AdmCountryMasters on c.Nationality equals e.CountryId into nationalityGroup
                                         from e in nationalityGroup.DefaultIfEmpty()
                                         join f in _context.AdmCountryMasters on c.CountryOfBirth equals f.CountryId into countryOfBirthGroup
                                         from f in countryOfBirthGroup.DefaultIfEmpty()
                                         join g in _context.AdmReligionMasters on c.Religion equals g.ReligionId into religionGroup
                                         from g in religionGroup.DefaultIfEmpty()
                                         where a.EmpId == employeeid
                                         select new PersonalDetailsDto
                                         {
                                             DateOfBirth = a.DateOfBirth.HasValue ? a.DateOfBirth.Value.ToString(_employeeSettings.DateFormat) : _employeeSettings.NotAvailable,

                                             Wedding_Date = c.WeddingDate,
                                             EMail = b.PersonalEmail ?? "",
                                             CountryID = c.Country,
                                             NationalityID = c.Nationality,
                                             CountryOfBirthID = c.CountryOfBirth,
                                             Blood_Grp = c.BloodGrp ?? "",
                                             ReligionID = c.Religion,
                                             Religion_Name = g.ReligionName ?? "",
                                             Ident_Mark = c.IdentMark ?? "",
                                             Height = c.Height ?? "",
                                             Weight = c.Weight ?? "",
                                             GenderID = c.Gender,
                                             Gender = GetGender(c.Gender).ToString(),
                                             Marital_Status = GetMaritalStatus(c.MaritalStatus).ToString(),
                                             Marital_StatusID = c.MaritalStatus,
                                             Guardians_Name = a.GuardiansName ?? "",
                                             Country = d.CountryName ?? "",
                                             Nationality = e.CountryName ?? "",
                                             CountryOfBirth = f.CountryId,
                                             bloodgroupnew = string.IsNullOrEmpty(c.BloodGrp) ? "" :
                                                             c.BloodGrp == "HH" ? "HH Group" : c.BloodGrp + "ve"
                                         }).AsNoTracking().ToListAsync();


            return employeeDetails;
        }
        public async Task<List<TrainingDto>> TrainingAsync(int employeeid)
        {
            var result = await (from hem in _context.HrEmpMasters
                                join ts in _context.TrainingSchedules on hem.EmpId equals ts.EmpId
                                join tm in _context.TrainingMasters on ts.TrMasterId equals tm.TrMasterId
                                join tm01 in _context.TrainingMaster01s on tm.TrMasterId equals tm01.TrMasterId into tm01Join
                                from tm01 in tm01Join.DefaultIfEmpty()
                                where hem.EmpId == employeeid && ts.SelectStatus == "S" && tm.Active == "Y"
                                select new TrainingDto
                                {
                                    Emp_Id = hem.EmpId,
                                    trMasterId = Convert.ToInt32(tm.TrMasterId),
                                    Emp_Code = hem.EmpCode,
                                    trName = tm.TrName,
                                    FileUrl = tm01.FileUrl,
                                    FileUpdId = tm01.FileUpdId,
                                    FileName = tm01.FileName,
                                    EmpName = hem.FirstName,//(hem.First_Name ?? " ") + " " + (hem.Middle_Name ?? " ") + " " + (hem.Last_Name ?? " "),
                                    IsSurvey = tm.IsSurvey,
                                    Survey = tm.Survey,
                                    Join_Dt = hem.JoinDt,
                                    selectStatus = ts.SelectStatus,
                                    AttDate = ts.AttDate.HasValue ? ts.AttDate.Value.ToString(_employeeSettings.DateFormat) : _employeeSettings.NotAvailable,
                                    IsAttended = ts.Status == "N" ? "NE" : ApprovalStatus.Approved.ToString()// "A"
                                }).AsNoTracking().ToListAsync();

            return result;
        }
        public async Task<CareerHistoryResultDto> CareerHistoryAsync(int employeeId)
        {
            // Fetch previous experience data in a single query
            var previousExpDays = await _context.HrEmpProfdtls
                .Where(empProf => empProf.EmpId == employeeId)
                .SumAsync(empProf => (int?)EF.Functions.DateDiffDay(empProf.JoinDt, empProf.LeavingDt) ?? 0);

            var previousExp = new CareerHistoryDto
            {
                Category = "Previous Experience",
                Relevant = "0Y: 0M: 0D",
                NonRelevent = await GetEmployeeExperienceLengthAsync(employeeId, previousExpDays),
                Total = await GetEmployeeExperienceLengthAsync(employeeId, previousExpDays)
            };

            // Fetch company experience in a single query
            var relevantDays = await _context.HrEmpMasters
                .Where(empMaster => empMaster.EmpId == employeeId)
                .Select(empMaster => (int?)EF.Functions.DateDiffDay(empMaster.FirstEntryDate, DateTime.UtcNow) ?? 0)
                .FirstOrDefaultAsync();

            var companyExp = new CareerHistoryDto
            {
                Category = "Company Experience (First Entry Date)",
                Relevant = await GetEmployeeExperienceLengthAsync(employeeId, relevantDays),
                NonRelevent = "0Y: 0M: 0D",
                Total = await GetEmployeeExperienceLengthAsync(employeeId, relevantDays)
            };

            // Compute total summary
            var totalRelevantDays = relevantDays;
            var totalNonRelevantDays = previousExpDays;
            var totalDays = totalRelevantDays + totalNonRelevantDays;

            var totalSummary = new CareerHistoryDto
            {
                Category = "Total",
                Relevant = await GetEmployeeExperienceLengthAsync(employeeId, totalRelevantDays),
                NonRelevent = await GetEmployeeExperienceLengthAsync(employeeId, totalNonRelevantDays),
                Total = await GetEmployeeExperienceLengthAsync(employeeId, totalDays)
            };

            var careerHistoryList = new List<CareerHistoryDto> { totalSummary, previousExp, companyExp };

            //table 1
            var TransferTransition00 = _context.TransferTransition00s
                .Where(a => a.BatchApprovalStatus != _employeeSettings.Status01
                            && (a.EmpApprovalStatus ?? _employeeSettings.EmployeeStatus) != _employeeSettings.Status01
                            && a.EmployeeId == employeeId)
                .Select(a => new
                {
                    a.TransferBatchId,
                    a.TransferId,
                    a.EntityOrder,
                    a.ActionId,
                    a.EmployeeId,
                    a.OldEntityId,
                    a.OldEntityDescription,
                    a.NewEntityId,
                    a.NewEntityDescription,
                    a.FromDate,
                    a.ToDate,
                    a.BatchApprovalStatus,
                    a.EmpApprovalStatus
                })
                .ToList();

            var transferCareerHistory = TransferTransition00
                .Select(t => new CareerHistoryTransferModelDto
                {
                    TransferId = t.TransferId,
                    EmployeeId = t.EmployeeId,
                    FromDate = null,
                    ToDate = null,
                    Level1 = null,
                    Level2 = null,
                    Level3 = null,
                    Level4 = null,
                    Level5 = null,
                    Level6 = null,
                    Level7 = null,
                    Level8 = null,
                    Level9 = null,
                    Level10 = null,
                    Level11 = null
                })
                .Distinct()
                .ToList();

            var transitionDict = TransferTransition00
                .GroupBy(t => t.TransferId)
                .ToDictionary(g => g.Key, g => g.ToList());

            foreach (var history in transferCareerHistory)
            {
                if (transitionDict.TryGetValue(history.TransferId, out var transitions))
                {
                    foreach (var transition in transitions)
                    {
                        history.FromDate = transition.FromDate;
                        history.ToDate = transition.ToDate;

                        switch (transition.EntityOrder)
                        {
                            case 1: history.Level1 = transition.OldEntityDescription; break;
                            case 2: history.Level2 = transition.OldEntityDescription; break;
                            case 3: history.Level3 = transition.OldEntityDescription; break;
                            case 4: history.Level4 = transition.OldEntityDescription; break;
                            case 5: history.Level5 = transition.OldEntityDescription; break;
                            case 6: history.Level6 = transition.OldEntityDescription; break;
                            case 7: history.Level7 = transition.OldEntityDescription; break;
                            case 8: history.Level8 = transition.OldEntityDescription; break;
                            case 9: history.Level9 = transition.OldEntityDescription; break;
                            case 10: history.Level10 = transition.OldEntityDescription; break;
                            case 11: history.Level11 = transition.OldEntityDescription; break;
                        }
                    }
                }
            }
            var entityLimit = _context.LicensedCompanyDetails
                .Select(l => l.EntityLimit)
                .FirstOrDefault();

            var categoryMaster = _context.Categorymasters
                .Where(c => c.SortOrder >= 1 && c.SortOrder <= 11)
                .OrderBy(c => c.SortOrder)
                .ToList();

            var categoryLevels = _context.Categorymasters
                .Where(c => c.SortOrder >= 1 && c.SortOrder <= entityLimit)
                .OrderBy(c => c.SortOrder)
                .ToDictionary(c => c.SortOrder, c => c.Description);

            var careerHistoryList01 = transferCareerHistory
                .Select(t => new CareerHistoryDataDto
                {
                    FromDate = t.FromDate,
                    ToDate = t.ToDate,
                    LevelData = categoryLevels.ToDictionary(
                        level => level.Value,
                        level => level.Key switch
                            {
                                1 => t.Level1 ?? "",
                                2 => t.Level2 ?? "",
                                3 => t.Level3 ?? "",
                                4 => t.Level4 ?? "",
                                5 => t.Level5 ?? "",
                                6 => t.Level6 ?? "",
                                7 => t.Level7 ?? "",
                                8 => t.Level8 ?? "",
                                9 => t.Level9 ?? "",
                                10 => t.Level10 ?? "",
                                11 => t.Level11 ?? "",
                                _ => ""
                            })
                })
                .ToList();

            var finalResult = careerHistoryList01
                .Select(x =>
                {
                    var flatObject = new Dictionary<string, object>
                    {
            { "FromDate", x.FromDate },
            { "ToDate", x.ToDate }
                    };

                    foreach (var kvp in x.LevelData)
                    {
                        flatObject[kvp.Key] = kvp.Value;
                    }

                    return flatObject;
                })
                .ToList();

            return new CareerHistoryResultDto
            {
                Table = careerHistoryList,
                Table1 = finalResult
            };
        }
        public async Task<List<object>> BiometricDetailsAsync(int employeeId)
        {
            var result = await (from a in _context.BiometricsDtls
                                join b in _context.HrEmpMasters on a.EmployeeId equals b.EmpId into bio
                                from b in bio.DefaultIfEmpty()
                                where a.EmployeeId == employeeId
                                select new
                                {
                                    CompanyID = a.CompanyId.ToString(),
                                    EmployeeID = a.EmployeeId.ToString(),
                                    DeviceID = a.DeviceId.ToString(),
                                    UserID = a.UserId,
                                    AttMarkId = b != null && b.IsMarkAttn.HasValue
                                    ? (b.IsMarkAttn.Value ? "1" : "2") : ""
                                }).AsNoTracking().ToListAsync();
            return result.Cast<object>().ToList();
        }
        public async Task<object> AccessDetailsAsync(int employeeId)
        {
            var employee = await _context.HrEmpMasters
                .Where(z => z.EmpId == employeeId)
                .Select(z => new
                {
                    Holiday = _context.HrmHolidayMasterAccesses
                        .Where(a => a.EmployeeId == z.EmpId)
                        .OrderBy(x => x.IdHolidayMasterAccess)
                        .Select(x => (long?)x.IdHolidayMasterAccess)
                        .FirstOrDefault(),

                    Attendance = _context.AttendancepolicyMasterAccesses
                        .Where(b => b.EmployeeId == z.EmpId)
                        .OrderBy(x => x.AttendanceAccessId)
                        .Select(x => (long?)x.AttendanceAccessId)
                        .FirstOrDefault(),

                    Shift = _context.ShiftMasterAccesses
                        .Where(c => c.EmployeeId == z.EmpId)
                        .OrderBy(x => x.ShiftAccessId)
                        .Select(x => (long?)x.ShiftAccessId)
                        .FirstOrDefault(),

                    Leave = _context.HrmLeaveEmployeeleaveaccesses
                        .Where(d => d.EmployeeId == z.EmpId)
                        .OrderBy(x => x.IdEmployeeLeaveAccess)
                        .Select(x => (long?)x.IdEmployeeLeaveAccess)
                        .FirstOrDefault(),

                    LeavePolicy = _context.LeavepolicyMasterAccesses
                        .Where(e => e.EmployeeId == z.EmpId)
                        .OrderBy(x => x.LeaveAccessId)
                        .Select(x => (long?)x.LeaveAccessId)
                        .FirstOrDefault(),

                    LeaveBasicSettings = _context.HrmLeaveBasicsettingsaccesses
                        .Where(f => f.EmployeeId == z.EmpId)
                        .OrderBy(x => x.IdEmployeeSettinsAccess)
                        .Select(x => (long?)x.IdEmployeeSettinsAccess)
                        .FirstOrDefault()
                })
              .AsNoTracking().FirstOrDefaultAsync();

            return employee; // Return empty object if no employee found
        }
        public async Task<List<Fill_ModulesWorkFlowDto>> FillModulesWorkFlowAsync(int entityID, int linkId)
        {
            var excludedTransactionIds = _context.ParamWorkFlow02s
                .Where(pwf02 => entityID == 13 && pwf02.LinkEmpId == linkId && pwf02.LinkLevel == entityID)
                .Select(pwf02 => pwf02.TransactionId)
                .Union(
                    _context.ParamWorkFlow01s
                    .Where(pwf01 => entityID != 13 && pwf01.LinkId == linkId && pwf01.LinkLevel == entityID)
                    .Select(pwf01 => pwf01.TransactionId)
                );

            var result = await (
                from b in _context.ParamWorkFlow00s.AsNoTracking()
                join c in _context.ParamWorkFlowEntityLevel00s.AsNoTracking()
                    on b.ValueId equals c.ValueId
                join d in _context.TransactionMasters.AsNoTracking()
                    on b.TransactionId equals d.TransactionId
                where c.EntityLevel == entityID && !excludedTransactionIds.Any(e => e == b.TransactionId)
                select new Fill_ModulesWorkFlowDto
                {
                    ValueId = (int?)b.ValueId,
                    TransactionId = b.TransactionId,
                    Description = d.Description
                }
            ).ToListAsync();

            return result;
        }
        public async Task<List<Fill_WorkFlowMasterDto>> FillWorkFlowMasterAsync(int emp_Id, int roleId)
        {
            var transid = await _context.TransactionMasters
                .Where(t => t.TransactionType == "W_Flow")
                .Select(t => t.TransactionId)
                .FirstOrDefaultAsync();

            int? lnklev = await _context.SpecialAccessRights
                .Where(s => s.RoleId == roleId)
                .Select(s => s.LinkLevel)
                .FirstOrDefaultAsync();

            bool hasAccess = await _context.EntityAccessRights02s
                .AnyAsync(s => s.RoleId == roleId && s.LinkLevel == 15);

            if (hasAccess)
            {
                return await _context.WorkFlowDetails
                    .Where(w => (bool)w.IsActive)
                    .Select(w => new Fill_WorkFlowMasterDto
                    {
                        WorkFlowId = w.WorkFlowId,
                        Description = w.Description
                    })
                    .ToListAsync();
            }

            // **Step 1: Compute `ctnew`**
            var empEntity = await _context.HrEmpMasters
                .Where(h => h.EmpId == emp_Id)
                .Select(h => h.EmpEntity)
                .FirstOrDefaultAsync();

            var ctnew = SplitStrings_XML(empEntity, ',')
                .Select((item, index) => new LinkItemDto { Item = item, LinkLevel = index + 2 })
                .Where(c => !string.IsNullOrEmpty(c.Item))
                .ToList();

            // **Step 2: Compute `applicableFinal`**
            var applicableFinal = await _context.EntityAccessRights02s
                .Where(s => s.RoleId == roleId)
                .SelectMany(s => SplitStrings_XML(s.LinkId, default),
                    (s, item) => new LinkItemDto { Item = item, LinkLevel = s.LinkLevel })
                .ToListAsync();

            if (lnklev > 0)
            {
                applicableFinal.AddRange(
                    ctnew.Where(c => c.LinkLevel >= lnklev)
                         .Select(c => new LinkItemDto { Item = c.Item, LinkLevel = c.LinkLevel })
                );
            }

            // Convert `applicableFinal` to HashSet for fast lookup
            //var applicableFinalSet = applicableFinal.Select(a => a.Item).ToHashSet();
            var applicableFinalSetLong = applicableFinal.Select(a => (long?)Convert.ToInt64(a.Item)).ToHashSet();

            // **Step 3: Fetch `EntityApplicable00Final`**
            var entityApplicable00Final = await _context.EntityApplicable00s
                .Where(e => e.TransactionId == transid)
                .Select(e => new { e.LinkId, e.LinkLevel, e.MasterId })
                .ToListAsync();

            // **Step 4: Compute `applicableFinal02`**
            var applicableFinal02 = applicableFinal.ToList(); // Already computed

            // **Step 5: Compute `applicableFinal02Emp`**
            var applicableFinal02Emp = await (
                from emp in _context.EmployeeDetails
                join ea in _context.EntityApplicable01s on emp.EmpId equals ea.EmpId
                join hlv in _context.HighLevelViewTables on emp.LastEntity equals hlv.LastEntityId
                join af2 in applicableFinal02 on hlv.LevelOneId.ToString() equals af2.Item into af2LevelOne
                from af2L1 in af2LevelOne.DefaultIfEmpty()
                where ea.TransactionId == transid
                select ea.MasterId
            ).Distinct().ToListAsync();

            // **Step 6: Compute `newhigh`**
            var newhigh = entityApplicable00Final
                .Where(e => applicableFinalSetLong.Contains(e.LinkId) || e.LinkLevel == 15)
                .Select(e => e.MasterId)
                .Union(applicableFinal02Emp)
                .Distinct()
                .ToList();

            // **Step 7: Final WorkFlowDetails Query**
            return await _context.WorkFlowDetails
                .Where(wf => wf.IsActive == true && newhigh.Contains(wf.WorkFlowId))
                .Select(wf => new Fill_WorkFlowMasterDto
                {
                    WorkFlowId = wf.WorkFlowId,
                    Description = wf.Description
                })
                .ToListAsync();
        }
        public Task<List<BindWorkFlowMasterEmpDto>> BindWorkFlowMasterEmpAsync(int linkId, int linkLevel)
        {

            if (linkLevel == 13)
            {
                // If LinkLevel is 13, select from ParamWorkFlow02 and use LinkEmpId
                var query = from a in _context.ParamWorkFlow02s
                            join b in _context.TransactionMasters on a.TransactionId equals b.TransactionId
                            join c in _context.WorkFlowDetails on a.WorkFlowId equals c.WorkFlowId
                            where a.LinkEmpId == linkId
                            select new BindWorkFlowMasterEmpDto
                            {
                                ValueId = (int?)a.ValueId,
                                TDescription = b.Description,
                                Description = c.Description,
                                FinalRuleName = c.FinalRuleName
                            };
                return query.AsNoTracking().ToListAsync();
            }
            else
            {
                // Otherwise, select from ParamWorkFlow01 and use LinkId
                var query = from a in _context.ParamWorkFlow01s
                            join b in _context.TransactionMasters on a.TransactionId equals b.TransactionId
                            join c in _context.WorkFlowDetails on a.WorkFlowId equals c.WorkFlowId
                            where a.LinkId == linkId
                            select new BindWorkFlowMasterEmpDto
                            {
                                ValueId = (int?)a.ValueId,
                                TDescription = b.Description,
                                Description = c.Description,
                                FinalRuleName = c.FinalRuleName
                            };
                return query.AsNoTracking().ToListAsync();
            }

        }
        public async Task<List<GetRejoinReportDto>> GetRejoinReportAsync(int employeeId)
        {
            return await (from a in _context.Resignations
                          join b in _context.EmployeeDetails on a.EmpId equals b.EmpId
                          join c in _context.ReasonMasters on Convert.ToInt32(a.Reason) equals c.ReasonId
                          join d in _context.HrEmpStatusSettings on Convert.ToInt32(a.RelievingType) equals d.StatusId
                          where a.ApprovalStatus == "RJ" && a.EmpId == employeeId
                          select new GetRejoinReportDto
                          {
                              Emp_Id = a.EmpId,
                              Emp_Code = b.EmpCode,
                              Name = b.Name,
                              Resignation_Id = (int?)a.ResignationId,
                              Resignation_Request_Id = a.ResignationRequestId,
                              Request_Date = a.RequestDate.HasValue ? a.RequestDate.Value.ToString(_employeeSettings.DateFormat) : null,
                              Resignation_Date = a.ResignationDate.HasValue ? a.ResignationDate.Value.ToString(_employeeSettings.DateFormat) : null,
                              RelievingDate = a.RelievingDate.HasValue ? a.RelievingDate.Value.ToString(_employeeSettings.DateFormat) : null,
                              Remarks = a.Remarks,
                              RejoinRequestID = a.RejoinRequestId,
                              RejoinRequestDate = a.RejoinRequestDate.HasValue ? a.RejoinRequestDate.Value.ToString(_employeeSettings.DateFormat) : null,
                              RejoinApprovalDate = a.RejoinApprovalDate.HasValue ? a.RejoinApprovalDate.Value.ToString(_employeeSettings.DateFormat) : null,
                              RejoinRemarks = a.RejoinRemarks
                          }).AsNoTracking().ToListAsync();
        }
        public async Task<int?> GetLastEntity(int? empId)
        {
            return await _context.HrEmpMasters
                  .Where(e => e.EmpId == empId)
                  .Select(e => e.LastEntity)
                  .FirstOrDefaultAsync();
        }
        public async Task<List<TransferAndPromotionDto>> TransferAndPromotionAsync(int employeeId)
        {
            var result = await (from c in _context.PositionHistories
                                join e in _context.HighLevelViewTables on c.LastEntity equals e.LastEntityId
                                join em in _context.EmployeeDetails on c.EmpId equals em.EmpId into empJoin
                                from em in empJoin.DefaultIfEmpty()
                                where c.EmpId == employeeId
                                orderby c.ToDate descending
                                select new TransferAndPromotionDto
                                {
                                    BandID = (int)em.BandId,
                                    BranchID = (int)em.BranchId,
                                    CurrentStatus = (int)em.CurrentStatus,
                                    DateOfBirth = (DateTime)em.DateOfBirth,
                                    DepId = (int)em.DepId,
                                    DesigId = (int)em.DesigId,
                                    Emp_Code = em.EmpCode,
                                    Emp_Id = em.EmpId,
                                    Emp_status = (int)em.EmpStatus,
                                    EmpEntity = em.EmpEntity,
                                    EmpFirstEntity = em.EmpFirstEntity,
                                    Gender = em.Gender,
                                    GradeID = em.GradeId,
                                    Guardians_Name = em.GuardiansName,
                                    Inst_Id = em.InstId,
                                    Join_Dt = em.JoinDt != null ? em.JoinDt.Value.ToString(_employeeSettings.DateFormat) : null,
                                    LastEntity = em.LastEntity,
                                    Name = em.Name,
                                    Probation_Dt = em.ProbationDt,
                                    Position_Id = c.PositionId,
                                    From_Date = c.FromDate != null ? c.FromDate.Value.ToString(_employeeSettings.DateFormat) : null,
                                    To_Date = c.ToDate != null ? c.ToDate.Value.ToString(_employeeSettings.DateFormat) : null,
                                    Status = c.Status,
                                    LevelOneDescription = e.LevelOneDescription,
                                    LevelOneId = e.LevelOneId,
                                    LevelTwoDescription = e.LevelTwoDescription,
                                    LevelTwoId = e.LevelTwoId,
                                    LevelThreeDescription = e.LevelThreeDescription,
                                    LevelThreeId = e.LevelThreeId,
                                    LevelFourDescription = e.LevelFourDescription,
                                    LevelFourId = e.LevelFourId,
                                    LevelFiveDescription = e.LevelFiveDescription,
                                    LevelFiveId = e.LevelFiveId,
                                    LevelSixDescription = e.LevelSixDescription,
                                    LevelSixId = e.LevelSixId,
                                    LevelSevenDescription = e.LevelSevenDescription,
                                    LevelSevenId = e.LevelSevenId,
                                    LevelEightDescription = e.LevelEightDescription,
                                    LevelEightId = e.LevelEightId,
                                    LevelNineDescription = e.LevelNineDescription,
                                    LevelNineId = e.LevelNineId,
                                    LevelTenDescription = e.LevelTenDescription,
                                    LevelTenId = e.LevelTenId,
                                    LevelElevenDescription = e.LevelElevenDescription,
                                    LevelElevenId = e.LevelElevenId,
                                    LevelTwelveDescription = e.LevelTwelveDescription,
                                    LevelTwelveId = e.LevelTwelveId,
                                    LastEntityID = e.LastEntityId
                                }).AsNoTracking().ToListAsync();
            return result;
        }
        public async Task<List<GetEmpReportingReportDto>> GetEmpReportingReportAsync(int employeeId)
        {
            var query = await (from a in _context.HrEmpReportingHstries
                               join b in _context.EmployeeDetails on a.EmpId equals b.EmpId
                               join d in _context.EmployeeDetails on a.ReportingTo equals d.EmpId
                               where a.EmpId == employeeId
                               select new GetEmpReportingReportDto
                               {
                                   Emp_Code = b.EmpCode,
                                   Name = b.Name,
                                   ReportingEmpCode = d.EmpCode,
                                   ReporteeName = d.Name,
                                   FromDate = a.EntryDate.HasValue ? a.EntryDate.Value.ToString(_employeeSettings.DateFormat) : null,
                                   ToDate = a.UpdatedDate.HasValue ? a.UpdatedDate.Value.ToString(_employeeSettings.DateFormat) : null
                               }).AsNoTracking().ToListAsync();

            return query;
        }
        public async Task<List<GetEmpWorkFlowRoleDetailstDto>> GetEmpWorkFlowRoleDetailsAsync(int linkId, int linkLevel)
        {
            var query = (linkLevel == 13)
                ? from a in _context.ParamRole02s
                  join b in _context.Categorymasterparameters on a.ParameterId equals b.ParameterId into bGroup
                  from b in bGroup.DefaultIfEmpty()
                  join c in _context.EmployeeDetails on a.EmpId equals c.EmpId into cGroup
                  from c in cGroup.DefaultIfEmpty()
                  where a.LinkEmpId == linkId && b.ShowRoleInEmployeeTab == 1
                  select new
                  {
                      a.ValueId,
                      b.ParamDescription,
                      EmployeeName = c.Name // No null check in query
                  }
                : from a in _context.ParamRole01s
                  join b in _context.Categorymasterparameters on a.ParameterId equals b.ParameterId into bGroup
                  from b in bGroup.DefaultIfEmpty()
                  join c in _context.EmployeeDetails on a.EmpId equals c.EmpId into cGroup
                  from c in cGroup.DefaultIfEmpty()
                  where a.LinkId == linkId && b.ShowRoleInEmployeeTab == 1
                  select new
                  {
                      a.ValueId,
                      b.ParamDescription,
                      EmployeeName = c.Name // No null check in query
                  };

            var result = await query.AsNoTracking().ToListAsync();

            // Apply null check AFTER the query execution
            return result.Select(r => new GetEmpWorkFlowRoleDetailstDto
            {
                ValueId = r.ValueId,
                ParamDescription = r.ParamDescription,
                EmployeeName = r.EmployeeName ?? "Not Assigned" // Handle nulls in memory
            }).ToList();




        }
        public async Task<List<Dictionary<string, object>>> SalarySeriesAsync1(int employeeId, string status)
        {
            // 1. Get distinct pay components (earnings + deductions)
            var payComponents = await (from a in _context.PayscaleRequest01s
                                       join b in _context.PayscaleRequest02s on a.PayRequest01Id equals b.PayRequestId01
                                       join payCode in _context.PayCodeMaster01s on b.PayComponentId equals payCode.PayCodeId
                                       where a.EmployeeId == employeeId
                                       select new
                                       {
                                           b.PayType,
                                           PayCodeDescription = payCode.PayCodeDescription.Trim()
                                       }).Distinct().AsNoTracking().ToListAsync();

            var earningsDescriptions = payComponents
                .Where(x => x.PayType == 1)
                .Select(x => x.PayCodeDescription)
                .Distinct()
                .ToList();

            var deductionDescriptions = payComponents
                .Where(x => x.PayType == 2)
                .Select(x => x.PayCodeDescription)
                .Distinct()
                .ToList();

            var allDescriptions = earningsDescriptions.Concat(deductionDescriptions).Distinct().ToList();

            // 2. Pivot actual pay component amounts
            var pivoted = await (from pr1 in _context.PayscaleRequest01s.AsNoTracking()
                                 join pr2 in _context.PayscaleRequest02s.AsNoTracking() on pr1.PayRequest01Id equals pr2.PayRequestId01
                                 join pcm in _context.PayCodeMaster01s.AsNoTracking() on pr2.PayComponentId equals pcm.PayCodeId
                                 where pr1.EmployeeId == employeeId &&
                                       (pr2.PayType == 1 || pr2.PayType == 2) &&
                                       pr1.EmployeeStatus == status
                                 group pr2 by new { pr1.PayRequest01Id, PayCodeDescription = pcm.PayCodeDescription.Trim() } into g
                                 select new
                                 {
                                     g.Key.PayRequest01Id,
                                     g.Key.PayCodeDescription,
                                     Amount = (decimal?)g.Sum(x => x.Amount) ?? 0
                                 }).ToListAsync();

            // 3. Convert pivot to dictionary for quick lookup
            var pivotedDict = pivoted
                .GroupBy(x => x.PayRequest01Id)
                .ToDictionary(
                    g => g.Key,
                    g => g.ToDictionary(k => k.PayCodeDescription, v => v.Amount)
                );

            // 4. Main data query
            var baseResult = await (
                from a in _context.PayscaleRequest00s.AsNoTracking()
                join b in _context.PayscaleRequest01s.AsNoTracking() on a.PayRequestId equals b.PayRequestId
                join c in _context.EmployeeDetails.AsNoTracking() on b.EmployeeId equals c.EmpId
                join br in _context.BranchDetails.AsNoTracking() on c.BranchId equals br.LinkId
                join d in _context.CurrencyMasters.AsNoTracking() on a.CurrencyId equals d.CurrencyId
                join e in _context.PayscaleRequest02s.AsNoTracking() on b.PayRequest01Id equals e.PayRequestId01
                join f in _context.PayCodeMaster01s.AsNoTracking() on e.PayComponentId equals f.PayCodeId
                where b.EmployeeId == employeeId && b.EmployeeStatus == status
                orderby b.EffectiveDate ascending
                select new
                {
                    c.EmpId,
                    a.PayRequestId,
                    b.PayRequest01Id,
                    c.EmpCode,
                    c.Name,
                    Branch = br.Branch,
                    b.TotalEarnings,
                    b.TotalDeductions,
                    b.TotalPay,
                    b.EffectiveDate,
                    d.CurrencyCode,
                    IsArrears = (a.Type == 4),
                    PaycodeBatch = (from elpb in _context.EmployeeLatestPayrollBatches.AsNoTracking()
                                    join pcm in _context.PayCodeMaster00s.AsNoTracking() on elpb.PayrollBatchId equals pcm.PayCodeMasterId
                                    where elpb.EmployeeId == employeeId
                                    orderby elpb.EntryDate descending
                                    select pcm.Description).FirstOrDefault() ?? _employeeSettings.NotAvailable,
                    PayPeriod = (from elpp in _context.EmployeeLatestPayrollPeriods.AsNoTracking()
                                 join p in _context.Payroll00s.AsNoTracking() on elpp.PayrollPeriodId equals p.PayrollPeriodId
                                 where elpp.EmployeeId == employeeId
                                 orderby elpp.EntryDate descending
                                 select p.Description).FirstOrDefault() ?? _employeeSettings.NotAvailable,
                    Remarks = b.PayscaleEmpRemarks
                }).Distinct().ToListAsync();

            // 5. Compose final flattened result with all pay components
            var finalResult = baseResult.Select(row =>
            {
                var dict = new Dictionary<string, object>
                {
                    ["empId"] = row.EmpId,
                    ["payRequestId"] = row.PayRequestId,
                    ["payRequest01Id"] = row.PayRequest01Id,
                    ["employeeCode"] = row.EmpCode,
                    ["employeeName"] = row.Name,
                    ["branch"] = row.Branch,
                    ["totalEarnings"] = row.TotalEarnings,
                    ["totalDeductions"] = row.TotalDeductions,
                    ["totalPay"] = row.TotalPay,
                    ["effectiveDate"] = row.EffectiveDate?.ToString(_employeeSettings.DateFormat),
                    ["currencyCode"] = row.CurrencyCode,
                    ["isArrears"] = row.IsArrears,
                    ["paycodeBatch"] = row.PaycodeBatch,
                    ["payPeriod"] = row.PayPeriod,
                    ["remarks"] = row.Remarks
                };

                // Add pay components, even if not present
                if (pivotedDict.TryGetValue(row.PayRequest01Id, out var payComponents))
                {
                    foreach (var desc in allDescriptions)
                    {
                        dict[desc] = payComponents.ContainsKey(desc) ? payComponents[desc] : 0m;
                    }
                }
                else
                {
                    foreach (var desc in allDescriptions)
                    {
                        dict[desc] = 0m;
                    }
                }

                return dict;
            }).ToList();

            return finalResult;
        }



        //public async Task<List<SalarySeriesDto>> SalarySeriesAsync(int employeeId, string status)
        //{
        //    var payComponents = await (from a in _context.PayscaleRequest01s
        //                               join b in _context.PayscaleRequest02s on a.PayRequest01Id equals b.PayRequestId01
        //                               join payCode in _context.PayCodeMaster01s on b.PayComponentId equals payCode.PayCodeId
        //                               where a.EmployeeId == employeeId
        //                               select new
        //                               {
        //                                   b.PayType,
        //                                   payCode.PayCodeDescription
        //                               }).Distinct().AsNoTracking().ToListAsync();

        //    var earningsDescriptions = payComponents
        //        .Where(x => x.PayType == 1)
        //        .Select(x => x.PayCodeDescription)
        //        .Distinct()
        //        .ToList();
        //    var deductionDescriptions = payComponents
        //        .Where(x => x.PayType == 2)
        //        .Select(x => x.PayCodeDescription)
        //        .Distinct()
        //        .ToList();
        //    var allDescriptions = earningsDescriptions.Concat(deductionDescriptions).Distinct().ToList();


        //    var pivoted = await (from pr1 in _context.PayscaleRequest01s
        //                         join pr2 in _context.PayscaleRequest02s on pr1.PayRequest01Id equals pr2.PayRequestId01
        //                         join pcm in _context.PayCodeMaster01s on pr2.PayComponentId equals pcm.PayCodeId
        //                         where pr1.EmployeeId == employeeId &&
        //                               (pr2.PayType == 1 || pr2.PayType == 2) &&
        //                               pr1.EmployeeStatus == status
        //                         group pr2 by new { pr1.PayRequest01Id, pcm.PayCodeDescription } into g
        //                         select new
        //                         {
        //                             g.Key.PayRequest01Id,
        //                             g.Key.PayCodeDescription,
        //                             Amount = (decimal?)g.Sum(x => x.Amount) ?? 0
        //                         })
        //                .ToListAsync();

        //    var result1 = pivoted
        //.GroupBy(x => x.PayRequest01Id)
        //.Select(g => new PayComponentPivotDto
        //{
        //    PayRequestId01 = (int)g.Key,
        //    PayCodeAmounts = earningsDescriptions.ToDictionary(
        //        desc => desc,
        //        desc => g.FirstOrDefault(x => x.PayCodeDescription == desc)?.Amount ?? 0
        //    )
        //})
        //.ToList();



        //    var pivotedDict = result1.ToDictionary(pe => pe.PayRequestId01, pe => pe.PayCodeAmounts);

        //    var result = await (from a in _context.PayscaleRequest00s
        //                        join b in _context.PayscaleRequest01s on a.PayRequestId equals b.PayRequestId
        //                        join c in _context.EmployeeDetails on b.EmployeeId equals c.EmpId
        //                        join br in _context.BranchDetails on c.BranchId equals br.LinkId
        //                        join d in _context.CurrencyMasters on a.CurrencyId equals d.CurrencyId
        //                        join e in _context.PayscaleRequest02s on b.PayRequest01Id equals e.PayRequestId01
        //                        join f in _context.PayCodeMaster01s on e.PayComponentId equals f.PayCodeId
        //                        where b.EmployeeId == employeeId && b.EmployeeStatus == status
        //                        orderby b.EffectiveDate ascending
        //                        select new SalarySeriesDto
        //                        {
        //                            EmpId = c.EmpId,
        //                            PayRequestId = (int)a.PayRequestId,
        //                            PayRequest01Id = (int)b.PayRequest01Id,
        //                            EmployeeCode = c.EmpCode,
        //                            EmployeeName = c.Name,
        //                            Branch = br.Branch,
        //                            TotalEarnings = (decimal)b.TotalEarnings,
        //                            TotalDeductions = (decimal)b.TotalDeductions,
        //                            TotalPay = (decimal)b.TotalPay,
        //                            EffectiveDate = b.EffectiveDate != null ? b.EffectiveDate.Value.ToString(_employeeSettings.DateFormat) : null,
        //                            CurrencyCode = d.CurrencyCode,
        //                            IsArrears = (a.Type == 4) ? true : false,
        //                            PaycodeBatch = (from elpb in _context.EmployeeLatestPayrollBatches
        //                                            join pcm in _context.PayCodeMaster00s on elpb.PayrollBatchId equals pcm.PayCodeMasterId
        //                                            where elpb.EmployeeId == employeeId
        //                                            orderby elpb.EntryDate descending
        //                                            select pcm.Description).FirstOrDefault() ?? _employeeSettings.NotAvailable,
        //                            PayPeriod = (from elpp in _context.EmployeeLatestPayrollPeriods
        //                                         join p in _context.Payroll00s on elpp.PayrollPeriodId equals p.PayrollPeriodId
        //                                         where elpp.EmployeeId == employeeId
        //                                         orderby elpp.EntryDate descending
        //                                         select p.Description).FirstOrDefault() ?? _employeeSettings.NotAvailable,
        //                            PayComponent = pivotedDict.ContainsKey((int)b.PayRequest01Id) ? pivotedDict[(int)b.PayRequest01Id] : new Dictionary<string, decimal>(),

        //                            Remarks = b.PayscaleEmpRemarks
        //                        }).Distinct().ToListAsync();
        //    return result;

        //}
        public async Task<List<FillEmpWorkFlowRoleDto>> FillEmpWorkFlowRoleAsync(int entityID)
        {
            return await (from b in _context.ParamRole00s
                          join c in _context.ParamRoleEntityLevel00s on b.ValueId equals c.ValueId
                          join d in _context.Categorymasterparameters on b.ParameterId equals d.ParameterId
                          where c.EntityLevel == entityID && d.ShowRoleInEmployeeTab == 1
                          select new FillEmpWorkFlowRoleDto
                          {
                              ValueId = b.ValueId,
                              ParameterId = b.ParameterId,
                              ParamDescription = d.ParamDescription
                          }).AsNoTracking().ToListAsync();


        }
        public async Task<List<EmployeeHraDto>> HraDetailsAsync(int employeeId)
        {
            var hraHistory = await (from a in _context.HraHistories
                                    join b in _context.EmployeeDetails on a.EmployeeId equals b.EmpId
                                    where a.EmployeeId == employeeId && (a.Initial ?? 0) == 0
                                    select new HraDetailsDto
                                    {
                                        EmpCode = b.EmpCode,
                                        Name = b.Name,
                                        IsHRA = a.IsHra ?? false,
                                        HRAStatus = (a.IsHra ?? false) ? "Enabled" : "Disabled",
                                        FromDate = a.FromDate.HasValue ? a.FromDate.Value.ToString(_employeeSettings.DateFormat) : "",
                                        ToDate = a.ToDate.HasValue ? a.ToDate.Value.ToString(_employeeSettings.DateFormat) : "",
                                        Remarks = a.Remarks ?? ""
                                    }).AsNoTracking().ToListAsync();

            var isHRA = await _context.HrEmpMasters
                .Where(e => e.EmpId == employeeId)
                .Select(e => e.Ishra)
                .FirstOrDefaultAsync();

            return new List<EmployeeHraDto>
            {
                new EmployeeHraDto
                {
                    HraHistory = hraHistory,
                    IsHRA = isHRA
                }
            };

        }
        private IQueryable<AuditInformationSubDto> GetAuditInformation(string employeeIDs, string infoCode, string informationLabel)
        {
            var employeeIdss = employeeIDs?.Split(',').Select(id => id.Trim()).ToList() ?? new List<string>();
            return from a in _context.EditInfoHistories
                   where employeeIdss.Contains(a.EmpId.ToString()) && a.InfoCode == infoCode
                   select new AuditInformationSubDto
                   {
                       InfoID = (int)a.InfoId,
                       Info01ID = a.Info01Id,
                       EmpID = (int)a.EmpId,
                       Information = informationLabel,
                       NewValue = a.Value ?? "",
                       OldValue = a.OldValue ?? "",
                       UpdatedBy = a.UpdatedBy,
                       UpdatedDate = a.UpdatedDate
                   };
        }
        private IQueryable<AuditInformationSubDto> GetCountryAuditInformation(string employeeIDs, string infoCode, string informationLabel)
        {
            var employeeIdss = employeeIDs?.Split(',').Select(id => id.Trim()).ToList() ?? new List<string>();

            return from a in _context.EditInfoHistories
                   join b in _context.AdmCountryMasters on EF.Functions.Collate(a.Value, "SQL_Latin1_General_CP1_CI_AS") equals b.CountryId.ToString()
                   join c in _context.AdmCountryMasters on EF.Functions.Collate(a.OldValue, "SQL_Latin1_General_CP1_CI_AS") equals c.CountryId.ToString()
                   where employeeIdss.Contains(a.EmpId.ToString()) && a.InfoCode == infoCode
                   select new AuditInformationSubDto
                   {
                       InfoID = (int)a.InfoId,
                       Info01ID = a.Info01Id,
                       EmpID = (int)a.EmpId,
                       Information = informationLabel,
                       NewValue = a.Value ?? "",
                       OldValue = a.OldValue ?? "",
                       UpdatedBy = a.UpdatedBy,
                       UpdatedDate = a.UpdatedDate
                   };
        }
        public async Task<List<AuditInformationDto>> AuditInformationAsync(string employeeIDs, int empId, int roleId, string? infotype, string? infoDesc, string? datefrom, string? dateto)
        {
            infoDesc = string.IsNullOrEmpty(infoDesc) ? _employeeSettings.empSystemStatus : infoDesc;
            infotype = string.IsNullOrEmpty(infotype) ? _employeeSettings.empSystemStatus : infotype;
            var employeeIdss = employeeIDs?.Split(',').Select(id => id.Trim()).ToList() ?? new List<string>();
            if (string.IsNullOrEmpty(datefrom))
            {
                var joinDate = await _context.HrEmpMasters.Where(emp => employeeIdss.Contains(emp.EmpId.ToString())).Select(emp => emp.JoinDt).FirstOrDefaultAsync();

                var dateFrom = joinDate?.ToString("yyyy-MM-dd");
            }
            var empDob = await GetAuditInformation(employeeIDs, "EMPDOB", "Date Of Birth").AsNoTracking().ToListAsync();
            var joiningDate = await GetAuditInformation(employeeIDs, "JOINDATE", "Joining Date").AsNoTracking().ToListAsync();
            var reviewDate = await GetAuditInformation(employeeIDs, "REVIEWDATE", "Review Date").AsNoTracking().ToListAsync();
            var GratuityDate = await GetAuditInformation(employeeIDs, "GRATUITYDATE", "Gratuity Start Date").AsNoTracking().ToListAsync();
            var EntryDate = await GetAuditInformation(employeeIDs, "FIRSTENTRYDATE", "First Entry Date").AsNoTracking().ToListAsync();
            var EmpCode = await GetAuditInformation(employeeIDs, "EMPCODE", "Employee Code").AsNoTracking().ToListAsync();
            var Country = await GetCountryAuditInformation(employeeIDs, "EMPCOUNTRY", "Country").AsNoTracking().ToListAsync();
            var FirstName = await GetAuditInformation(employeeIDs, "FIRSTNAME", "First Name").AsNoTracking().ToListAsync();
            var MiddleName = await GetAuditInformation(employeeIDs, "MIDDLENAME", "Middle Name").AsNoTracking().ToListAsync();
            var LastName = await GetAuditInformation(employeeIDs, "LASTNAME", "Last Name").AsNoTracking().ToListAsync();
            var GuardiansName = await GetAuditInformation(employeeIDs, "GUARDIANSNAME", "Guardians Name").AsNoTracking().ToListAsync();
            var Gender = await GetAuditInformation(employeeIDs, "GENDER", "Gender").AsNoTracking().ToListAsync();
            var maritalStatus = await GetAuditInformation(employeeIDs, "MARITAL", "Marital Status").AsNoTracking().ToListAsync();
            var NationalId = await GetAuditInformation(employeeIDs, "NATIONALID", "National ID").AsNoTracking().ToListAsync();
            var Passport = await GetAuditInformation(employeeIDs, "PASSPORTID", "Passport ID").AsNoTracking().ToListAsync();
            var CompanyEmail = await GetAuditInformation(employeeIDs, "COMPANYMAIL", "Company Email").AsNoTracking().ToListAsync();
            var PersonalEmail = await GetAuditInformation(employeeIDs, "PERSONALMAIL", "Personal Email").AsNoTracking().ToListAsync();
            var Mobile = await GetAuditInformation(employeeIDs, "PERSONALMOBILE", "Personal Mobile").AsNoTracking().ToListAsync();
            var HomeNo = await GetAuditInformation(employeeIDs, "HOMECOUNTRYPHONENO", "Home Country Phone Number").AsNoTracking().ToListAsync();
            var Nationality = await GetCountryAuditInformation(employeeIDs, "NATIONALITY", "Nationality").AsNoTracking().ToListAsync();
            var CountryName = await GetCountryAuditInformation(employeeIDs, "COUNTRYOFBIRTH", "Country of Birth").AsNoTracking().ToListAsync();
            var BloodGroup = await GetAuditInformation(employeeIDs, "BLOODGROUP", "Blood Group").AsNoTracking().ToListAsync();


            var Religion = await (
             from a in _context.EditInfoHistories
             join b in _context.AdmReligionMasters on Convert.ToInt32(a.Value) equals b.ReligionId
             join c in _context.AdmReligionMasters on Convert.ToInt32(a.OldValue) equals c.ReligionId
             where employeeIdss.Contains(a.EmpId.ToString()) && a.InfoCode == "RELIGION"
             select new AuditInformationSubDto
             {
                 InfoID = (int)a.InfoId,
                 Info01ID = a.Info01Id,
                 EmpID = (int)a.EmpId,
                 Information = "Religion",
                 NewValue = a.Value == null ? "" : b.ReligionName,
                 OldValue = a.OldValue == null ? "" : c.ReligionName,
                 UpdatedBy = a.UpdatedBy,
                 UpdatedDate = a.UpdatedDate
             }).AsNoTracking().ToListAsync();

            var Height = await GetAuditInformation(employeeIDs, "HEIGHT", "Height").AsNoTracking().ToListAsync();
            var Weight = await GetAuditInformation(employeeIDs, "WEIGHT", "Weight").AsNoTracking().ToListAsync();


            var Identification = await GetAuditInformation(employeeIDs, "IDENTIFICATION", "Identification Mark").AsNoTracking().ToListAsync();
            var NoticePeriod = await GetAuditInformation(employeeIDs, "NOTICEPERIOD", "Notice Period").AsNoTracking().ToListAsync();
            var AppNeeded = await GetAuditInformation(employeeIDs, "APPNEEDED", "Is Mobile App Needed").AsNoTracking().ToListAsync();
            var Accomodation = await GetAuditInformation(employeeIDs, "COMPANYACCOMODATION", "Staying in Company Accomodation").AsNoTracking().ToListAsync();
            var Expatriate = await GetAuditInformation(employeeIDs, "EXPATRIATE", "Is Expatriate").AsNoTracking().ToListAsync();
            var CasualHoliday = await GetAuditInformation(employeeIDs, "CASUALHOLYDAY", "Enable Casual Holiday Leave").AsNoTracking().ToListAsync();
            var Conveyance = await GetAuditInformation(employeeIDs, "COMPANYCONVEYANCE", "Company Conveyance").AsNoTracking().ToListAsync();
            var Vehicle = await GetAuditInformation(employeeIDs, "COMPANYVEHICLE", "Company Vehicle").AsNoTracking().ToListAsync();
            var ProbationNoticePeriod = await GetAuditInformation(employeeIDs, "PROBATIONNOTICEPERIOD", "Probation Notice Period").AsNoTracking().ToListAsync();
            var IsProbation = await GetAuditInformation(employeeIDs, "ISPROBATION", "Is Probation").AsNoTracking().ToListAsync();
            var ProbationStartDate = await GetAuditInformation(employeeIDs, _employeeSettings.ProbationEndDate, "Probation Start Date").AsNoTracking().ToListAsync();
            var MealAllowanceDeduction = await GetAuditInformation(employeeIDs, "MEALALLOWANCEDEDUCTION", "Meal Allowance Deduction").AsNoTracking().ToListAsync();


            var EmployeeReporting = await (
                 from a in _context.EditInfoHistories
                 join b in _context.EmployeeDetails on Convert.ToInt32(a.Value) equals b.EmpId
                 join c in _context.EmployeeDetails on Convert.ToInt32(a.OldValue) equals c.EmpId
                 where employeeIdss.Contains(a.EmpId.ToString()) && a.InfoCode == "EMPREPORTING"
                 select new AuditInformationSubDto
                 {
                     InfoID = (int)a.InfoId,
                     Info01ID = a.Info01Id,
                     EmpID = (int)a.EmpId,
                     Information = "Employee Reporting",
                     NewValue = b.Name,
                     OldValue = c.Name,
                     UpdatedBy = a.UpdatedBy,
                     UpdatedDate = a.UpdatedDate
                 }).AsNoTracking().ToListAsync();



            var Payscale = await (from a in _context.Payscale00s
                                  join b in _context.EmployeeDetails on a.EmployeeId equals b.EmpId
                                  join f in _context.PayscaleRequest00s on a.PayRequestId equals f.PayRequestId
                                  join j in _context.EditInfoMaster01s on 1 equals 1
                                  where j.Description == "Employee Payscale"
                                        && employeeIdss.Contains(b.EmpId.ToString())
                                  select new AuditInformationSubDto
                                  {
                                      InfoID = (int)j.InfoId,
                                      Info01ID = j.Info01Id,
                                      EmpID = b.EmpId,
                                      Information = "Employee Payscale",
                                      NewValue = a.EffectiveTo == null ? a.TotalEarnings.ToString() : "",
                                      OldValue = a.EffectiveTo != null ? a.TotalEarnings.ToString() : "",
                                      UpdatedBy = f.EntryBy,
                                      UpdatedDate = a.EffectiveFrom
                                  }).AsNoTracking().ToListAsync();
            var EmployeeShift = await (from a in _context.ShiftMasterAccesses
                                       join b in _context.EmployeeDetails on a.EmployeeId equals b.EmpId
                                       join c in _context.HrShift00s on a.ShiftId equals c.ShiftId
                                       from d in _context.EditInfoMaster01s.Where(d => d.Description == "Employee Shift") // Equivalent to CROSS JOIN
                                       where employeeIdss.Contains(b.EmpId.ToString())
                                       select new AuditInformationSubDto
                                       {
                                           InfoID = (int)d.InfoId,
                                           Info01ID = d.Info01Id,
                                           EmpID = b.EmpId,
                                           Information = "Employee Shift",
                                           NewValue = a.ValidDateTo == null ? c.ShiftName : "",
                                           OldValue = a.ValidDateTo != null ? c.ShiftName : "",
                                           UpdatedBy = a.CreatedBy,
                                           UpdatedDate = a.ValidDatefrom
                                       }).AsNoTracking().ToListAsync();




            var query = (empDob).Concat(joiningDate).Concat(reviewDate).Concat(GratuityDate).Concat(EntryDate).Concat(EmpCode).Concat(Country).Concat(FirstName).Concat(MiddleName).Concat(LastName).Concat(GuardiansName).Concat(Gender).Concat(maritalStatus).Concat(NationalId).Concat(Passport).Concat(CompanyEmail).Concat(PersonalEmail).Concat(Mobile).Concat(HomeNo).Concat(Nationality).Concat(CountryName).Concat(BloodGroup).Concat(Religion).Concat(Height).Concat(Weight).Concat(Identification).Concat(NoticePeriod).Concat(AppNeeded).Concat(Accomodation).Concat(Expatriate).Concat(CasualHoliday).Concat(Conveyance).Concat(Vehicle).Concat(ProbationNoticePeriod).Concat(IsProbation).Concat(ProbationStartDate).Concat(MealAllowanceDeduction).Concat(EmployeeReporting).Concat(Payscale).Concat(EmployeeShift);


            DateTime? startDate = null;
            DateTime? endDate = null;

            // Use HashSet for faster lookups
            var infoDescs = new HashSet<int>(infoDesc.Split(',')
                .Where(id => !string.IsNullOrWhiteSpace(id))
                .Select(id => int.TryParse(id, out int parsedId) ? parsedId : 0)
                .Where(id => id != 0)); // Remove default 0 values

            var infotypes = new HashSet<int>(infotype.Split(',')
                .Where(id => !string.IsNullOrWhiteSpace(id))
                .Select(id => int.TryParse(id, out int parsedId) ? parsedId : 0)
                .Where(id => id != 0));

            var parsedEmployeeIds = new HashSet<int>(employeeIDs?.Split(',')
                .Where(id => !string.IsNullOrWhiteSpace(id))
                .Select(id => int.Parse(id.Trim())) ?? Enumerable.Empty<int>());

            int.TryParse(infoDesc, out int parsedInfoDesc);
            int.TryParse(infotype, out int parsedInfotype);

            var result = (
                from a in query
                join b in _context.EmployeeDetails on a.EmpID equals b.EmpId
                join c in _context.HrEmployeeUserRelations on a.UpdatedBy equals c.UserId
                join d in _context.EmployeeDetails on c.EmpId equals d.EmpId
                where
                    (!startDate.HasValue || !endDate.HasValue || (a.UpdatedDate >= startDate && a.UpdatedDate <= endDate)) &&
                    (parsedEmployeeIds.Count == 0 || parsedEmployeeIds.Contains(a.EmpID)) &&
                    (parsedInfoDesc == 0 || infoDescs.Contains(parsedInfoDesc)) &&
                    (parsedInfotype == 0 || infotypes.Contains(parsedInfotype))
                orderby a.EmpID, a.UpdatedDate descending
                select new AuditInformationDto
                {
                    InfoID = a.InfoID,
                    Info01ID = a.Info01ID,
                    EmpID = a.EmpID,
                    EmpCode = b.EmpCode,
                    Name = b.Name,
                    InformationType = a.Information,
                    OldValue = a.OldValue,
                    NewValue = a.NewValue,
                    UpdatedBy = d.Name,
                    EffectiveFrom = a.UpdatedDate.HasValue ? a.UpdatedDate.Value.ToString(_employeeSettings.DateFormat) : _employeeSettings.NotAvailable,
                    EffectiveTime = a.UpdatedDate.HasValue ? a.UpdatedDate.Value.ToString("HH:mm:ss") : _employeeSettings.NotAvailable
                }
            ).ToList();


            return result;

        }
        public async Task<List<object>> EmployeeTypeAsync(int employeeid)
        {
            var result = await (from emp in _context.HrEmpPersonals
                                join a in _context.HrEmpMasters on emp.EmpId equals a.EmpId
                                where emp.EmpId == employeeid
                                select new
                                {
                                    Emp_Id = emp.EmpId,
                                    EmployeeCode = a.EmpCode,
                                    Name = $"{a.FirstName} {a.MiddleName} {a.LastName}",
                                    EmployeeType = emp.EmployeeType
                                }).AsNoTracking().ToListAsync();

            return result.Cast<object>().ToList();
        }
        public async Task<List<object>> GeoSpacingTypeAndCriteriaAsync(string type)
        {
            var result = await _context.HrmValueTypes.Where(v => v.Type == type).Select(v => new { v.Value, v.Description }).AsNoTracking().ToListAsync();
            return result.Cast<object>().ToList();
        }
        public async Task<List<GeoSpacingDto>> GetGeoSpacingAsync(int employeeid)
        {


            var query1 = await (from a in _context.Geotagging02s
                                join b in _context.Geotagging02As on a.GeoEmpId equals b.GeoEmpId
                                join c in _context.HrmValueTypes on new { Value = a.Geotype, Type = "GeoSpacingType" } equals new { c.Value, c.Type }
                                join d in _context.HrmValueTypes on new { Value = b.GeoCriteria, Type = "GeoSpacingCriteria" } equals new { d.Value, d.Type }
                                where a.EmpId == employeeid
                                select new GeoSpacingDto
                                {
                                    GeoEmpId = a.GeoEmpId,
                                    GeoEmpAid = b.GeoEmpAid,
                                    EmpId = a.EmpId,
                                    LevelId = a.LevelId,
                                    Geotype = a.Geotype,
                                    GeotypeCode = c.Code,
                                    GeotypeDescription = c.Description,
                                    GeoCriteria = b.GeoCriteria,
                                    GeoCriteriaCode = d.Code,
                                    GeoCriteriaDescription = d.Description,
                                    Latitude = b.Latitude ?? "",
                                    Longitude = b.Longitude ?? "",
                                    Radius = b.Radius ?? "",
                                    LiveTracking = a.LiveTracking,
                                    LocationId = (int?)b.LocationId ?? -1,
                                    GeoCoordinates = b.Coordinates
                                }).ToListAsync();

            if (query1.Any())
            {
                return query1.ToList();
            }
            else
            {
                var ctnew = SplitStrings_XML(_context.HrEmpMasters
                     .Where(h => h.EmpId == Convert.ToInt32(employeeid))
                     .Select(h => h.EmpEntity).FirstOrDefault(), ',')
                    .Select((item, index) => new LinkItemDto
                    {
                        Item = item,
                        LinkLevel = index + 2
                    }).Where(c => !string.IsNullOrEmpty(c.Item));

                var query2 = await Task.Run(() => (from f in ctnew
                                                   join b in _context.Geotagging01s on Convert.ToInt32(f.Item) equals b.LinkId
                                                   join c in _context.Geotagging01As on b.GeoEntityId equals c.GeoEntityId
                                                   join d in _context.HrmValueTypes on new { Value = b.Geotype, Type = "GeoSpacingType" } equals new { d.Value, d.Type }
                                                   join e in _context.HrmValueTypes on new { Value = c.GeoCriteria, Type = "GeoSpacingCriteria" } equals new { e.Value, e.Type }
                                                   select new GeoSpacingDto
                                                   {
                                                       GeoEmpId = b.GeoEntityId,
                                                       GeoEmpAid = c.GeoEntityAid,
                                                       EmpId = Convert.ToInt32(employeeid),
                                                       LevelId = b.LevelId,
                                                       Geotype = b.Geotype,
                                                       GeotypeCode = d.Code,
                                                       GeotypeDescription = d.Description,
                                                       GeoCriteria = c.GeoCriteria,
                                                       GeoCriteriaCode = e.Code,
                                                       GeoCriteriaDescription = e.Description,
                                                       Latitude = c.Latitude ?? "",
                                                       Longitude = c.Longitude ?? "",
                                                       Radius = c.Radius ?? "",
                                                       LiveTracking = b.LiveTracking,
                                                       LocationId = (int?)c.LocationId ?? -1
                                                   }).ToList());


                if (query2.Any())
                {
                    return query2.ToList();
                }
                else
                {
                    var query3 = await (from a in _context.Geotagging00s
                                        join b in _context.Geotagging00As on a.GeoCompId equals b.GeoCompId
                                        join c in _context.HrmValueTypes on new { Value = a.Geotype, Type = "GeoSpacingType" } equals new { c.Value, c.Type }
                                        join d in _context.HrmValueTypes on new { Value = b.GeoCriteria, Type = "GeoSpacingCriteria" } equals new { d.Value, d.Type }
                                        select new GeoSpacingDto
                                        {
                                            GeoEmpId = a.GeoCompId,
                                            LevelId = a.LevelId,
                                            Geotype = a.Geotype,
                                            GeotypeCode = c.Code,
                                            GeotypeDescription = c.Description,
                                            GeoCriteria = b.GeoCriteria,
                                            GeoCriteriaCode = d.Code,
                                            GeoCriteriaDescription = d.Description,
                                            Latitude = b.Latitude ?? "",
                                            Longitude = b.Longitude ?? "",
                                            Radius = b.Radius ?? "",
                                            LiveTracking = a.LiveTracking,
                                            LocationId = (int?)b.LocationId ?? -1
                                        }).ToListAsync();

                    return query3.ToList();
                }
            }
        }
        public async Task<List<FillEmployeesBasedOnwWorkflowDto>> FillEmployeesBasedOnwWorkflowAsync(int firstEntityId, int secondEntityId)
        {
            // Step 1: Fetch data first, then apply SplitStrings_XML in memory
            var entityAccessRights = await _context.EntityAccessRights02s
                .Where(s => s.RoleId == firstEntityId)
                .ToListAsync(); // Fetch first

            var applicableFinal = entityAccessRights
                .SelectMany(s => SplitStrings_XML(s.LinkId, default)
                    .Select(item => new { Item = item, s.LinkLevel }))
                .ToList(); // Apply in-memory processing
            var cteOrg = new HashSet<int>(); // Optimized lookup
            var queue = new Queue<int>();

            var initialEmp = await _context.HrEmpReportings
                .Where(e => e.EmpId == secondEntityId)
                .Select(e => e.ReprotToWhome == e.EmpId ? (int?)null : e.ReprotToWhome)
                .FirstOrDefaultAsync();

            if (initialEmp.HasValue)
            {
                queue.Enqueue(initialEmp.Value);
                cteOrg.Add(initialEmp.Value);
            }

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                var subordinates = await _context.HrEmpReportings
                    .Where(e => e.ReprotToWhome == current && e.ReprotToWhome != e.EmpId)
                    .Select(e => e.EmpId)
                    .ToListAsync();

                foreach (var sub in subordinates)
                {
                    if (!cteOrg.Contains(sub))
                    {
                        queue.Enqueue(sub);
                        cteOrg.Add(sub);
                    }
                }
            }



            var hasRoleAccess = _context.EntityAccessRights02s
     .Any(s => s.RoleId == firstEntityId && s.LinkLevel == 15);

            var applicableHierarchyLevels = _context.EmployeeDetails
                .Join(_context.HighLevelViewTables,
                    d1 => d1.LastEntity,
                    a => a.LastEntityId,
                    (d1, a) => new { d1, a })
                .Join(applicableFinal,
                    _ => 1,
                    _ => 1,
                    (x, b) => new { x.d1, x.a, b })
                .Where(joined =>
                    (joined.b.LinkLevel == 1 && joined.a.LevelOneId == int.Parse(joined.b.Item)) ||
                    (joined.b.LinkLevel == 2 && joined.a.LevelTwoId == int.Parse(joined.b.Item)) ||
                    (joined.b.LinkLevel == 3 && joined.a.LevelThreeId == int.Parse(joined.b.Item)) ||
                    (joined.b.LinkLevel == 4 && joined.a.LevelFourId == int.Parse(joined.b.Item)) ||
                    (joined.b.LinkLevel == 5 && joined.a.LevelFiveId == int.Parse(joined.b.Item)) ||
                    (joined.b.LinkLevel == 6 && joined.a.LevelSixId == int.Parse(joined.b.Item)) ||
                    (joined.b.LinkLevel == 7 && joined.a.LevelSevenId == int.Parse(joined.b.Item)) ||
                    (joined.b.LinkLevel == 8 && joined.a.LevelEightId == int.Parse(joined.b.Item)) ||
                    (joined.b.LinkLevel == 9 && joined.a.LevelNineId == int.Parse(joined.b.Item)) ||
                    (joined.b.LinkLevel == 10 && joined.a.LevelTenId == int.Parse(joined.b.Item)) ||
                    (joined.b.LinkLevel == 11 && joined.a.LevelElevenId == int.Parse(joined.b.Item)) ||
                    (joined.b.LinkLevel == 12 && joined.a.LevelTwelveId == int.Parse(joined.b.Item))
                )
                .Select(joined => joined.d1.EmpId);

            var reportsToSecondEntity = _context.HrEmpReportings
                .Where(hr => hr.ReprotToWhome == secondEntityId)
                .Select(hr => hr.EmpId);

            // Check if EntityAccessRights02s has the role and hierarchy enabled
            var hasHierarchyAccess = _context.EntityAccessRights02s
                .Any(s => s.RoleId == firstEntityId && s.Hierarchy == 1);

            // Get all Employee IDs that exist in the hierarchical structure
            var hierarchicalEmpIds = hasHierarchyAccess ? cteOrg.ToList() : new List<int>();

            // Main Query
            var query = from d in _context.EmployeeDetails
                        join e in _context.BranchDetails on d.BranchId equals e.LinkId
                        where
                            (hasRoleAccess ||
                            applicableHierarchyLevels.Contains(d.EmpId) ||
                            reportsToSecondEntity.Contains(d.EmpId) ||
                            hierarchicalEmpIds.Contains(d.EmpId)) // Corrected usage
                            && (d.SeperationStatus == 0 || d.SeperationStatus == -1)
                            && !d.IsDelete
                            && d.IsSave == 0
                        select new FillEmployeesBasedOnwWorkflowDto
                        {
                            Emp_Id = d.EmpId,
                            Employee = d.Name + " || " + d.EmpCode + " || " + e.Branch
                        };



            return await query.ToListAsync();
        }
        public async Task<List<object>> GetCountry()
        {
            var countryNames = await _context.AdmCountryMasters
                .Select(c => new { c.CountryId, c.CountryName })
                .ToListAsync();

            return countryNames.Cast<object>().ToList();
        }
        public async Task<List<object>> GetNationalities()
        {
            var nationalities = await _context.AdmCountryMasters
                .Select(c => new { c.CountryId, c.Nationality })
                .ToListAsync();

            return nationalities.Cast<object>().ToList();
        }
        public async Task<List<object>> GetBloodGroup()
        {
            var bloodGroups = await _context.HrmValueTypes
                .Where(v => v.Type == "BloodGroup")
                .Select(v => $"<option value={v.Code}>{v.Description}</option>")
                .ToListAsync();  // Get all the options as a list

            // Aggregate the list of options into a single string
            var bloodGroupsHtml = bloodGroups.Aggregate((result, item) => result + item);

            return new List<object> { new { BloodGroups = bloodGroupsHtml } };
        }
        public async Task<List<object>> FillReligion()
        {
            var religions = await _context.AdmReligionMasters
                .Select(r => new { r.ReligionId, r.ReligionName }) // Select ID and Name
                .ToListAsync();

            return religions.Cast<object>().ToList(); // Ensure it returns List<object>
        }
        public async Task<string> InsertOrUpdateLanguageSkills(LanguageSkillsSaveDto langSkills)
        {
            bool isUpdated = false; // Track if any update happens


            foreach (var skill in langSkills.Lilanguage)
            {
                var existingSkill = await _context.EmployeeLanguageSkills
                    .FirstOrDefaultAsync(e => e.EmpId == langSkills.EmpID
                                && e.LanguageId == skill.LanguageId
                                && e.Status != ApprovalStatus.Deleted.ToString());

                if (existingSkill == null)
                {
                    // Insert a new language skill
                    _context.EmployeeLanguageSkills.Add(new EmployeeLanguageSkill
                    {
                        EmpId = langSkills.EmpID,
                        LanguageId = skill.LanguageId,
                        Read = skill.Read,
                        Write = skill.Write,
                        Speak = skill.Speak,
                        Comprehend = skill.Comprehend,
                        MotherTongue = skill.MotherTongue,
                        Status = _employeeSettings.EmployeeStatus
                    });
                }
                else
                {
                    // Update the existing language skill
                    existingSkill.Read = skill.Read;
                    existingSkill.Write = skill.Write;
                    existingSkill.Speak = skill.Speak;
                    existingSkill.Comprehend = skill.Comprehend;
                    existingSkill.MotherTongue = skill.MotherTongue;

                    isUpdated = true; // Mark as updated
                }
            }

            await _context.SaveChangesAsync();

            return isUpdated ? "Successfully Updated" : "Successfully Saved";

        }
        public async Task<List<object>> FillLanguageTypes()
        {
            var languageTypes = await _context.HrEmpLanguagemasters
                .Select(v => new { v.LanguageId, v.Description })
                .ToListAsync();

            return languageTypes.Cast<object>().ToList();
        }
        public async Task<List<object>> FillConsultant()
        {
            var consultant = await _context.ConsultantDetails
                .Select(v => new { v.Id, v.ConsultantName })
                .ToListAsync();

            return consultant.Cast<object>().ToList();
        }
        public async Task<string> InsertOrUpdateReference(ReferenceSaveDto Reference)
        {
            var existingEntity = await _context.HrEmpreferences
                .FirstOrDefaultAsync(e => e.RefId == Reference.DetailID);

            var reference = _mapper.Map<HrEmpreference>(Reference);
            string result;

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                if (existingEntity != null)
                {
                    // Update the existing reference
                    existingEntity.EmpId = Reference.EmpID;
                    existingEntity.RefType = Reference.RefType;
                    existingEntity.RefMethod = Reference.RefMethod;
                    existingEntity.RefEmpId = Reference.RefEmpID;
                    existingEntity.ConsultantId = Reference.ConsultantID;
                    existingEntity.RefName = Reference.RefName;
                    existingEntity.PhoneNo = Reference.PhoneNo;
                    existingEntity.Address = Reference.Address;
                    existingEntity.Status = Reference.Status;
                    existingEntity.UpdatedBy = Reference.EntryBy;
                    existingEntity.UpdatedDate = DateTime.UtcNow;

                    // Update the entity in the context
                    _context.HrEmpreferences.Update(existingEntity);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    result = "Successfully Updated";
                }
                else
                {
                    reference.EntryDate = DateTime.UtcNow;
                    _context.HrEmpreferences.Add(reference);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    result = "Successfully Saved";
                }
            }
            catch (Exception)
            {
                // If something goes wrong, roll back the transaction
                await transaction.RollbackAsync();
                throw; // Optionally rethrow the exception or return an error message
            }

            return result; // Return the message
        }
        public async Task<List<object>> FillRewardType()
        {
            var rewardType = await _context.HrmValueTypes
                 .Where(v => v.Type == "Reward")
                .Select(v => new { v.Id, v.Description })
                .ToListAsync();

            return rewardType.Cast<object>().ToList();
        }
        public async Task<string> InsertOrUpdateEmpRewards(EmpRewardsSaveDto empRewardsDto)
        {
            bool isUpdated = false;
            try
            {
                // Check if the achievement exists in AchievementMasters
                var existingAchievement = await _context.AchievementMasters
                    .FirstOrDefaultAsync(a => a.Description.Trim() == empRewardsDto.Achievement.Trim());

                if (existingAchievement == null)
                {
                    var newAchievement = new AchievementMaster { Description = empRewardsDto.Achievement.Trim() };
                    _context.AchievementMasters.Add(newAchievement);
                    await _context.SaveChangesAsync(); // Save to generate AchievementID
                    existingAchievement = newAchievement;
                }

                int achievementId = existingAchievement.AchievementId;

                // Fetch RewardType ID from HRM_VALUE_TYPES
                var rewardTypeId = await _context.HrmValueTypes
                    .Where(h => h.Type == "Reward" && h.Id == empRewardsDto.RewardType)
                    .Select(h => h.Id)
                    .FirstOrDefaultAsync();

                // Check if the reward entry exists
                var existingReward = await _context.EmpRewards
                    .FirstOrDefaultAsync(r => r.EmpId == empRewardsDto.EmpId && r.RewardId == empRewardsDto.DetailID);

                if (existingReward != null)
                {
                    // Update existing reward
                    existingReward.AchievementId = achievementId;
                    existingReward.RewardType = rewardTypeId;
                    existingReward.Reason = empRewardsDto.Reason;
                    existingReward.RewardDate = DateTime.UtcNow;
                    existingReward.Amount = empRewardsDto.Amount;

                    isUpdated = true;
                }
                else
                {
                    // Insert new reward
                    _context.EmpRewards.Add(new EmpReward
                    {
                        EmpId = empRewardsDto.EmpId,
                        AchievementId = achievementId,
                        RewardType = rewardTypeId,
                        Status = _employeeSettings.EmployeeStatus,
                        Reason = empRewardsDto.Reason,
                        RewardDate = DateTime.UtcNow,
                        Amount = empRewardsDto.Amount
                    });
                }

                // Update ModifiedDate in HR_EMP_MASTER for the employee
                var employee = await _context.HrEmpMasters.FirstOrDefaultAsync(e => e.EmpId == empRewardsDto.EmpId);
                if (employee != null)
                {
                    employee.ModifiedDate = DateTime.UtcNow;
                }

                await _context.SaveChangesAsync();
                return isUpdated ? "Successfully Updated" : "Successfully Saved";
            }
            catch (Exception ex)
            {
                // Log the error (consider using a logging framework like Serilog, NLog, or built-in logging)
                Console.WriteLine($"Error: {ex.Message}");

                // Return an error message
                return "An error occurred while processing the request.";
            }
        }
        public async Task<List<object>> FillBankDetails(int empID)
        {
            var documents = await (
                from a in _context.HrmsDocument00s
                join b in _context.HrmsDocTypeMasters on (long)a.DocType equals b.DocTypeId // Explicit cast to long
                join c in _context.EmpDocumentAccesses on a.DocId equals (long)c.DocId
                where c.EmpId == empID &&
                      (
                          ((a.Active ?? false) && b.DocType == "BANK DETAILS" &&
                           !_context.HrmsEmpdocuments00s.Any(e => e.EmpId == empID && e.DocId == a.DocId && e.Status != "R"))
                          ||
                          (_context.HrmsDocument00s
                           .Join(_context.HrmsDocTypeMasters, d => (long)d.DocType, e => e.DocTypeId,
                                (d, e) => new { d.DocId, d.IsAllowMultiple, e.Code }) // Ensure d.IsAllowMultiple is used
                           .Any(x => (x.IsAllowMultiple ?? 0) == 1 && x.Code == "BNK" && x.DocId == a.DocId)) // Convert int? to bool correctly
                      )
                select new
                {
                    a.DocId,
                    a.DocName
                }).ToListAsync();

            return documents.Cast<object>().ToList();
        }
        public async Task<EmployeeDetailsDto> GetHrEmpDetailsAsync(int employeeId, int roleId)
        {

            var employeeDetails = await (from a in _context.HrEmpMasters
                                         join b in _context.HrEmpAddresses on a.EmpId equals b.EmpId into ab
                                         from b in ab.DefaultIfEmpty()
                                         join c in _context.HrEmpPersonals on a.EmpId equals c.EmpId into ac
                                         from c in ac.DefaultIfEmpty()
                                         join r in _context.HrEmpReportings on a.EmpId equals r.EmpId into ar
                                         from r in ar.DefaultIfEmpty()
                                         join f in _context.HighLevelViewTables on a.LastEntity equals f.LastEntityId into af
                                         from f in af.DefaultIfEmpty()
                                         join g in _context.HrEmployeeUserRelations on a.EmpId equals g.EmpId into ag
                                         from g in ag.DefaultIfEmpty()
                                         join h in _context.AdmUserRoleMasters on g.UserId equals h.UserId into gh
                                         from h in gh.DefaultIfEmpty()
                                         join j in _context.EmployeeCurrentStatuses on a.CurrentStatus equals j.Status into aj
                                         from j in aj.DefaultIfEmpty()
                                         join i in _context.AdmUserMasters on h.UserId equals i.UserId into hi
                                         from i in hi.DefaultIfEmpty()
                                         where a.EmpId == employeeId
                                         select new HrEmpMasterDto
                                         {
                                             EmpId = a.EmpId,
                                             EmpCode = a.EmpCode,
                                             FirstName = a.FirstName,
                                             MiddleName = a.MiddleName,
                                             LastName = a.LastName,
                                             JoinDt = FormatDate(a.JoinDt, _employeeSettings.DateFormat),//.ToString("dd/MM/yyyy"),
                                             EntryBy = a.EntryBy,
                                             EntryDt = FormatDate(a.EntryDt, _employeeSettings.DateFormat),//.ToString("dd/MM/yyyy"),
                                             EmpStatus = a.EmpStatus,
                                             CurrentStatus = j.Status,
                                             StatusDesc = j.StatusDesc,
                                             ReviewDt = FormatDate(a.ReviewDt, _employeeSettings.DateFormat),//.Value.ToString("dd/MM/yyyy") : null,
                                             WeddingDate = FormatDate(c.WeddingDate, _employeeSettings.DateFormat),//.HasValue ? c.WeddingDate.Value.ToString("dd/MM/yyyy") : null,
                                             ProbationDt = FormatDate(a.ProbationDt, _employeeSettings.DateFormat),
                                             IsProbation = a.IsProbation,
                                             NationalIdNo = a.NationalIdNo,
                                             DateOfBirth = FormatDate(a.DateOfBirth, _employeeSettings.DateFormat),// .HasValue ? a.DateOfBirth.Value.ToString("dd/MM/yyyy") : null,
                                             NoticePeriod = a.NoticePeriod,
                                             PassportNo = a.PassportNo,
                                             BranchId = a.BranchId,
                                             DepId = a.DepId,
                                             DesigId = a.DesigId,
                                             GuardiansName = a.GuardiansName,
                                             Religion = c.Religion,
                                             BloodGrp = c.BloodGrp,
                                             Nationality = c.Nationality,
                                             Country = c.Country,
                                             IdentMark = c.IdentMark,
                                             CountryOfBirth = a.CountryOfBirth,
                                             Height = c.Height,
                                             Weight = c.Weight,
                                             GratuityStrtDate = a.GratuityStrtDate.HasValue ? a.GratuityStrtDate.Value.ToString("dd/MM/yyyy") : null,
                                             FirstEntryDate = a.FirstEntryDate.HasValue ? a.FirstEntryDate.Value.ToString("dd/MM/yyyy") : null,
                                             OfficialEmail = b.OfficialEmail,
                                             PersonalEmail = b.PersonalEmail,
                                             Phone = b.Phone,
                                             HomeCountryPhone = b.HomeCountryPhone,
                                             Gender = c.Gender,
                                             MaritalStatus = GetMaritalStatus(c.MaritalStatus).ToString(),
                                             ReprotToWhome = r.ReprotToWhome,
                                             EffectDate = r.EffectDate.HasValue ? r.EffectDate.Value.ToString("dd/MM/yyyy") : null,
                                             RoleId = h.RoleId,
                                             NeedApp = i.NeedApp,
                                             Ishra = a.Ishra,
                                             CompanyConveyance = a.CompanyConveyance,
                                             CompanyVehicle = a.CompanyVehicle,
                                             LevelOneId = f.LevelOneId,
                                             LevelTwoId = f.LevelTwoId,
                                             LevelThreeId = f.LevelThreeId,
                                             LevelFourId = f.LevelFourId,
                                             LevelFiveId = f.LevelFiveId,
                                             LevelSixId = f.LevelSixId,
                                             LevelSevenId = f.LevelSevenId,
                                             LevelEightId = f.LevelEightId,
                                             LevelNineId = f.LevelNineId,
                                             LevelTenId = f.LevelTenId,
                                             LevelElevenId = f.LevelElevenId,
                                             ReportingEmp = _context.EmployeeDetails.Where(e => e.EmpId == r.ReprotToWhome).Select(e => e.Name).FirstOrDefault(),
                                             IsExpat = a.IsExpat ?? 0,
                                             PublicHoliday = a.PublicHoliday,
                                             MealAllowanceDeduct = a.MealAllowanceDeduct,
                                             EmpFileNumber = a.EmpFileNumber ?? "",
                                             PayrollMode = a.PayrollMode ?? 0,
                                             DailyRateTypeId = a.DailyRateTypeId ?? 0,
                                             CanteenRequest = a.CanteenRequest ?? 0
                                         }).FirstAsync();



            var defaultCompParameter = GetDefaultCompanyParameter(employeeId, "EMPLINKADD", _employeeSettings.companyParameterCodesType).Result;




            string code = "EnableEntityLinkInEmployeeCreation";

            var accessId = (from a in _context.TabAccessRights
                            join b in _context.TabMasters on a.TabId equals Convert.ToInt32(b.TabId)
                            where a.RoleId == roleId && b.Code == code
                            select (int?)a.AccessId)
                            .FirstOrDefault() ?? 0;


            return new EmployeeDetailsDto
            {
                HrEmpmasterDto = employeeDetails,
                DetailId = defaultCompParameter,
                AccessId = accessId
            };
        }
        public async Task<string?> UpdateEmployeeDetails(EmployeeDetailsUpdateDto employeeDetailsDto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                int entityChange = 0;

                // Check if the employee already exists
                bool existsEmployee = await _context.HrEmpMasters
                    .AnyAsync(emp => emp.EmpCode.Trim() == employeeDetailsDto.EmpCode.Trim()
                                     && emp.IsDelete == false
                                     && emp.EmpId != employeeDetailsDto.EmpId);



                // Fetch the last entity directly instead of selecting first and then comparing
                int? lastEntityChange = await _context.HrEmpMasters
                    .Where(e => e.EmpId == employeeDetailsDto.EmpId)
                    .Select(e => e.LastEntity)
                    .FirstOrDefaultAsync();

                if (lastEntityChange != employeeDetailsDto.LastEntity)
                {
                    entityChange = (int)ProbationStatus.PROBATION;
                }

                // Check if the employee has pending transfer history
                bool checkTransferHistory = await _context.TransferDetails00s
                                           .Join(_context.TransferDetails,
                                                 a => a.TransferBatchId,
                                                 b => b.TransferBatchId,
                                                 (a, b) => new { a, b })
                                           .AnyAsync(joined => new[] { ApprovalStatus.Approved.ToString(), ApprovalStatus.Pending.ToString() }.Contains(joined.b.ApprovalStatus)
                                                               && joined.a.EmpId == employeeDetailsDto.EmpId);

                if (checkTransferHistory)
                {
                    bool existsTransfer = await _context.TransferDetails00s
                        .Join(_context.TransferDetails,
                              a => a.TransferBatchId,
                              b => b.TransferBatchId,
                              (a, b) => new { a, b })
                        .Where(joined => new[] { ApprovalStatus.Approved.ToString(), ApprovalStatus.Pending.ToString() }.Contains(joined.b.ApprovalStatus)
                                         && joined.a.EmpId == employeeDetailsDto.EmpId)
                        .OrderByDescending(joined => joined.a.ToDate)
                        .Skip(1) // Exclude the most recent record
                        .AnyAsync(joined => joined.a.EmpId == employeeDetailsDto.EmpId
                                            && joined.a.LastEntity == employeeDetailsDto.LastEntity);

                    //if (existsTransfer)
                    //{
                    //    return new EmployeeDetailsUpdateDto();
                    //}
                }

                // Continue processing if conditions are not met...



                //// Get SortOrder
                //  var sortOrder = _context.LicensedCompanyDetails.Select(x => x.EntityLimit).FirstOrDefault();

                //  var mappings = await _context.Categorymasters
                //.Join(_context.HrmValueTypes,
                //      cat => cat.CatTrxTypeId,
                //      val => val.Value,
                //      (cat, val) => new { cat, val })
                //.Where(x => x.val.Type == typeof(CatTrxType).FullName &&
                //            new[] { CatTrxType.BranchType.ToString(), CatTrxType.DepartmentType.ToString(), CatTrxType.BandType.ToString(), CatTrxType.GradeType.ToString(), CatTrxType.DesigType.ToString(), CatTrxType.CompanyType.ToString() }
                //            .Contains(x.val.Code))
                //.GroupBy(x => x.val.Code)
                //.Select(g => new
                //{
                //    Code = g.Key,
                //    SortOrder = g.Max(x => x.cat.SortOrder)
                //})
                //.ToListAsync();

                var mappings = await GetCategoryMapperData();



                // // Extract values
                int branchMapId = mappings.FirstOrDefault(m => m.Code == CatTrxType.BranchType.ToString())?.SortOrder ?? 0;
                int depMapId = mappings.FirstOrDefault(m => m.Code == CatTrxType.DepartmentType.ToString())?.SortOrder ?? 0;
                int bandMapId = mappings.FirstOrDefault(m => m.Code == CatTrxType.BandType.ToString())?.SortOrder ?? 0;
                int designationMapId = mappings.FirstOrDefault(m => m.Code == CatTrxType.DesigType.ToString())?.SortOrder ?? 0;
                int countryMapId = mappings.FirstOrDefault(m => m.Code == CatTrxType.CompanyType.ToString())?.SortOrder ?? 0;
                int gradeId = mappings.FirstOrDefault(m => m.Code == CatTrxType.GradeType.ToString())?.SortOrder ?? 0;



                // Check if SortOrder is 11 before proceeding
                if (mappings.First().SortOrder == 8)
                {


                    var entityLevel = await _context.HighLevelViewTables
                        .Where(e => e.LastEntityId == employeeDetailsDto.LastEntity)
                        .Select(e => new
                        {
                            e.LastEntityId,
                            e.LevelOneId,
                            e.LevelTwoId,
                            e.LevelThreeId,
                            e.LevelFourId,
                            e.LevelFiveId,
                            e.LevelSixId,
                            e.LevelSevenId,
                            e.LevelEightId,
                            e.LevelNineId,
                            e.LevelTenId,
                            e.LevelElevenId
                        })
                        .FirstOrDefaultAsync();

                    if (entityLevel != null)
                    {
                        int GetLevelId(int mapId)
                        {
                            var propertyName = $"Level{mapId}Id";
                            var property = entityLevel.GetType().GetProperty(propertyName);
                            return property != null ? (int)property.GetValue(entityLevel) : 0;
                        }
                        employeeDetailsDto.BranchId = GetLevelId(branchMapId);
                        employeeDetailsDto.DepId = GetLevelId(depMapId);
                        employeeDetailsDto.BandId = GetLevelId(bandMapId);
                        employeeDetailsDto.GradeId = GetLevelId(gradeId);
                        employeeDetailsDto.DesigId = GetLevelId(designationMapId);

                        if (employeeDetailsDto.FirstEntryDate == null)
                        {
                            employeeDetailsDto.FirstEntryDate = employeeDetailsDto.JoinDt;
                        }
                        var OldLastEntity = await _context.HrEmpMasters.Where(e => e.EmpId == employeeDetailsDto.EmpId).Select(e => e.LastEntity).FirstOrDefaultAsync();


                        var employee = _context.HrEmpMasters.FirstOrDefault(e => e.EmpId == employeeDetailsDto.EmpId);

                        if (employee != null)
                        {
                            employee.EmpCode = employeeDetailsDto.EmpCode;

                            _mapper.Map(employeeDetailsDto, employee);
                            _context.HrEmpMasters.Update(employee);

                        }





                        var empData = (from a in _context.HrEmpMasters
                                       join b in _context.HighLevelViewTables on a.LastEntity equals b.LastEntityId
                                       where a.EmpId == employeeDetailsDto.EmpId
                                       select new { EmpMaster = a, HighLevel = b })
                                 .FirstOrDefault();

                        if (empData != null)
                        {
                            var levels = new List<int?>
                                {
                                    empData.HighLevel.LevelOneId, empData.HighLevel.LevelTwoId, empData.HighLevel.LevelThreeId,
                                    empData.HighLevel.LevelFourId, empData.HighLevel.LevelFiveId, empData.HighLevel.LevelSixId,
                                    empData.HighLevel.LevelSevenId, empData.HighLevel.LevelEightId, empData.HighLevel.LevelNineId,
                                    empData.HighLevel.LevelTenId, empData.HighLevel.LevelElevenId, empData.HighLevel.LevelTwelveId
                                };

                            empData.EmpMaster.EmpEntity = string.Join(",", levels.Where(x => x.HasValue && x != 0));
                            empData.EmpMaster.EmpFirstEntity = empData.HighLevel.LevelOneId?.ToString();


                        }

                        var cteOldEntity = GetEmployeeLevels(employeeDetailsDto.EmpId, OldLastEntity).Result;
                        var cteNewEntity = GetEmployeeLevels(employeeDetailsDto.EmpId, employeeDetailsDto.LastEntity).Result;

                        var levelNumbers = Enumerable.Range(1, 12); // Equivalent to CROSS JOIN VALUES(1-12)

                        var cteTransferEntityWise =
                            (from a in cteNewEntity
                             join b in cteOldEntity on a.EmpId equals b.EmpId
                             from v in levelNumbers // CROSS JOIN equivalent
                             join cm in _context.Categorymasters on v equals cm.SortOrder
                             where
                                (v == 1 && a.LevelOneDescription != b.LevelOneDescription) ||
                                (v == 2 && a.LevelTwoDescription != b.LevelTwoDescription) ||
                                (v == 3 && a.LevelThreeDescription != b.LevelThreeDescription) ||
                                (v == 4 && a.LevelFourDescription != b.LevelFourDescription) ||
                                (v == 5 && a.LevelFiveDescription != b.LevelFiveDescription) ||
                                (v == 6 && a.LevelSixDescription != b.LevelSixDescription) ||
                                (v == 7 && a.LevelSevenDescription != b.LevelSevenDescription) ||
                                (v == 8 && a.LevelEightDescription != b.LevelEightDescription) ||
                                (v == 9 && a.LevelNineDescription != b.LevelNineDescription) ||
                                (v == 10 && a.LevelTenDescription != b.LevelTenDescription) ||
                                (v == 11 && a.LevelElevenDescription != b.LevelElevenDescription) ||
                                (v == 12 && a.LevelTwelveDescription != b.LevelTwelveDescription)
                             select new
                             {
                                 a.EmpId,
                                 Entity = cm.Description,
                                 EntityOrder = cm.SortOrder,
                                 OldID = v switch
                                 {
                                     1 => b.LevelOneId,
                                     2 => b.LevelTwoId,
                                     3 => b.LevelThreeId,
                                     4 => b.LevelFourId,
                                     5 => b.LevelFiveId,
                                     6 => b.LevelSixId,
                                     7 => b.LevelSevenId,
                                     8 => b.LevelEightId,
                                     9 => b.LevelNineId,
                                     10 => b.LevelTenId,
                                     11 => b.LevelElevenId,
                                     12 => b.LevelTwelveId,
                                     _ => null
                                 },
                                 DescriptionOld = v switch
                                 {
                                     1 => b.LevelOneDescription,
                                     2 => b.LevelTwoDescription,
                                     3 => b.LevelThreeDescription,
                                     4 => b.LevelFourDescription,
                                     5 => b.LevelFiveDescription,
                                     6 => b.LevelSixDescription,
                                     7 => b.LevelSevenDescription,
                                     8 => b.LevelEightDescription,
                                     9 => b.LevelNineDescription,
                                     10 => b.LevelTenDescription,
                                     11 => b.LevelElevenDescription,
                                     12 => b.LevelTwelveDescription,
                                     _ => null
                                 },
                                 NewEID = v switch
                                 {
                                     1 => a.LevelOneId,
                                     2 => a.LevelTwoId,
                                     3 => a.LevelThreeId,
                                     4 => a.LevelFourId,
                                     5 => a.LevelFiveId,
                                     6 => a.LevelSixId,
                                     7 => a.LevelSevenId,
                                     8 => a.LevelEightId,
                                     9 => a.LevelNineId,
                                     10 => a.LevelTenId,
                                     11 => a.LevelElevenId,
                                     12 => a.LevelTwelveId,
                                     _ => null
                                 },
                                 DescriptionNew = v switch
                                 {
                                     1 => a.LevelOneDescription,
                                     2 => a.LevelTwoDescription,
                                     3 => a.LevelThreeDescription,
                                     4 => a.LevelFourDescription,
                                     5 => a.LevelFiveDescription,
                                     6 => a.LevelSixDescription,
                                     7 => a.LevelSevenDescription,
                                     8 => a.LevelEightDescription,
                                     9 => a.LevelNineDescription,
                                     10 => a.LevelTenDescription,
                                     11 => a.LevelElevenDescription,
                                     12 => a.LevelTwelveDescription,
                                     _ => null
                                 },
                                 //a.FromDate,
                                 //a.ToDate,
                                 SortOrder = v,
                                 IsExist = 0
                             }).ToList();


                        var cteLatestTransferTransition = _context.TransferTransition00s
                                 .Where(t => t.EmployeeId == employeeDetailsDto.EmpId
                                            && !new[] { ApprovalStatus.Rejceted.ToString(), ApprovalStatus.Deleted.ToString() }.Contains(t.BatchApprovalStatus)
                                            && (t.EmpApprovalStatus ?? ApprovalStatus.Approved.ToString()) != ApprovalStatus.Rejceted.ToString())
                                 .AsEnumerable() // Switch to client-side evaluation
                                 .GroupBy(t => new { t.EmployeeId, t.EntityOrder }) // Group by EmployeeID and EntityOrder
                                 .SelectMany(g => g
                                     .OrderByDescending(t => t.ToDate) // Order by ToDate descending
                                     .ThenByDescending(t => t.EntityOrder) // If ToDate is the same, order by EntityOrder
                                     .Select((t, index) => new { t, Rank = index + 1 }) // Assign rank
                                     .Where(x => x.Rank == 1)) // Only select rank 1
                                 .Select(t => new
                                 {
                                     t.t.TransferTransId,
                                     t.t.EntityOrder,
                                     t.t.ActionId,
                                     t.t.EmployeeId,
                                     t.t.OldEntityId,
                                     t.t.OldEntityDescription,
                                     t.t.NewEntityId,
                                     t.t.NewEntityDescription,
                                     t.t.ToDate,
                                     t.t.BatchApprovalStatus,
                                     t.t.EmpApprovalStatus
                                 })
                                 .ToList();



                        var cteLatestTransferTransitionList = cteLatestTransferTransition.ToList();
                        var cteTransferEntityWiseList = cteTransferEntityWise.ToList();

                        var updateInfo = (from a in _context.TransferTransition00s.AsEnumerable()  // Client-side execution
                                          join b in cteLatestTransferTransitionList
                                              on a.TransferTransId equals b.TransferTransId
                                          join c in cteTransferEntityWiseList
                                              on new { EmployeeId = a.EmployeeId ?? 0, EntityOrder = b.EntityOrder ?? 0 }
                                              equals new { EmployeeId = c.EmpId, EntityOrder = c.SortOrder }
                                          select new { a, c }).ToList();




                        if (updateInfo.Count > 0)
                        {
                            foreach (var item in updateInfo)
                            {
                                item.a.NewEntityId = item.c.NewEID;
                                item.a.NewEntityDescription = item.c.DescriptionNew;
                                //_context.TransferTransition00s.Update(item.a);
                                //   await _context.SaveChangesAsync();

                            }

                        }
                        var existingRecord = await _context.HrEmpAddresses.FirstOrDefaultAsync(e => e.EmpId == employeeDetailsDto.EmpId);
                        if (existingRecord is not null)
                        {
                            // Update existing record
                            existingRecord.InstId = employeeDetailsDto.InstId;
                            existingRecord.OfficialEmail = employeeDetailsDto.EmailId;
                            existingRecord.PersonalEmail = employeeDetailsDto.PersonalEmail;
                            existingRecord.Phone = employeeDetailsDto.Phone;
                            existingRecord.HomeCountryPhone = employeeDetailsDto.HomeCountryPhone;
                            existingRecord.UpdatedBy = employeeDetailsDto.EntryBy;
                            existingRecord.UpdatedDate = DateTime.UtcNow;
                        }
                        else
                        {
                            // Insert new record
                            await _context.HrEmpAddresses.AddAsync(new HrEmpAddress
                            {
                                EmpId = employeeDetailsDto.EmpId, // Ensure EmpId is set
                                InstId = employeeDetailsDto.InstId,
                                OfficialEmail = employeeDetailsDto.EmailId,
                                PersonalEmail = employeeDetailsDto.PersonalEmail,
                                Phone = employeeDetailsDto.Phone,
                                HomeCountryPhone = employeeDetailsDto.HomeCountryPhone,
                                EntryBy = employeeDetailsDto.EntryBy,
                                EntryDt = DateTime.UtcNow
                            });
                        }

                        var employeePersonal = await _context.HrEmpPersonals.FirstOrDefaultAsync(e => e.EmpId == employeeDetailsDto.EmpId);

                        if (employeePersonal is not null)
                        {
                            employeePersonal.InstId = employeeDetailsDto.InstId;
                            employeePersonal.Dob = employeeDetailsDto.DateOfBirth;
                            employeePersonal.Gender = employeeDetailsDto.Gender;
                            employeePersonal.MaritalStatus = employeeDetailsDto.PersonalDetailsDto.Marital_Status;
                            employeePersonal.Religion = employeeDetailsDto.PersonalDetailsDto.ReligionID;
                            employeePersonal.BloodGrp = employeeDetailsDto.PersonalDetailsDto.Blood_Grp;
                            employeePersonal.Nationality = employeeDetailsDto.PersonalDetailsDto.NationalityID;
                            employeePersonal.Country = employeeDetailsDto.PersonalDetailsDto.CountryID;
                            employeePersonal.IdentMark = employeeDetailsDto.PersonalDetailsDto.Ident_Mark;
                            employeePersonal.EntryBy = employeeDetailsDto.EntryBy;
                            employeePersonal.EntryDt = DateTime.UtcNow;
                            employeePersonal.Height = employeeDetailsDto.PersonalDetailsDto.Height;
                            employeePersonal.Weight = employeeDetailsDto.PersonalDetailsDto.Weight;
                            employeePersonal.WeddingDate = employeeDetailsDto.PersonalDetailsDto.Wedding_Date; // Ensures only the date part is stored
                            employeePersonal.CountryOfBirth = employeeDetailsDto.PersonalDetailsDto.CountryOfBirth;

                        }

                        var employeeReporting = await _context.HrEmpReportings.FirstOrDefaultAsync(e => e.EmpId == employeeDetailsDto.EmpId);
                        if (employeeReporting != null)
                        {
                            employeeReporting.InstId = employeeDetailsDto.InstId;
                            // employeeReporting.ReprotToWhome = employeeDetailsDto.PersonalDetailsDto.e;
                            //employeeReporting.EffectDate = employeeDetailsDto.EffectDate;
                            employeeReporting.Active = _employeeSettings.ActiveStatus;
                            employeeReporting.EntryBy = employeeDetailsDto.EntryBy;
                            employeeReporting.EntryDate = DateTime.UtcNow;
                            //await _context.SaveChangesAsync();
                        }


                        #region Lulu Client Customized Code


                        //-------------below code is been hidden for now(this one is using for Client like Lulu----------------
                        //                var employeeHRA = _context.HrEmpMasters
                        //.FirstOrDefault(e => e.EmpId == employeeDetailsDto.EmpID && (e.Ishra ?? false) == employeeDetailsDto.IsHRA);

                        //                if (employeeHRA == null)
                        //                {
                        //                    var latestHRA = _context.HraHistories
                        //                        .Where(h => h.EmployeeId == employeeDetailsDto.EmpID)
                        //                        .OrderByDescending(h => h.EntryDate)
                        //                        .FirstOrDefault();

                        //                    DateTime isHraToDate = latestHRA?.FromDate ?? DateTime.UtcNow;

                        //                    if (isHraToDate < DateTime.UtcNow)
                        //                    {
                        //                        if (latestHRA != null)
                        //                        {
                        //                            latestHRA.ToDate = DateTime.UtcNow.AddDays(-1);
                        //                            _context.HraHistories.Update(latestHRA);
                        //                        }
                        //                        else
                        //                        {
                        //                            isHraToDate = DateTime.UtcNow;
                        //                        }

                        //                        var newHRAHistory = new HraHistory
                        //                        {
                        //                            EmployeeId = employeeDetailsDto.EmpID,
                        //                            IsHra = employeeDetailsDto.IsHRA,
                        //                            FromDate = isHraToDate,
                        //                            ToDate = null,
                        //                            Entryby = employeeDetailsDto.EntryBy,
                        //                            EntryDate = DateTime.UtcNow
                        //                        };


                        //                        await _context.HraHistories.AddAsync(newHRAHistory);

                        #endregion



                        var latestRecord = _context.CompanyConveyanceHistories
                            .Where(c => c.EmployeeId == employeeDetailsDto.EmpId)
                            .OrderByDescending(c => c.EntryDate)
                            .FirstOrDefault();


                        int? companyConveyanceID = latestRecord?.Id ?? 0;
                        DateTime? companyConveyanceToDate = latestRecord?.ToDate;
                        if (companyConveyanceID != null)
                        {

                            if (latestRecord != null)
                            {
                                // Update the ToDate of the latest record
                                latestRecord.ToDate = DateTime.UtcNow;
                                companyConveyanceToDate = DateTime.UtcNow;
                            }
                            else
                            {
                                // Fetch ToDate from HR_EMP_MASTER if no previous history exists
                                companyConveyanceToDate = _context.HrEmpMasters
                                    .Where(e => e.EmpId == employeeDetailsDto.EmpId)
                                    .Select(e => companyConveyanceToDate) // Equivalent to GETUTCDATE()
                                    .FirstOrDefault();
                            }

                            // Insert new entry in CompanyConveyance_History
                            var newEntry = new CompanyConveyanceHistory
                            {
                                EmployeeId = employeeDetailsDto.EmpId,
                                CompanyConveyance = _context.HrEmpMasters
                                    .Where(e => e.EmpId == employeeDetailsDto.EmpId)
                                    .Select(e => e.CompanyConveyance)
                                    .FirstOrDefault(),
                                FromDate = companyConveyanceToDate,
                                ToDate = null,
                                EntryBy = employeeDetailsDto.EntryBy,
                                EntryDate = DateTime.UtcNow
                            };

                            await _context.CompanyConveyanceHistories.AddAsync(newEntry);

                            var latestHistory = _context.CompanyVehicleHistories
                                                 .Where(h => h.EmployeeId == employeeDetailsDto.EmpId)
                                                 .OrderByDescending(h => h.EntryDate)
                                                 .FirstOrDefault();

                            DateTime companyVehicleToDate = DateTime.UtcNow;

                            if (latestHistory != null)
                            {
                                // Update the existing record's ToDate
                                latestHistory.ToDate = DateTime.UtcNow;
                                //_context.CompanyVehicleHistories.Update(latestHistory);//
                                // _context.SaveChanges();
                            }

                            // Insert new record
                            var newHistory = new CompanyVehicleHistory
                            {
                                EmployeeId = employeeDetailsDto.EmpId,
                                CompanyVehicle = _context.HrEmpMasters
                                    .Where(e => e.EmpId == employeeDetailsDto.EmpId)
                                    .Select(e => e.CompanyVehicle)
                                    .FirstOrDefault(),
                                FromDate = companyVehicleToDate,
                                ToDate = null,
                                EntryBy = employeeDetailsDto.EntryBy
                            };


                            await _context.CompanyVehicleHistories.AddAsync(newHistory);

                            //var payRollschemeID = GetEmployeeSchemeID(employeeDetailsDto.EmpId, "ASSPAYROLLPERIOD", "PRL").Result;
                            var payRollschemeID = GetEmployeeSchemeID(employeeDetailsDto.EmpId, _employeeSettings.PayrollPeriodScheme, _employeeSettings.PayrollPeriodType).Result;
                            if (payRollschemeID > 0 && lastEntityChange == 1)
                            {
                                var payrollPeriod = _context.EmployeeLatestPayrollPeriods.Where(a => a.EmployeeId == employeeDetailsDto.EmpId)
                                                    .Join(_context.HrEmpMasters, a => a.EmployeeId, b => b.EmpId, (a, b) => new { a, b }).FirstOrDefault();

                                if (payrollPeriod != null)
                                {
                                    // Update logic similar to the CASE statements in SQL
                                    if (payRollschemeID != payrollPeriod.a.PayrollPeriodId)
                                    {
                                        payrollPeriod.a.PayrollPeriodId = payRollschemeID;
                                        payrollPeriod.a.EntryBy = employeeDetailsDto.EntryBy;  // Assuming 'entryBy' is passed as a parameter
                                        payrollPeriod.a.EntryDate = DateTime.UtcNow;  // Equivalent to GETUTCDATE()
                                    }

                                    //_context.EmployeeLatestPayrollPeriods.Update(payrollPeriod.a);
                                    // _context.SaveChangesAsync();

                                }
                            }
                            //var payRollBatchId = GetEmployeeSchemeID(employeeDetailsDto.EmpId, "ASSPAYCODE", "PRL").Result;
                            var payRollBatchId = GetEmployeeSchemeID(employeeDetailsDto.EmpId, _employeeSettings.PayrollBatchScheme, _employeeSettings.PayrollPeriodType).Result;
                            if (payRollBatchId > 0 && lastEntityChange == 1)
                            {
                                var payRollBatch = _context.EmployeeLatestPayrollBatches.Where(a => a.EmployeeId == employeeDetailsDto.EmpId)
                                                    .Join(_context.HrEmpMasters, a => a.EmployeeId, b => b.EmpId, (a, b) => new { a, b }).FirstOrDefault();

                                if (payRollBatch != null)
                                {
                                    // Update logic similar to the CASE statements in SQL
                                    if (payRollschemeID != payRollBatch.a.PayrollBatchId)
                                    {
                                        payRollBatch.a.PayrollBatchId = payRollBatchId;
                                        payRollBatch.a.EntryBy = employeeDetailsDto.EntryBy;  // Assuming 'entryBy' is passed as a parameter
                                        payRollBatch.a.EntryDate = DateTime.UtcNow;  // Equivalent to GETUTCDATE()
                                    }
                                    //_context.EmployeeLatestPayrollBatches.Update(payRollBatch.a);
                                    // _context.SaveChangesAsync();

                                }
                            }
                            bool existsTransferId = await (from a in _context.TransferDetails00s
                                                           join b in _context.TransferDetails on a.TransferBatchId equals b.TransferBatchId
                                                           where new[] { ApprovalStatus.Approved.ToString() }.Contains(b.ApprovalStatus)
                                                           && a.EmpId == employeeDetailsDto.EmpId
                                                           select a).AnyAsync();
                            if (existsTransferId && lastEntityChange == 1)
                            {
                                var transferID = (from a in _context.TransferDetails00s
                                                  join b in _context.TransferDetails
                                                      on a.TransferBatchId equals b.TransferBatchId
                                                  where b.ApprovalStatus == ApprovalStatus.Approved.ToString() && a.EmpId == employeeDetailsDto.EmpId
                                                  group new { a, b } by a.EmpId into grouped
                                                  select new
                                                  {
                                                      EmpID = grouped.Key,
                                                      TransferID = grouped.OrderByDescending(g => g.a.ToDate)
                                                                          .FirstOrDefault().a.TransferId
                                                  }).Where(t => t.EmpID == employeeDetailsDto.EmpId).Select(t => t.TransferID).FirstOrDefault();
                                if (transferID > 0)
                                {
                                    var transferDetails = await _context.TransferDetails00s.FirstOrDefaultAsync(t => t.TransferId == transferID);
                                    if (transferDetails != null)
                                    {
                                        //transferDetails.BranchId = employeeDetailsDto.BranchID;
                                        //transferDetails.DepartmentId = employeeDetailsDto.DeptID;
                                        //transferDetails.BandId = employeeDetailsDto.BandID;
                                        //transferDetails.GradeId = employeeDetailsDto.GradeID;
                                        //transferDetails.DesignationId = employeeDetailsDto.DesigID;
                                        transferDetails.LastEntity = employeeDetailsDto.LastEntity;


                                    }
                                    var positionHistory = _context.PositionHistories.FirstOrDefaultAsync(ph => ph.TransferId == transferID).Result;

                                    if (positionHistory != null)
                                    {
                                        positionHistory.LastEntity = employeeDetailsDto.LastEntity;

                                    }

                                    var query = from emp in _context.HrEmpMasters
                                                join high in _context.HighLevelViewTables
                                                    on emp.LastEntity equals high.LastEntityId
                                                where emp.EmpId == employeeDetailsDto.EmpId
                                                select new { emp, high };

                                    var employeeToUpdate = query.FirstOrDefault();

                                    if (employeeToUpdate != null)
                                    {
                                        var levelIds = new List<int?>
                                                    {
                                                        employeeToUpdate.high.LevelOneId, employeeToUpdate.high.LevelTwoId, employeeToUpdate.high.LevelThreeId,
                                                        employeeToUpdate.high.LevelFourId, employeeToUpdate.high.LevelFiveId, employeeToUpdate.high.LevelSixId,
                                                        employeeToUpdate.high.LevelSevenId, employeeToUpdate.high.LevelEightId, employeeToUpdate.high.LevelNineId,
                                                        employeeToUpdate.high.LevelTenId, employeeToUpdate.high.LevelElevenId, employeeToUpdate.high.LevelTwelveId
                                                    };

                                        // Constructing EmpEntity string by joining non-zero, non-null values
                                        employeeToUpdate.emp.EmpEntity = string.Join(",", levelIds.Where(id => id.HasValue && id > 0));

                                        // Assigning LevelOneId as EmpFirstEntity
                                        employee.EmpFirstEntity = employeeToUpdate.high.LevelOneId?.ToString();

                                        //dbContext.SaveChanges();
                                    }



                                }

                                var surveyExists = _context.SurveyRelations
                                    .Any(sr => sr.EmpId == employeeDetailsDto.EmpId && sr.ProbationReview == (int)ProbationStatus.PROBATION);

                                if (surveyExists)
                                {

                                    var probationRecord = (from pr in _context.ProbationRating00s
                                                           join pr2 in _context.ProbationRating02s
                                                           on pr.ProbRateId equals pr2.ProbRateId into prGroup
                                                           from pr2 in prGroup.DefaultIfEmpty()
                                                           where pr.ApprovalStatus == ApprovalStatus.Pending.ToString()
                                                                 && pr.EmpId == employeeDetailsDto.EmpId
                                                                 && (pr.CurrentRecord ?? 0) == (int)ProbationStatus.PROBATION
                                                                 && pr2.NextRemarkDate != null
                                                           orderby pr.ProbRateId
                                                           select new
                                                           {
                                                               ProbRateID = pr.ProbRateId,
                                                               UpdatedProbDate = pr2.NextRemarkDate
                                                           }).FirstOrDefault();

                                    if (probationRecord != null)
                                    {
                                        var probationEntry = await _context.ProbationRating00s.FindAsync(probationRecord.ProbRateID);
                                        if (probationEntry != null)
                                        {
                                            probationEntry.ApprovalStatus = ApprovalStatus.Approved.ToString();
                                            probationEntry.ManualApprover = employeeDetailsDto.EntryBy;
                                            probationEntry.ManualApproveDate = DateTime.UtcNow;
                                        }

                                        var workflowStatus = _context.ProbationWorkFlowstatuses
                                            .Where(pw => pw.RequestId == probationRecord.ProbRateID)
                                            .ToList();

                                        foreach (var status in workflowStatus)
                                        {
                                            status.ApprovalStatus = ApprovalStatus.Approved.ToString();
                                        }

                                        var surveyRelation = _context.SurveyRelations
                                            .Where(sr => sr.EmpId == employeeDetailsDto.EmpId)
                                            .ToList();

                                        foreach (var survey in surveyRelation)
                                        {
                                            //survey.ProbationReview = 1;
                                            survey.ProbationReview = (int)ProbationStatus.PROBATION;
                                        }

                                        if (probationRecord.UpdatedProbDate != null)
                                        {
                                            var employeeToUpdate = _context.HrEmpMasters.FirstOrDefault(e => e.EmpId == employeeDetailsDto.EmpId);
                                            if (employee != null)
                                            {
                                                employee.ProbationDt = probationRecord.UpdatedProbDate;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        //var surveyRelationsToDelete = _context.SurveyRelations
                                        //    .Where(sr => sr.EmpId == employeeDetailsDto.EmpId && sr.ProbationReview == 1)
                                        //    .ToList();
                                        var surveyRelationsToDelete = _context.SurveyRelations
                                         .Where(sr => sr.EmpId == employeeDetailsDto.EmpId && sr.ProbationReview == (int)ProbationStatus.PROBATION)
                                         .ToList();

                                        _context.SurveyRelations.RemoveRange(surveyRelationsToDelete);
                                    }



                                }

                            }
                        }

                    }

                    //int result = await _context.SaveChangesAsync();
                    //await transaction.CommitAsync();
                    //return result > 0 ? _employeeSettings.DataUpdateSuccessStatus : _employeeSettings.DataInsertFailedStatus;

                }
                int result = await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return result > 0 ? _employeeSettings.DataUpdateSuccessStatus : _employeeSettings.DataInsertFailedStatus;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception(ex.Message);
            }
        }
        public async Task<int?> GetEmployeeSchemeID(int? employeeId, string? code, string? type)
        {


            int? value = 0;

            // Fetch Employee Entity and FirstEntity
            var employee = await _context.EmployeeDetails
                .Where(e => e.EmpId == employeeId)
                .Select(e => new { e.EmpEntity, e.EmpFirstEntity })
                .FirstOrDefaultAsync();

            if (employee == null)
                return 0;

            string entity = employee.EmpEntity;
            string firstEntity = employee.EmpFirstEntity;


            value = await (from cp02 in _context.CompanyParameters02s
                           join cp in _context.CompanyParameters on cp02.ParamId equals cp.Id
                           where cp02.EmpId == employeeId &&
                                 cp.ParameterCode == code &&
                                 cp.Type == type
                           orderby cp02.Id
                           select cp02.Value).FirstOrDefaultAsync() ?? 0;


            // If not found, check in CompanyParameters01 with EmpEntity
            if (value == 0 && !string.IsNullOrEmpty(entity))
            {
                var entityIds = entity.Split(',').Select(int.Parse).ToList();


                value = await (from cp01 in _context.CompanyParameters01s
                               join cp in _context.CompanyParameters
                               on cp01.ParamId equals cp.Id
                               where cp01.LevelId != 1 &&
                                     entityIds.Contains((int)cp01.LinkId) &&
                                     cp.ParameterCode == code &&
                                     cp.Type == type
                               orderby cp01.LevelId descending
                               select cp01.Value)
                .FirstOrDefaultAsync() ?? 0;


            }

            // If still not found, check in CompanyParameters01 with EmpFirstEntity
            if (value == 0 && !string.IsNullOrEmpty(firstEntity))
            {
                var firstEntityIds = firstEntity.Split(',').Select(int.Parse).ToList();
                value = await (from cp01 in _context.CompanyParameters01s
                               join cp in _context.CompanyParameters
                               on cp01.ParamId equals cp.Id
                               where firstEntityIds.Contains((int)cp01.LinkId) &&
                                     cp.ParameterCode == code &&
                                     cp.Type == type
                               orderby cp01.LevelId descending
                               select cp01.Value)
            .FirstOrDefaultAsync() ?? 0;

            }

            // If still not found, check at the company level
            if (value == 0)
            {
                value = await (from cp in _context.CompanyParameters
                               where cp.ParameterCode == code && cp.Type == type
                               select cp.Value)
              .FirstOrDefaultAsync();

            }

            return value;

        }
        private async Task<List<HighLevelTableDto>> GetEmployeeLevels(int empId, int? entityId)
        {
            var employeeLevel = await _context.HighLevelViewTables
                                  .Where(h => h.LastEntityId == entityId)
                                  .Select(h => new HighLevelTableDto
                                  {
                                      EmpId = empId,
                                      LevelOneId = h.LevelOneId,
                                      LevelOneDescription = h.LevelOneDescription,
                                      LevelTwoId = h.LevelTwoId,
                                      LevelTwoDescription = h.LevelTwoDescription,
                                      LevelThreeId = h.LevelThreeId,
                                      LevelThreeDescription = h.LevelThreeDescription,
                                      LevelFourId = h.LevelFourId,
                                      LevelFourDescription = h.LevelFourDescription,
                                      LevelFiveId = h.LevelFiveId,
                                      LevelFiveDescription = h.LevelFiveDescription,
                                      LevelSixId = h.LevelSixId,
                                      LevelSixDescription = h.LevelSixDescription,
                                      LevelSevenId = h.LevelSevenId,
                                      LevelSevenDescription = h.LevelSevenDescription,
                                      LevelEightId = h.LevelEightId,
                                      LevelEightDescription = h.LevelEightDescription,
                                      LevelNineId = h.LevelNineId,
                                      LevelNineDescription = h.LevelNineDescription,
                                      LevelTenId = h.LevelTenId,
                                      LevelTenDescription = h.LevelTenDescription,
                                      LevelElevenId = h.LevelElevenId,
                                      LevelElevenDescription = h.LevelElevenDescription,
                                      LevelTwelveId = h.LevelTwelveId,
                                      LevelTwelveDescription = h.LevelTwelveDescription,
                                      //FromDate = (DateTime?)null,
                                      //ToDate = (DateTime?)null
                                  }).ToListAsync();
            return employeeLevel;
        }
        public async Task<List<object>> BankTypeEdit()
        {
            return await (
                from b in _context.HrmsDocument00s
                join c in _context.HrmsDocument00s on b.DocId equals c.DocId into bcJoin
                from c in bcJoin.DefaultIfEmpty()
                join d in _context.HrmsDocTypeMasters on (long)c.DocType equals d.DocTypeId into cdJoin
                from d in cdJoin.DefaultIfEmpty()
                where b.Active == true && d.DocType == "BANK DETAILS"
                select new
                {
                    b.DocId,
                    b.DocName
                }
            ).AsNoTracking()
            .Select(x => (object)x)
            .ToListAsync();
        }
        public async Task<List<object>> CertificationsDropdown(string description)
        {
            return await (from a in _context.GeneralCategories
                          join b in _context.ReasonMasters on a.Id equals b.Value
                          where a.Description == description
                          select new { b.ReasonId, b.Description })
                               .AsNoTracking()
                               .Select(x => (object)x)
                               .ToListAsync();

        }
        public async Task<string> InsertOrUpdateCertificates(CertificationSaveDto certificates)
        {
            var existingCertification = await _context.EmployeeCertifications
                .FirstOrDefaultAsync(e => e.EmpId == certificates.EmpId && e.CertificationId == certificates.CertificationID);

            var certification = _mapper.Map<EmployeeCertification>(certificates);
            string result;

            using var transaction = await _context.Database.BeginTransactionAsync();

            if (existingCertification != null)
            {
                existingCertification.CertificationName = certificates.CertificationName;
                existingCertification.CertificationField = certificates.CertificationField;
                existingCertification.YearofCompletion = certificates.YearOfCompletion;
                existingCertification.IssuingAuthority = certificates.IssuingAuthority;
                existingCertification.UpdatedBy = certificates.EntryBy;
                existingCertification.UpdatedDate = DateTime.UtcNow;

                _context.EmployeeCertifications.Update(existingCertification);
                result = "2";
            }
            else
            {
                certification.EntryDate = DateTime.UtcNow;
                certification.Status = ApprovalStatus.Approved.ToString();// "A";
                _context.EmployeeCertifications.Add(certification);
                result = "1";
            }

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return result;
        }
        public async Task<string> UpdateEmployeeType(EmployeeTypeDto EmployeeType)
        {
            string ErrorID = _employeeSettings.empSystemStatus;
            var employee = await _context.HrEmpPersonals
        .FirstOrDefaultAsync(e => e.EmpId == EmployeeType.EmpID);

            if (employee != null)
            {
                employee.EmployeeType = EmployeeType.EmployeeType;
                await _context.SaveChangesAsync();

                ErrorID = "1";
            }

            return ErrorID;
        }
        public async Task<string> InsertOrUpdateSkill(SaveSkillSetDto skillset)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            bool isWorkflowNeeded = await IsWorkflowNeeded();

            var hrSaveSkill = _mapper.Map<HrEmpTechnicalApprl>(skillset);

            if (isWorkflowNeeded)
            {
                string? codeId = await GenerateRequestId(skillset.Emp_Id);
                if (!string.IsNullOrEmpty(codeId))
                {
                    hrSaveSkill.RequestId = await GetLastSequence(codeId);
                    await _context.HrEmpTechnicalApprls.AddAsync(hrSaveSkill);
                    await UpdateCodeGeneration(codeId);
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return "Successfully Saved";
            }


            var hrEmpTechnicalApprl = new HrEmpTechnicalApprl
            {
                InstId = skillset.InstId,
                EmpId = skillset.Emp_Id,
                Course = skillset.Course,
                CourseDtls = skillset.Course_Dtls,
                Year = skillset.Year,
                DurFrm = skillset.DurationFrom,
                DurTo = skillset.DurationTo,
                MarkPer = skillset.Rating,
                Status = ApprovalStatus.Approved.ToString(),// "A",
                FlowStatus = _employeeSettings.E,
                EntryBy = skillset.Entry_By,
                EntryDt = skillset.EntryDt,
                InstName = skillset.Inst_Name,
                RequestId = null,
                DateFrom = DateTime.UtcNow,
                LangSkills = skillset.Mark_Per
            };
            await _context.HrEmpTechnicalApprls.AddAsync(hrEmpTechnicalApprl);

            var hrEmpTechnical = await _context.HrEmpTechnicals
                .FirstOrDefaultAsync(x => x.TechId == skillset.DetailID && x.EmpId == skillset.Emp_Id);

            if (hrEmpTechnical != null)
            {
                hrEmpTechnical.InstId = skillset.InstId;
                hrEmpTechnical.Course = skillset.Course;
                hrEmpTechnical.CourseDtls = skillset.Course_Dtls;
                hrEmpTechnical.Year = skillset.Year;
                hrEmpTechnical.DurFrm = skillset.DurationFrom;
                hrEmpTechnical.DurTo = skillset.DurationTo;
                hrEmpTechnical.MarkPer = skillset.Rating;
                hrEmpTechnical.EntryBy = skillset.Entry_By;
                hrEmpTechnical.EntryDt = skillset.EntryDt;
                hrEmpTechnical.InstName = skillset.Inst_Name;
                hrEmpTechnical.LangSkills = skillset.Mark_Per;

                _context.HrEmpTechnicals.Update(hrEmpTechnical);
            }
            else
            {

                hrEmpTechnical = new HrEmpTechnical
                {
                    InstId = skillset.InstId,
                    EmpId = skillset.Emp_Id,
                    Course = skillset.Course,
                    CourseDtls = skillset.Course_Dtls,
                    Year = skillset.Year,
                    DurFrm = skillset.DurationFrom,
                    DurTo = skillset.DurationTo,
                    MarkPer = skillset.Rating,

                    EntryBy = skillset.Entry_By,
                    EntryDt = skillset.EntryDt,
                    InstName = skillset.Inst_Name,
                    LangSkills = skillset.Mark_Per
                };

                await _context.HrEmpTechnicals.AddAsync(hrEmpTechnical);
            }

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return "Successfully Saved";
        }
        public async Task<string> UploadOrUpdateEmployeeDocuments(List<IFormFile> files, string filePath, QualificationAttachmentDto attachmentDto)
        {
            var result = await _qualificationAttachmentRepository.UploadOrUpdateDocuments(
                files,
                filePath,
                (file, filePath) => new QualificationAttachment
                {
                    EmpId = attachmentDto.EmployeeId,
                    QualFileName = Path.GetFileName(filePath),
                    FilePath = filePath,
                    FileType = file.ContentType,
                    EntryBy = attachmentDto.EntryBy,
                    EntryDate = DateTime.UtcNow,
                    UpdatedBy = attachmentDto.EntryBy,
                    UpdatedDate = DateTime.UtcNow,
                    DocStatus = ApprovalStatus.Approved.ToString(),// "A",
                    QualificationId = attachmentDto.QualificationId
                },
                entity => ((QualificationAttachment)(object)entity).EmpId
            );

            return result.Message;
        }
        public int? GetEmployeeParametersettingsNew(int? employeeId, string code, string type)
        {


            int? value = (from a in _context.CompanyParameters02s
                          join b in _context.CompanyParameters on a.ParamId equals b.Id
                          where b.ParameterCode == code && b.Type == type && a.EmpId == employeeId
                          select (int?)a.Value).FirstOrDefault();


            if (!value.HasValue || value == 0)
            {
                var entity = _context.EmployeeDetails
                                   .Where(a => a.EmpId == employeeId)
                                   .Select(a => a.EmpEntity)
                                   .FirstOrDefault();

                if (!string.IsNullOrEmpty(entity))
                {
                    var entityList = entity.Split(',').ToList();

                    value = (from a in _context.CompanyParameters01s
                             join b in _context.CompanyParameters on a.ParamId equals b.Id into paramJoin
                             from b in paramJoin.DefaultIfEmpty()
                             where entityList.Contains(a.LinkId.ToString()) && a.LevelId != 1 &&
                                   b.ParameterCode == code && b.Type == type
                             orderby a.LevelId descending
                             select (int?)a.Value).FirstOrDefault();
                }
            }


            if (!value.HasValue || value == 0)
            {
                var firstEntity = _context.EmployeeDetails
                                        .Where(a => a.EmpId == employeeId)
                                        .Select(a => a.EmpFirstEntity)
                                        .FirstOrDefault();

                if (!string.IsNullOrEmpty(firstEntity))
                {
                    var firstEntityList = firstEntity.Split(',').ToList();

                    value = (from a in _context.CompanyParameters01s
                             join b in _context.CompanyParameters on a.ParamId equals b.Id into paramJoin
                             from b in paramJoin.DefaultIfEmpty()
                             where firstEntityList.Contains(a.LinkId.ToString()) &&
                                   b.ParameterCode == code && b.Type == type
                             orderby a.LevelId descending
                             select (int?)a.Value).FirstOrDefault();
                }
            }

            // If still null or 0, check Company-level parameters
            if (!value.HasValue || value == 0)
            {
                value = _context.CompanyParameters
                               .Where(p => p.ParameterCode == code && p.Type == type)
                               .Select(p => (int?)p.Value)
                               .FirstOrDefault();
            }

            return value;

        }
        public async Task<string> InsertQualification(QualificationTableDto Qualification, string updateType, string FirstEntityID, int EmpEntityIds)
        {
            //throw new NotImplementedException ( );
            var Detailid = Qualification.Qlficationid;
            if (Detailid == 0)
            {

                var WorkFlowNeed = await (from a in _context.CompanyParameters
                                          join b in _context.HrmValueTypes on a.Value equals b.Value
                                          where b.Type == "EmployeeReporting" && a.ParameterCode == "WRKFLO"
                                          select b.Code).FirstAsync();

                int Qualifid = 0;

                if (WorkFlowNeed == "Yes")
                {
                    var TransactionID = _context.TransactionMasters
                                   .Where(tm => tm.TransactionType == "Qualification")
                                   .Select(tm => tm.TransactionId)
                                   .FirstOrDefault();



                    //var CodeID = await GetSequence (Qualification.Emp_Id, TransactionID, FirstEntityID, EmpEntityIds);

                    var codeID = GetSequence(Qualification.EmpId ?? 0, TransactionID, FirstEntityID, EmpEntityIds);

                    if (codeID == null)
                    {
                        return "No sequence";
                    }

                    var RequestID = _context.AdmCodegenerationmasters.Where(ac => ac.CodeId == Convert.ToInt32(codeID)).Select(ac => ac.LastSequence).FirstOrDefault();


                    var qualificationApproval = new HrEmpQualificationApprl
                    {
                        InstId = Qualification.InstId,
                        EmpId = Qualification.EmpId ?? 0,
                        Course = Qualification.Course,
                        University = Qualification.University,
                        InstName = Qualification.InstName,
                        //DurFrm = Qualification.Dur_Frm,
                        DurFrm = DateTime.TryParse(Qualification.DurFrm, out var tempDurFrm) ? tempDurFrm : (DateTime?)null,
                        DurTo = DateTime.TryParse(Qualification.DurTo, out var tempDurTo) ? tempDurTo : (DateTime?)null,

                        // DurTo = Qualification.Dur_To,
                        YearPass = Qualification.YearPass,
                        MarkPer = Qualification.MarkPer,
                        Class = Qualification.Class,
                        Subjects = Qualification.Subjects,
                        Status = Qualification.Status,
                        FlowStatus = Qualification.Status,
                        EntryBy = (int)Qualification.Entryby,
                        EntryDt = (DateTime)Qualification.EntryDate,
                        RequestId = RequestID
                    };
                    _context.HrEmpQualificationApprls.Add(qualificationApproval);
                    _context.SaveChanges();
                    Qualification.Qlficationid = qualificationApproval.QlfId;

                    Qualifid = qualificationApproval.QlfId;


                    var codeGenMaster = _context.AdmCodegenerationmasters.FirstOrDefault(ac => ac.CodeId == Convert.ToInt32(codeID));
                    if (codeGenMaster != null)
                    {
                        codeGenMaster.CurrentCodeValue = (_context.AdmCodegenerationmasters
                                                              .Where(ac => ac.CodeId == Convert.ToInt32(codeID))
                                                              .Max(ac => ac.CurrentCodeValue) ?? 0) + 1;

                        string numberFormat = codeGenMaster.NumberFormat;
                        string currentCodeValue = codeGenMaster.CurrentCodeValue.ToString();

                        int length = numberFormat.Length - currentCodeValue.Length;
                        string seq = numberFormat.Substring(0, length);
                        string finalCode = codeGenMaster.Code + seq + currentCodeValue;

                        codeGenMaster.LastSequence = finalCode;
                        _context.SaveChanges();
                    }
                }
                else
                {
                    // Check Employee Qualification Settings
                    int? optionenb = GetEmployeeParametersettingsNew(Qualification.Entryby, "ENBQUALIF", _employeeSettings.companyParameterCodesType);

                    var qualificationApprl = new HrEmpQualificationApprl
                    {
                        InstId = Qualification.InstId,
                        EmpId = Qualification.EmpId ?? 0,
                        Course = Qualification.Course,
                        University = Qualification.University,
                        InstName = Qualification.InstName,

                        DurFrm = DateTime.TryParse(Qualification.DurFrm, out var tempDurFrm) ? tempDurFrm : (DateTime?)null,
                        DurTo = DateTime.TryParse(Qualification.DurTo, out var tempDurTo) ? tempDurTo : (DateTime?)null,

                        YearPass = Qualification.YearPass,
                        MarkPer = Qualification.MarkPer,
                        Class = Qualification.Class,
                        Subjects = Qualification.Subjects,
                        Status = ApprovalStatus.Approved.ToString(),// "A",
                        FlowStatus = _employeeSettings.E,
                        EntryBy = (int)Qualification.Entryby,
                        EntryDt = (DateTime)Qualification.EntryDate,
                        RequestId = null,
                        DateFrom = DateTime.UtcNow,
                        CourseId = Qualification.CourseId,
                        UniversityId = Qualification.UniversityId,
                        InstitId = Qualification.InstitutId,
                        SpecialId = Qualification.SpecialId
                    };
                    _context.HrEmpQualificationApprls.Add(qualificationApprl);
                    _context.SaveChanges();

                    if (optionenb == 1)
                    {
                        var qualification = new HrEmpQualification
                        {
                            InstId = Qualification.InstId,
                            EmpId = Qualification.EmpId ?? 0,
                            Course = Qualification.Course,
                            University = Qualification.University,
                            InstName = Qualification.InstName,
                            DurFrm = DateTime.TryParse(Qualification.DurFrm, out var tempDurFrm1) ? tempDurFrm1 : (DateTime?)null,
                            DurTo = DateTime.TryParse(Qualification.DurTo, out var tempDurTo1) ? tempDurTo1 : (DateTime?)null,

                            YearPass = Qualification.YearPass,
                            MarkPer = Qualification.MarkPer,
                            Class = Qualification.Class,
                            Subjects = Qualification.Subjects,
                            EntryBy = (int)Qualification.Entryby,
                            EntryDt = (DateTime)Qualification.EntryDate,
                            CourseId = Qualification.CourseId,
                            UniversityId = Qualification.UniversityId,
                            InstitId = Qualification.InstitutId,
                            SpecialId = Qualification.SpecialId
                        };
                        _context.HrEmpQualifications.Add(qualification);
                        _context.SaveChanges();
                        Qualification.Qlficationid = qualification.QlfId;

                        Qualifid = qualification.QlfId;

                    }

                    else
                    {
                        var qualification = new HrEmpQualification
                        {
                            InstId = Qualification.InstId,
                            EmpId = Qualification.EmpId ?? 0,
                            Course = Qualification.Course,
                            University = Qualification.University,
                            InstName = Qualification.InstName,
                            DurFrm = DateTime.TryParse(Qualification.DurFrm, out var tempDurFrm2) ? tempDurFrm2 : (DateTime?)null,
                            DurTo = DateTime.TryParse(Qualification.DurTo, out var tempDurTo2) ? tempDurTo2 : (DateTime?)null,
                            YearPass = Qualification.YearPass,
                            MarkPer = Qualification.MarkPer,
                            Class = Qualification.Class,
                            Subjects = Qualification.Subjects,
                            EntryBy = (int)Qualification.Entryby,
                            EntryDt = (DateTime)Qualification.EntryDate
                        };
                        _context.HrEmpQualifications.Add(qualification);
                        _context.SaveChanges();
                        Qualification.Qlficationid = qualification.QlfId;

                        Qualifid = qualification.QlfId;

                    }

                    // Update HR_EMP_MASTER
                    var empMaster = _context.HrEmpMasters.FirstOrDefault(e => e.EmpId == Qualification.EmpId);
                    if (empMaster != null)
                    {
                        empMaster.ModifiedDate = DateTime.UtcNow;
                        _context.SaveChanges();
                    }
                }

                // Set response message
                var Error_Message = Qualifid.ToString();
                return Error_Message;
            }
            else
            {

                string errorMessage = "";
                int? optionenb = GetEmployeeParametersettingsNew(Qualification.Entryby, "ENBQUALIF", _employeeSettings.companyParameterCodesType);


                if (updateType == "Pending")
                {
                    var apprl = await _context.HrEmpQualificationApprls
                        .FirstOrDefaultAsync(q => q.QlfId == Qualification.Qlficationid && q.EmpId == Qualification.EmpId);

                    if (apprl != null)
                    {
                        apprl.Course = Qualification.Course;
                        apprl.University = Qualification.University;
                        apprl.InstName = Qualification.InstName;
                        apprl.DurFrm = DateTime.TryParse(Qualification.DurFrm, out var tempDurFrm) ? tempDurFrm : (DateTime?)null;
                        apprl.DurTo = DateTime.TryParse(Qualification.DurTo, out var tempDurTo) ? tempDurTo : (DateTime?)null;
                        apprl.YearPass = Qualification.YearPass;
                        apprl.MarkPer = Qualification.MarkPer;
                        apprl.Class = Qualification.Class;
                        apprl.Subjects = Qualification.Subjects;
                        apprl.EntryBy = Qualification.Entryby;
                        apprl.EntryDt = Qualification.EntryDate;
                        apprl.Status = "Pending"; // Assuming 'Pending' for status in your case

                        await _context.SaveChangesAsync();
                    }
                }
                else if (updateType == "Approved")
                {
                    var workFlowNeed = await _context.CompanyParameters
                        .Join(_context.HrmValueTypes, a => a.Value, b => b.Value, (a, b) => new { a, b })
                        .Where(x => x.b.Type == "EmployeeReporting" && x.a.ParameterCode == "WRKFLO")
                        .Select(x => x.b.Code)
                        .FirstOrDefaultAsync();

                    if (workFlowNeed == "Yes")
                    {
                        var transactionID = await _context.TransactionMasters
                            .Where(tm => tm.TransactionType == "Qualification")
                            .Select(tm => tm.TransactionId)
                            .FirstOrDefaultAsync();

                        var codeID = GetSequence(Qualification.EmpId ?? 0, transactionID, FirstEntityID, EmpEntityIds);
                        // GetSequence(Qualification.EmpId ?? 0, TransactionID, FirstEntityID, EmpEntityIds);
                        if (codeID == null)
                        {
                            errorMessage = "NoSequence";
                            return errorMessage;
                        }

                        var requestID = await _context.AdmCodegenerationmasters
                            .Where(ac => ac.CodeId == Convert.ToInt32(codeID))
                            .Select(ac => ac.LastSequence)
                            .FirstOrDefaultAsync();

                        //var codeGenMaster = _context.AdmCodegenerationmasters.FirstOrDefault(ac => ac.CodeId == Convert.ToInt32(codeID));

                        var qualificationApproval = new HrEmpQualificationApprl
                        {
                            InstId = Qualification.InstId,
                            EmpId = Qualification.EmpId ?? 0,
                            Course = Qualification.Course,
                            University = Qualification.University,
                            InstName = Qualification.InstName,
                            DurFrm = DateTime.TryParse(Qualification.DurFrm, out var tempDurFrm) ? tempDurFrm : (DateTime?)null,
                            DurTo = DateTime.TryParse(Qualification.DurTo, out var tempDurTo) ? tempDurTo : (DateTime?)null,
                            YearPass = Qualification.YearPass,
                            MarkPer = Qualification.MarkPer,
                            Class = Qualification.Class,
                            Subjects = Qualification.Subjects,
                            Status = "Approved",
                            FlowStatus = "E", // Assuming 'E' as FlowStatus
                            EntryBy = Qualification.Entryby,
                            EntryDt = Qualification.EntryDate,
                            RequestId = requestID,
                            MasterId = Qualification.Qlficationid // Assuming MasterID should be set here
                        };

                        _context.HrEmpQualificationApprls.Add(qualificationApproval);
                        await _context.SaveChangesAsync();

                        errorMessage = "Successfully Updated";

                        // Workflow handling (assuming some function exists for this)
                        // await WorkFlowActivityFlowAsync(qualification.EmpId, "Qualification", qualificationApproval.QlfId, qualification.Entryby);

                        // Update code generation
                        var codeGenMaster = await _context.AdmCodegenerationmasters
                            .Where(ac => ac.CodeId == Convert.ToInt32(codeID))
                            .FirstOrDefaultAsync();

                        if (codeGenMaster != null)
                        {
                            codeGenMaster.CurrentCodeValue = (await _context.AdmCodegenerationmasters
                                    .Where(ac => ac.CodeId == Convert.ToInt32(codeID))
                                    .MaxAsync(ac => (int?)ac.CurrentCodeValue)) ?? 0 + 1;

                            string numberFormat = codeGenMaster.NumberFormat;
                            string currentCodeValue = codeGenMaster.CurrentCodeValue.ToString();

                            int length = numberFormat.Length - currentCodeValue.Length;
                            string seq = numberFormat.Substring(0, length);
                            string finalCode = codeGenMaster.Code + seq + currentCodeValue;

                            codeGenMaster.LastSequence = finalCode;
                            await _context.SaveChangesAsync();
                        }
                    }
                    else if (workFlowNeed == "No")
                    {
                        if (optionenb == 1)
                        {
                            var qualificationRecord = await _context.HrEmpQualifications
                                .FirstOrDefaultAsync(q => q.QlfId == Qualification.Qlficationid && q.EmpId == Qualification.EmpId);

                            if (qualificationRecord != null)
                            {
                                qualificationRecord.Course = Qualification.Course;
                                qualificationRecord.University = Qualification.University;
                                qualificationRecord.InstName = Qualification.InstName;
                                qualificationRecord.DurFrm = DateTime.TryParse(Qualification.DurFrm, out var tempDurFrm1) ? tempDurFrm1 : (DateTime?)null;
                                qualificationRecord.DurTo = DateTime.TryParse(Qualification.DurTo, out var tempDurTo1) ? tempDurTo1 : (DateTime?)null;
                                qualificationRecord.YearPass = Qualification.YearPass;
                                qualificationRecord.MarkPer = Qualification.MarkPer;
                                qualificationRecord.Class = Qualification.Class;
                                qualificationRecord.Subjects = Qualification.Subjects;
                                qualificationRecord.EntryBy = Qualification.Entryby;
                                qualificationRecord.EntryDt = Qualification.EntryDate;
                                qualificationRecord.CourseId = Qualification.CourseId;
                                qualificationRecord.UniversityId = Qualification.UniversityId;
                                qualificationRecord.InstitId = Qualification.InstitutId;
                                qualificationRecord.SpecialId = Qualification.SpecialId;

                                await _context.SaveChangesAsync();
                            }
                        }
                        else
                        {
                            var qualificationRecord = await _context.HrEmpQualifications
                                .FirstOrDefaultAsync(q => q.QlfId == Qualification.Qlficationid && q.EmpId == Qualification.EmpId);

                            if (qualificationRecord != null)
                            {
                                qualificationRecord.Course = Qualification.Course;
                                qualificationRecord.University = Qualification.University;
                                qualificationRecord.InstName = Qualification.InstName;
                                qualificationRecord.DurFrm = DateTime.TryParse(Qualification.DurFrm, out var tempDurFrm2) ? tempDurFrm2 : (DateTime?)null;
                                qualificationRecord.DurTo = DateTime.TryParse(Qualification.DurTo, out var tempDurTo2) ? tempDurTo2 : (DateTime?)null;
                                qualificationRecord.YearPass = Qualification.YearPass;
                                qualificationRecord.MarkPer = Qualification.MarkPer;
                                qualificationRecord.Class = Qualification.Class;
                                qualificationRecord.Subjects = Qualification.Subjects;
                                qualificationRecord.EntryBy = Qualification.Entryby;
                                qualificationRecord.EntryDt = Qualification.EntryDate;

                                await _context.SaveChangesAsync();
                            }
                        }

                        // Insert to HR_EMP_QUALIFICATION_APPRL
                        var qualificationApproval = new HrEmpQualificationApprl
                        {
                            InstId = Qualification.InstId,
                            EmpId = Qualification.EmpId ?? 0,
                            Course = Qualification.Course,
                            University = Qualification.University,
                            InstName = Qualification.InstName,
                            DurFrm = DateTime.TryParse(Qualification.DurFrm, out var tempDurFrm) ? tempDurFrm : (DateTime?)null,
                            DurTo = DateTime.TryParse(Qualification.DurTo, out var tempDurTo) ? tempDurTo : (DateTime?)null,
                            YearPass = Qualification.YearPass,
                            MarkPer = Qualification.MarkPer,
                            Class = Qualification.Class,
                            Subjects = Qualification.Subjects,
                            Status = "A",
                            FlowStatus = "E", // Assuming 'E' as FlowStatus
                            EntryBy = Qualification.Entryby,
                            EntryDt = Qualification.EntryDate,
                            RequestId = null,
                            DateFrom = DateTime.UtcNow,
                            MasterId = Qualification.Qlficationid
                        };

                        _context.HrEmpQualificationApprls.Add(qualificationApproval);
                        await _context.SaveChangesAsync();
                    }
                }

                return errorMessage;
            }
        }
        public async Task<object> FillCountry()
        {
            var countries = await _context.AdmCountryMasters
                .Select(c => new
                {
                    c.CountryId,
                    c.CountryName,
                    c.Nationality
                })
                .ToListAsync();

            var countryNameHtml = countries.Aggregate("", (result, c) =>
                result + $"<option value={c.CountryId}>{c.CountryName}</option>");

            var nationalityHtml = countries.Aggregate("", (result, c) =>
                result + $"<option value={c.CountryId}>{c.Nationality}</option>");

            return new
            {
                Country_Name = countryNameHtml,
                Nationality = nationalityHtml
            };
        }
        public async Task<List<object>> FillEmployeeDropdown(string activeStatus, string employeeStatus, string probationStatus)
        {
            int activeStatusInt = int.TryParse(activeStatus, out int tempStatus) ? tempStatus : 0;
            int probationStatusInt = int.TryParse(probationStatus, out int tempProb) ? tempProb : 0;

            var employeeStatusList = employeeStatus.Split(',')
                .Select(s => int.TryParse(s, out int val) ? val : (int?)null)
                .Where(val => val.HasValue)
                .Select(val => val.Value)
                .ToList();

            var query = await _context.HrEmpMasters
                .Where(a =>
                    (a.CurrentStatus == activeStatusInt || activeStatusInt == 0) &&
                    (employeeStatusList.Contains(a.EmpStatus.Value) || new List<int> { 1, 2, 3, 7 }.Contains(a.EmpStatus.Value)) &&
                    (probationStatusInt == 1 ||
                     (probationStatusInt == 2 && a.IsProbation == true) ||
                     (probationStatusInt == 3 && a.IsProbation == false)) &&
                    a.IsDelete == false
                )
                .Select(a => (object)new
                {
                    EmpId = a.EmpId,
                    empcode = a.EmpCode,
                    Name =
                           (a.MiddleName == null && a.LastName == null ? a.FirstName :
                            a.MiddleName == null ? a.FirstName + " " + a.LastName :
                            a.LastName == null ? a.FirstName + " " + a.MiddleName :
                            a.FirstName + " " + a.MiddleName + " " + a.LastName)
                })
                .ToListAsync();

            return query.Cast<object>().ToList();
        }
        public async Task<List<object>> AssetGroupDropdownEdit()
        {
            var groupId = await _context.CategoryGroups
                .Where(g => g.DescriptionGrp == "Assets")
                .OrderBy(g => g.GroupId) // Ensures a deterministic "TOP 1"
                .Select(g => g.GroupId)
                .FirstOrDefaultAsync();

            if (groupId == null)
                return new List<object>();

            var result = await _context.GeneralCategories
                .Where(a => a.GroupId == groupId && a.GeneralIsActive == 1)
                .OrderBy(a => a.Description)
                .Select(a => new
                {
                    Reason_Id = a.Id,
                    a.Description,
                    EmplinkID = a.EmplinkId ?? 0
                })
                .ToListAsync();

            return result.Cast<object>().ToList();
        }
        public async Task<List<object>> GetAssetDropdownEdit(int varAssestTypeID)
        {
            var reasons = await _context.ReasonMasters
                .Where(a => a.Value == varAssestTypeID && a.Status != ApprovalStatus.Deleted.ToString())
                .OrderBy(a => a.Description)
                .Select(a => new
                {
                    a.ReasonId,
                    a.Description,
                    a.Value,
                    a.Status
                })
                .ToListAsync();

            return reasons.Cast<object>().ToList();
        }
        public async Task<List<object>> GetAssetDetailsEdit(string CommonName)
        {
            var assetCategories = await _context.AssetcategoryCodes
                .Where(a => a.Code == CommonName
                    && a.Status != ApprovalStatus.Deleted.ToString()
                    && a.Status != ApprovalStatus.Approved.ToString())
                .OrderBy(a => a.Description)
                .Select(a => new
                {
                    a.Id,
                    a.Code,
                    a.Description,
                    a.Value,
                    a.Status,
                    a.Assetclass,
                    a.AssetModel
                })
                .ToListAsync();

            return assetCategories.Cast<object>().ToList();
        }
        public async Task<string> AssetEdit(AssetEditDto assetEdits)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync()) // Start Transaction
            {
                try
                {
                    // Check if the asset is already assigned and not returned
                    bool isAssetAssigned = await _context.EmployeesAssetsAssigns
                        .AnyAsync(ea => ea.AssetGroup == assetEdits.AssetGroup &&
                                        ea.AssignId != assetEdits.varAssestID &&
                                        ea.EmpId == assetEdits.varEmpID &&
                                        (ea.ReturnDate == null || ea.ReturnDate >= assetEdits.ReceiveDate) &&
                                        ea.Status == "Open");

                    if (isAssetAssigned)
                    {
                        return "2"; // Asset is already assigned and not available
                    }

                    // Retrieve asset assignment
                    var assetToUpdate = await _context.EmployeesAssetsAssigns
                        .FirstOrDefaultAsync(a => a.AssignId == assetEdits.varAssestID);

                    var assetCategory = await _context.AssetcategoryCodes
                        .FirstOrDefaultAsync(a => a.Code == assetEdits.AssetNo);

                    if (assetToUpdate != null)
                    {
                        // Update existing asset details
                        assetToUpdate.EmpId = assetEdits.varEmpID;
                        assetToUpdate.AssetGroup = assetEdits.AssetGroup;
                        assetToUpdate.Asset = assetEdits.varAssestName;
                        assetToUpdate.AssetNo = assetEdits.AssetNo;
                        assetToUpdate.AssetModel = assetEdits.AssetModel;
                        assetToUpdate.Monitor = assetEdits.Monitor;
                        assetToUpdate.InWarranty = assetEdits.InWarranty;
                        assetToUpdate.OutOfWarranty = assetEdits.OutOfWarranty;
                        assetToUpdate.Status = assetEdits.varAssignAsetStatus;
                        assetToUpdate.ReceivedDate = assetEdits.ReceiveDate;
                        assetToUpdate.ExpiryDate = assetEdits.ExpiryDate;
                        assetToUpdate.ReturnDate = assetEdits.ReturnDate;
                        assetToUpdate.Remarks = assetEdits.Remarks;
                    }
                    else
                    {
                        // Insert new asset assignment if not found
                        var newAssetAssign = new EmployeesAssetsAssign
                        {

                            EmpId = assetEdits.varEmpID,
                            AssetGroup = assetEdits.AssetGroup,
                            Asset = assetEdits.varAssestName,
                            AssetNo = assetEdits.AssetNo,
                            AssetModel = assetEdits.AssetModel,
                            Monitor = assetEdits.Monitor,
                            InWarranty = assetEdits.InWarranty,
                            OutOfWarranty = assetEdits.OutOfWarranty,
                            Status = assetEdits.varAssignAsetStatus,
                            ReceivedDate = assetEdits.ReceiveDate,
                            ExpiryDate = assetEdits.ExpiryDate,
                            ReturnDate = assetEdits.ReturnDate,
                            Remarks = assetEdits.Remarks,
                            IsActive = true,
                            EntryBy = assetEdits.EntryBy,
                            EntryDate = DateTime.Now
                        };

                        _context.EmployeesAssetsAssigns.Add(newAssetAssign);
                    }

                    // Update AssetCategoryCode Status
                    if (assetCategory != null)
                    {
                        assetCategory.Status = assetEdits.ReturnDate != null ? ApprovalStatus.Pending.ToString() : ApprovalStatus.Approved.ToString();
                    }

                    // Add Asset Request History
                    var assetRequestHistory = new AssetRequestHistory
                    {
                        RequestId = assetEdits.varAssestID,
                        AssetEntryBy = assetEdits.EntryBy,
                        EntryDate = DateTime.UtcNow,
                        StatusType = 2,
                        AssetEmpId = assetEdits.varEmpID
                    };

                    _context.AssetRequestHistories.Add(assetRequestHistory);

                    // Save all changes in one go
                    await _context.SaveChangesAsync();

                    // Commit transaction
                    await transaction.CommitAsync();

                    return "1";
                }
                catch (Exception ex)
                {
                    // Rollback in case of error
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }
        public async Task<List<object>> GetAssetEditDatas(int varSelectedTypeID, int varAssestID)
        {
            List<object> result = new List<object>();

            // Get the date format from EmployeeSettings
            string dateFormat = _employeeSettings.DateFormat ?? "dd/MM/yyyy";

            if (varSelectedTypeID == 0)
            {
                var assetDetails = await (from e in _context.EmployeesAssetsAssigns
                                          join r in _context.ReasonMasters on e.Asset equals r.ReasonId.ToString()
                                          where e.AssignId == varAssestID
                                          select new
                                          {
                                              e.AssignId,
                                              e.EmpId,
                                              e.AssetGroup,
                                              e.Asset,
                                              e.AssetNo,
                                              e.AssetModel,
                                              e.Monitor,
                                              r.Description,
                                              InWarranty = e.InWarranty.HasValue ? e.InWarranty.Value.ToString(dateFormat) : null,
                                              OutOfWarranty = e.OutOfWarranty.HasValue ? e.OutOfWarranty.Value.ToString(dateFormat) : null,
                                              ReceivedDate = e.ReceivedDate.HasValue ? e.ReceivedDate.Value.ToString(dateFormat) : null,
                                              e.Status,
                                              ExpiryDate = e.ExpiryDate.HasValue ? e.ExpiryDate.Value.ToString(dateFormat) : null,
                                              ReturnDate = e.ReturnDate.HasValue ? e.ReturnDate.Value.ToString(dateFormat) : null,
                                              e.Remarks
                                          }).FirstOrDefaultAsync();

                if (assetDetails != null)
                {
                    result.Add(assetDetails);
                }
            }
            else
            {
                var assetBasicDetails = await (from e in _context.EmployeesAssetsAssigns
                                               join r in _context.ReasonMasters on e.Asset equals r.ReasonId.ToString()
                                               where e.AssignId == varAssestID
                                               select new
                                               {
                                                   e.AssignId,
                                                   e.EmpId,
                                                   e.AssetGroup,
                                                   e.Asset,
                                                   e.AssetNo,
                                                   e.AssetModel,
                                                   e.Monitor,
                                                   r.Description,
                                                   InWarranty = e.InWarranty.HasValue ? e.InWarranty.Value.ToString(dateFormat) : null,
                                                   OutOfWarranty = e.OutOfWarranty.HasValue ? e.OutOfWarranty.Value.ToString(dateFormat) : null,
                                                   ReceivedDate = e.ReceivedDate.HasValue ? e.ReceivedDate.Value.ToString(dateFormat) : null,
                                                   e.Status,
                                                   ExpiryDate = e.ExpiryDate.HasValue ? e.ExpiryDate.Value.ToString(dateFormat) : null,
                                                   ReturnDate = e.ReturnDate.HasValue ? e.ReturnDate.Value.ToString(dateFormat) : null,
                                                   e.Remarks
                                               }).FirstOrDefaultAsync();

                if (assetBasicDetails != null)
                {
                    result.Add(assetBasicDetails);
                }

                var assetDetailsWithFields = await (from a in _context.EmployeesAssetsAssigns
                                                    join b in _context.GeneralCategories on a.AssetGroup equals b.Id.ToString()
                                                    join c in _context.ReasonMasters on a.Asset equals c.ReasonId.ToString() into reasonJoin
                                                    from c in reasonJoin.DefaultIfEmpty()
                                                    join d in _context.ReasonMasterFieldValues on c.ReasonId equals d.ReasonId into fieldValueJoin
                                                    from d in fieldValueJoin.DefaultIfEmpty()
                                                    join e in _context.GeneralCategoryFields on d.CategoryFieldId equals e.CategoryFieldId into fieldJoin
                                                    from e in fieldJoin.DefaultIfEmpty()
                                                    join f in _context.HrmsDatatypes on e.DataTypeId equals f.DataTypeId into dataTypeJoin
                                                    from f in dataTypeJoin.DefaultIfEmpty()
                                                    where a.AssignId == varAssestID
                                                    select new
                                                    {
                                                        a.AssignId,
                                                        a.EmpId,
                                                        a.AssetGroup,
                                                        a.Asset,
                                                        a.AssetNo,
                                                        a.AssetModel,
                                                        a.Monitor,
                                                        c.Description,
                                                        InWarranty = a.InWarranty.HasValue ? a.InWarranty.Value.ToString(dateFormat) : null,
                                                        OutOfWarranty = a.OutOfWarranty.HasValue ? a.OutOfWarranty.Value.ToString(dateFormat) : null,
                                                        ReceivedDate = a.ReceivedDate.HasValue ? a.ReceivedDate.Value.ToString(dateFormat) : null,
                                                        a.Status,
                                                        ExpiryDate = a.ExpiryDate.HasValue ? a.ExpiryDate.Value.ToString(dateFormat) : null,
                                                        ReturnDate = a.ReturnDate.HasValue ? a.ReturnDate.Value.ToString(dateFormat) : null,
                                                        a.Remarks,
                                                        EmplinkID = b.EmplinkId ?? 0,
                                                        FieldDescription = e.FieldDescription,
                                                        FieldValues = f.IsDate == 1 && d.FieldValues != null
                                                                        ? DateTime.Parse(d.FieldValues).ToString(dateFormat)
                                                                        : d.FieldValues,
                                                        DataType = f.DataType,
                                                        DataTypeID = f.DataTypeId,
                                                        f.IsDate,
                                                        f.IsGeneralCategory,
                                                        f.IsDropdown,
                                                        ReasonFieldID = d.ReasonFieldId,
                                                        CategoryFieldID = e.CategoryFieldId,
                                                        c.ReasonId
                                                    }).ToListAsync();

                if (assetDetailsWithFields.Any())
                {
                    var groupedResult = assetDetailsWithFields
                        .GroupBy(x => new
                        {
                            x.AssignId,
                            x.EmpId,
                            x.AssetGroup,
                            x.Asset,
                            x.AssetNo,
                            x.AssetModel,
                            x.Monitor,
                            x.Description,
                            x.InWarranty,
                            x.OutOfWarranty,
                            x.ReceivedDate,
                            x.Status,
                            x.ExpiryDate,
                            x.ReturnDate,
                            x.Remarks,
                            x.EmplinkID,
                            x.ReasonId
                        })
                        .Select(g => new
                        {
                            g.Key.AssignId,
                            g.Key.EmpId,
                            g.Key.AssetGroup,
                            g.Key.Asset,
                            g.Key.AssetNo,
                            g.Key.AssetModel,
                            g.Key.Monitor,
                            g.Key.Description,
                            g.Key.InWarranty,
                            g.Key.OutOfWarranty,
                            g.Key.ReceivedDate,
                            g.Key.Status,
                            g.Key.ExpiryDate,
                            g.Key.ReturnDate,
                            g.Key.Remarks,
                            g.Key.EmplinkID,
                            g.Key.ReasonId,
                            Fields = g.Select(f => new
                            {
                                f.FieldDescription,
                                f.FieldValues,
                                f.DataType,
                                f.DataTypeID,
                                f.IsDate,
                                f.IsGeneralCategory,
                                f.IsDropdown,
                                f.ReasonFieldID,
                                f.CategoryFieldID
                            }).ToList()
                        }).ToList();

                    result.AddRange(groupedResult);
                }
            }

            return result;
        }
        public async Task<string> AssetDelete(int varEmpID, int varAssestID)
        {

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {

                var assetReasonIdString = await _context.EmployeesAssetsAssigns
                                                         .Where(ea => ea.AssignId == varAssestID)
                                                         .Select(ea => ea.Asset)
                                                         .FirstOrDefaultAsync();


                if (int.TryParse(assetReasonIdString, out int assetReasonId) && assetReasonId != 0)
                {

                    var assetCategory = await _context.AssetcategoryCodes
                                                       .Where(ac => ac.Code == assetReasonId.ToString())
                                                       .FirstOrDefaultAsync();

                    if (assetCategory != null)
                    {
                        assetCategory.Status = "P";
                        _context.AssetcategoryCodes.Update(assetCategory);
                    }


                    var assetAssignment = await _context.EmployeesAssetsAssigns
                                                         .Where(ea => ea.AssignId == varAssestID)
                                                         .FirstOrDefaultAsync();

                    if (assetAssignment != null)
                    {
                        _context.EmployeesAssetsAssigns.Remove(assetAssignment);

                        if (varEmpID == 1)
                        {
                            var reasonMasterFieldValues = await _context.ReasonMasterFieldValues
                                                                        .Where(rmf => rmf.ReasonId == assetReasonId)
                                                                        .ToListAsync();

                            _context.ReasonMasterFieldValues.RemoveRange(reasonMasterFieldValues);

                            var reasonMaster = await _context.ReasonMasters
                                                              .Where(rm => rm.ReasonId == assetReasonId)
                                                              .ToListAsync();

                            _context.ReasonMasters.RemoveRange(reasonMaster);
                        }

                        // Save changes and commit transaction
                        await _context.SaveChangesAsync();
                        await transaction.CommitAsync();

                        return "1"; // Return success
                    }
                    else
                    {
                        // If the asset assignment was not found, return failure message
                        return "Asset assignment not found";
                    }
                }
                else
                {
                    // If assetReasonId could not be parsed or is 0, return failure message
                    return "Invalid asset or asset not found";
                }
            }
        }
        public async Task<object> GetBankType(int employeeId)
        {
            var result = await (from a in _context.HrmsDocument00s
                                join b in _context.HrmsDocTypeMasters on a.DocType equals (int?)b.DocTypeId
                                join c in _context.EmpDocumentAccesses on (int?)a.DocId equals c.DocId
                                where c.EmpId == employeeId && a.Active == true &&
                                      (!_context.HrmsEmpdocuments00s.Any(d => d.DocId == a.DocId && d.Status != _employeeSettings.Status01) ||
                                       (b.DocType == _employeeSettings.Documents02) ||
                                       (_context.HrmsDocument00s.Join(_context.HrmsDocTypeMasters,
                                             doc => doc.DocType ?? 0,
                                             dt => dt.DocTypeId,
                                             (doc, dt) => new { doc.DocId, doc.IsAllowMultiple, dt.Code })
                                         .Any(x => x.IsAllowMultiple == 1 && x.Code == _employeeSettings.BankDocCode && x.DocId == a.DocId)))
                                select new
                                {
                                    DocId = a.DocId,
                                    DocName = a.DocName
                                }).ToListAsync();

            return result; // Returning as object (List of anonymous objects)
        }
        public async Task<object> GetGeneralSubCategoryList(string remarks)
        {
            var result = await (from a in _context.ReasonMasters
                                join b in _context.GeneralCategories on a.Type equals b.Description
                                where (b.Description == remarks) && a.Status == _employeeSettings.EmployeeStatus
                                select new
                                {
                                    ReasonId = a.ReasonId,
                                    Description = a.Description
                                }).ToListAsync();
            return result;

        }
        public async Task<string> SetEmpDocumentDetails(SetEmpDocumentDetailsDto SetEmpDocumentDetails)
        {
            var workFlowNeed = (from a in _context.CompanyParameters
                                join b in _context.HrmValueTypes on a.Value equals b.Value
                                where b.Type == _employeeSettings.EmployeeReportingType
                                      && a.ParameterCode == _employeeSettings.ParameterCode02
                                      && a.Type == _employeeSettings.CompanyParameterType
                                select b.Code).FirstOrDefault();

            var transactionID = (from a in _context.TransactionMasters
                                 where a.TransactionType == _employeeSettings.TransactionType
                                 select a.TransactionId).FirstOrDefault();
            var codeID = GetSequence(SetEmpDocumentDetails.EmpID ?? 0, transactionID, "", 0);

            if (codeID == null)
            {

                var ErrorId = 0;
                var ErrorMessage = _employeeSettings.ErrorMessage;

            }

            var RequestSequence = (from a in _context.AdmCodegenerationmasters
                                   where codeID == codeID
                                   select a.LastSequence).FirstOrDefault();
            var maxCurrentCodeValue = _context.AdmCodegenerationmasters
                .Where(a => a.CodeId == Convert.ToInt32(codeID))
                .Max(a => (int?)a.CurrentCodeValue) ?? 0;

            var updatedValue = maxCurrentCodeValue + 1;

            var recordToUpdate = _context.AdmCodegenerationmasters
                .FirstOrDefault(a => a.CodeId == Convert.ToInt32(codeID));

            if (recordToUpdate != null)
            {
                recordToUpdate.CurrentCodeValue = updatedValue;
                _context.SaveChanges();
            }

            var record = _context.AdmCodegenerationmasters
                .FirstOrDefault(a => a.CodeId == Convert.ToInt32(codeID));

            if (record != null)
            {

                int length = (record.NumberFormat.Length) - (record.CurrentCodeValue?.ToString().Length ?? 0);

                string seq = record.NumberFormat.Substring(0, Math.Max(0, length));

                string finalvalue = record.Code.ToString() + seq + (record.CurrentCodeValue?.ToString() ?? _employeeSettings.empSystemStatus);
                record.LastSequence = finalvalue;
                _context.SaveChanges();

            }

            if (workFlowNeed == _employeeSettings.Yes)
            {
                HrmsEmpdocuments00 newDocument;

                if (SetEmpDocumentDetails.ProxyID > 0)
                {
                    if (SetEmpDocumentDetails.ProxyID == SetEmpDocumentDetails.EmpID)
                    {
                        newDocument = new HrmsEmpdocuments00
                        {
                            DocId = SetEmpDocumentDetails.DocumentID,
                            EmpId = SetEmpDocumentDetails.EmpID,
                            RequestId = RequestSequence,
                            TransactionType = _employeeSettings.TransactionType,
                            Status = _employeeSettings.Status02,
                            FlowStatus = SetEmpDocumentDetails.FlowStatus,
                            ProxyId = 0,
                            EntryBy = SetEmpDocumentDetails.EntryBy,
                            EntryDate = DateTime.UtcNow
                        };
                    }
                    else
                    {
                        newDocument = new HrmsEmpdocuments00
                        {
                            DocId = SetEmpDocumentDetails.DocumentID,
                            EmpId = SetEmpDocumentDetails.ProxyID,
                            RequestId = RequestSequence,
                            TransactionType = _employeeSettings.TransactionType,
                            Status = _employeeSettings.Status02,
                            FlowStatus = SetEmpDocumentDetails.FlowStatus,
                            ProxyId = SetEmpDocumentDetails.EmpID,
                            EntryBy = SetEmpDocumentDetails.EntryBy,
                            EntryDate = DateTime.UtcNow
                        };
                    }
                }
                else
                {
                    newDocument = new HrmsEmpdocuments00
                    {
                        DocId = SetEmpDocumentDetails.DocumentID,
                        EmpId = SetEmpDocumentDetails.EmpID,
                        RequestId = RequestSequence,
                        TransactionType = _employeeSettings.TransactionType,
                        Status = _employeeSettings.Status02,
                        FlowStatus = SetEmpDocumentDetails.FlowStatus,
                        ProxyId = SetEmpDocumentDetails.ProxyID,
                        EntryBy = SetEmpDocumentDetails.EntryBy,
                        EntryDate = DateTime.UtcNow
                    };
                }


                _context.HrmsEmpdocuments00s.Add(newDocument);
                _context.SaveChanges();


                int newDetailID = newDocument.DetailId;


                //_context.Database.ExecuteSqlRaw ("exec WorkFlowActivityFlow @p0, @p1, @p2, @p3",
                //    parameters: new object[] { EmpID, "Document", newDetailID, EntryBy });
            }
            else if (workFlowNeed == _employeeSettings.No)
            {
                HrmsEmpdocuments00 newDocument;

                if (SetEmpDocumentDetails.ProxyID > 0)
                {
                    if (SetEmpDocumentDetails.ProxyID == SetEmpDocumentDetails.EmpID)
                    {
                        newDocument = new HrmsEmpdocuments00
                        {
                            DocId = SetEmpDocumentDetails.DocumentID,
                            EmpId = SetEmpDocumentDetails.EmpID,
                            RequestId = RequestSequence,
                            TransactionType = _employeeSettings.TransactionType,
                            Status = _employeeSettings.EmployeeStatus,
                            FlowStatus = _employeeSettings.Status03,
                            ProxyId = 0,
                            EntryBy = SetEmpDocumentDetails.EntryBy,
                            EntryDate = DateTime.UtcNow,
                            FinalApprovalDate = DateTime.UtcNow
                        };
                    }
                    else
                    {
                        newDocument = new HrmsEmpdocuments00
                        {
                            DocId = SetEmpDocumentDetails.DocumentID,
                            EmpId = SetEmpDocumentDetails.ProxyID,
                            RequestId = RequestSequence,
                            TransactionType = _employeeSettings.TransactionType,
                            Status = _employeeSettings.EmployeeStatus,
                            FlowStatus = _employeeSettings.Status03,
                            ProxyId = SetEmpDocumentDetails.EmpID,
                            EntryBy = SetEmpDocumentDetails.EntryBy,
                            EntryDate = DateTime.UtcNow,
                            FinalApprovalDate = DateTime.UtcNow
                        };
                    }
                }
                else
                {
                    newDocument = new HrmsEmpdocuments00
                    {
                        DocId = SetEmpDocumentDetails.DocumentID,
                        EmpId = SetEmpDocumentDetails.EmpID,
                        RequestId = RequestSequence,
                        TransactionType = _employeeSettings.TransactionType,
                        Status = _employeeSettings.EmployeeStatus,
                        FlowStatus = _employeeSettings.Status03,
                        ProxyId = SetEmpDocumentDetails.ProxyID,
                        EntryBy = SetEmpDocumentDetails.EntryBy,
                        EntryDate = DateTime.UtcNow,
                        FinalApprovalDate = DateTime.UtcNow
                    };
                }


                _context.HrmsEmpdocuments00s.Add(newDocument);
                _context.SaveChanges();


                int newDetailID = newDocument.DetailId;


                var approvedDocument = new HrmsEmpdocumentsApproved00
                {
                    DetailId = newDetailID,
                    DocId = newDocument.DocId,
                    EmpId = newDocument.EmpId,
                    RequestId = newDocument.RequestId,
                    TransactionType = _employeeSettings.TransactionType,
                    Status = _employeeSettings.EmployeeStatus,
                    ProxyId = newDocument.ProxyId,
                    FlowStatus = _employeeSettings.Status03,
                    DateFrom = DateTime.UtcNow,
                    EntryBy = newDocument.EntryBy,
                    EntryDate = newDocument.EntryDate,
                    FinalApprovalDate = newDocument.FinalApprovalDate
                };

                _context.HrmsEmpdocumentsApproved00s.Add(approvedDocument);
                _context.SaveChanges();
            }
            var sequenceid = 0;

            var codeGenMaster = _context.AdmCodegenerationmasters
                .FirstOrDefault(c => c.CodeId == sequenceid);

            if (codeGenMaster != null)
            {

                int maxCurrentCodeValue1 = _context.AdmCodegenerationmasters
                    .Where(c => c.CodeId == sequenceid)
                    .Max(c => (int?)c.CurrentCodeValue) ?? 0;

                codeGenMaster.CurrentCodeValue = maxCurrentCodeValue1 + 1;

                int numberFormatLength = codeGenMaster.NumberFormat.Length;
                int currentCodeValueLength = codeGenMaster.CurrentCodeValue.ToString().Length;
                int lengthDiff = numberFormatLength - currentCodeValueLength;

                string seq = codeGenMaster.NumberFormat.Substring(0, Math.Max(0, lengthDiff));

                string finalSequence = $"{codeGenMaster.Code}{seq}{codeGenMaster.CurrentCodeValue}";

                codeGenMaster.LastSequence = finalSequence;

                _context.SaveChanges();
            }
            string errorID = _context.HrmsEmpdocuments00s
                .OrderByDescending(d => d.DetailId)
                .Select(d => d.DetailId.ToString())
                .FirstOrDefault() ?? _employeeSettings.empSystemStatus;

            return errorID;

        }
        public async Task<string?> UpdatePersonalDetails(PersonalDetailsUpdateDto personalDetailsDto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var empMaster = await _context.HrEmpMasters.FindAsync(personalDetailsDto.EmpId);
                //var empAddress = await _context.HrEmpAddresses.FindAsync(personalDetailsDto.EmpId);
                var empAddress = await _context.HrEmpAddresses.Where(x => x.EmpId == personalDetailsDto.EmpId).FirstOrDefaultAsync();
                var empPersonalHistory = await _context.HrEmpPersonals.FindAsync(personalDetailsDto.EmpId);

                bool hasChanges = false;
                PersonalDetailsHistoryDto? history = null;
                if (empMaster != null)
                {
                    //if (empMaster.EmpId == personalDetailsDto.EmpId && (empMaster.GuardiansName != personalDetailsDto.GuardiansName || empMaster.Gender != personalDetailsDto.Gender || empMaster.DateOfBirth != personalDetailsDto.DateOfBirth))
                    //{
                    empMaster.GuardiansName = personalDetailsDto.GuardiansName;
                    empMaster.DateOfBirth = personalDetailsDto.DateOfBirth;
                    empMaster.Gender = personalDetailsDto.Gender;
                    empMaster.UpdatedBy = personalDetailsDto.EntryBy;
                    empMaster.UpdatedDate = DateTime.UtcNow;
                    hasChanges = true;
                    //}
                }

                if (empAddress != null)
                {
                    //if (empAddress.AddId == personalDetailsDto.EmpId && !string.Equals(empAddress.PersonalEmail?.Trim(), personalDetailsDto.PersonalEMail?.Trim(), StringComparison.OrdinalIgnoreCase))
                    //{
                    empAddress.PersonalEmail = personalDetailsDto.PersonalEMail;
                    empAddress.UpdatedBy = personalDetailsDto.EntryBy;
                    empAddress.UpdatedDate = DateTime.UtcNow;
                    hasChanges = true;
                    //}
                }

                if (empPersonalHistory != null)
                {
                    //if (empPersonalHistory.Dob != personalDetailsDto.DateOfBirth || empPersonalHistory.Nationality != personalDetailsDto.NationalityID || empPersonalHistory.Gender != personalDetailsDto.Gender ||
                    //    empPersonalHistory.Religion != personalDetailsDto.ReligionID || empPersonalHistory.Country != personalDetailsDto.CountryID || empPersonalHistory.MaritalStatus != personalDetailsDto.MaritalStatus ||
                    //    empPersonalHistory.IdentMark != personalDetailsDto.IdentificationMark || empPersonalHistory.BloodGrp != personalDetailsDto.BloodGrp ||
                    //    empPersonalHistory.Height != personalDetailsDto.Height || empPersonalHistory.Weight != personalDetailsDto.Weight || empPersonalHistory.WeddingDate != personalDetailsDto.WeddingDate ||
                    //    empPersonalHistory.CountryOfBirth != personalDetailsDto.country2ID)
                    //{
                    empPersonalHistory.Dob = personalDetailsDto.DateOfBirth;
                    empPersonalHistory.Nationality = personalDetailsDto.NationalityID;
                    empPersonalHistory.Gender = personalDetailsDto.Gender;
                    empPersonalHistory.Religion = personalDetailsDto.ReligionID;
                    empPersonalHistory.Country = personalDetailsDto.CountryID;
                    empPersonalHistory.MaritalStatus = personalDetailsDto.MaritalStatus;
                    empPersonalHistory.IdentMark = personalDetailsDto.IdentificationMark;
                    empPersonalHistory.BloodGrp = personalDetailsDto.BloodGrp;
                    empPersonalHistory.Height = personalDetailsDto.Height;
                    empPersonalHistory.Weight = personalDetailsDto.Weight;
                    empPersonalHistory.WeddingDate = personalDetailsDto.WeddingDate;
                    empPersonalHistory.CountryOfBirth = personalDetailsDto.country2ID;
                    empPersonalHistory.UpdatedBy = personalDetailsDto.EntryBy;
                    empPersonalHistory.UpdatedDate = DateTime.UtcNow;
                    hasChanges = true;
                    //}
                }

                //Insert into PersonalDetailsHistory only if data has changed
                if (hasChanges)
                {
                    history = new PersonalDetailsHistoryDto
                    {
                        EmployeeId = personalDetailsDto.EmpId,
                        GuardiansName = personalDetailsDto.GuardiansName,
                        DateOfBirth = personalDetailsDto.DateOfBirth,
                        PersonalMail = personalDetailsDto.PersonalEMail,
                        Country = personalDetailsDto.CountryID,
                        Nationality = personalDetailsDto.NationalityID,
                        CountryOfBirth = personalDetailsDto.country2ID,
                        BloodGroup = personalDetailsDto.BloodGrp,
                        Religion = personalDetailsDto.ReligionID,
                        IdentificationMark = personalDetailsDto.IdentificationMark,
                        Height = personalDetailsDto.Height,
                        Weight = personalDetailsDto.Weight,
                        WeddingDate = personalDetailsDto.WeddingDate,
                        MaritalStatus = personalDetailsDto.MaritalStatus,
                        Gender = personalDetailsDto.Gender,
                        EntryBy = personalDetailsDto.EntryBy,
                        EntryDate = DateTime.UtcNow
                    };
                    var addingData = _mapper.Map<PersonalDetailsHistory>(history);
                    await _context.PersonalDetailsHistories.AddAsync(addingData);
                }

                // Save changes if any modifications were made
                if (hasChanges)
                {
                    int result = await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return result > 0 ? _employeeSettings.DataUpdateSuccessStatus : _employeeSettings.DataInsertFailedStatus;
                }

                await transaction.CommitAsync();
                return null; // No changes were made, return null
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
            //return new PersonalDetailsHistoryDto();
        }
        public async Task<List<FillDocumentTypeDto>> FillDocumentType(int EmpID)
        {
            return await (from a in _context.HrmsDocument00s
                          join b in _context.HrmsDocTypeMasters on a.DocType equals Convert.ToInt32(b.DocTypeId)
                          join c in _context.EmpDocumentAccesses on Convert.ToInt32(a.DocId) equals c.DocId
                          where c.EmpId == EmpID
                                && !_context.HrmsEmpdocuments00s.Any(d => d.DocId == a.DocId && d.EmpId == EmpID && d.Status != _employeeSettings.Status01)
                                && a.Active == true
                                && b.DocType != _employeeSettings.Documents01
                                && b.DocType != _employeeSettings.Documents02
                                || (_context.HrmsDocument00s
                                    .Join(_context.HrmsDocTypeMasters, x => (long)x.DocType, y => y.DocTypeId, (x, y) => new { x.DocId, x.IsAllowMultiple, y.DocType, y.Code })
                                    .Any(d => d.DocId == a.DocId && d.IsAllowMultiple == 1 && d.DocType != _employeeSettings.Documents01 && d.Code != _employeeSettings.BankDocCode))
                          select new FillDocumentTypeDto
                          {
                              DocID = a.DocId,
                              DocName = a.DocName

                          }).AsNoTracking().ToListAsync();


        }
        public async Task<List<DocumentFieldDto>> DocumentField(int DocumentID)
        {
            return await (from a in _context.HrmsDocumentField00s
                          join b in _context.HrmsDatatypes on a.DataTypeId equals b.DataTypeId into datatypeGroup
                          from b in datatypeGroup.DefaultIfEmpty()
                          join c in _context.HrmsDocument00s on (long)a.DocId equals c.DocId
                          where a.DocId == DocumentID

                          select new DocumentFieldDto
                          {
                              DocFieldID = a.DocFieldId,
                              DocDescription = a.DocDescription,
                              DataTypeId = a.DataTypeId,
                              DocID = a.DocId,
                              CreatedBy = a.CreatedBy,
                              ModifiedBy = a.ModifiedBy,
                              ModifiedDate = a.ModifiedDate,
                              IsMandatory = a.IsMandatory,
                              TypeId = b.TypeId,
                              DataType = b.DataType,
                              IsDate = b.IsDate,
                              IsGeneralCategory = b.IsGeneralCategory,
                              IsDropdown = b.IsDropdown,
                              DocName = c.DocName,
                              DocType = c.DocType,
                              Active = c.Active,
                              IsExpiry = c.IsExpiry,
                              NotificationCountDays = c.NotificationCountDays,
                              FolderName = c.FolderName,
                              IsAllowMultiple = c.IsAllowMultiple,
                              IsESI = c.IsEsi,
                              IsPF = c.IsPf,
                              ShowInRecruitment = c.ShowInRecruitment
                          }).AsNoTracking().ToListAsync();

        }
        public async Task<string> InsertDocumentsFieldDetails(List<TmpDocFileUpDto> DocumentBankField, int DocumentID, int In_EntryBy)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var workFlowNeedValue = await (from a in _context.CompanyParameters
                                               join b in _context.HrmValueTypes on a.Value equals b.Value
                                               where b.Type == _employeeSettings.EmployeeReportingType && a.ParameterCode == _employeeSettings.ParameterCode02 && a.Type == _employeeSettings.CompanyParameterType
                                               select b.Code).FirstOrDefaultAsync();

                var tmpDocFileUpList = DocumentBankField;

                bool recordsExist = (from a in _context.HrmsEmpdocuments01s.ToList()
                                     join b in tmpDocFileUpList
                                     on a.DetailId equals b.DetailID
                                     select a).Any();

                if (recordsExist)
                {
                    var tmpDocFileUpListConverted = tmpDocFileUpList
                        .Select(b => new
                        {
                            DetailID = b.DetailID ?? 0,
                            DocFieldID = int.TryParse(b.DocFieldID, out int parsedId) ? parsedId : 0,
                            DocFieldText = b.DocFieldText
                        })
                        .ToList();

                    var updateRecords = (from a in _context.HrmsEmpdocuments01s.AsEnumerable() // Force client-side processing
                                         join b in tmpDocFileUpListConverted
                                         on new { DetailId = a.DetailId ?? 0, DocFields = a.DocFields ?? 0 }
                                         equals new { DetailId = b.DetailID, DocFields = b.DocFieldID }
                                         select new { a, b }).ToList();


                    foreach (var record in updateRecords)
                    {
                        record.a.DocValues = record.b.DocFieldText;
                    }

                    await _context.SaveChangesAsync();

                    var insertRecords = from b in tmpDocFileUpListConverted
                                        join a in _context.HrmsEmpdocuments01s
                                            on new { DetailID = b.DetailID, DocFields = b.DocFieldID }
                                            equals new { DetailID = a.DetailId ?? 0, DocFields = a.DocFields ?? 0 } into joined
                                        from a in joined.DefaultIfEmpty()
                                        where a == null && b.DocFieldText != null
                                        select new HrmsEmpdocuments01
                                        {
                                            DetailId = b.DetailID,
                                            DocFields = b.DocFieldID,
                                            DocValues = b.DocFieldText
                                        };

                    await _context.HrmsEmpdocuments01s.AddRangeAsync(insertRecords);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    var insertRecords = tmpDocFileUpList
                         .Where(b => b.DocFieldText != null)
                        .Select(b => new HrmsEmpdocuments01
                        {
                            DetailId = b.DetailID,
                            DocFields = int.TryParse(b.DocFieldID, out int parsedId) ? parsedId : 0,
                            DocValues = b.DocFieldText
                        })
                        .ToList();


                    await _context.HrmsEmpdocuments01s.AddRangeAsync(insertRecords);
                    await _context.SaveChangesAsync();
                }

                var detailIds = tmpDocFileUpList.Select(b => b.DetailID).ToList();

                bool recordsExistApproved = _context.HrmsEmpdocumentsApproved01s
                    .Any(a => detailIds.Contains(a.DetailId));

                if (recordsExistApproved)
                {

                    var documentHistory = _context.HrmsEmpdocumentsApproved00s
                        .Where(a => a.DetailId == DocumentID)
                        .Select(a => new HrmsEmpdocumentsHistory00
                        {
                            DetailId = a.DetailId,
                            DocApprovedId = a.DocApprovedId,
                            DocId = a.DocId,
                            EmpId = a.EmpId,
                            Status = _employeeSettings.Status04,
                            RequestId = a.RequestId,
                            DateFrom = DateTime.UtcNow,
                            EntryBy = In_EntryBy,
                            EntryDate = DateTime.UtcNow
                        }).ToList();

                    await _context.HrmsEmpdocumentsHistory00s.AddRangeAsync(documentHistory);
                    await _context.SaveChangesAsync();

                    int docHisId = documentHistory.FirstOrDefault()?.DocApprovedId ?? 0;
                    var tmpDocFileUpListConverted = tmpDocFileUpList
                        .Select(b => new
                        {
                            DetailID = b.DetailID,
                            DocFieldID = string.IsNullOrEmpty(b.DocFieldID) ? (int?)null : int.Parse(b.DocFieldID),
                            DocFieldText = b.DocFieldText
                        })
                        .ToList();


                    var historyRecords = (from a in _context.HrmsEmpdocumentsApproved01s.AsEnumerable() // Fetch records in memory
                                          join b in tmpDocFileUpListConverted
                                          on new { DetailId = (int)(a.DetailId ?? 0), DocFields = (int)(a.DocFields ?? 0) } // Explicit casting
                                          equals new { DetailId = (int)b.DetailID, DocFields = (int)b.DocFieldID } // Explicit casting
                                          where a.DocValues != b.DocFieldText
                                          select new HrmsEmpdocumentsHistory01
                                          {
                                              DocHisId = docHisId,
                                              DetailId = b.DetailID,
                                              DocFields = b.DocFieldID,
                                              DocValues = b.DocFieldText,
                                              OldDocValues = a.DocValues
                                          }).ToList();




                    await _context.HrmsEmpdocumentsHistory01s.AddRangeAsync(historyRecords);
                    await _context.SaveChangesAsync();

                    var missingHistoryRecords = (from b in tmpDocFileUpList
                                                 join a in _context.HrmsEmpdocumentsApproved01s
                                                 on new { DetailId = b.DetailID ?? 0, DocFields = int.TryParse(b.DocFieldID, out int parsedId) ? parsedId : 0 }
                                                 equals new { DetailId = a.DetailId ?? 0, DocFields = a.DocFields ?? 0 }
                                                 into joined
                                                 from a in joined.DefaultIfEmpty()
                                                 where a == null
                                                 select new HrmsEmpdocumentsHistory01
                                                 {
                                                     DocHisId = docHisId,
                                                     DetailId = b.DetailID,
                                                     DocFields = int.TryParse(b.DocFieldID, out int parsedId) ? parsedId : 0,
                                                     DocValues = b.DocFieldText,
                                                     OldDocValues = null
                                                 }).ToList();

                    await _context.HrmsEmpdocumentsHistory01s.AddRangeAsync(missingHistoryRecords);
                    await _context.SaveChangesAsync();

                    var tmpDocFileUpListConverted01 = tmpDocFileUpList
                        .Select(b => new
                        {
                            DetailID = b.DetailID ?? 0,
                            DocFieldID = string.IsNullOrEmpty(b.DocFieldID) ? 0 : int.Parse(b.DocFieldID),
                            DocFieldText = b.DocFieldText
                        })
                        .ToList();
                    var updateRecords = (from a in _context.HrmsEmpdocumentsApproved01s.AsEnumerable() // Force client-side evaluation
                                         join b in tmpDocFileUpListConverted01
                                         on new { DetailId = a.DetailId ?? 0, DocFields = a.DocFields ?? 0 }
                                         equals new { DetailId = b.DetailID, DocFields = b.DocFieldID }
                                         select new { a, b }).ToList();


                    foreach (var record in updateRecords)
                    {
                        record.a.DocValues = record.b.DocFieldText;
                    }
                    await _context.SaveChangesAsync();

                    var newRecords = (from b in tmpDocFileUpList
                                      join a in _context.HrmsEmpdocumentsApproved01s
                                      on new { DetailId = b.DetailID ?? 0, DocFields = int.TryParse(b.DocFieldID, out int parsedId) ? parsedId : 0 }
                                      equals new { DetailId = a.DetailId ?? 0, DocFields = a.DocFields ?? 0 }
                                      into joined
                                      from a in joined.DefaultIfEmpty()
                                      where a == null
                                      select new HrmsEmpdocumentsApproved01
                                      {
                                          DetailId = b.DetailID,
                                          DocFields = int.TryParse(b.DocFieldID, out int parsedId) ? parsedId : 0,
                                          DocValues = b.DocFieldText
                                      }).ToList();

                    await _context.HrmsEmpdocumentsApproved01s.AddRangeAsync(newRecords);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    if (workFlowNeedValue == _employeeSettings.No)
                    {

                        var documentHistory = _context.HrmsEmpdocumentsApproved00s
                            .Where(a => a.DetailId == DocumentID)
                            .Select(a => new HrmsEmpdocumentsHistory00
                            {
                                DetailId = a.DetailId,
                                DocApprovedId = a.DocApprovedId,
                                DocId = a.DocId,
                                EmpId = a.EmpId,
                                Status = _employeeSettings.Status05,
                                RequestId = a.RequestId,
                                DateFrom = DateTime.UtcNow,
                                EntryBy = In_EntryBy,
                                EntryDate = DateTime.UtcNow
                            }).ToList();

                        await _context.HrmsEmpdocumentsHistory00s.AddRangeAsync(documentHistory);
                        await _context.SaveChangesAsync();

                        int docHisId = documentHistory.FirstOrDefault()?.DocApprovedId ?? 0;


                        var historyInsert = tmpDocFileUpList.Select(b => new HrmsEmpdocumentsHistory01
                        {
                            DetailId = b.DetailID,
                            DocFields = int.TryParse(b.DocFieldID, out int parsedId) ? parsedId : 0,
                            DocValues = b.DocFieldText,
                            OldDocValues = null
                        }).ToList();

                        await _context.HrmsEmpdocumentsHistory01s.AddRangeAsync(historyInsert);
                        await _context.SaveChangesAsync();

                        var newEntries = tmpDocFileUpList.Select(b => new HrmsEmpdocumentsApproved01
                        {
                            DetailId = b.DetailID,
                            DocFields = int.TryParse(b.DocFieldID, out int parsedId) ? parsedId : 0,
                            DocValues = b.DocFieldText
                        }).ToList();

                        await _context.HrmsEmpdocumentsApproved01s.AddRangeAsync(newEntries);
                        await _context.SaveChangesAsync();
                    }
                }

                await transaction.CommitAsync();
                return _employeeSettings.DataInsertSuccessStatus;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return $"Error: {ex.Message}";
            }
        }
        public async Task<string> SetEmpDocuments(TmpFileUpDto DocumentBankField, int DetailID, string Status, int In_EntryBy)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var workFlowNeedValue = await (from a in _context.CompanyParameters
                                               join b in _context.HrmValueTypes on a.Value equals b.Value
                                               where b.Type == _employeeSettings.EmployeeReportingType && a.ParameterCode == _employeeSettings.ParameterCode02 && a.Type == _employeeSettings.CompanyParameterType
                                               select b.Code).FirstOrDefaultAsync();


                if ((Status == _employeeSettings.Approved || Status == _employeeSettings.EmployeeStatus) || workFlowNeedValue == _employeeSettings.No)
                {
                    int? docHisId = 0;

                    bool documentExists = await _context.HrmsEmpdocumentsApproved02s
                        .AnyAsync(d => d.DetailId == DetailID);

                    if (documentExists)
                    {
                        docHisId = await _context.HrmsEmpdocumentsHistory00s
                            .Where(h => h.DetailId == DetailID)
                            .OrderByDescending(h => h.DocHistId)
                            .Select(h => (int?)h.DocHistId)
                            .FirstOrDefaultAsync();

                        var historyEntry = await (from a in _context.HrmsEmpdocuments02s
                                                  where a.DetailId == DetailID
                                                  select new HrmsEmpdocumentsHistory02
                                                  {
                                                      DocHisId = docHisId ?? 0,
                                                      DetailId = a.DetailId,
                                                      FileName = DocumentBankField.FileName,
                                                      FileType = DocumentBankField.FileType,
                                                      FileData = DocumentBankField.FileData,
                                                      OldFileName = a.FileName
                                                  }).ToListAsync();

                        await _context.HrmsEmpdocumentsHistory02s.AddRangeAsync(historyEntry);
                    }
                    else
                    {
                        docHisId = await _context.HrmsEmpdocumentsHistory00s
                            .Where(h => h.DetailId == DetailID)
                            .OrderByDescending(h => h.DocHistId)
                            .Select(h => (int?)h.DocHistId)
                            .FirstOrDefaultAsync();

                        var historyEntry = new HrmsEmpdocumentsHistory02
                        {
                            DocHisId = docHisId ?? 0,
                            DetailId = DocumentBankField.DetailID,
                            FileName = DocumentBankField.FileName,
                            FileType = DocumentBankField.FileType,
                            FileData = DocumentBankField.FileData,
                            OldFileName = null
                        };

                        await _context.HrmsEmpdocumentsHistory02s.AddAsync(historyEntry);

                    }

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                }

                if (Status == _employeeSettings.StatusP || Status == _employeeSettings.Status02 || Status == _employeeSettings.Status06)
                {
                    var existingDocs = _context.HrmsEmpdocuments02s.Where(d => d.DetailId == DetailID);
                    _context.HrmsEmpdocuments02s.RemoveRange(existingDocs);

                    var newDocument = new HrmsEmpdocuments02
                    {
                        DetailId = DocumentBankField.DetailID,
                        FileName = DocumentBankField.FileName,
                        FileType = DocumentBankField.FileType,
                        FileData = DocumentBankField.FileData
                    };

                    await _context.HrmsEmpdocuments02s.AddAsync(newDocument);

                    if (workFlowNeedValue == _employeeSettings.No)
                    {
                        var existingApprovedDocs = _context.HrmsEmpdocumentsApproved02s.Where(d => d.DetailId == DetailID);
                        _context.HrmsEmpdocumentsApproved02s.RemoveRange(existingApprovedDocs);
                        await _context.SaveChangesAsync();

                        var newApprovedDocument = new HrmsEmpdocumentsApproved02
                        {
                            DetailId = DocumentBankField.DetailID,
                            FileName = DocumentBankField.FileName,
                            FileType = DocumentBankField.FileType,
                            FileData = DocumentBankField.FileData
                        };

                        await _context.HrmsEmpdocumentsApproved02s.AddAsync(newApprovedDocument);
                        await _context.SaveChangesAsync();
                    }
                }
                else if (Status == _employeeSettings.Approved || Status == _employeeSettings.EmployeeStatus)
                {
                    var existingDocs01 = _context.HrmsEmpdocuments02s.Where(d => d.DetailId == DetailID);
                    _context.HrmsEmpdocuments02s.RemoveRange(existingDocs01);

                    var newDocument = new HrmsEmpdocuments02
                    {
                        DetailId = DocumentBankField.DetailID,
                        FileName = DocumentBankField.FileName,
                        FileType = DocumentBankField.FileType,
                        FileData = DocumentBankField.FileData
                    };

                    await _context.HrmsEmpdocuments02s.AddAsync(newDocument);

                    var existingApprovedDocs = _context.HrmsEmpdocumentsApproved02s.Where(d => d.DetailId == DetailID);
                    _context.HrmsEmpdocumentsApproved02s.RemoveRange(existingApprovedDocs);


                    var newApprovedDocument = new HrmsEmpdocumentsApproved02
                    {
                        DetailId = DocumentBankField.DetailID,
                        FileName = DocumentBankField.FileName,
                        FileType = DocumentBankField.FileType,
                        FileData = DocumentBankField.FileData
                    };

                    await _context.HrmsEmpdocumentsApproved02s.AddAsync(newApprovedDocument);

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                }

                return _employeeSettings.DataInsertSuccessStatus;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return $"Error: {ex.Message}";
            }
        }
        public async Task<string?> CheckLetterTypeRequest(int? LetterTypeId, int? LetterSubType, int? MasterId)
        {
            var varRowCount = await (from a in _context.AssignedLetterTypes
                                     join b in _context.AssignedLetterTypes
                                     on (int)a.LetterReqId equals b.LiabilityReqId into bGroup
                                     from b in bGroup.DefaultIfEmpty()
                                     where a.LetterTypeId == LetterTypeId
                                           && a.LetterSubType == LetterSubType
                                           && a.EmpId == MasterId
                                           && (b.ApprovalStatus ?? "C") != ApprovalStatus.Approved.ToString()
                                           && !new[] { ApprovalStatus.Rejceted.ToString(), _employeeSettings.Re }.Contains(a.ApprovalStatus ?? "C")
                                           && a.IsLiability == null
                                           && a.Active == true
                                     select a).CountAsync();

            return varRowCount > 0 ? "-1" : _employeeSettings.empSystemStatus;
        }
        public async Task<string?> InsertLetterTypeRequest(LetterInsertUpdateDto dto)
        {

            if (dto.Mode == "InsertLetterTypeRequest")
            {
                var transactionId = await _context.TransactionMasters
               .Where(a => a.TransactionType == _employeeSettings.LetterConfig)
               .Select(a => a.TransactionId)
               .FirstOrDefaultAsync();
                if (!_context.AssignedLetterTypes.Any(x => x.LetterReqId == dto.MasterID))
                {

                    var sequenceData = await _context.AdmCodegenerationmasters.Where(x => x.TypeId == transactionId).Select(x => new { x.CodeId, x.LastSequence }).FirstOrDefaultAsync();
                    using (var transaction = await _context.Database.BeginTransactionAsync())
                    {
                        try
                        {
                            var assignedLetter = new AssignedLetterType
                            {
                                RequestCode = sequenceData.LastSequence,
                                LetterTypeId = dto.LetterTypeID,
                                Remark = dto.LetterType,
                                ApprovalStatus = ApprovalStatus.Pending.ToString(),
                                FlowStatus = ApprovalStatus.Pending.ToString(),
                                Active = true,
                                EmpId = dto.EmpID,
                                TemplateStyle = dto.FileName == _employeeSettings.DirectPosting ? dto.TemplateStyle : "",
                                UploadFileName = (dto.FileName == _employeeSettings.DirectPosting && !string.IsNullOrEmpty(dto.EmployeeList)) ? dto.EmployeeList : "",
                                IsLetterAttached = (dto.FileName == _employeeSettings.DirectPosting && !string.IsNullOrEmpty(dto.EmployeeList)) ? true : false,
                                CreatedBy = dto.UserID,
                                CreatedDate = DateTime.UtcNow,
                                LetterSubType = dto.LetterSubTypeID,
                                IsProxy = dto.FileName == "Proxy" ? 1 : 0,
                                Language = dto.Language,
                                IssueDate = dto.IssueDate
                            };

                            await _context.AssignedLetterTypes.AddAsync(assignedLetter);
                            await _context.SaveChangesAsync();
                            int varReturn = (int)assignedLetter.LetterReqId;

                            if (dto.LetterTypeID == 4)
                            {
                                var salaryConfirmation = new SalaryConfirmationLetterType
                                {
                                    LetterReqId = varReturn,
                                    BankName = dto.BankName,
                                    BranchName = dto.BranchName,
                                    AccountNo = dto.AccountNo,
                                    IdentificationCode = dto.IdentificationCode,
                                    Location = dto.Location
                                };

                                await _context.SalaryConfirmationLetterTypes.AddAsync(salaryConfirmation);
                            }

                            var maxCodeValue = _context.AdmCodegenerationmasters
                                .Where(c => c.CodeId == sequenceData.CodeId)
                                .Max(c => (int?)c.CurrentCodeValue) ?? 0;

                            var recordToUpdate = _context.AdmCodegenerationmasters.FirstOrDefault(c => c.CodeId == sequenceData.CodeId);
                            if (recordToUpdate != null)
                            {
                                recordToUpdate.CurrentCodeValue = maxCodeValue + 1;
                            }

                            var record = _context.AdmCodegenerationmasters
                                .Where(x => x.CodeId == sequenceData.CodeId)
                                .Select(x => new { x.NumberFormat, x.CurrentCodeValue, x.Code })
                                .FirstOrDefault();

                            if (record != null)
                            {
                                int length = record.NumberFormat.Length - record.CurrentCodeValue.ToString().Length;
                                string seq = length > 0 ? record.NumberFormat.Substring(0, length) : "";
                                string finalSequence = $"{record.Code}{seq}{record.CurrentCodeValue}";

                                var updateRecord = _context.AdmCodegenerationmasters.FirstOrDefault(x => x.CodeId == sequenceData.CodeId);
                                if (updateRecord != null)
                                {
                                    updateRecord.LastSequence = finalSequence;
                                }
                            }

                            if (dto.FileName == _employeeSettings.DirectPosting)
                            {
                                var letterWorkflowStatus = new LetterWorkflowStatus
                                {
                                    RequestId = varReturn,
                                    ShowStatus = true,
                                    ApprovalStatus = _employeeSettings.Re,
                                    Rule = 1,
                                    HierarchyType = true,
                                    Approver = dto.UserID,
                                    ApprovalRemarks = dto.LetterType,
                                    EntryBy = dto.UserID,
                                    EntryDt = DateTime.UtcNow,
                                    RuleOrder = 1,
                                    UpdatedDt = DateTime.UtcNow,
                                };

                                await _context.LetterWorkflowStatuses.AddAsync(letterWorkflowStatus);

                                var assignedLetterTypeRecord = _context.AssignedLetterTypes
                                    .FirstOrDefault(x => x.LetterReqId == varReturn &&
                                        (x.ApprovalStatus != _employeeSettings.Re || x.FlowStatus != _employeeSettings.E ||
                                         x.ReleaseDate == null || x.ReleaseDate != DateTime.UtcNow ||
                                         x.IsDirectPosting != 1 || x.FinalApprovalDate == null ||
                                         x.FinalApprovalDate != DateTime.UtcNow || x.FinalApproverId != dto.EmpID));

                                if (assignedLetterTypeRecord != null)
                                {
                                    assignedLetterTypeRecord.ApprovalStatus = _employeeSettings.Re;
                                    assignedLetterTypeRecord.FlowStatus = _employeeSettings.E;
                                    assignedLetterTypeRecord.ReleaseDate = DateTime.UtcNow;
                                    assignedLetterTypeRecord.IsDirectPosting = 1;
                                    assignedLetterTypeRecord.FinalApprovalDate = DateTime.UtcNow;
                                    assignedLetterTypeRecord.FinalApproverId = dto.UserID;
                                }

                                var emailNotificationsVarReturn = await (from a in _context.LetterWorkflowStatuses
                                                                         join b in _context.AssignedLetterTypes
                                                                         on (int)a.RequestId equals b.LetterReqId
                                                                         where b.LetterReqId == varReturn
                                                                         select new EmailNotification
                                                                         {
                                                                             InstdId = 1,
                                                                             RequestId = (int?)b.LetterReqId,
                                                                             RequestIdCode = b.RequestCode,
                                                                             RequesterEmpId = a.Approver,
                                                                             ReceiverEmpId = b.EmpId,
                                                                             TriggerDate = DateTime.UtcNow,
                                                                             TransactionId = transactionId,
                                                                             ShowStatus = Convert.ToInt32(a.ShowStatus),
                                                                             RequesterDate = DateTime.UtcNow,
                                                                             NotificationMessage = "Has Released a Letter",
                                                                             MailType = "N"
                                                                         }).ToListAsync();

                                if (emailNotificationsVarReturn.Any())
                                {
                                    _context.EmailNotifications.AddRange(emailNotificationsVarReturn);
                                }
                            }

                            await _context.SaveChangesAsync(); // Single call at the end
                            await transaction.CommitAsync();
                            return varReturn.ToString();
                        }
                        catch (Exception ex)
                        {
                            await transaction.RollbackAsync();
                            throw;
                        }
                    }


                }
                else
                {
                    var result = await (from a in _context.AssignedLetterTypes
                                        join lm1 in _context.LetterMaster01s
                                        on a.LetterSubType equals lm1.ModuleSubId into lmGroup
                                        from lm1 in lmGroup.DefaultIfEmpty()
                                        where a.LetterReqId == dto.MasterID
                                        select new
                                        {
                                            letrtypeold = a.LetterTypeId,
                                            empidold = a.EmpId,
                                            Essold = lm1 != null ? lm1.IsEss : (bool?)null
                                        }).FirstOrDefaultAsync();


                    var recordt = await _context.AssignedLetterTypes
                        .FirstOrDefaultAsync(a => a.LetterReqId == dto.MasterID);

                    if (recordt != null)
                    {
                        recordt.LetterTypeId = dto.LetterTypeID;// varLetterTypeID;
                        recordt.LetterSubType = dto.LetterSubTypeID;// varFileType;
                        recordt.Remark = dto.LetterType;// varLetterType;
                        recordt.Language = dto.Language;// varLanguage;
                        recordt.EmpId = dto.EmpID;// varEmpID;

                        // Uncomment if these fields are needed
                        // record.ModifiedBy = varUserID;
                        // record.ModifiedDate = DateTime.UtcNow;

                        await _context.SaveChangesAsync();

                        // Get the identity value (if needed)
                        var varReturnedLetterReqId = recordt.LetterReqId;

                        var Essupdatd = await (from a in _context.AssignedLetterTypes
                                               join lm1 in _context.LetterMaster01s
                                               on a.LetterSubType equals lm1.ModuleSubId
                                               where a.LetterReqId == dto.MasterID
                                               select lm1.IsEss).FirstOrDefaultAsync();

                        var letterToUpdate = _context.SalaryConfirmationLetterTypes.FirstOrDefault(l => l.LetterReqId == dto.MasterID);
                        if (result.letrtypeold == dto.LetterTypeID && result.letrtypeold == 4)
                        {

                            if (letterToUpdate != null)
                            {
                                letterToUpdate.BankName = dto.BankName;
                                letterToUpdate.BranchName = dto.BranchName;
                                letterToUpdate.AccountNo = dto.AccountNo;
                                letterToUpdate.IdentificationCode = dto.IdentificationCode;
                                letterToUpdate.Location = dto.Location;
                                // letter.LetterSubType = varFileType; // Uncomment if needed
                                await _context.SaveChangesAsync();
                            }
                        }
                        else if (result.letrtypeold != dto.LetterTypeID && result.letrtypeold == 4)
                        {

                            _context.SalaryConfirmationLetterTypes.RemoveRange(letterToUpdate);
                            // _context.SaveChanges();
                        }
                        else if (result.letrtypeold != dto.LetterTypeID && dto.LetterTypeID == 4)
                        {
                            var salaryConfirmationLetter = new SalaryConfirmationLetterType
                            {
                                LetterReqId = (int)varReturnedLetterReqId,
                                BankName = dto.BankName,
                                BranchName = dto.BranchName,
                                AccountNo = dto.AccountNo,
                                IdentificationCode = dto.IdentificationCode,
                                Location = dto.Location,
                            };

                            await _context.SalaryConfirmationLetterTypes.AddAsync(salaryConfirmationLetter);
                            await _context.SaveChangesAsync();
                        }
                        if (dto.FileName == "Proxy")
                        {
                            if (result.empidold != dto.LetterTypeID)
                            {
                                var letterWorkFlowToDelete = await _context.LetterWorkflowStatuses.FirstOrDefaultAsync(l => l.RequestId == dto.MasterID);
                                var emailNotificationToDelete = await _context.LetterWorkflowStatuses.FirstOrDefaultAsync(l => l.RequestId == dto.MasterID);
                                _context.LetterWorkflowStatuses.RemoveRange(letterWorkFlowToDelete);
                                _context.LetterWorkflowStatuses.RemoveRange(emailNotificationToDelete);
                                bool existsWorkFlow = _context.SpecialWorkFlows.Any(swf => swf.MainType == dto.LetterSubTypeID && swf.TransactionId == dto.MasterID);
                                if (existsWorkFlow)
                                {

                                }
                            }
                            else
                            {
                                if (result.Essold != Essupdatd)
                                {
                                    var requestToDelete = await _context.EmailNotifications.FirstOrDefaultAsync(l => l.RequestId == dto.MasterID);
                                    _context.EmailNotifications.RemoveRange(requestToDelete);
                                    await _context.SaveChangesAsync();

                                    var emailNotifications = from a in _context.LetterWorkflowStatuses
                                                             join b in _context.AssignedLetterTypes on Convert.ToInt64(a.RequestId) equals b.LetterReqId
                                                             join lm in _context.LetterMaster01s on new { b.LetterTypeId, LetterSubType = b.LetterSubType ?? 0 }
                                                             equals new { lm.LetterTypeId, LetterSubType = lm.ModuleSubId }
                                                                into lmJoin
                                                             from lm in lmJoin.DefaultIfEmpty()
                                                             join hr in _context.HrEmployeeUserRelations
                                                                 on b.CreatedBy equals hr.UserId into hrJoin
                                                             from hr in hrJoin.DefaultIfEmpty()
                                                             where b.LetterReqId == dto.MasterID
                                                             select new EmailNotification
                                                             {
                                                                 InstdId = 1,
                                                                 RequestId = (int?)b.LetterReqId,
                                                                 RequestIdCode = b.RequestCode,
                                                                 RequesterEmpId = lm != null && lm.IsEss == true ? b.EmpId : (hr != null ? hr.EmpId : null),
                                                                 ReceiverEmpId = a.Approver,
                                                                 TriggerDate = DateTime.UtcNow,
                                                                 TransactionId = transactionId,
                                                                 ShowStatus = Convert.ToInt32(a.ShowStatus),
                                                                 RequesterDate = DateTime.UtcNow,
                                                                 NotificationMessage = "Has Submitted A Letter Request For Approval",
                                                                 MailType = ApprovalStatus.Approved.ToString()// "A"
                                                             };

                                    _context.EmailNotifications.AddRange(emailNotifications);
                                    _context.SaveChanges();
                                }
                            }
                        }

                    }

                }

            }

            //----------
            else if (dto.Mode == "CheckLetterSubType")
            {
                var varReturn = await _context.LetterMaster01s.Where(lm => lm.LetterTypeId == dto.LetterTypeID && lm.LetterSubName == dto.LetterSubType && lm.IsActive == true).CountAsync();
                return varReturn.ToString();
            }
            else if (dto.Mode == "SubmitLetterSubType")
            {
                var existingLetter = await _context.LetterMaster01s
                    .FirstOrDefaultAsync(l => l.ModuleSubId == dto.LetterSubTypeID);

                if (existingLetter == null)
                {
                    var entity = new LetterMaster01
                    {
                        LetterSubName = dto.FileName,
                        LetterTypeId = dto.LetterTypeID,
                        IsEss = dto.IsEssApplicable,
                        CreatedBy = dto.UserID,
                        CreatedDate = DateTime.UtcNow,
                        IsSelfApprove = dto.IsSelfApprove,
                        ApproveText = dto.ApprovalText,
                        RejectText = dto.RejectText,
                        HideReject = dto.HideReject,
                        WrkFlowRoleId = dto.WrkFlowRole,
                        IsActive = true,// dto.IsActive,
                        AdjustImagePos = dto.AdjustImagePos,
                        AppointmentLetter = dto.AppointmentLetter,

                    };

                    await _context.LetterMaster01s.AddAsync(entity);
                    await _context.SaveChangesAsync(); // Save to generate the ID

                    return entity.ModuleSubId.ToString(); // Return the newly inserted ID
                }
                else
                {
                    existingLetter.LetterSubName = dto.FileName;
                    existingLetter.LetterTypeId = dto.LetterTypeID;
                    existingLetter.IsEss = dto.IsEssApplicable;
                    // existingLetter.IsActive = dto.IsActive;
                    existingLetter.ModifiedBy = dto.UserID;
                    existingLetter.ModifiedDate = DateTime.UtcNow;
                    existingLetter.IsSelfApprove = dto.IsSelfApprove;
                    existingLetter.ApproveText = dto.ApprovalText;
                    existingLetter.RejectText = dto.RejectText;
                    existingLetter.HideReject = dto.HideReject;
                    existingLetter.WrkFlowRoleId = dto.WrkFlowRole;
                    existingLetter.AdjustImagePos = dto.AdjustImagePos;
                    existingLetter.AppointmentLetter = dto.AppointmentLetter;

                    await _context.SaveChangesAsync();

                    return existingLetter.ModuleSubId.ToString(); // Return the updated ID
                }
            }

            else if (dto.Mode == "UploadbackgroundImage")
            {
                var existingLetter = await _context.LetterMaster01s
                    .FirstOrDefaultAsync(l => l.ModuleSubId == dto.LetterSubTypeID);
                if (existingLetter != null)
                {
                    existingLetter.BackgroundImage = dto.FileName;
                    await _context.SaveChangesAsync();
                }
            }
            return _employeeSettings.empSystemStatus;

            //return await Task.FromResult(GetSequence(0, transactionId, "", 0));
        }
        public async Task<string> DirectUploadLetter(List<IFormFile> files, string filePath, int masterID)
        {
            var record = await _context.AssignedLetterTypes.FirstOrDefaultAsync(x => x.LetterReqId == masterID);
            if (record == null)
                return "Record not found."; // Return an appropriate message

            var result = await _assignedLetterTypeRepository.UploadOrUpdateDocuments(
                files,
                "DirectUploadLetters",
                (file, filePath) =>
                {
                    // Update only the required fields
                    record.UploadFileName = Path.GetFileName(filePath);
                    record.IsLetterAttached = true;
                    return record; // Return the modified record
                },
                entity => ((AssignedLetterType)(object)entity).LetterReqId
            );

            return result.Message;

        }
        public async Task<object> EditEmployeeCommonInformation(string? empIds, int? employeeid)
        {
            var employeeList = empIds?.Split(',')
                                   .Select(s => int.Parse(s.Trim()))
                                   .ToList();

            var employees = await _context.EmployeeDetails.Where(e => employeeList.Contains(e.EmpId))
                            .Select(e => new
                            {
                                e.EmpId,
                                EmpName = e.EmpCode + " || " + e.Name
                            }).ToListAsync();
            var infoData = await _context.EditInfoMaster00s
                           .Where(info => info.InfoCode == _employeeSettings.companyParameterCodesType.Substring(0, _employeeSettings.companyParameterCodesType.Length - 1))
                           .Select(info => new
                           {
                               info.InfoId,
                               info.InfoCode,
                               info.Description
                           }).ToListAsync();


            var employeeDetails = await _context.EmployeeDetails.Where(e => e.EmpId == 115)
                .Join(_context.DesignationDetails, e => e.DesigId, d => d.LinkId, (e, d) => new { e, d })
                .Join(_context.BranchDetails, ed => ed.e.BranchId, b => b.LinkId, (ed, b) => new { ed.e, ed.d, b })
                .GroupJoin(_context.HrEmpImages, edb => edb.e.EmpId, img => img.EmpId, (edb, img) => new { edb, img })
                .SelectMany(edb_img => edb_img.img.DefaultIfEmpty(),
                  (edb_img, img) => new
                  {
                      edb_img.edb.e.Name,
                      Branch = edb_img.edb.d.Designation + " / " + edb_img.edb.e.EmpCode + " / " + edb_img.edb.b.Branch,
                      Image = img != null ? img.ImageUrl : null
                  }).ToListAsync();

            return new
            {
                Employees = employees,
                InfoData = infoData,
                EmployeeDetails = employeeDetails
            };
        }
        public async Task<string?> EditInformationAsync(List<TmpEmpInformation> inputs)
        {
            try
            {
                var empIds = inputs.Select(i => i.EmpID).Distinct().ToList();
                var userMasters = await _context.HrEmpMasters.Where(e => empIds.Contains(e.EmpId)).ToListAsync();
                var personalInfos = await _context.HrEmpPersonals.Where(p => empIds.Contains(p.EmpId)).ToListAsync();
                var addressInfos = await _context.HrEmpAddresses.Where(a => empIds.Contains(a.EmpId)).ToListAsync();
                var userRelations = await _context.HrEmployeeUserRelations.Where(r => empIds.Contains(r.EmpId)).ToListAsync();
                var surveyRelations = await _context.SurveyRelations.Where(sr => empIds.Contains(sr.EmpId)).ToListAsync();

                var userIds = userRelations.Select(r => r.UserId).Distinct().ToList();
                var users = await _context.AdmUserMasters.Where(u => userIds.Contains(u.UserId)).ToListAsync();

                List<EditInfoHistory> editHistoryRecords = new();

                foreach (var input in inputs)
                {
                    var userMaster = userMasters.FirstOrDefault(e => e.EmpId == input.EmpID);
                    if (userMaster != null)
                    {
                        UpdateUserMaster(userMaster, input);
                    }

                    var personalInfo = personalInfos.FirstOrDefault(p => p.EmpId == input.EmpID);
                    if (personalInfo != null)
                    {
                        UpdatePersonalInfo(personalInfo, input);
                    }

                    var addressInfo = addressInfos.FirstOrDefault(a => a.EmpId == input.EmpID);
                    if (addressInfo != null)
                    {
                        UpdateAddressInfo(addressInfo, input);
                    }

                    var userRelation = userRelations.FirstOrDefault(r => r.EmpId == input.EmpID);
                    if (userRelation != null)
                    {
                        var user = users.FirstOrDefault(u => u.UserId == userRelation.UserId);
                        if (user != null)
                        {
                            UpdateUser(user, input);
                        }
                    }

                    if (input.InfoCode.ToUpper() == _employeeSettings.ProbationEndDate)
                    {
                        foreach (var survey in surveyRelations.Where(sr => sr.EmpId == input.EmpID))
                        {
                            if (DateTime.TryParse(input.Value, out DateTime dateValue))
                            {
                                int daysToSubtract = input.probationReviewCount > 0 ? input.probationReviewCount : 30;
                                survey.ReviewDate = dateValue.AddDays(-daysToSubtract);
                            }
                        }
                    }
                    if (input.InfoCode.ToUpper() == _employeeSettings.CompanyAccomodation)// "COMPANYACCOMODATION"
                    {
                        var hraLatestRecords = await _context.HraHistories.Where(h => _context.HraHistories
                                                      .Where(inner => inner.EmployeeId == h.EmployeeId)
                                                      .OrderByDescending(inner => inner.Id)
                                                      .Select(inner => inner.Id)
                                                      .FirstOrDefault() == h.Id)
                                                      .ToListAsync();

                        foreach (var hra in hraLatestRecords)
                        {
                            hra.ToDate = DateTime.UtcNow;
                        }

                        var hraNewRecords = inputs
                                            .Where(input => input.InfoCode.ToUpper() == _employeeSettings.CompanyAccomodation)
                                            .Select(input => new HraHistory
                                            {
                                                EmployeeId = input.EmpID,
                                                IsHra = Convert.ToBoolean(input.Value),
                                                FromDate = DateTime.UtcNow,
                                                ToDate = null,
                                                Remarks = "",
                                                Entryby = input.UserId
                                            })
                                            .ToList();

                        await _context.HraHistories.AddRangeAsync(hraNewRecords);
                    }


                    editHistoryRecords.Add(new EditInfoHistory
                    {
                        EmpId = input.EmpID,
                        InfoCode = input.InfoCode,
                        InfoId = input.InfoID,
                        Info01Id = input.Info01ID,
                        Value = input.Value,
                        UpdatedBy = input.UserId,
                        UpdatedDate = DateTime.UtcNow
                    });
                }

                if (editHistoryRecords.Any())
                {
                    await _context.EditInfoHistories.AddRangeAsync(editHistoryRecords);
                }

                int result = await _context.SaveChangesAsync();
                return result > 0 ? "Updated" : "Failed";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating employee info: {ex.Message}");
                return "Failed";
            }
        }
        private void UpdateUserMaster(HrEmpMaster userMaster, TmpEmpInformation input)
        {
            switch (input.InfoCode.ToUpper())
            {
                case "EMPCODE":
                    userMaster.EmpCode = input.Value;
                    break;
                case "WEDDING_DATE":
                    userMaster.WeddingDate = Convert.ToDateTime(input.Value);
                    break;
                case "FIRSTNAME":
                    userMaster.FirstName = input.Value;
                    break;
                case "MIDDLENAME":
                    userMaster.MiddleName = input.Value;
                    break;
                case "LASTNAME":
                    userMaster.LastName = input.Value;
                    break;
                case "GUARDIANSNAME":
                    userMaster.GuardiansName = input.Value;
                    break;
                case "GENDER":
                    userMaster.Gender = input.Value;
                    break;
                case "EMPDOB":
                    userMaster.DateOfBirth = Convert.ToDateTime(input.Value);
                    break;
                case "NATIONALID":
                    userMaster.NationalIdNo = input.Value;
                    break;
                case "PASSPORTID":
                    userMaster.PassportNo = input.Value;
                    break;
                case "JOINDATE":
                    userMaster.JoinDt = Convert.ToDateTime(input.Value);
                    break;
                case "REVIEWDATE":
                    userMaster.ReviewDt = Convert.ToDateTime(input.Value);
                    break;
                case "GRATUITYDATE":
                    userMaster.GratuityStrtDate = Convert.ToDateTime(input.Value);
                    break;
                case "FIRSTENTRYDATE":
                    userMaster.FirstEntryDate = Convert.ToDateTime(input.Value);
                    break;
                case "NOTICEPERIOD":
                    userMaster.NoticePeriod = Convert.ToInt32(input.Value);
                    break;
                case "COMPANYACCOMODATION":
                    userMaster.Ishra = Convert.ToBoolean(input.Value);
                    break;
                case "EXPATRIATE":
                    userMaster.IsExpat = Convert.ToInt32(input.Value);
                    break;
                case "CASUALHOLYDAY":
                    userMaster.PublicHoliday = Convert.ToBoolean(input.Value);
                    break;
                case "COMPANYCONVEYANCE":
                    userMaster.CompanyConveyance = Convert.ToBoolean(input.Value);
                    break;
                case "COMPANYVEHICLE":
                    userMaster.CompanyVehicle = Convert.ToBoolean(input.Value);
                    break;
                case "ISPROBATION":
                    userMaster.IsProbation = Convert.ToBoolean(input.Value);
                    break;
                case "PROBATIONENDDATE":
                    userMaster.ProbationDt = Convert.ToDateTime(input.Value);
                    break;
                case "PROBATIONNOTICEPERIOD":
                    userMaster.ProbationNoticePeriod = Convert.ToInt32(input.Value);
                    break;
                case "MEALALLOWANCEDEDUCTION":
                    userMaster.MealAllowanceDeduct = Convert.ToBoolean(input.Value);
                    break;
            }
        }
        private void UpdatePersonalInfo(HrEmpPersonal personalInfo, TmpEmpInformation input)
        {
            switch (input.InfoCode.ToUpper())
            {

                case "GENDER":
                    personalInfo.Gender = input.Value;
                    break;
                case "EMPCOUNTRY":
                    personalInfo.Country = int.TryParse(input.Value, out int country) ? country : personalInfo.Country;
                    break;
                case "NATIONALITY":
                    personalInfo.Nationality = int.TryParse(input.Value, out int nationality) ? nationality : personalInfo.Nationality;
                    break;
                case "COUNTRYOFBIRTH":
                    personalInfo.CountryOfBirth = Convert.ToInt32(input.Value);
                    break;
                case "EMPLOYEE_TYPE":
                    personalInfo.EmployeeType = input.Value == "1" ? "D" : input.Value == "2" ? "H" : personalInfo.EmployeeType;
                    break;
                case "MARITAL":
                    personalInfo.MaritalStatus = input.Value;
                    break;
                case "BLOODGROUP":

                    if (int.TryParse(input.Value, out int bloodGroupValue))
                    {
                        var bloodGroup = _context.HrmValueTypes
                            .Where(v => v.Type == "BloodGroup" && v.Value == bloodGroupValue)
                            .Select(v => v.Code)
                            .FirstOrDefault();

                        personalInfo.BloodGrp = bloodGroup ?? personalInfo.BloodGrp;
                    }
                    else
                    {
                        personalInfo.BloodGrp = input.Value;
                    }
                    break;
                case "WEDDING_DATE":
                    personalInfo.WeddingDate = Convert.ToDateTime(input.Value);
                    break;
                case "RELIGION":
                    personalInfo.Religion = Convert.ToInt32(input.Value);
                    break;
                case "HEIGHT":
                    personalInfo.Height = input.Value;
                    break;
                case "WEIGHT":
                    personalInfo.Weight = input.Value;
                    break;
                case "IDENTIFICATION":
                    personalInfo.IdentMark = input.Value;
                    break;
            }
        }
        private void UpdateAddressInfo(HrEmpAddress addressInfo, TmpEmpInformation input)
        {
            switch (input.InfoCode.ToUpper())
            {
                case "COMPANYMAIL":
                    addressInfo.OfficialEmail = input.Value;
                    break;
                case "PERSONALMAIL":
                    addressInfo.PersonalEmail = input.Value;
                    break;
                case "PERSONALMOBILE":
                    addressInfo.Mobile = input.Value;
                    break;
                case "OFFICIALMOBILE":
                    addressInfo.Phone = input.Value;
                    break;
                case "HOMECOUNTRYPHONENO":
                    addressInfo.HomeCountryPhone = input.Value;
                    break;
            }
        }
        private void UpdateUser(AdmUserMaster user, TmpEmpInformation input)
        {
            switch (input.InfoCode.ToUpper())
            {
                case "COMPANYMAIL":
                    user.Email = input.Value;
                    break;
                case "EMPCODE":
                    user.UserName = input.Value;
                    break;
                case "APPNEEDED":
                    user.NeedApp = Convert.ToBoolean(input.Value);
                    break;
            }
        }
        public async Task<object> GetInformationDescriptionAsync(int infoId)
        {
            return await _context.EditInfoMaster01s
                 .Where(e => e.InfoId == infoId)
                 .Select(e => new
                 {
                     e.Info01Id,
                     e.InfoId,
                     e.InfoCode,
                     e.Description
                 }).ToListAsync();
        }
        public async Task<object> GetLetterTypeAsync()
        {
            return await _context.LetterMaster00s.Where(x => x.Active == true).Select(x => new { x.LetterTypeId, x.LetterTypeName }).ToListAsync();
        }
        public async Task<object> LetterSignatureAuthorityAsync()
        {
            return await _context.Categorymasterparameters
               .Select(x => new
               {
                   x.ParameterId,
                   x.ParamDescription
               }).ToListAsync();
        }
        public async Task<string> DeleteDesciplinaryLetter(string? masterId)
        {
            var transactionID = await _context.TransactionMasters
                .Where(tm => tm.TransactionType == _employeeSettings.LetterConfig)
                .Select(tm => tm.TransactionId)
                .FirstOrDefaultAsync();

            if (transactionID != null)
            {
                var masterIds = masterId?.Split(',').Select(int.Parse).ToList();
                if (masterIds == null || !masterIds.Any()) return "-1"; // No valid IDs to process

                // Fetch and update EMAIL_NOTIFICATION
                var emailNotifications = await _context.EmailNotifications
                    .Where(en => masterIds.Contains((int)en.RequestId) && en.TransactionId == transactionID)
                    .ToListAsync();

                var emailUpdates = emailNotifications.Count;
                emailNotifications.ForEach(en => en.ShowStatus = 2);

                // Fetch and update AssignedLetterType
                var assignedLetterTypes = await _context.AssignedLetterTypes
                    .Where(alt => masterIds.Contains((int)alt.LetterReqId))
                    .ToListAsync();

                var letterUpdates = assignedLetterTypes.Count;
                assignedLetterTypes.ForEach(alt => alt.ApprovalStatus = ApprovalStatus.Deleted.ToString());

                // If no records were updated, return -1
                if (emailUpdates == 0 && letterUpdates == 0) return "-1";

                // Save changes to the database
                await _context.SaveChangesAsync();

                return "1";
            }

            return "-1"; // No valid transaction ID found
        }
        public async Task<LoadCompanyDetailsResultDto> LoadCompanyDetailsAsync(LoadCompanyDetailsRequestDto loadCompanyDetailsRequestDto)
        {
            DateTime? offsetDateApp = null;

            // Fetch TransactionID
            int? transactionId = await _context.TransactionMasters
                .Where(t => t.TransactionType == loadCompanyDetailsRequestDto.Code)
                .Select(t => (int?)t.TransactionId)
                .FirstOrDefaultAsync();

            // Check Access Rights
            bool hasAccess = await _context.TabAccessRights
                .AnyAsync(a => _context.TabMasters
                    .Any(t => t.Code == "SelfAssgnShft" && t.TabId == a.TabId) &&
                    a.RoleId == loadCompanyDetailsRequestDto.FirstEntityId);

            // Fetch Link Level
            int? linkSelect = await _context.SpecialAccessRights
                .Where(s => s.RoleId == loadCompanyDetailsRequestDto.FirstEntityId)
                .Select(s => s.LinkLevel)
                .FirstOrDefaultAsync();

            int? entityAccess = await _context.EntityAccessRights02s
                .Where(e => e.RoleId == loadCompanyDetailsRequestDto.FirstEntityId)
                .OrderBy(e => e.LinkLevel)
                .Select(e => e.LinkLevel)
                .FirstOrDefaultAsync();

            // Fetch High-Level Data
            //var highLevelData = await _context.HighLevelViewTables
            //    .AsNoTracking()
            //    .Select(h => new HighLevelTableDto
            //    {
            //        LevelOneId = h.LevelOneId,
            //        LevelOneDescription = h.LevelOneDescription,
            //        LevelTwoId = h.LevelTwoId,
            //        LevelTwoDescription = h.LevelTwoDescription + "(" + h.LevelOneDescription + ")",
            //        LevelThreeId = h.LevelThreeId,
            //        LevelThreeDescription = h.LevelThreeDescription + "(" + h.LevelOneDescription + "-" + h.LevelTwoDescription + ")",
            //        LevelFourId = h.LevelFourId,
            //        LevelFourDescription = h.LevelFourDescription + "(" + h.LevelThreeDescription + ")"
            //    })
            //    .ToListAsync();
            var highLevelData = await GetAccessLevel();

            // Determine final link selection
            int finalLinkSelect = linkSelect ?? entityAccess ?? 13;

            return new LoadCompanyDetailsResultDto
            {
                HighLevelData = highLevelData,
                LinkSelect = finalLinkSelect
            };
        }

        //private async Task<List<HighLevelTableDto>> GetAccessLevel()
        //{
        //    // Step 1: Fetch raw data from the DB
        //    var rawData = await _context.HighLevelViewTables
        //        .AsNoTracking()
        //        .ToListAsync();

        //    // Step 2: Format it in memory using LINQ
        //    var result = rawData.Select(h => new HighLevelTableDto
        //    {
        //        LevelOneId = h.LevelOneId,
        //        LevelOneDescription = h.LevelOneDescription,

        //        LevelTwoId = h.LevelTwoId,
        //        LevelTwoDescription = $"{h.LevelTwoDescription} ({h.LevelOneDescription})",

        //        LevelThreeId = h.LevelThreeId,
        //        LevelThreeDescription = $"{h.LevelThreeDescription} ({h.LevelOneDescription}-{h.LevelTwoDescription})",

        //        LevelFourId = h.LevelFourId,
        //        LevelFourDescription = $"{h.LevelFourDescription} ({h.LevelThreeDescription})",

        //        LevelFiveId = h.LevelFiveId,
        //        LevelFiveDescription = $"{h.LevelFiveDescription} ({h.LevelThreeDescription}-{h.LevelFourDescription})",

        //        LevelSixId = h.LevelSixId,
        //        LevelSixDescription = $"{h.LevelSixDescription} ({h.LevelFourDescription}-{h.LevelFiveDescription})",

        //        LevelSevenId = h.LevelSevenId,
        //        LevelSevenDescription = $"{h.LevelSevenDescription} ({h.LevelThreeDescription}-{h.LevelSixDescription})",

        //        LevelEightId = h.LevelEightId,
        //        LevelEightDescription = $"{h.LevelEightDescription} ({h.LevelThreeDescription}-{h.LevelSevenDescription})",

        //        LevelNineId = h.LevelNineId,
        //        LevelNineDescription = $"{h.LevelNineDescription} ({h.LevelSevenDescription}-{h.LevelEightDescription})",

        //        LevelTenId = h.LevelTenId,
        //        LevelTenDescription = $"{h.LevelTenDescription} ({h.LevelEightDescription}-{h.LevelNineDescription})",

        //        LevelElevenId = h.LevelElevenId,
        //        LevelElevenDescription = $"{h.LevelElevenDescription} ({h.LevelNineDescription}-{h.LevelTenDescription})"

        //    }).ToList();

        //    return result;
        //    //return new List<HighLevelTableDto>();
        //}


        public async Task<List<HighLevelTableDto>> GetAccessLevel()
        {
            // Step 1: Fetch raw data from the DB
            var rawData = await _context.HighLevelViewTables.AsNoTracking().ToListAsync();
            var result = _mapper.Map<List<HighLevelTableDto>>(rawData);
            return result;
        }





        //private async Task<List<HighLevelTableDto>> GetAccessLevel()
        //{
        //    return await _context.HighLevelViewTables
        //     .AsNoTracking()
        //     .Select(h => new HighLevelTableDto
        //     {
        //         LevelOneId = h.LevelOneId,
        //         LevelOneDescription = h.LevelOneDescription,

        //         LevelTwoId = h.LevelTwoId,
        //         LevelTwoDescription = h.LevelTwoDescription + " (" + h.LevelOneDescription + ")",

        //         LevelThreeId = h.LevelThreeId,
        //         LevelThreeDescription = h.LevelThreeDescription + " (" + h.LevelOneDescription + "-" + h.LevelTwoDescription + ")",

        //         LevelFourId = h.LevelFourId,
        //         LevelFourDescription = h.LevelFourDescription + " (" + h.LevelThreeDescription + ")",

        //         LevelFiveId = h.LevelFiveId,
        //         LevelFiveDescription = h.LevelFiveDescription + " (" + h.LevelThreeDescription + "-" + h.LevelFourDescription + ")",

        //         LevelSixId = h.LevelSixId,
        //         LevelSixDescription = h.LevelSixDescription + " (" + h.LevelFourDescription + "-" + h.LevelFiveDescription + ")",

        //         LevelSevenId = h.LevelSevenId,
        //         LevelSevenDescription = h.LevelSevenDescription + " (" + h.LevelThreeDescription + "-" + h.LevelSixDescription + ")",

        //         LevelEightId = h.LevelEightId,
        //         LevelEightDescription = h.LevelEightDescription + " (" + h.LevelThreeDescription + "-" + h.LevelSevenDescription + ")",

        //         LevelNineId = h.LevelNineId,
        //         LevelNineDescription = h.LevelNineDescription + " (" + h.LevelSevenDescription + "-" + h.LevelEightDescription + ")",

        //         LevelTenId = h.LevelTenId,
        //         LevelTenDescription = h.LevelTenDescription + " (" + h.LevelEightDescription + "-" + h.LevelNineDescription + ")",

        //         LevelElevenId = h.LevelElevenId,
        //         LevelElevenDescription = h.LevelElevenDescription + " (" + h.LevelNineDescription + "-" + h.LevelTenDescription + ")"




        //     })
        //     .ToListAsync();
        //}
        public async Task<object> GetLevelAsync(int level)
        {
            var query = _context.HighLevelViewTables.AsQueryable();

            switch (level)
            {
                case 1:
                    return await query.Select(h => new
                    {
                        LevelOneId = h.LevelOneId,
                        LevelOneDescription = h.LevelOneDescription
                    }).Distinct().ToListAsync();

                case 2:
                    return await query
                        .Select(h => new
                        {
                            LevelTwoId = h.LevelTwoId,
                            LevelTwoDescription = $"{h.LevelTwoDescription}({h.LevelOneDescription})"
                        }).Distinct().ToListAsync();

                case 3:
                    return await query
                        .Select(h => new
                        {
                            LevelThreeId = h.LevelThreeId,
                            LevelThreeDescription = $"{h.LevelThreeDescription}({h.LevelOneDescription}-{h.LevelTwoDescription})"
                        }).Distinct().ToListAsync();

                case 4:
                    return await query
                        .Select(h => new
                        {
                            LevelFourId = h.LevelFourId,
                            LevelFourDescription = $"{h.LevelFourDescription}({h.LevelThreeDescription})"
                        }).Distinct().ToListAsync();
                case 5:
                    return await query
                        .Select(h => new
                        {
                            LevelFiveId = h.LevelFiveId,
                            LevelFiveDescription = $"{h.LevelFiveDescription}({h.LevelThreeDescription}-{h.LevelFourDescription})"
                        }).Distinct().ToListAsync();
                case 6:
                    return await query
                        .Select(h => new
                        {
                            LevelSixId = h.LevelSixId,
                            LevelFiveDescription = $"{h.LevelSixDescription}({h.LevelFourDescription}-{h.LevelFiveDescription})"
                        }).Distinct().ToListAsync();
                case 7:
                    return await query
                        .Select(h => new
                        {
                            LevelSevenId = h.LevelSevenId,
                            LevelSevenDescription = $"{h.LevelSevenDescription}({h.LevelThreeDescription}-{h.LevelSixDescription})"
                        }).Distinct().ToListAsync();
                case 8:
                    return await query
                        .Select(h => new
                        {
                            LevelEightId = h.LevelEightId,
                            LevelEightDescription = $"{h.LevelEightDescription}({h.LevelThreeDescription}-{h.LevelSevenDescription})"
                        }).Distinct().ToListAsync();
                case 9:
                    return await query
                        .Select(h => new
                        {
                            LevelNineId = h.LevelNineId,
                            LevelNineDescription = $"{h.LevelNineDescription}({h.LevelSevenDescription}-{h.LevelEightDescription})"
                        }).Distinct().ToListAsync();
                case 10:
                    return await query
                        .Select(h => new
                        {
                            LevelTenId = h.LevelTenId,
                            LevelTenDescription = $"{h.LevelTenDescription}({h.LevelEightDescription}-{h.LevelNineDescription})"
                        }).Distinct().ToListAsync();
                case 11:
                    return await query
                        .Select(h => new
                        {
                            LevelElevenId = h.LevelElevenId,
                            LevelElevenDescription = $"{h.LevelElevenDescription}({h.LevelNineDescription}-{h.LevelTenDescription})"
                        }).Distinct().ToListAsync();

                default:
                    return new List<object>();
            }
        }
        public async Task<object> GetAllLetterType()
        {
            return await (from lm1 in _context.LetterMaster01s
                          join lm0 in _context.LetterMaster00s
                          on lm1.LetterTypeId equals lm0.LetterTypeId
                          where lm0.Active == true && lm1.IsActive == true
                          orderby lm1.CreatedDate descending
                          select new
                          {
                              lm0.LetterCode,
                              lm0.LetterTypeId,
                              lm0.LetterTypeName,
                              lm1.ModuleSubId,
                              lm1.LetterSubName,
                              lm1.IsEss,
                              lm1.CreatedDate
                          }).ToListAsync();
        }
        public async Task<LetterMaster01Dto> GetLetterSubTypeByIdAsync(int LetterSubTypeID)
        {
            return await _context.LetterMaster01s
                        .Where(l => l.ModuleSubId == LetterSubTypeID && l.IsActive == true)
                        .Select(l => new LetterMaster01Dto
                        {
                            ModuleSubId = l.ModuleSubId,
                            LetterSubName = l.LetterSubName,
                            LetterTypeId = l.LetterTypeId,
                            IsEss = l.IsEss,
                            CreatedBy = l.CreatedBy,
                            CreatedDate = l.CreatedDate,
                            IsActive = l.IsActive,
                            ModifiedBy = l.ModifiedBy,
                            ModifiedDate = l.ModifiedDate,
                            BackGroundImage = l.BackgroundImage,
                            IsSelfApprove = l.IsSelfApprove,
                            ApproveText = l.ApproveText ?? string.Empty,
                            RejectText = l.RejectText ?? string.Empty,
                            HideReject = l.HideReject ?? 0,
                            WrkFlowRoleId = l.WrkFlowRoleId ?? 0,
                            AdjustImagePos = l.AdjustImagePos ?? 0,
                            AppointmentLetter = l.AppointmentLetter ?? 0
                        }).FirstAsync();
        }
        private async Task<int> GetTransactionIdByTransactionType(string transactionType)
        {
            return await _context.TransactionMasters.Where(a => a.TransactionType == transactionType).Select(a => a.TransactionId).FirstOrDefaultAsync();
        }
        private async Task<bool> GetExistsRoleAsync(int roleId)
        {
            return await _context.AdmRoleMasters.AnyAsync(a => a.Type == "A" && a.RoleId == roleId);
        }
        public async Task<object> GetUserRoles(RoleDetailsDto RoleDetailsDtos)
        {
            // Fetch transactionId & roleId if required (not used in current logic)
            var transactionId = await GetTransactionIdByTransactionType(RoleDetailsDtos.TransactionType);
            var roleId = await GetLinkLevelByRoleId(RoleDetailsDtos.FirstEntityId);

            // Check if role exists
            bool roleExists = await GetExistsRoleAsync(RoleDetailsDtos.FirstEntityId);

            // Build base query
            var query = from a in _context.AdmRoleMasters
                        join b in _context.UserTypes on a.UserTypeId equals b.Id into userTypeGroup
                        from b in userTypeGroup.DefaultIfEmpty() // Left Join
                        select new
                        {
                            RoleId = a.RoleId,
                            RoleName = a.RoleName,
                            RoleCode = a.RoleCode,
                            UserType = b.Description ?? "NA",
                            Type = a.Type,
                            Code = b.Code
                        };

            // Apply additional conditions if role does not exist
            if (!roleExists)
            {
                //query = query.Where(a => a.RoleId != null && a.RoleCode != "A"); // Adjust filter logic as needed
                query = query.Where(a => a.Type != "A" && a.Code != "A"); // Adjust filter logic as needed
            }

            // Execute query asynchronously
            var result = await query.ToListAsync();

            return result;
        }


        public async Task<object> GetUserRoles(int? firstEntityId, int? secondEntityId)
        {
            int empId = 269;
            var transId = await GetTransactionIdByTransactionType("Role");
            var workflowTransId = await _context.TransactionMasters
                .Where(t => t.TransactionType == "W_Flow")
                .Select(t => t.TransactionId)
                .FirstOrDefaultAsync();

            int? lnklev = await _context.SpecialAccessRights
                .Where(s => s.RoleId == firstEntityId)
                .Select(s => s.LinkLevel)
                .FirstOrDefaultAsync();

            bool hasAccess = await _context.EntityAccessRights02s
                .AnyAsync(s => s.RoleId == firstEntityId && s.LinkLevel == 15);

            if (hasAccess)
            {
                var roles = await _context.AdmRoleMasters.Join(_context.UserTypes, r => r.UserTypeId, u => u.Id,
                    (r, u) => new
                    {
                        RoleId = r.RoleId,
                        RoleName = r.RoleName,
                        RoleCode = r.RoleCode,
                        UserType = u.Description ?? "NA",
                        Code = u.Code,
                        Type = r.Type
                    }).ToListAsync();

                return roles.Where(a => a.Type != "A" && a.Code != "A").ToList();
            }

            var empEntity = await _context.HrEmpMasters
                .Where(h => h.EmpId == secondEntityId)
                .Select(h => h.EmpEntity)
                .FirstOrDefaultAsync();

            var empEntities = SplitStrings_XML(empEntity, ',')
                .Select((item, index) => new { Item = item, LinkLevelSelf = index + 2 })
                .Where(e => !string.IsNullOrEmpty(e.Item));

            var applicableFinal = await _context.EntityAccessRights02s
                .Where(s => s.RoleId == firstEntityId)
                .SelectMany(s => SplitStrings_XML(s.LinkId, default),
                    (s, item) => new { Item = item, LinkLevel = s.LinkLevel })
                .ToListAsync();

            if (lnklev > 0)
            {
                //applicableFinal.AddRange(empEntities
                //    .Where(e => e.LinkLevelSelf >= lnklev)
                //    .Select(e => new { e.Item, inLinkLevel = e.LinkLevelSelf }));
                applicableFinal.AddRange(empEntities.Where(e => e.LinkLevelSelf >= lnklev).Select(e => new { e.Item, LinkLevel = (int?)e.LinkLevelSelf })); // Explicitly casting to int?


            }

            var applicableFinalSet = applicableFinal.Select(a => (long?)Convert.ToInt64(a.Item)).ToHashSet();

            var entityApplicable00Final = await _context.EntityApplicable00s
                .Where(e => e.TransactionId == transId)
                .Select(e => new { e.LinkId, e.LinkLevel, e.MasterId })
                .ToListAsync();

            var applicableFinal02Emp = await (
                from emp in _context.EmployeeDetails
                join ea in _context.EntityApplicable01s on emp.EmpId equals ea.EmpId
                join hlv in _context.HighLevelViewTables on emp.LastEntity equals hlv.LastEntityId
                where ea.TransactionId == transId && applicableFinalSet.Contains(hlv.LevelOneId)
                select ea.MasterId
            ).Distinct().ToListAsync();

            var newHigh = entityApplicable00Final
                .Where(e => applicableFinalSet.Contains(e.LinkId) || e.LinkLevel == 15)
                .Select(e => e.MasterId)
                .Union(applicableFinal02Emp)
                .Distinct()
                .ToList();

            var finalRoles = await _context.AdmRoleMasters
                .Join(_context.UserTypes, a => a.UserTypeId, b => b.Id, (a, b) => new { a, b })
                .Join(newHigh, x => x.a.RoleId, e => e, (x, e) => new { x.a, x.b })
                .Where(x => x.a.Type != "A" && x.b.Code != "A" && x.a.RoleId != firstEntityId && x.b.Id != 5)
                .Select(x => new
                {
                    x.a.RoleId,
                    x.a.RoleName,
                    x.a.RoleCode,
                    UserType = x.b.Description ?? "NA"
                }).ToListAsync();

            var additionalRoles = await _context.HrEmployeeUserRelations
                .Where(a => a.EmpId == empId)
                .Join(_context.AdmUserRoleMasters, a => a.UserId, c => c.UserId, (a, c) => new { a, c })
                .Join(_context.AdmRoleMasters, ac => ac.c.RoleId, d => d.RoleId, (ac, d) => new { ac, d })
                .Join(_context.UserTypes, d => d.d.UserTypeId, e => e.Id, (d, e) => new { d, e })
                .Select(x => new
                {
                    x.d.d.RoleId,
                    x.d.d.RoleName,
                    x.d.d.RoleCode,
                    UserType = x.e.Description ?? "NA"
                }).ToListAsync();

            return finalRoles.Union(additionalRoles).ToList();
        }
        public async Task<(string EmployeeStatuses, string SystemStatuses)> GetEmployeeAndSystemStatusesAsync(int empId)
        {
            // Fetch WorkerInESS value
            var workerInESS = await GetDefaultCompanyParameter(empId, "EnableWorkerInEs", _employeeSettings.companyParameterCodesType);//"EMP1"

            // Fetch Employee Statuses
            var excludedStatuses = new HashSet<string> { "R", "D", "A", "X", "O" };
            if (workerInESS == "0")
            {
                excludedStatuses.Add("W");
            }

            var employeeStatuses = await _context.HrEmpStatusSettings
                .Where(s => !excludedStatuses.Contains(s.Status))
                .Select(s => $"<option value={s.StatusId}>{s.StatusDesc}</option>")
                .ToListAsync();

            // Fetch System Statuses
            var employeeCurrentStatuses = await _context.HrEmpMasters
                .Where(e => e.EmpId == empId)
                .Select(e => e.CurrentStatus)
                .Union(new int?[] { 7 }) // Including '7' as per the SQL UNION
                .ToListAsync();

            var systemStatuses = await _context.EmployeeCurrentStatuses
                .Where(s => employeeCurrentStatuses.Contains(s.Status))
                .Select(s => $"<option value={s.Status}>{s.StatusDesc}</option>")
                .ToListAsync();

            // Return concatenated results
            return (string.Join("", employeeStatuses), string.Join("", systemStatuses));
        }
        public async Task<object> GetReligionsAsync()
        {
            var religions = await _context.AdmReligionMasters
                .Select(r => $"<option value={r.ReligionId}>{r.ReligionName}</option>")
                .ToListAsync();

            return new
            {
                Religion_Name = string.Join("", religions) // Assign concatenated string under column name
            };

        }
        public async Task<object> GetEmployeeMasterHeaderDataAsync()
        {

            return await _context.EmployeeFieldMaster00s.OrderBy(f => f.FieldMaster00Id)
                        .Select(f => new
                        {
                            f.FieldMaster00Id,
                            f.FieldCode,
                            f.FieldDescription
                        }).ToListAsync();
        }
        public async Task<object> GetCategoryMasterDetailsAsync(int roleId)
        {

            var categoryMaster = await _context.Categorymasters
                 .Join(_context.HrmValueTypes
                         .Where(b => b.Type == typeof(CatTrxType).Name), // Filter BEFORE join
                       a => a.CatTrxTypeId,
                       b => b.Value,
                       (a, b) => new
                       {
                           SortOrder = a.SortOrder,
                           Description = a.Description,
                           Value = b.Value,
                           Code = b.Code.Trim()
                       })
                 .ToListAsync();

            var linkLevel = await GetLinkLevelByRoleId(roleId);

            return new
            {
                CategoryMaster = categoryMaster,
                LinkLevel = linkLevel
            };

        }
        public async Task<object> GetEmployeeMasterHeaderEditDataAsync()
        {
            var excludedCodes = new[] { "ASSIGN", "PAYSCALE" };
            return await _context.EmployeeFieldMaster00s
                                .Where(f => !excludedCodes.Contains(f.FieldCode))
                                .OrderBy(f => f.FieldMaster00Id)
                                .Select(f => new
                                {
                                    f.FieldMaster00Id,
                                    f.FieldCode,
                                    f.FieldDescription
                                }).ToListAsync();
        }
        //public async Task<object> GetFieldsToHideAsync()
        //{
        //    return await _context.CompanyParameters
        //                     .Where(cp => cp.ParameterCode == "HIDEEMPCREATFLD" && cp.Type == _employeeSettings.companyParameterCodesType)
        //                     .Select(cp => (int?)cp.Value)  // Convert to nullable to handle NULL cases
        //                     .FirstOrDefaultAsync() ?? 0;
        //}
        public async Task<object> GetFieldsToHideAsync()
        {
            var parameterVal = await _context.CompanyParameters
                                  .Where(cp => cp.ParameterCode == "HIDEEMPCREATFLD" && cp.Type == _employeeSettings.companyParameterCodesType)
                                  .Select(cp => (int?)cp.Value)  // Convert to nullable to handle NULL cases
                                  .FirstOrDefaultAsync() ?? 0;

            List<string> FieldCode = new List<string>(); // Ensure FieldCode is always initialized

            if (parameterVal == 1)
            {
                FieldCode = await _context.EmployeeFieldMaster01s
                              .Where(a => a.Visibility == 0 && a.FieldMaster00Id == 1)
                              .Select(a => a.FieldCode)
                              .ToListAsync();
            }

            return FieldCode;
        }
        public async Task<object> EmployeeCreationFilterAsync()
        {

            int linkselect = 0;  // Replace with actual value
            string lnk = "";
            var lnkList = await _context.Categorymasters
               .Where(c => c.SortOrder >= linkselect || linkselect == 15)
               .Select(c => c.SortOrder.ToString()) // Convert SortOrder to string
               .ToListAsync();

            // Concatenating SortOrder values into a single string
            lnk = string.Join(",", lnkList) + ",13";
            var accessLevels = await GetAccessLevel();
            return new
            {
                LinkData = lnk,
                AccessLevels = accessLevels
            };
        }
        public async Task<object> EmployeeCreationFilterAsync(int? firstEntityId)
        {
            var linkselect = await _context.EntityAccessRights02s
                .Where(e => e.RoleId == firstEntityId)
                .OrderBy(e => e.LinkLevel)
                .Select(e => e.LinkLevel)
                .FirstOrDefaultAsync();


            // Replace with actual value
            string lnk = "";
            var lnkList = await _context.Categorymasters
               .Where(c => c.SortOrder >= linkselect || linkselect == 15)
               .Select(c => c.SortOrder.ToString()) // Convert SortOrder to string
               .ToListAsync();

            // Concatenating SortOrder values into a single string
            lnk = string.Join(",", lnkList) + ",13";
            var accessLevels = await GetAccessLevel();
            return new
            {
                LinkData = lnk,
                AccessLevels = accessLevels
            };
        }
        public async Task<IEnumerable<DependentDto1>> GetDependentsByEmpId(int empId)
        {
            return await (from a in _context.Dependent00s
                          join b in _context.DependentMasters on a.RelationshipId equals b.DependentId
                          where a.DepEmpId == empId
                          select new DependentDto1
                          {
                              DepId = a.DepId,
                              Name = a.Name,
                              Description = b.Description,
                              DateOfBirth = a.DateOfBirth.HasValue ? a.DateOfBirth.Value.ToString("dd/MM/yyyy") : "0",
                              InterEmpId = a.InterEmpId ?? 0,
                              Type = a.Type,
                              Phone = a.Description,
                              Gender = a.Gender == "M" ? "Male" : (a.Gender == "O" ? "Others" : "Female")
                          }).ToListAsync();
        }
        public async Task<List<DailyRatePolicyDto>> GetDailyRatePoliciesAsync()
        {
            return await _context.DailyRatePolicy00s
                        .Select(d => new DailyRatePolicyDto
                        {
                            RateId = d.RateId,
                            Name = d.Name
                        }).ToListAsync();
        }
        public async Task<object> GetWageTypesWithRatesAsync()
        {
            var wageTypes = new List<object>
                            {
                                new { ID = 1, Description = "Monthly wage" },
                                new { ID = 2, Description = "Daily wage" },
                                new { ID = 3, Description = "Hourly Rate Fixed" },
                                new { ID = 4, Description = "Hourly Rate Based On Month" }
                            };

            var dailyRatePolicies = await _context.DailyRatePolicy00s
                                    .Select(d => new
                                    {
                                        RateId = d.RateId,
                                        Name = d.Name
                                    }).ToListAsync();

            return new
            {
                WageTypes = wageTypes,
                DailyRatePolicies = dailyRatePolicies
            };


        }
        public async Task<int> IsEnableWeddingDate(int empId)
        {
            var weddingDateEnable = await GetDefaultCompanyParameter(empId, "ENABLEWEDDINGDATE", _employeeSettings.companyParameterCodesType);//"EMP1"
            return int.Parse(weddingDateEnable);

        }


        public async Task<object> GetEmployeePersonalDetails(int empId)
        {

            var designationDetails = (from a in _context.Categorymasters
                                      join b in _context.SubCategoryLinksNews
                                      on new { SortOrder = (long?)a.SortOrder, CatTrxTypeId = a.CatTrxTypeId.ToString() }
                                          equals new { SortOrder = (long?)b.LinkLevel, CatTrxTypeId = "4" }
                                      join c in _context.Subcategories
                                      on b.SubcategoryId equals c.SubEntityId
                                      select new
                                      {
                                          LinkID = b.LinkId,
                                          Designation = c.Description
                                      }).AsEnumerable();

            var gradeDetails = (from a in _context.Categorymasters
                                join b in _context.SubCategoryLinksNews
                                on new { SortOrder = (long?)a.SortOrder, CatTrxTypeId = a.CatTrxTypeId.ToString() }
                                    equals new { SortOrder = (long?)b.LinkLevel, CatTrxTypeId = "6" }
                                join c in _context.Subcategories
                                on b.SubcategoryId equals c.SubEntityId
                                select new
                                {
                                    LinkID = b.LinkId,
                                    Designation = c.Description
                                }).AsEnumerable();

            var bandDetails = (from a in _context.Categorymasters
                               join b in _context.SubCategoryLinksNews
                               on new { SortOrder = (long?)a.SortOrder, CatTrxTypeId = a.CatTrxTypeId.ToString() }
                                   equals new { SortOrder = (long?)b.LinkLevel, CatTrxTypeId = "5" }
                               join c in _context.Subcategories
                               on b.SubcategoryId equals c.SubEntityId
                               select new
                               {
                                   LinkID = b.LinkId,
                                   Designation = c.Description
                               }).AsEnumerable();

            var branchDetails = (from a in _context.Categorymasters
                                 join b in _context.SubCategoryLinksNews
                                 on new { SortOrder = (long?)a.SortOrder, CatTrxTypeId = a.CatTrxTypeId.ToString() }
                                     equals new { SortOrder = (long?)b.LinkLevel, CatTrxTypeId = "2" }
                                 join c in _context.Subcategories
                                 on b.SubcategoryId equals c.SubEntityId
                                 select new
                                 {
                                     LinkID = b.LinkId,
                                     Designation = c.Description
                                 }).AsEnumerable();

            var departmentDetails = (from a in _context.Categorymasters
                                     join b in _context.SubCategoryLinksNews
                                     on new { SortOrder = (long?)a.SortOrder, CatTrxTypeId = a.CatTrxTypeId.ToString() }
                                         equals new { SortOrder = (long?)b.LinkLevel, CatTrxTypeId = "3" }
                                     join c in _context.Subcategories
                                     on b.SubcategoryId equals c.SubEntityId
                                     select new
                                     {
                                         LinkID = b.LinkId,
                                         Designation = c.Description
                                     }).AsEnumerable();



            var empPersonal = await (from emp in _context.HrEmpMasters
                                     join e in designationDetails on emp.DesigId equals e.LinkID into desig
                                     from e in desig.DefaultIfEmpty()
                                     join f in gradeDetails on emp.GradeId equals f.LinkID into grade
                                     from f in grade.DefaultIfEmpty()
                                     join g in bandDetails on emp.BandId equals g.LinkID into band
                                     from g in band.DefaultIfEmpty()
                                     join i in _context.HrEmpImages on emp.EmpId equals i.EmpId into images
                                     from i in images.DefaultIfEmpty()
                                     join j in branchDetails on emp.BranchId equals j.LinkID into branch
                                     from j in branch.DefaultIfEmpty()
                                     join k in departmentDetails on emp.DepId equals k.LinkID into dept
                                     from k in dept.DefaultIfEmpty()
                                     join l in _context.HrEmpAddresses on emp.EmpId equals l.EmpId into address
                                     from l in address.DefaultIfEmpty()
                                     join m in _context.HrEmpPersonals on emp.EmpId equals m.EmpId into personal
                                     from m in personal.DefaultIfEmpty()
                                     join n in _context.AdmCountryMasters on m.Nationality equals n.CountryId into nationality
                                     from n in nationality.DefaultIfEmpty()
                                     join o in _context.AdmCountryMasters on m.Country equals o.CountryId into country
                                     from o in country.DefaultIfEmpty()
                                     join p in _context.AdmCountryMasters on m.CountryOfBirth equals p.CountryId into birthCountry
                                     from p in birthCountry.DefaultIfEmpty()
                                     join rep in _context.HrEmpReportings on emp.EmpId equals rep.EmpId into repGroup
                                     from rep in repGroup.DefaultIfEmpty()
                                     join res in _context.Resignations on emp.EmpId equals res.EmpId into resGroup
                                     from res in resGroup.DefaultIfEmpty()
                                     join empDetails in _context.EmployeeDetails on rep.ReprotToWhome equals empDetails.EmpId into empDetailsGroup
                                     from empDetails in empDetailsGroup.DefaultIfEmpty()
                                     join h in _context.HighLevelViewTables on emp.LastEntity equals
                                                     (h.LevelSixId == 0 ? h.LevelFiveId :
                                                     h.LevelSevenId == 0 ? h.LevelSixId :
                                                     h.LevelEightId == 0 ? h.LevelSevenId :
                                                     h.LevelNineId == 0 ? h.LevelEightId :
                                                     h.LevelTenId == 0 ? h.LevelNineId :
                                                     h.LevelElevenId == 0 ? h.LevelTenId :
                                                     h.LevelTwelveId == 0 ? h.LevelElevenId : (int?)null)
                                     into highLevelJoin
                                     from b in highLevelJoin.DefaultIfEmpty()
                                     where emp.EmpId == empId
                                     select new
                                     {

                                         emp.EmpId,
                                         Name = (string.IsNullOrEmpty(emp.MiddleName) && string.IsNullOrEmpty(emp.LastName)) ? emp.FirstName :
                                                string.IsNullOrEmpty(emp.MiddleName) ? emp.FirstName + " " + emp.LastName :
                                                string.IsNullOrEmpty(emp.LastName) ? emp.FirstName + " " + emp.MiddleName :
                                                emp.FirstName + " " + emp.MiddleName + " " + emp.LastName,
                                         Branch = j != null ? j.Designation : "NA",
                                         Department = k != null ? k.Designation : "NA",
                                         Grade = f != null ? f.Designation : "NA",
                                         Band = g != null ? g.Designation : "NA",
                                         Designation = e != null ? e.Designation : "NA",
                                         emp.LastEntity,
                                         DateOfBirth = FormatDate(emp.DateOfBirth, _employeeSettings.DateFormat),
                                         EmpCode = emp != null ? emp.EmpCode : "NA",
                                         JoinDate = FormatDate(emp.JoinDt, _employeeSettings.DateFormat),
                                         ProbationDate = FormatDate(emp.ProbationDt, _employeeSettings.DateFormat),
                                         BloodGroup = m != null ? m.BloodGrp : "NA",
                                         CompanyEmail = l != null ? l.OfficialEmail : null,
                                         PersonalEMail = l != null ? l.PersonalEmail : null,
                                         ImageUrl = i != null && !string.IsNullOrEmpty(i.ImageUrl) ? i.ImageUrl : "default.jpg",
                                         ReportingTo = empDetails.Name,
                                         ResignationDate = FormatDate(res.ResignationDate, _employeeSettings.DateFormat),
                                         RelievingDate = FormatDate(res.RelievingDate, _employeeSettings.DateFormat),
                                         emp.CurrentStatus,
                                         emp.EmpStatus,
                                         Age = CalculateAge(emp.DateOfBirth, _employeeSettings.DateFormat),
                                         Gender = GetGender(emp.Gender).ToString(),
                                         emp.NoticePeriod,
                                         Country = o.CountryName,
                                         Nationality = n.Nationality,
                                         CountryOfBirth = p.CountryName,
                                         GratuityStrtDate = FormatDate(emp.GratuityStrtDate, _employeeSettings.DateFormat),
                                         FirstEntryDate = FormatDate(emp.FirstEntryDate, _employeeSettings.DateFormat),
                                         emp.IsProbation,
                                         emp.Ishra,
                                         emp.IsExpat,
                                         emp.MealAllowanceDeduct,
                                         ServiceLength = GetEmployeeServiceLength(empId).Result,
                                         emp.CompanyConveyance,
                                         emp.CompanyVehicle,

                                         LevelOneDescription = b.LevelOneDescription,
                                         //LevelTwoId = b.LevelTwoId,
                                         LevelTwoDescription = b.LevelTwoDescription + " (" + b.LevelOneDescription + ")",
                                         //LevelThreeId = b.LevelThreeId,
                                         LevelThreeDescription = b.LevelThreeDescription + " (" + b.LevelOneDescription + "-" + b.LevelTwoDescription + ")",
                                         //LevelFourId = b.LevelFourId,
                                         LevelFourDescription = b.LevelFourDescription + " (" + b.LevelThreeDescription + ")",
                                         // LevelFiveId = b.LevelFiveId,
                                         LevelFiveDescription = b.LevelFiveDescription + " (" + b.LevelThreeDescription + "-" + b.LevelFourDescription + ")",
                                         //LevelSixId = b.LevelSixId,
                                         LevelSixDescription = b.LevelSixDescription + " (" + b.LevelFourDescription + "-" + b.LevelFiveDescription + ")",
                                         // LevelSevenId = b.LevelSevenId,
                                         LevelSevenDescription = b.LevelSevenDescription + " (" + b.LevelThreeDescription + "-" + b.LevelSixDescription + ")",
                                         // LevelEightId = b.LevelEightId,
                                         LevelEightDescription = b.LevelEightDescription + " (" + b.LevelThreeDescription + "-" + b.LevelSevenDescription + ")",
                                         //LevelNineId = b.LevelNineId,
                                         LevelNineDescription = b.LevelNineDescription + " (" + b.LevelSevenDescription + "-" + b.LevelEightDescription + ")",
                                         // LevelTenId = b.LevelTenId,
                                         LevelTenDescription = b.LevelTenDescription + " (" + b.LevelEightDescription + "-" + b.LevelNineDescription + ")",
                                         // LevelElevenId = b.LevelElevenId,
                                         LevelElevenDescription = b.LevelElevenDescription + " (" + b.LevelNineDescription + "-" + b.LevelTenDescription + ")",
                                         LevelTwelveDescription = b.LevelTwelveDescription + " (" + b.LevelTenDescription + "-" + b.LevelElevenDescription + ")",
                                         EnableEditButton = GetEditButtonCode(empId).Result,
                                         IsSave = emp.IsSave ?? 0,
                                         ProfilePercentage = "0%"

                                     }).Distinct().ToListAsync();

            var empParameterValue = await GetDefaultCompanyParameter(empId, "EDITOPFOREMP", _employeeSettings.companyParameterCodesType);

            return new
            {
                EmpData = empPersonal,
                Value = empParameterValue
            };
        }
        private async Task<string> GetEmployeeServiceLength(int empId)
        {
            // Get join date
            var joinDate = await _context.EmployeeDetails
                                  .Where(e => e.EmpId == empId)
                                  .Select(e => e.JoinDt)
                                  .FirstOrDefaultAsync();

            if (joinDate == null)
                return "0Y: 0M: 0D"; // Employee not found

            // Determine last working date
            var lastDate = await _context.Resignations
                .Where(r => r.EmpId == empId
                            && (r.ApprovalStatus == "P" || r.ApprovalStatus == "A")
                            && r.RejoinStatus != "A"
                            && r.ApprovalStatus != "D"
                            && r.RelievingDate < DateTime.UtcNow)
                .OrderByDescending(r => r.RelievingDate)
                .Select(r => r.RelievingDate)
                .FirstOrDefaultAsync() ?? DateTime.Now;

            // Future join date scenario
            if (joinDate > lastDate)
                return "0Y: 0M: 0D";

            // Calculate service length
            int years = lastDate.Year - joinDate.Value.Year;
            int months = lastDate.Month - joinDate.Value.Month;
            int days = lastDate.Day - joinDate.Value.Day;

            // Adjust for negative month or day
            if (days < 0)
            {
                months--;
                days += DateTime.DaysInMonth(lastDate.Year, lastDate.Month);
            }

            if (months < 0)
            {
                years--;
                months += 12;
            }

            return $"{years}Y: {months}M: {days}D";
        }
        private async Task<string> GetEditButtonCode(int empId)
        {

            // Get the default company parameter from the function
            var defaultValue = await GetDefaultCompanyParameter(empId, "EMPEDITBUTTN", "EMP1");

            // If no result from the function, return null or appropriate value
            if (defaultValue == null)
                return null;

            // Query HRM_VALUE_TYPES for the required code
            var editButtonCode = await _context.HrmValueTypes
                .Where(v => v.Type == "EmployeeReporting" && v.Value == Convert.ToInt32(defaultValue))
                .Select(v => v.Code)
                .FirstOrDefaultAsync();  // This returns the first match or null if no match found

            return editButtonCode;

        }
        public async Task<object> FillEmpProject(int empId)
        {
            var result = await (from a in _context.Projects
                                join b in _context.GeneralCategories on a.ProjectNameid equals b.Id
                                join c in _context.ReasonMasters on a.ProjectDescriptionid equals c.ReasonId
                                where a.EmployeeId == empId && a.Status == null
                                orderby a.Id
                                select new
                                {
                                    a.Id,
                                    Projectdes = b.Description,
                                    c.Description,
                                    StartDate = a.StartDate.HasValue ? FormatDate(a.StartDate, _employeeSettings.DateFormat) : null,
                                    EndDate = a.EndDate.HasValue ? FormatDate(a.EndDate, _employeeSettings.DateFormat) : null
                                }).AsNoTracking().ToListAsync(); // Execute the database query asynchronously

            // Assign row numbers in memory
            var finalResult = result
                .Select((x, index) => new
                {
                    x.Id,
                    x.Projectdes,
                    x.Description,
                    x.StartDate,
                    x.EndDate,
                    Sno = index + 1 // Generate row numbers
                })
                .ToList();

            return finalResult;
        }
        public async Task<string> DeleteEmployeeDetails(string empIds, int entryBy)
        {
            // Split empIds into a list
            List<int> empIdList = empIds.Split(',').Select(int.Parse).ToList();

            // Fetch matching UserIds from HR_EMPLOYEE_USER_RELATION
            var userIds = _context.HrEmployeeUserRelations
                            .Where(rel => empIdList.Contains(rel.EmpId))
                            .Select(rel => rel.UserId)
                            .ToList();

            var usersToUpdate = _context.AdmUserMasters
                                       .Where(u => userIds.Contains(u.UserId))
                                       .ToList();

            foreach (var user in usersToUpdate)
            {
                user.Active = _employeeSettings.LetterN;// "N";
                user.Status = _employeeSettings.LetterN;// "N";
                user.NeedApp = false;
            }

            // Update HR_EMP_MASTER
            var employeesToUpdate = _context.HrEmpMasters
                                           .Where(e => empIdList.Contains(e.EmpId))
                                           .ToList();

            foreach (var emp in employeesToUpdate)
            {
                emp.IsDelete = true;
                emp.DeletedBy = entryBy;
                emp.DeletedDate = DateTime.UtcNow;
                emp.ModifiedDate = DateTime.UtcNow;
                emp.SeperationStatus = 4;
                emp.CurrentStatus = 4;
            }

            // Delete from SurveyRelation
            var surveysToDelete = _context.SurveyRelations
                                         .Where(s => empIdList.Contains((int)s.EmpId))
                                         .ToList();

            _context.SurveyRelations.RemoveRange(surveysToDelete);


            int result = await _context.SaveChangesAsync();

            return result > 0 ? _employeeSettings.DataDeletedSuccessStatus : _employeeSettings.DataInsertFailedStatus;


        }
        public async Task<object> GetProbationEffective(string linkId)
        {

            var splitLinkIds = linkId.Split(',')
                         .Where(id => int.TryParse(id, out _))  // Ensure only valid integers
                         .Select(int.Parse)  // Convert to int
                         .ToList();

            var companyParams = await (from a in _context.CompanyParameters
                                       join b in _context.CompanyParameters01s on a.Id equals b.ParamId
                                       where new[] { "PRODTE", "REVDTE", "EMPNOTCE" }.Contains(a.ParameterCode)
                                       && splitLinkIds.Contains((int)b.LinkId.Value) // Ensure int comparison
                                       select new
                                       {
                                           a.ParameterCode,
                                           Value = (object)b.Value, // Ensure both return 'object' type
                                           LevelId = (long?)b.LevelId, // Convert to nullable long
                                           IsLinked = true
                                       })
                             .Union(
                                 from a in _context.CompanyParameters
                                 where new[] { "PRODTE", "REVDTE", "EMPNOTCE" }.Contains(a.ParameterCode)
                                 select new
                                 {
                                     a.ParameterCode,
                                     Value = (object)a.Value, // Ensure both return 'object' type
                                     LevelId = (long?)null, // Convert to nullable long
                                     IsLinked = false
                                 }
                             )
                             .ToListAsync();



            var prodte = companyParams.Where(p => p.ParameterCode == "PRODTE")
                                       .OrderByDescending(p => p.IsLinked)
                                       .ThenByDescending(p => p.LevelId)
                                       .FirstOrDefault();

            var revdte = companyParams.Where(p => p.ParameterCode == "REVDTE")
                                       .OrderByDescending(p => p.IsLinked)
                                       .ThenByDescending(p => p.LevelId)
                                       .FirstOrDefault();

            var empnotce = companyParams.Where(p => p.ParameterCode == "EMPNOTCE")
                                         .OrderByDescending(p => p.IsLinked)
                                         .ThenByDescending(p => p.LevelId)
                                         .FirstOrDefault();

            var result = new { PRODTE = prodte, REVDTE = revdte, EMPNOTCE = empnotce };
            return result;

        }
        public async Task<string> DeleteSavedEmployee()
        {
            return await Task.FromResult(string.Empty);
        }
        public async Task<(int errorID, string errorMessage)> DeleteSavedEmployee(int empId, string status, int entryBy)
        {
            try
            {
                using var transaction = await _context.Database.BeginTransactionAsync();

                // Fetch the employee record
                var employee = await _context.HrEmpMasters.FindAsync(empId);
                if (employee == null)
                    return (1, "Employee not found");

                // Insert into history table
                var history = new DeletedSavedEmployeeHistory
                {
                    EmpId = empId,
                    Comments = status,
                    EntryBy = entryBy,
                    EntryDate = DateTime.UtcNow
                };

                _context.DeletedSavedEmployeeHistories.Add(history);
                await _context.SaveChangesAsync();

                // Delete from employee master table
                _context.HrEmpMasters.Remove(employee);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
                return (0, "Deleted");
            }
            catch (Exception ex)
            {
                return (1, $"Error: {ex.Message}");
            }
        }
        public async Task<int> UpdateEditEmployeeDetailsAsync(UpdateEmployeeRequestDto request)
        {
            var employee = _context.HrEmpMasters.FirstOrDefault(e => e.EmpId == request.EmpID);
            var employeePersonal = _context.HrEmpPersonals.FirstOrDefault(e => e.EmpId == request.EmpID);
            var employeeAddress = _context.HrEmpAddresses.FirstOrDefault(e => e.EmpId == request.EmpID);

            if (employee == null) return (0);

            // Update Employee Table
            employee.GuardiansName = request.GuardiansName;
            employee.NationalIdNo = request.NationalID;
            employee.PassportNo = request.PassportNo;
            employee.NoticePeriod = request.NoticePeriod;
            employee.CountryOfBirth = request.Country2ID;
            employee.GratuityStrtDate = request.GraStrtDate;
            employee.FirstEntryDate ??= request.FrstEntryDate;
            employee.Ishra = request.ISHRA;
            employee.IsExpat = request.IsExpat;
            employee.IsMarkAttn = request.MarkAttn;
            employee.CompanyConveyance = request.CompanyConveyance;
            employee.CompanyVehicle = request.CompanyVehicle;
            employee.MealAllowanceDeduct = request.MealAllowanceDeduct;
            employee.EmpFileNumber = request.EmpFileNumber;
            employee.ModifiedDate = DateTime.UtcNow;

            // Update Employee Personal Table
            if (employeePersonal != null)
            {
                employeePersonal.MaritalStatus = request.MaritalStatus;
                employeePersonal.BloodGrp = request.BloodGroup;
                employeePersonal.Religion = request.ReligionID;
                employeePersonal.IdentMark = request.IdentificationMark;
                employeePersonal.EntryBy = request.EntryBy;
                employeePersonal.EntryDt = request.EntryDate;
                employeePersonal.Height = request.Height;
                employeePersonal.Weight = request.Weight;
                employeePersonal.WeddingDate = request.WeddingDate;
            }

            // Update Employee Address Table
            if (employeeAddress != null)
            {
                employeeAddress.OfficialEmail = request.EmailId;
                employeeAddress.PersonalEmail = request.PersonalEMail;
            }

            // Update ADM_User_Master Table
            var userRelation = _context.HrEmployeeUserRelations.FirstOrDefault(r => r.EmpId == request.EmpID);
            if (userRelation != null)
            {
                var user = _context.AdmUserMasters.FirstOrDefault(u => u.UserId == userRelation.UserId);
                if (user != null) user.Email = request.EmailId;
            }

            return await _context.SaveChangesAsync() > 0 ? request.EmpID : 0;

        }

        //public async Task<(int, string)> UpdateEditEmployeeDetailsAsync(UpdateEmployeeRequestDto request)
        //{
        //    var employee = _context.HrEmpMasters.FirstOrDefault(e => e.EmpId == request.EmpID);
        //    var employeePersonal = _context.HrEmpPersonals.FirstOrDefault(e => e.EmpId == request.EmpID);
        //    var employeeAddress = _context.HrEmpAddresses.FirstOrDefault(e => e.EmpId == request.EmpID);

        //    if (employee == null) return (1, "Employee not found");

        //    // Update Employee Table
        //    employee.GuardiansName = request.GuardiansName;
        //    employee.NationalIdNo = request.NationalID;
        //    employee.PassportNo = request.PassportNo;
        //    employee.NoticePeriod = request.NoticePeriod;
        //    employee.CountryOfBirth = request.Country2ID;
        //    employee.GratuityStrtDate = request.GraStrtDate;
        //    employee.FirstEntryDate ??= request.FrstEntryDate;
        //    employee.Ishra = request.ISHRA;
        //    employee.IsExpat = request.IsExpat;
        //    employee.IsMarkAttn = request.MarkAttn;
        //    employee.CompanyConveyance = request.CompanyConveyance;
        //    employee.CompanyVehicle = request.CompanyVehicle;
        //    employee.MealAllowanceDeduct = request.MealAllowanceDeduct;
        //    employee.EmpFileNumber = request.EmpFileNumber;
        //    employee.ModifiedDate = DateTime.UtcNow;

        //    // Update Employee Personal Table
        //    if (employeePersonal != null)
        //    {
        //        employeePersonal.MaritalStatus = request.MaritalStatus;
        //        employeePersonal.BloodGrp = request.BloodGroup;
        //        employeePersonal.Religion = request.ReligionID;
        //        employeePersonal.IdentMark = request.IdentificationMark;
        //        employeePersonal.EntryBy = request.EntryBy;
        //        employeePersonal.EntryDt = request.EntryDate;
        //        employeePersonal.Height = request.Height;
        //        employeePersonal.Weight = request.Weight;
        //        employeePersonal.WeddingDate = request.WeddingDate;
        //    }

        //    // Update Employee Address Table
        //    if (employeeAddress != null)
        //    {
        //        employeeAddress.OfficialEmail = request.EmailId;
        //        employeeAddress.PersonalEmail = request.PersonalEMail;
        //    }

        //    // Update ADM_User_Master Table
        //    var userRelation = _context.HrEmployeeUserRelations.FirstOrDefault(r => r.EmpId == request.EmpID);
        //    if (userRelation != null)
        //    {
        //        var user = _context.AdmUserMasters.FirstOrDefault(u => u.UserId == userRelation.UserId);
        //        if (user != null) user.Email = request.EmailId;
        //    }

        //    await _context.SaveChangesAsync();
        //    return (request.EmpID, _employeeSettings.DataUpdateSuccessStatus);
        //}
        public async Task<object> GetGeoDetails(string mode, int? geoSpacingType, int? geoCriteria)
        {
            return mode switch
            {
                "GetGeoType" => await _context.HrmValueTypes
                                              .Where(a => a.Type == "GeoSpacingType")
                                              .Select(a => new { a.Value, a.Code })
                                              .ToListAsync(),

                "GetGeoCoordinates" => await _context.MasterGeotaggings
                                                     .Where(g => g.GeoSpaceType == geoSpacingType
                                                              && g.GeoCriteria == geoCriteria
                                                              && g.Status == 1)
                                                     .Select(g => new
                                                     {
                                                         Value = g.GeoMasterId,
                                                         Description = g.CoordinateName
                                                     }).ToListAsync(),

                _ => string.Empty
            };
        }

        public async Task<string?> EmployeeHraDtoAsync(EmployeeHraDto employeeHraDtos)
        {
            string? errorMessage = null;


            foreach (var hraDetail in employeeHraDtos.HraHistory)
            {

                var empCode = hraDetail.EmpCode;
                var isHRA = hraDetail.IsHRA;
                var remarks = hraDetail.Remarks;
                var empid = hraDetail.Empid;
                var entryby = hraDetail.Entryby;


                if (!DateTime.TryParseExact(hraDetail.FromDate,
                    new[] { "yyyy-MM-dd", "yyyy-MM-dd HH:mm:ss.fff" },
                    null,
                    System.Globalization.DateTimeStyles.None,
                    out DateTime effectDate))
                {
                    errorMessage = "Invalid FromDate format. Expected format: yyyy-MM-dd or yyyy-MM-dd HH:mm:ss.fff.";
                    return errorMessage;
                }


                var empHistory = await _context.HraHistories
                    .Where(h => h.EmployeeId == empid)
                    .OrderByDescending(h => h.EntryDate)
                    .FirstOrDefaultAsync();


                if (empHistory != null)
                {

                    if (effectDate <= empHistory.FromDate)
                    {
                        errorMessage = "AS";
                        return errorMessage;
                    }


                    var empMaster = await _context.HrEmpMasters
                        .FirstOrDefaultAsync(e => e.EmpId == empid);


                    if (empMaster == null)
                    {
                        errorMessage = "Employee not found in HR_EMP_MASTER.";
                        return errorMessage;
                    }


                    if (empMaster.Ishra == isHRA)
                    {
                        errorMessage = "SS";
                        return errorMessage;
                    }


                    empMaster.Ishra = isHRA;
                    empMaster.ModifiedDate = DateTime.UtcNow;
                    await _context.SaveChangesAsync();


                    empHistory.ToDate = effectDate.AddDays(-1);
                    await _context.SaveChangesAsync();
                }


                DateTime? toDate = null;
                if (hraDetail.ToDate != "string" && !string.IsNullOrWhiteSpace(hraDetail.ToDate))
                {
                    if (!DateTime.TryParseExact(hraDetail.ToDate, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out DateTime parsedToDate))
                    {
                        errorMessage = "Invalid ToDate format. Expected format: yyyy-MM-dd.";
                        return errorMessage;
                    }
                    toDate = parsedToDate;
                }


                var newHraHistory = new HraHistory
                {
                    EmployeeId = empid,
                    IsHra = isHRA,
                    FromDate = effectDate,
                    ToDate = toDate,
                    Remarks = remarks,
                    Entryby = entryby
                };

                _context.HraHistories.Add(newHraHistory);
                await _context.SaveChangesAsync();


                errorMessage = "Successfully Saved";
            }

            return errorMessage;
        }

        public async Task<object> GetEmployeeCertifications(int employeeid)
        {
            var certifications = await _context.EmployeeCertifications
                .Where(c => c.EmpId == employeeid && c.Status != "D")
                .Select(c => new
                {
                    c.CertificationId,
                    c.CertificationName,
                    c.CertificationField,
                    c.YearofCompletion,
                    c.IssuingAuthority
                })
                .ToListAsync();

            if (certifications == null || !certifications.Any())
            {
                return new { message = "No certifications found for the given employee." };
            }

            return new { certifications };
        }
        public async Task<string> DeleteCertificate(int certificateid)
        {

            var certification = await _context.EmployeeCertifications
                .FirstOrDefaultAsync(c => c.CertificationId == certificateid);

            if (certification == null)
            {
                return "0";
            }


            certification.Status = "D";


            await _context.SaveChangesAsync();


            return "1";
        }




        public async Task<string?> AddEmpModuleDetailsAsync(BiometricDto biometricDto)
        {
            string? errorMessage = null;

            try
            {
                var existingRecord = await _context.BiometricsDtls
                    .FirstOrDefaultAsync(b => b.EmployeeId == biometricDto.EmployeeID);

                if (existingRecord == null)
                {
                    var newBiometric = new BiometricsDtl
                    {
                        CompanyId = biometricDto.InstId,
                        EmployeeId = biometricDto.EmployeeID,
                        DeviceId = biometricDto.BranchBiometricId,
                        UserId = biometricDto.BiometricId,
                        EntryBy = biometricDto.EntryBy,
                        EntryDt = biometricDto.EntryDt
                    };

                    await _context.BiometricsDtls.AddAsync(newBiometric);
                }
                else
                {
                    existingRecord.CompanyId = biometricDto.InstId;
                    existingRecord.DeviceId = biometricDto.BranchBiometricId;
                    existingRecord.UserId = biometricDto.BiometricIdEdit ?? biometricDto.BiometricId;
                    existingRecord.EntryBy = biometricDto.EntryBy;
                    existingRecord.EntryDt = biometricDto.EntryDt;

                    _context.BiometricsDtls.Update(existingRecord);
                }

                var empRecord = await _context.HrEmpMasters
                    .FirstOrDefaultAsync(e => e.EmpId == biometricDto.EmployeeID);

                if (empRecord != null)
                {
                    empRecord.IsMarkAttn = biometricDto.MarkAttn;
                }

                await _context.SaveChangesAsync();
                errorMessage = "Successfully Saved";
            }
            catch (Exception ex)
            {
                errorMessage = $"Error: {ex.Message}";
            }

            return errorMessage;
        }

        public List<ParamWorkFlowViewDto> GetWorkFlowData(int linkLevel, int valueId)
        {
            if (linkLevel == 13)
            {
                var result = (from a in _context.ParamWorkFlow02s
                              join b in _context.WorkFlowDetails on a.WorkFlowId equals b.WorkFlowId
                              join c in _context.TransactionMasters on a.TransactionId equals c.TransactionId
                              where a.ValueId == valueId
                              select new ParamWorkFlowViewDto
                              {
                                  ValueId = a.ValueId,
                                  WorkFlowId = b.WorkFlowId,
                                  TransactionId = c.TransactionId,
                                  Description = c.Description
                              }).ToList();
                return result;
            }
            else
            {
                var result1 = (from a in _context.ParamWorkFlow01s
                               join b in _context.WorkFlowDetails on a.WorkFlowId equals b.WorkFlowId
                               join c in _context.TransactionMasters on a.TransactionId equals c.TransactionId
                               where a.ValueId == valueId
                               select new ParamWorkFlowViewDto
                               {
                                   ValueId = a.ValueId,
                                   WorkFlowId = b.WorkFlowId,
                                   TransactionId = c.TransactionId,
                                   Description = c.Description
                               }).ToList();
                return result1;
            }
        }

        public async Task<UpdateResult> UpdateWorkFlowELAsync(ParamWorkFlow01s2sDto dto)
        {
            try
            {
                if (dto.LinkLevel == 13)
                {
                    var entity = await _context.ParamWorkFlow02s.AsNoTracking()
                        .FirstOrDefaultAsync(p => p.ValueId == dto.ValueId);

                    entity.LinkEmpId = dto.LinkId;
                    entity.WorkFlowId = dto.WorkFlowId;
                    entity.LinkLevel = dto.LinkLevel;
                    entity.ModifiedBy = dto.ModifiedBy;
                    entity.ModifiedDate = DateTime.UtcNow;

                    _context.ParamWorkFlow02s.Update(entity);
                    await _context.SaveChangesAsync();
                    return new UpdateResult("Updated Successfully", 200);
                }
                else
                {
                    var entity = await _context.ParamWorkFlow01s
                        .FirstOrDefaultAsync(p => p.ValueId == dto.ValueId);

                    entity.LinkId = dto.LinkId;
                    entity.WorkFlowId = dto.WorkFlowId;
                    entity.LinkLevel = dto.LinkLevel;
                    entity.ModifiedBy = dto.ModifiedBy;
                    entity.ModifiedDate = dto.ModifiedDate;
                    _context.ParamWorkFlow01s.Update(entity);
                    await _context.SaveChangesAsync();
                    return new UpdateResult("Updated Successfully", 200);
                }
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<int> GetAgeLimitValue(int empId)
        {
            var AgeLimit = await GetDefaultCompanyParameter(empId, "EMPAGELIMIT", _employeeSettings.companyParameterCodesType);//"EMP1"

            var dateOfBirth = await _context.HrEmpMasters.Where(e => e.EmpId == empId)
            .Select(e => e.DateOfBirth).FirstOrDefaultAsync();

            var age = DateTime.Now.Year - dateOfBirth.Value.Year;
            var isAgeLimitValid = (age < Convert.ToInt32(AgeLimit)) ? 1 : 0;

            return isAgeLimitValid;

        }

        public async Task<ProfessionalDto> GetUpdateProfessional(int empId, string updateType, int Detailid)
        {
            if (updateType == "Pending")
            {
                return await (from a in _context.HrEmpProfdtlsApprls
                              join b in _context.CurrencyMasters
                                  on a.CurrencyId equals b.CurrencyId into gj
                              from b in gj.DefaultIfEmpty()
                              where a.ProfId == Detailid && a.EmpId == empId
                              select new ProfessionalDto
                              {
                                  Prof_Id = a.ProfId,
                                  Emp_Id = a.EmpId,
                                  Comp_Name = a.CompName,
                                  Designation = a.Designation,
                                  Comp_Address = a.CompAddress,
                                  PBno = a.Pbno,
                                  Contact_Per = a.ContactPer,
                                  Contact_No = a.ContactNo,
                                  Job_Desc = a.JobDesc,
                                  Join_Dt = FormatDate(a.JoinDt, _employeeSettings.DateFormat),// a.JoinDt.HasValue ? a.JoinDt.Value.ToString("dd/MM/yyyy") : null,
                                  Leaving_Dt = FormatDate(a.LeavingDt, _employeeSettings.DateFormat),// a.LeavingDt.HasValue ? a.LeavingDt.Value.ToString("dd/MM/yyyy") : null,
                                  Leave_Reason = a.LeaveReason,
                                  Ctc = a.Ctc,
                                  Currency_Id = b != null ? b.CurrencyId : (int?)null,
                                  Currency = b != null ? b.Currency : null
                              }).FirstOrDefaultAsync();
            }
            else if (updateType == "Approved")
            {
                return await (from a in _context.HrEmpProfdtls
                              join b in _context.CurrencyMasters
                                  on a.CurrencyId equals b.CurrencyId into gj
                              from b in gj.DefaultIfEmpty()
                              where a.ProfId == Detailid && a.EmpId == empId
                              select new ProfessionalDto
                              {
                                  Prof_Id = a.ProfId,
                                  Emp_Id = a.EmpId,
                                  Comp_Name = a.CompName,
                                  Designation = a.Designation,
                                  Comp_Address = a.CompAddress,
                                  PBno = a.Pbno,
                                  Contact_Per = a.ContactPer,
                                  Contact_No = a.ContactNo,
                                  Job_Desc = a.JobDesc,
                                  Join_Dt = a.JoinDt.HasValue ? a.JoinDt.Value.ToString("dd/MM/yyyy") : null,
                                  Leaving_Dt = a.LeavingDt.HasValue ? a.LeavingDt.Value.ToString("dd/MM/yyyy") : null,
                                  Leave_Reason = a.LeaveReason,
                                  Ctc = a.Ctc,
                                  Currency_Id = b != null ? b.CurrencyId : (int?)null,
                                  Currency = b != null ? b.Currency : null
                              }).FirstOrDefaultAsync();
            }

            return null;
        }

        public async Task<QualificationTableDto> GetUpdateQualification(int empId, string updateType, int Detailid)
        {
            int? optionenb = GetEmployeeParametersettingsNew(empId, "ENBQUALIF", "EMP1");

            //QualificationTableDto result = null;

            if (updateType == "Pending")
            {
                if (optionenb == 1)
                {
                    return await (from q in _context.HrEmpQualificationApprls
                                  where q.QlfId == Detailid && q.EmpId == empId
                                  select new QualificationTableDto
                                  {
                                      Qlficationid = q.QlfId,
                                      EmpId = q.EmpId,
                                      Course = q.Course,
                                      University = q.University,
                                      InstName = q.InstName,
                                      DurFrm = q.DurFrm.HasValue ? q.DurFrm.Value.ToString("dd/MM/yyyy") : null,
                                      DurTo = q.DurTo.HasValue ? q.DurTo.Value.ToString("dd/MM/yyyy") : null,
                                      YearPass = q.YearPass,
                                      MarkPer = q.MarkPer,
                                      Subjects = q.Subjects,
                                      Class = q.Class,
                                      CourseId = q.CourseId ?? 0,
                                      UniversityId = q.UniversityId ?? 0,
                                      InstitutId = q.InstitId ?? 0,
                                      SpecialId = q.SpecialId ?? 0
                                  }).FirstOrDefaultAsync();
                }
                else
                {
                    return await (from q in _context.HrEmpQualificationApprls
                                  where q.QlfId == Detailid && q.EmpId == empId
                                  select new QualificationTableDto
                                  {
                                      Qlficationid = q.QlfId,
                                      EmpId = q.EmpId,
                                      Course = q.Course,
                                      University = q.University,
                                      InstName = q.InstName,
                                      DurFrm = q.DurFrm.HasValue ? q.DurFrm.Value.ToString("dd/MM/yyyy") : null,
                                      DurTo = q.DurTo.HasValue ? q.DurTo.Value.ToString("dd/MM/yyyy") : null,
                                      YearPass = q.YearPass,
                                      MarkPer = q.MarkPer,
                                      Subjects = q.Subjects,
                                      Class = q.Class
                                  }).FirstOrDefaultAsync();
                }
            }
            else if (updateType == "Approved")
            {
                if (optionenb == 1)
                {
                    return await (from q in _context.HrEmpQualifications
                                  where q.QlfId == Detailid && q.EmpId == empId
                                  select new QualificationTableDto
                                  {
                                      Qlficationid = q.QlfId,
                                      EmpId = q.EmpId,
                                      Course = q.Course,
                                      University = q.University,
                                      InstName = q.InstName,
                                      DurFrm = q.DurFrm.HasValue ? q.DurFrm.Value.ToString("dd/MM/yyyy") : null,
                                      DurTo = q.DurTo.HasValue ? q.DurTo.Value.ToString("dd/MM/yyyy") : null,
                                      YearPass = q.YearPass,
                                      MarkPer = q.MarkPer,
                                      Subjects = q.Subjects,
                                      Class = q.Class,
                                      CourseId = q.CourseId ?? 0,
                                      UniversityId = q.UniversityId ?? 0,
                                      InstitutId = q.InstitId ?? 0,
                                      SpecialId = q.SpecialId ?? 0
                                  }).FirstOrDefaultAsync();
                }
                else
                {
                    return await (from q in _context.HrEmpQualifications
                                  where q.QlfId == Detailid && q.EmpId == empId
                                  select new QualificationTableDto
                                  {
                                      Qlficationid = q.QlfId,
                                      EmpId = q.EmpId,
                                      Course = q.Course,
                                      University = q.University,
                                      InstName = q.InstName,
                                      DurFrm = q.DurFrm.HasValue ? q.DurFrm.Value.ToString("dd/MM/yyyy") : null,
                                      DurTo = q.DurTo.HasValue ? q.DurTo.Value.ToString("dd/MM/yyyy") : null,
                                      YearPass = q.YearPass,
                                      MarkPer = q.MarkPer,
                                      Subjects = q.Subjects,
                                      Class = q.Class
                                  }).FirstOrDefaultAsync();
                }
            }

            return null;
        }

        public async Task<RewardAndRecognitionDto> GetEmployeeRewardsDetails(int empId)
        {
            return await (from a in _context.EmpRewards
                          join b in _context.AchievementMasters on a.AchievementId equals b.AchievementId
                          join c in _context.HrmValueTypes on a.RewardType equals c.Id
                          where a.EmpId == empId && a.Status == "A"
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
                          }).FirstOrDefaultAsync();


        }

        public async Task<SkillSetDto> GetUpdateTechnical(int empId, string updateType, int Detailid)
        {
            if (updateType == "Pending")
            {

                return await (from t in _context.HrEmpTechnicalApprls
                              where t.TechId == Detailid && t.EmpId == empId
                              select new SkillSetDto
                              {
                                  Tech_id = t.TechId,
                                  Emp_Id = t.EmpId,
                                  Course = t.Course,
                                  Course_Dtls = t.CourseDtls,
                                  Year = t.Year,
                                  Dur_Frm = t.DurFrm.HasValue ? t.DurFrm.Value.ToString("dd/MM/yyyy") : null,
                                  Dur_To = t.DurTo.HasValue ? t.DurTo.Value.ToString("dd/MM/yyyy") : null,
                                  Mark_Per = t.MarkPer,
                                  langSkills = t.LangSkills,
                                  Inst_Name = t.InstName
                              }).FirstOrDefaultAsync();

            }
            else if (updateType == "Approved")
            {

                return await (from t in _context.HrEmpTechnicals
                              where t.TechId == Detailid && t.EmpId == empId
                              select new SkillSetDto
                              {
                                  Tech_id = t.TechId,
                                  Emp_Id = t.EmpId,
                                  Course = t.Course,
                                  Course_Dtls = t.CourseDtls,
                                  Year = t.Year,
                                  Dur_Frm = t.DurFrm.HasValue ? t.DurFrm.Value.ToString("dd/MM/yyyy") : null,
                                  Dur_To = t.DurTo.HasValue ? t.DurTo.Value.ToString("dd/MM/yyyy") : null,
                                  Mark_Per = t.MarkPer,
                                  langSkills = t.LangSkills,
                                  Inst_Name = t.InstName
                              }).FirstOrDefaultAsync();




            }
            return null;
        }

        public async Task<CommunicationTableDto> GetUpdateCommunication(int empId, string updateType, int Detailid)
        {
            if (updateType == "Pending")
            {

                return await (from a in _context.HrEmpAddressApprls
                              join b in _context.AdmCountryMasters on a.Country equals b.CountryId into gj
                              from b in gj.DefaultIfEmpty() // LEFT JOIN equivalent
                              where a.AddId == Detailid && a.EmpId == empId
                              select new CommunicationTableDto
                              {
                                  Inst_Id = a.InstId,
                                  Add_Id = a.AddId,
                                  Emp_Id = a.EmpId,
                                  Add1 = a.Add1,
                                  Add2 = a.Add2,
                                  Country_ID = b != null ? b.CountryId : (int?)null, // Safe null handling for LEFT JOIN
                                  Country_Name = b != null ? b.CountryName : null,
                                  PBNo = a.Pbno,
                                  Phone = a.Phone,
                                  Mobile = a.Mobile,
                                  OfficePhone = a.OfficePhone,
                                  Extension = a.Extension,
                                  EMail = a.Email,
                                  PersonalEMail = a.PersonalEmail,
                                  Status = a.Status
                              }).FirstOrDefaultAsync(); // Get the first matching result (or null if none found)

            }
            return null;
        }

        public async Task<CommunicationTableDto> GetUpdateCommunicationExtra(int empId, string updateType, int Detailid)
        {
            if (updateType == "Pending")
            {

                return await (from a in _context.HrEmpAddress01Apprls
                              join b in _context.AdmCountryMasters on a.Addr2Country equals b.CountryId into gj
                              from b in gj.DefaultIfEmpty()
                              where a.AddrId == Detailid && a.EmpId == empId
                              select new CommunicationTableDto
                              {
                                  Add_Id = a.AddrId,
                                  Emp_Id = (int)a.EmpId,
                                  Add1 = a.PermanentAddr,
                                  Add2 = a.ContactAddr,
                                  PBNo = a.PinNo2,
                                  Country_ID = b != null ? b.CountryId : (int?)null,
                                  Country_Name = b != null ? b.CountryName : null,
                                  Phone = a.PhoneNo,
                                  Mobile = a.MobileNo,
                                  OfficePhone = a.AlterPhoneNo,
                                  Extension = null,
                                  EMail = null,
                                  PersonalEMail = null,
                                  Status = null,
                                  Inst_Id = 0
                              }).FirstOrDefaultAsync();


            }
            else if (updateType == "Approved")
            {
                return await (from a in _context.HrEmpAddress01s
                              join b in _context.AdmCountryMasters on a.Addr2Country equals b.CountryId into gj
                              from b in gj.DefaultIfEmpty() // LEFT JOIN
                              where a.AddrId == Detailid && a.EmpId == empId
                              select new CommunicationTableDto
                              {
                                  Add_Id = a.AddrId,
                                  Emp_Id = (int)a.EmpId,
                                  Add1 = a.PermanentAddr,
                                  Add2 = a.ContactAddr,
                                  PBNo = a.PinNo2,
                                  Country_ID = b != null ? b.CountryId : (int?)null,
                                  Country_Name = b != null ? b.CountryName : null,
                                  Phone = a.PhoneNo,
                                  OfficePhone = a.AlterPhoneNo,
                                  Mobile = a.MobileNo,
                                  Inst_Id = 0,
                                  Extension = null,
                                  EMail = null,
                                  PersonalEMail = null,
                                  Status = null
                              }).FirstOrDefaultAsync();


            }
            return null;
        }

        public async Task<CommunicationTableDto> GetUpdateEmergencyExtra(int empId, int Detailid)
        {

            return await (from a in _context.HrEmpEmergaddresses
                          join b in _context.AdmCountryMasters on a.Country equals b.CountryId into gj
                          from b in gj.DefaultIfEmpty() // LEFT JOIN
                          where a.AddrId == Detailid && a.EmpId == empId
                          select new CommunicationTableDto
                          {
                              Add_Id = a.AddrId,
                              Emp_Id = (int)a.EmpId,
                              Add1 = a.Address,
                              PBNo = a.PinNo,
                              Country_ID = b != null ? b.CountryId : (int?)null,
                              Country_Name = b != null ? b.CountryName : null,
                              Phone = a.PhoneNo,
                              OfficePhone = a.AlterPhoneNo,
                              Mobile = a.MobileNo,
                              emername = a.EmerName,
                              emerrelation = a.EmerRelation,
                              // Hardcoded or unused fields
                              Status = "A",
                              Inst_Id = 0,
                              Add2 = null,
                              Extension = null,
                              EMail = null,
                              PersonalEMail = null
                          }).FirstOrDefaultAsync();



        }

        public async Task<ReferenceDto> GetUpdateReference(int Detailid)
        {
            return await (from a in _context.HrEmpreferences
                          join b in _context.ConsultantDetails on a.ConsultantId equals b.Id into consultantJoin
                          from b in consultantJoin.DefaultIfEmpty()
                          join c in _context.EmployeeDetails on a.RefEmpId equals c.EmpId into empJoin
                          from c in empJoin.DefaultIfEmpty()
                          where a.RefId == Detailid
                          select new ReferenceDto
                          {
                              RefID = a.RefId,
                              RefTypedet = a.RefType == "I" ? "Internal"
                                         : a.RefType == "E" ? "External" : null,
                              RefType = a.RefType,
                              RefMethoddet = a.RefMethod == "C" ? "Consultant"
                                             : a.RefMethod == "O" ? "Others" : "NA",
                              RefMethod = a.RefMethod,
                              Id = b.Id,
                              ConsultantName = b.ConsultantName ?? "",
                              RefEmpName = c.Name ?? "",
                              Emp_Code = c.EmpCode ?? "",
                              RefEmpID = a.RefEmpId,
                              RefName = a.RefName ?? "",
                              PhoneNo = a.PhoneNo ?? "",
                              Address = a.Address ?? ""
                          }).FirstOrDefaultAsync();
        }

        public async Task<List<EmployeeLanguageSkill>> RetrieveEmployeeLanguage(int empId, int detailId)
        {
            return await _context.EmployeeLanguageSkills
                .Where(x => x.EmpId == empId && x.EmpLangId == detailId)
                .ToListAsync();
        }
        public async Task<List<ParamRoleViewDto>> EditRoleELAsync(int linkLevel, int valueId)
        {
            if (linkLevel == 13)
            {
                var result = from a in _context.ParamRole02s.AsNoTracking()
                             where a.ValueId == valueId
                             join b in _context.Categorymasterparameters.AsNoTracking() on a.ParameterId equals b.ParameterId
                             join c in _context.EmployeeDetails.AsNoTracking() on a.EmpId equals c.EmpId into empGroup
                             from c in empGroup.DefaultIfEmpty() // LEFT JOIN
                             select new ParamRoleViewDto
                             {
                                 ValueId = a.ValueId,
                                 ParamID = b.ParameterId,
                                 ParamDescription = b.ParamDescription,
                                 Emp_Id = c != null ? c.EmpId : null,
                                 EmployeeName = c != null ? c.Name : null
                             };
                return await result.ToListAsync();
            }
            else
            {
                var result = from a in _context.ParamRole01s.AsNoTracking()
                             where a.ValueId == valueId
                             join b in _context.Categorymasterparameters.AsNoTracking() on a.ParameterId equals b.ParameterId
                             join c in _context.EmployeeDetails.AsNoTracking() on a.EmpId equals c.EmpId into empGroup
                             from c in empGroup.DefaultIfEmpty() // LEFT JOIN
                             select new ParamRoleViewDto
                             {
                                 ValueId = a.ValueId,
                                 ParamID = b.ParameterId,
                                 ParamDescription = b.ParamDescription,
                                 Emp_Id = c != null ? c.EmpId : null,
                                 EmployeeName = c != null ? c.Name : null
                             };
                return await result.ToListAsync();
            }
        }

        public async Task<UpdateResult> UpdateRoleEL(ParamRole01AND02Dto dto)
        {
            try
            {
                if (dto.LinkLevel == 13)
                {
                    var entity = await _context.ParamRole02s.Where(x => x.ValueId == dto.ValueId).AsNoTracking().FirstOrDefaultAsync();
                    if (entity != null)
                    {
                        entity.LinkEmpId = dto.LinkEmpId;
                        entity.EmpId = dto.EmpId;
                        entity.ModifiedBy = dto.ModifiedBy;
                        entity.ModifiedDate = dto.ModifiedDate;
                        await _context.SaveChangesAsync();
                        return new UpdateResult("Updated Successfully", 200);
                    }
                    else
                    {
                        return new UpdateResult("Not Updated: Record not found", 404);
                    }
                }
                else
                {
                    var entity = await _context.ParamRole01s.AsNoTracking().FirstOrDefaultAsync(x => x.ValueId == dto.ValueId);
                    if (entity != null)
                    {
                        entity.LinkId = dto.LinkId;
                        entity.EmpId = dto.EmpId;
                        entity.LinkLevel = dto.LinkLevel;
                        entity.ModifiedBy = dto.ModifiedBy;
                        entity.ModifiedDate = dto.ModifiedDate;
                        await _context.SaveChangesAsync();
                        return new UpdateResult("Updated Successfully", 200);
                    }
                    else
                    {
                        return new UpdateResult("Not Updated: Record not found", 404);
                    }
                }
            }
            catch (DbUpdateException ex)
            {
                return new UpdateResult("Not Updated: Database error", 400);
            }
            //catch (Exception ex)
            //{
            //    return new UpdateResult ("Not Updated: An unexpected error occurred ", 500);
            //}
        }

        //public async Task<List<HrmValueTypeViewDto>> GetGeoType ( )
        //{
        //    var geotypes = from x in _context.HrmValueTypes
        //                   where x.Type == "GeoSpacingType"
        //                   select new HrmValueTypeViewDto
        //                   {
        //                       Value = x.Value,
        //                       Description = x.Description
        //                   };

        //    return await geotypes.ToListAsync ( );
        //}

        public async Task<CompanyParameterDto> EnableGeoCriteria()
        {

            var geocriteria = await _context.CompanyParameters.Where(x => x.Description == "Enable Coordinate Naming in Geospatial").FirstOrDefaultAsync();
            var geocriteriaDto = _mapper.Map<CompanyParameterDto>(geocriteria);
            return geocriteriaDto;
        }
        public async Task<string> GetGeoCoordinateNameStatus(int EmployeeId)
        {
            var Status = await GetDefaultCompanyParameter(EmployeeId, "COORDINATENAMEGEO", "ATTN");
            int status = int.Parse(Status);
            if (status == 1)
            {
                return "Activated";
            }
            else
            {
                return "Disabled";
            }
        }

        public async Task<string> GetGeotaggingMasterStatus(int EmployeeId)
        {
            var Status = await GetDefaultCompanyParameter(EmployeeId, "GEOTAGGINGMASTER", "EMP1");
            int status = int.Parse(Status);
            if (status == 1)
            {
                return "Activated";
            }
            else
            {
                return "Disabled";
            }
        }

        public async Task<List<EmployeeDocumentListDto>> DownloadIndividualEmpDocuments(int EmployeeId)
        {
            var result = from a in _context.HrmsEmpdocuments00s.AsNoTracking()
                         join b in _context.HrmsEmpdocumentsApproved02s.AsNoTracking() on a.DetailId equals b.DetailId
                         join c in _context.HrmsDocument00s.AsNoTracking() on a.DocId equals (int?)c.DocId
                         join d in _context.HrEmpMasters.AsNoTracking() on a.EmpId equals d.EmpId
                         where a.EmpId == EmployeeId
                         select new EmployeeDocumentListDto
                         {
                             DocID = a.DocId,
                             EmpID = a.EmpId,
                             Emp_Code = d.EmpCode,
                             FileName = b.FileName,
                             DocName = c.DocName,
                             FolderName = c.FolderName
                         };
            return await result.ToListAsync();


        }

        public async Task<List<DocumentDetailDto>> GetDocumentDetailsAsync(string status, int detailId)
        {
            IQueryable<DocumentDetailDto> query;

            if (status == "Pending")
            {
                query = from a in _context.HrmsEmpdocuments02s.AsNoTracking()
                        join b in _context.HrmsEmpdocuments00s.AsNoTracking() on a.DetailId equals b.DetailId
                        join c in _context.HrmsDocument00s.AsNoTracking() on a.DocId equals (int?)c.DocId
                        where b.DetailId == detailId
                        select new DocumentDetailDto
                        {
                            DocID = a.DocId,
                            DetailID = a.DetailId,
                            DocName = c.DocName,
                            FileName = a.FileName
                        };
            }
            else if (status == "Approved")
            {
                var result = _context.HrmsEmpdocumentsApproved02s.AsNoTracking().Where(b => b.DetailId == detailId);
                query = from a in _context.HrmsEmpdocumentsApproved02s.AsNoTracking()
                        join b in _context.HrmsEmpdocumentsApproved00s.AsNoTracking() on a.DetailId equals b.DetailId
                        join c in _context.HrmsDocument00s.AsNoTracking() on b.DocId equals (int?)c.DocId
                        where b.DetailId == detailId
                        select new DocumentDetailDto
                        {
                            DocID = a.DocId,
                            DetailID = a.DetailId,
                            //DocName = c.DocName,
                            FileName = a.FileName
                        };
            }
            else
            {
                throw new ArgumentException("Invalid status. Use 'Pending' or 'Approved'.", nameof(status));
            }

            return await query.ToListAsync();
        }

        public async Task<int> GetSlabEnabledAsync(int enteredBy)
        {
            string? slabEnabled = await GetDefaultCompanyParameter(enteredBy, "PRLPayscaleSlab", "PRL");
            return int.TryParse(slabEnabled, out int result) ? result : 0;
        }

        //public async Task<string> GetDefaultCompanyParameterAsync(int employeeId, string parameterCode, string type)
        //{
        //    if (employeeId == 0)
        //    {
        //        return "0";
        //    }
        //    else
        //    {
        //        var paramControl = await (from cp in _context.CompanyParameters
        //                                  join pct in _context.ParameterControlTypes on cp.ControlType equals pct.ParamControlId
        //                                  where cp.ParameterCode == parameterCode && cp.Type == type
        //                                  select new
        //                                  {
        //                                      ParamControlDesc = pct.ParamControlDesc,
        //                                      IsMultiple = pct.IsMultiple ?? 0
        //                                  })
        //                    .FirstOrDefaultAsync ( );
        //    }
        //}
        //public async Task<List<GeoSpacingDto>> GetEmpGeoSpacingAsync (int empId)    
        //{
        //    bool useGeo02 = await _context.Geotagging02s.AnyAsync (x => x.EmpId == empId);
        //    bool useGeo01 = !useGeo02 && await _context.HrEmpMasters
        //        .Where (s => s.EmpId == empId)
        //        .SelectMany (s => s.EmpEntity.Split (',', StringSplitOptions.RemoveEmptyEntries)
        //            .Select (item => int.Parse (item)))
        //        .Join (_context.Geotagging01s, f => f, b => b.LinkId, (f, b) => b)
        //        .AnyAsync ( );

        //    if (useGeo02)
        //    {

        //        var result = from a in _context.Geotagging02s
        //                     where a.EmpId == empId
        //                     join b in _context.Geotagging02As on a.GeoEmpId equals b.GeoEmpId
        //                     join c in _context.HrmValueTypes on new { a.Geotype, Type = "GeoSpacingType" } equals new { Geotype = c.Value, c.Type }
        //                     join d in _context.HrmValueTypes on new { b.GeoCriteria, Type = "GeoSpacingCriteria" } equals new { GeoCriteria = d.Value, d.Type }
        //                     select new GeoSpacingDto
        //                     {
        //                         GeoEmpId = a.GeoEmpId,
        //                         GeoEmpAid = b.GeoEmpAid,
        //                         EmpId = a.EmpId,
        //                         LevelId = a.LevelId,
        //                         Geotype = a.Geotype,
        //                         GeotypeCode = c.Code,
        //                         GeotypeDescription = c.Description,
        //                         GeoCriteria = b.GeoCriteria,
        //                         GeoCriteriaCode = d.Code,
        //                         GeoCriteriaDescription = d.Description,
        //                         Latitude = b.Latitude ?? "",
        //                         Longitude = b.Longitude ?? "",
        //                         Radius = b.Radius ?? "",
        //                         LiveTracking = a.LiveTracking,
        //                         LocationId = b.LocationId ?? -1,
        //                         GeoCoordinates = b.Coordinates
        //                     };

        //        return await result.ToListAsync ( );
        //    }
        //    else if (useGeo01)
        //    {
        //        var result = from s in _context.HrEmpMasters
        //                     where s.EmpId == empId
        //                     from f in s.EmpEntity.Split (',', StringSplitOptions.RemoveEmptyEntries)
        //                     join b in _context.Geotagging01s on int.Parse (f) equals b.LinkId
        //                     join c in _context.Geotagging01As on b.GeoEntityId equals c.GeoEntityId
        //                     join d in _context.HrmValueTypes on new { b.Geotype, Type = "GeoSpacingType" } equals new { Geotype = d.Value, d.Type }
        //                     join e in _context.HrmValueTypes on new { c.GeoCriteria, Type = "GeoSpacingCriteria" } equals new { GeoCriteria = e.Value, e.Type }
        //                     select new GeoSpacingDto
        //                     {
        //                         GeoEmpId = b.GeoEntityId,
        //                         GeoEmpAid = c.GeoEntityAid,
        //                         EmpId = s.EmpId,
        //                         LevelId = b.LevelId,
        //                         Geotype = b.Geotype,
        //                         GeotypeCode = d.Code,
        //                         GeotypeDescription = d.Description,
        //                         GeoCriteria = c.GeoCriteria,
        //                         GeoCriteriaCode = e.Code,
        //                         GeoCriteriaDescription = e.Description,
        //                         Latitude = c.Latitude ?? "",
        //                         Longitude = c.Longitude ?? "",
        //                         Radius = c.Radius ?? "",
        //                         LiveTracking = b.LiveTracking,
        //                         LocationId = c.LocationId ?? -1,
        //                         GeoCoordinates = null
        //                     };
        //        return await result.ToListAsync ( );
        //    }
        //    else
        //    {
        //        var result = from G00 in _context.Geotagging00s
        //                     join G00A in _context.Geotagging00As on G00.GeoCompId equals G00A.GeoCompId
        //                     join H in _context.HrmValueTypes.Where (x => x.Type == "GeoSpacingType") on G00.Geotype equals H.Value
        //                     join H2 in _context.HrmValueTypes.Where (x => x.Type == "GeoSpacingCriteria") on G00A.GeoCriteria equals H2.Value
        //                     select new GeoSpacingDto
        //                     {
        //                         GeoEmpId = G00.GeoCompId,
        //                         GeoEmpAid = null,
        //                         EmpId = null,
        //                         LevelId = G00.LevelId,
        //                         Geotype = G00.Geotype,
        //                         GeotypeCode = H.Code,
        //                         GeotypeDescription = H.Description,
        //                         GeoCriteria = G00A.GeoCriteria,
        //                         GeoCriteriaCode = H2.Code,
        //                         GeoCriteriaDescription = H2.Description,
        //                         Latitude = G00A.Latitude ?? "",
        //                         Longitude = G00A.Longitude ?? "",
        //                         Radius = G00A.Radius ?? "",
        //                         LiveTracking = G00.LiveTracking,
        //                         LocationId = G00A.LocationId ?? -1,
        //                         GeoCoordinates = null
        //                     };
        //        return await result.ToListAsync ( );
        //    }
        //}

        public async Task<int> EnableNewQualif(int empId)
        {

            int? optionenb = GetEmployeeParametersettingsNew(empId, "ENBQUALIF", "EMP1");
            return optionenb ?? 0;

        }
        //Helper method to get default company parameter-for AssignAttendancePolicyAccessAsync
        public async Task<int> GetDefaultAttendancePolicyAsync(int empId)
        {
            if (empId == 0)
                return 0;

            int? defaultPolicyId = null;

            // Step 1: Try CompanyParameters02
            defaultPolicyId = await (from a in _context.CompanyParameters02s
                                     join b in _context.CompanyParameters on a.ParamId equals b.Id
                                     where b.ParameterCode == "DEFATTNPOL"
                                           && b.Type == "EMP1"
                                           && a.EmpId == empId
                                     select a.Value).FirstOrDefaultAsync();

            if (defaultPolicyId == null || defaultPolicyId == 0)
            {
                // Step 2: Try via EmpEntity (except LevelID 1)
                var empEntity = await _context.EmployeeDetails
                    .Where(e => e.EmpId == empId)
                    .Select(e => e.EmpEntity)
                    .FirstOrDefaultAsync();

                var empEntityIds = empEntity?.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                             .Select(long.Parse)
                                             .ToList();

                if (empEntityIds?.Any() == true)
                {
                    defaultPolicyId = await (from a in _context.CompanyParameters01s
                                             join b in _context.CompanyParameters on a.ParamId equals b.Id into joined
                                             from b in joined.DefaultIfEmpty()
                                             where empEntityIds.Contains(a.LinkId ?? 0)
                                                   && b.ParameterCode == "DEFATTNPOL"
                                                   && b.Type == "EMP1"
                                                   && a.LevelId != 1
                                             orderby a.LevelId descending
                                             select a.Value).FirstOrDefaultAsync();
                }
            }

            if (defaultPolicyId == null || defaultPolicyId == 0)
            {
                // Step 3: Try EmpFirstEntity
                var empFirstEntity = await _context.EmployeeDetails
                    .Where(e => e.EmpId == empId)
                    .Select(e => e.EmpFirstEntity)
                    .FirstOrDefaultAsync();

                var firstEntityIds = empFirstEntity?.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                                    .Select(long.Parse)
                                                    .ToList();
                if (firstEntityIds?.Any() == true)
                {
                    defaultPolicyId = await (from a in _context.CompanyParameters01s
                                             join b in _context.CompanyParameters on a.ParamId equals b.Id into joined
                                             from b in joined.DefaultIfEmpty()
                                             where firstEntityIds.Contains(a.LinkId ?? 0)
                                                   && b.ParameterCode == "DEFATTNPOL"
                                                   && b.Type == "EMP1"
                                             orderby a.LevelId descending
                                             select a.Value).FirstOrDefaultAsync();
                }
            }

            if (defaultPolicyId == null || defaultPolicyId == 0)
            {
                // Step 4: Company-level fallback
                defaultPolicyId = await _context.CompanyParameters
                    .Where(p => p.ParameterCode == "DEFATTNPOL" && p.Type == "EMP1")
                    .Select(p => p.Value)
                    .FirstOrDefaultAsync();
            }

            return defaultPolicyId ?? 0;
        }
        //helper method  for helper method of SHIFT ACCESS
        public string GetControlValue(dynamic param, string controlType, bool isMultiple)
        {
            if (param == null) return null;

            return controlType switch
            {
                "Number" => param.Value,
                "Text" => param.Text,
                "Image" => param.Data,
                _ when isMultiple => param.MultipleValues,
                _ => param.Value
            };
        }





        public async Task AssignHolidayAccessAsync(AssignEmployeeAccessRequestDto request)
        {



            if (!long.TryParse(request.EmpFirstEntity, out var parsedFirstEntityId))
                throw new Exception("Invalid First Entity ID format");

            var joiningDate = request.JoinDt;
            var empEntityIds = request.EmpEntity;

            var holidayAccessExists = await _context.HrmHolidayMasterAccesses
                .AnyAsync(a => a.EmployeeId == request.empId);


            if (!holidayAccessExists)
            {
                var transactionId = await _context.TransactionMasters
                    .Where(t => t.TransactionType == "H")
                    .OrderBy(t => t.TransactionId)
                    .Select(t => t.TransactionId)
                    .FirstOrDefaultAsync();

                var empEntityIdsList = empEntityIds
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(long.Parse)
                    .ToList();

                var applicableHolidayIds = await _context.EntityApplicable00s
                    .Where(e =>
                                                e.TransactionId == transactionId &&
                        (
                            (e.LinkLevel == 1 && e.LinkId == parsedFirstEntityId) ||
                            (e.LinkLevel == 15) ||
                            (e.LinkLevel != 1 && empEntityIdsList.Contains(e.LinkId.Value))
                        )

                    )
                    .Select(e => e.MasterId)
                    .ToListAsync();

                var holidaysToInsert = await _context.HolidaysMasters
                    .Where(s => s.HolidayFromDate > joiningDate &&
                                applicableHolidayIds.Contains(s.HolidayMasterId))
                                       .Select(s => new HrmHolidayMasterAccess
                                       {
                                           EmployeeId = request.empId,
                                           HolidayMasterId = s.HolidayMasterId,
                                           IsCompanyLevel = 0,
                                           Active = "Y",
                                           CreatedBy = request.entryBy, // Fix: Convert string to int
                                           CreatedDate = DateTime.UtcNow,
                                           ValidDatefrom = joiningDate
                                       })

                    .ToListAsync();

                if (holidaysToInsert.Any())
                {
                    await _context.HrmHolidayMasterAccesses.AddRangeAsync(holidaysToInsert);
                    await _context.SaveChangesAsync();
                }
            }
        }

        public async Task AssignAttendancePolicyAccessAsync(AssignEmployeeAccessRequestDto request)
        {


            if (request == null)
                throw new Exception("Employee not found");

            if (!long.TryParse(request.EmpFirstEntity, out var firstEntityId))
                throw new Exception("Invalid FirstEntityId");

            var empEntityIdsList = request.EmpEntity?
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(long.Parse)
                .ToList() ?? new List<long>();


            var alreadyHasAccess = await _context.AttendancepolicyMasterAccesses
                .AnyAsync(a => a.EmployeeId == request.empId);

            if (alreadyHasAccess)
                return;
            //using the getdefault method

            var policyId = await GetDefaultAttendancePolicyAsync(request.empId); // Assume a function exists

            if (policyId == 0)
            {
                var transactionId = await _context.TransactionMasters
                    .Where(t => t.TransactionType == "AtnPolicy")
                    .OrderBy(t => t.TransactionId)
                    .Select(t => t.TransactionId)
                    .FirstOrDefaultAsync();

                var policyIdInt = await _context.EntityApplicable00s
                    .Where(e =>
                        e.TransactionId == transactionId &&
                        (
                            (e.LinkLevel == 1 && e.LinkId == firstEntityId) ||
                            (e.LinkLevel == 15) ||
                            (e.LinkLevel != 1 && empEntityIdsList.Contains(e.LinkId ?? 0))
                        )
                    )
                    .Join(_context.Attendancepolicy00s,
                        applicable => applicable.MasterId,
                        policy => policy.AttendancePolicyId,
                        (applicable, policy) => policy.AttendancePolicyId)
                    .FirstOrDefaultAsync();

                policyId = policyIdInt;
            }


            if (policyId != null && policyId > 0)
            {
                var newAccess = new AttendancepolicyMasterAccess
                {
                    EmployeeId = request.empId,
                    PolicyId = policyId,
                    IsCompanyLevel = 0,
                    CreatedBy = request.entryBy,
                    CreatedDate = DateTime.UtcNow,
                    Active = "Y",
                    ValidDatefrom = request.JoinDt
                };

                _context.AttendancepolicyMasterAccesses.Add(newAccess);
                await _context.SaveChangesAsync();
            }
        }

        //shift access
        public async Task<int> GetDefaultCompanyParameterAsync(int employeeId, string parameterCode, string type)
        {
            if (employeeId == 0)
                return 0;

            string defaultValue = null;

            // Step 1: Get ParameterControlDesc and IsMultiple
            var controlInfo = await (from a in _context.CompanyParameters
                                     join b in _context.ParameterControlTypes
                                     on a.ControlType equals b.ParamControlId
                                     where a.ParameterCode == parameterCode && a.Type == type
                                     select new
                                     {
                                         b.ParamControlDesc,
                                         IsMultiple = a.MultipleValues != null ? int.Parse(a.MultipleValues.ToString()) : 0
                                     }).FirstOrDefaultAsync();

            if (controlInfo == null)
                return 0;

            string controlType = controlInfo.ParamControlDesc;
            bool isMultiple = controlInfo.IsMultiple == 1;

            // STEP 2: Try employee-level (CompanyParameters02)
            var empParam = await (from a in _context.CompanyParameters02s
                                  join b in _context.CompanyParameters
                                  on a.ParamId equals b.Id
                                  where a.EmpId == employeeId && b.ParameterCode == parameterCode && b.Type == type
                                  select new
                                  {
                                      Value = a.Value.ToString(),
                                      Text = a.Text != null ? a.Text.ToString() : null,
                                      Data = a.Data != null ? a.Data.ToString() : null,
                                      MultipleValues = a.MultipleValues != null ? a.MultipleValues.ToString() : null
                                  }).FirstOrDefaultAsync();


            defaultValue = GetControlValue(empParam, controlType, isMultiple);
            if (int.TryParse(defaultValue, out int result) && result != 0)
                return result;

            // STEP 3: Try entity-level (excluding LevelID = 1)
            var emp = await _context.EmployeeDetails
                .Where(e => e.EmpId == employeeId)
                .Select(e => new { e.EmpEntity, e.EmpFirstEntity })
                .FirstOrDefaultAsync();

            if (emp != null && !string.IsNullOrEmpty(emp.EmpEntity))
            {
                var entityIds = emp.EmpEntity.Split(',').Select(long.Parse).ToList();

                var entityParam = await (from a in _context.CompanyParameters01s
                                         join b in _context.CompanyParameters on a.ParamId equals b.Id
                                         where b.ParameterCode == parameterCode
                                               && b.Type == type
                                               && a.LevelId != 1
                                               && entityIds.Contains(a.LinkId ?? 0)
                                         orderby a.LevelId descending
                                         select new
                                         {
                                             Value = a.Value.ToString(),
                                             Text = a.Text != null ? a.Text.ToString() : null,
                                             Data = a.Data != null ? a.Data.ToString() : null,
                                             MultipleValues = a.MultipleValues != null ? a.MultipleValues.ToString() : null
                                         }).FirstOrDefaultAsync();


                defaultValue = GetControlValue(entityParam, controlType, isMultiple);
                if (int.TryParse(defaultValue, out result) && result != 0)
                    return result;
            }

            // STEP 4: Try first entity (LevelID = 1)
            if (!string.IsNullOrEmpty(emp?.EmpFirstEntity))
            {
                var firstEntityParam = await (from a in _context.CompanyParameters01s
                                              join b in _context.CompanyParameters on a.ParamId equals b.Id
                                              where b.ParameterCode == parameterCode
                                                    && b.Type == type
                                                    && a.LevelId == 1
                                                    && a.LinkId.ToString() == emp.EmpFirstEntity
                                              orderby a.LevelId descending
                                              select new
                                              {
                                                  Value = a.Value.ToString(),
                                                  Text = a.Text != null ? a.Text.ToString() : null,
                                                  Data = a.Data != null ? a.Data.ToString() : null,
                                                  MultipleValues = a.MultipleValues != null ? a.MultipleValues.ToString() : null
                                              }).FirstOrDefaultAsync();


                defaultValue = GetControlValue(firstEntityParam, controlType, isMultiple);
                if (int.TryParse(defaultValue, out result) && result != 0)
                    return result;
            }

            // STEP 5: Company-level fallback
            var companyParam = await _context.CompanyParameters
                .Where(p => p.ParameterCode == parameterCode && p.Type == type)
                .Select(p => new
                {
                    Value = p.Value.ToString(),
                    Text = p.Text != null ? p.Text.ToString() : null,
                    Data = p.Data != null ? p.Data.ToString() : null,
                    MultipleValues = p.MultipleValues != null ? p.MultipleValues.ToString() : null
                }).FirstOrDefaultAsync();


            defaultValue = GetControlValue(companyParam, controlType, isMultiple);
            return int.TryParse(defaultValue, out result) ? result : 0;
        }



        public async Task AssignShiftAccessAsync(AssignEmployeeAccessRequestDto request)
        {
            if (request == null)
                throw new Exception("Employee not found");
            // Step 1: Check if shift access already exists
            var hasAccess = await _context.ShiftMasterAccesses
                .AnyAsync(s => s.EmployeeId == request.empId);

            if (hasAccess)
                return;

            // Step 2: Get default parameters
            var defShiftId = await GetDefaultCompanyParameterAsync(request.empId, "DEFSHIFT", "EMP1");
            var shiftStart = await GetDefaultCompanyParameterAsync(request.empId, "SHIFTSTART", "ATTN");

            // Step 3: Get employee info




            var shiftValidFrom = request.JoinDt;

            if (shiftStart == 1)
            {
                if (request.JoinDt.HasValue)
                {
                    shiftValidFrom = new DateTime(request.JoinDt.Value.Year, request.JoinDt.Value.Month, 1);
                }
                else
                {
                    // Handle the case where JoinDt is null
                    shiftValidFrom = DateTime.MinValue; // or any other default value
                }
            }

            if (defShiftId > 0)
            {
                // Step 4: Insert default shift
                var defaultAccess = new ShiftMasterAccess
                {
                    EmployeeId = request.empId,
                    ShiftId = defShiftId,
                    IsCompanyLevel = 0,
                    CreatedBy = request.entryBy,
                    CreatedDate = DateTime.UtcNow,
                    Active = "Y",
                    ValidDatefrom = shiftValidFrom,
                    ApprovalStatus = "A"
                };

                _context.ShiftMasterAccesses.Add(defaultAccess);
                await _context.SaveChangesAsync();
                return;
            }

            // Step 5: Use entity-based mapping if no default shift
            var transactionId = await _context.TransactionMasters
                .Where(t => t.TransactionType == "Shift")
                .Select(t => t.TransactionId)
                .FirstOrDefaultAsync(); // Already returns smallest ID


            var empEntityIds = request.EmpEntity?
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(long.Parse)
                .ToList() ?? new List<long>();

            if (!long.TryParse(request.EmpFirstEntity, out var firstEntityId))
            {
                firstEntityId = 0; // or log it
            }


            var applicableShiftId = await _context.EntityApplicable00s
                .Where(e =>
                    e.TransactionId == transactionId &&
                    (
                        (e.LinkLevel == 1 && e.LinkId == firstEntityId) ||
                        (e.LinkLevel == 15) ||
                        (e.LinkLevel != 1 && empEntityIds.Contains(e.LinkId ?? 0))
                    )
                )
                .Join(_context.HrShift00s,
                      ea => ea.MasterId,
                      s => s.ShiftId,
                      (ea, s) => s.ShiftId)
                .FirstOrDefaultAsync();

            if (applicableShiftId > 0)
            {
                var newAccess = new ShiftMasterAccess
                {
                    EmployeeId = request.empId,
                    ShiftId = applicableShiftId,
                    IsCompanyLevel = 0,
                    CreatedBy = request.entryBy,
                    CreatedDate = DateTime.UtcNow,
                    Active = "Y",
                    ValidDatefrom = shiftValidFrom,
                    ApprovalStatus = "A"
                };

                _context.ShiftMasterAccesses.Add(newAccess);
                await _context.SaveChangesAsync();
            }
        }


        //leave access
        //helper
        private List<long?> SplitStringToLongList(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return new List<long?>();

            return input
                .Replace(" ", ",")
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(x => long.TryParse(x.Trim(), out var val) ? val : (long?)null)
                .Where(x => x.HasValue)
                .ToList();
        }
        //helper for GetDefaultLeavePolicyAsync
        private async Task<int?> GetPolicyFromEntityAsync(List<long?> linkIds, bool excludeLevel1)
        {
            //if (!linkIds.Any()) return null;

            var query = from cp01 in _context.CompanyParameters01s
                        join cp in _context.CompanyParameters on cp01.ParamId equals cp.Id
                        where linkIds.Contains((int)cp01.LinkId) &&
                              cp.ParameterCode == "DEFLEAVEPOL" &&
                              cp.Type == "EMP1"
                        select new { cp01.Value, cp01.LevelId };


            if (excludeLevel1)
            {
                query = query.Where(a => a.LevelId != 1);
            }

            return await query
                .OrderByDescending(a => a.LevelId)
                .Select(a => (int?)a.Value)
                .FirstOrDefaultAsync();
        }

        //helper for LEAVE POLICY ACCESS
        public async Task<int?> GetDefaultLeavePolicyAsync(int employeeId)
        {
            if (employeeId == 0) return 0;

            var empValue = await (
                from a in _context.CompanyParameters02s
                join b in _context.CompanyParameters on a.ParamId equals b.Id
                where b.ParameterCode == "DEFLEAVEPOL"
                      && b.Type == "EMP1"
                      && a.EmpId == employeeId
                select a.Value)
                    .FirstOrDefaultAsync();

            if (empValue.HasValue && empValue.Value != 0)
                return empValue.Value;

            var empEntityInfo = await _context.EmployeeDetails
                .Where(e => e.EmpId == employeeId)
                .Select(e => new { e.EmpEntity, e.EmpFirstEntity })
                .FirstOrDefaultAsync();

            var entityIds = SplitStringToLongList(empEntityInfo?.EmpEntity);
            var entityValue = await GetPolicyFromEntityAsync(entityIds, excludeLevel1: true);
            if (entityValue is int val2 && val2 != 0)
                return val2;

            var firstEntityIds = SplitStringToLongList(empEntityInfo?.EmpFirstEntity);
            var firstEntityValue = await GetPolicyFromEntityAsync(firstEntityIds, excludeLevel1: false);
            if (firstEntityValue is int val3 && val3 != 0)
                return val3;

            var companyValue = await _context.CompanyParameters
                .Where(p => p.ParameterCode == "DEFLEAVEPOL" && p.Type == "EMP1")
                .Select(p => p.Value)
                .FirstOrDefaultAsync();

            if (companyValue is int val4 && val4 != 0)
                return val4;

            return null;
        }


        public async Task AssignLeavePolicyIfNotExistsAsync(AssignEmployeeAccessRequestDto request)
        {


            var exists = await _context.LeavePolicyMasters
             .AnyAsync(a => a.EmpId == request.empId);

            if (exists)
                return;

            var leavePolicyMasterId = await GetDefaultLeavePolicyAsync(request.empId);

            if (leavePolicyMasterId == null || leavePolicyMasterId == 0)
            {
                var transactionId = await _context.TransactionMasters
                    .Where(t => t.TransactionType == "LeavePolicy")
                    .Select(t => t.TransactionId)
                    .FirstOrDefaultAsync();

                var entityIdList = SplitStringToLongList(request.EmpEntity);


                leavePolicyMasterId = await _context.LeavePolicyMasters
                  .Where(lpm => _context.EntityApplicable00s
                        .Where(ea =>
                            (ea.LinkLevel == 1 && ea.LinkId == Convert.ToInt64(request.EmpFirstEntity) && ea.TransactionId == transactionId) ||
                            (ea.LinkLevel == 15 && ea.TransactionId == transactionId) ||
                            (ea.LinkLevel != 1 && entityIdList.Contains(ea.LinkId) && ea.TransactionId == transactionId)
                        )

                        .Select(ea => ea.MasterId)
                        .Contains(lpm.LeavePolicyMasterId)
                    )
                    .Select(lpm => lpm.LeavePolicyMasterId)
                    .FirstOrDefaultAsync();
            }
            if (leavePolicyMasterId != null && leavePolicyMasterId > 0)
            {
                var access = new LeavepolicyMasterAccess
                {
                    EmployeeId = request.empId,
                    PolicyId = leavePolicyMasterId.Value,
                    IsCompanyLevel = 0,
                    CreatedBy = request.entryBy,
                    CreatedDate = DateTime.UtcNow,
                    Fromdate = request.JoinDt
                };
                _context.LeavepolicyMasterAccesses.Add(access);
                await _context.SaveChangesAsync();
            }
        }

        public async Task AssignEmployeeLeaveAccessAsync(AssignEmployeeAccessRequestDto request)
        {
            bool accessExists = await _context.HrmLeaveEmployeeleaveaccesses
        .AnyAsync(x => x.EmployeeId == request.empId);


            if (accessExists)
                return;

            var transactionId = await _context.TransactionMasters
            .Where(t => t.TransactionType == "Leave")
            .Select(t => t.TransactionId)
            .FirstOrDefaultAsync();

            if (transactionId == 0)
                return;

            var empEntityIdList = SplitStringToLongList(request.EmpFirstEntity);

            var leaveMasterIds = await
                 (from lm in _context.HrmLeaveMasters
                  where lm.Active == 1
              && (from ea in _context.EntityApplicable00s
                  where ea.TransactionId == transactionId &&
                         (
                             (ea.LinkLevel == 1 && ea.LinkId == Convert.ToInt64(request.EmpFirstEntity)) ||
                             (ea.LinkLevel == 15) ||
                             (ea.LinkLevel != 1 && empEntityIdList.Contains(ea.LinkId))
                         )

                  select ea.MasterId).Contains(lm.LeaveMasterId)
                  select lm.LeaveMasterId).ToListAsync();

            if (leaveMasterIds.Count == 0)
                return;

            var newAccess = leaveMasterIds.Select(masterId => new HrmLeaveEmployeeleaveaccess
            {
                EmployeeId = request.empId,
                LeaveMaster = masterId,
                IsCompanyLevel = 0,
                CreatedBy = request.entryBy,
                CreatedDate = DateTime.UtcNow,
                Status = 1,
                FromDate = request.JoinDt
            }).ToList();
            await _context.HrmLeaveEmployeeleaveaccesses.AddRangeAsync(newAccess);
            await _context.SaveChangesAsync();
        }

        public async Task AssignLeaveBasicSettingsAccessAsync(AssignEmployeeAccessRequestDto request)
        {
            var exists = await _context.HrmLeaveBasicsettingsaccesses
             .AnyAsync(x => x.EmployeeId == request.empId);

            if (exists) return;



            var leaveTransactionId = await _context.TransactionMasters
                .Where(x => x.TransactionType == "Leave")
                .Select(x => x.TransactionId)
                .FirstOrDefaultAsync();

            var empEntityIdList = SplitStringToLongList(request.EmpEntity);

            var applicableLeaveMasterIds = await GetApplicableMasterIds(leaveTransactionId, Convert.ToInt64(request.EmpFirstEntity), request.EmpEntity);

            var activeLeaveMasterIds = await _context.HrmLeaveMasters
                   .Where(l => l.Active == 1 && applicableLeaveMasterIds.Contains(l.LeaveMasterId))
                   .Select(l => l.LeaveMasterId)
                   .ToListAsync();
            var leaveBsTransactionId = await _context.TransactionMasters
                   .Where(x => x.TransactionType == "Leave_BS")
                   .Select(x => x.TransactionId)
                   .FirstOrDefaultAsync();


            var settingsToInsert = await (from s in _context.HrmLeaveMasterandsettingsLinks
                                          join a in _context.HrmLeaveMasters on s.LeaveMasterId equals a.LeaveMasterId
                                          where a.Active == 1 &&


                                                activeLeaveMasterIds.Contains(s.LeaveMasterId.Value) &&
                                                applicableLeaveMasterIds.Contains(s.IdMasterandSettingsLink)


                                          select new HrmLeaveBasicsettingsaccess
                                          {
                                              EmployeeId = request.empId,
                                              SettingsId = s.SettingsId,
                                              LeaveMasterId = s.LeaveMasterId,
                                              IsCompanyLevel = 0,
                                              CreatedBy = request.entryBy,
                                              CreatedDate = DateTime.UtcNow,
                                              FromDateBs = request.JoinDt,
                                              Laps = 1.00m
                                          }).ToListAsync();
            await _context.HrmLeaveBasicsettingsaccesses.AddRangeAsync(settingsToInsert);
            await _context.SaveChangesAsync();


        }

        public async Task AssignEmployeeAccessAsync(AssignEmployeeAccessRequestDto request)
        {
            var employee = await _context.HrEmpMasters
                .Where(e => e.EmpId == request.empId)
                .Select(e => new { e.EmpEntity, e.EmpFirstEntity, e.JoinDt })
                .FirstOrDefaultAsync();

            if (employee is not null)
            {
                request.EmpEntity = employee.EmpEntity;
                request.EmpFirstEntity = employee.EmpFirstEntity;
                request.JoinDt = employee.JoinDt;
            }
            //Holiday
            await AssignHolidayAccessAsync(request);
            //attendancePolicy
            await AssignAttendancePolicyAccessAsync(request);
            //shift access
            await AssignShiftAccessAsync(request);
            //leave policy
            await AssignLeavePolicyIfNotExistsAsync(request);
            //leave access
            await AssignEmployeeLeaveAccessAsync(request);
            //leave basic settings
            await AssignLeaveBasicSettingsAccessAsync(request);
        }
        //helper FUNCTION FOR AssignLeaveBasicSettingsAccessAsync
        private async Task<List<long?>> GetApplicableMasterIds(long transactionId, long firstEntityId, string empEntityIdsCsv)
        {
            var empEntityIdList = SplitStringToLongList(empEntityIdsCsv);

            var applicableMasterIds = await _context.EntityApplicable00s
                .Where(x =>
                    x.TransactionId == transactionId &&
                    (
                        (x.LinkLevel == 1 && x.LinkId == firstEntityId) ||
                        (x.LinkLevel == 15) ||
                        (x.LinkLevel != 1 && empEntityIdList.Contains(x.LinkId))
                    )
                )
                .Select(x => x.MasterId)
                .ToListAsync();

            return applicableMasterIds;
        }

        public async Task<int> SaveParamWorkflow(SaveParamWorkflowDto request)
        {
            var moduleIdList = SplitStringToLongList(request.ModuleIds);
            var currentDate = DateTime.Now;

            if (request.LinkLevel == 13)
            {
                var entries = moduleIdList.Select(item => new ParamWorkFlow02
                {
                    LinkEmpId = request.LinkId,
                    TransactionId = (int?)item,
                    WorkFlowId = request.WorkFlowId,
                    LinkLevel = request.LinkLevel,
                    CreatedBy = request.CreatedBy,
                    CreatedDate = currentDate
                }).ToList();

                await _context.ParamWorkFlow02s.AddRangeAsync(entries);
                await _context.SaveChangesAsync();
                return 2; // ErrorID = 02
            }
            else
            {
                var entries = moduleIdList.Select(item => new ParamWorkFlow01
                {
                    LinkId = request.LinkId,
                    TransactionId = (int?)item,
                    WorkFlowId = request.WorkFlowId,
                    LinkLevel = request.LinkLevel,
                    CreatedBy = request.CreatedBy,
                    CreatedDate = currentDate
                }).ToList();

                await _context.ParamWorkFlow01s.AddRangeAsync(entries);
                await _context.SaveChangesAsync();
                return 1; // ErrorID = 1
            }
        }

        public async Task<int> InsertRoleAsync(RoleInsertDTO roleInsertDto)
        {
            int errorId = 0;

            if (roleInsertDto.LinkLevel == 13)
            {
                // Insert into ParamRole02
                var paramRole02 = new ParamRole02
                {
                    LinkEmpId = roleInsertDto.LinkId,
                    ParameterId = roleInsertDto.ParameterId,
                    EmpId = roleInsertDto.EmpId,
                    LinkLevel = roleInsertDto.LinkLevel,
                    CreatedBy = roleInsertDto.CreatedBy,
                    CreatedDate = DateTime.Now
                };
                _context.ParamRole02s.Add(paramRole02);
                errorId = 2;
            }
            else
            {
                // Insert into ParamRole01
                var paramRole01 = new ParamRole01
                {
                    LinkId = roleInsertDto.LinkId,
                    ParameterId = roleInsertDto.ParameterId,
                    EmpId = roleInsertDto.EmpId,
                    LinkLevel = roleInsertDto.LinkLevel,
                    CreatedBy = roleInsertDto.CreatedBy,
                    CreatedDate = DateTime.Now
                };
                _context.ParamRole01s.Add(paramRole01);
                errorId = 1;
            }

            await _context.SaveChangesAsync();
            return errorId;
        }

        public async Task<List<RoleDetailsDTO>> GetRoleDetailsAsync(int linkId, int linkLevel)
        {
            IQueryable<RoleDetailsDTO> query;

            if (linkLevel == 13)
            {
                // Query for ParamRole02
                query = from a in _context.ParamRole02s
                        join b in _context.Categorymasterparameters on a.ParameterId equals b.ParameterId
                        join c in _context.EmployeeDetails on a.EmpId equals c.EmpId into employeeGroup
                        from c in employeeGroup.DefaultIfEmpty() // Ensures a left join
                        where a.LinkEmpId == linkId
                        select new RoleDetailsDTO
                        {
                            ValueId = a.ValueId,
                            ParamDescription = b.ParamDescription,
                            EmployeeName = c.Name != null ? c.Name : "Not Assigned"

                        };

            }
            else
            {
                // Query for ParamRole01
                query = from a in _context.ParamRole01s
                        join b in _context.Categorymasterparameters on a.ParameterId equals b.ParameterId
                        join c in _context.EmployeeDetails on a.EmpId equals c.EmpId into employeeGroup
                        from c in employeeGroup.DefaultIfEmpty() // Ensures a left join
                        where a.LinkId == linkId
                        select new RoleDetailsDTO
                        {
                            ValueId = a.ValueId,
                            ParamDescription = b.ParamDescription,
                            EmployeeName = c.Name != null ? c.Name : "Not Assigned"

                        };

            }

            return await query.ToListAsync();
        }

        //public async Task<List<object>> GetGeoCoordinatesAsync(int geoSpacingType, int geoCriteria)
        //{
        //    return await _context.MasterGeotaggings
        //        .Where(x => x.GeoSpaceType == geoSpacingType &&
        //                    x.GeoCriteria == geoCriteria &&
        //                    x.Status == 1)
        //        .Select(x => new
        //        {
        //            Value = x.GeoMasterId,
        //            Description = x.CoordinateName
        //        })
        //        .Cast<object>()
        //        .ToListAsync();
        //}

        public async Task<List<object>> GetGeoSpacingCriteria()
        {
            return await _context.HrmValueTypes
                .Where(x => x.Type == "GeoSpacingCriteria")
                .Select(x => new
                {
                    x.Value,
                    x.Description
                })
                .Cast<object>()
                .ToListAsync();
        }
        public async Task<List<object>> GetGeoCoordinatesTabAsync(int geoSpacingType, int geoCriteria)
        {
            return await _context.MasterGeotaggings
                .Where(x => x.GeoSpaceType == geoSpacingType &&
                            x.GeoMasterId == geoCriteria &&
                            x.Status == 1)
                .Select(x => new
                {
                    Value = x.GeoMasterId,
                    Description = x.CoordinateName
                })
                .Cast<object>()
                .ToListAsync();
        }
        public async Task<string> SaveOrUpdateGeoLocationAsync(SaveGeoLocationRequestDTO dto)
        {
            var geoEmp = await _context.Geotagging02s.FirstOrDefaultAsync(x => x.EmpId == dto.EmpID);

            if (geoEmp != null)
            {
                // Update existing Geotagging02
                geoEmp.Geotype = dto.GeoSpacingType;
                geoEmp.LiveTracking = dto.LiveTracking;
                geoEmp.UpdatedBy = dto.EntryBy;
                geoEmp.UpdatedDate = DateTime.UtcNow;

                // Delete existing locations
                var oldLocations = _context.Geotagging02As.Where(a => a.GeoEmpId == geoEmp.GeoEmpId);
                _context.Geotagging02As.RemoveRange(oldLocations);

                // Insert new
                foreach (var spacing in dto.GeoSpacings)
                {
                    _context.Geotagging02As.Add(new Geotagging02A
                    {
                        GeoEmpId = geoEmp.GeoEmpId,
                        GeoCriteria = spacing.GeoCriteria,
                        Latitude = spacing.Latitude,
                        Longitude = spacing.Longitude,
                        Radius = spacing.Radius,
                        LocationId = spacing.LocationId,
                        Coordinates = spacing.Coordinates
                    });
                }

                await _context.SaveChangesAsync();
                return "Updated Successfully";
            }
            else
            {
                // Insert new Geotagging02
                var newGeoEmp = new Geotagging02
                {
                    EmpId = dto.EmpID,
                    LevelId = 13,
                    Geotype = dto.GeoSpacingType,
                    LiveTracking = dto.LiveTracking,
                    EntryBy = dto.EntryBy,
                    EntryDate = DateTime.UtcNow
                };
                _context.Geotagging02s.Add(newGeoEmp);
                await _context.SaveChangesAsync(); // Save to get GeoEmpID

                foreach (var spacing in dto.GeoSpacings)
                {
                    _context.Geotagging02As.Add(new Geotagging02A
                    {
                        GeoEmpId = newGeoEmp.GeoEmpId,
                        GeoCriteria = spacing.GeoCriteria,
                        Latitude = spacing.Latitude,
                        Longitude = spacing.Longitude,
                        Radius = spacing.Radius,
                        LocationId = spacing.LocationId,
                        Coordinates = spacing.Coordinates
                    });
                }

                await _context.SaveChangesAsync();
                return "Saved Successfully";
            }
        }
        public async Task<IEnumerable<AssetCategoryCodeDto>> GetFilteredAssetCategoriesAsync(int varAssetTypeID)
        {
            var result = await _context.AssetcategoryCodes
                .Where(a => a.Value == varAssetTypeID &&
                            a.Status != "D" &&
                            a.Status != "A" &&
                            a.Status != "Dis" &&
                            a.Status != "DA")
                .OrderBy(a => a.Description)
                .Select(a => new AssetCategoryCodeDto
                {
                    Id = a.Id,
                    Code = a.Code,
                    Description = a.Description,
                    Value = a.Value,
                    Createdby = a.Createdby,
                    Status = a.Status,
                    Assetclass = a.Assetclass,
                    AssetModel = a.AssetModel
                })
                .ToListAsync();





            return result;
        }
        public async Task<IEnumerable<AssetCategoryCodeDto>> GetAssignedOrPendingAssetCategoriesAsync(int varAssetTypeID, string varAssignAssetStatus)
        {
            var result = await _context.AssetcategoryCodes
                .Where(a => a.Value == varAssetTypeID &&
                            a.Status != "D" &&
                            (
                                (a.AssetModel == varAssignAssetStatus && a.Status == "A") ||
                                a.Status == "P" || a.Status == "Pending"
                            ))
                .Select(a => new AssetCategoryCodeDto
                {
                    Id = a.Id,
                    Code = a.Code,
                    Description = a.Description,
                    Value = a.Value,
                    Createdby = a.Createdby,
                    Status = a.Status,
                    Assetclass = a.Assetclass,
                    AssetModel = a.AssetModel
                })
                .ToListAsync();

            return result;
        }

        public async Task<IEnumerable<ReasonDto>> GetGeneralSubCategoryAsync(string code)
        {
            var result = await (from a in _context.ReasonMasters
                                join b in _context.GeneralCategories
                                on a.Type equals b.Description
                                where b.Description == code
                                select new ReasonDto
                                {
                                    Reason_Id = a.ReasonId,
                                    Description = a.Description
                                }).ToListAsync();

            return result;
        }

        public async Task<string> SaveShiftMasterAccessAsync(ShiftMasterAccessInputDto request)
        {
            // Get latest Emp_Id
            var latestEmpId = await _context.EmployeeDetails
                .OrderByDescending(e => e.EmpId)
                .Select(e => e.EmpId)
                .FirstOrDefaultAsync();

            var shiftAccess = new ShiftMasterAccess
            {
                EmployeeId = latestEmpId,
                ShiftId = request.ShiftId,
                IsCompanyLevel = 0,
                CreatedBy = request.CreatedBy,
                CreatedDate = DateTime.UtcNow,
                ValidDatefrom = request.ValidDateFrom,
                ValidDateTo = request.ValidDateTo,
                Active = "Y",
                WeekEndMasterId = request.WeekEndMasterId,
                ShiftApprovalId = 0,
                ApprovalStatus = "A",
                ProjectId = null
            };

            _context.ShiftMasterAccesses.Add(shiftAccess);
            await _context.SaveChangesAsync();

            return "Saved";
        }
        public async Task<List<object>> GetLanguagesAsync()
        {
            return await _context.HrEmpLanguagemasters
                .Select(lang => new
                {
                    lang.LanguageId,
                    lang.Description
                })
                .Cast<object>()
                .ToListAsync();
        }
        public async Task<PayscaleComponentsResponseDto> PayscaleComponentsListManual(int batchId, int employeeIds, int type)
        {
            var response = new PayscaleComponentsResponseDto();

            // Call the equivalent of [dbo].[GetEmployeeParametersettings]
            int enablePayscaleHourly = await GetEmployeeParameterSettingsAsync(1, "EmployeeReporting", "ENBLEPAYSCALEHRLY", "PRL");

            if (enablePayscaleHourly == 1 && type == 1)
            {
                // Query 1: Employee hours and amount
                var employeeHoursQuery = from pr in _context.PayscaleRequest01s.AsNoTracking()
                                         where pr.PayRequest01Id == batchId && pr.EmployeeId == employeeIds
                                         select new PayscaleHourlyDto
                                         {
                                             EmployeeId = pr.EmployeeId,
                                             TotalHours = pr.TotalHours,
                                             HourlyAmount = pr.HourlyAmount
                                         };

                response.HourlyComponents = await employeeHoursQuery.ToListAsync();

                // Query 2: Employee details
                var employeeDetailsQuery = from pr in _context.PayscaleRequest01s.AsNoTracking()
                                           join emp in _context.EmployeeDetails.AsNoTracking() on pr.EmployeeId equals emp.EmpId
                                           join pr0 in _context.PayscaleRequest00s.AsNoTracking() on pr.BatchId equals pr0.BatchId
                                           join cm in _context.CurrencyMasters.AsNoTracking() on pr0.CurrencyId equals cm.CurrencyId
                                           join des in _context.DesignationDetails.AsNoTracking() on emp.DesigId equals des.LinkId
                                           where emp.EmpId == employeeIds
                                           select new PayscaleEmployeeInfoDto
                                           {
                                               EmpId = emp.EmpId,
                                               EmpCode = emp.EmpCode,
                                               Name = emp.Name,
                                               JoinDate = emp.JoinDt.HasValue ? emp.JoinDt.Value.ToString("dd/MM/yyyy") : string.Empty,
                                               Designation = des.Designation,
                                               EffectiveDate = pr.EffectiveDate.HasValue ? pr.EffectiveDate.Value.ToString("dd/MM/yyyy") : string.Empty,
                                               CurrencyCode = cm.CurrencyCode
                                           };

                response.EmployeeInfo = await employeeDetailsQuery.FirstOrDefaultAsync();
            }
            else
            {
                // Query 1: Earnings components (PayType = 1)
                var earningsQuery = from pr in _context.PayscaleRequest01s.AsNoTracking()
                                    join pr2 in _context.PayscaleRequest02s.AsNoTracking() on pr.PayRequest01Id equals pr2.PayRequestId01
                                    join pcm in _context.PayCodeMaster01s.AsNoTracking() on pr2.PayComponentId equals pcm.PayCodeId
                                    where pr.PayRequest01Id == batchId && pr2.PayType == 1
                                    select new PayscaleComponentDto
                                    {
                                        ComponentId = pr2.PayRequestId02,
                                        Component = pcm.PayCode + " - " + pcm.PayCodeDescription,
                                        Amount = (decimal)pr2.Amount,
                                        TotalEarnings = pr.TotalEarnings,
                                        TotalDeductions = pr.TotalDeductions
                                    };

                response.EarningsComponents = await earningsQuery.ToListAsync();

                // Query 2: Deductions components (PayType = 2)
                var deductionsQuery = from pr in _context.PayscaleRequest01s.AsNoTracking()
                                      join pr2 in _context.PayscaleRequest02s.AsNoTracking() on pr.PayRequest01Id equals pr2.PayRequestId01
                                      join pcm in _context.PayCodeMaster01s.AsNoTracking() on pr2.PayComponentId equals pcm.PayCodeId
                                      where pr.PayRequest01Id == batchId && pr2.PayType == 2
                                      select new PayscaleComponentDto
                                      {
                                          ComponentId = pr2.PayRequestId02,
                                          Component = pcm.PayCode + " - " + pcm.PayCodeDescription,
                                          Amount = (decimal)pr2.Amount,
                                          TotalEarnings = pr.TotalEarnings,
                                          TotalDeductions = pr.TotalDeductions
                                      };

                response.DeductionComponents = await deductionsQuery.ToListAsync();

                // Query 3: Employee details
                var employeeDetailsQuery = from pr in _context.PayscaleRequest01s.AsNoTracking()
                                           join emp in _context.EmployeeDetails.AsNoTracking() on pr.EmployeeId equals emp.EmpId
                                           join pr0 in _context.PayscaleRequest00s.AsNoTracking() on pr.BatchId equals pr0.BatchId
                                           join cm in _context.CurrencyMasters.AsNoTracking() on pr0.CurrencyId equals cm.CurrencyId
                                           join des in _context.DesignationDetails.AsNoTracking() on emp.DesigId equals des.LinkId
                                           where emp.EmpId == employeeIds
                                           select new PayscaleEmployeeInfoDto
                                           {
                                               EmpCode = emp.EmpCode,
                                               Name = emp.Name,
                                               JoinDate = emp.JoinDt.HasValue ? emp.JoinDt.Value.ToString("dd/MM/yyyy") : string.Empty,
                                               Designation = des.Designation,
                                               EffectiveDate = pr.EffectiveDate.HasValue ? pr.EffectiveDate.Value.ToString("dd/MM/yyyy") : string.Empty,
                                               CurrencyCode = cm.CurrencyCode
                                           };

                response.EmployeeInfo = await employeeDetailsQuery.FirstOrDefaultAsync();
            }

            return response;
        }



        public async Task<int> GetEmployeeParameterSettingsAsync(int employeeId, string drpType = "", string parameterCode = "", string parameterType = "")
        {
            int? dailyRate = 0;

            // First query: Check CompanyParameters02
            var dailyRate1 = (from cp2 in _context.CompanyParameters02s
                              join cp in _context.CompanyParameters on cp2.ParamId equals cp.Id
                              select new
                              {
                                  a = cp.Id,
                                  parameterCode = cp.ParameterCode,
                                  parameterType = cp.Type,
                                  EmpId = cp2.EmpId
                              }).Where(x => x.parameterCode == parameterCode && x.parameterType == parameterType && x.EmpId == employeeId);

            dailyRate = await dailyRate1.CountAsync();

            if (dailyRate == 0 || string.IsNullOrEmpty(dailyRate.ToString()))
            {
                var entity = await _context.EmployeeDetails
                    .Where(ed => ed.EmpId == employeeId)
                    .Select(ed => ed.EmpEntity)
                    .FirstOrDefaultAsync() ?? "";
                var entityItems = entity.Split(',');
                var entityList = entity.Split(',').Select(s => long.Parse(s)).ToList();

                // Second query: Check CompanyParameters01 with HRM_VALUE_TYPES
                dailyRate = await (from cp1 in _context.CompanyParameters01s.AsNoTracking()
                                   join cp in _context.CompanyParameters.AsNoTracking() on cp1.ParamId equals cp.Id into cpJoin
                                   from cp in cpJoin.DefaultIfEmpty()
                                   join hvt in _context.HrmValueTypes.AsNoTracking()
                                       on new { cp1.Value, Type = drpType } equals new { hvt.Value, hvt.Type } into hvtJoin
                                   from hvt in hvtJoin.DefaultIfEmpty()
                                   where cp1.LinkId.HasValue && entityList.Contains(cp1.LinkId.Value)
                                         && cp != null && cp.ParameterCode == parameterCode
                                         && cp.Type == parameterType
                                   orderby cp1.LevelId descending
                                   select hvt.Value).FirstOrDefaultAsync();
            }

            if (dailyRate == 0 || string.IsNullOrEmpty(dailyRate.ToString()))
            {
                // Third query: Fallback to CompanyParameters with HRM_VALUE_TYPES
                dailyRate = await (from cp in _context.CompanyParameters
                                   join hvt in _context.HrmValueTypes
                                       on new { cp.Value, Type = drpType } equals new { hvt.Value, hvt.Type }
                                   where cp.ParameterCode == parameterCode
                                   && cp.Type == parameterType
                                   select hvt.Value)
                                  .FirstOrDefaultAsync();
            }

            dailyRate = int.TryParse(dailyRate?.ToString(), out int parsedValue) ? parsedValue : 0;
            return (int)dailyRate;
        }

        #region GetCategoryDetails


        //public string GetCategoryDetails(int subEntityId = 0, int instId = 0, string? code = null, string? description = null,
        //string? mode = null, bool grievance = false, bool codeRequired = false, bool mandatory = false, DateTime? trxDate = null, DateTime? entryDate = null, int entryBy = 0, int valueTypeId = 0,
        //int sortOrder = 0, bool restrictDuplicate = false, int entityId = 0, bool linkToEmp = false, string? lastEntity = "", string? entityIds = null, int empId = 0)
        //{

        //    int transferLinkId = _context.SubCategoryLinksNews.Count();

        //    // Temporary List Instead of #EntitiesTempEmp
        //    var entitiesTempEmp = (from a in _context.tmp context.TmpEntityTable
        //                           join b in context.HrEmpMaster on a.EmpID equals b.Emp_Id
        //                           join c in context.HighLevelView on b.LastEntity equals c.LastEntityID
        //                           select new
        //                           {
        //                               LevelOneDescription = a.LevelOneDescription ?? c.LevelOneDescription,
        //                               LevelTwoDescription = a.LevelTwoDescription ?? c.LevelTwoDescription,
        //                               LevelThreeDescription = a.LevelThreeDescription ?? c.LevelThreeDescription,
        //                               LevelFourDescription = a.LevelFourDescription ?? c.LevelFourDescription,
        //                               LevelFiveDescription = a.LevelFiveDescription ?? c.LevelFiveDescription,
        //                               LevelSixDescription = a.LevelSixDescription ?? c.LevelSixDescription,
        //                               LevelSevenDescription = a.LevelSevenDescription ?? c.LevelSevenDescription,
        //                               LevelEightDescription = a.LevelEightDescription ?? c.LevelEightDescription,
        //                               LevelNineDescription = a.LevelNineDescription ?? c.LevelNineDescription,
        //                               LevelTenDescription = a.LevelTenDescription ?? c.LevelTenDescription,
        //                               LevelElevenDescription = a.LevelElevenDescription ?? c.LevelElevenDescription,
        //                               LevelTwelveDescription = a.LevelTwelveDescription ?? c.LevelTwelveDescription,
        //                               EntryBy = entryBy
        //                           }).ToList();

        //    // Inserting into SubCategoryLinksNew for SortOrder = 2
        //    if (context.CategoryMaster.Any(x => x.SortOrder == 2))
        //    {
        //        var insertData = (from a in entitiesTempEmp
        //                          join b in context.EntityLevelTwo on new { a.LevelOneDescription, a.LevelTwoDescription } equals new { b.LevelOneDescription, b.LevelTwoDescription } into bGroup
        //                          from b in bGroup.DefaultIfEmpty()
        //                          join c in context.SubCategory on new { a.LevelOneDescription } equals new { c.Description } into cGroup
        //                          from c in cGroup.DefaultIfEmpty()
        //                          join d in context.SubCategory on new { a.LevelTwoDescription } equals new { d.Description } into dGroup
        //                          from d in dGroup.DefaultIfEmpty()
        //                          where b == null && a.EntryBy == entryBy
        //                          select new SubCategoryLinksNew
        //                          {
        //                              CategoryID = d.EntityID,
        //                              SubcategoryID = d.SubEntityID,
        //                              LinkableCategoryID = c.EntityID,
        //                              LinkableSubcategory = c.SubEntityID,
        //                              Root = 0,
        //                              LinkLevel = 2,
        //                              LinkedEntityID = transferLinkId
        //                          }).ToList();

        //        context.SubCategoryLinksNew.AddRange(insertData);
        //    }

        //    // Inserting into SubCategoryLinksNew for SortOrder = 3
        //    if (context.CategoryMaster.Any(x => x.SortOrder == 3))
        //    {
        //        var insertData = (from a in entitiesTempEmp
        //                          join b in context.EntityLevelThree on new { a.LevelOneDescription, a.LevelTwoDescription, a.LevelThreeDescription } equals new { b.LevelOneDescription, b.LevelTwoDescription, b.LevelThreeDescription } into bGroup
        //                          from b in bGroup.DefaultIfEmpty()
        //                          join c in context.SubCategory on new { a.LevelOneDescription } equals new { c.Description } into cGroup
        //                          from c in cGroup.DefaultIfEmpty()
        //                          join d in context.SubCategory on new { a.LevelTwoDescription } equals new { d.Description } into dGroup
        //                          from d in dGroup.DefaultIfEmpty()
        //                          join e in context.SubCategory on new { a.LevelThreeDescription } equals new { e.Description } into eGroup
        //                          from e in eGroup.DefaultIfEmpty()
        //                          join f in context.EntityLevelTwo on new { a.LevelOneDescription, a.LevelTwoDescription } equals new { f.LevelOneDescription, f.LevelTwoDescription } into fGroup
        //                          from f in fGroup.DefaultIfEmpty()
        //                          where b == null && a.EntryBy == entryBy
        //                          select new SubCategoryLinksNew
        //                          {
        //                              CategoryID = e.EntityID,
        //                              SubcategoryID = e.SubEntityID,
        //                              LinkableCategoryID = d.EntityID,
        //                              LinkableSubcategory = d.SubEntityID,
        //                              Root = f.LevelTwoId,
        //                              LinkLevel = 3,
        //                              LinkedEntityID = transferLinkId
        //                          }).ToList();

        //        context.SubCategoryLinksNew.AddRange(insertData);
        //    }

        //    // Inserting into HighLevelViewTable
        //    var highLevelData = (from a in context.HighLevelView
        //                         join b in entitiesTempEmp on a.LevelOneDescription equals b.LevelOneDescription into bGroup
        //                         from b in bGroup.DefaultIfEmpty()
        //                         where !context.HighLevelViewTable.Any(h => h.LastEntityID == a.LastEntityID)
        //                         select new HighLevelViewTable
        //                         {
        //                             Root = a.Root,
        //                             LevelOneDescription = a.LevelOneDescription,
        //                             LevelOneId = a.LevelOneId,
        //                             LevelTwoDescription = a.LevelTwoDescription,
        //                             LevelTwoId = a.LevelTwoId,
        //                             LevelThreeDescription = a.LevelThreeDescription,
        //                             LevelThreeId = a.LevelThreeId,
        //                             LevelFourDescription = a.LevelFourDescription,
        //                             LevelFourId = a.LevelFourId,
        //                             LevelFiveDescription = a.LevelFiveDescription,
        //                             LevelFiveId = a.LevelFiveId,
        //                             LevelSixDescription = a.LevelSixDescription,
        //                             LevelSixId = a.LevelSixId,
        //                             LevelSevenDescription = a.LevelSevenDescription,
        //                             LevelSevenId = a.LevelSevenId,
        //                             LevelEightDescription = a.LevelEightDescription,
        //                             LevelEightId = a.LevelEightId,
        //                             LevelNineDescription = a.LevelNineDescription,
        //                             LevelNineId = a.LevelNineId,
        //                             LevelTenDescription = a.LevelTenDescription,
        //                             LevelTenId = a.LevelTenId,
        //                             LevelElevenDescription = a.LevelElevenDescription,
        //                             LevelElevenId = a.LevelElevenId,
        //                             LevelTwelveDescription = a.LevelTwelveDescription,
        //                             LevelTwelveId = a.LevelTwelveId,
        //                             LastEntityID = a.LastEntityID,
        //                             ActiveEntityStatus = true
        //                         }).ToList();

        //    context.HighLevelViewTable.AddRange(highLevelData);
        //    context.SaveChanges();

        //    // Fetch last entity
        //    string lastEntityId = context.LicensedCompanyDetails.Select(x => x.EntityLimit).FirstOrDefault()?.ToString() ?? "0";

        //    return lastEntityId;
        //}
        #endregion

        #region SaveEmployee

        public async Task<object> AddEmployeeAsync(AddEmployeeDto inserEmployeeDto)
        {

            bool employeeExists = await _context.HrEmpMasters.AnyAsync(emp =>
                                        emp.SeperationStatus == 0 &&
                                        (emp.IsDelete == false || emp.IsDelete == null) &&
                                        emp.FirstName == inserEmployeeDto.FirstName &&
                                        EF.Functions.DateDiffDay(emp.JoinDt, inserEmployeeDto.JoinDt) == 0 &&
                                        EF.Functions.DateDiffDay(emp.EntryDt, inserEmployeeDto.EntryDt) == 0 &&
                                        EF.Functions.DateDiffDay(emp.DateOfBirth, inserEmployeeDto.DateOfBirth) == 0);

            if (employeeExists)
            {
                return "NotSaved";
            }
            else
            {
                //var transactionId = await _context.TransactionMasters.Where(t => t.TransactionType == "EmpCode").Select(t => t.TransactionId).FirstOrDefaultAsync();


                var EmpCodeTransactionId = await GetTransactionIdByTransactionType("EmpCode");

                var record = await _context.HighLevelViewTables.FirstOrDefaultAsync(x => x.LastEntityId == inserEmployeeDto.LastEntity);

                if (record != null)
                {
                    int chkEmpFirst = (int)record.LevelOneId;

                    // Collect non-zero level values (from LevelTwo to LevelTwelve)
                    var levels = new List<int?>
                                {
                                    record.LevelTwoId,
                                    record.LevelThreeId,
                                    record.LevelFourId,
                                    record.LevelFiveId,
                                    record.LevelSixId,
                                    record.LevelSevenId,
                                    record.LevelEightId,
                                    record.LevelNineId,
                                    record.LevelTenId,
                                    record.LevelElevenId,
                                    record.LevelTwelveId
                                };

                    string chkEmpEntities = string.Join(",", levels.Where(id => id != 0));

                    //---- Till this code -- logic is correct------------------

                    // Inputs (normally passed in or derived elsewhere)

                    int empId = 0;
                    //int transactionId = 21;
                    //string chkEmpEntities = "1,3,5,13211,21291,21431,21573,21638,21788,22088";
                    //int chkEmpFirst = 1;

                    // Output holders
                    string employeeCode = string.Empty;
                    string seqNumber = string.Empty;
                    string seqWithPrefix = string.Empty;
                    string seqWithSuffix = string.Empty;
                    string seqPrefixSuffix = string.Empty;

                    string? codeId = null;
                    int? errorId = null;
                    string errorMessage = string.Empty;

                    if (inserEmployeeDto.IsAutoCode == "Y")
                    {

                        codeId = GetSequenceAPI(empId, EmpCodeTransactionId, chkEmpEntities, chkEmpFirst);
                        if (codeId == null)
                        {
                            errorId = 0;
                            errorMessage = "-2"; // Sequence not generated

                        }

                        // Get code formatting (equivalent to dbo.GetSequencePrefixSuffixFormat)
                        employeeCode = GetSequencePrefixSuffixFormat_API(Convert.ToInt32(codeId));

                        var codeGenMaster = await _context.AdmCodegenerationmasters
                            .FirstOrDefaultAsync(x => x.CodeId == Convert.ToInt32(codeId));

                        if (codeGenMaster == null)
                        {
                            // Unexpected: CodeGen entry not found

                        }

                        string yearStr = DateTime.UtcNow.Year.ToString();
                        string finalCodeWithNoSeq = codeGenMaster.FinalCodeWithNoSeq;

                        // Prefix logic
                        seqWithPrefix = codeGenMaster.PrefixFormatId switch
                        {
                            1 => yearStr + finalCodeWithNoSeq,
                            2 => (codeGenMaster.Code ?? "") + finalCodeWithNoSeq,
                            _ => finalCodeWithNoSeq
                        };

                        // Suffix logic
                        seqWithSuffix = codeGenMaster.SuffixFormatId switch
                        {
                            1 => finalCodeWithNoSeq + yearStr,
                            2 => finalCodeWithNoSeq + (codeGenMaster.Suffix ?? ""),
                            _ => finalCodeWithNoSeq
                        };

                        // Combined Prefix + Suffix logic
                        seqPrefixSuffix = (codeGenMaster.PrefixFormatId, codeGenMaster.SuffixFormatId) switch
                        {
                            (1, 1) => yearStr + finalCodeWithNoSeq + yearStr,
                            (1, 2) => yearStr + finalCodeWithNoSeq + (codeGenMaster.Suffix ?? ""),
                            (2, 1) => (codeGenMaster.Code ?? "") + finalCodeWithNoSeq + yearStr,
                            (2, 2) => (codeGenMaster.Code ?? "") + finalCodeWithNoSeq + (codeGenMaster.Suffix ?? ""),
                            _ => finalCodeWithNoSeq
                        };

                        // Check for existing employee code
                        bool codeExists = await _context.HrEmpMasters
                            .AnyAsync(emp => EF.Functions.Like(emp.EmpCode.Trim(), employeeCode.Trim()) && (emp.IsDelete ?? false) == false);

                        if (codeExists)
                        {
                            errorId = 0;
                            errorMessage = "-3"; // Duplicate EmpCode

                        }

                        // Increment CurrentCodeValue
                        codeGenMaster.CurrentCodeValue = (codeGenMaster.CurrentCodeValue ?? 0) + 1;
                        await _context.SaveChangesAsync();

                        // Update FinalCodeWithNoSeq and LastSequence
                        string paddedValue = codeGenMaster.CurrentCodeValue.Value.ToString().PadLeft(codeGenMaster.NumberFormat.Length, '0');
                        codeGenMaster.FinalCodeWithNoSeq = paddedValue;
                        codeGenMaster.LastSequence = (codeGenMaster.Code ?? "") + paddedValue;

                        await _context.SaveChangesAsync();

                        // Set output
                        seqNumber = paddedValue;
                    }
                    else
                    {
                        // Manual code assignment fallback
                        seqNumber = employeeCode;
                        seqWithPrefix = employeeCode;
                        seqWithSuffix = employeeCode;
                        seqPrefixSuffix = employeeCode;
                    }
                    var mapping = await GetCategoryMapperData();

                    int branchMapId = mapping.FirstOrDefault(m => m.Code == GetCode(CatTrxType.BranchType))?.SortOrder ?? 0;
                    int depMapId = mapping.FirstOrDefault(m => m.Code == GetCode(CatTrxType.DepartmentType))?.SortOrder ?? 0;
                    int bandMapId = mapping.FirstOrDefault(m => m.Code == GetCode(CatTrxType.BandType))?.SortOrder ?? 0;
                    int designationMapId = mapping.FirstOrDefault(m => m.Code == GetCode(CatTrxType.DesigType))?.SortOrder ?? 0;
                    int countryMapId = mapping.FirstOrDefault(m => m.Code == GetCode(CatTrxType.CompanyType))?.SortOrder ?? 0;
                    int gradeIdMapId = mapping.FirstOrDefault(m => m.Code == GetCode(CatTrxType.GradeType))?.SortOrder ?? 0;

                    var sortOrder = await _context.LicensedCompanyDetails.Select(x => x.EntityLimit).FirstOrDefaultAsync();



                    var highLevelEntity = await _context.HighLevelViewTables
                        .FirstOrDefaultAsync(x => x.LastEntityId == inserEmployeeDto.LastEntity);


                    //inserEmployeeDto.BranchId = GetMappedLevelId(highLevelEntity, branchMapId);
                    //inserEmployeeDto.DepId = GetMappedLevelId(highLevelEntity, depMapId);
                    //inserEmployeeDto.BandId = GetMappedLevelId(highLevelEntity, bandMapId);
                    //inserEmployeeDto.GradeId = GetMappedLevelId(highLevelEntity, gradeIdMapId);
                    //inserEmployeeDto.DesigId = GetMappedLevelId(highLevelEntity, designationMapId);


                    var BranchId = GetMappedLevelId(highLevelEntity, branchMapId);
                    var DepId = GetMappedLevelId(highLevelEntity, depMapId);
                    var BandId = GetMappedLevelId(highLevelEntity, bandMapId);
                    var GradeId = GetMappedLevelId(highLevelEntity, gradeIdMapId);
                    var DesigId = GetMappedLevelId(highLevelEntity, designationMapId);

                    var currentDateTime = DateTime.UtcNow;

                    var empMaster = new HrEmpMaster
                    {
                        InstId = inserEmployeeDto.InstId,
                        EmpCode = inserEmployeeDto.empCode,
                        FirstName = inserEmployeeDto.FirstName,
                        MiddleName = inserEmployeeDto.MiddleName,
                        LastName = inserEmployeeDto.LastName,
                        GuardiansName = inserEmployeeDto.GuardiansName,
                        JoinDt = inserEmployeeDto.JoinDt,
                        EntryBy = inserEmployeeDto.EntryBy,
                        EntryDt = inserEmployeeDto.EntryDt,
                        EmpStatus = inserEmployeeDto.EmpStatus,
                        ReviewDt = inserEmployeeDto.ReviewDt,
                        NoticePeriod = inserEmployeeDto.NoticePeriod ?? 0,
                        IsProbation = inserEmployeeDto.IsProbation,
                        ProbationDt = inserEmployeeDto.ProbationDt,
                        DateOfBirth = inserEmployeeDto.DateOfBirth,
                        NationalIdNo = inserEmployeeDto.NationalIdNo,
                        PassportNo = inserEmployeeDto.PassportNo,
                        BranchId = BranchId,//  inserEmployeeDto.BranchId,
                        DepId = DepId,// inserEmployeeDto.DepId,
                        BandId = BandId,// inserEmployeeDto.BandId,
                        GradeId = GradeId,// inserEmployeeDto.GradeId,
                        DesigId = DesigId,// inserEmployeeDto.DesigId,
                        LastEntity = inserEmployeeDto.LastEntity,
                        Gender = inserEmployeeDto.Gender,
                        CurrentStatus = inserEmployeeDto.CurrentStatus,
                        SeperationStatus = inserEmployeeDto.SeperationStatus ?? 0,
                        CountryOfBirth = inserEmployeeDto.CountryOfBirth,
                        Ishra = inserEmployeeDto.Ishra,
                        GratuityStrtDate = inserEmployeeDto.GratuityStrtDate,
                        FirstEntryDate = inserEmployeeDto.FirstEntryDate,
                        IsExpat = inserEmployeeDto.IsExpat,
                        PublicHoliday = false,
                        CompanyConveyance = inserEmployeeDto.CompanyConveyance,
                        CompanyVehicle = inserEmployeeDto.CompanyVehicle,
                        InitialDate = inserEmployeeDto.InitialDate,
                        ModifiedDate = currentDateTime,
                        MealAllowanceDeduct = inserEmployeeDto.MealAllowanceDeduct,
                        InitialPaymentPending = inserEmployeeDto.InitialPaymentPending,
                        UpdatedBy = inserEmployeeDto.EntryBy,
                        UpdatedDate = currentDateTime,
                        EmpFileNumber = inserEmployeeDto.EmpFileNumber,
                        PayrollMode = inserEmployeeDto.PayrollMode,
                        DailyRateTypeId = inserEmployeeDto.DailyRateTypeId,
                        CanteenRequest = inserEmployeeDto.CanteenRequest == true ? 1 : 0,
                        EmpFirstEntity = chkEmpFirst.ToString(),// Not sure about its logic,

                    };

                    await _context.HrEmpMasters.AddAsync(empMaster);
                    await _context.SaveChangesAsync();

                    var empID = empMaster.EmpId;

                    var empAddress = new HrEmpAddress// no missing columns
                    {
                        InstId = inserEmployeeDto.InstId,
                        Add1 = inserEmployeeDto.Add1,
                        EmpId = empID,
                        Country = inserEmployeeDto.CountryID,
                        Pbno = inserEmployeeDto.PBNo,
                        PersonalEmail = inserEmployeeDto.PersonalEmail,
                        OfficialEmail = inserEmployeeDto.EmailId,
                        Phone = inserEmployeeDto.Phone,
                        Mobile = inserEmployeeDto.Mobile,
                        OfficePhone = inserEmployeeDto.OfficePhone,
                        HomeCountryPhone = inserEmployeeDto.HomeCountryPhone,
                        EntryBy = inserEmployeeDto.EntryBy,
                        EntryDt = currentDateTime,
                    };

                    await _context.HrEmpAddresses.AddAsync(empAddress);

                    var empPersonal = new HrEmpPersonal// no missing values into table
                    {
                        InstId = inserEmployeeDto.InstId,
                        EmpId = empID,
                        Dob = inserEmployeeDto.DateOfBirth,
                        Gender = inserEmployeeDto.Gender,
                        MaritalStatus = inserEmployeeDto.MaritalStatus,
                        Religion = inserEmployeeDto.ReligionID,
                        BloodGrp = inserEmployeeDto.BloodGrp,
                        Nationality = inserEmployeeDto.NationalityID,
                        Country = inserEmployeeDto.CountryID,
                        IdentMark = inserEmployeeDto.IdentMark,
                        EntryBy = inserEmployeeDto.EntryBy,
                        EntryDt = currentDateTime,
                        CountryOfBirth = inserEmployeeDto.CountryOfBirth,
                        Height = inserEmployeeDto.Height,
                        Weight = inserEmployeeDto.Weight,
                        WeddingDate = inserEmployeeDto.WeddingDate,
                    };

                    await _context.HrEmpPersonals.AddAsync(empPersonal);

                    var empReporting = new HrEmpReporting
                    {
                        InstId = inserEmployeeDto.InstId,
                        EmpId = empID,
                        ReprotToWhome = inserEmployeeDto.ReprotToWhome,
                        EffectDate = inserEmployeeDto.EffectDate,
                        Active = inserEmployeeDto.Active,
                        EntryBy = inserEmployeeDto.EntryBy,
                        EntryDate = currentDateTime,
                    };

                    await _context.HrEmpReportings.AddAsync(empReporting);

                    //-------------Testing need to be done 08 April 2025-----------Start--------

                    if (inserEmployeeDto.IsAutoCode == "Y")
                    {


                        var userIDTypeCode = await (from a in _context.CompanyParameters
                                                    join b in _context.HrmValueTypes
                                                    on a.Value equals b.Value
                                                    where a.ParameterCode == "USERIDMAPPING" && b.Type == "UserIDMapping"
                                                    select b.Code).FirstOrDefaultAsync();

                        // Get Employee Reporting Setting
                        var reportingSetting = await (from a in _context.CompanyParameters
                                                      join b in _context.HrmValueTypes
                                                      on a.Value equals b.Value
                                                      where a.ParameterCode == "ACTUSREMPCR" && b.Type == "EmployeeReporting"
                                                      select b.Code).FirstOrDefaultAsync();

                        // Determine active status
                        var active = (reportingSetting == "Yes") ? "Y" : "N";

                        // Get employee details
                        var employee2 = await _context.HrEmpMasters.FirstOrDefaultAsync(e => e.EmpId == empID);
                        if (employee2 == null)
                        {
                            return "NotSaved";
                        }

                        // Determine UserName based on type
                        string userName = userIDTypeCode switch
                        {
                            "UDP" => seqWithPrefix,// seqNumberWithPrefix,
                            "UDNP" => seqNumber,
                            "UDWS" => seqWithSuffix,// seqNumberWithSuffix,
                            "UDWPS" => seqPrefixSuffix,// seqNumberPrefixSuffix,
                            _ => employee2.EmpCode
                        };

                        // Determine Need_App value
                        //var needApp = (reportingSetting == "Yes") ? true : false;

                        // Create user entity
                        var user = new AdmUserMaster
                        {
                            UserName = userName,
                            DetailedName = $"{employee2.FirstName} {employee2.MiddleName} {employee2.LastName}",
                            Password = "kohNZpjfnsZdZdiqvYllow==", // You should encrypt this securely
                            EntryDate = employee2.EntryDt,
                            Active = active,
                            Status = "Y",
                            Email = inserEmployeeDto.EmailId,
                            NeedApp = inserEmployeeDto.MobileAppNeeded == 1 ? true : false,
                        };

                        // Insert and save
                        await _context.AdmUserMasters.AddAsync(user);
                        await _context.SaveChangesAsync();

                        // Set output UserID (if needed)
                        int newUserId = user.UserId; // Assuming UserId is an identity PK in ADM_User_Master
                    }
                    else
                    {
                        var employee1 = await _context.HrEmpMasters.Where(e => e.EmpId == empID)
                                            .Select(e => new
                                            {
                                                e.InstId,
                                                e.EmpId,
                                                e.EmpCode,
                                                e.FirstName,
                                                e.MiddleName,
                                                e.LastName,
                                                e.EntryDt,
                                                e.EntryBy
                                            })
                                            .FirstOrDefaultAsync();

                        if (employee1 == null)
                        {

                        }

                        // Create and insert ADM_User_Master
                        var newUser = new AdmUserMaster
                        {
                            UserName = employee1.EmpCode,
                            DetailedName = $"{employee1.FirstName} {employee1.MiddleName} {employee1.LastName}",
                            Password = "kohNZpjfnsZdZdiqvYllow==", // Replace with proper encryption
                            EntryDate = employee1.EntryDt,
                            Active = "Y",
                            Status = "Y",
                            Email = inserEmployeeDto.EmailId,
                            NeedApp = inserEmployeeDto.MobileAppNeeded == 1 ? true : false
                        };

                        await _context.AdmUserMasters.AddAsync(newUser);
                        await _context.SaveChangesAsync(); // to generate UserID (identity column)

                        int newUserId = newUser.UserId;

                        // Insert HR_EMPLOYEE_USER_RELATION
                        var empUserRelation = new HrEmployeeUserRelation// HR_EMPLOYEE_USER_RELATION
                        {
                            InstId = employee1.InstId,
                            UserId = newUserId,
                            EmpId = employee1.EmpId,
                            EntryBy = employee1.EntryBy,
                            EntryDt = employee1.EntryDt
                        };
                        await _context.HrEmployeeUserRelations.AddAsync(empUserRelation);

                        // Get default image from DB function
                        string imageUrl = DefaultImage.GetDefaultImages();

                        // Insert HR_EMP_IMAGES
                        var empImage = new HrEmpImage
                        {
                            InstId = employee1.InstId,
                            EmpId = employee1.EmpId,
                            ImageUrl = "default.jpg",
                            Active = "Y",
                            FingerUrl = "default.jpg",
                            EmpImage = imageUrl
                        };
                        await _context.HrEmpImages.AddAsync(empImage);

                        // Insert into ADM_UserRoleMaster
                        var userRole = new AdmUserRoleMaster
                        {
                            InstId = employee1.InstId,
                            RoleId = inserEmployeeDto.UserRole,//userRoleId, // This should be passed or available from context
                            UserId = newUserId,
                            Acess = 1
                        };
                        await _context.AdmUserRoleMasters.AddAsync(userRole);

                        // Update HR_EMP_MASTER
                        var empToUpdate = _context.HrEmpMasters.FirstOrDefault(e => e.EmpId == empID);
                        if (empToUpdate != null)
                        {
                            empToUpdate.EmpEntity = chkEmpEntities;
                            empToUpdate.EmpFirstEntity = chkEmpFirst.ToString();
                        }

                        await _context.SaveChangesAsync();
                    }


                    int? deviceID = 0;
                    var currentUTC = DateTime.UtcNow;

                    // Get common employee data once
                    var emp = await _context.HrEmpMasters
                        .Where(e => e.EmpId == empID)
                        .Select(e => new
                        {
                            e.EmpId,
                            e.EmpCode,
                            e.EmpFirstEntity,
                            e.EmpEntity,
                            e.Ishra,
                            e.CompanyConveyance,
                            e.CompanyVehicle
                        })
                        .FirstOrDefaultAsync();

                    if (emp == null) return "No";

                    // Handle Biometric Device ID and UserId
                    if (inserEmployeeDto.IsAutoCode == "Y")
                    {
                        var typeID = await _context.CompanyParameters
                            .Join(_context.HrmValueTypes,
                                  a => a.Value,
                                  b => b.Value,
                                  (a, b) => new { a, b })
                            .Where(x => x.a.ParameterCode == "BIOMAPPING" && x.b.Type == "BiometricMapping")
                            .Select(x => x.b.Code)
                            .FirstOrDefaultAsync();

                        if (typeID is "SEC" or "SECN" or "SECS" or "SECPS")
                        {
                            var empEntityList = (emp.EmpFirstEntity + "," + emp.EmpEntity).Split(',').ToList();

                            deviceID = await (from a in _context.CompanyParameters
                                              join b in _context.CompanyParameters01s on a.Id equals b.ParamId
                                              where a.ParameterCode == "DEVICEID" && empEntityList.Contains(b.LinkId.ToString())
                                              orderby b.LevelId
                                              select b.Value)
                                              .FirstOrDefaultAsync();

                            deviceID ??= await _context.CompanyParameters
                                              .Where(p => p.ParameterCode == "DEVICEID")
                                              .Select(p => p.Value)
                                              .FirstOrDefaultAsync();

                            var userID = typeID switch
                            {
                                "SECN" => seqNumber,
                                "SEC" => seqWithPrefix,
                                "SECS" => seqWithSuffix,
                                "SECPS" => seqPrefixSuffix,
                                _ => null
                            };

                            if (!string.IsNullOrEmpty(userID))
                            {
                                await _context.BiometricsDtls.AddAsync(new BiometricsDtl
                                {
                                    CompanyId = inserEmployeeDto.InstId,
                                    EmployeeId = empID,
                                    DeviceId = Convert.ToInt32(deviceID),
                                    UserId = userID,
                                    EntryBy = inserEmployeeDto.EntryBy,
                                    EntryDt = currentUTC
                                });
                                await _context.SaveChangesAsync();
                            }
                        }
                    }
                    else
                    {
                        await _context.BiometricsDtls.AddAsync(new BiometricsDtl
                        {
                            CompanyId = inserEmployeeDto.InstId,
                            EmployeeId = emp.EmpId,
                            DeviceId = (int)deviceID,
                            UserId = emp.EmpCode,
                            EntryBy = inserEmployeeDto.EntryBy,
                            EntryDt = currentUTC
                        });
                        await _context.SaveChangesAsync();
                    }

                    // Auto-assign PayrollPeriod and Payscale if setting enabled
                    var typeId = await (from a in _context.CompanyParameters
                                        join b in _context.HrmValueTypes on a.Value equals b.Value
                                        where a.ParameterCode == "ATASGNPAY" && b.Type == "EmployeeReporting"
                                        select b.Code).FirstOrDefaultAsync();

                    if (typeId == "Yes")
                    {
                        var empEntityIdlist = SplitAndParseEntityIds(emp.EmpEntity);
                        var firstEntityId = emp.EmpFirstEntity;

                        var payrollTransactionId = await _context.TransactionMasters
                            .Where(t => t.TransactionType == "PayrollPeriod")
                            .Select(t => t.TransactionId)
                            .FirstOrDefaultAsync();

                        var payrollPeriodId = await _context.Payroll00s
                            .Where(p => _context.EntityApplicable00s.Any(e =>
                                e.TransactionId == payrollTransactionId &&
                                ((e.LinkLevel == 1 && e.LinkId == Convert.ToInt64(firstEntityId)) ||
                                 (e.LinkLevel == 15) ||
                                 (e.LinkLevel != 1 && empEntityIdlist.Contains((int)e.LinkId)))))
                            .Select(p => p.PayrollPeriodId)
                            .FirstOrDefaultAsync();

                        if (payrollPeriodId is > 0)
                        {
                            await _context.PayPeriodMasterAccesses.AddAsync(new PayPeriodMasterAccess
                            {
                                EmployeeId = emp.EmpId,
                                PayrollPeriodId = payrollPeriodId,
                                IsCompanyLevel = 1,
                                CreatedBy = 1,
                                CreatedDate = currentUTC,
                                Active = "Y",
                                ValidDateFrom = currentUTC
                            });
                            await _context.SaveChangesAsync();
                        }

                        var payCodeTransactionId = await _context.TransactionMasters
                            .Where(t => t.TransactionType == "PayCodeBatch")
                            .Select(t => t.TransactionId)
                            .FirstOrDefaultAsync();

                        var payCodeMasterId = _context.PayCodeMaster00s
                            .Where(p => _context.EntityApplicable00s.Any(e =>
                                e.TransactionId == payCodeTransactionId &&
                                ((e.LinkLevel == 1 && e.LinkId == Convert.ToInt64(firstEntityId)) ||
                                 (e.LinkLevel == 15) ||
                                 (e.LinkLevel != 1 && empEntityIdlist.Contains((int)e.LinkId)))))
                            .Select(p => p.PayCodeMasterId)
                            .FirstOrDefault();

                        if (payCodeMasterId is > 0)
                        {
                            await _context.HrmPayscaleMasterAccesses.AddAsync(new HrmPayscaleMasterAccess
                            {
                                EmployeeId = emp.EmpId,
                                PayscaleMasterId = payCodeMasterId,
                                IsCompanyLevel = 1,
                                CreatedBy = 1,
                                CreatedDate = currentUTC,
                                Active = "Y",
                                ValidDatefrom = currentUTC
                            });
                            await _context.SaveChangesAsync();
                        }
                    }

                    // Insert initial records (HRA, Conveyance, Vehicle)
                    await _context.HraHistories.AddAsync(new HraHistory
                    {
                        EmployeeId = emp.EmpId,
                        IsHra = emp.Ishra,
                        FromDate = inserEmployeeDto.JoinDt,
                        Entryby = inserEmployeeDto.EntryBy,
                        Initial = 1
                    });

                    await _context.CompanyConveyanceHistories.AddAsync(new CompanyConveyanceHistory
                    {
                        EmployeeId = emp.EmpId,
                        CompanyConveyance = emp.CompanyConveyance,
                        FromDate = inserEmployeeDto.JoinDt,
                        EntryBy = inserEmployeeDto.EntryBy,
                        Initial = 1
                    });

                    await _context.CompanyVehicleHistories.AddAsync(new CompanyVehicleHistory
                    {
                        EmployeeId = emp.EmpId,
                        CompanyVehicle = emp.CompanyVehicle,
                        FromDate = inserEmployeeDto.JoinDt,
                        EntryBy = inserEmployeeDto.EntryBy,
                        Initial = 1
                    });

                    await _context.SaveChangesAsync();


                    var currentUtc = DateTime.UtcNow;

                    // Get transaction IDs where TransactionType is 'Document'
                    var documentTransactionIds = _context.TransactionMasters
                        .Where(tm => tm.TransactionType == "Document")
                        .Select(tm => tm.TransactionId)
                        .ToList();

                    // Get employee
                    var employee = _context.HrEmpMasters
                        .FirstOrDefault(e => e.EmpId == empID);
                    var empEntityIds = SplitAndParseEntityIds(emp.EmpEntity);
                    if (employee != null)
                    {
                        // var empEntities = SplitStrings_XML(employee.EmpEntity, default); // Assume returns List<string>



                        //var empEntityIds = empEntities.Select(int.Parse).ToList();



                        // Get b1: LinkLevel != 1
                        var b1 = _context.EntityApplicable00s
                            .Where(ea => documentTransactionIds.Contains((int)ea.TransactionId) && ea.LinkLevel != 1)
                            .ToList();

                        // Get b2: LinkLevel == 1
                        var b2 = _context.EntityApplicable00s
                            .Where(ea => documentTransactionIds.Contains((int)ea.TransactionId) && ea.LinkLevel == 1)
                            .ToList();

                        var activeDocs = _context.HrmsDocument00s
                            .Where(doc => doc.Active == true)
                            .ToList();

                        var docTypeMasters = _context.HrmsDocTypeMasters
                            .ToList();

                        var accessEntries = (
                            from doc in activeDocs
                            join dt in docTypeMasters on doc.DocType equals Convert.ToInt32(dt.DocTypeId)
                            where
                                (
                                    (b1.Any(x => empEntityIds.Contains((int)x.LinkId) && x.MasterId == doc.DocId)) ||
                                    (b2.Any(x => x.LinkId == Convert.ToInt32(employee.EmpFirstEntity) && x.MasterId == doc.DocId))
                                )
                            select new EmpDocumentAccess
                            {
                                InstId = 1,
                                EmpId = empID,
                                DocId = (int?)doc.DocId,
                                ValidFrom = currentUtc,
                                IsCompanyLevel = 0,
                                Status = 0,
                                CreatedBy = inserEmployeeDto.EntryBy,
                                CreatedDate = currentUtc
                            }
                        ).Distinct().ToList();

                        _context.EmpDocumentAccesses.AddRange(accessEntries);
                        _context.SaveChanges();
                    }
                    var leaveTransactionId = await GetTransactionIdByTransactionType("Leave");

                    // Step 2: Get all applicable LeaveMasterIds
                    var applicableLeaveMasterIds = await _context.EntityApplicable00s
                        .Where(ea =>
                            ea.TransactionId == leaveTransactionId &&
                            (
                                (ea.LinkLevel == 1 && ea.LinkId == inserEmployeeDto.empFirstEntiry) ||
                                ea.LinkLevel == 15 ||
                                (ea.LinkLevel != 1 && empEntityIds.Contains((int)ea.LinkId))
                            )
                        )
                        .Select(ea => ea.MasterId)
                        .Distinct()
                        .ToListAsync();

                    // Step 3: Get active leave masters matching the IDs
                    var leaveMasters = await _context.HrmLeaveMasters
                        .Where(lm => lm.Active == 1 && applicableLeaveMasterIds.Contains(lm.LeaveMasterId))
                        .ToListAsync();

                    //Step 4: Create LeaveAccess entries
                    var leaveAccessEntries = leaveMasters.Select(lm => new HrmLeaveEmployeeleaveaccess
                    {
                        EmployeeId = empID,
                        LeaveMaster = lm.LeaveMasterId,
                        IsCompanyLevel = 0,
                        CreatedBy = inserEmployeeDto.EntryBy,
                        CreatedDate = DateTime.UtcNow,
                        Status = 1,
                        FromDate = inserEmployeeDto.JoinDt
                    }).ToList();

                    // Step 5: Insert to DB
                    _context.HrmLeaveEmployeeleaveaccesses.AddRange(leaveAccessEntries);
                    await _context.SaveChangesAsync();

                    //-----------------------------------HrmLeaveBasicsettingsaccesses--------------------Start-------------------------

                    //                    var leaveBasicTransactionId = await GetTransactionIdByTransactionType("Leave_BS");

                    //                    var applicableSettingIds = await _context.EntityApplicable00s.Where(ea =>
                    //                                               ea.TransactionId == leaveBasicTransactionId &&
                    //                                               (
                    //                                                   (ea.LinkLevel == 1 && ea.LinkId == inserEmployeeDto.firtEntity) ||
                    //                                                   (ea.LinkLevel == 15) ||
                    //                                                   (ea.LinkLevel != 1 && empEntityIds.Contains((int)ea.LinkId))
                    //                                               )
                    //                    )
                    //                                               .Select(ea => ea.MasterId)
                    //                                               .Distinct()
                    //                                               .ToListAsync();
                    //                    var leaveBasicSettingsAccessList = await (
                    //    from s in _context.HrmLeaveMasterandsettingsLinks
                    //    join a in _context.HrmLeaveMasters on s.LeaveMasterId equals a.LeaveMasterId
                    //    where a.Active == 1 &&
                    //          inserEmployeeDto.m.Contains(s.LeaveMasterId) &&
                    //          applicableSettingIds.Contains(s.LeaveMasterId) // Assuming s.Id == Id_MasterandSettingsLink
                    //    select new HrmLeaveBasicsettingsaccess
                    //    {
                    //        EmployeeId = empId,
                    //        SettingsId = s.SettingsId,
                    //        LeaveMasterId = s.LeaveMasterId,
                    //        IsCompanyLevel = 0,
                    //        CreatedBy = entryBy,
                    //        CreatedDate = DateTime.UtcNow,
                    //        FromDateBs = joinDate,
                    //        Laps = 1.0m
                    //    }
                    //).ToListAsync();

                    //                    // 3. Save to DB
                    //                    _context.HrmLeaveBasicsettingsaccesses.AddRange(leaveBasicSettingsAccessList);
                    //                    await _context.SaveChangesAsync();


                    //-----------------------------------HrmLeaveBasicsettingsaccesses--------------------End-------------------------

                }



                ////////////---------------------------Code started on 16 April 2025---------------End------------------///////////






                //-------------Testing need to be done on 08 April 2025-----------End--------


                await _context.SaveChangesAsync();





            }
            return "Success";
        }




        private string GetCode(CatTrxType type)
        {
            return type switch
            {
                CatTrxType.BranchType => "BRANCH",
                CatTrxType.DepartmentType => "DEPT",
                CatTrxType.BandType => "BAND",
                CatTrxType.GradeType => "GRADE",
                CatTrxType.DesigType => "DESIG",
                CatTrxType.CompanyType => "COMPANY",
                _ => ""
            };
        }

        private int? GetMappedLevelId(HighLevelViewTable entity, int mapId)
        {
            return mapId switch
            {
                1 => entity.LevelOneId,
                2 => entity.LevelTwoId,
                3 => entity.LevelThreeId,
                4 => entity.LevelFourId,
                5 => entity.LevelFiveId,
                6 => entity.LevelSixId,
                7 => entity.LevelSevenId,
                8 => entity.LevelEightId,
                9 => entity.LevelNineId,
                10 => entity.LevelTenId,
                11 => entity.LevelElevenId,
                _ => null
            };
        }
        private string GetSequencePrefixSuffixFormat_API(int codeID)
        {
            var currentYear = DateTime.UtcNow.Year.ToString();

            var codeGen = _context.AdmCodegenerationmasters
                .Where(c => c.CodeId == codeID)
                .Select(c => new
                {
                    BaseSeq = c.FinalCodeWithNoSeq,
                    PrefixFormatID = c.PrefixFormatId ?? 0,
                    SuffixFormatID = c.SuffixFormatId ?? 0,
                    Prefix = c.Code ?? "",
                    Suffix = c.Suffix ?? ""
                })
                .FirstOrDefault();

            if (codeGen == null)
                return null;

            // Apply prefix
            var formattedSeq = codeGen.PrefixFormatID switch
            {
                1 => currentYear + codeGen.BaseSeq,
                2 => codeGen.Prefix + codeGen.BaseSeq,
                _ => codeGen.BaseSeq
            };

            // Apply suffix
            formattedSeq = codeGen.SuffixFormatID switch
            {
                1 => formattedSeq + currentYear,
                2 => formattedSeq + codeGen.Suffix,
                _ => formattedSeq
            };

            return formattedSeq;
        }
        public string GetSequenceAPI(int employeeId, int mainMasterId, string entityCsv = "", int firstEntity = 0)
        {
            string sequence = null;
            int? codeId = null;

            var seqGenTransactionIds = _context.TransactionMasters
                .Where(t => t.TransactionType == "Seq_Gen")
                .Select(t => t.TransactionId)
                .ToList();


            // Level 17 logic
            if (_context.LevelSettingsAccess00s.Any(x => seqGenTransactionIds.Contains((int)x.TransactionId) && x.Levels == "17"))
            {
                if (!string.IsNullOrEmpty(entityCsv))
                {
                    var entityList = entityCsv.Split(',').Select(int.Parse).ToList();

                    var entityMatch = (from a in _context.EntityApplicable00s
                                       join c in _context.AdmCodegenerationmasters on a.MasterId equals c.CodeId
                                       where seqGenTransactionIds.Contains((int)a.TransactionId)
                                             && a.MainMasterId == mainMasterId
                                             && a.LinkLevel != 1
                                             && entityList.Contains((int)a.LinkId)
                                       orderby a.LinkLevel
                                       select new { c.CodeId, c.LastSequence }).FirstOrDefault();

                    if (entityMatch != null)
                    {
                        codeId = entityMatch.CodeId;
                        sequence = entityMatch.LastSequence;
                    }

                    if (sequence == null && firstEntity != 0)
                    {
                        var firstEntityMatch = (from a in _context.EntityApplicable00s
                                                join c in _context.AdmCodegenerationmasters on a.MasterId equals c.CodeId
                                                where seqGenTransactionIds.Contains((int)a.TransactionId)
                                                      && a.MainMasterId == mainMasterId
                                                      && a.LinkLevel == 1
                                                      && a.LinkId == firstEntity
                                                orderby a.LinkLevel
                                                select new { c.CodeId, c.LastSequence }).FirstOrDefault();

                        if (firstEntityMatch != null)
                        {
                            codeId = firstEntityMatch.CodeId;
                            sequence = firstEntityMatch.LastSequence;
                        }
                    }

                    if (sequence == null)
                    {
                        var defaultMatch = (from a in _context.EntityApplicable00s
                                            join c in _context.AdmCodegenerationmasters on a.MasterId equals c.CodeId
                                            where seqGenTransactionIds.Contains((int)a.TransactionId)
                                                  && a.MainMasterId == mainMasterId
                                                  && a.LinkLevel == 15
                                            select new { c.CodeId, c.LastSequence }).FirstOrDefault();

                        if (defaultMatch != null)
                        {
                            codeId = defaultMatch.CodeId;
                            sequence = defaultMatch.LastSequence;
                        }
                    }
                }
                else
                {
                    var emp = _context.HrEmpMasters.FirstOrDefault(e => e.EmpId == employeeId);
                    if (emp != null)
                    {
                        var empEntities = emp.EmpEntity.Split(',').Select(int.Parse).ToList();

                        var entityMatch = (from b in _context.EntityApplicable00s
                                           join c in _context.AdmCodegenerationmasters on b.MasterId equals c.CodeId
                                           where seqGenTransactionIds.Contains((int)b.TransactionId)
                                                 && b.MainMasterId == mainMasterId
                                                 && b.LinkLevel != 1
                                                 && empEntities.Contains((int)b.LinkId)
                                           orderby b.LinkLevel
                                           select new { c.CodeId, c.LastSequence }).FirstOrDefault();

                        if (entityMatch != null)
                        {
                            codeId = entityMatch.CodeId;
                            sequence = entityMatch.LastSequence;
                        }

                        if (sequence == null)
                        {
                            var firstMatch = (from b in _context.EntityApplicable00s
                                              join c in _context.AdmCodegenerationmasters on b.MasterId equals c.CodeId
                                              where seqGenTransactionIds.Contains((int)b.TransactionId)
                                                    && b.MainMasterId == mainMasterId
                                                    && b.LinkLevel == 1
                                                    && b.LinkId == Convert.ToInt32(emp.EmpFirstEntity)
                                              orderby b.LinkLevel
                                              select new { c.CodeId, c.LastSequence }).FirstOrDefault();

                            if (firstMatch != null)
                            {
                                codeId = firstMatch.CodeId;
                                sequence = firstMatch.LastSequence;
                            }
                        }

                        if (sequence == null)
                        {
                            var defaultMatch = (from a in _context.EntityApplicable00s
                                                join c in _context.AdmCodegenerationmasters on a.MasterId equals c.CodeId
                                                where seqGenTransactionIds.Contains((int)a.TransactionId)
                                                      && a.MainMasterId == mainMasterId
                                                      && a.LinkLevel == 15
                                                select new { c.CodeId, c.LastSequence }).FirstOrDefault();

                            if (defaultMatch != null)
                            {
                                codeId = defaultMatch.CodeId;
                                sequence = defaultMatch.LastSequence;
                            }
                        }
                    }
                }
            }
            // Level 16 (Company level)
            else if (_context.LevelSettingsAccess00s.Any(x => seqGenTransactionIds.Contains((int)x.TransactionId) && x.Levels == "16"))
            {
                var master = _context.AdmCodegenerationmasters
                    .Where(x => x.TypeId == mainMasterId)
                    .Select(x => new { x.CodeId, x.LastSequence })
                    .FirstOrDefault();

                if (master != null)
                {
                    codeId = master.CodeId;
                    sequence = master.LastSequence;
                }
            }
            // Other levels (employee-specific access)
            else if (_context.LevelSettingsAccess00s.Any(x => seqGenTransactionIds.Contains((int)x.TransactionId) && x.Levels != "16" && x.Levels != "17"))
            {
                var empMatch = (from a in _context.AdmCodegenerationmasters
                                join b in _context.TransactionMasters on a.TypeId equals b.TransactionId
                                join c in _context.EmployeeSequenceAccesses on a.CodeId equals c.SequenceId
                                where b.TransactionId == mainMasterId
                                      && c.EmployeeId == employeeId
                                select a.CodeId).FirstOrDefault();

                if (empMatch != 0)
                    codeId = empMatch;
            }

            return codeId?.ToString() ?? "";
        }

        private async Task<List<CategoryMappingDto>> GetCategoryMapperData()
        {
            var mappings = await _context.Categorymasters
                .Join(_context.HrmValueTypes,
                      cat => cat.CatTrxTypeId,
                      val => val.Value,
                      (cat, val) => new { cat, val })
                .Where(x => x.val.Type == "CatTrxType" &&
                            new[] { "BRANCH", "DEPT", "BAND", "GRADE", "DESIG", "COMPANY" }
                            .Contains(x.val.Code))
                .GroupBy(x => x.val.Code)
                .Select(g => new CategoryMappingDto
                {
                    Code = g.Key,
                    SortOrder = g.Max(x => x.cat.SortOrder)
                })
                .ToListAsync();

            return mappings;
        }

        #endregion

        public async Task<List<object>> RetrieveShiftEmpCreationAsync()
        {
            var result = await _context.HrShift00s
                .Select(s => new
                {
                    s.ShiftId,
                    s.ShiftName
                })
                .ToListAsync<object>();

            return result;
        }
        public async Task<List<object>> FillWeekEndShiftEmpCreationAsync()
        {
            var result = await _context.WeekEndMasters
                .Select(w => new
                {
                    w.WeekEndMasterId,
                    w.Name


                })
                .ToListAsync<object>();

            return result;
        }
        public async Task<List<object>> FillbatchslabsEmpAsync(int batchId)
        {
            var result = await _context.SpecialcomponentsBatchSlabs
                .Where(s => s.BatchId == batchId && (s.Active == 1)) // handles null as true
                .Select(s => new
                {
                    s.SpecialcomponentsBatchSlab1,
                    s.BatchId,
                    s.BatchSlabDescripttion,
                    s.EntryBy,
                    s.EntryDate,
                    s.Active
                    // Add other fields you want to return
                })
                .ToListAsync<object>();

            return result;
        }
        public async Task<int> EnableBatchOptionEmpwiseAsync(int empId)
        {
            var slabId = GetEmployeeSchemeID(empId, "ASSPAYCODE", "PRL").Result;  // Block the async call (NOT ideal)
            return Convert.ToInt32(slabId);
        }




    }
}
