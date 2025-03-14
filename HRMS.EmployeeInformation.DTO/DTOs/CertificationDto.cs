namespace MPLOYEE_INFORMATION.DTO.DTOs
{
    public class CertificationDto
    {
        public int? empId { get; set; }
        public int? certificationId { get; set; }
        public string? certificateName { get; set; }
        public string? certificateField { get; set; }
        public string? issuingAuthority { get; set; }
        public string? yearOfCompletion { get; set; }
    }
}
