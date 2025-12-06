using AutoMapper;
using Entities.Concrete.Aggrements;
using Entities.Concrete.Customers;
using Entities.Concrete.Devices;
using Entities.Concrete.DocumentFile;
using Entities.Concrete.Duties;
using Entities.Concrete.InstallationRequests;
using Entities.Concrete.Offers;
using Entities.Concrete.Payments;
using Entities.Concrete.Services;
using Entities.Concrete.Settings;
using Entities.Concrete.Stocks;
using Entities.DTOs.BillingDtos;

namespace Entities.Profiles.AutoMapperProfiles
{
    public class EntitiesAutoMapperProfile : Profile
    {
        public EntitiesAutoMapperProfile()
        {
            // CUSTOMER
            CreateMap<Customer, CustomerDto>().ReverseMap();

            // BILLING
            CreateMap<Billing, BillingDto>().ReverseMap();

            // ---------------------------
            // OFFER → DTO
            // ---------------------------
            CreateMap<Offer, OfferDto>()
                .ForMember(dest => dest.CustomerName,
                    opt => opt.MapFrom(src => src.Customer.CompanyName))
                .ForMember(dest => dest.Items,
                    opt => opt.MapFrom(src => src.Items));

            // ---------------------------
            // DTO → OFFER
            // ---------------------------
            CreateMap<OfferDto, Offer>()
                .ForMember(dest => dest.Customer, opt => opt.Ignore())   // Navigation ignore
                .ForMember(dest => dest.Items, opt => opt.Ignore());      // Items manual added later

            // OFFER ITEM
            CreateMap<OfferItem, OfferItemDto>().ReverseMap();

            // AGREEMENT
            CreateMap<AggrementDto, Aggrement>()
                .ForMember(dest => dest.Customer, opt => opt.Ignore())
                .ForMember(dest => dest.Offer, opt => opt.Ignore())
                .ForMember(dest => dest.Billings, opt => opt.Ignore());

            CreateMap<Aggrement, AggrementDto>()
                .ForMember(dest => dest.CustomerName,
                    opt => opt.MapFrom(src => src.Customer.CompanyName))
                .ForMember(dest => dest.Billings,
                    opt => opt.MapFrom(src => src.Billings))
                .ForMember(dest => dest.Offer,
                    opt => opt.MapFrom(src => src.Offer));

            // MISC
            CreateMap<Device, DeviceDto>().ReverseMap();
            CreateMap<InstallationRequest, InstallationRequestDto>().ReverseMap();
            CreateMap<Servicing, ServicingAddDto>().ReverseMap();
            CreateMap<Stock, StockAddDto>().ReverseMap();
            CreateMap<Duty, DutyDto>().ReverseMap();
            CreateMap<AgGridSettings, AgGridSettingsDto>().ReverseMap();
        }
    }
}
