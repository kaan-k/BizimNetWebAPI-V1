using AutoMapper;
using Entities.Concrete.Device;
using Entities.Concrete.DocumentFile;
using Entities.Concrete.InstallationRequest;
using Entities.Concrete.Offer;

namespace Entities.Profiles.AutoMapperProfiles
{
    public class EntitiesAutoMapperProfile : Profile
    {
        public EntitiesAutoMapperProfile()
        {
            CreateMap<DocumentFileUploadDto, DocumentFile>();
            CreateMap<OfferDto, Offer>();
            CreateMap<InstallationRequestDto, InstallationRequest>();
            CreateMap<InstallationRequest, InstallationRequest>();
            CreateMap<DeviceDto, Device>();
        }
    }
}
