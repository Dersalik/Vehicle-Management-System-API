using AutoMapper;
using Maintenance_API.Model;
using Maintenance_API.DTO;

namespace Maintenance_API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
             CreateMap<MaintenanceRecord, MaintenanceRecordDTO>().ReverseMap();
            CreateMap<MaintenanceRecord, MaintenanceRecordPostDTO>().ReverseMap();

        }
    }
}
