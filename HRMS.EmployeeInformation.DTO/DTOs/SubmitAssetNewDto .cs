using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.EmployeeInformation.DTO.DTOs
{
    public class SubmitAssetNewDto
    {
        public string Type { get; set; }
        public string Description { get; set; }
        public int Value { get; set; }
        public int EmpID { get; set; }
        public int Entryby { get; set; }
        public string Reason_id { get; set; }
        public DateTime ReleaseDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public string Remarks { get; set; }
        public List<AssetFieldDto> Array { get; set; }
        public List<CategoryFieldDto> ArraySecond { get; set; }
    }

    public class AssetFieldDto
    {
        public int GcategoryFieldID { get; set; }
        public string FieldValues { get; set; }
        public string GcategoryID { get; set; }
        public int MasterID { get; set; }
    }

    public class CategoryFieldDto
    {
        public int GcategoryID { get; set; }
        public string FieldValues { get; set; }
        public int Others { get; set; }
        public int MasterID { get; set; }
    }

}
