namespace HRMS.EmployeeInformation.Models.Models.Entity;

public partial class SubCategoryLinksNew
{
    public int LinkId { get; set; }

    public int? CategoryId { get; set; }

    public int? SubcategoryId { get; set; }

    public int? LinkableCategoryId { get; set; }

    public int? LinkableSubcategory { get; set; }

    public long? Root { get; set; }

    public long? LinkLevel { get; set; }

    public DateTime? EntryDate { get; set; }

    public int? LinkedEntityId { get; set; }
}
