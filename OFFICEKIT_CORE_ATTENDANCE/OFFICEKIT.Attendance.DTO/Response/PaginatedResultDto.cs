using OFFICEKIT_CORE_ATTENDANCE.OFFICEKIT.Attendance.DTO.Request;

namespace OFFICEKIT_CORE_ATTENDANCE.OFFICEKIT.Attendance.DTO.Response
{
    public class PaginatedResultDto
    {
        public int? TotalCount { get; set; }
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }
        public List<ShiftAccessDto> Data { get; set; }
    }

}
