﻿using HRMS.EmployeeInformation.DTO.DTOs;
using MPLOYEE_INFORMATION.DTO.DTOs;

namespace HRMS.EmployeeInformation.Repository.Common.RepositoryB
{
    public interface IRepositoryB
    {
        Task<List<object>> QualificationDocumentsDetails(int QualificationId);
        Task<string> InsertOrUpdateCommunication(SaveCommunicationSDto communications);
        Task<string> InsertOrUpdateCommunicationEmergency(SaveCommunicationSDto communications);
        Task<string> UpdateCommunication(SaveCommunicationSDto communications);
        Task<string> SubmitAssetDetailsNewAsync(SubmitAssetNewDto submitAssetNewDto);
        Task<string> UpdateAssetDetailsNewAsync(SubmitAssetNewDto submitAssetNewDto, int assetID);
        Task<List<AssetParameterDto>> GetAssetParameterAsync();
        Task<List<dynamic>> FillAssetsubOnchange1Async(int ComFieldID, string AssignAssetStatus);

        Task<List<dynamic>> GenrlCategoryFieldsReasonAsync(int Reason_Id);

        Task<string> SavefieldsReasonsAsync(SaveReasonDto saveReasonDto);

    }
}
