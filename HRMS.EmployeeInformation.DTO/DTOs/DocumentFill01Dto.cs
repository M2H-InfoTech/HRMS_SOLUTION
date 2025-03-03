using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.EmployeeInformation.DTO.DTOs
    {
    public class DocumentFill01Dto
        {
        public int Reason_Id {  get; set; }
        public int? CategoryFieldID {  get; set; }
        public string? FieldValues {  get; set; }
        public string? FieldDescription {  get; set; }
        public int? DataTypeID {  get; set; }

        }
    }
