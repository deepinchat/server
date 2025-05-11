using Deepin.API.Models.Files;
using Deepin.Application.Commands.Files;
using Deepin.Application.Interfaces;
using Deepin.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Deepin.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FilesController(IMediator mediator, IUserContext userContext, IFileQueries fileQueries) : ControllerBase
    {
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get(Guid id, CancellationToken cancellationToken)
        {
            var file = await fileQueries.GetByIdAsync(id, cancellationToken);
            if (file is null)
            {
                return NotFound();
            }
            return Ok(file);
        }

        [HttpGet("download/{id:guid}")]
        public async Task<IActionResult> Download(Guid id, CancellationToken cancellationToken)
        {
            var file = await fileQueries.GetByIdAsync(id, cancellationToken);
            if (file is null)
            {
                return NotFound();
            }
            var stream = await mediator.Send(new DownloadFileCommand(file.Id), cancellationToken);
            return File(stream, file.ContentType, file.Name);
        }
        [HttpPost]
        public async Task<IActionResult> Upload([FromForm] FileUploadRequest request, CancellationToken cancellationToken)
        {
            var command = new UploadFileCommand(
                FileStream: request.File.OpenReadStream(),
                FileName: request.File.FileName,
                UserId: userContext.UserId,
                ContainerName: request.ContainerName,
                StorageKey: request.StorageKey);
            var file = await mediator.Send(command, cancellationToken);
            if (file is null)
            {
                return BadRequest("File upload failed.");
            }
            return CreatedAtAction(nameof(Get), new { id = file.Id }, file);
        }
    }
}
