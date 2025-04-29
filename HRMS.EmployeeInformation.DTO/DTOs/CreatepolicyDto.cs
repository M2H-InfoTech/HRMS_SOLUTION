using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Xml.Linq;

namespace HRMS.EmployeeInformation.DTO.DTOs
{
    public class CreatePolicyDto
    {
        public int LeavePolicyMasterID { get; set; }
        public int Inst_Id { get; set; }
        public string Name { get; set; }
        public int BlockMultiUnapprovedLeaves { get; set; }
        public int EntryBy { get; set; }
        public int EmpId { get; set; }
    }

}