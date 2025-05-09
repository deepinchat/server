using AutoMapper;
using Deepin.Application.DTOs.Files;
using Deepin.Application.Helpers;
using Deepin.Application.Interfaces;
using Deepin.Application.Queries;
using Deepin.Domain.FileAggregate;
using HeyRed.Mime;
using MediatR;

namespace Deepin.Application.Commands.Files;
public class UploadFileCommandHandler(IMapper mapper, IFileStorage fileStorage, IFileRepository fileRepository, IFileQueries fileQueries) : IRequestHandler<UploadFileCommand, FileDto>
{
    public async Task<FileDto> Handle(UploadFileCommand request, CancellationToken cancellationToken)
    {
        var hash = await FileHelper.GetSHA256HashAsync(request.FileStream, cancellationToken);
        var existingFile = await fileQueries.GetByHashAsync(hash, cancellationToken);
        if (existingFile is not null)
        {
            return existingFile;
        }
        var checksum = FileHelper.GetCRC32Checksum(request.FileStream, cancellationToken);
        var fileId = Guid.NewGuid();
        var storageKey = request.StorageKey ?? await fileStorage.BuildStorageKeyAsync(fileId, request.FileName, request.ContainerName);
        var file = new FileObject(
            id: fileId,
            uploaderUserId: request.UserId,
            name: request.FileName,
            storageKey: storageKey,
            contentType: MimeTypesMap.GetMimeType(request.FileName),
            length: request.FileStream.Length,
            containerName: request.ContainerName,
            hash: hash,
            checksum: checksum,
            format: Path.GetExtension(request.FileName),
            provider: fileStorage.Provider
        );
        await fileRepository.AddAsync(file, cancellationToken);
        var result = mapper.Map<FileDto>(file);
        await fileRepository.UnitOfWork.ExecuteInTransactionAsync(
            async () =>
            {
                await fileRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
                await fileStorage.CreateAsync(result, request.FileStream);
            },
            cancellationToken
        );
        return result;
    }
}