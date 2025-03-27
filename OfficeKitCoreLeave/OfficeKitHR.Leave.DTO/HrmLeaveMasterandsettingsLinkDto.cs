namespace OFFICEKITCORELEAVE.OfficeKitHR.Leave.DTO
{
    public class HrmLeaveMasterandsettingsLinkDto
    {
        public int IdMasterandSettingsLink { get; set; }

        public int? LeaveMasterId { get; set; }

        public int? SettingsId { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }
    }
}
