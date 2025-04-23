using System;

namespace HRMS.EmployeeInformation.DTO.DTOs
{
    public class AssignEmployeeAccessRequestDto
    {
        public int empId { get; set; }
        public int entryBy { get; set; }
        public string? EmpEntity { get; set; }
        public string? EmpFirstEntity { get; set; }
        public DateTime? JoinDt { get; set; }

    }
}