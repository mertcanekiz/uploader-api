using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Uploader.Application.Common.Models;
using Uploader.Application.Images.Commands.CreateImage;
using Uploader.Application.Images.Commands.DeleteImage;
using Uploader.Application.Images.Commands.UpdateImageDescription;
using Uploader.Application.Images.Queries.GetImagesWithPagination;
using Uploader.Domain.Entities;

namespace Uploader.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImagesController : ControllerBase
    {
        private IMediator _mediator;

        public ImagesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<PaginatedList<Image>>> GetImages([FromQuery] GetImagesWithPaginationQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateImage([FromForm] CreateImageCommand command)
        {
            var createdId = await _mediator.Send(command);
            return Ok(new {Id = createdId});
        }

        [HttpPatch("{id:guid}/updateDescription")]
        public async Task<IActionResult> UpdateDescription(Guid id, [FromBody] UpdateImageDescriptionCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }
            
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteImage(Guid id)
        {
            await _mediator.Send(new DeleteImageCommand {Id = id});
            return NoContent();
        }
    }
}