using AutoMapper;
using Deepin.Application.DTOs.Chats;
using Deepin.Domain.ChatAggregate;

namespace Deepin.Application.MappingProfiles;

public class ChatMappingProfile : Profile
{
    public ChatMappingProfile()
    {
        CreateMap<DirectChat, DirectChatDto>(MemberList.Destination);
        CreateMap<GroupChat, GroupChatDto>(MemberList.Destination);
        CreateMap<ChatMember, ChatMemberDto>(MemberList.Destination);
    }
}
