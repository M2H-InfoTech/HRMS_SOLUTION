using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.EmployeeInformation.DTO.DTOs
{
    public class CreateMasterDto
    {
        public int MasterId { get; set; }
        public string LeaveCode { get; set; }
        public string Description { get; set; }
        public int PayType { get; set; }
        public int LeaveUnit { get; set; }
        public int Active { get; set; }
        public int CreatedBy { get; set; }
        public string Colour { get; set; }
    }
}
