using AutoMapper;
using EMPLOYEE_INFORMATION.Data;
using HRMS.EmployeeInformation.DTO.DTOs;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using MPLOYEE_INFORMATION.DTO.DTOs;

namespace HRMS.EmployeeInformation.Repository.Common.RepositoryC
{
    public class RepositoryC : IRepositoryC
    {
        private readonly EmployeeDBContext _context;
        //private IStringLocalizer _stringLocalizer;
        private readonly IMemoryCache _memoryCache;
        private readonly EmployeeSettings _employeeSettings;
        private int paramDynVal;

        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;
        public RepositoryC(EmployeeDBContext dbContext, EmployeeSettings employeeSettings, IMemoryCache memoryCache, IMapper mapper, IWebHostEnvironment env)
        {
            _context = dbContext;
            _employeeSettings = employeeSettings;
            _memoryCache = memoryCache;
            _mapper = mapper;
            _env = env ?? throw new ArgumentNullException(nameof(env));
        }

        public async Task<FillTravelTypeDto> FillTravelType ( )
            {
            var result = new FillTravelTypeDto
                {
                
                Traveltype = await _context.TravelTypes
                    .Select (t => new TraveltypeDto
                        {
                        TravelType_Id = t.TravelTypeId, 
                        TravelType = t.TravelType1,    
                        value = t.Value                 
                        })
                    .ToListAsync ( ),

                // Fetching only dependents where Self != 1
                DependentMaster1 = await _context.DependentMasters
                    .Where (d => d.Self != 1) 
                    .Select (d => new DependentMaster1Dto
                        {
                        DependentId = d.DependentId,
                        Description = d.Description
                        })
                    .ToListAsync ( ),

                // Fetching all dependents
                AllDependents = await _context.DependentMasters
                    .Select (d => new DependentMaster1Dto
                        {
                        DependentId = d.DependentId,
                        Description = d.Description
                        })
                    .ToListAsync ( )
                };

            return result;
            }


        }
    }
