using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.EmployeeInformation.DTO.DTOs
{
    public class SaveSkillSetDto
    {

        public string Course { get; set; }
        public int InstId { get; set; }
        public int Emp_Id { get; set; }
        public string Course_Dtls { get; set; }
        public string Year { get; set; }
        public DateTime DurationFrom { get; set; }
        public DateTime DurationTo { get; set; }
        public DateTime EntryDt { get; set; }
        public string RequestId { get; set; }
        public string Mark_Per { get; set; }
        public string Status { get; set; }
        public string Inst_Name { get; set; }
        public string FlowStatus { get; set; }

        public int Entry_By { get; set; }
        public string Request_ID { get; set; }
        public DateTime DateFrom { get; set; }

        public string langSkills { get; set; }

        public int DetailID { get; set; }
    }



}