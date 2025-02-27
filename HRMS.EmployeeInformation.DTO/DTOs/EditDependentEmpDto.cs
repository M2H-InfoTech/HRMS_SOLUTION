using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.EmployeeInformation.DTO.DTOs
    {
    public class EditDependentEmpDto
        {
        public int depid { get; set; }
        public string? Name { get; set; }
        public string? Gender { get; set; }
        public string? DateOfBirth { get; set; }
        public int? RelationshipId { get; set; }
        public string? RelationShip { get; set; }
        public string? Description { get; set; }
        public int? DepEmpId { get; set; }
        public string? DocFileType { get; set; }
        public string? DocFileName { get; set; }
        public byte[]? FileData { get; set; }
        public int? InterEmpID { get; set; }
        public string? Type { get; set; }
        public bool? TicketEligible { get; set; }
        public int? DocumentID { get; set; }
        public string? IdentificationNo { get; set; }
        }
    }
