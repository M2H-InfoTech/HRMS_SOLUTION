using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.EmployeeInformation.DTO.DTOs
{
    public class AssetCategoryCodeDto
    {
        public int Id { get; set; }
        public string? Code { get; set; }
        public string? Description { get; set; }
        public int? Value { get; set; }
        public DateTime? Createdby { get; set; }
        public string? Status { get; set; }
        public string? Assetclass { get; set; }
        public string? AssetModel { get; set; }

    }

}
