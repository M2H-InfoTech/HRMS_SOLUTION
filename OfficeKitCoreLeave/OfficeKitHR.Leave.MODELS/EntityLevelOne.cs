using System;
using System.Collections.Generic;

namespace OFFICEKITCORELEAVE.OfficeKitHR.Leave.MODELS;

public partial class EntityLevelOne
{
    public int LevelOneId { get; set; }

    public string? LevelOneCode { get; set; }

    public int LinkableSubcategory { get; set; }

    public string? LevelOneDescription { get; set; }
}
