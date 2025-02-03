using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.EmployeeInformation.DTO.DTOs.Documents
{
    public class DocumentsDto
    {
        public int? DetailID { get; set; }
        public int? DocID { get; set; }
        public int? EmpID { get; set; }
        public long? DocFieldID { get; set; }
        public string? DocDescription { get; set; }
        public string? DocValues { get; set; }
        public int? IsGeneralCategory { get; set; }
        public string? DataType { get; set; }
        public string? DocName { get; set; }
        public int? IsDate { get; set; }
        public string? FieldValues { get; set; }
        public string? FieldDescription { get; set; }
        public int? repeatrank { get; set; }

        public DocumentFillDto? MatchedDoc { get; set; }
    }
}
