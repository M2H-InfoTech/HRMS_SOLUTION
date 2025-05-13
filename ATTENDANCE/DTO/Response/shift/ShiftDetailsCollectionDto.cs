namespace ATTENDANCE.DTO.Response.shift
{
    public class ShiftDetailsCollectionDto
    {
        public List<ShiftDto> Shifts { get; set; }
        public List<ShiftDetailDto> ShiftDetails { get; set; }
        public List<ShiftBreakDto> ShiftBreaks { get; set; }
        public List<ShiftMasterDto> ShiftMasters { get; set; }
        public List<ShiftSeasonDto> ShiftSeasons { get; set; }
        public List<ShiftSeasonBreakDto> ShiftSeasonBreaks { get; set; }
    }
}
