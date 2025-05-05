namespace LEAVE.Dto
{
    public class HrmLeaveMasterandsettingsLinksDto
    {
        public int Id { get; set; }                   // Primary key (adjust name to match your DB)
        public int LeaveMasterId { get; set; }
        public int SettingsId { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
