using Microsoft.EntityFrameworkCore;
using OFFICEKITCORELEAVE.OfficeKit.Leave.MODELS;
using OFFICEKITCORELEAVE.OfficeKitHR.Leave.MODELS;

namespace OFFICEKITCORELEAVE.OfficeKit.Leave.Data;
public class LeaveDBContext : DbContext
    {
    private IConfiguration _configuration { get; }

    public LeaveDBContext (DbContextOptions<LeaveDBContext> options, IConfiguration configuration) : base (options)
        {
        _configuration = configuration;
        }
    public virtual DbSet<HrmLeaveMaster> HrmLeaveMasters { get; set; }
    public virtual DbSet<HrmLeaveBasicSetting> HrmLeaveBasicSettings { get; set; }
    public virtual DbSet<HrmLeaveMasterandsettingsLink> HrmLeaveMasterandsettingsLinks { get; set; }
    public virtual DbSet<HrmLeaveEntitlementReg> HrmLeaveEntitlementRegs { get; set; }
    public virtual DbSet<HrmLeaveServicedbasedleave> HrmLeaveServicedbasedleaves { get; set; }
    public virtual DbSet<HrmLeaveEntitlementHead> HrmLeaveEntitlementHeads { get; set; }
    public virtual DbSet<HrmLeavePartialPayment> HrmLeavePartialPayments { get; set; }
    public virtual DbSet<HrmLeaveBasicsettingsDetail> HrmLeaveBasicsettingsDetails { get; set; }
    public virtual DbSet<HrmLeaveBasicsettingsDetailsHistory> HrmLeaveBasicsettingsDetailsHistories { get; set; }
    protected override void OnConfiguring (DbContextOptionsBuilder optionsBuilder)

        //=> optionsBuilder.UseSqlServer("Server=10.25.25.250\\sql2017,1435;Database=VELLAPALLY-02-01-2025;User Id=sa;Password=asd@123.;Integrated Security=False;TrustServerCertificate=True;");
        => optionsBuilder.UseSqlServer (_configuration.GetConnectionString ("Default"));

    protected override void OnModelCreating (ModelBuilder modelBuilder)
        {
        modelBuilder.Entity<HrmLeaveMaster> (entity =>
        {
            entity.HasKey (e => e.LeaveMasterId);

            entity.ToTable ("HRM_LEAVE_MASTER");

            entity.Property (e => e.Colour)
                .HasMaxLength (1000)
                .IsUnicode (false);
            entity.Property (e => e.CreatedDate)
                .HasDefaultValueSql ("(getdate())")
                .HasColumnType ("datetime");
            entity.Property (e => e.Description)
                .HasMaxLength (50)
                .IsUnicode (false);
            entity.Property (e => e.LeaveCode)
                .HasMaxLength (50)
                .IsUnicode (false);
        });

        modelBuilder.Entity<HrmLeaveBasicSetting> (entity =>
        {
            entity.HasKey (e => e.SettingsId);

            entity.ToTable ("HRM_LEAVE_BASIC_SETTINGS");

            entity.Property (e => e.CreatedDate)
                .HasDefaultValueSql ("(getdate())")
                .HasColumnType ("datetime");
            entity.Property (e => e.SettingsDescription)
                .HasMaxLength (50)
                .IsUnicode (false);
            entity.Property (e => e.SettingsName)
                .HasMaxLength (50)
                .IsUnicode (false);
        });

        modelBuilder.Entity<HrmLeaveMasterandsettingsLink> (entity =>
        {
            entity.HasKey (e => e.IdMasterandSettingsLink);

            entity.ToTable ("HRM_LEAVE_MASTERANDSETTINGS_LINK");

            entity.Property (e => e.IdMasterandSettingsLink).HasColumnName ("Id_MasterandSettingsLink");
            entity.Property (e => e.CreatedDate)
                .HasDefaultValueSql ("(getdate())")
                .HasColumnType ("datetime");
        });

        modelBuilder.Entity<HrmLeaveEntitlementReg> (entity =>
        {
            entity.HasKey (e => e.LeaveentitlementregId);

            entity.ToTable ("HRM_LEAVE_ENTITLEMENT_REG");

            entity.Property (e => e.LeaveentitlementregId).HasColumnName ("leaveentitlementregId");
            entity.Property (e => e.Count).HasColumnType ("decimal(18, 2)");
            entity.Property (e => e.LeaveCondition).HasColumnName ("leaveCondition");
        });

        modelBuilder.Entity<HrmLeaveServicedbasedleave> (entity =>
        {
            entity.HasKey (e => e.IdServiceLeave);

            entity.ToTable ("HRM_LEAVE_SERVICEDBASEDLEAVE");

            entity.Property (e => e.Experiancebasedrollover).HasColumnType ("decimal(18, 2)");
            entity.Property (e => e.LeaveCount).HasColumnType ("decimal(18, 2)");
        });

        modelBuilder.Entity<HrmLeaveEntitlementHead> (entity =>
        {
            entity.HasKey (e => e.LeaveEntitlementId);

            entity.ToTable ("HRM_LEAVE_ENTITLEMENT_HEAD");

            entity.Property (e => e.AllemployeeLeaveCount).HasColumnType ("decimal(18, 2)");
            entity.Property (e => e.CarryforwardNj).HasColumnName ("CarryforwardNJ");
            entity.Property (e => e.Cfbasedon).HasColumnName ("CFbasedon");
            entity.Property (e => e.CfbasedonNj).HasColumnName ("CFbasedonNJ");
            entity.Property (e => e.CreatedDate)
                .HasDefaultValueSql ("(getdate())")
                .HasColumnType ("datetime");
            entity.Property (e => e.ExtraLeaveCountProxy).HasColumnType ("decimal(18, 2)");
            entity.Property (e => e.Laps).HasColumnType ("decimal(18, 2)");
            entity.Property (e => e.LeaveCount).HasColumnType ("decimal(18, 2)");
            entity.Property (e => e.LeavefromProbationDt).HasColumnName ("LeavefromProbationDT");
            entity.Property (e => e.Rollovercount).HasColumnType ("decimal(18, 2)");
            entity.Property (e => e.RollovercountNj)
                .HasColumnType ("decimal(18, 2)")
                .HasColumnName ("RollovercountNJ");
            entity.Property (e => e.StartDate).HasColumnType ("datetime");
        });

        modelBuilder.Entity<HrmLeavePartialPayment> (entity =>
        {
            entity.HasKey (e => e.PartialpaymentId);

            entity.ToTable ("HRM_LEAVE_PartialPayment");

            entity.Property (e => e.Daysfrom).HasColumnType ("decimal(18, 2)");
            entity.Property (e => e.Daysto).HasColumnType ("decimal(18, 2)");
            entity.Property (e => e.PayPercentage).HasColumnType ("decimal(18, 2)");
        });

        modelBuilder.Entity<HrmLeaveBasicsettingsDetail> (entity =>
        {
            entity.HasKey (e => e.SettingsDetailsId);

            entity.ToTable ("HRM_LEAVE_BASICSETTINGS_DETAILS");

            entity.Property (e => e.CompCaryfrwrd).HasColumnType ("decimal(18, 2)");
            entity.Property (e => e.CreatedDate)
                .HasDefaultValueSql ("(getdate())")
                .HasColumnType ("datetime");
            entity.Property (e => e.CsectionMaxLeave).HasColumnType ("decimal(18, 2)");
            entity.Property (e => e.Lopcheck).HasColumnName ("LOPCheck");
            entity.Property (e => e.MinServiceDays).HasColumnType ("decimal(18, 2)");
            entity.Property (e => e.RolloverCount).HasColumnType ("decimal(18, 2)");
        });

        modelBuilder.Entity<HrmLeaveBasicsettingsDetailsHistory> (entity =>
        {
            entity
                .HasNoKey ( )
                .ToTable ("HRM_LEAVE_BASICSETTINGS_DETAILS_HISTORY");

            entity.Property (e => e.Ipaddress).HasColumnName ("IPAddress");
            entity.Property (e => e.SettingsHistoryId).ValueGeneratedOnAdd ( );
            entity.Property (e => e.UpdatedDate).HasColumnType ("datetime");
        });


    }
    }