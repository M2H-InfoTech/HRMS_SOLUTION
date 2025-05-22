namespace LEAVE.Dto
{
    public class EmployeeDetailWFDto
    {
        public int? EmployeeID { get; set; }
        public string TransactionType { get; set; }
        public int specialWorkFlowId { get; set; }
        public int requestId { get; set; }
        public int workflowType { get; set; }
        public DateTime CommonDate { get; set; }
        public int GrievanceWistleBlower { get; set; }
        public int SpecialWorkFlowSubID { get; set; }
    }
    public class WorkFlowActivityFlowLeaveAppDto
    {
        public int? EmployeeID { get; set; } = 0;
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
        public int TransactionId { get; set; } = 0; 
        public int? WorkFlowID { get; set; } = 0;
        public string? FinalRule { get; set; } = "";
        public int WorkflowRuleId { get; set; } = 0;
        public bool LinkToEmp { get; set; } = false;
        public string? LinkToEntity { get; set; } = "";
        public int LinkID { get; set; } = 0;
        public int Count { get; set; } = 0;
        public int RoleValue { get; set; } = 0;
        public string? ParameterId { get; set; } = "";
        public bool? HierarchyType { get; set; } = false;
        public int ForwardNext { get; set; } = 0;
        public int FlowRoleID { get; set; } = 0;
        public int CheckReporting {  get; set; } = 0;
    }
}
