namespace LEAVE.Dto
{
    public class BasicSettingDto
    {
        public string SettingsName { get; set; }
        public string SettingsDescription { get; set; }
        public int LeaveMasterId { get; set; }
        public int? DaysOrHours { get; set; }
    }
}
