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

        }
    }