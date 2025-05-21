using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.EmployeeInformation.DTO.DTOs.WorkFlow
{
    public class TempLeaveWorkFlowDto
    {
        public int? RequestId { get; set; }
        public int? ShowStatus { get; set; }
        public string? ApprovalStatus { get; set; }
        public int? Rule { get; set; }
        public int? RuleOrder { get; set; }
        public bool? HierarchyType { get; set; }
        public int? Approver { get; set; }
        public string? ApprovalRemarks { get; set; }
        public int? EntryBy { get; set; }
        public DateTime? EntryDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? Delegate { get; set; }
        public int? WorkFlowID { get; set; } = 0;
        public int? FlowRoleID { get; set; } = 0;
        public int ForwardNext { get; set; } = 0;
        public int HideFlow { get; set; } = 0;
        public int WorkflowType { get; set; } = 0;
        public int? IsSelf { get; set; }
    }
}
