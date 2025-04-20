namespace HRMS.EmployeeInformation.DTO.DTOs
{
    public class EntityRoleRequestDto
    {
        public int RoleId { get; set; }
        public int EntityLimit { get; set; }
        public List<EntityDetailDto> CustomEntityList { get; set; }
    }
}
