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

        }
    }
}
