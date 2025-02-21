using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.EmployeeInformation.DTO.DTOs
{
    public class DocumentFieldDto
    {
        public long DocFieldID { get; set; }
        public string? DocDescription { get; set; }
        public int? DataTypeId { get; set; }
        public int? DocID { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool? IsMandatory { get; set; }
        public int? TypeId { get; set; }
        public string? DataType { get; set; }
        public int? IsDate { get; set; }
        public int? IsGeneralCategory { get; set; }
        public int? IsDropdown { get; set; }
        public string? DocName { get; set; }
        public int? DocType { get; set; }
        public bool? Active { get; set; }
        public int? IsExpiry { get; set; }
        public int? NotificationCountDays { get; set; }
        public string? FolderName { get; set; }
        public int? IsAllowMultiple { get; set; }
        public int? IsESI { get; set; }
        public int? IsPF { get; set; }
        public int? ShowInRecruitment { get; set; }
    }
}
