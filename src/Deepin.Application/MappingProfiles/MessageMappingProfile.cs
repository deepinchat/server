using System;
using AutoMapper;
using Deepin.Application.DTOs.Messages;
using Deepin.Domain.MessageAggregate;

namespace Deepin.Application.MappingProfiles;

public class MessageMappingProfile : Profile
{
    public MessageMappingProfile()
    {
        CreateMap<Message, MessageDto>();
        CreateMap<MessageAttachment, MessageAttachmentDto>();
    }
}
