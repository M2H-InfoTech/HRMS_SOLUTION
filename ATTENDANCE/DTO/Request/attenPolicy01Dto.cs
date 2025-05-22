namespace ATTENDANCE.DTO.Request
{
    public class attenPolicy01Dto
    {
        public string MaxLateComingLimitNo { get; set; }
        public string MaxEarlyOutLimitNo { get; set; }
        public string MaxLateComingLimitMin { get; set; }
        public string MaxEarlyOutLimitMin { get; set; }
        public string EarlyGapLimitNo { get; set; }
        public string LateGapLimitNo { get; set; }
        public int PolicyConId { get; set; }
        public int EntryBy { get; set; }
    }
}
