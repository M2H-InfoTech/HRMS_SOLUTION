using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPLOYEE_INFORMATION.DTO.DTOs
{
    public class SkillSetDto
    {
        public int Tech_id { get; set; }
        public int? Emp_Id { get; set; }
        public string? Course { get; set; }
        public string? Course_Dtls { get; set; }
        public string? Inst_Name { get; set; }
        public string? Dur_Frm { get; set; }
        public string? Dur_To { get; set; }
        public string? Year { get; set; }
        public string? Mark_Per { get; set; }
        public string? langSkills { get; set; }
        public string? Status { get; set; }
    
    }
}
