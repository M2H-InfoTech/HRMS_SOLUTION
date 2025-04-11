using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.EmployeeInformation.DTO.DTOs
{
    public class ParamWorkFlowViewDto
    {
        public long ValueId { get; set; }
        public int WorkFlowId { get; set; }
        public int TransactionId { get; set; }
        public string? Description { get; set; }
    }

    public class ParamWorkFlow01s2sDto
    {
        public long ValueId { get; set; }

        public int? LinkId { get; set; }

        public int? TransactionId { get; set; }

        public int? WorkFlowId { get; set; }

        public int? LinkLevel { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public int? LinkEmpId { get; set; }
    }
}
