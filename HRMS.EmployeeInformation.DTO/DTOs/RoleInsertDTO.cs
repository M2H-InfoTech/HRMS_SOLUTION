using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.EmployeeInformation.DTO.DTOs
{
    public class RoleInsertDTO
    {
        public int ParameterId  { get; set; }
        public int EmpId { get; set; }
        public int LinkId { get; set; }
        public int WorkFlowId { get; set; }
        public int LinkLevel { get; set; }
        public int CreatedBy { get; set; }
    }
}
