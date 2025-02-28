using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.EmployeeInformation.DTO.DTOs
    {
    public class EditDependentEducationDto
        {
        public string? UniversityEdu { get; set; }
        public string? Year { get; set; }
        public string? CourseType { get; set; }
        public int? EducId { get; set; }
        public int? CourseId { get; set; }
        public int? EdSpecId { get; set; }
        public int? UniId { get; set; }
        }
    }
