using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.EmployeeInformation.DTO.DTOs
{
    public class ShiftMasterAccessInputDto
    {
        public int ShiftId { get; set; }
        public int CreatedBy { get; set; }
        public DateTime ValidDateFrom { get; set; }
        public DateTime ValidDateTo { get; set; }
        public int WeekEndMasterId { get; set; }
    }

}
