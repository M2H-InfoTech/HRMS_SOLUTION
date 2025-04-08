namespace OFFICEKITCORELEAVE.OfficeKitHR.Leave.DTO
{
    public class SaveApplicableParameters
    {
        public int? FirstEntityId { get; set; }
        public string? TransactionType { get; set; }
        public int? TranId1 { get; set; }
        public int? MasterId { get; set; }
        public int? SecondEntityId { get; set; }
        public int? EntryBy { get; set; }
        public string LinkIds { get; set; }
        public string? EmployeeIds { get; set; }
        public string? EntityList { get; set; }

        // Additional internal variables for processing
        public int? LinkLevel1 { get; set; }
        public string? ValueApplicable1 { get; set; }
        public int? PosApplicable1 { get; set; }
        public int? LenApplicable1 { get; set; }
        public string? ApplicableIds1 { get; set; }
        public int? LinkLevelApplicable1 { get; set; }
        public string? ApplicableLevels1 { get; set; }
        public DateTime? CreateDate { get; set; } = DateTime.UtcNow;    
        public int EmployeeId { get; set; }
    }
}
