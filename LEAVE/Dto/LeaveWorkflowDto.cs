using HRMS.EmployeeInformation.DTO.DTOs;

namespace LEAVE.Dto
{
    public class LeaveWorkflowDto
    {
        public string? EmpName { get; set; } = string.Empty;
        public string? Name { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public string? Designation { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public string ApprovalStatus { get; set; } = string.Empty;
        public string? Branch { get; set; } = string.Empty;
        public int? Rule { get; set; } = 0;
        public int? RuleOrder { get; set; }
        public int? ShowStatus { get; set; }
        public int? FlowRoleID { get; set; }
        public int ForwardNext { get; set; }
        public int? WorkFlowID { get; set; }
        public string WorkFlowStatus { get; set; } = string.Empty;
        public int Emp_Id { get; set; }
        public string? Code { get; set; } = string.Empty;
    }
    public class ProbationWorkflowDisplayDto
    {
        public string? EmpName { get; set; } = string.Empty;
        public string? Name { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public string? Designation { get; set; } = string.Empty;
        public string Image_Url { get; set; } = string.Empty;
        public string ApprovalStatus { get; set; } = string.Empty;
        public string? Branch { get; set; } = string.Empty;
        public int? Rule { get; set; } 
        public int? RuleOrder { get; set; }
        public int? ShowStatus { get; set; }
        public int? FlowRoleID { get; set; }
        public int ForwardNext { get; set; }
        public int? WorkFlowID { get; set; }
        public string WorkFlowStatus { get; set; } = string.Empty;
        public int Emp_Id { get; set; }
        public string? Code { get; set; } = string.Empty;
        public int? IsSelf { get; set; }
    }
    public class WorkFlowActivityProbationInputDto
    {
        public int EmpId { get; set; } = 0;
        public string TransactionType { get; set; } = string.Empty;
        public int RequestId { get; set; } = 0;
        public DateTime EntryDt { get; set; } = default;
        public bool ShowStatus { get; set; } = false;
        public string ApprovalRemarks { get; set; } = string.Empty;
        public int EntryBy { get; set; } = 0;
        public int UpdatedBy { get; set; } = 0;
        public DateTime UpdatedDt { get; set; } = default;
        public string Deligate { get; set; } = "0";
        public bool ReturnWorkFlowTable { get; set; } = false;
        public int SpecialWorkFlowID { get; set; } = 0;
        public string EntryFrom { get; set; } = string.Empty;
        public int WorkflowType { get; set; } = 1;
        public int SelfEmpID { get; set; } = 0;
    }
    public class EmployeeBasicDto
    {
        public int EmpId { get; set; }
        public string Name { get; set; } = "";
        public string EmpCode { get; set; } = "";
        public string Designation { get; set; } = "";
        public DateTime? DateOfBirth { get; set; }
        public DateTime? JoinDt { get; set; }
        public string Branch { get; set; } = "";
        public string? ImageUrl { get; set; }
        public string? Grade { get; set; }
        public string Address { get; set; } = "";
    }

    public class WorkFlowMainDto
    {
        public EmployeeResignationDto?  employeeResignationDto { get; set; }
        public EmployeeDetailDto? EmployeeDetailDto { get; set; }
        public List<LeaveWorkflowDto>? leaveWorkflowDtos { get; set; }
        public List<ProbationWorkflowDisplayDto> probationWorkflowDisplayDtos { get; set; }
    }


}
