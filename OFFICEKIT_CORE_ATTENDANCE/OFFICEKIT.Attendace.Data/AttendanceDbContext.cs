using Microsoft.EntityFrameworkCore;
using OFFICEKIT_CORE_ATTENDANCE.OFFICEKIT.Attendance.Models;

namespace OFFICEKIT_CORE_ATTENDANCE.OFFICEKIT.Attendace.Data
{
    public class AttendanceDbContext(DbContextOptions<AttendanceDbContext> options) : DbContext(options)
    {
        public virtual DbSet<EmployeeDetail> EmployeeDetails { get; set; }
        public virtual DbSet<DesignationDetail> DesignationDetails { get; set; }
        public virtual DbSet<DepartmentDetail> DepartmentDetails { get; set; }
        public virtual DbSet<Attendancelog> Attendancelogs { get; set; }
        public virtual DbSet<TransactionMaster> TransactionMasters { get; set; }
        public virtual DbSet<HrEmpMaster> HrEmpMasters { get; set; }
        public virtual DbSet<SpecialAccessRight> SpecialAccessRights { get; set; }
        public virtual DbSet<ShiftMasterAccess> ShiftMasterAccesses { get; set; }
        public virtual DbSet<HrShift00> HrShift00s { get; set; }
        public virtual DbSet<HrShift01> HrShift01s { get; set; }
        public virtual DbSet<WeekEndMaster> WeekEndMasters { get; set; }
        public virtual DbSet<ProjectMaster> ProjectMasters { get; set; }
        public virtual DbSet<BranchDetail> BranchDetails { get; set; }
        public virtual DbSet<EntityAccessRights02> EntityAccessRights02s { get; set; }
        public virtual DbSet<HighLevelViewTable> HighLevelViewTables { get; set; }
        public virtual DbSet<EntityApplicable00> EntityApplicable00s { get; set; }
        public virtual DbSet<EntityApplicable01> EntityApplicable01s { get; set; }
        public virtual DbSet<HrEmpReporting> HrEmpReportings { get; set; }
        public virtual DbSet<ShiftApproval00> ShiftApproval00s { get; set; }
        public virtual DbSet<HrEmployeeUserRelation> HrEmployeeUserRelations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

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
            modelBuilder.Entity<DesignationDetail>(entity =>
            {
                entity
                    .HasNoKey()
                    .ToView("DesignationDetails");

                entity.Property(e => e.Designation).IsUnicode(false);
                entity.Property(e => e.LinkId).HasColumnName("LinkID");
            });
            modelBuilder.Entity<DepartmentDetail>(entity =>
            {
                entity
                    .HasNoKey()
                    .ToView("DepartmentDetails");

                entity.Property(e => e.Department).IsUnicode(false);
                entity.Property(e => e.LinkId).HasColumnName("LinkID");
            });
            modelBuilder.Entity<Attendancelog>(entity =>
            {
                entity.HasKey(e => e.AttLogId).HasName("PK__ATTENDAN__52A6441597EA1CCD");

                entity.ToTable("ATTENDANCELOG");

                entity.HasIndex(e => e.EmployeeId, "ix_employee_id");

                entity.Property(e => e.AttDirection).HasMaxLength(255);
                entity.Property(e => e.C1).HasMaxLength(255);
                entity.Property(e => e.C2).HasMaxLength(255);
                entity.Property(e => e.C3).HasMaxLength(255);
                entity.Property(e => e.C4).HasMaxLength(255);
                entity.Property(e => e.C5).HasMaxLength(255);
                entity.Property(e => e.C6).HasMaxLength(255);
                entity.Property(e => e.C7).HasMaxLength(255);
                entity.Property(e => e.Direction).HasMaxLength(255);
                entity.Property(e => e.DownloadDate).HasColumnType("datetime");
                entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");
                entity.Property(e => e.IsProcessed)
                    .HasMaxLength(5)
                    .IsUnicode(false);
                entity.Property(e => e.Latitude)
                    .HasMaxLength(5000)
                    .IsUnicode(false);
                entity.Property(e => e.LiveLocation).IsUnicode(false);
                entity.Property(e => e.LogDate).HasColumnType("datetime");
                entity.Property(e => e.LogInDeviceName)
                    .HasMaxLength(5000)
                    .IsUnicode(false);
                entity.Property(e => e.LogInDeviceType)
                    .HasMaxLength(5)
                    .IsUnicode(false);
                entity.Property(e => e.Longitude)
                    .HasMaxLength(5000)
                    .IsUnicode(false);
                entity.Property(e => e.MissingInOutId).HasDefaultValue(0);
                entity.Property(e => e.ProcessedDate).HasColumnType("smalldatetime");
                entity.Property(e => e.RequestSequenceId)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("RequestSequenceID");
                entity.Property(e => e.UniversalLogDate).HasColumnType("datetime");
                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
                entity.Property(e => e.UserId).HasMaxLength(50);
                entity.Property(e => e.WorkCode).HasMaxLength(255);
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
            modelBuilder.Entity<SpecialAccessRight>(entity =>
            {
                entity.HasKey(e => e.SpecialId);
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
            modelBuilder.Entity<HrShift01>(entity =>
            {
                entity.HasKey(e => e.Shift01Id).HasName("PK__HR_SHIFT__E287E454CF39E98C");

                entity.ToTable("HR_SHIFT01");

                entity.Property(e => e.Shift01Id).HasColumnName("Shift01ID");
                entity.Property(e => e.EffectiveFrom).HasColumnType("datetime");
                entity.Property(e => e.EndTime).HasColumnType("decimal(18, 2)");
                entity.Property(e => e.MinimumWorkHours).HasColumnType("decimal(18, 2)");
                entity.Property(e => e.ShiftId).HasColumnName("ShiftID");
                entity.Property(e => e.StartTime).HasColumnType("decimal(18, 2)");
                entity.Property(e => e.TotalHours).HasColumnType("decimal(18, 2)");
            });
            modelBuilder.Entity<WeekEndMaster>(entity =>
            {
                entity.HasKey(e => e.WeekEndMasterId).HasName("PK__WeekEndM__EE87957BA8A1504B");

                entity.ToTable("WeekEndMaster");

                entity.Property(e => e.WeekEndMasterId).HasColumnName("WeekEndMasterID");
                entity.Property(e => e.EntryDate).HasColumnType("datetime");
                entity.Property(e => e.Name)
                    .HasMaxLength(500)
                    .IsUnicode(false);
            });
            modelBuilder.Entity<ProjectMaster>(entity =>
            {
                entity
                    .HasNoKey()
                    .ToTable("project_Master");

                entity.Property(e => e.EndDate).HasColumnType("datetime");
                entity.Property(e => e.EntryBy)
                    .HasMaxLength(10)
                    .IsUnicode(false);
                entity.Property(e => e.EntryDate).HasColumnType("datetime");
                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");
                entity.Property(e => e.ProjectCode)
                    .HasMaxLength(10)
                    .IsUnicode(false);
                entity.Property(e => e.ProjectName).IsUnicode(false);
                entity.Property(e => e.StartDate).HasColumnType("datetime");
                entity.Property(e => e.Status)
                    .HasMaxLength(10)
                    .IsUnicode(false);
                entity.Property(e => e.UpdatedBy)
                    .HasMaxLength(10)
                    .IsUnicode(false);
                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            });
            modelBuilder.Entity<BranchDetail>(entity =>
            {
                entity
                    .HasNoKey()
                    .ToView("BranchDetails");

                entity.Property(e => e.Branch).IsUnicode(false);
                entity.Property(e => e.LinkId).HasColumnName("LinkID");
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
            modelBuilder.Entity<EntityApplicable00>(entity =>
            {
                entity.HasKey(e => e.ApplicableId);

                entity.ToTable("EntityApplicable00");

                entity.Property(e => e.EntryDate).HasColumnType("datetime");
            });
            modelBuilder.Entity<EntityApplicable01>(entity =>
            {
                entity.HasKey(e => e.ApplicableId);

                entity.ToTable("EntityApplicable01");

                entity.Property(e => e.EntryDate).HasColumnType("datetime");
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
            modelBuilder.Entity<ShiftApproval00>(entity =>
            {
                entity
                    .HasNoKey()
                    .ToTable("ShiftApproval00");

                entity.Property(e => e.ApprovalStatus)
                    .HasMaxLength(10)
                    .IsUnicode(false);
                entity.Property(e => e.EntryDate).HasColumnType("datetime");
                entity.Property(e => e.FlowStatus)
                    .HasMaxLength(10)
                    .IsUnicode(false);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.RequestId)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

        }
    }
}
