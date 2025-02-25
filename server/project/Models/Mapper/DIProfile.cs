using AutoMapper;
using project.DAL;
using project.Models.DTO;
using static System.Net.Mime.MediaTypeNames;

namespace project.Models.Mapper
{
    public class DIProfile : Profile
    {
        public DIProfile()
        {
            CreateMap<DonorDTO, Donor>();
            CreateMap<GiftDTO, Gift>()
                .ForMember(dest => dest.WinnerId, opt => opt.MapFrom(src => (int?)null));
            CreateMap<CategoryDTO, Category>();
            CreateMap<CustomerDTO, Customer>()
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => "Customer"));
            CreateMap<PurchaseDTO, Purchase>();
        }
    }
    //Manager
}
