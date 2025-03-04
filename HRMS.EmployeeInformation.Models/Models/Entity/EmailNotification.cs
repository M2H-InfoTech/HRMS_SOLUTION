namespace HRMS.EmployeeInformation.Models.Models.Entity;

public partial class EmailNotification
{
    public int Id { get; set; }

    public int InstdId { get; set; }

    public string? SenderEmail { get; set; }

    public string? SenderEmailPwd { get; set; }

    public string? ReceiverEmail { get; set; }

    public int? ReceiverUserId { get; set; }

    public int? ReceiverEmpId { get; set; }

    public int? RequesterEmpId { get; set; }

    public DateTime? SendDate { get; set; }

    public DateTime? TriggerDate { get; set; }

    public string? EmailSubject { get; set; }

    public string? EmailBody { get; set; }

    public bool? SendStatus { get; set; }

    public string? ExceptionLog { get; set; }

    public int? ErrorId { get; set; }

    public int? ProxyId { get; set; }

    public string? SendMode { get; set; }

    public int? TransactionId { get; set; }

    public int? RequestId { get; set; }

    public string? RequestIdCode { get; set; }

    public string? Path { get; set; }

    public string? FileByte { get; set; }

    public string? AttachFormat { get; set; }

    public byte[]? UploadedData { get; set; }

    public string? MonthYear { get; set; }

    public string? MailType { get; set; }

    public string? NotificationMessage { get; set; }

    public int? ShowStatus { get; set; }

    public DateTime? RequesterDate { get; set; }

    public int? MobileStatus { get; set; }

    public DateTime? ExpiryDate { get; set; }

    public bool? WithoutExpire { get; set; }

    public string? SendMail { get; set; }

    public string? NotificationType { get; set; }

    public DateTime? FromDate { get; set; }

    public DateTime? ToDate { get; set; }

    public int? Type { get; set; }

    public bool? IsRecruitment { get; set; }

    public int? Workflowtype { get; set; }
}
