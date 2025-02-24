using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.EmployeeInformation.DTO.DTOs
    {
    
        public class FillTravelTypeDto
            {
            public IEnumerable<TraveltypeDto>? Traveltype { get; set; }
            public IEnumerable<DependentMaster1Dto>? DependentMaster1 { get; set; }
            public IEnumerable<DependentMaster1Dto>? AllDependents { get; set; } 
            }
        
    }
