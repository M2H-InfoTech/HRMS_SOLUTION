namespace HRMS.EmployeeInformation.DTO.DTOs
{
    public class LetterMaster01Dto
    {

        public string? LetterSubName { get; set; }

        public int? LetterTypeId { get; set; }

        public bool? IsEss { get; set; }

        public bool? IsActive { get; set; }

        public int? CreatedBy { get; set; }

        public int? ModifiedBy { get; set; }
        public bool? IsSelfApprove { get; set; }

        public string? ApproveText { get; set; }

        public string? RejectText { get; set; }

        public int? HideReject { get; set; }

        public int? WrkFlowRoleId { get; set; }


    }
}
