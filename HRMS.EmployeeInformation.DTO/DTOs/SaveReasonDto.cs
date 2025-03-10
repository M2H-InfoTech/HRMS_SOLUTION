using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.EmployeeInformation.DTO.DTOs
{
    

        public class SaveReasonDto
        {
            public string Type { get; set; }
            public string Description { get; set; }
            public int Value { get; set; }
            public int Reason_Id { get; set; }
            public int AssetRole { get; set; }
            public int Others { get; set; }
            public List<GeneralCategoryFieldDto> Array { get; set; }
            public List<CategoryFieldSaveDto> ArraySecond { get; set; }
        }

        public class GeneralCategoryFieldDto
        {
            public int GcategoryFieldID { get; set; }
            public string FieldValues { get; set; }
            public int GcategoryID { get; set; }
            public int MasterID { get; set; }
        }

        public class CategoryFieldSaveDto
        {
            public int GcategoryID { get; set; }
            public string FieldValues { get; set; }
            public int Others { get; set; }
            public int CategoryRole { get; set; }
            public int MasterID { get; set; }
            public int SubCatLinkId { get; set; }
        

    }
}
