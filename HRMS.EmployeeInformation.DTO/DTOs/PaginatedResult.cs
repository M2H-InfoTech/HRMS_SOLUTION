﻿namespace MPLOYEE_INFORMATION.DTO.DTOs
{
    public class PaginatedResult<T>
    {
        public int TotalRecords { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public List<T>? Records { get; set; }
    }
}
