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
using Entities.Concrete.Warehouses;
using Entities.DTOs.BillingDtos;

namespace Entities.Profiles.AutoMapperProfiles
{
    public class EntitiesAutoMapperProfile : Profile
    {
        private static DateTime? ToUtc(DateTime? value)
        {
            if (value == null) return null;
            if (value.Value.Kind == DateTimeKind.Utc) return value;
            return DateTime.SpecifyKind(value.Value, DateTimeKind.Utc);
        }

        public EntitiesAutoMapperProfile()
        {
            // -----------------------------
            // CUSTOMER
            // -----------------------------
            CreateMap<Customer, CustomerDto>().ReverseMap();


            // -----------------------------
            // BILLING  (FIXED — CLEAN SINGLE MAP)
            // -----------------------------
            CreateMap<Billing, BillingDto>().ReverseMap()
                .ForMember(dest => dest.Customer, opt => opt.Ignore())
                .ForMember(dest => dest.Agreement, opt => opt.Ignore());


            // -----------------------------
            // OFFER → DTO
            // -----------------------------
            CreateMap<Offer, OfferDto>()
                .ForMember(dest => dest.CustomerName,
                    opt => opt.MapFrom(src => src.Customer.CompanyName))
                .ForMember(dest => dest.Items,
                    opt => opt.MapFrom(src => src.Items));

            // -----------------------------
            // DTO → OFFER
            // -----------------------------
            CreateMap<OfferDto, Offer>()
                .ForMember(dest => dest.Customer, opt => opt.Ignore())
                .ForMember(dest => dest.Items, opt => opt.Ignore());


            // OFFER ITEM
            CreateMap<OfferItem, OfferItemDto>();
            CreateMap<OfferItemDto, OfferItem>()
                .ForMember(dest => dest.OfferId, opt => opt.Ignore());


            // -----------------------------
            // AGREEMENT → DTO
            // -----------------------------
            CreateMap<Aggrement, AggrementDto>()
                .ForMember(dest => dest.CustomerName,
                    opt => opt.MapFrom(src => src.Customer.CompanyName))
                .ForMember(dest => dest.Offer,
                    opt => opt.MapFrom(src => src.Offer))
                .ForMember(dest => dest.Billings,
                    opt => opt.MapFrom(src => src.Billings));


            //Warehouse

            CreateMap<Warehouse, WarehouseAddDto>();

            CreateMap<Warehouse, WarehouseAddDto>().ReverseMap();


            // -----------------------------
            // DTO → AGREEMENT
            // -----------------------------
            CreateMap<AggrementDto, Aggrement>()
                .ForMember(dest => dest.Customer, opt => opt.Ignore())
                .ForMember(dest => dest.Offer, opt => opt.Ignore())
                .ForMember(dest => dest.Billings, opt => opt.Ignore());


            // -----------------------------
            // DUTY → DTO
            // -----------------------------
            CreateMap<Duty, DutyDto>()
                .ForMember(dest => dest.CustomerName,
                    opt => opt.MapFrom(src => src.Customer.CompanyName))
                .ForMember(dest => dest.AssignedEmployeeName,
                    opt => opt.MapFrom(src =>
                        src.AssignedEmployee != null
                        ? src.AssignedEmployee.FirstName + " " + src.AssignedEmployee.LastName
                        : null
                ));

            // -----------------------------
            // DTO → DUTY
            // -----------------------------
            CreateMap<DutyDto, Duty>()
                .ForMember(dest => dest.Customer, opt => opt.Ignore())
                .ForMember(dest => dest.Deadline,
                    opt => opt.MapFrom(src => ToUtc(src.Deadline)))
                .ForMember(dest => dest.BeginsAt,
                    opt => opt.MapFrom(src => ToUtc(src.BeginsAt)))
                .ForMember(dest => dest.EndsAt,
                    opt => opt.MapFrom(src => ToUtc(src.EndsAt)))
                .ForMember(dest => dest.UpdatedAt,
                    opt => opt.MapFrom(src => ToUtc(src.UpdatedAt)))
                .ForMember(dest => dest.CompletedAt,
                    opt => opt.MapFrom(src => ToUtc(src.CompletedAt)))
                .ForMember(dest => dest.SignatureBase64,
                    opt => opt.MapFrom(src => src.SignatureBase64));


            // -----------------------------
            // OTHER
            // -----------------------------
            CreateMap<Device, DeviceDto>().ReverseMap();
            CreateMap<InstallationRequest, InstallationRequestDto>().ReverseMap();
            CreateMap<Servicing, ServicingAddDto>().ReverseMap();
            CreateMap<Stock, StockAddDto>().ReverseMap();
            CreateMap<AgGridSettings, AgGridSettingsDto>().ReverseMap();
        }
    }
}
