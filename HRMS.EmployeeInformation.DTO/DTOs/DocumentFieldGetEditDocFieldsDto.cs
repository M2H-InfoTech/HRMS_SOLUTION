using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.EmployeeInformation.DTO.DTOs
    {
    public class DocumentFieldGetEditDocFieldsDto
        {
        public long DocFieldID { get; set; }
        public string? DocDescription { get; set; }
        public int? DocID { get; set; }
        public int? DataTypeId { get; set; }
        public int? IsDate { get; set; }
        public int? IsGeneralCategory { get; set; }
        public string? DataType { get; set; }
        public int DetailID { get; set; }
        public string? TransactionType { get; set; }
        public string? Status { get; set; }
        public int DocFieldID01 { get; set; }
        public string? DocValues { get; set; }
        public string? FolderName { get; set; }
        public int EmpID { get; set; }
        public bool? IsMandatory { get; set; }
        public int? IsAllowMultiple { get; set; }

        }
    }
