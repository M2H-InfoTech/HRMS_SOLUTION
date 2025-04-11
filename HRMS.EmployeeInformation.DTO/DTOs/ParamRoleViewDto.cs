using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.EmployeeInformation.DTO.DTOs
{
    public class ParamRoleViewDto
    {
        public long ValueId { get; set; }
        public int ParamID { get; set; }
        public string? ParamDescription { get; set; }
        public int? Emp_Id { get; set; }
        public string EmployeeName { get; set; }
    }

    public class ParamRole01AND02Dto
    {
        public long ValueId { get; set; }

        public int? LinkId { get; set; }

        public int? ParameterId { get; set; }

        public int? EmpId { get; set; }

        public int? LinkLevel { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; } = DateTime.UtcNow;

        public int? ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; } = DateTime.UtcNow;

        public int? LinkEmpId { get; set; }

    }

    public class UpdateResult
    {
        public string Message { get; set; }
        public int StatusCode { get; set; }

        public UpdateResult (string message, int statusCode)
        {
            Message = message;
            StatusCode = statusCode;
        }
    }

}
