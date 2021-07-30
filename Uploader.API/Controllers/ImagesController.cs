using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Uploader.Application.Common.Models;
using Uploader.Application.Images.Commands.CreateImage;
using Uploader.Application.Images.Commands.DeleteImage;
using Uploader.Application.Images.Commands.SoftDeleteImage;
using Uploader.Application.Images.Commands.UpdateImageDescription;
using Uploader.Application.Images.Queries.GetImagesWithPagination;
using Uploader.Domain.Entities;

namespace Uploader.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImagesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ImagesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get all images
        /// </summary>
        /// <param name="query"></param>
        /// <returns>A paginated list of all images</returns>
        [HttpGet]
        public async Task<ActionResult<PaginatedList<Image>>> GetImages([FromQuery] GetImagesWithPaginationQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Create an image
        /// </summary>
        /// <param name="command"></param>
        /// <returns>The id of the created image</returns>
        [HttpPost]
        public async Task<IActionResult> CreateImage([FromForm] CreateImageCommand command)
        {
            var createdId = await _mediator.Send(command);
            return Ok(new {Id = createdId});
        }

        /// <summary>
        /// Update description for an image with the given id
        /// </summary>
        /// <param name="id">Id of the image to be updated</param>
        /// <param name="command"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Hard delete an image
        /// </summary>
        /// <param name="id">Id of the image to be deleted</param>
        /// <returns></returns>
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteImage(Guid id)
        {
            await _mediator.Send(new DeleteImageCommand {Id = id});
            return NoContent();
        }

        /// <summary>
        /// Soft delete an image
        /// </summary>
        /// <param name="id">Id of the image to be soft deleted</param>
        /// <returns></returns>
        [HttpPost("{id:guid}/softDelete")]
        public async Task<IActionResult> SoftDeleteImage(Guid id)
        {
            await _mediator.Send(new SoftDeleteImageCommand {Id = id});
            return NoContent();
        }
    }
}