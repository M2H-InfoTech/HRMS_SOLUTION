using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRMS.EmployeeInformation.DTO.DTOs;

namespace HRMS.EmployeeInformation.DTO.DTOs.Documents
{
    public class AllDocumentsDto
    {
        public IEnumerable<DocumentFillDto>? TempDocumentFill { get; set; }
        public IEnumerable<DocumentsDto>? ApprovedDocuments { get; set; }
        public IEnumerable<DocumentListDto>? DocumentList { get; set; }

        public IEnumerable<DocumentsDto>? PendingDocumentsDto { get; set; }
        public IEnumerable<FilesDto>? PendingFiles { get; set; }

        public IEnumerable<FilesDto>? ApprovedFiles { get; set; }

    }

  
}

