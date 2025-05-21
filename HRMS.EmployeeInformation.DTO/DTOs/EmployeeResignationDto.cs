using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.EmployeeInformation.DTO.DTOs
{
    public class EmployeeResignationDto
    {
        public int Emp_Id { get; set; }
        public string Name { get; set; }
        public string Emp_Code { get; set; }
        public string Designation { get; set; }
        public string? DateOfBirth { get; set; }
        public string? Join_Dt { get; set; }
        public string Join_Date { get; set; }
        public string ServiceLength { get; set; }
        public string BranchName { get; set; }
        public string Url { get; set; }
        public string Grade { get; set; }
        public string Address { get; set; }
        public string Mobile { get; set; }
        public string LOS { get; set; }
        public string ExtensionCode { get; set; }
        public int NoticePeriod { get; set; }
    }
    public class EmployeeDetailDto
    {
        public int Emp_Id { get; set; }
        public string Name { get; set; }
        public string Emp_Code { get; set; }
        public string Designation { get; set; }
        public string? DateOfBirth { get; set; }
        public string Join_Dt { get; set; }
        public string Join_Date { get; set; }
        public string ServiceLength { get; set; }
        public string BranchName { get; set; }
        public string Url { get; set; }
        public string Grade { get; set; }
        public string Address { get; set; }
        public string Mobile { get; set; }
        public string LOS { get; set; }
    }
}
