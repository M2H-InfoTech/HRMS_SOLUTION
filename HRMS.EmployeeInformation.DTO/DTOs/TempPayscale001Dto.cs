using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.EmployeeInformation.DTO.DTOs
{
    public class TempPayscale001Dto
    {
        public long PayScaleId { get; set; }
        public long? PayRequest01Id { get; set; }
        public long? PayRequestId { get; set; }
        public int? BatchId { get; set; }
        public string EmployeeCode { get; set; }
        public int? EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public int? EmployeeStatus { get; set; }
        public DateTime? EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
    }

}
