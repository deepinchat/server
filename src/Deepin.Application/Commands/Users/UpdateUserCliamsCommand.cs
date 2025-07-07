using AutoMapper;
using Deepin.Application.DTOs.Users;
using Deepin.Application.Requests.Users;
using Deepin.Domain.Exceptions;
using Deepin.Domain.Identity;
using MediatR;

namespace Deepin.Application.Commands.Users;

public record UpdateUserCliamsCommand(Guid Id, IEnumerable<UserCliamRequest> UserCliams) : IRequest<UserDto>;
public class UpdateUserCliamsCommandHandler(IMapper mapper, IUserRepository userRepository) : IRequestHandler<UpdateUserCliamsCommand, UserDto>
{
    public async Task<UserDto> Handle(UpdateUserCliamsCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.FindByIdAsync(request.Id, cancellationToken);
        if (user == null)
        {
            throw new EntityNotFoundException($"User with ID {request.Id} not found.");
        }
        user.ClearClaims();
        if (request.UserCliams != null && request.UserCliams.Any())
        {
            foreach (var claim in request.UserCliams)
            {
                user.AddClaims(claim.ClaimType, claim.ClaimValue);
            }
        }
        await userRepository.UpdateAsync(user, cancellationToken);
        await userRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
        return mapper.Map<UserDto>(user);
    }
}
