using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.EmployeeInformation.DTO.DTOs.WorkFlow
{
    public class WorkFlowActivityFlowLeaveAppParametersDto
    {
        public int EmployeeID { get; set; } = 0;
        public string? TransactionType { get; set; } = "";
        public int RequestID { get; set; } = 0;
        public DateTime? EntryDate { get; set; }
        public bool ShowStatus { get; set; } = false;
        public string? ApprovalRemarks { get; set; } = "";
        public int EntryBy { get; set; } = 0;
        public int UpdatedBy { get; set; } = 0;
        public DateTime? UpdatedDate { get; set; }
        public string? Delegate { get; set; } = "0";
        public bool ReturnWorkFlowTable { get; set; } = false;
        public int SpecialWorkFlowID { get; set; } = 0;
        public string? EntryFrom { get; set; } = "";
        public int WorkflowType { get; set; } = 1;
    }
    public class WorkflowRuleResult
    {
        public int RuleOrder { get; set; }
        public int FlowRoleId { get; set; }
        public int EmployeeId { get; set; }
        public int CheckReporting { get; set; }
    }

}
