using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.EmployeeInformation.DTO.DTOs
{
    public class SaveParamWorkflowDto
    {
        public int LinkLevel { get; set; }
        public int LinkId { get; set; }
        public int WorkFlowId { get; set; }
        public string ModuleIds { get; set; }  // Comma-separated string (e.g., "101,102,103")
        public int CreatedBy { get; set; }
    }

}
