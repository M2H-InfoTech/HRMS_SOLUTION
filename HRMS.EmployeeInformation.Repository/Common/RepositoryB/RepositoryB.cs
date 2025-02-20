using AutoMapper;
using EMPLOYEE_INFORMATION.Data;
using Microsoft.AspNetCore.Hosting;
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
    }
}
