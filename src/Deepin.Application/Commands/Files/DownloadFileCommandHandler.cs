using Deepin.Application.Interfaces;
using Deepin.Application.Queries;
using MediatR;

namespace Deepin.Application.Commands.Files;

public class DownloadFileCommandHandler(IFileQueries fileQueries, IFileStorage fileStorage) : IRequestHandler<DownloadFileCommand, Stream>
{
    public async Task<Stream> Handle(DownloadFileCommand request, CancellationToken cancellationToken)
    {
        var file = await fileQueries.GetByIdAsync(request.FileId, cancellationToken);
        if (file is null)
        {
            throw new FileNotFoundException($"File with ID {request.FileId} not found.");
        }

        return await fileStorage.GetStreamAsync(file, cancellationToken);
    }
}
