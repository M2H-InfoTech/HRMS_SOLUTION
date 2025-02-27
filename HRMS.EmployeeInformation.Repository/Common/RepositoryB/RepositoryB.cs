using AutoMapper;
using EMPLOYEE_INFORMATION.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using MPLOYEE_INFORMATION.DTO.DTOs;


namespace HRMS.EmployeeInformation.Repository.Common.RepositoryB
{
    public class RepositoryB : IRepositoryB
    {
        private readonly EmployeeDBContext _context;
        //private IStringLocalizer _stringLocalizer;
        private readonly IMemoryCache _memoryCache;
        private readonly EmployeeSettings _employeeSettings;
        private int paramDynVal;

        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;
        public RepositoryB(EmployeeDBContext dbContext, EmployeeSettings employeeSettings, IMemoryCache memoryCache, IMapper mapper, IWebHostEnvironment env)
        {
            _context = dbContext;
            _employeeSettings = employeeSettings;
            _memoryCache = memoryCache;
            _mapper = mapper;
            _env = env ?? throw new ArgumentNullException(nameof(env));
        }

        public async Task<List<object>> QualificationDocumentsDetails(int QualificationId)
        {
            return await (from a in _context.QualificationAttachments
                          where a.QualificationId == QualificationId && a.DocStatus == "A"
                          select new
                          {
                              a.QualAttachId,
                              a.QualificationId,
                              a.QualFileName,
                              a.DocStatus,
                              a.EmpId
                          }).AsNoTracking().ToListAsync<object>();
        }


    }
}
