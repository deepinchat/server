using AutoMapper;
using Deepin.Application.Models.Chats;
using Deepin.Domain.ChatAggregate;

namespace Deepin.Application.MappingProfiles;

public class ChatMappingProfile : Profile
{
    public ChatMappingProfile()
    {
        CreateMap<Chat, ChatDto>(MemberList.Destination);
        CreateMap<GroupInfo, ChatInfo>(MemberList.Destination);
        CreateMap<ChatMember, ChatMemberDto>(MemberList.Destination);
    }
}
