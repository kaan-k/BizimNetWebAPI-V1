using AutoMapper;
using Entities.Concrete.DocumentFile;
using Entities.Concrete.Offer;

namespace Entities.Profiles.AutoMapperProfiles
{
    public class EntitiesAutoMapperProfile : Profile
    {
        public EntitiesAutoMapperProfile()
        {
            CreateMap<DocumentFileUploadDto, DocumentFile>();
            CreateMap<OfferDto, Offer>();
        }
    }
}
