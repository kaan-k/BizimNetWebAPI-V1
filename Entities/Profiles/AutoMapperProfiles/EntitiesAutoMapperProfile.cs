using AutoMapper;
using Entities.Concrete.Customer;
using Entities.Concrete.Device;
using Entities.Concrete.DocumentFile;
using Entities.Concrete.InstallationRequest;
using Entities.Concrete.Offer;
using Entities.Concrete.Service;
using Entities.Concrete.Stock;

namespace Entities.Profiles.AutoMapperProfiles
{
    public class EntitiesAutoMapperProfile : Profile
    {
        public EntitiesAutoMapperProfile()
        {
            CreateMap<DocumentFileUploadDto, DocumentFile>();
            CreateMap<DocumentFileAddRequest, DocumentFile>();
            CreateMap<DocumentFileUpdateRequest, DocumentFile>();
            CreateMap<OfferDto, Offer>();
            CreateMap<InstallationRequestDto, InstallationRequest>();
            CreateMap<InstallationRequest, InstallationRequest>();
            CreateMap<DeviceDto, Device>();
            CreateMap<ServicingAddDto, Servicing>();
            CreateMap<StockAddDto, Stock>();
            CreateMap<CustomerDto, Customer>();

        }
    }
}
