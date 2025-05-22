namespace ATTENDANCE.DTO.Request
{
    public class InsertAttendancePolicyDto
    {
        public int AttendancePolicyID { get; set; }
        public int empId { get; set; }
        public string? PolicyName { get; set; }
        public string? Criteria { get; set; }
        public string? CheckDirection { get; set; }
        public decimal? LateIn { get; set; }
        public decimal? LateOut { get; set; }
        public decimal? EarlyIn { get; set; }
        public decimal? EarlyOut { get; set; }
        public int? RoundOf { get; set; }
        public int? SpeacialSeasonId { get; set; }
        public bool? StrictShiftTime { get; set; }
        public bool? OverTimeInclude { get; set; }
        public bool? CkhOtconsiderInShortage { get; set; }
        public bool? EnableOtonRequest { get; set; }
        public int? StatusOnAbsentShortage { get; set; }
        public bool? PresentForMinimumWorkHrs { get; set; }
        public decimal? MinimumWorkHrsForPrsnt { get; set; }
        public double? ShortageFreeMinutes { get; set; }
        public int? EnableLateinPolicy { get; set; }
        public int? CountForLateIn { get; set; }
        public int? TimeFrom { get; set; }
        public int? TimeTo { get; set; }

        public List<attenPolicy01Dto> attendancePolicy01Dto { get; set; }
        public List<SpecialOvertimeDto>? SpecialOvertimes { get; set; }

       





        public List<TblOverTimeDto> OverTimeList { get; set; }
        public List<ShortageDto> ShortageList { get; set; }

    }
}
