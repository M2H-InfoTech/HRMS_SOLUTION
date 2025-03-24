using AutoMapper;
using OFFICEKITCORELEAVE.OfficeKit.Leave.DTO;
using OFFICEKITCORELEAVE.OfficeKit.Leave.MODELS;
using OFFICEKITCORELEAVE.OfficeKitHR.Leave.DTO;
using OFFICEKITCORELEAVE.OfficeKitHR.Leave.MODELS;

namespace OFFICEKITCORELEAVE.OfficeKit.Leave.Mapping
    {
    public class LeaveMappings : Profile
        {
         public LeaveMappings()
            {
            CreateMap<HrmLeaveMaster, HrmLeaveMasterDTO> ( ).ReverseMap();
            CreateMap<HrmLeaveBasicSetting,HrmLeaveBasicSettingDto>().ReverseMap();
            }
        }
    }
