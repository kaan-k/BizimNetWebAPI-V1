using AutoMapper;
using Entities.Concrete.Aggrements;
using Entities.Concrete.Customers;
using Entities.Concrete.Devices;
using Entities.Concrete.DocumentFile;
using Entities.Concrete.Duties;
using Entities.Concrete.InstallationRequests;
using Entities.Concrete.Offer;
using Entities.Concrete.Offers;
using Entities.Concrete.Payments;
using Entities.Concrete.Services;
using Entities.Concrete.Settings;
using Entities.Concrete.Stocks;

namespace Entities.Profiles.AutoMapperProfiles
{
    public class EntitiesAutoMapperProfile : Profile
    {
        public EntitiesAutoMapperProfile()
        {
            CreateMap<DocumentFileUploadDto, DocumentFile>();
            CreateMap<DocumentFileAddRequest, DocumentFile>();
            CreateMap<DocumentFileUpdateRequest, DocumentFile>();
            CreateMap<BillingDto, Billing>();
            CreateMap<AggrementDto, Aggrement>();
            CreateMap<OfferDto, Offer>();
            CreateMap<InstallationRequestDto, InstallationRequest>();
            CreateMap<InstallationRequest, InstallationRequest>();
            CreateMap<DeviceDto, Device>();
            CreateMap<ServicingAddDto, Servicing>();
            CreateMap<StockAddDto, Stock>();
            CreateMap<CustomerDto, Customer>();
            CreateMap<DutyDto, Duty>();
            CreateMap<AgGridSettingsDto, AgGridSettings>();


        }
    }
}
