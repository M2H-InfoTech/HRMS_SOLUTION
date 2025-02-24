using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.EmployeeInformation.DTO.DTOs
    {
    public class SaveDependentEmpDto
        {
        public int EmpId { get; set; }
        public string? Description { get; set; }
        public string? Name { get; set; }
        public int EntryBy { get; set; }
        public int DependentID { get; set; }
        public string? Type { get; set; }
        public string? Gender { get; set; }
        public string? DateOfBirth { get; set; }
        public string? IdentificationNo { get; set; }
        public int RelationshipId { get; set; }
        public int SchemeId { get; set; }
        public bool TicketEligible { get; set; }
        public string? FileName { get; set; }
        public string? FileType { get; set; }
        public byte[]? Bytedepndent { get; set; }
        public bool IsEducation { get; set; }
        public int EducationID { get; set; }
        public int CourseID { get; set; }
        public int SpecialID { get;set; }
        public string? Year { get; set; }
        public string? CourseType { get; set; }
        public int EdUniversity { get; set; }

        }
    }
