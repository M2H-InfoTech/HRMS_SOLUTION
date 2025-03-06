using EMPLOYEE_INFORMATION.HRMS.EmployeeInformation.Models.Models.Entity;
using EMPLOYEE_INFORMATION.Models;
using EMPLOYEE_INFORMATION.Models.Entity;
using HRMS.EmployeeInformation.Models;
using HRMS.EmployeeInformation.Models.Models.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace EMPLOYEE_INFORMATION.Data;

public partial class EmployeeDBContext : DbContext
{
    private IConfiguration _configuration { get; }
    public EmployeeDBContext()
    {
    }

    public EmployeeDBContext(DbContextOptions<EmployeeDBContext> options, IConfiguration configuration) : base(options)
    {
        _configuration = configuration;
    }


    public virtual DbSet<CompanyParameter> CompanyParameters { get; set; }

    public virtual DbSet<CompanyParameters01> CompanyParameters01s { get; set; }

    public virtual DbSet<CompanyParameters02> CompanyParameters02s { get; set; }

    public virtual DbSet<EmployeeDetail> EmployeeDetails { get; set; }

    public virtual DbSet<ParameterControlType> ParameterControlTypes { get; set; }
    public virtual DbSet<EmployeeCurrentStatus> EmployeeCurrentStatuses { get; set; }

    public virtual DbSet<EntityAccessRights02> EntityAccessRights02s { get; set; }

    public virtual DbSet<HighLevelViewTable> HighLevelViewTables { get; set; }

    public virtual DbSet<HrEmpAddress> HrEmpAddresses { get; set; }

    public virtual DbSet<HrEmpImage> HrEmpImages { get; set; }

    public virtual DbSet<HrEmpMaster> HrEmpMasters { get; set; }

    public virtual DbSet<HrEmpPersonal> HrEmpPersonals { get; set; }

    public virtual DbSet<HrEmpReporting> HrEmpReportings { get; set; }

    public virtual DbSet<HrEmpStatusSetting> HrEmpStatusSettings { get; set; }

    public virtual DbSet<Resignation> Resignations { get; set; }

    public virtual DbSet<SpecialAccessRight> SpecialAccessRights { get; set; }

    public virtual DbSet<HrmValueType> HrmValueTypes { get; set; }

    public virtual DbSet<AdmCountryMaster> AdmCountryMasters { get; set; }

    public virtual DbSet<ReasonMaster> ReasonMasters { get; set; }

    public virtual DbSet<AdmReligionMaster> AdmReligionMasters { get; set; }

    public virtual DbSet<Payscale00> Payscale00s { get; set; }

    public virtual DbSet<HrmsDocument00> HrmsDocument00s { get; set; }

    public virtual DbSet<HrmsDocumentField00> HrmsDocumentField00s { get; set; }

    public virtual DbSet<HrmsEmpdocumentsApproved00> HrmsEmpdocumentsApproved00s { get; set; }

    public virtual DbSet<HrmsEmpdocumentsApproved01> HrmsEmpdocumentsApproved01s { get; set; }

    public virtual DbSet<HrEmpAddress01> HrEmpAddress01s { get; set; }

    public virtual DbSet<HrEmpLanguagemaster> HrEmpLanguagemasters { get; set; }

    public virtual DbSet<EmployeeLanguageSkill> EmployeeLanguageSkills { get; set; }
    public virtual DbSet<CommunicationRequestWorkFlowstatus> CommunicationRequestWorkFlowstatuses { get; set; }

    public virtual DbSet<HrEmpAddressApprl> HrEmpAddressApprls { get; set; }

    public virtual DbSet<HrEmpAddress01Apprl> HrEmpAddress01Apprls { get; set; }

    public virtual DbSet<CommunicationAdditionalRequestWorkFlowstatus> CommunicationAdditionalRequestWorkFlowstatuses { get; set; }

    public virtual DbSet<HrEmpEmergaddress> HrEmpEmergaddresses { get; set; }
    public virtual DbSet<GeneralCategory> GeneralCategories { get; set; }
    public virtual DbSet<EmployeeHobby> EmployeeHobbies { get; set; }

    public virtual DbSet<EmpReward> EmpRewards { get; set; }

    public virtual DbSet<AchievementMaster> AchievementMasters { get; set; }

    public virtual DbSet<QualificationRequestWorkFlowstatus> QualificationRequestWorkFlowstatuses { get; set; }

    public virtual DbSet<HrEmpQualificationApprl> HrEmpQualificationApprls { get; set; }

    public virtual DbSet<HrEmpQualification> HrEmpQualifications { get; set; }
    public virtual DbSet<QualificationAttachment> QualificationAttachments { get; set; }

    public virtual DbSet<HrEmpTechnicalApprl> HrEmpTechnicalApprls { get; set; }

    public virtual DbSet<SkillSetRequestWorkFlowstatus> SkillSetRequestWorkFlowstatuses { get; set; }

    public virtual DbSet<HrEmpTechnical> HrEmpTechnicals { get; set; }

    public virtual DbSet<ReasonMasterFieldValue> ReasonMasterFieldValues { get; set; }
    public virtual DbSet<GeneralCategoryField> GeneralCategoryFields { get; set; }

    public virtual DbSet<HrmsDatatype> HrmsDatatypes { get; set; }

    public virtual DbSet<HrmsDocTypeMaster> HrmsDocTypeMasters { get; set; }

    public virtual DbSet<HrmsEmpdocuments00> HrmsEmpdocuments00s { get; set; }

    public virtual DbSet<HrmsEmpdocuments01> HrmsEmpdocuments01s { get; set; }

    public virtual DbSet<EmpDocumentAccess> EmpDocumentAccesses { get; set; }

    public virtual DbSet<HrmsEmpdocuments02> HrmsEmpdocuments02s { get; set; }

    public virtual DbSet<HrmsEmpdocumentsApproved02> HrmsEmpdocumentsApproved02s { get; set; }
    public virtual DbSet<AssignedLetterType> AssignedLetterTypes { get; set; }

    public virtual DbSet<Dependent00> Dependent00s { get; set; }

    public virtual DbSet<DependentMaster> DependentMasters { get; set; }

    public virtual DbSet<LetterMaster00> LetterMaster00s { get; set; }

    public virtual DbSet<LetterMaster01> LetterMaster01s { get; set; }

    public virtual DbSet<TaxDeclarationFileUpload> TaxDeclarationFileUploads { get; set; }
    public virtual DbSet<EmployeeCertification> EmployeeCertifications { get; set; }

    public virtual DbSet<CurrencyMaster> CurrencyMasters { get; set; }

    public virtual DbSet<HrEmpProfdtl> HrEmpProfdtls { get; set; }

    public virtual DbSet<HrEmpProfdtlsApprl> HrEmpProfdtlsApprls { get; set; }

    public virtual DbSet<ProfessionalRequestWorkFlowstatus> ProfessionalRequestWorkFlowstatuses { get; set; }
    public virtual DbSet<ConsultantDetail> ConsultantDetails { get; set; }
    public virtual DbSet<HrEmpreference> HrEmpreferences { get; set; }
    public virtual DbSet<EmployeesAssetsAssign> EmployeesAssetsAssigns { get; set; }
    //public virtual DbSet<HrmsCommonField01> HrmsCommonField01s { get; set; }
    public virtual DbSet<HrmsCommonMaster00> HrmsCommonMaster00s { get; set; }
    public virtual DbSet<AssignedAsset> AssignedAssets { get; set; }
    public virtual DbSet<HrEmployeeUserRelation> HrEmployeeUserRelations { get; set; }
    public virtual DbSet<CommonField> CommonFields { get; set; }
    public virtual DbSet<PersonalDetailsHistory> PersonalDetailsHistories { get; set; }

    public virtual DbSet<AdmCodegenerationmaster> AdmCodegenerationmasters { get; set; }

    public virtual DbSet<EntityApplicable00> EntityApplicable00s { get; set; }

    public virtual DbSet<LevelSettingsAccess00> LevelSettingsAccess00s { get; set; }

    public virtual DbSet<TransactionMaster> TransactionMasters { get; set; }
    public virtual DbSet<HrmsCommonField01> HrmsCommonField01s { get; set; }
    public virtual DbSet<TrainingSchedule> TrainingSchedules { get; set; }
    public virtual DbSet<TrainingMaster> TrainingMasters { get; set; }
    public virtual DbSet<TrainingMaster01> TrainingMaster01s { get; set; }
    public virtual DbSet<BiometricsDtl> BiometricsDtls { get; set; }
    public virtual DbSet<HrmHolidayMasterAccess> HrmHolidayMasterAccesses { get; set; }
    public virtual DbSet<AttendancepolicyMasterAccess> AttendancepolicyMasterAccesses { get; set; }
    public virtual DbSet<ShiftMasterAccess> ShiftMasterAccesses { get; set; }
    public virtual DbSet<HrmLeaveEmployeeleaveaccess> HrmLeaveEmployeeleaveaccesses { get; set; }
    public virtual DbSet<LeavepolicyMasterAccess> LeavepolicyMasterAccesses { get; set; }
    public virtual DbSet<HrmLeaveBasicsettingsaccess> HrmLeaveBasicsettingsaccesses { get; set; }
    public virtual DbSet<ParamWorkFlow00> ParamWorkFlow00s { get; set; }
    public virtual DbSet<ParamWorkFlowEntityLevel00> ParamWorkFlowEntityLevel00s { get; set; }
    public virtual DbSet<ParamWorkFlow02> ParamWorkFlow02s { get; set; }
    public virtual DbSet<ParamWorkFlow01> ParamWorkFlow01s { get; set; }
    public virtual DbSet<WorkFlowDetail> WorkFlowDetails { get; set; }
    public virtual DbSet<EntityApplicable01> EntityApplicable01s { get; set; }
    public virtual DbSet<HrEmpReportingHstry> HrEmpReportingHstries { get; set; }
    public virtual DbSet<PositionHistory> PositionHistories { get; set; }
    public virtual DbSet<PayscaleRequest00> PayscaleRequest00s { get; set; }
    public virtual DbSet<PayscaleRequest01> PayscaleRequest01s { get; set; }
    public virtual DbSet<PayscaleRequest02> PayscaleRequest02s { get; set; }
    public virtual DbSet<PayCodeMaster01> PayCodeMaster01s { get; set; }
    public virtual DbSet<BranchDetail> BranchDetails { get; set; }
    public virtual DbSet<EmployeeLatestPayrollPeriod> EmployeeLatestPayrollPeriods { get; set; }
    public virtual DbSet<EmployeeLatestPayrollBatch> EmployeeLatestPayrollBatches { get; set; }
    public virtual DbSet<Payroll00> Payroll00s { get; set; }
    public virtual DbSet<PayCodeMaster00> PayCodeMaster00s { get; set; }

    public virtual DbSet<EditInfoHistory> EditInfoHistories { get; set; }

    public virtual DbSet<EditInfoMaster01> EditInfoMaster01s { get; set; }

    public virtual DbSet<HrShift00> HrShift00s { get; set; }
    public virtual DbSet<ParamRole02> ParamRole02s { get; set; }
    public virtual DbSet<Categorymasterparameter> Categorymasterparameters { get; set; }
    public virtual DbSet<ParamRole01> ParamRole01s { get; set; }

    public virtual DbSet<ParamRole00> ParamRole00s { get; set; }
    public virtual DbSet<ParamRoleEntityLevel00> ParamRoleEntityLevel00s { get; set; }


    public virtual DbSet<Geotagging00> Geotagging00s { get; set; }

    public virtual DbSet<Geotagging00A> Geotagging00As { get; set; }

    public virtual DbSet<Geotagging01> Geotagging01s { get; set; }

    public virtual DbSet<Geotagging01A> Geotagging01As { get; set; }

    public virtual DbSet<Geotagging02> Geotagging02s { get; set; }

    public virtual DbSet<Geotagging02A> Geotagging02As { get; set; }

    public virtual DbSet<HraHistory> HraHistories { get; set; }

    public virtual DbSet<AdmUserMaster> AdmUserMasters { get; set; }

    public virtual DbSet<AdmUserRoleMaster> AdmUserRoleMasters { get; set; }
    public virtual DbSet<TabAccessRight> TabAccessRights { get; set; }
    public virtual DbSet<TabMaster> TabMasters { get; set; }
    public virtual DbSet<TransferDetail> TransferDetails { get; set; }

    public virtual DbSet<TransferDetails00> TransferDetails00s { get; set; }

    public virtual DbSet<LicensedCompanyDetail> LicensedCompanyDetails { get; set; }
    public virtual DbSet<Categorymaster> Categorymasters { get; set; }

    public virtual DbSet<TransferTransition00> TransferTransition00s { get; set; }
    public virtual DbSet<CompanyConveyanceHistory> CompanyConveyanceHistories { get; set; }

    public virtual DbSet<CompanyVehicleHistory> CompanyVehicleHistories { get; set; }
    public virtual DbSet<SurveyRelation> SurveyRelations { get; set; }

    public virtual DbSet<ProbationRating00> ProbationRating00s { get; set; }

    public virtual DbSet<ProbationRating02> ProbationRating02s { get; set; }

    public virtual DbSet<ProbationWorkFlowstatus> ProbationWorkFlowstatuses { get; set; }
    public virtual DbSet<CategoryGroup> CategoryGroups { get; set; }
    public virtual DbSet<AssetcategoryCode> AssetcategoryCodes { get; set; }

    public virtual DbSet<AssetRequestHistory> AssetRequestHistories { get; set; }
    public virtual DbSet<HrmsEmpdocumentsHistory00> HrmsEmpdocumentsHistory00s { get; set; }
    public virtual DbSet<HrmsEmpdocumentsHistory01> HrmsEmpdocumentsHistory01s { get; set; }

    public virtual DbSet<HrmsEmpdocumentsHistory02> HrmsEmpdocumentsHistory02s { get; set; }

    public virtual DbSet<TravelType> TravelTypes { get; set; }

    public virtual DbSet<TimeOffSet> TimeOffSets { get; set; }

    public virtual DbSet<RoleDelegation00> RoleDelegation00s { get; set; }

    public virtual DbSet<Roledelegationtransaction> Roledelegationtransactions { get; set; }

    public virtual DbSet<Dependent01> Dependent01s { get; set; }

    public virtual DbSet<DependentEducation> DependentEducations { get; set; }

    public virtual DbSet<EducationMaster> EducationMasters { get; set; }

    public virtual DbSet<EdCourseMaster> EdCourseMasters { get; set; }

    public virtual DbSet<EdSpecializationMaster> EdSpecializationMasters { get; set; }

    public virtual DbSet<UniversityMaster> UniversityMasters { get; set; }
    public virtual DbSet<SpecialWorkFlow> SpecialWorkFlows { get; set; }
    public virtual DbSet<SalaryConfirmationLetterType> SalaryConfirmationLetterTypes { get; set; }
    public virtual DbSet<LetterWorkflowStatus> LetterWorkflowStatuses { get; set; }
    public virtual DbSet<EmailNotification> EmailNotifications { get; set; }

    public virtual DbSet<HrEmpEmergaddressApprl> HrEmpEmergaddressApprls { get; set; }
    public virtual DbSet<EmployeeSequenceAccess> EmployeeSequenceAccesses { get; set; }

    public virtual DbSet<EditInfoMaster00> EditInfoMaster00s { get; set; }
    public virtual DbSet<DesignationDetail> DesignationDetails { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)

         //=> optionsBuilder.UseSqlServer("Server=10.25.25.250\\sql2017,1435;Database=VELLAPALLY-02-01-2025;User Id=sa;Password=asd@123.;Integrated Security=False;TrustServerCertificate=True;");
         => optionsBuilder.UseSqlServer(_configuration.GetConnectionString("Default"));


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CompanyParameter>(entity =>
        {
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Data).IsUnicode(false);
            entity.Property(e => e.Description).IsUnicode(false);
            entity.Property(e => e.EntityLevel).IsUnicode(false);
            entity.Property(e => e.FileName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.MultipleValues)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("multipleValues");
            entity.Property(e => e.ParameterCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Text).IsUnicode(false);
            entity.Property(e => e.Type)
                .HasMaxLength(30)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Payscale00>(entity =>
        {
            entity.HasKey(e => e.PayScaleId).HasName("PK__Payscale__3CF87F943B7906D0");

            entity.ToTable("Payscale00");

            entity.Property(e => e.PayScaleId).HasColumnName("PayScaleID");
            entity.Property(e => e.EffectiveFrom).HasColumnType("datetime");
            entity.Property(e => e.EffectiveTo).HasColumnType("datetime");
            entity.Property(e => e.EmployeeStatus)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.RevisionFrom).HasColumnType("datetime");
        });

        modelBuilder.Entity<CompanyParameters01>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("CompanyParameters01");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Data).IsUnicode(false);
            entity.Property(e => e.FileName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.LevelId).HasColumnName("LevelID");
            entity.Property(e => e.LinkId).HasColumnName("LinkID");
            entity.Property(e => e.MultipleValues)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("multipleValues");
            entity.Property(e => e.Text).IsUnicode(false);
        });

        modelBuilder.Entity<CompanyParameters02>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__CompanyP__3214EC07538F6B6E");

            entity.ToTable("CompanyParameters02");

            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Data).IsUnicode(false);
            entity.Property(e => e.EmpId).HasColumnName("EmpID");
            entity.Property(e => e.FileName)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.MultipleValues)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("multipleValues");
            entity.Property(e => e.Text)
                .HasMaxLength(1000)
                .IsUnicode(false);
        });
        modelBuilder.Entity<AdmReligionMaster>(entity =>
        {
            entity.HasKey(e => e.ReligionId).HasName("PK_Religion_Master");

            entity.ToTable("ADM_Religion_Master");

            entity.HasIndex(e => e.ReligionName, "IX_Religion_Master").IsUnique();

            entity.Property(e => e.ReligionId).HasColumnName("Religion_ID");
            entity.Property(e => e.EntryBy).HasColumnName("Entry_By");
            entity.Property(e => e.EntryDate)
                .HasColumnType("datetime")
                .HasColumnName("Entry_Date");
            entity.Property(e => e.InstId).HasColumnName("Inst_ID");
            entity.Property(e => e.ModiBy).HasColumnName("Modi_By");
            entity.Property(e => e.ModiDate)
                .HasColumnType("datetime")
                .HasColumnName("Modi_Date");
            entity.Property(e => e.ReligionLocName)
                .HasMaxLength(50)
                .HasColumnName("Religion_LocName");
            entity.Property(e => e.ReligionName)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("Religion_Name");
        });

        modelBuilder.Entity<EmployeeDetail>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("EmployeeDetails");

            entity.Property(e => e.BandId).HasColumnName("BandID");
            entity.Property(e => e.BranchId).HasColumnName("BranchID");
            entity.Property(e => e.DateOfBirth).HasColumnType("datetime");
            entity.Property(e => e.DeletedDate).HasColumnType("datetime");
            entity.Property(e => e.EmpCode)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Emp_Code");
            entity.Property(e => e.EmpEntity).IsUnicode(false);
            entity.Property(e => e.EmpFirstEntity)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.EmpId)
                .ValueGeneratedOnAdd()
                .HasColumnName("Emp_Id");
            entity.Property(e => e.EmpStatus).HasColumnName("emp_status");
            entity.Property(e => e.FirstEntryDate).HasColumnType("datetime");
            entity.Property(e => e.Gender)
                .HasMaxLength(6)
                .IsUnicode(false);
            entity.Property(e => e.GradeId).HasColumnName("GradeID");
            entity.Property(e => e.GratuityStrtDate).HasColumnType("datetime");
            entity.Property(e => e.GuardiansName)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("Guardians_Name");
            entity.Property(e => e.InstId).HasColumnName("Inst_Id");
            entity.Property(e => e.Ishra).HasColumnName("ISHRA");
            entity.Property(e => e.JoinDt)
                .HasColumnType("datetime")
                .HasColumnName("Join_Dt");
            entity.Property(e => e.Name)
                .HasMaxLength(152)
                .IsUnicode(false);
            entity.Property(e => e.ProbationDt)
                .HasColumnType("datetime")
                .HasColumnName("Probation_Dt");
        });

        modelBuilder.Entity<ParameterControlType>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("ParameterControlType");

            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.ParamControlDesc)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ParamControlId).HasColumnName("ParamControlID");
        });


        //------------------------------------Start--------------------------------------------------

        modelBuilder.Entity<EmployeeCurrentStatus>(entity =>
        {
            entity.HasKey(e => e.StatusId).HasName("PK__Employee__C8EE20437105B1C6");

            entity.ToTable("EmployeeCurrentStatus");

            entity.Property(e => e.StatusId).HasColumnName("StatusID");
            entity.Property(e => e.Active)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.StatusDesc)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Status_Desc");
        });

        modelBuilder.Entity<EntityAccessRights02>(entity =>
        {
            entity.HasKey(e => e.SubTrxId);

            entity.ToTable("EntityAccessRights02");

            entity.Property(e => e.LinkId).IsUnicode(false);
            entity.Property(e => e.SubCategoryList).IsUnicode(false);
        });

        modelBuilder.Entity<HighLevelViewTable>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("HighLevelViewTable");

            entity.HasIndex(e => e.LastEntityId, "IX_tblHighlevelviewtable");

            entity.Property(e => e.ActiveEntityStatus).HasDefaultValue(1);
            entity.Property(e => e.LastEntityId).HasColumnName("LastEntityID");
            entity.Property(e => e.LevelEightDescription)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.LevelElevenDescription)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.LevelFiveDescription)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.LevelFourDescription)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.LevelNineDescription)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.LevelOneDescription)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.LevelSevenDescription)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.LevelSixDescription)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.LevelTenDescription)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.LevelThreeDescription)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.LevelTwelveDescription)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.LevelTwoDescription)
                .HasMaxLength(1000)
                .IsUnicode(false);
        });

        modelBuilder.Entity<HrEmpAddress>(entity =>
        {
            entity.HasKey(e => e.AddId).HasName("PK__HR_EMP_A__819EBA9B2BF84BE3");

            entity.ToTable("HR_EMP_ADDRESS");

            entity.Property(e => e.AddId).HasColumnName("Add_Id");
            entity.Property(e => e.Add1)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Add2)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Add3)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.AddType)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("Add_Type");
            entity.Property(e => e.ApprlId).HasColumnName("ApprlID");
            entity.Property(e => e.OfficialEmail)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("EMail");
            entity.Property(e => e.EmpId).HasColumnName("Emp_Id");
            entity.Property(e => e.EntryBy).HasColumnName("Entry_By");
            entity.Property(e => e.EntryDt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Entry_Dt");
            entity.Property(e => e.Extension)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.HomeCountryPhone)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.InstId).HasColumnName("Inst_Id");
            entity.Property(e => e.Mobile)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.OfficePhone)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Pbno)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("PBNo");
            entity.Property(e => e.PersonalEmail)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("PersonalEMail");
            entity.Property(e => e.Phone)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<HrEmpImage>(entity =>
        {
            entity.HasKey(e => e.EmpImgId);

            entity.ToTable("HR_EMP_IMAGES");

            entity.Property(e => e.EmpImgId).HasColumnName("emp_img_id");
            entity.Property(e => e.Active)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("active");
            entity.Property(e => e.EmpId).HasColumnName("emp_id");
            entity.Property(e => e.EmpImage)
                .IsUnicode(false)
                .HasColumnName("Emp_image");
            entity.Property(e => e.FingerUrl)
                .IsUnicode(false)
                .HasColumnName("finger_url");
            entity.Property(e => e.ImageUrl)
                .IsUnicode(false)
                .HasColumnName("image_url");
            entity.Property(e => e.InstId).HasColumnName("inst_id");
        });

        modelBuilder.Entity<HrEmpMaster>(entity =>
        {
            entity.HasKey(e => e.EmpId).HasName("PK__HR_EMP_M__262359AB964509AB");

            entity.ToTable("HR_EMP_MASTER");

            entity.Property(e => e.EmpId).HasColumnName("Emp_Id");
            entity.Property(e => e.BandId).HasColumnName("BandID");
            entity.Property(e => e.BonusDt)
                .HasColumnType("datetime")
                .HasColumnName("Bonus_Dt");
            entity.Property(e => e.BranchId).HasColumnName("BranchID");
            entity.Property(e => e.CompanyId)
                .HasDefaultValue(0)
                .HasColumnName("CompanyID");
            entity.Property(e => e.CurrentStatus).HasDefaultValue(0);
            entity.Property(e => e.DateOfBirth).HasColumnType("datetime");
            entity.Property(e => e.DeletedDate).HasColumnType("datetime");
            entity.Property(e => e.EmpCode)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Emp_Code");
            entity.Property(e => e.EmpEntity).IsUnicode(false);
            entity.Property(e => e.EmpFileNumber)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.EmpFirstEntity)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.EmpStatus).HasColumnName("emp_status");
            entity.Property(e => e.EntryBy).HasColumnName("Entry_By");
            entity.Property(e => e.EntryDt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Entry_Dt");
            entity.Property(e => e.FirstEntryDate).HasColumnType("datetime");
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("First_Name");
            entity.Property(e => e.Gender)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.GradeId).HasColumnName("GradeID");
            entity.Property(e => e.GratuityStrtDate).HasColumnType("datetime");
            entity.Property(e => e.GuardiansName)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("Guardians_Name");
            entity.Property(e => e.InitialDate).HasColumnType("datetime");
            entity.Property(e => e.InstId).HasColumnName("Inst_Id");
            entity.Property(e => e.IsDelete).HasDefaultValue(false);
            entity.Property(e => e.IsMarkAttn).HasColumnName("isMarkAttn");
            entity.Property(e => e.IsProbation).HasColumnName("Is_probation");
            entity.Property(e => e.IsSave).HasDefaultValue(0);
            entity.Property(e => e.IsVerified).HasDefaultValue(0);
            entity.Property(e => e.Ishra).HasColumnName("ISHRA");
            entity.Property(e => e.JoinDt)
                .HasColumnType("datetime")
                .HasColumnName("Join_Dt");
            entity.Property(e => e.LastEntity).HasDefaultValue(0);
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Last_Name");
            entity.Property(e => e.MiddleName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Middle_Name");
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.NationalIdNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.NaturePost)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("Nature_Post");
            entity.Property(e => e.NoticePeriod).HasColumnName("Notice_period");
            entity.Property(e => e.PassportNo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Passport_No");
            entity.Property(e => e.ProbationDt)
                .HasColumnType("datetime")
                .HasColumnName("Probation_Dt");
            entity.Property(e => e.RejoinRemarks)
                .IsUnicode(false)
                .HasColumnName("Rejoin_Remarks");
            entity.Property(e => e.RelievingDate).HasColumnType("datetime");
            entity.Property(e => e.ReviewDt)
                .HasColumnType("datetime")
                .HasColumnName("Review_Dt");
            entity.Property(e => e.StatusChangeDate)
                .HasColumnType("datetime")
                .HasColumnName("status_change_date");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            entity.Property(e => e.UserType)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.WeddingDate)
                .HasColumnType("datetime")
                .HasColumnName("Wedding_Date");
        });

        modelBuilder.Entity<HrEmpPersonal>(entity =>
        {
            entity.HasKey(e => e.PerId).HasName("PK__HR_EMP_P__2705F9403F588FDC");

            entity.ToTable("HR_EMP_PERSONAL");

            entity.Property(e => e.PerId).HasColumnName("Per_Id");
            entity.Property(e => e.BirthPlace)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Birth_Place");
            entity.Property(e => e.BloodGrp)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("Blood_Grp");
            entity.Property(e => e.Dob).HasColumnType("datetime");
            entity.Property(e => e.EmpId).HasColumnName("Emp_Id");
            entity.Property(e => e.EmployeeType)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.EntryBy).HasColumnName("Entry_By");
            entity.Property(e => e.EntryDt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Entry_Dt");
            entity.Property(e => e.Gender)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Height)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.IdentMark)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Ident_Mark");
            entity.Property(e => e.InstId).HasColumnName("Inst_Id");
            entity.Property(e => e.MaritalStatus)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("Marital_Status");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            entity.Property(e => e.WeddingDate)
                .HasColumnType("datetime")
                .HasColumnName("Wedding_Date");
            entity.Property(e => e.Weight)
                .HasMaxLength(10)
                .IsUnicode(false);
        });

        modelBuilder.Entity<HrEmpReporting>(entity =>
        {
            entity.HasKey(e => e.ReportId).HasName("PK__HR_EMP_R__30FA9DB19F77FA97");

            entity.ToTable("HR_EMP_REPORTING");

            entity.Property(e => e.ReportId).HasColumnName("Report_ID");
            entity.Property(e => e.Active)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("active");
            entity.Property(e => e.EffectDate)
                .HasColumnType("datetime")
                .HasColumnName("effect_date");
            entity.Property(e => e.EmpId).HasColumnName("emp_id");
            entity.Property(e => e.EntryDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.InstId).HasColumnName("inst_id");
            entity.Property(e => e.ReprotToWhome).HasColumnName("Reprot_to_whome");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<HrEmpStatusSetting>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__HR_EMP_S__3214EC272B2C6BAF");

            entity.ToTable("HR_EMP_STATUS_SETTINGS");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Active)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("active");
            entity.Property(e => e.EntryDate).HasColumnType("datetime");
            entity.Property(e => e.IsResignation)
                .HasDefaultValue(false)
                .HasColumnName("isResignation");
            entity.Property(e => e.Status)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.StatusDesc)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Status_Desc");
            entity.Property(e => e.StatusId).HasColumnName("StatusID");
        });

        modelBuilder.Entity<Resignation>(entity =>
        {
            entity.ToTable("Resignation");

            entity.Property(e => e.ResignationId).HasColumnName("Resignation_Id");
            entity.Property(e => e.ActualRelievingDate).HasColumnType("datetime");
            entity.Property(e => e.ApprovalStatus)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.CurrentRequest).HasDefaultValue(1);
            entity.Property(e => e.DescriptionHeight)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.EmpId).HasColumnName("Emp_Id");
            entity.Property(e => e.EntryBy).HasColumnName("Entry_By");
            entity.Property(e => e.EntryFrom)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.EsobentryDate)
                .HasColumnType("datetime")
                .HasColumnName("ESOBEntryDate");
            entity.Property(e => e.Esobentryby).HasColumnName("ESOBentryby");
            entity.Property(e => e.Esobremark)
                .IsUnicode(false)
                .HasColumnName("ESOBRemark");
            entity.Property(e => e.ExitClearenceStatus)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.FinalApprovalDate).HasColumnType("datetime");
            entity.Property(e => e.FinalSettleStatus)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.FlowStatus)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.HandOverStatus)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.IsDirect).HasDefaultValue(0);
            entity.Property(e => e.IsEmployeeLeftOrganisation)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.OnNoticeEndDate)
                .HasColumnType("datetime")
                .HasColumnName("OnNotice_EndDate");
            entity.Property(e => e.OtherReason)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.PayrollStatus)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.ProxyId).HasColumnName("Proxy_Id");
            entity.Property(e => e.Reason).IsUnicode(false);
            entity.Property(e => e.RejoinApprovalDate).HasColumnType("datetime");
            entity.Property(e => e.RejoinApprovalStatus)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.RejoinDate).HasColumnType("datetime");
            entity.Property(e => e.RejoinFlowStatus)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.RejoinRemarks).IsUnicode(false);
            entity.Property(e => e.RejoinRequestDate).HasColumnType("datetime");
            entity.Property(e => e.RejoinRequestId)
                .IsUnicode(false)
                .HasColumnName("RejoinRequestID");
            entity.Property(e => e.RejoinStatus)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("Rejoin_Status");
            entity.Property(e => e.RelievingDate)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.RelievingType)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("Relieving_Type");
            entity.Property(e => e.Remarks).IsUnicode(false);
            entity.Property(e => e.RequestDate)
                .HasColumnType("datetime")
                .HasColumnName("Request_Date");
            entity.Property(e => e.ResignationDate)
                .HasColumnType("datetime")
                .HasColumnName("Resignation_Date");
            entity.Property(e => e.ResignationRequestId)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("Resignation_Request_Id");
            entity.Property(e => e.ResignationType)
                .HasMaxLength(20)
                .IsUnicode(false);
        });
        modelBuilder.Entity<HrmValueType>(entity =>
        {
            entity.ToTable("HRM_VALUE_TYPES");

            entity.Property(e => e.Code)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Description).IsUnicode(false);
            entity.Property(e => e.ReqId).HasDefaultValue(0);
            entity.Property(e => e.Type)
                .HasMaxLength(50)
                .IsUnicode(false);
        });
        modelBuilder.Entity<AdmCountryMaster>(entity =>
        {
            entity.HasKey(e => e.CountryId).HasName("PK_Country_Master");

            entity.ToTable("ADM_Country_Master");

            entity.HasIndex(e => e.CountryName, "IX_Country_Master").IsUnique();

            entity.HasIndex(e => e.Nationality, "IX_Country_Master_1").IsUnique();

            entity.Property(e => e.CountryId).HasColumnName("Country_ID");
            entity.Property(e => e.CountryLocName)
                .HasMaxLength(50)
                .HasColumnName("Country_locName");
            entity.Property(e => e.CountryName)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("Country_Name");
            entity.Property(e => e.EntryBy).HasColumnName("Entry_By");
            entity.Property(e => e.EntryDate)
                .HasColumnType("datetime")
                .HasColumnName("Entry_Date");
            entity.Property(e => e.InstId).HasColumnName("Inst_ID");
            entity.Property(e => e.ModiBy).HasColumnName("Modi_By");
            entity.Property(e => e.ModiDate)
                .HasColumnType("datetime")
                .HasColumnName("Modi_Date");
            entity.Property(e => e.Nationality)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.NationaltyLocName).HasColumnName("Nationalty_LocName");
        });

        modelBuilder.Entity<ReasonMaster>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("ReasonMaster");

            entity.Property(e => e.AssetRoleId).HasColumnName("AssetRoleID");
            entity.Property(e => e.AssetSpecEmpId).HasColumnName("AssetSpecEmpID");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.DisableInEss).HasColumnName("DisableInESS");
            entity.Property(e => e.DivMasterId)
                .HasDefaultValue(0)
                .HasColumnName("DivMasterID");
            entity.Property(e => e.EntryDate).HasColumnType("datetime");
            entity.Property(e => e.IsMultiAsset).HasColumnName("isMultiAsset");
            entity.Property(e => e.ReasonId)
                .ValueGeneratedOnAdd()
                .HasColumnName("Reason_Id");
            entity.Property(e => e.SeperationType).HasDefaultValue(0);
            entity.Property(e => e.Status)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Type)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Value).HasColumnName("value");
        });
        modelBuilder.Entity<SpecialAccessRight>(entity =>
        {
            entity.HasKey(e => e.SpecialId);
        });
        //------------------------------------End----------------------------------------------------
        modelBuilder.Entity<HrmsDocument00>(entity =>
        {
            entity.HasKey(e => e.DocId).HasName("PK_Document00");

            entity.ToTable("HRMS_Document00");

            entity.Property(e => e.DocId).HasColumnName("DocID");
            entity.Property(e => e.Active).HasDefaultValue(true);
            entity.Property(e => e.DocName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FolderName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.IsEsi).HasColumnName("IsESI");
            entity.Property(e => e.IsMandatory).HasDefaultValue(true);
            entity.Property(e => e.IsPf).HasColumnName("IsPF");
        });

        modelBuilder.Entity<HrmsDocumentField00>(entity =>
        {
            entity.HasKey(e => e.DocFieldId).HasName("PK_DocumentField00");

            entity.ToTable("HRMS_DocumentField00");

            entity.Property(e => e.DocFieldId).HasColumnName("DocFieldID");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DocDescription)
                .HasMaxLength(180)
                .IsUnicode(false);
            entity.Property(e => e.DocId).HasColumnName("DocID");
            entity.Property(e => e.IsMandatory).HasDefaultValue(true);
            entity.Property(e => e.ModifiedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<HrmsEmpdocumentsApproved00>(entity =>
        {
            entity.HasKey(e => e.DocApprovedId).HasName("PK__HRMS_EMP__4E1362CB4D03CC33");

            entity.ToTable("HRMS_EMPDocumentsApproved00");

            entity.Property(e => e.DocApprovedId).HasColumnName("DocApprovedID");
            entity.Property(e => e.DateFrom).HasColumnType("datetime");
            entity.Property(e => e.DetailId).HasColumnName("DetailID");
            entity.Property(e => e.DocId).HasColumnName("DocID");
            entity.Property(e => e.EmpId).HasColumnName("EmpID");
            entity.Property(e => e.EntryDate).HasColumnType("datetime");
            entity.Property(e => e.FinalApprovalDate).HasColumnType("datetime");
            entity.Property(e => e.FlowStatus)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.ProxyId)
                .HasDefaultValue(0)
                .HasColumnName("ProxyID");
            entity.Property(e => e.RequestId)
                .IsUnicode(false)
                .HasColumnName("RequestID");
            entity.Property(e => e.Status)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.TransactionType)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<HrmsEmpdocumentsApproved01>(entity =>
        {
            entity.HasKey(e => e.DocFieldId).HasName("PK__HRMS_EMP__E2BF7E506958AC85");

            entity.ToTable("HRMS_EMPDocumentsApproved01");

            entity.Property(e => e.DocFieldId).HasColumnName("DocFieldID");
            entity.Property(e => e.DetailId).HasColumnName("DetailID");
            entity.Property(e => e.DocValues)
                .HasMaxLength(100)
                .IsUnicode(false);
        });
        modelBuilder.Entity<HrEmpAddress01>(entity =>
        {
            entity.ToTable("HR_EMP_ADDRESS_01");

            // Define Primary Key
            entity.HasKey(e => e.AddrId).HasName("PK_HrEmpAddress");

            entity.Property(e => e.AddrId)
                .ValueGeneratedOnAdd() // This ensures EF knows it's an identity column
                .HasColumnName("AddrID");

            entity.Property(e => e.AlterPhoneNo)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.ApprlId)
                .HasColumnName("ApprlID");

            entity.Property(e => e.ContactAddr)
                .HasMaxLength(5000)
                .IsUnicode(false);

            entity.Property(e => e.EmpId)
                .HasColumnName("EmpID");

            entity.Property(e => e.EntryDate)
                .HasColumnType("datetime");

            entity.Property(e => e.MobileNo)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.PermanentAddr)
                .HasMaxLength(5000)
                .IsUnicode(false);

            entity.Property(e => e.PhoneNo)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.PinNo1)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.Property(e => e.PinNo2)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<HrEmpLanguagemaster>(entity =>
        {
            entity.HasKey(e => e.LanguageId).HasName("PK__HR_EMP_L__12696A622B6100D9");

            entity.ToTable("HR_EMP_LANGUAGEMASTER");

            entity.Property(e => e.LanguageId).HasColumnName("languageId");
            entity.Property(e => e.Code)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Description).IsUnicode(false);
        });

        modelBuilder.Entity<EmployeeLanguageSkill>(entity =>
        {
            entity.HasKey(e => e.EmpLangId).HasName("Emp_LangId");

            entity.ToTable("Employee_LanguageSkills");

            entity.Property(e => e.EmpLangId).HasColumnName("Emp_LangId");
            entity.Property(e => e.LanguageId).IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(5)
                .IsUnicode(false);
        });


        modelBuilder.Entity<CommunicationRequestWorkFlowstatus>(entity =>
        {
            entity.HasKey(e => e.FlowId).HasName("PK__Communic__1184B35CCA6B99A1");

            entity.ToTable("CommunicationRequestWorkFlowstatus");

            entity.Property(e => e.FlowId).HasColumnName("FlowID");
            entity.Property(e => e.ApprovalRemarks)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.ApprovalStatus)
                .HasMaxLength(5)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Deligate)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.EntryBy).HasColumnName("Entry_By");
            entity.Property(e => e.EntryDt)
                .HasColumnType("datetime")
                .HasColumnName("Entry_Dt");
            entity.Property(e => e.EntryFrom)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedBy).HasColumnName("Updated_By");
            entity.Property(e => e.UpdatedDt)
                .HasColumnType("datetime")
                .HasColumnName("Updated_Dt");
            entity.Property(e => e.UpdatedFrom)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<HrEmpAddressApprl>(entity =>
        {
            entity.HasKey(e => e.AddId).HasName("PK__HR_EMP_A__819EBA9B255E7949");

            entity.ToTable("HR_EMP_ADDRESS_APPRL");

            entity.Property(e => e.AddId).HasColumnName("Add_Id");
            entity.Property(e => e.Add1)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Add2)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Add3)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.AddType)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("Add_Type");
            entity.Property(e => e.DateFrom).HasColumnType("datetime");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("EMail");
            entity.Property(e => e.EmpId).HasColumnName("Emp_Id");
            entity.Property(e => e.EntryBy).HasColumnName("Entry_By");
            entity.Property(e => e.EntryDt)
                .HasColumnType("datetime")
                .HasColumnName("Entry_Dt");
            entity.Property(e => e.Extension)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.FlowStatus)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.InstId).HasColumnName("Inst_Id");
            entity.Property(e => e.MasterId)
                .HasDefaultValue(0)
                .HasColumnName("MasterID");
            entity.Property(e => e.Mobile)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.OfficePhone)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Pbno)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("PBNo");
            entity.Property(e => e.PersonalEmail)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("PersonalEMail");
            entity.Property(e => e.Phone)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.RequestId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Request_ID");
            entity.Property(e => e.Status)
                .HasMaxLength(10)
                .IsUnicode(false);
        });

        modelBuilder.Entity<HrEmpAddress01Apprl>(entity =>
        {
            entity.ToTable("HR_EMP_ADDRESS_01_APPRL");

            // Define Primary Key
            entity.HasKey(e => e.AddrId).HasName("PK_HrEmpAddress");

            // Handle Primary Key Mapping
            entity.Property(e => e.AddrId)
                .HasColumnName("AddrID")
                .ValueGeneratedOnAdd(); // Use this if AddrId is an Identity Column

            // If AddrId is NOT an identity column, use this instead:
            // entity.Property(e => e.AddrId).ValueGeneratedNever();

            entity.Property(e => e.AlterPhoneNo)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.ContactAddr)
                .HasMaxLength(5000)
                .IsUnicode(false);

            entity.Property(e => e.DateFrom)
                .HasColumnType("datetime");

            entity.Property(e => e.EmpId)
                .HasColumnName("EmpID");

            entity.Property(e => e.EntryDate)
                .HasColumnType("datetime");

            entity.Property(e => e.FlowStatus)
                .HasMaxLength(10)
                .IsUnicode(false);

            entity.Property(e => e.MasterId)
                .HasDefaultValue(0)
                .HasColumnName("MasterID");

            entity.Property(e => e.MobileNo)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.PermanentAddr)
                .HasMaxLength(5000)
                .IsUnicode(false);

            entity.Property(e => e.PhoneNo)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.PinNo1)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.Property(e => e.PinNo2)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.Property(e => e.RequestId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Request_ID");

            entity.Property(e => e.Status)
                .HasMaxLength(10)
                .IsUnicode(false);
        });

        modelBuilder.Entity<CommunicationAdditionalRequestWorkFlowstatus>(entity =>
        {
            entity.HasKey(e => e.FlowId).HasName("PK__Communic__1184B35CDACD0516");

            entity.ToTable("CommunicationAdditionalRequestWorkFlowstatus");

            entity.Property(e => e.FlowId).HasColumnName("FlowID");
            entity.Property(e => e.ApprovalRemarks)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.ApprovalStatus)
                .HasMaxLength(5)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Deligate)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.EntryBy).HasColumnName("Entry_By");
            entity.Property(e => e.EntryDt)
                .HasColumnType("datetime")
                .HasColumnName("Entry_Dt");
            entity.Property(e => e.EntryFrom)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedBy).HasColumnName("Updated_By");
            entity.Property(e => e.UpdatedDt)
                .HasColumnType("datetime")
                .HasColumnName("Updated_Dt");
            entity.Property(e => e.UpdatedFrom)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<HrEmpEmergaddress>(entity =>
        {
            entity.HasKey(e => e.AddrId).HasName("PK__HR_EMP_E__BCDB8FA3B96DFF75");

            entity.ToTable("HR_EMP_EMERGADDRESS");

            entity.Property(e => e.AddrId).HasColumnName("AddrID");
            entity.Property(e => e.Address)
                .HasMaxLength(5000)
                .IsUnicode(false);
            entity.Property(e => e.AlterPhoneNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ApprlId).HasColumnName("ApprlID");
            entity.Property(e => e.EmerName).IsUnicode(false);
            entity.Property(e => e.EmerRelation).IsUnicode(false);
            entity.Property(e => e.EmpId).HasColumnName("EmpID");
            entity.Property(e => e.EntryDate).HasColumnType("datetime");
            entity.Property(e => e.MobileNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PhoneNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PinNo)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<GeneralCategory>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("GeneralCategory");

            entity.Property(e => e.Code)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Createddate).HasColumnType("datetime");
            entity.Property(e => e.DataTypeId).HasColumnName("DataTypeID");
            entity.Property(e => e.Description).IsUnicode(false);
            entity.Property(e => e.EmplinkId).HasColumnName("EmplinkID");
            entity.Property(e => e.GroupId).HasColumnName("GroupID");
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
        });

        modelBuilder.Entity<EmployeeHobby>(entity =>
        {
            entity.HasKey(e => e.Hid)
                .HasName("PK__Employee__C75515271CF8445B")
                .HasFillFactor(90);

            entity.Property(e => e.Hid).HasColumnName("HID");
            entity.Property(e => e.EntryDate).HasColumnType("datetime");
            entity.Property(e => e.HobbieId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("HobbieID");
        });

        modelBuilder.Entity<EmpReward>(entity =>
        {
            entity.HasKey(e => e.RewardId).HasName("PK__EMP_Rewa__8250159989F51541");

            entity.ToTable("EMP_Reward");

            entity.Property(e => e.RewardId).HasColumnName("RewardID");
            entity.Property(e => e.AchievementId).HasColumnName("AchievementID");
            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.EmpId).HasColumnName("Emp_Id");
            entity.Property(e => e.Reason)
                .HasMaxLength(2000)
                .IsUnicode(false);
            entity.Property(e => e.RewardDate)
                .HasColumnType("datetime")
                .HasColumnName("Reward_Date");
            entity.Property(e => e.Status)
                .HasMaxLength(10)
                .IsUnicode(false);
        });

        modelBuilder.Entity<AchievementMaster>(entity =>
        {
            entity.HasKey(e => e.AchievementId).HasName("PK__Achievem__276330E0B9E85B8D");

            entity.ToTable("AchievementMaster");

            entity.Property(e => e.AchievementId).HasColumnName("AchievementID");
            entity.Property(e => e.Code)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.EntryDate).HasColumnType("datetime");
            entity.Property(e => e.UpdatedBy).HasColumnName("Updated_By");
            entity.Property(e => e.UpdatedDate)
                .HasColumnType("datetime")
                .HasColumnName("Updated_Date");
        });

        modelBuilder.Entity<QualificationRequestWorkFlowstatus>(entity =>
        {
            entity.HasKey(e => e.FlowId).HasName("PK__Qualific__1184B35C458A60C1");

            entity.ToTable("QualificationRequestWorkFlowstatus");

            entity.Property(e => e.FlowId).HasColumnName("FlowID");
            entity.Property(e => e.ApprovalRemarks)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.ApprovalStatus)
                .HasMaxLength(5)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Deligate)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.EntryBy).HasColumnName("Entry_By");
            entity.Property(e => e.EntryDt)
                .HasColumnType("datetime")
                .HasColumnName("Entry_Dt");
            entity.Property(e => e.EntryFrom)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedBy).HasColumnName("Updated_By");
            entity.Property(e => e.UpdatedDt)
                .HasColumnType("datetime")
                .HasColumnName("Updated_Dt");
            entity.Property(e => e.UpdatedFrom)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<HrEmpQualificationApprl>(entity =>
        {
            entity.HasKey(e => e.QlfId).HasName("PK__HR_EMP_Q__06E26F5095C529AB");

            entity.ToTable("HR_EMP_QUALIFICATION_APPRL");

            entity.Property(e => e.QlfId).HasColumnName("Qlf_id");
            entity.Property(e => e.Class)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Course)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.DateFrom).HasColumnType("datetime");
            entity.Property(e => e.DurFrm)
                .HasColumnType("datetime")
                .HasColumnName("Dur_Frm");
            entity.Property(e => e.DurTo)
                .HasColumnType("datetime")
                .HasColumnName("Dur_To");
            entity.Property(e => e.EmpId).HasColumnName("Emp_Id");
            entity.Property(e => e.EntryBy).HasColumnName("Entry_By");
            entity.Property(e => e.EntryDt)
                .HasColumnType("datetime")
                .HasColumnName("Entry_Dt");
            entity.Property(e => e.FlowStatus)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.InstId).HasColumnName("Inst_Id");
            entity.Property(e => e.InstName)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("Inst_Name");
            entity.Property(e => e.MarkPer)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("Mark_Per");
            entity.Property(e => e.MasterId)
                .HasDefaultValue(0)
                .HasColumnName("MasterID");
            entity.Property(e => e.RequestId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Request_ID");
            entity.Property(e => e.Status)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Subjects)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.University)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.YearPass)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("Year_Pass");
        });

        modelBuilder.Entity<HrEmpQualification>(entity =>
        {
            entity.HasKey(e => e.QlfId).HasName("PK__HR_EMP_Q__06E26F50A00790CE");

            entity.ToTable("HR_EMP_QUALIFICATION");

            entity.Property(e => e.QlfId).HasColumnName("Qlf_id");
            entity.Property(e => e.ApprlId).HasColumnName("ApprlID");
            entity.Property(e => e.Class)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Course)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.DurFrm)
                .HasColumnType("datetime")
                .HasColumnName("Dur_Frm");
            entity.Property(e => e.DurTo)
                .HasColumnType("datetime")
                .HasColumnName("Dur_To");
            entity.Property(e => e.EmpId).HasColumnName("Emp_Id");
            entity.Property(e => e.EntryBy).HasColumnName("Entry_By");
            entity.Property(e => e.EntryDt)
                .HasColumnType("datetime")
                .HasColumnName("Entry_Dt");
            entity.Property(e => e.InstId).HasColumnName("Inst_Id");
            entity.Property(e => e.InstName)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("Inst_Name");
            entity.Property(e => e.MarkPer)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("Mark_Per");
            entity.Property(e => e.Subjects)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.University)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.YearPass)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("Year_Pass");
        });
        modelBuilder.Entity<QualificationAttachment>(entity =>
        {
            entity.HasKey(e => e.QualAttachId);

            entity.ToTable("QualificationAttachment");

            entity.Property(e => e.DocStatus)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.EntryDate).HasColumnType("datetime");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<HrEmpTechnicalApprl>(entity =>
        {
            entity.HasKey(e => e.TechId).HasName("PK__HR_EMP_T__F90BF03BEAF77512");

            entity.ToTable("HR_EMP_TECHNICAL_APPRL");

            entity.Property(e => e.TechId).HasColumnName("Tech_id");
            entity.Property(e => e.Course)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.CourseDtls)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("Course_Dtls");
            entity.Property(e => e.DateFrom).HasColumnType("datetime");
            entity.Property(e => e.DurFrm)
                .HasColumnType("datetime")
                .HasColumnName("Dur_Frm");
            entity.Property(e => e.DurTo)
                .HasColumnType("datetime")
                .HasColumnName("Dur_To");
            entity.Property(e => e.EmpId).HasColumnName("Emp_Id");
            entity.Property(e => e.EntryBy).HasColumnName("Entry_By");
            entity.Property(e => e.EntryDt).HasColumnType("datetime");
            entity.Property(e => e.FlowStatus)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.InstId).HasColumnName("Inst_Id");
            entity.Property(e => e.InstName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Inst_Name");
            entity.Property(e => e.LangSkills)
                .HasMaxLength(5000)
                .IsUnicode(false)
                .HasColumnName("langSkills");
            entity.Property(e => e.MarkPer)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("Mark_Per");
            entity.Property(e => e.MasterId)
                .HasDefaultValue(0)
                .HasColumnName("MasterID");
            entity.Property(e => e.RequestId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Request_ID");
            entity.Property(e => e.Status)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Year)
                .HasMaxLength(10)
                .IsUnicode(false);
        });

        modelBuilder.Entity<SkillSetRequestWorkFlowstatus>(entity =>
        {
            entity.HasKey(e => e.FlowId).HasName("PK__SkillSet__1184B35C9D259644");

            entity.ToTable("SkillSetRequestWorkFlowstatus");

            entity.Property(e => e.FlowId).HasColumnName("FlowID");
            entity.Property(e => e.ApprovalRemarks)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.ApprovalStatus)
                .HasMaxLength(5)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Deligate)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.EntryBy).HasColumnName("Entry_By");
            entity.Property(e => e.EntryDt)
                .HasColumnType("datetime")
                .HasColumnName("Entry_Dt");
            entity.Property(e => e.EntryFrom)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedBy).HasColumnName("Updated_By");
            entity.Property(e => e.UpdatedDt)
                .HasColumnType("datetime")
                .HasColumnName("Updated_Dt");
            entity.Property(e => e.UpdatedFrom)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<HrEmpTechnical>(entity =>
{
    entity.HasKey(e => e.TechId).HasName("PK__HR_EMP_T__F90BF03B205D1992");

    entity.ToTable("HR_EMP_TECHNICAL");

    entity.Property(e => e.TechId).HasColumnName("Tech_id");
    entity.Property(e => e.ApprlId).HasColumnName("ApprlID");
    entity.Property(e => e.Course)
        .HasMaxLength(500)
        .IsUnicode(false);
    entity.Property(e => e.CourseDtls)
        .HasMaxLength(500)
        .IsUnicode(false)
        .HasColumnName("Course_Dtls");
    entity.Property(e => e.DurFrm)
        .HasColumnType("datetime")
        .HasColumnName("Dur_Frm");
    entity.Property(e => e.DurTo)
        .HasColumnType("datetime")
        .HasColumnName("Dur_To");
    entity.Property(e => e.EmpId).HasColumnName("Emp_Id");
    entity.Property(e => e.EntryBy).HasColumnName("Entry_By");
    entity.Property(e => e.EntryDt).HasColumnType("datetime");
    entity.Property(e => e.InstId).HasColumnName("Inst_Id");
    entity.Property(e => e.InstName)
        .HasMaxLength(100)
        .IsUnicode(false)
        .HasColumnName("Inst_Name");
    entity.Property(e => e.LangSkills)
        .HasMaxLength(5000)
        .IsUnicode(false)
        .HasColumnName("langSkills");
    entity.Property(e => e.MarkPer)
        .HasMaxLength(20)
        .IsUnicode(false)
        .HasColumnName("Mark_Per");
    entity.Property(e => e.Year)
        .HasMaxLength(10)
        .IsUnicode(false);
});

        modelBuilder.Entity<ReasonMasterFieldValue>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("ReasonMasterFieldValue");

            entity.Property(e => e.CategoryFieldId).HasColumnName("CategoryFieldID");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.FieldValues).IsUnicode(false);
            entity.Property(e => e.ReasonFieldId)
                .ValueGeneratedOnAdd()
                .HasColumnName("ReasonFieldID");
            entity.Property(e => e.ReasonId).HasColumnName("Reason_Id");
        });


        modelBuilder.Entity<GeneralCategoryField>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("GeneralCategoryField");

            entity.Property(e => e.CategoryFieldId)
                .ValueGeneratedOnAdd()
                .HasColumnName("CategoryFieldID");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.DataTypeId).HasColumnName("DataTypeID");
            entity.Property(e => e.FieldDescription).IsUnicode(false);
            entity.Property(e => e.GeneralCategoryId).HasColumnName("GeneralCategoryID");
        });

        modelBuilder.Entity<HrmsDatatype>(entity =>
        {
            entity.HasKey(e => e.TypeId);

            entity.ToTable("HRMS_Datatype");

            entity.Property(e => e.DataType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.DataTypeId).HasColumnName("DataTypeID");
        });

        modelBuilder.Entity<HrmsDocTypeMaster>(entity =>
        {
            entity.HasKey(e => e.DocTypeId);

            entity.ToTable("HRMS_DocTypeMaster");

            entity.Property(e => e.Code)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.DocType)
                .HasMaxLength(50)
                .IsUnicode(false);
        });
        modelBuilder.Entity<HrmsEmpdocuments00>(entity =>
        {
            entity.HasKey(e => e.DetailId).HasName("PK__HRMS_EMP__135C314D1CDBE34B");

            entity.ToTable("HRMS_EMPDocuments00");

            entity.Property(e => e.DetailId).HasColumnName("DetailID");
            entity.Property(e => e.DateFrom).HasColumnType("datetime");
            entity.Property(e => e.DocId).HasColumnName("DocID");
            entity.Property(e => e.EmpId).HasColumnName("EmpID");
            entity.Property(e => e.EntryDate).HasColumnType("datetime");
            entity.Property(e => e.FinalApprovalDate).HasColumnType("datetime");
            entity.Property(e => e.FlowStatus)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.ProxyId)
                .HasDefaultValue(0)
                .HasColumnName("ProxyID");
            entity.Property(e => e.RequestId)
                .IsUnicode(false)
                .HasColumnName("RequestID");
            entity.Property(e => e.Status)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.TransactionType)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<HrmsEmpdocuments01>(entity =>
        {
            entity.HasKey(e => e.DocFieldId).HasName("PK__HRMS_EMP__E2BF7E5056FB8466");

            entity.ToTable("HRMS_EMPDocuments01");

            entity.Property(e => e.DocFieldId).HasColumnName("DocFieldID");
            entity.Property(e => e.DetailId).HasColumnName("DetailID");
            entity.Property(e => e.DocValues)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<EmpDocumentAccess>(entity =>
        {
            entity.HasKey(e => e.DocAccessId).HasName("PK__EMP_Docu__589C76DEA8F6D9D2");

            entity.ToTable("EMP_DocumentAccess");

            entity.Property(e => e.DocAccessId).HasColumnName("DocAccessID");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.DocId).HasColumnName("DocID");
            entity.Property(e => e.EmpId).HasColumnName("EmpID");
            entity.Property(e => e.InstId).HasColumnName("Inst_Id");
            entity.Property(e => e.ValidFrom).HasColumnType("datetime");
            entity.Property(e => e.ValidTo).HasColumnType("datetime");
        });

        modelBuilder.Entity<HrmsEmpdocuments02>(entity =>
        {
            entity.HasKey(e => e.DocId).HasName("PK__HRMS_EMP__3EF1888DDACE7E32");

            entity.ToTable("HRMS_EMPDocuments02");

            entity.Property(e => e.DocId).HasColumnName("DocID");
            entity.Property(e => e.DetailId).HasColumnName("DetailID");
            entity.Property(e => e.FileName)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.FileType)
                .HasMaxLength(500)
                .IsUnicode(false);
        });

        modelBuilder.Entity<HrmsEmpdocumentsApproved02>(entity =>
        {
            entity.HasKey(e => e.DocId).HasName("PK__HRMS_EMP__3EF1888D86F5E4CA");

            entity.ToTable("HRMS_EMPDocumentsApproved02");

            entity.Property(e => e.DocId).HasColumnName("DocID");
            entity.Property(e => e.DetailId).HasColumnName("DetailID");
            entity.Property(e => e.FileName)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.FileType)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(5)
                .IsUnicode(false);
        });
        modelBuilder.Entity<AssignedLetterType>(entity =>
        {
            entity.HasKey(e => e.LetterReqId);

            entity.ToTable("AssignedLetterType");

            entity.Property(e => e.LetterReqId).HasColumnName("LetterReqID");
            entity.Property(e => e.ApprovalStatus)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ApproverRemark).IsUnicode(false);
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.EmpId).HasColumnName("EmpID");
            entity.Property(e => e.FileType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FinalApprovalDate).HasColumnType("datetime");
            entity.Property(e => e.FinalApproverRemark).IsUnicode(false);
            entity.Property(e => e.FlowStatus)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.IsLetterAttached).HasDefaultValue(false);
            entity.Property(e => e.IssueDate).HasColumnType("datetime");
            entity.Property(e => e.LetterTypeId).HasColumnName("LetterTypeID");
            entity.Property(e => e.LiabilityLetter).IsUnicode(false);
            entity.Property(e => e.LiabilityReqId).HasColumnName("LiabilityReqID");
            entity.Property(e => e.ModifiedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ReleaseDate)
                .HasColumnType("datetime")
                .HasColumnName("Release_Date");
            entity.Property(e => e.Remark)
                .HasMaxLength(2000)
                .IsUnicode(false);
            entity.Property(e => e.RequestCode)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.TemplateStyle).IsUnicode(false);
            entity.Property(e => e.UploadFileName)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.Uploaded).HasColumnName("uploaded");
            entity.Property(e => e.ValidFrom)
                .HasMaxLength(250)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Dependent00>(entity =>
        {
            entity.HasKey(e => e.DepId);

            entity.ToTable("Dependent00");

            entity.Property(e => e.DateOfBirth).HasColumnType("datetime");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.DocumentId).HasColumnName("DocumentID");
            entity.Property(e => e.EntryDate).HasColumnType("datetime");
            entity.Property(e => e.Gender)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.IdentificationNo)
                .HasMaxLength(5000)
                .IsUnicode(false);
            entity.Property(e => e.InterEmpId).HasColumnName("InterEmpID");
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.Name)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Type)
                .HasMaxLength(10)
                .IsUnicode(false);
        });

        modelBuilder.Entity<DependentMaster>(entity =>
        {
            entity.HasKey(e => e.DependentId);

            entity.ToTable("DependentMaster");

            entity.Property(e => e.Code)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Description)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.EntryDate).HasColumnType("datetime");
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.Self).HasDefaultValue(0);
        });

        modelBuilder.Entity<LetterMaster00>(entity =>
        {
            entity.HasKey(e => e.LetterTypeId);

            entity.ToTable("LetterMaster00");

            entity.Property(e => e.LetterTypeId).HasColumnName("LetterTypeID");
            entity.Property(e => e.Active).HasDefaultValue(true);
            entity.Property(e => e.LetterCode)
                .HasMaxLength(80)
                .IsUnicode(false);
            entity.Property(e => e.LetterTypeName)
                .HasMaxLength(150)
                .IsUnicode(false);
        });

        modelBuilder.Entity<LetterMaster01>(entity =>
        {
            entity.HasKey(e => e.ModuleSubId).HasName("PK_LetterSubType");

            entity.ToTable("LetterMaster01");

            entity.Property(e => e.ModuleSubId).HasColumnName("ModuleSubID");
            entity.Property(e => e.AppointmentLetter).HasDefaultValue(0);
            entity.Property(e => e.ApproveText)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.BackgroundImage).IsUnicode(false);
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.IsEss).HasDefaultValue(false);
            entity.Property(e => e.LetterSubName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.LetterTypeId).HasColumnName("LetterTypeID");
            entity.Property(e => e.ModifiedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.RejectText)
                .HasMaxLength(250)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TaxDeclarationFileUpload>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("TaxDeclarationFileUpload");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.EmpId).HasColumnName("EmpID");
            entity.Property(e => e.FileType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FlowStatus)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.InvstmntFileId).ValueGeneratedOnAdd();
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.Remark)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.RequestCode)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.UploadFileName).IsUnicode(false);
        });
        modelBuilder.Entity<EmployeeCertification>(entity =>
        {
            entity.HasKey(e => e.CertificationId).HasName("PK__Employee__1237E5AABC7875EE");

            entity.Property(e => e.CertificationId).HasColumnName("CertificationID");
            entity.Property(e => e.EmpId).HasColumnName("Emp_Id");
            entity.Property(e => e.EntryDate).HasColumnType("datetime");
            entity.Property(e => e.Status)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });
        modelBuilder.Entity<CurrencyMaster>(entity =>
        {
            entity.HasKey(e => e.CurrencyId);

            entity.ToTable("Currency_Master");

            entity.Property(e => e.CurrencyId).HasColumnName("Currency_Id");
            entity.Property(e => e.CountryId).HasColumnName("Country_Id");
            entity.Property(e => e.Currency)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CurrencyCode)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Currency_Code");
            entity.Property(e => e.DecimalValue).HasDefaultValue(0);
            entity.Property(e => e.EntryDate).HasColumnType("datetime");
            entity.Property(e => e.IsActive)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("isActive");
            entity.Property(e => e.Symbol)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<EmployeeCertification>(entity =>
        {
            entity.HasKey(e => e.CertificationId).HasName("PK__Employee__1237E5AABC7875EE");

            entity.Property(e => e.CertificationId).HasColumnName("CertificationID");
            entity.Property(e => e.EmpId).HasColumnName("Emp_Id");
            entity.Property(e => e.EntryDate).HasColumnType("datetime");
            entity.Property(e => e.Status)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<HrEmpProfdtl>(entity =>
        {
            entity.HasKey(e => e.ProfId).HasName("PK__HR_EMP_P__A46610E521D23F48");

            entity.ToTable("HR_EMP_PROFDTLS");

            entity.Property(e => e.ProfId).HasColumnName("Prof_Id");
            entity.Property(e => e.ApprlId).HasColumnName("ApprlID");
            entity.Property(e => e.CompAddress)
                .IsUnicode(false)
                .HasColumnName("Comp_Address");
            entity.Property(e => e.CompName)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("Comp_Name");
            entity.Property(e => e.CompSite)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("Comp_Site");
            entity.Property(e => e.ContactNo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Contact_No");
            entity.Property(e => e.ContactPer)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("Contact_Per");
            entity.Property(e => e.Ctc)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.CurrencyId).HasColumnName("CurrencyID");
            entity.Property(e => e.Designation)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.EmpId).HasColumnName("Emp_Id");
            entity.Property(e => e.EntryBy).HasColumnName("Entry_By");
            entity.Property(e => e.EntryDt)
                .HasColumnType("datetime")
                .HasColumnName("Entry_Dt");
            entity.Property(e => e.InstId).HasColumnName("Inst_Id");
            entity.Property(e => e.JobDesc)
                .IsUnicode(false)
                .HasColumnName("Job_Desc");
            entity.Property(e => e.JoinDt)
                .HasColumnType("datetime")
                .HasColumnName("Join_Dt");
            entity.Property(e => e.LeaveReason)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("Leave_Reason");
            entity.Property(e => e.LeavingDt)
                .HasColumnType("datetime")
                .HasColumnName("Leaving_Dt");
            entity.Property(e => e.Pbno)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("PBno");
        });

        modelBuilder.Entity<HrEmpProfdtlsApprl>(entity =>
        {
            entity.HasKey(e => e.ProfId).HasName("PK__HR_EMP_P__A46610E59FDAC84A");

            entity.ToTable("HR_EMP_PROFDTLS_APPRL");

            entity.Property(e => e.ProfId).HasColumnName("Prof_Id");
            entity.Property(e => e.CompAddress)
                .IsUnicode(false)
                .HasColumnName("Comp_Address");
            entity.Property(e => e.CompName)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("Comp_Name");
            entity.Property(e => e.CompSite)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("Comp_Site");
            entity.Property(e => e.ContactNo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Contact_No");
            entity.Property(e => e.ContactPer)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("Contact_Per");
            entity.Property(e => e.Ctc)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.CurrencyId)
                .HasDefaultValue(0)
                .HasColumnName("CurrencyID");
            entity.Property(e => e.DateFrom).HasColumnType("datetime");
            entity.Property(e => e.Designation)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.EmpId).HasColumnName("Emp_Id");
            entity.Property(e => e.EntryBy).HasColumnName("Entry_By");
            entity.Property(e => e.EntryDt)
                .HasColumnType("datetime")
                .HasColumnName("Entry_Dt");
            entity.Property(e => e.FlowStatus)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.InstId).HasColumnName("Inst_Id");
            entity.Property(e => e.JobDesc)
                .IsUnicode(false)
                .HasColumnName("Job_Desc");
            entity.Property(e => e.JoinDt)
                .HasColumnType("datetime")
                .HasColumnName("Join_Dt");
            entity.Property(e => e.LeaveReason)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("Leave_Reason");
            entity.Property(e => e.LeavingDt)
                .HasColumnType("datetime")
                .HasColumnName("Leaving_Dt");
            entity.Property(e => e.MasterId)
                .HasDefaultValue(0)
                .HasColumnName("MasterID");
            entity.Property(e => e.Pbno)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("PBno");
            entity.Property(e => e.RequestId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Request_ID");
            entity.Property(e => e.Status)
                .HasMaxLength(5)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ProfessionalRequestWorkFlowstatus>(entity =>
        {
            entity.HasKey(e => e.FlowId).HasName("PK__Professi__1184B35C7549EE3E");

            entity.ToTable("ProfessionalRequestWorkFlowstatus");

            entity.Property(e => e.FlowId).HasColumnName("FlowID");
            entity.Property(e => e.ApprovalRemarks)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.ApprovalStatus)
                .HasMaxLength(5)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Deligate)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.EntryBy).HasColumnName("Entry_By");
            entity.Property(e => e.EntryDt)
                .HasColumnType("datetime")
                .HasColumnName("Entry_Dt");
            entity.Property(e => e.EntryFrom)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedBy).HasColumnName("Updated_By");
            entity.Property(e => e.UpdatedDt)
                .HasColumnType("datetime")
                .HasColumnName("Updated_Dt");
            entity.Property(e => e.UpdatedFrom)
                .HasMaxLength(20)
                .IsUnicode(false);
        });
        modelBuilder.Entity<ConsultantDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Consulta__3214EC0738201DD0");

            entity.Property(e => e.Address)
                .HasMaxLength(5000)
                .IsUnicode(false);
            entity.Property(e => e.ConsultantName)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.MobileNumber)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.PhoneNo)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<HrEmpreference>(entity =>
        {
            entity.HasKey(e => e.RefId).HasName("PK__HR_EMPRe__2D2A2CD105D41E1A");

            entity.ToTable("HR_EMPReference");

            entity.Property(e => e.RefId).HasColumnName("RefID");
            entity.Property(e => e.Address)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.ConsultantId).HasColumnName("ConsultantID");
            entity.Property(e => e.EmpId).HasColumnName("EmpID");
            entity.Property(e => e.EntryDate).HasColumnType("datetime");
            entity.Property(e => e.PhoneNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.RefEmpId).HasColumnName("RefEmpID");
            entity.Property(e => e.RefMethod)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.RefName)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.RefType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<EmployeesAssetsAssign>(entity =>
        {
            entity.HasKey(e => e.AssignId).HasName("PK__Employee__9FFF4C4F30953A37");

            entity.ToTable("EmployeesAssetsAssign");

            entity.Property(e => e.AssignId).HasColumnName("AssignID");
            entity.Property(e => e.Asset)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.AssetGroup)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.AssetModel)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.AssetNo)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.AssetRequestId).HasColumnName("AssetRequestID");
            entity.Property(e => e.AssetValue)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.EmpId).HasColumnName("Emp_Id");
            entity.Property(e => e.EntryDate).HasColumnType("datetime");
            entity.Property(e => e.ExpiryDate).HasColumnType("datetime");
            entity.Property(e => e.InWarranty).HasColumnType("datetime");
            entity.Property(e => e.IssueDate).HasColumnType("datetime");
            entity.Property(e => e.Monitor)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.OutOfWarranty).HasColumnType("datetime");
            entity.Property(e => e.ReceivedDate).HasColumnType("datetime");
            entity.Property(e => e.Remarks).IsUnicode(false);
            entity.Property(e => e.ReturnDate).HasColumnType("datetime");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<HrmsCommonField01>(entity =>
        {
            entity.HasKey(e => e.CommonFieldId);

            entity.ToTable("HRMS_CommonField01");

            entity.Property(e => e.CommonFieldId).HasColumnName("CommonFieldID");
            entity.Property(e => e.CommonVal)
                .HasMaxLength(150)
                .IsUnicode(false);
        });

        modelBuilder.Entity<HrmsCommonMaster00>(entity =>
        {
            entity.HasKey(e => e.ComMasId);

            entity.ToTable("HRMS_CommonMaster00");

            entity.Property(e => e.ComMasId).HasColumnName("ComMasID");
            entity.Property(e => e.AssetType)
                .HasMaxLength(180)
                .IsUnicode(false);
            entity.Property(e => e.SepActive).HasDefaultValue(true);
            entity.Property(e => e.Status)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<AssignedAsset>(entity =>
        {
            entity.HasKey(e => e.AssignId);

            entity.ToTable("AssignedAsset");

            entity.Property(e => e.AssignId).HasColumnName("AssignID");
            entity.Property(e => e.Active).HasDefaultValue(true);
            entity.Property(e => e.Assest)
                .HasMaxLength(190)
                .IsUnicode(false);
            entity.Property(e => e.AssestId)
                .HasMaxLength(80)
                .IsUnicode(false)
                .HasColumnName("AssestID");
            entity.Property(e => e.AssestRequestId)
                .HasMaxLength(180)
                .IsUnicode(false)
                .HasColumnName("AssestRequestID");
            entity.Property(e => e.AssestType)
                .HasMaxLength(80)
                .IsUnicode(false);
            entity.Property(e => e.AssestTypeId)
                .HasMaxLength(180)
                .IsUnicode(false)
                .HasColumnName("AssestTypeID");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.EmpId).HasColumnName("EmpID");
            entity.Property(e => e.Employee)
                .HasMaxLength(190)
                .IsUnicode(false);
            entity.Property(e => e.IssuedDate)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.ProxyRemark)
                .HasMaxLength(280)
                .IsUnicode(false);
            entity.Property(e => e.RequestRemark)
                .HasMaxLength(280)
                .IsUnicode(false);
            entity.Property(e => e.ReturnedDate).HasColumnType("datetime");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<HrEmployeeUserRelation>(entity =>
        {
            entity.HasKey(e => e.EmpUsrRelatnId).HasName("PK__HR_EMPLO__ACF32ECE384FCBF6");

            entity.ToTable("HR_EMPLOYEE_USER_RELATION");

            entity.Property(e => e.EmpUsrRelatnId).HasColumnName("Emp_Usr_Relatn_Id");
            entity.Property(e => e.EmpId).HasColumnName("Emp_Id");
            entity.Property(e => e.EntryBy).HasColumnName("Entry_By");
            entity.Property(e => e.EntryDt)
                .HasColumnType("datetime")
                .HasColumnName("Entry_Dt");
            entity.Property(e => e.InstId).HasColumnName("inst_Id");
        });

        modelBuilder.Entity<CommonField>(entity =>
        {
            entity.HasKey(e => e.ComFieldId);

            entity.ToTable("CommonField");

            entity.Property(e => e.ComFieldId).HasColumnName("ComFieldID");
            entity.Property(e => e.ComDataTypeId).HasColumnName("ComDataTypeID");
            entity.Property(e => e.ComDescription)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.ComId).HasColumnName("ComID");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.IsMandatoryField).HasDefaultValue(true);
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
        });


        modelBuilder.Entity<PersonalDetailsHistory>(entity =>
        {
            entity.HasKey(e => e.EmployeeId);
            entity
                .ToTable("PersonalDetailsHistory");

            entity.Property(e => e.BloodGroup)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.DateOfBirth).HasColumnType("datetime");
            entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");
            entity.Property(e => e.EntryDate).HasColumnType("datetime");
            entity.Property(e => e.Gender)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.GuardiansName)
                .HasMaxLength(5000)
                .IsUnicode(false);
            entity.Property(e => e.Height)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.IdentificationMark)
                .HasMaxLength(5000)
                .IsUnicode(false);
            entity.Property(e => e.MaritalStatus)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.PersonalMail)
                .HasMaxLength(5000)
                .IsUnicode(false);
            entity.Property(e => e.WeddingDate)
                .HasColumnType("datetime")
                .HasColumnName("Wedding_Date");
            entity.Property(e => e.Weight)
                .HasMaxLength(500)
                .IsUnicode(false);
        });
        modelBuilder.Entity<AdmCodegenerationmaster>(entity =>
        {
            entity.HasKey(e => e.CodeId).HasName("PK__ADM_CODE__CEB3CDC2DCBEC6F8");

            entity.ToTable("ADM_CODEGENERATIONMASTER");

            entity.Property(e => e.CodeId).HasColumnName("Code_ID");
            entity.Property(e => e.Code)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FinalCodeWithNoSeq)
                .HasMaxLength(5000)
                .IsUnicode(false);
            entity.Property(e => e.InstId).HasColumnName("Inst_id");
            entity.Property(e => e.IsDelete).HasDefaultValue(0);
            entity.Property(e => e.LastSequence)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.MnodifiedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("MNodifiedDate");
            entity.Property(e => e.NumberFormat)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.PrefixFormatId).HasColumnName("PrefixFormatID");
            entity.Property(e => e.Suffix)
                .HasMaxLength(5000)
                .IsUnicode(false);
            entity.Property(e => e.SuffixFormatId).HasColumnName("SuffixFormatID");
            entity.Property(e => e.TypeId).HasColumnName("TypeID");
        });

        modelBuilder.Entity<EntityApplicable00>(entity =>
        {
            entity.HasKey(e => e.ApplicableId);

            entity.ToTable("EntityApplicable00");

            entity.Property(e => e.EntryDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<LevelSettingsAccess00>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("LevelSettingsAccess00");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.InstId).HasColumnName("Inst_Id");
            entity.Property(e => e.Levels)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TransactionId).HasColumnName("TransactionID");
        });

        modelBuilder.Entity<TransactionMaster>(entity =>
        {
            entity.HasKey(e => e.TransactionId).HasName("PK__Transact__55433A4B7BC0D39D");

            entity.ToTable("TransactionMaster");

            entity.Property(e => e.TransactionId).HasColumnName("TransactionID");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.InstId).HasColumnName("Inst_Id");
            entity.Property(e => e.IsRequest).HasDefaultValue(0);
            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.TransactionType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Transactionbit).HasColumnName("transactionbit");
            entity.Property(e => e.TrxParam).HasDefaultValue(0);
        });

        modelBuilder.Entity<TrainingSchedule>(entity =>
        {
            entity.HasKey(e => e.TrSchd);

            entity.ToTable("TrainingSchedule");

            entity.Property(e => e.TrSchd).HasColumnName("trSchd");
            entity.Property(e => e.AttDate).HasColumnType("datetime");
            entity.Property(e => e.EntryDate).HasColumnType("datetime");
            entity.Property(e => e.SelectStatus)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("selectStatus");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("status");
            entity.Property(e => e.TrMasterId).HasColumnName("trMasterId");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<TrainingMaster>(entity =>
        {
            entity.HasKey(e => e.TrMasterId);

            entity.ToTable("TRAINING_MASTER");

            entity.Property(e => e.TrMasterId).HasColumnName("trMasterId");
            entity.Property(e => e.Active)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("active");
            entity.Property(e => e.Description)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.EntryBy).HasColumnName("Entry_by");
            entity.Property(e => e.EntryDate)
                .HasColumnType("datetime")
                .HasColumnName("Entry_date");
            entity.Property(e => e.FileUrl)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("fileUrl");
            entity.Property(e => e.FromDate).HasColumnType("datetime");
            entity.Property(e => e.IsInside).HasColumnName("isInside");
            entity.Property(e => e.Survey)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TargetPeople)
                .HasMaxLength(1500)
                .IsUnicode(false);
            entity.Property(e => e.ToDate).HasColumnType("datetime");
            entity.Property(e => e.TrCode)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("trCode");
            entity.Property(e => e.TrName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("trName");
            entity.Property(e => e.TrainerName)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.TrainingLocation)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });


        modelBuilder.Entity<TrainingMaster01>(entity =>
        {
            entity.HasKey(e => e.FileUpdId);

            entity.ToTable("TrainingMaster01");

            entity.Property(e => e.FileName).IsUnicode(false);
            entity.Property(e => e.FileType).IsUnicode(false);
            entity.Property(e => e.FileUrl).IsUnicode(false);
            entity.Property(e => e.TrMasterId).HasColumnName("trMasterId");
        });

        modelBuilder.Entity<BiometricsDtl>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__BIOMETRI__3213E83FE98A02B7");

            entity.ToTable("BIOMETRICS_DTL");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CompanyId).HasColumnName("CompanyID");
            entity.Property(e => e.DeviceId).HasColumnName("DeviceID");
            entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");
            entity.Property(e => e.EntryDt).HasColumnType("datetime");
            entity.Property(e => e.UserId)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("UserID");
        });
        modelBuilder.Entity<EditInfoHistory>(entity =>
        {
            entity.HasKey(e => e.InfoHistoryId).HasName("PK__EditInfo__C879104E2B85B6AB");

            entity.ToTable("EditInfoHistory");

            entity.Property(e => e.InfoHistoryId).HasColumnName("InfoHistoryID");
            entity.Property(e => e.EmpId).HasColumnName("EmpID");
            entity.Property(e => e.Info01Id).HasColumnName("Info01ID");
            entity.Property(e => e.InfoCode)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.InfoId).HasColumnName("InfoID");
            entity.Property(e => e.OldValue)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedDate)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Value)
                .HasMaxLength(500)
                .IsUnicode(false);
        });

        modelBuilder.Entity<EditInfoMaster01>(entity =>
        {
            entity.HasKey(e => e.Info01Id).HasName("PK__EditInfo__2D3D5212431B0514");

            entity.ToTable("EditInfoMaster01");

            entity.Property(e => e.Info01Id).HasColumnName("Info01ID");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.EntryDate)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.InfoCode)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.InfoId).HasColumnName("InfoID");
            entity.Property(e => e.TableColumn)
                .HasMaxLength(800)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedDate)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<HrShift00>(entity =>
        {
            entity.HasKey(e => e.ShiftId).HasName("PK__HR_SHIFT__C0A838E123AB493F");

            entity.ToTable("HR_SHIFT00");

            entity.Property(e => e.ShiftId).HasColumnName("ShiftID");
            entity.Property(e => e.CompanyId).HasColumnName("CompanyID");
            entity.Property(e => e.EndwithNextDay)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.EntryDate).HasColumnType("datetime");
            entity.Property(e => e.IsUpload)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.ShiftCode)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.ShiftName)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.ShiftType)
                .HasMaxLength(500)
                .IsUnicode(false);
        });

        modelBuilder.Entity<HrmHolidayMasterAccess>(entity =>
        {
            entity.HasKey(e => e.IdHolidayMasterAccess).HasName("PK__HRM_HOLI__1A80B3CF9CC8C054");

            entity.ToTable("HRM_HOLIDAY_MASTER_ACCESS");

            entity.Property(e => e.IdHolidayMasterAccess).HasColumnName("Id_HolidayMasterAccess");
            entity.Property(e => e.Active)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.HolidayMasterId).HasColumnName("HolidayMaster_id");
            entity.Property(e => e.ValidDateTo).HasColumnType("datetime");
            entity.Property(e => e.ValidDatefrom).HasColumnType("datetime");
        });

        modelBuilder.Entity<AttendancepolicyMasterAccess>(entity =>
        {
            entity.HasKey(e => e.AttendanceAccessId).HasName("PK__ATTENDAN__C979F80AA42FACFF");

            entity.ToTable("ATTENDANCEPOLICY_MASTER_ACCESS");

            entity.Property(e => e.AttendanceAccessId).HasColumnName("AttendanceAccessID");
            entity.Property(e => e.Active)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");
            entity.Property(e => e.PolicyId).HasColumnName("PolicyID");
            entity.Property(e => e.ValidDateTo).HasColumnType("datetime");
            entity.Property(e => e.ValidDatefrom).HasColumnType("datetime");
        });

        modelBuilder.Entity<ShiftMasterAccess>(entity =>
        {
            entity.HasKey(e => e.ShiftAccessId).HasName("PK__SHIFT_MA__D343B905E36F048C");

            entity.ToTable("SHIFT_MASTER_ACCESS");

            entity.Property(e => e.ShiftAccessId).HasColumnName("ShiftAccessID");
            entity.Property(e => e.Active)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.ApprovalStatus)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");
            entity.Property(e => e.ProjectId).HasColumnName("ProjectID");
            entity.Property(e => e.ShiftApprovalId).HasColumnName("ShiftApprovalID");
            entity.Property(e => e.ShiftId).HasColumnName("ShiftID");
            entity.Property(e => e.ValidDateTo).HasColumnType("datetime");
            entity.Property(e => e.ValidDatefrom).HasColumnType("datetime");
            entity.Property(e => e.WeekEndMasterId).HasColumnName("WeekEndMasterID");
        });

        modelBuilder.Entity<HrmLeaveEmployeeleaveaccess>(entity =>
        {
            entity.HasKey(e => e.IdEmployeeLeaveAccess);

            entity.ToTable("HRM_LEAVE_EMPLOYEELEAVEACCESS");

            entity.Property(e => e.IdEmployeeLeaveAccess).HasColumnName("Id_EmployeeLeaveAccess");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FromDate).HasColumnType("datetime");
            entity.Property(e => e.ValidTo).HasColumnType("datetime");
        });

        modelBuilder.Entity<LeavepolicyMasterAccess>(entity =>
        {
            entity.HasKey(e => e.LeaveAccessId).HasName("PK__LEAVEPOL__A6D71DCE86B1D991");

            entity.ToTable("LEAVEPOLICY_MASTER_ACCESS");

            entity.Property(e => e.LeaveAccessId).HasColumnName("LeaveAccessID");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");
            entity.Property(e => e.Fromdate).HasColumnType("datetime");
            entity.Property(e => e.PolicyId).HasColumnName("PolicyID");
            entity.Property(e => e.Validto).HasColumnType("datetime");
        });

        modelBuilder.Entity<HrmLeaveBasicsettingsaccess>(entity =>
        {
            entity.HasKey(e => e.IdEmployeeSettinsAccess);

            entity.ToTable("HRM_LEAVE_BASICSETTINGSACCESS");

            entity.Property(e => e.IdEmployeeSettinsAccess).HasColumnName("Id_EmployeeSettinsAccess");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FromDateBs).HasColumnType("datetime");
            entity.Property(e => e.Laps).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ValidToBs).HasColumnType("datetime");
        });

        modelBuilder.Entity<ParamWorkFlow00>(entity =>
        {
            entity.HasKey(e => e.ValueId);

            entity.ToTable("ParamWorkFlow00");

            entity.Property(e => e.AdditionalRoleNotif).IsUnicode(false);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.EntityLevel).IsUnicode(false);
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.RoleNotification).HasDefaultValue(0);
            entity.Property(e => e.SecondLevelWorkflowId).HasDefaultValue(0);
        });

        modelBuilder.Entity<ParamWorkFlowEntityLevel00>(entity =>
        {
            entity.ToTable("ParamWorkFlowEntityLevel00");
        });

        modelBuilder.Entity<ParamWorkFlow02>(entity =>
        {
            entity.HasKey(e => e.ValueId);

            entity.ToTable("ParamWorkFlow02");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<ParamWorkFlow01>(entity =>
        {
            entity.HasKey(e => e.ValueId);

            entity.ToTable("ParamWorkFlow01");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<WorkFlowDetail>(entity =>
        {
            entity.HasKey(e => e.WorkFlowId);

            entity.Property(e => e.Description).IsUnicode(false);
            entity.Property(e => e.FinalRule).IsUnicode(false);
            entity.Property(e => e.FinalRuleName).IsUnicode(false);
            entity.Property(e => e.InstId).HasColumnName("Inst_Id");
            entity.Property(e => e.OldType).HasDefaultValue(0);
            entity.Property(e => e.ReqNotifForProxy).HasDefaultValue(0);
            entity.Property(e => e.TransactionDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<EntityApplicable01>(entity =>
        {
            entity.HasKey(e => e.ApplicableId);

            entity.ToTable("EntityApplicable01");

            entity.Property(e => e.EntryDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<HrEmpReportingHstry>(entity =>
        {
            entity.HasKey(e => e.ReportHistId).HasName("PK__HR_EMP_R__5FC67BAAF9C61086");

            entity.ToTable("HR_EMP_REPORTING_HSTRY");

            entity.Property(e => e.ReportHistId).HasColumnName("ReportHistID");
            entity.Property(e => e.EmpId).HasColumnName("EmpID");
            entity.Property(e => e.EntryDate).HasColumnType("datetime");
            entity.Property(e => e.FromDate).HasColumnType("datetime");
            entity.Property(e => e.InstId).HasColumnName("inst_id");
            entity.Property(e => e.ReportId).HasColumnName("ReportID");
            entity.Property(e => e.ResignationId).HasColumnName("ResignationID");
            entity.Property(e => e.ToDate).HasColumnType("datetime");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });
        modelBuilder.Entity<PositionHistory>(entity =>
        {
            entity.HasKey(e => e.PositionId);

            entity.ToTable("Position_History");

            entity.Property(e => e.PositionId).HasColumnName("Position_Id");
            entity.Property(e => e.BandId).HasColumnName("BandID");
            entity.Property(e => e.EmpId).HasColumnName("Emp_Id");
            entity.Property(e => e.EmpStatus).HasColumnName("Emp_status");
            entity.Property(e => e.FromDate)
                .HasColumnType("datetime")
                .HasColumnName("From_Date");
            entity.Property(e => e.OldEmpCode)
                .IsUnicode(false)
                .HasColumnName("OldEmp_code");
            entity.Property(e => e.Status)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.ToDate)
                .HasColumnType("datetime")
                .HasColumnName("To_Date");
            entity.Property(e => e.TransferBatchId).HasColumnName("TransferBatchID");
            entity.Property(e => e.TransferId).HasColumnName("TransferID");
        });
        modelBuilder.Entity<PayscaleRequest01>(entity =>
        {
            entity.HasKey(e => e.PayRequest01Id).HasName("PK__Payscale__5433D06BA421AAEF");

            entity.ToTable("PayscaleRequest01");

            entity.Property(e => e.EffectiveDate).HasColumnType("datetime");
            entity.Property(e => e.EmployeeStatus)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.OverrideId).HasColumnName("OverrideID");
            entity.Property(e => e.PayscaleSlab).HasColumnName("payscaleSlab");
            entity.Property(e => e.RevisionFrom).HasColumnType("datetime");
        });
        modelBuilder.Entity<PayscaleRequest02>(entity =>
        {
            entity.HasKey(e => e.PayRequestId02).HasName("PK__Payscale__2CEF8F36870E9E68");

            modelBuilder.Entity<PayscaleRequest00>(entity =>
            {
                entity.HasKey(e => e.PayRequestId);

                entity.ToTable("PayscaleRequest00");

                entity.Property(e => e.BatchStatus)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();
                entity.Property(e => e.EffectiveDate).HasColumnType("datetime");
                entity.Property(e => e.EmployeeIds).IsUnicode(false);
                entity.Property(e => e.EntryDate).HasColumnType("datetime");
                entity.Property(e => e.FlowStatus)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();
                entity.Property(e => e.PayReqCode)
                    .HasMaxLength(1000)
                    .IsUnicode(false);
                entity.Property(e => e.RejectReason)
                    .HasMaxLength(1000)
                    .IsUnicode(false);
                entity.Property(e => e.RejectStatus)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();
            });

            modelBuilder.Entity<ShiftMasterAccess>(entity =>
            {
                entity.HasKey(e => e.ShiftAccessId).HasName("PK__SHIFT_MA__D343B905E36F048C");

                entity.ToTable("SHIFT_MASTER_ACCESS");

                entity.Property(e => e.ShiftAccessId).HasColumnName("ShiftAccessID");
                entity.Property(e => e.Active)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();
                entity.Property(e => e.ApprovalStatus)
                    .HasMaxLength(10)
                    .IsUnicode(false);
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");
                entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");
                entity.Property(e => e.ProjectId).HasColumnName("ProjectID");
                entity.Property(e => e.ShiftApprovalId).HasColumnName("ShiftApprovalID");
                entity.Property(e => e.ShiftId).HasColumnName("ShiftID");
                entity.Property(e => e.ValidDateTo).HasColumnType("datetime");
                entity.Property(e => e.ValidDatefrom).HasColumnType("datetime");
                entity.Property(e => e.WeekEndMasterId).HasColumnName("WeekEndMasterID");
            });


            entity.ToTable("PayscaleRequest02");
        });
        modelBuilder.Entity<PayCodeMaster01>(entity =>
        {
            entity.HasKey(e => e.PayCodeId).HasName("PK__PayCodeM__8D8AC6AA45A74F0F");

            entity.ToTable("PayCodeMaster01");

            entity.Property(e => e.ApplicableEsipf)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ApplicableESIPF");
            entity.Property(e => e.ApplicableValue)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Code)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CompPercentage)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.PayCode)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.PayCodeDescription).IsUnicode(false);
            entity.Property(e => e.PayCodeMasterId).HasColumnName("PayCodeMasterID");
            entity.Property(e => e.PayRollReportId).HasColumnName("PayRollReportID");
            entity.Property(e => e.SortOrder)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Wpsorder).HasColumnName("WPSOrder");
        });
        modelBuilder.Entity<PayscaleRequest00>(entity =>
        {
            entity.HasKey(e => e.PayRequestId);

            entity.ToTable("PayscaleRequest00");

            entity.Property(e => e.BatchStatus)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.EffectiveDate).HasColumnType("datetime");
            entity.Property(e => e.EmployeeIds).IsUnicode(false);
            entity.Property(e => e.EntryDate).HasColumnType("datetime");
            entity.Property(e => e.FlowStatus)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.PayReqCode)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.RejectReason)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.RejectStatus)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();
        });
        modelBuilder.Entity<BranchDetail>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("BranchDetails");

            entity.Property(e => e.Branch).IsUnicode(false);
            entity.Property(e => e.LinkId).HasColumnName("LinkID");
        });
        modelBuilder.Entity<EmployeeLatestPayrollPeriod>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_ID");

            entity.ToTable("EmployeeLatestPayrollPeriod");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");
            entity.Property(e => e.EntryDate).HasColumnType("datetime");
            entity.Property(e => e.PayrollPeriodId).HasColumnName("PayrollPeriodID");
        });
        modelBuilder.Entity<EmployeeLatestPayrollBatch>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Employee__3214EC2732A48C90");

            entity.ToTable("EmployeeLatestPayrollBatch");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");
            entity.Property(e => e.EntryDate).HasColumnType("datetime");
            entity.Property(e => e.PayrollBatchId).HasColumnName("PayrollBatchID");
        });
        modelBuilder.Entity<Payroll00>(entity =>
        {
            entity.HasKey(e => e.PayrollPeriodId).HasName("PK__Payroll0__06190D56835A23C6");

            entity.ToTable("Payroll00");

            entity.Property(e => e.PayrollPeriodId).HasColumnName("PayrollPeriodID");
            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.IsIndia).HasColumnName("Is_india");
            entity.Property(e => e.PeriodCode)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.StartDate).HasColumnType("datetime");
        });
        modelBuilder.Entity<PayCodeMaster00>(entity =>
        {
            entity.HasKey(e => e.PayCodeMasterId).HasName("PK__PayCodeM__C06729EF010E6A30");

            entity.ToTable("PayCodeMaster00");

            entity.Property(e => e.PayCodeMasterId).HasColumnName("PayCodeMasterID");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Description)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.Wpsformat).HasColumnName("WPSFormat");
        });


        modelBuilder.Entity<ParamRole02>(entity =>
        {
            entity.HasKey(e => e.ValueId);

            entity.ToTable("ParamRole02");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.EmpId).HasColumnName("Emp_Id");
            entity.Property(e => e.LinkEmpId).HasColumnName("LinkEmp_Id");
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
        });


        modelBuilder.Entity<Categorymasterparameter>(entity =>
        {
            entity.HasKey(e => e.ParameterId);

            entity.ToTable("CATEGORYMASTERPARAMETERS");

            entity.Property(e => e.ParameterId).HasColumnName("ParameterID");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.DataType).IsUnicode(false);
            entity.Property(e => e.EntityId).HasColumnName("EntityID");
            entity.Property(e => e.InstId).HasColumnName("Inst_Id");
            entity.Property(e => e.ParamDescription).IsUnicode(false);
        });

        modelBuilder.Entity<ParamRole01>(entity =>
        {
            entity.HasKey(e => e.ValueId);

            entity.ToTable("ParamRole01");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.EmpId).HasColumnName("Emp_Id");
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<ParamRole00>(entity =>
        {
            entity.HasKey(e => e.ValueId);

            entity.ToTable("ParamRole00");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.EmpId).HasColumnName("Emp_Id");
            entity.Property(e => e.EntityLevel).IsUnicode(false);
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<ParamRoleEntityLevel00>(entity =>
        {
            entity.ToTable("ParamRoleEntityLevel00");
        });

        modelBuilder.Entity<BranchDetail>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("BranchDetails");

            entity.Property(e => e.Branch).IsUnicode(false);
            entity.Property(e => e.LinkId).HasColumnName("LinkID");
        });
        modelBuilder.Entity<Geotagging00>(entity =>
        {
            entity.HasKey(e => e.GeoCompId).HasName("PK__Geotaggi__6F155C9C1629416B");

            entity.ToTable("Geotagging00");

            entity.Property(e => e.GeoCompId).HasColumnName("GeoCompID");
            entity.Property(e => e.EntryDate).HasColumnType("datetime");
            entity.Property(e => e.LevelId).HasColumnName("LevelID");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<Geotagging00A>(entity =>
        {
            entity.HasKey(e => e.GeoCompAid).HasName("PK__Geotaggi__02617E62AE362FCC");

            entity.ToTable("Geotagging00A");

            entity.Property(e => e.GeoCompAid).HasColumnName("GeoCompAID");
            entity.Property(e => e.GeoCompId).HasColumnName("GeoCompID");
            entity.Property(e => e.Latitude)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.Longitude)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.Radius)
                .HasMaxLength(1000)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Geotagging01>(entity =>
        {
            entity.HasKey(e => e.GeoEntityId).HasName("PK__Geotaggi__712CC4893B84D6CE");

            entity.ToTable("Geotagging01");

            entity.Property(e => e.GeoEntityId).HasColumnName("GeoEntityID");
            entity.Property(e => e.EntryDate).HasColumnType("datetime");
            entity.Property(e => e.LevelId).HasColumnName("LevelID");
            entity.Property(e => e.LinkId).HasColumnName("LinkID");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<Geotagging01A>(entity =>
        {
            entity.HasKey(e => e.GeoEntityAid).HasName("PK__Geotaggi__DB6475D20DE5DD81");

            entity.ToTable("Geotagging01A");

            entity.Property(e => e.GeoEntityAid).HasColumnName("GeoEntityAID");
            entity.Property(e => e.GeoEntityId).HasColumnName("GeoEntityID");
            entity.Property(e => e.Latitude)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.Longitude)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.Radius)
                .HasMaxLength(1000)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Geotagging02>(entity =>
        {
            entity.HasKey(e => e.GeoEmpId).HasName("PK__Geotaggi__56C49986F19D9287");

            entity.ToTable("Geotagging02");

            entity.Property(e => e.GeoEmpId).HasColumnName("GeoEmpID");
            entity.Property(e => e.EmpId).HasColumnName("EmpID");
            entity.Property(e => e.EntryDate).HasColumnType("datetime");
            entity.Property(e => e.LevelId).HasColumnName("LevelID");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });
        modelBuilder.Entity<HraHistory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__HRA_Hist__3214EC07EECA6A1D");

            entity.ToTable("HRA_History");

            entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");
            entity.Property(e => e.EntryDate)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FromDate).HasColumnType("datetime");
            entity.Property(e => e.Initial).HasDefaultValue(0);
            entity.Property(e => e.IsHra).HasColumnName("IsHRA");
            entity.Property(e => e.Remarks)
                .HasMaxLength(5000)
                .IsUnicode(false);
            entity.Property(e => e.ToDate).HasColumnType("datetime");
        });
        modelBuilder.Entity<Geotagging02A>(entity =>
        {
            entity.HasKey(e => e.GeoEmpAid).HasName("PK__Geotaggi__43E46D224C75468B");

            entity.ToTable("Geotagging02A");

            entity.Property(e => e.GeoEmpAid).HasColumnName("GeoEmpAID");
            entity.Property(e => e.Coordinates).IsUnicode(false);
            entity.Property(e => e.GeoEmpId).HasColumnName("GeoEmpID");
            entity.Property(e => e.Latitude)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.Longitude)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.Radius)
                .HasMaxLength(1000)
                .IsUnicode(false);
        });
        modelBuilder.Entity<AdmUserMaster>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK_User_Master");

            entity.ToTable("ADM_User_Master");

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.Active)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("active");
            entity.Property(e => e.Address1)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Address2)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ContactNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.DetailedName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.EntryDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FirstLoginTime).HasColumnType("datetime");
            entity.Property(e => e.LangId).HasColumnName("LangID");
            entity.Property(e => e.NeedApp).HasColumnName("Need_App");
            entity.Property(e => e.Password).HasMaxLength(30);
            entity.Property(e => e.Status)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("status");
            entity.Property(e => e.UploadEmpId).HasColumnName("UploadEmpID");
            entity.Property(e => e.UserName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UserType)
                .HasMaxLength(5)
                .IsUnicode(false);
        });

        modelBuilder.Entity<AdmUserRoleMaster>(entity =>
        {
            entity.HasKey(e => e.InstId);
            entity.ToTable("ADM_UserRoleMaster");

            entity.HasIndex(e => new { e.UserId, e.Acess }, "ix_user_access");

            entity.Property(e => e.InstId).HasColumnName("Inst_Id");
            entity.Property(e => e.RoleId).HasColumnName("Role_Id");
            entity.Property(e => e.UserId).HasColumnName("User_Id");
        });
        modelBuilder.Entity<TabAccessRight>(entity =>
        {
            entity.HasKey(e => e.AccessId);

            entity.Property(e => e.TabOrNot).HasDefaultValue(0);
        });
        modelBuilder.Entity<TabMaster>(entity =>
        {
            entity.HasKey(e => e.TabId);

            entity.ToTable("TabMaster");

            entity.Property(e => e.Active)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Code)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.TabName)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.TabOrNot).HasDefaultValue(0);
        });
        modelBuilder.Entity<TransferDetail>(entity =>
        {
            entity.HasKey(e => e.TransferBatchId).HasName("PK__Transfer__DB4C6F609F3C7750");

            entity.Property(e => e.TransferBatchId).HasColumnName("TransferBatchID");
            entity.Property(e => e.ApprovalStatus)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.EntryDate)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FinalApprovalDate).HasColumnType("datetime");
            entity.Property(e => e.FlowStatus)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ProxyId).HasColumnName("ProxyID");
            entity.Property(e => e.TransferSequence)
                .HasMaxLength(2000)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TransferDetails00>(entity =>
        {
            entity.HasKey(e => e.TransferId).HasName("PK__Transfer__9549017179AD5AF0");

            entity.ToTable("TransferDetails00");

            entity.Property(e => e.TransferId).HasColumnName("TransferID");
            entity.Property(e => e.ActionId).HasColumnName("ActionID");
            entity.Property(e => e.ApprovalStatus)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.BandId).HasColumnName("BandID");
            entity.Property(e => e.BranchId).HasColumnName("BranchID");
            entity.Property(e => e.CompanyAccomodation).HasDefaultValue(0);
            entity.Property(e => e.DepartmentId).HasColumnName("DepartmentID");
            entity.Property(e => e.DesignationId).HasColumnName("DesignationID");
            entity.Property(e => e.EmpEntity).IsUnicode(false);
            entity.Property(e => e.EmpId).HasColumnName("EmpID");
            entity.Property(e => e.EntryDate).HasColumnType("datetime");
            entity.Property(e => e.EntryFrom)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.FlowStatus)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.GradeId).HasColumnName("GradeID");
            entity.Property(e => e.HistoryFromDate).HasColumnType("datetime");
            entity.Property(e => e.Hraeligible).HasColumnName("HRAEligible");
            entity.Property(e => e.IsDirect).HasDefaultValue(0);
            entity.Property(e => e.IsFutureDate).HasDefaultValue(0);
            entity.Property(e => e.LeaveCount).HasDefaultValue(0.0);
            entity.Property(e => e.ProxyId).HasColumnName("ProxyID");
            entity.Property(e => e.RelievingDate).HasColumnType("datetime");
            entity.Property(e => e.RelocationAllowance).HasDefaultValue(0);
            entity.Property(e => e.RelocationLeave).HasDefaultValue(0);
            entity.Property(e => e.Remarks).IsUnicode(false);
            entity.Property(e => e.RevokedDate).HasColumnType("datetime");
            entity.Property(e => e.ToDate).HasColumnType("datetime");
            entity.Property(e => e.TransferBatchId).HasColumnName("TransferBatchID");
            entity.Property(e => e.TransferRemarks).IsUnicode(false);
            entity.Property(e => e.TransferSequence)
                .HasMaxLength(500)
                .IsUnicode(false);
        });
        modelBuilder.Entity<LicensedCompanyDetail>(entity =>
        {
            entity.HasKey(e => e.LicenseId).HasName("PK__Licensed__72D60082F948EAFC");

            entity.ToTable(tb =>
            {
                tb.HasTrigger("SetEntityUploadSettings");
                tb.HasTrigger("SetUploadSettings");
            });

            entity.Property(e => e.CompanyName)
                .HasMaxLength(3000)
                .IsUnicode(false);
            entity.Property(e => e.DomainName)
                .HasMaxLength(2000)
                .IsUnicode(false);
        });
        modelBuilder.Entity<Categorymaster>(entity =>
        {
            entity.HasKey(e => e.EntityId);

            entity.ToTable("CATEGORYMASTER");

            entity.Property(e => e.EntityId).HasColumnName("EntityID");
            entity.Property(e => e.CatTrxTypeId).HasColumnName("CatTrxTypeID");
            entity.Property(e => e.Description).IsUnicode(false);
            entity.Property(e => e.EntryBy).HasColumnName("Entry_By");
            entity.Property(e => e.InstId).HasColumnName("Inst_Id");
            entity.Property(e => e.TransactionDate).HasColumnType("datetime");
        });
        modelBuilder.Entity<TransferTransition00>(entity =>
        {
            entity.HasKey(e => e.TransferTransId).HasName("PK__Transfer__83C76B485DE21C00");

            entity.ToTable("TransferTransition00");

            entity.Property(e => e.TransferTransId).HasColumnName("TransferTransID");
            entity.Property(e => e.ActionId).HasColumnName("ActionID");
            entity.Property(e => e.BatchApprovalStatus)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.CurrentEmpId).HasColumnName("CurrentEmpID");
            entity.Property(e => e.EmpApprovalStatus)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");
            entity.Property(e => e.FromDate).HasColumnType("datetime");
            entity.Property(e => e.NewEntityDescription)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.NewEntityId).HasColumnName("NewEntityID");
            entity.Property(e => e.OldEntityDescription)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.OldEntityId).HasColumnName("OldEntityID");
            entity.Property(e => e.PreviousEmpId).HasColumnName("PreviousEmpID");
            entity.Property(e => e.RevokedDate).HasColumnType("datetime");
            entity.Property(e => e.ToDate).HasColumnType("datetime");
            entity.Property(e => e.TransferBatchId).HasColumnName("TransferBatchID");
            entity.Property(e => e.TransferId).HasColumnName("TransferID");
        });
        modelBuilder.Entity<CompanyConveyanceHistory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__CompanyC__3214EC07F769034C");

            entity.ToTable("CompanyConveyance_History");

            entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");
            entity.Property(e => e.EntryDate).HasColumnType("datetime");
            entity.Property(e => e.FromDate).HasColumnType("datetime");
            entity.Property(e => e.Initial).HasDefaultValue(0);
            entity.Property(e => e.ToDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<CompanyVehicleHistory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__CompanyV__3214EC07484FBAF2");

            entity.ToTable("CompanyVehicle_History");

            entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");
            entity.Property(e => e.EntryDate).HasColumnType("datetime");
            entity.Property(e => e.FromDate).HasColumnType("datetime");
            entity.Property(e => e.Initial).HasDefaultValue(0);
            entity.Property(e => e.ToDate).HasColumnType("datetime");
        });
        modelBuilder.Entity<SurveyRelation>(entity =>
        {
            entity.HasKey(e => e.ProbId).HasName("PK__SurveyRe__078036D7D0267F0A");

            entity.ToTable("SurveyRelation");

            entity.Property(e => e.EntryDate).HasColumnType("datetime");
            entity.Property(e => e.ReviewDate).HasColumnType("datetime");
        });
        modelBuilder.Entity<ProbationRating00>(entity =>
        {
            entity.HasKey(e => e.ProbRateId).HasName("PK__Probatio__1A758B52EF4673E7");

            entity.ToTable("ProbationRating00");

            entity.Property(e => e.ApprovalStatus)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.EntryDate).HasColumnType("datetime");
            entity.Property(e => e.FinalApprovalDate).HasColumnType("datetime");
            entity.Property(e => e.FinalReviewStatus)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FlowStatus)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.ManualApproveDate).HasColumnType("datetime");
            entity.Property(e => e.ManualApprover).HasDefaultValue(0);
            entity.Property(e => e.NextReviewDate).HasColumnType("datetime");
            entity.Property(e => e.Remark).IsUnicode(false);
            entity.Property(e => e.RequestCode).HasMaxLength(500);
            entity.Property(e => e.RequesterEmpId)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.TrainingRequiredIds).IsUnicode(false);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<ProbationRating02>(entity =>
        {
            entity.HasKey(e => e.ProbRateId2).HasName("PK__Probatio__1BDCEE345975D2B3");

            entity.ToTable("ProbationRating02");

            entity.Property(e => e.NextRemarkDate).HasColumnType("datetime");
            entity.Property(e => e.RemarkStatus)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.RemarkerId).HasColumnName("RemarkerID");
            entity.Property(e => e.RuleOrder).HasDefaultValue(0);
        });
        modelBuilder.Entity<ProbationWorkFlowstatus>(entity =>
        {
            entity.HasKey(e => e.FlowId).HasName("PK__Probatio__1184B35CC2DF2EBD");

            entity.ToTable("ProbationWorkFlowstatus");

            entity.Property(e => e.FlowId).HasColumnName("FlowID");
            entity.Property(e => e.ApprovalStatus)
                .HasMaxLength(5)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Deligate)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.EntryBy).HasColumnName("Entry_By");
            entity.Property(e => e.EntryDt)
                .HasColumnType("datetime")
                .HasColumnName("Entry_Dt");
            entity.Property(e => e.EntryFrom)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedBy).HasColumnName("Updated_By");
            entity.Property(e => e.UpdatedDt)
                .HasColumnType("datetime")
                .HasColumnName("Updated_Dt");
            entity.Property(e => e.UpdatedFrom)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<CategoryGroup>(entity =>
        {
            entity.HasKey(e => e.CatId);

            entity.ToTable("CategoryGroup");

            entity.Property(e => e.GroupId).HasColumnName("GroupID");
        });
        modelBuilder.Entity<AssetcategoryCode>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Assetcat__3213E83F204A50B5");

            entity.ToTable("AssetcategoryCode");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AssetModel).IsUnicode(false);
            entity.Property(e => e.Assetclass).IsUnicode(false);
            entity.Property(e => e.Code)
                .IsUnicode(false)
                .HasColumnName("code");
            entity.Property(e => e.Createdby)
                .HasColumnType("datetime")
                .HasColumnName("createdby");
            entity.Property(e => e.Description)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("status");
            entity.Property(e => e.Value).HasColumnName("value");
        });

        modelBuilder.Entity<AssetRequestHistory>(entity =>
        {
            entity.HasKey(e => e.AsstHisId);

            entity.ToTable("AssetRequestHistory");

            entity.Property(e => e.EntryDate).HasColumnType("datetime");
        });
        modelBuilder.Entity<HrmsEmpdocumentsHistory00>(entity =>
        {
            entity.HasKey(e => e.DocHistId).HasName("PK__HRMS_EMP__6297437FD3210C68");

            entity.ToTable("HRMS_EMPDocumentsHistory00");

            entity.Property(e => e.DocHistId).HasColumnName("DocHistID");
            entity.Property(e => e.DateFrom).HasColumnType("datetime");
            entity.Property(e => e.DetailId).HasColumnName("DetailID");
            entity.Property(e => e.DocApprovedId).HasColumnName("DocApprovedID");
            entity.Property(e => e.DocId).HasColumnName("DocID");
            entity.Property(e => e.DocStatus)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.EmpId).HasColumnName("EmpID");
            entity.Property(e => e.EntryDate).HasColumnType("datetime");
            entity.Property(e => e.RequestId)
                .IsUnicode(false)
                .HasColumnName("RequestID");
            entity.Property(e => e.Status)
                .HasMaxLength(100)
                .IsUnicode(false);
        });
        modelBuilder.Entity<HrmsEmpdocumentsHistory01>(entity =>
        {
            entity.HasKey(e => e.DocFieldId).HasName("PK__HRMS_EMP__E2BF7E5075376BCC");

            entity.ToTable("HRMS_EMPDocumentsHistory01");

            entity.Property(e => e.DocFieldId).HasColumnName("DocFieldID");
            entity.Property(e => e.DetailId).HasColumnName("DetailID");
            entity.Property(e => e.DocHisId).HasColumnName("DocHisID");
            entity.Property(e => e.DocValues)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.OldDocValues)
                .HasMaxLength(1000)
                .IsUnicode(false);
        });

        modelBuilder.Entity<HrmsEmpdocumentsHistory02>(entity =>
        {
            entity.HasKey(e => e.DocAttachId).HasName("PK__HRMS_EMP__DBC91E97F7A7F818");

            entity.ToTable("HRMS_EMPDocumentsHistory02");

            entity.Property(e => e.DocAttachId).HasColumnName("DocAttachID");
            entity.Property(e => e.DetailId).HasColumnName("DetailID");
            entity.Property(e => e.DocHisId).HasColumnName("DocHisID");
            entity.Property(e => e.FileName)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.FileType)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.OldFileName)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(5)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TravelType>(entity =>
        {
            entity.ToTable("TRAVEL_TYPE");

            entity.Property(e => e.TravelTypeId).HasColumnName("TravelType_Id");
            entity.Property(e => e.TravelType1)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("TravelType");
            entity.Property(e => e.Value).HasColumnName("value");
        });

        modelBuilder.Entity<TimeOffSet>(entity =>
        {
            entity.HasKey(e => e.TimeOffSetId).HasName("PK__TimeOffS__A4BBC91B5BCE9A6C");

            entity.ToTable("TimeOffSet");

            entity.Property(e => e.AddSign)
                .HasMaxLength(4)
                .IsUnicode(false);
            entity.Property(e => e.Description)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Offset)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.OffsetValue)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<RoleDelegation00>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("RoleDelegation00");

            entity.Property(e => e.Acceptstatus).HasColumnName("acceptstatus");
            entity.Property(e => e.ApprovalStatus)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CompanyId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.EntryDate)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FromDate).HasColumnType("datetime");
            entity.Property(e => e.Remarks).IsUnicode(false);
            entity.Property(e => e.Revoke)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.RevokeDate).HasColumnType("datetime");
            entity.Property(e => e.RoleDelegationId).ValueGeneratedOnAdd();
            entity.Property(e => e.SequenceId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ToDate).HasColumnType("datetime");
            entity.Property(e => e.Transactionids).IsUnicode(false);
        });

        modelBuilder.Entity<Roledelegationtransaction>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("roledelegationtransaction");

            entity.Property(e => e.TransactionId)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.TransactionIdString).IsUnicode(false);
        });

        modelBuilder.Entity<Dependent01>(entity =>
        {
            entity.HasKey(e => e.DocId);

            entity.ToTable("Dependent01");

            entity.Property(e => e.DocFileName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.DocFileType)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<DependentEducation>(entity =>
        {
            entity.HasKey(e => e.DepEduId);

            entity.ToTable("DependentEducation");

            entity.Property(e => e.CourseType)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.EntryDate).HasColumnType("datetime");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            entity.Property(e => e.Year)
                .HasMaxLength(10)
                .IsUnicode(false);
        });

        modelBuilder.Entity<EducationMaster>(entity =>
        {
            entity.HasKey(e => e.EducId);

            entity.ToTable("EducationMaster");

            entity.Property(e => e.EntryDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<EdCourseMaster>(entity =>
        {
            entity.HasKey(e => e.CourseId);

            entity.ToTable("EdCourseMaster");

            entity.Property(e => e.EntryDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<EdSpecializationMaster>(entity =>
        {
            entity.HasKey(e => e.EdSpecId);

            entity.ToTable("EdSpecializationMaster");
        });

        modelBuilder.Entity<UniversityMaster>(entity =>
        {
            entity.HasKey(e => e.UniId);

            entity.ToTable("UniversityMaster");

            entity.Property(e => e.EntryDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<SpecialWorkFlow>(entity =>
        {
            entity.HasKey(e => e.ValueId).HasName("PK__SpecialW__93364E48B376BC26");

            entity.ToTable("SpecialWorkFlow");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.EntityLevel).IsUnicode(false);
            entity.Property(e => e.GrievanceTypeId)
                .HasDefaultValue(0)
                .HasColumnName("GrievanceTypeID");
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
        });
        modelBuilder.Entity<SalaryConfirmationLetterType>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("SalaryConfirmationLetterType");

            entity.Property(e => e.AccountNo).IsUnicode(false);
            entity.Property(e => e.BankName).IsUnicode(false);
            entity.Property(e => e.BranchName).IsUnicode(false);
            entity.Property(e => e.IdentificationCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.LetterReqId).HasColumnName("LetterReqID");
            entity.Property(e => e.Location).IsUnicode(false);
            entity.Property(e => e.SalLetterId)
                .ValueGeneratedOnAdd()
                .HasColumnName("Sal_LetterID");
        });
        modelBuilder.Entity<LetterWorkflowStatus>(entity =>
        {
            entity.HasKey(e => e.FlowId);

            entity.ToTable("LetterWorkflowStatus");

            entity.Property(e => e.FlowId).HasColumnName("FlowID");
            entity.Property(e => e.ApprovalRemarks)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.ApprovalStatus)
                .HasMaxLength(5)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Deligate)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.EntryBy).HasColumnName("Entry_By");
            entity.Property(e => e.EntryDt)
                .HasColumnType("datetime")
                .HasColumnName("Entry_Dt");
            entity.Property(e => e.EntryFrom)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.HrRemarks).IsUnicode(false);
            entity.Property(e => e.UpdatedBy).HasColumnName("Updated_By");
            entity.Property(e => e.UpdatedDt)
                .HasColumnType("datetime")
                .HasColumnName("Updated_Dt");
            entity.Property(e => e.UpdatedFrom)
                .HasMaxLength(20)
                .IsUnicode(false);
        });
        modelBuilder.Entity<EmailNotification>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_ADM_EMAIL_NOTIFICATION");

            entity.ToTable("EMAIL_NOTIFICATION", tb => tb.HasTrigger("AutoNotification"));

            entity.Property(e => e.AttachFormat)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.EmailBody).IsUnicode(false);
            entity.Property(e => e.EmailSubject)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.ExceptionLog)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.ExpiryDate).HasColumnType("datetime");
            entity.Property(e => e.FileByte).IsUnicode(false);
            entity.Property(e => e.FromDate).HasColumnType("datetime");
            entity.Property(e => e.InstdId).HasColumnName("Instd_Id");
            entity.Property(e => e.MailType)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.MobileStatus).HasDefaultValue(0);
            entity.Property(e => e.MonthYear)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("Month_Year");
            entity.Property(e => e.NotificationMessage).IsUnicode(false);
            entity.Property(e => e.NotificationType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Path).IsUnicode(false);
            entity.Property(e => e.ReceiverEmail)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.ReceiverEmpId).HasColumnName("ReceiverEmpID");
            entity.Property(e => e.RequestIdCode)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.RequesterDate).HasColumnType("datetime");
            entity.Property(e => e.RequesterEmpId).HasColumnName("RequesterEmpID");
            entity.Property(e => e.SendDate).HasColumnType("datetime");
            entity.Property(e => e.SendMail)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.SendMode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.SenderEmail)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.SenderEmailPwd)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.ToDate).HasColumnType("datetime");
            entity.Property(e => e.TriggerDate).HasColumnType("datetime");
            entity.Property(e => e.Workflowtype).HasDefaultValue(1);
        });

        modelBuilder.Entity<HrEmpEmergaddressApprl>(entity =>
        {
            entity.HasKey(e => e.AddrId).HasName("PK__HR_EMP_E__BCDB8FA31D1B7990");

            entity.ToTable("HR_EMP_EMERGADDRESS_APPRL");

            entity.Property(e => e.AddrId).HasColumnName("AddrID");
            entity.Property(e => e.Address)
                .HasMaxLength(5000)
                .IsUnicode(false);
            entity.Property(e => e.AlterPhoneNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.DateFrom).HasColumnType("datetime");
            entity.Property(e => e.EmpId).HasColumnName("EmpID");
            entity.Property(e => e.EntryDate).HasColumnType("datetime");
            entity.Property(e => e.FlowStatus)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.MasterId)
                .HasDefaultValue(0)
                .HasColumnName("MasterID");
            entity.Property(e => e.MobileNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PhoneNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PinNo)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.RequestId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Request_ID");
            entity.Property(e => e.Status)
                .HasMaxLength(10)
                .IsUnicode(false);
        });
        modelBuilder.Entity<EmployeeSequenceAccess>(entity =>
        {
            entity.HasKey(e => e.SequenceEmployeeId);

            entity.ToTable("EmployeeSequenceAccess");

            entity.Property(e => e.FromDate).HasColumnType("datetime");
            entity.Property(e => e.ValidTo).HasColumnType("datetime");
        });
        modelBuilder.Entity<EditInfoMaster00>(entity =>
        {
            entity.HasKey(e => e.InfoId).HasName("PK__EditInfo__4DEC9D9AAFC859D1");

            entity.ToTable("EditInfoMaster00");

            entity.Property(e => e.InfoId).HasColumnName("InfoID");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.EntryDate)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.InfoCode)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedDate)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");
        });
        modelBuilder.Entity<DesignationDetail>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("DesignationDetails");

            entity.Property(e => e.Designation).IsUnicode(false);
            entity.Property(e => e.LinkId).HasColumnName("LinkID");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
