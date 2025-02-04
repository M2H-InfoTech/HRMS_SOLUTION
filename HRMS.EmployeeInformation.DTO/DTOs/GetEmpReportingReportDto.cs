using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.EmployeeInformation.DTO.DTOs
    {
    public class GetEmpReportingReportDto
        {
        public string? Emp_Code { get; set; }
        public string? Name { get; set; }
        public string? ReportingEmpCode { get; set; }
        public string? ReporteeName { get; set; }
        public string? FromDate { get;set; }
        public string? ToDate { get; set; }
        
        }
    }
