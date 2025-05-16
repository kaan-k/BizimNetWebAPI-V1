using AutoMapper;
using Entities.Concrete;
using Entities.Concrete.DocumentFiles;


namespace Entities.Profiles.AutoMapperProfiles
{
    public class EntitiesAutoMapperProfile : Profile
    {
        public EntitiesAutoMapperProfile()
        {
            CreateMap<DocumentFileUploadDto, DocumentFile>();
        }
    }
}
