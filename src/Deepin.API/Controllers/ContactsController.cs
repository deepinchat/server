using Deepin.Application.Commands.Contacts;
using Deepin.Application.DTOs.Contacts;
using Deepin.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Deepin.API.Controllers
{
    public class ContactsController(
        IMediator mediator,
        IContactQueries contactQueries) : ApiControllerBase
    {
        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(Guid id, CancellationToken cancellationToken)
        {
            var contact = await contactQueries.GetByIdAsync(id, cancellationToken);
            if (contact == null)
            {
                return NotFound();
            }
            return Ok(contact);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateContactRequest request, CancellationToken cancellationToken)
        {
            var contact = await mediator.Send(new CreateContactCommand(request), cancellationToken);
            if (contact == null)
            {
                return BadRequest("Failed to create contact.");
            }
            return CreatedAtAction(nameof(Get), new { id = contact.Id }, contact);
        }

        [HttpPut("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateContactRequest request, CancellationToken cancellationToken)
        {
            var contact = await contactQueries.GetByIdAsync(id, cancellationToken);
            if (contact is null)
            {
                return NotFound();
            }
            var updatedContact = await mediator.Send(new UpdateContactCommand(id, request), cancellationToken);
            return Ok(updatedContact);
        }

        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new DeleteContactCommand(id), cancellationToken);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPaged(int offset, int limit, string? search = null, CancellationToken cancellationToken = default)
        {
            var contacts = await contactQueries.GetPagedAsync(offset, limit, search, cancellationToken);
            return Ok(contacts);
        }
    }
}
