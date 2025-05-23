﻿using HRMS.EmployeeInformation.DTO.DTOs;

namespace HRMS.EmployeeInformation.Service.InterfaceB
{
    public interface IEmployeeInformationServiceB
    {
        Task<List<object>> QualificationDocumentsDetails(int QualificationId);
        Task<string> InsertOrUpdateCommunication(SaveCommunicationSDto communications);
        Task<string> InsertOrUpdateCommunicationEmergency(SaveCommunicationSDto communications);
        Task<string> UpdateCommunication(SaveCommunicationSDto communications);
        Task<string> SubmitAssetDetailsNewAsync(SubmitAssetNewDto submitAssetNewDto);
        Task<string> UpdateAssetDetailsNewAsync(SubmitAssetNewDto submitAssetNewDto, int AssetRole);
        Task<List<AssetParameterDto>> GetAssetParameterAsync();
        Task<List<dynamic>> FillAssetsubOnchange1Async(int ComFieldID, string AssignAssetStatus);

        Task<List<dynamic>> GenrlCategoryFieldsReasonAsync(int Reason_Id);

        Task<string> SavefieldsReasonsAsync(SaveReasonDto saveReasonDto);
    }
}
