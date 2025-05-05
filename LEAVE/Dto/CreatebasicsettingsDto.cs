namespace LEAVE.Dto
{
    public class CreatebasicsettingsDto
    {
        public int masterId { get; set; }
        public int basicSettingsId { get; set; }
        public string? leaveCode { get; set; }
        public string? description { get; set; }
        public int basedOn { get; set; }

        public int createdBy { get; set; }
         

    }
}
