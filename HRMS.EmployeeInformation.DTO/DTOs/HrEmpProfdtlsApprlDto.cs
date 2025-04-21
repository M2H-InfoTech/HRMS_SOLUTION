namespace HRMS.EmployeeInformation.Repository.Common
{
    public class HrEmpProfdtlsApprlDto
    {
        public int InstId { get; set; }

        public int ProfId { get; set; }

        public int EmpId { get; set; }

        public string? CompName { get; set; }

        public string? Designation { get; set; }

        public string? CompAddress { get; set; }

        public string? Pbno { get; set; }

        public string? ContactPer { get; set; }

        public string? ContactNo { get; set; }

        public string? JobDesc { get; set; }

        public DateTime? JoinDt { get; set; }

        public DateTime? LeavingDt { get; set; }

        public string? LeaveReason { get; set; }

        public string? Ctc { get; set; }

        public string? CompSite { get; set; }

        public int EntryBy { get; set; }

        public DateTime EntryDt { get; set; }

        public int? CurrencyId { get; set; }

        public string? Status { get; set; }

        public string? FlowStatus { get; set; }

        public string? RequestId { get; set; }

        public DateTime? DateFrom { get; set; }

        public int? MasterId { get; set; }
        public string? updateType { get; set; }
        public string? entityList { get; set; }
        public int FirstEntityID { get; set; }
    }
}