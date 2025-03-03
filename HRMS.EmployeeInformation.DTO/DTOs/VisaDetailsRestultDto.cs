using MPLOYEE_INFORMATION.DTO.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.EmployeeInformation.DTO.DTOs
    {
    public class VisaDetailsRestultDto
        {
        public IEnumerable<VisaTableApproversDto>? VisaTable01 { get; set; }
        public IEnumerable<VisaTableDto>? VisaTable02 { get; set; }
        public IEnumerable<VisaSubmitedDoc>? VisaSubmitedDoc { get; set; }
        public IEnumerable<VisaDocDetailsDto>? VisaDocDetails { get; set; }       
        public IEnumerable<VisaFilesPendingDto>? VisaFiles01 { get; set; }
        public IEnumerable<VisaFilesApprovedDto>? VisaFiles02 { get; set; }
        
        }
    }
