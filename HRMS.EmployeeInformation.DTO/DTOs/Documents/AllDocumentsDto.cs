namespace HRMS.EmployeeInformation.DTO.DTOs.Documents
{
    public class AllDocumentsDto
    {

        public IEnumerable<DocumentsDto>? ApprovedDocuments { get; set; }
        public IEnumerable<DocumentListDto>? DocumentList { get; set; }

        public IEnumerable<DocumentsDto>? PendingDocumentsDto { get; set; }
        public IEnumerable<FilesDto>? PendingFiles { get; set; }

        public IEnumerable<FilesDto>? ApprovedFiles { get; set; }

    }


}

