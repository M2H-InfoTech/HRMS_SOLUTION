using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OFFICEKITCORELEAVE.OfficeKit.Leave.Data;
using OFFICEKITCORELEAVE.OfficeKitHR.Leave.DTO;
using OFFICEKITCORELEAVE.OfficeKitHR.Leave.Interface.HrmLeaveBasicSettingInterface;
using OFFICEKITCORELEAVE.OfficeKitHR.Leave.MODELS;

namespace OFFICEKITCORELEAVE.OfficeKitHR.Leave.SERVICE.HrmLeaveBasicSettings
    {
    public class HrmLeaveBasicSettingService : IHrmLeaveBasicSettingService
        {
            private readonly LeaveDBContext _leavedbContext;
            private readonly IMapper _mapper;
            public HrmLeaveBasicSettingService(LeaveDBContext leavedbContext_,IMapper mapper_)
            {
                _leavedbContext = leavedbContext_;
                _mapper = mapper_;
            }

            public Task<int> HrmLeaveBasicSettingSave (HrmLeaveBasicSettingDto dto)
            {
                var HRMBasicSettings = _mapper.Map<HrmLeaveBasicSetting> (dto);
                _leavedbContext.HrmLeaveBasicSettings.Add(HRMBasicSettings);
                _leavedbContext.SaveChanges();
                return Task.FromResult(HRMBasicSettings.SettingsId);
            }
            public Task<HrmLeaveBasicSettingDto> GetLeaveBasicSettingsById (int leavebasicSettingsId)
            {
                var BasicSettings = _leavedbContext.HrmLeaveBasicSettings.FindAsync (leavebasicSettingsId);
                if (BasicSettings == null)
                {
                    return null;
                }
                else
                {
                    var returnSettings = _mapper.Map<HrmLeaveBasicSettingDto>(BasicSettings);
                    return Task.FromResult(returnSettings);
                }
            }
            public Task<bool> DeleteBasicSettings (int leavebasicSettingsId)
            {
                bool flag = false;
                var DeleteData = GetLeaveBasicSettingsById(leavebasicSettingsId);
                if (DeleteData == null)
                {
                    flag = false;
                    return Task.FromResult(flag);
                }
                else
                {
                    var SettingToDelete = _mapper.Map<HrmLeaveBasicSetting>(DeleteData);
                    _leavedbContext.HrmLeaveBasicSettings.Remove (SettingToDelete);
                    _leavedbContext.SaveChangesAsync ();
                    flag = true;
                    return Task.FromResult (flag);
                }
                
            }
            public async Task<List<HrmLeaveBasicSettingDto>> GetAllLeaveBasicSettings ( )
            {
                var settingsList = await _leavedbContext.HrmLeaveBasicSettings.ToListAsync ( );

                if (settingsList == null || !settingsList.Any ( ))
                {
                    return new List<HrmLeaveBasicSettingDto> ( ); 
                }

                return _mapper.Map<List<HrmLeaveBasicSettingDto>> (settingsList);
            }


        }
    }
