namespace HRMS.EmployeeInformation.DTO.DTOs
{
    public class GetRejoinReportDto
    {
        public int? Emp_Id { get; set; }
        public string? Emp_Code { get; set; }
        public string? Name { get; set; }
        public int? Resignation_Id { get; set; }
        public string? Resignation_Request_Id { get; set; }
        public string? Request_Date { get; set; }
        public string? Resignation_Date { get; set; }
        public string? RelievingDate { get; set; }
        public string? Remarks { get; set; }
        public string? RejoinRequestID { get; set; }
        public string? RejoinRequestDate { get; set; }
        public string? RejoinApprovalDate { get; set; }
        public string? RejoinRemarks { get; set; }

    }
}
