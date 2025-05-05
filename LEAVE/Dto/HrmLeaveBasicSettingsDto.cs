namespace LEAVE.Dto
{
    public class HrmLeaveBasicSettingsDto
    {
        public int SettingsId { get; set; }           // Primary key
        public string SettingsName { get; set; }
        public string SettingsDescription { get; set; }
        public int DaysOrHours { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }    // Nullable for flexibility
    }
}
