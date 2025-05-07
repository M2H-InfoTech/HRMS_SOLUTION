namespace LEAVE.Dto
{
    public class EntityApplicableApiDto
    {
        public string? EntityList { get; set; }
        public string? TransactionType { get; set; }
        public string? LinkIds { get; set; }
        public string? EmployeeIds { get; set; }
        public int FirstEntityId { get; set; }
        public int SecondEntityId { get; set; }
        public int MasterId { get; set; }
        public int EntryBy { get; set; }
    }
}
