using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.EmployeeInformation.DTO.DTOs
{
    public class AssetParameterDto
    {
        public int? Value { get; set; }  // ✅ Nullable int to handle NULL values
        public string ParameterCode { get; set; }
        public string Description { get; set; }
    }

}
