using Deepin.Application.DTOs.Files;
using MediatR;

namespace Deepin.Application.Commands.Files;

public record UploadFileCommand(Stream FileStream, string FileName, Guid UserId, string? ContainerName = null, string? StorageKey = null) : IRequest<FileDto>;