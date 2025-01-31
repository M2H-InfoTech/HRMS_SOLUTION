 

namespace MPLOYEE_INFORMATION.DTO.DTOs
{
    public class CommunicationTableDto
    {
        public int Inst_Id { get; set; }
        public int Add_Id { get; set; }
        public int Emp_Id { get; set; }
        public string? Add1 { get; set; }
        public string? Add2 { get; set; }
        public string? PBNo { get; set; }

        public int? Country_ID { get; set; }
        public string? Country_Name { get; set; }
        public string? Phone { get; set; }
        public string? Mobile { get; set; }
        public string? OfficePhone { get; set; }
        public string? Extension { get; set; }
        public string? EMail { get; set; }
        public string? PersonalEMail { get; set; }

        public string? Status { get; set; }
    }
}
