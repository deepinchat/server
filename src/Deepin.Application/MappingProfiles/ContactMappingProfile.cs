using System;
using AutoMapper;
using Deepin.Application.DTOs.Contacts;
using Deepin.Domain.ContactAggregate;

namespace Deepin.Application.MappingProfiles;

public class ContactMappingProfile : Profile
{
    public ContactMappingProfile()
    {
        CreateMap<Contact, ContactDto>();
    }
}
