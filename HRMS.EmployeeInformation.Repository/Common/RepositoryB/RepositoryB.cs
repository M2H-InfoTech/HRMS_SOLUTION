using AutoMapper;
using EMPLOYEE_INFORMATION.Data;
using EMPLOYEE_INFORMATION.Models.Entity;
using HRMS.EmployeeInformation.DTO.DTOs;
using HRMS.EmployeeInformation.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using MPLOYEE_INFORMATION.DTO.DTOs;
using EMPLOYEE_INFORMATION.Models;


namespace HRMS.EmployeeInformation.Repository.Common.RepositoryB
{
    public class RepositoryB : IRepositoryB
    {
        private readonly EmployeeDBContext _context;
        //private IStringLocalizer _stringLocalizer;
        private readonly IMemoryCache _memoryCache;
        private readonly EmployeeSettings _employeeSettings;
        private int paramDynVal;

        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;
        private readonly IEmployeeRepository _employeeRepository;

        public RepositoryB(EmployeeDBContext dbContext, EmployeeSettings employeeSettings, IMemoryCache memoryCache, IMapper mapper, IWebHostEnvironment env, IEmployeeRepository employeeRepository)
        {
            _context = dbContext;
            _employeeSettings = employeeSettings;
            _memoryCache = memoryCache;
            _mapper = mapper;
            _env = env ?? throw new ArgumentNullException(nameof(env));
            _employeeRepository = employeeRepository;
        }

        public async Task<List<object>> QualificationDocumentsDetails(int QualificationId)
        {
            return await (from a in _context.QualificationAttachments
                          where a.QualificationId == QualificationId && a.DocStatus == _employeeSettings.EmployeeStatus
                          select new
                          {
                              a.QualAttachId,
                              a.QualificationId,
                              a.QualFileName,
                              a.DocStatus,
                              a.EmpId
                          }).AsNoTracking().ToListAsync<object>();
        }
        public async Task<string> InsertOrUpdateCommunication(SaveCommunicationSDto communications)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            bool isWorkflowNeeded = await _employeeRepository.IsWorkflowNeeded();
            var hrSavecommunication = new HrEmpAddress01Apprl
            {

                EmpId = communications.EmpID,
                PermanentAddr = communications.address1,
                PinNo1 = communications.PostboxNo,
                Addr1Country = communications.countryID,
                ContactAddr = communications.address2,
                PinNo2 = communications.PostboxNo2,
                Addr2Country = communications.countryID2,
                PhoneNo = communications.ContactNo,
                AlterPhoneNo = communications.OfficeNo,
                MobileNo = communications.mobileNo,
                EntryBy = communications.Entry_By,
                EntryDate = DateTime.UtcNow
            };

            if (isWorkflowNeeded)
            {
                string? codeId = await _employeeRepository.GenerateRequestId(communications.EmpID);
                if (!string.IsNullOrEmpty(codeId))
                {
                    hrSavecommunication.RequestId = await _employeeRepository.GetLastSequence(codeId);
                    await _context.HrEmpAddress01Apprls.AddAsync(hrSavecommunication);
                    await _employeeRepository.UpdateCodeGeneration(codeId);
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return "Successfully Saved";
            }
            else
            {
                if (communications.UpdateType == "")
                {
                    // If workflow is NOT needed, insert the record directly
                    await _context.HrEmpAddress01Apprls.AddAsync(hrSavecommunication);
                    var hrEmpAddress5 = new HrEmpAddress01
                    {
                        EmpId = communications.EmpID,
                        PermanentAddr = communications.address1,
                        PinNo1 = communications.PostboxNo,
                        Addr1Country = communications.countryID,
                        ContactAddr = communications.address2,
                        PinNo2 = communications.PostboxNo2,
                        Addr2Country = communications.countryID2,
                        PhoneNo = communications.ContactNo,
                        AlterPhoneNo = communications.OfficeNo,
                        MobileNo = communications.mobileNo
                    };
                    await _context.HrEmpAddress01s.AddAsync(hrEmpAddress5);

                }
            }
            if (communications.UpdateType == "pending")
            {
                var hrEmpAddress = await _context.HrEmpAddress01Apprls
                    .FirstOrDefaultAsync(x => x.AddrId == communications.DetailID && x.EmpId == communications.EmpID);

                if (hrEmpAddress != null)
                {
                    hrEmpAddress.PermanentAddr = communications.address1;
                    hrEmpAddress.PinNo1 = communications.PostboxNo;
                    hrEmpAddress.Addr1Country = communications.countryID;
                    hrEmpAddress.ContactAddr = communications.address2;
                    hrEmpAddress.PinNo2 = communications.PostboxNo2;
                    hrEmpAddress.Addr2Country = communications.countryID2;
                    hrEmpAddress.PhoneNo = communications.ContactNo;
                    hrEmpAddress.AlterPhoneNo = communications.OfficeNo;
                    hrEmpAddress.MobileNo = communications.mobileNo;
                    hrEmpAddress.EntryBy = communications.Entry_By;
                    hrEmpAddress.EntryDate = DateTime.UtcNow;

                    _context.HrEmpAddress01Apprls.Update(hrEmpAddress);
                }
            }
            else if (communications.UpdateType == "Approved")
            {
                var hrEmpAddress = await _context.HrEmpAddress01s
                    .FirstOrDefaultAsync(x => x.AddrId == communications.DetailID && x.EmpId == communications.EmpID);

                if (hrEmpAddress != null)
                {
                    hrEmpAddress.PermanentAddr = communications.address1;
                    hrEmpAddress.PinNo1 = communications.PostboxNo;
                    hrEmpAddress.Addr1Country = communications.countryID;
                    hrEmpAddress.ContactAddr = communications.address2;
                    hrEmpAddress.PinNo2 = communications.PostboxNo2;
                    hrEmpAddress.Addr2Country = communications.countryID2;
                    hrEmpAddress.PhoneNo = communications.ContactNo;
                    hrEmpAddress.AlterPhoneNo = communications.OfficeNo;
                    hrEmpAddress.MobileNo = communications.mobileNo;
                    hrEmpAddress.EntryBy = communications.Entry_By;
                    hrEmpAddress.EntryDate = DateTime.UtcNow;

                    _context.HrEmpAddress01s.Update(hrEmpAddress);
                }
                else
                {
                    hrEmpAddress = new HrEmpAddress01
                    {
                        EmpId = communications.EmpID,
                        PermanentAddr = communications.address1,
                        PinNo1 = communications.PostboxNo,
                        Addr1Country = communications.countryID,
                        ContactAddr = communications.address2,
                        PinNo2 = communications.PostboxNo2,
                        Addr2Country = communications.countryID2,
                        PhoneNo = communications.ContactNo,
                        AlterPhoneNo = communications.OfficeNo,
                        MobileNo = communications.mobileNo
                    };
                    await _context.HrEmpAddress01s.AddAsync(hrEmpAddress);
                }

                var hrEmpCommunication = new HrEmpAddress01Apprl
                {
                    EmpId = communications.EmpID,
                    PermanentAddr = communications.address1,
                    PinNo1 = communications.PostboxNo,
                    Addr1Country = communications.countryID,
                    ContactAddr = communications.address2,
                    PinNo2 = communications.PostboxNo2,
                    Addr2Country = communications.countryID2,
                    PhoneNo = communications.ContactNo,
                    Status = "A",
                    FlowStatus = "E",
                    AlterPhoneNo = communications.OfficeNo,
                    MobileNo = communications.mobileNo,
                    RequestId = null,
                    DateFrom = DateTime.UtcNow,
                    MasterId = communications.DetailID
                };
                await _context.HrEmpAddress01Apprls.AddAsync(hrEmpCommunication);

                var hrEmpMaster = await _context.HrEmpMasters
                    .FirstOrDefaultAsync(x => x.EmpId == communications.EmpID);

                if (hrEmpMaster != null)
                {
                    hrEmpMaster.ModifiedDate = DateTime.UtcNow;
                    _context.HrEmpMasters.Update(hrEmpMaster);
                }
            }

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return "Successfully Saved";
        }

        public async Task<string> InsertOrUpdateCommunicationEmergency(SaveCommunicationSDto communications)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var hrEmpAddress = await _context.HrEmpEmergaddresses
                    .FirstOrDefaultAsync(x => x.AddrId == communications.DetailID && x.EmpId == communications.EmpID);

                if (hrEmpAddress != null)
                {
                    // Update existing record
                    hrEmpAddress.Address = communications.address1;
                    hrEmpAddress.PinNo = communications.PostboxNo;
                    hrEmpAddress.Country = communications.countryID;
                    hrEmpAddress.PhoneNo = communications.ContactNo;
                    hrEmpAddress.AlterPhoneNo = communications.OfficeNo;
                    hrEmpAddress.MobileNo = communications.mobileNo;
                    hrEmpAddress.EntryBy = communications.Entry_By;
                    hrEmpAddress.EntryDate = DateTime.UtcNow;
                    hrEmpAddress.EmerName = communications.Emername;
                    hrEmpAddress.EmerRelation = communications.RelationId;

                    _context.HrEmpEmergaddresses.Update(hrEmpAddress);
                }
                else
                {
                    // Insert new record
                    var newHrEmpAddress = new HrEmpEmergaddress
                    {
                        EmpId = communications.EmpID,
                        AddrId = communications.DetailID, // Ensure this ID is properly generated if necessary
                        Address = communications.address1,
                        PinNo = communications.PostboxNo,
                        Country = communications.countryID,
                        PhoneNo = communications.ContactNo,
                        AlterPhoneNo = communications.OfficeNo,
                        MobileNo = communications.mobileNo,
                        EntryBy = communications.Entry_By,
                        EntryDate = DateTime.UtcNow,
                        EmerName = communications.Emername,
                        EmerRelation = communications.RelationId
                    };

                    await _context.HrEmpEmergaddresses.AddAsync(newHrEmpAddress);

                    // Insert into HR_EMP_EMERGADDRESS_APPRL table
                    var newHrEmpAddressApprvl = new HrEmpEmergaddressApprl
                    {
                        EmpId = communications.EmpID,
                        Address = communications.address1,
                        PinNo = communications.PostboxNo,
                        Country = communications.countryID,
                        PhoneNo = communications.ContactNo,
                        AlterPhoneNo = communications.OfficeNo,
                        MobileNo = communications.mobileNo,
                        RequestId = null, // Set correct value if needed
                        Status = "A",
                        FlowStatus = "E",
                        DateFrom = DateTime.UtcNow
                    };

                    await _context.HrEmpEmergaddressApprls.AddAsync(newHrEmpAddressApprvl);
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return hrEmpAddress != null ? "Successfully Updated" : "Successfully Saved";
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return $"Error: {ex.Message}";
            }
        }
        public async Task<string> UpdateCommunication(SaveCommunicationSDto communications)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                bool isWorkflowNeeded = await _employeeRepository.IsWorkflowNeeded();

                if (communications.UpdateType == "pending")
                {
                    var hrEmpAddress = await _context.HrEmpAddressApprls
                        .FirstOrDefaultAsync(x => x.AddId == communications.DetailID && x.EmpId == communications.EmpID);

                    if (hrEmpAddress != null)
                    {
                        hrEmpAddress.InstId = communications.InstId;
                        hrEmpAddress.Add1 = communications.address1;
                        hrEmpAddress.Add2 = communications.address2;
                        hrEmpAddress.Country = communications.countryID;
                        hrEmpAddress.Pbno = communications.PostboxNo;
                        hrEmpAddress.Phone = communications.ContactNo;
                        hrEmpAddress.Mobile = communications.mobileNo;
                        hrEmpAddress.OfficePhone = communications.OfficeNo;
                        hrEmpAddress.Extension = communications.Extension;
                        hrEmpAddress.Email = communications.EmailField;
                        hrEmpAddress.PersonalEmail = communications.PersonalEmail;
                        hrEmpAddress.EntryBy = communications.Entry_By;
                        hrEmpAddress.EntryDt = DateTime.UtcNow;

                        _context.HrEmpAddressApprls.Update(hrEmpAddress);
                    }
                }
                else if (communications.UpdateType == "Approved")
                {
                    if (isWorkflowNeeded)
                    {
                        string? codeId = await _employeeRepository.GenerateRequestId(communications.EmpID);
                        if (!string.IsNullOrEmpty(codeId))
                        {
                            var hrSavecommunication = new HrEmpAddressApprl
                            {
                                EmpId = communications.EmpID,
                                InstId = communications.InstId,
                                Add1 = communications.address1,
                                Add2 = communications.address2,
                                Country = communications.countryID,
                                Pbno = communications.PostboxNo,
                                Phone = communications.ContactNo,
                                Mobile = communications.mobileNo,
                                OfficePhone = communications.OfficeNo,
                                Extension = communications.Extension,
                                Email = communications.EmailField,
                                PersonalEmail = communications.PersonalEmail,
                                EntryBy = communications.Entry_By,
                                EntryDt = DateTime.UtcNow,
                                Status = "A",
                                FlowStatus = "E",
                                RequestId = await _employeeRepository.GetLastSequence(codeId),
                                MasterId = communications.DetailID
                            };

                            await _context.HrEmpAddressApprls.AddAsync(hrSavecommunication);
                            await _employeeRepository.UpdateCodeGeneration(codeId);
                        }
                    }
                    else
                    {
                        var hrEmpAddress = await _context.HrEmpAddresses
                            .FirstOrDefaultAsync(x => x.AddId == communications.DetailID && x.EmpId == communications.EmpID);

                        if (hrEmpAddress != null)
                        {
                            hrEmpAddress.InstId = communications.InstId;
                            hrEmpAddress.Add1 = communications.address1;
                            hrEmpAddress.Add2 = communications.address2;
                            hrEmpAddress.Country = communications.countryID;
                            hrEmpAddress.Pbno = communications.PostboxNo;
                            hrEmpAddress.Phone = communications.ContactNo;
                            hrEmpAddress.Mobile = communications.mobileNo;
                            hrEmpAddress.OfficePhone = communications.OfficeNo;
                            hrEmpAddress.Extension = communications.Extension;
                            hrEmpAddress.OfficialEmail = communications.EmailField;
                            hrEmpAddress.PersonalEmail = communications.PersonalEmail;
                            hrEmpAddress.EntryBy = communications.Entry_By;
                            hrEmpAddress.EntryDt = DateTime.UtcNow;

                            _context.HrEmpAddresses.UpdateRange(hrEmpAddress);
                        }

                        var hrEmpMaster = await _context.HrEmpMasters
                            .FirstOrDefaultAsync(x => x.EmpId == communications.EmpID);

                        if (hrEmpMaster != null)
                        {
                            hrEmpMaster.ModifiedDate = DateTime.UtcNow;
                            _context.HrEmpMasters.Update(hrEmpMaster);
                        }

                        var hrEmpCommunication = new HrEmpAddressApprl
                        {
                            InstId = communications.InstId,
                            EmpId = communications.EmpID,
                            Add1 = communications.address1,
                            Add2 = communications.address2,
                            Country = communications.countryID,
                            Pbno = communications.PostboxNo,
                            Phone = communications.ContactNo,
                            Mobile = communications.mobileNo,
                            OfficePhone = communications.OfficeNo,
                            Extension = communications.Extension,
                            Email = communications.EmailField,
                            PersonalEmail = communications.PersonalEmail,
                            EntryBy = communications.Entry_By,
                            EntryDt = DateTime.UtcNow,
                            Status = "A",
                            FlowStatus = "E",
                            RequestId = null,
                            DateFrom = DateTime.UtcNow
                        };

                        await _context.HrEmpAddressApprls.AddAsync(hrEmpCommunication);
                    }

                    var admUser = await _context.AdmUserMasters
                        .Join(_context.HrEmployeeUserRelations,
                              a => a.UserId,
                              b => b.UserId,
                              (a, b) => new { User = a, Relation = b })
                        .Where(joined => joined.Relation.EmpId == communications.EmpID)
                        .Select(joined => joined.User)
                        .FirstOrDefaultAsync();

                    if (admUser != null)
                    {
                        admUser.Email = communications.EmailField;
                        _context.AdmUserMasters.Update(admUser);
                    }
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return "Successfully updated";
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                Console.WriteLine($"Error: {ex.Message}");
                return "Error occurred while updating communication.";
            }
        }

        public async Task<string> SubmitAssetDetailsNewAsync(SubmitAssetNewDto submitAssetNewDto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                if (submitAssetNewDto == null)
                    return "Invalid data provided.";

                // Step 1: Insert into ReasonMaster
                var reasonMaster = new ReasonMaster
                {
                    Description = submitAssetNewDto.Description,
                    Type = submitAssetNewDto.Type,
                    EntryBy = submitAssetNewDto.EmpID,
                    EntryDate = DateTime.UtcNow,
                    Value = submitAssetNewDto.Value,
                    Others = submitAssetNewDto.ArraySecond?.FirstOrDefault()?.Others ?? 0,
                    Status = "A",
                    AssetSpecEmpId = submitAssetNewDto.EmpID
                };

                await _context.ReasonMasters.AddAsync(reasonMaster);
                await _context.SaveChangesAsync();
                int reasonId = reasonMaster.ReasonId; 

                
                if (submitAssetNewDto.Array?.Any() == true)
                {
                    var reasonMasterFieldValues = submitAssetNewDto.Array
                        .Where(field => field.FieldValues != null && field.GcategoryFieldID > 0) 
                        .Select(field => new ReasonMasterFieldValue
                        {
                            ReasonId = reasonId,
                            CategoryFieldId = field.GcategoryFieldID, 
                            FieldValues = field.FieldValues,
                            CreatedDate = DateTime.UtcNow
                        }).ToList();

                    _context.ReasonMasterFieldValues.AddRange(reasonMasterFieldValues);
                    await _context.SaveChangesAsync();
                }


                if (submitAssetNewDto.ArraySecond?.Any() == true)
                {
                 
                    var generalCategoryIds = submitAssetNewDto.ArraySecond
                        .Select(c => c.GcategoryID) // Ensure GcategoryID is int
                        .Distinct()
                        .ToList();

                    // Fetch GeneralCategory dictionary
                    var generalCategoryDict = generalCategoryIds.Any()
                        ? await _context.GeneralCategories
                            .Where(gc => generalCategoryIds.Contains(gc.Id)) // Ensure Id is int
                            .ToDictionaryAsync(gc => gc.Id, gc => gc.Id)
                        : new Dictionary<int, int>();

                    // Fetch ReasonMaster dictionary (only non-null `Value` entries)
                    var reasonMasterDict = new Dictionary<int, int>();

                    if (generalCategoryIds.Any())
                    {
                        var reasonMasters = await _context.ReasonMasters
                            .Where(rm => rm.Value.HasValue && generalCategoryIds.Contains(rm.Value.Value))
                            .ToListAsync();

                        foreach (var rm in reasonMasters)
                        {
                            reasonMasterDict.TryAdd(rm.Value.Value, rm.ReasonId); 
                        }
                    }

                  
                    var employeesAssetsAssignList = submitAssetNewDto.ArraySecond
                        .Select(category => new EmployeesAssetsAssign
                        {
                            EmpId = submitAssetNewDto.EmpID,
                            AssetGroup = generalCategoryDict.TryGetValue(category.GcategoryID, out var assetGroupId)
                                ? assetGroupId.ToString() : "0", // Convert int to string
                            Asset = reasonMasterDict.TryGetValue(category.GcategoryID, out var reasonMasterId)
                                ? reasonMasterId.ToString() : reasonId.ToString(), // Convert int to string
                            ReceivedDate = submitAssetNewDto.ReleaseDate,
                            Status = "Open",
                            EntryBy = submitAssetNewDto.Entryby,
                            Remarks = submitAssetNewDto.Remarks,
                            ReturnDate = submitAssetNewDto.ReturnDate,
                            IssueDate = submitAssetNewDto.ReleaseDate,
                            IsActive = true
                        }).ToList();

                    _context.EmployeesAssetsAssigns.AddRange(employeesAssetsAssignList);
                    await _context.SaveChangesAsync();
                }

                await transaction.CommitAsync();
                return "Successfully Saved";
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return $"Error: {ex.Message}";
            }
        }

        public async Task<string> UpdateAssetDetailsNewAsync(SubmitAssetNewDto submitAssetNewDto, int AssetRole)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                if (submitAssetNewDto == null)
                    return "Invalid data provided.";

                // Step 1: Get AssetReasonId from EmployeesAssetsAssign
                var assetAssign = await _context.EmployeesAssetsAssigns
                    .Where(a => a.AssignId == AssetRole)
                    .Select(a => a.Asset)
                    .FirstOrDefaultAsync();

                if (string.IsNullOrEmpty(assetAssign))
                    return "Asset not found.";

                int assetReasonId = int.Parse(assetAssign); // Ensure conversion to int

                // Step 2: Update ReasonMaster description
                var reasonMaster = await _context.ReasonMasters.FindAsync(assetReasonId);
                if (reasonMaster != null)
                {
                    reasonMaster.Description = submitAssetNewDto.Description;
                    _context.ReasonMasters.UpdateRange(reasonMaster);
                }

                // Step 3: Update ReasonMasterFieldValue using array data
                if (submitAssetNewDto.Array?.Any() == true)
                {
                    var reasonMasterFieldValues = await _context.ReasonMasterFieldValues
                        .Where(a => a.ReasonId == assetReasonId)
                        .ToListAsync();

                    foreach (var field in submitAssetNewDto.Array)
                    {
                        var existingField = reasonMasterFieldValues
                            .FirstOrDefault(f => f.CategoryFieldId == field.GcategoryFieldID);

                        if (existingField != null)
                        {
                            existingField.FieldValues = field.FieldValues; // Update existing field value
                        }
                    }

                    _context.ReasonMasterFieldValues.UpdateRange(reasonMasterFieldValues);
                }

                // Step 4: Update EmployeesAssetsAssign
                var employeeAsset = await _context.EmployeesAssetsAssigns
                    .FirstOrDefaultAsync(a => a.AssignId == AssetRole);

                if (employeeAsset != null)
                {
                    employeeAsset.ReceivedDate = submitAssetNewDto.ReleaseDate;
                    employeeAsset.EntryBy = submitAssetNewDto.EmpID;
                    employeeAsset.Remarks = submitAssetNewDto.Remarks;
                    employeeAsset.ReturnDate = submitAssetNewDto.ReturnDate;
                    employeeAsset.IssueDate = submitAssetNewDto.ReleaseDate;

                    _context.EmployeesAssetsAssigns.Update(employeeAsset);
                }

                // Save changes & commit transaction
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return "Successfully Saved";
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return $"Error: {ex.Message}";
            }
        }


        public async Task<List<AssetParameterDto>> GetAssetParameterAsync()
        {
            return await _context.CompanyParameters
                .Where(c => c.ParameterCode == "EnableAssetSequenceNodropdown")
                .Select(c => new AssetParameterDto
                {
                    Value = c.Value ?? 0,   // ✅ No conversion needed (if Value is nullable)
                    ParameterCode = c.ParameterCode,
                    Description = c.Description
                })
                .ToListAsync();
        }





















    }
}