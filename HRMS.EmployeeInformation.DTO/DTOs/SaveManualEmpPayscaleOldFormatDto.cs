using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.EmployeeInformation.DTO.DTOs
{
    //Shan Lal Created
    public class SaveManualEmpPayscaleOldFormatDto
    {
        public string? EmployeeIDs { get; set; }
        public int EmployeeId { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public int PayrollType { get; set; }
        public int PaycodeId { get; set; }
        public int EntryBy { get; set; }
        public int IncrementRequestID { get; set; }
        public int SlabId { get; set; }
        public string? Status { get; set; }
        public float SlabAmount { get; set; }
        public string? XmlStringArrear { get; set; }
        public string? Remarks { get; set; }
        public int IsEditExclude { get; set; }
        public string? ExcludeComp { get; set; }
        public List<EarnDto>? earnDtos { get; set; }
        public List<DedDto>? dededDtos { get; set; }

    }
    public class EmployeePayscaleResult
    {
        public int EmployeeId { get; set; }
        public string EmpCode { get; set; }
        public string? EffectiveDate { get; set; } // formatted as "dd/MM/yyyy"
        public DateTime? RawEffectiveDate { get; set; } // original datetime
        public int Data { get; set; }
        public int PrlAppr { get; set; }
    }
    public class EarnDto
    {
        public int? EarnId { get; set; }
        public double? EarnNewAmount { get; set; } // Using double for float equivalent in C#
    }

    public class DedDto
    {
        public int? DedId { get; set; }
        public double? DedNewAmount { get; set; }
    }
}
