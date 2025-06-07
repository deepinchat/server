using AutoMapper;
using Deepin.Application.DTOs.Files;
using Deepin.Domain.FileAggregate;

namespace Deepin.Application.MappingProfiles;

public class FileMappingProfile : Profile
{
    public FileMappingProfile()
    {
        CreateMap<FileObject, FileDto>();
    }
}
