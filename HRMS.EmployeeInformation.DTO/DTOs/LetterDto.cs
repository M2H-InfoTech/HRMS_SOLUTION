using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPLOYEE_INFORMATION.DTO.DTOs
{
    public class LetterDto
    {
        public int LetterReqID { get; set; }
        public string? LetterSubName { get; set; }
        public int? EmpID { get; set; }
        public string? ReleaseDate { get; set; }
        public int? AppointmentLetter { get; set; }
    }
}
