using AutoMapper;
using DiscountgRPC.Entities;
using DiscountgRPC.Protos;

namespace DiscountgRPC.Mapper
{
    public class DiscountProfile : Profile
    {
        public DiscountProfile()
        {
            CreateMap<Coupon, CouponModel>().ReverseMap();
        }
    }
}
