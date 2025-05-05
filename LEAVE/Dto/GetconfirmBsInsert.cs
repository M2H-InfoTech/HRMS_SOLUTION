namespace LEAVE.Dto
{
    public class GetconfirmBsInsert
    {
        public string leavemasters { get; set; }
        public string employeeids { get; set; }
        public DateTime fromDate { get; set; }
        public DateTime validTo { get; set; }
        public int entryBy { get; set; }
        public int? linkLevel { get; set; }

    }
}
