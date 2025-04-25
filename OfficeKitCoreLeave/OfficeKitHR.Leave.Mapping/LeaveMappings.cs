using AutoMapper;
using EMPLOYEE_INFORMATION.Models.Entity;
using OFFICEKITCORELEAVE.OfficeKit.Leave.DTO;
using OFFICEKITCORELEAVE.OfficeKitHR.Leave.DTO;

namespace OFFICEKITCORELEAVE.OfficeKit.Leave.Mapping
{
    public class LeaveMappings : Profile
    {
        public LeaveMappings()
        {
            CreateMap<HrmLeaveMaster, HrmLeaveMasterDTO>().ReverseMap();
            CreateMap<HrmLeaveBasicSetting, HrmLeaveBasicSettingDto>().ReverseMap();
        }
    }
}
