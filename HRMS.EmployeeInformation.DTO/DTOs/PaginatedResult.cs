namespace MPLOYEE_INFORMATION.DTO.DTOs
{
    public class PaginatedResult<T>
    {
        public int recordsTotal { get; set; }
        public int recordsFiltered { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int draw { get; set; }
        public List<T>? data { get; set; }

    }
}
