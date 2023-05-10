using AutoMapper;
using Vehicle_API.Model;
using Vehicle_API.DTO;

namespace Vehicle_API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
             CreateMap<Vehicle, VehicleDTO>().ReverseMap();
        }
    }
}
