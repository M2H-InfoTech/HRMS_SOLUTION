using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.EmployeeInformation.DTO.DTOs
{
    public class UploadColumnDto
    {
        public string DtColumnName { get; set; }
        public string TableColumnName { get; set; }
    }

    public class DocumentDto
    {
        public string DocDescription { get; set; }
        public int DocID { get; set; }
    }

    public class DownloadExeclEmployeeDto
    {
        public List<UploadColumnDto> EmpUploadColumns { get; set; }
        public List<UploadColumnDto> EntUploadColumns { get; set; }
        public List<DocumentDto> SatDocument { get; set; }
        public List<DocumentDto> BnkDocument { get; set; }
    }

}
