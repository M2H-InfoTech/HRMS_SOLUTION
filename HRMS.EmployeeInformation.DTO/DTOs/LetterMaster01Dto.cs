namespace HRMS.EmployeeInformation.DTO.DTOs
{
    public class LetterMaster01Dto
    {
        public int ModuleSubId { get; set; }
        public string? LetterSubName { get; set; }

        public int? LetterTypeId { get; set; }

        public bool? IsEss { get; set; }

        public bool? IsActive { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? ModifiedBy { get; set; }

        public string? BackGroundImage { get; set; }

        public DateTime? ModifiedDate { get; set; }
        public bool? IsSelfApprove { get; set; }

        public string? ApproveText { get; set; }

        public string? RejectText { get; set; }

        public int? HideReject { get; set; }

        public int? WrkFlowRoleId { get; set; }
        public int AdjustImagePos { get; set; }
        public int AppointmentLetter { get; set; }


    }
}
