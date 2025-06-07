using MediatR;

namespace Deepin.Application.Commands.Files;
public record DownloadFileCommand(Guid FileId) : IRequest<Stream>;