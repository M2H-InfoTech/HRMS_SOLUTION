using System;
using System.Collections.Generic;

namespace HRMS.EmployeeInformation.Models;

public partial class EntityLevelFive
{
    public int LevelOneId { get; set; }

    public string? LevelOneDescription { get; set; }

    public int LevelTwoId { get; set; }

    public string? LevelTwoDescription { get; set; }

    public int LevelThreeId { get; set; }

    public string? LevelThreeDescription { get; set; }

    public int LevelFourId { get; set; }

    public string? LevelFourDescription { get; set; }

    public int LevelFiveId { get; set; }

    public string? LevelFiveDescription { get; set; }

    public int LinkableSubcategory { get; set; }

    public long? Root { get; set; }
}
