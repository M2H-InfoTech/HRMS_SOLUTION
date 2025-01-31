using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPLOYEE_INFORMATION.DTO.DTOs
{
    public class DisciplinaryActionsDto
    {
        public int? EmpID { get; set; }
        public string? LetterName { get; set; }
        public string? Reason { get; set; }
        public string? LetterSubName { get; set; }
        public int? IsLetterAttached { get; set; }
        public string? ReleaseDate { get; set; }
        public int? LetterReqID { get; set; }
        public string? ApprovalStatus { get; set; }
        public string? UploadFileName { get; set; }
        public string? IssueDate { get; set; }
        public int? Template { get; set; }
    }
}
