using AutoMapper;
using Deepin.Application.DTOs.Messages;
using Deepin.Domain.MessageAggregate;
using Newtonsoft.Json;

namespace Deepin.Application.MappingProfiles;

public class MessageMappingProfile : Profile
{
    public MessageMappingProfile()
    {
        CreateMap<Message, MessageDto>()
            .ForMember(dest => dest.Mentions, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.Mentions) ? new List<MessageMentionDto>() : JsonConvert.DeserializeObject<List<MessageMentionDto>>(src.Mentions)));
        CreateMap<MessageAttachment, MessageAttachmentDto>();
        CreateMap<MessageReaction, MessageReactionDto>();
    }
}
