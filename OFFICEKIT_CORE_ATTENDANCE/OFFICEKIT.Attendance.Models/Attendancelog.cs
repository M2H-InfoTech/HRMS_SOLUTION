using System;
using System.Collections.Generic;

namespace OFFICEKIT_CORE_ATTENDANCE.OFFICEKIT.Attendance.Models;

public partial class Attendancelog
{
    public long AttLogId { get; set; }

    public int DeviceLogId { get; set; }

    public DateTime? DownloadDate { get; set; }

    public int DeviceId { get; set; }

    public string UserId { get; set; } = null!;

    public int? EmployeeId { get; set; }

    public DateTime LogDate { get; set; }

    public string? Direction { get; set; }

    public string? AttDirection { get; set; }

    public string? C1 { get; set; }

    public string? C2 { get; set; }

    public string? C3 { get; set; }

    public string? C4 { get; set; }

    public string? C5 { get; set; }

    public string? C6 { get; set; }

    public string? C7 { get; set; }

    public string? WorkCode { get; set; }

    public int? IsManul { get; set; }

    public string? IsProcessed { get; set; }

    public DateTime? ProcessedDate { get; set; }

    public DateTime? UniversalLogDate { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public string? RequestSequenceId { get; set; }

    public int? MissingInOutId { get; set; }

    public string? Latitude { get; set; }

    public string? Longitude { get; set; }

    public string? LogInDeviceType { get; set; }

    public string? LogInDeviceName { get; set; }

    public int? GeolocationIdMob { get; set; }

    public string? LiveLocation { get; set; }

    public int? IsOfflinePunch { get; set; }
}
