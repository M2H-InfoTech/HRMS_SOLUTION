namespace HRMS.EmployeeInformation.DTO.DTOs
{
    public class LetterInsertUpdateDto
    {

        public int LetterTypeID { get; set; }
        public int LetterSubTypeID { get; set; }
        public string LetterType { get; set; }
        public string LetterSubType { get; set; }
        public bool IsEssApplicable { get; set; }
        public string Mode { get; set; }
        public int UserID { get; set; }
        public int EmpID { get; set; }
        public string ValidDateFrom { get; set; }
        public string EmployeeList { get; set; }
        public string Remark { get; set; }
        public string ApprovalType { get; set; }
        public int MasterID { get; set; }
        public string FileName { get; set; }
        public byte[] FileImage { get; set; }
        public string FileType { get; set; }
        public string TemplateStyle { get; set; }
        public string BankName { get; set; }
        public string BranchName { get; set; }
        public string AccountNo { get; set; }
        public string Location { get; set; }
        public string IdentificationCode { get; set; }
        public int Language { get; set; }
        public bool IsSelfApprove { get; set; }
        public string Return { get; set; }
        public string EmployeeID { get; set; }
        public DateTime? IssueDate { get; set; }
        public string ApprovalText { get; set; }
        public string RejectText { get; set; }
        public int HideReject { get; set; }
        public int WrkFlowRole { get; set; }
        public int MaxRule { get; set; }
        public int MaxRuleID { get; set; }
        public int RuleOrder { get; set; }
        public int EditID { get; set; }

    }
}
