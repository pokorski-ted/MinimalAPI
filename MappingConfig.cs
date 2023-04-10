using AutoMapper;
using MinimalAPI.DTO;
using MinimalAPI.Models;

namespace MinimalAPI
{
    public class MappingConfig : Profile
    {

        public MappingConfig()
        {
            CreateMap<Coupon, CouponCreateDTO>().ReverseMap();
        }

    }
}
