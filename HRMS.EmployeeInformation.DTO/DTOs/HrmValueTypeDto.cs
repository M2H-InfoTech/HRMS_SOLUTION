using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.EmployeeInformation.DTO.DTOs
{
    public  class HrmValueTypeDto
    {
        public int Id { get; set; }

        public string? Type { get; set; }

        public int? Value { get; set; }

        public string? Code { get; set; }

        public string? Description { get; set; }


        public int? ReqId { get; set; }
    }
}
