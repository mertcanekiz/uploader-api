using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Uploader.Application.Common.Models;
using Uploader.Application.Images.Commands.CreateImage;
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
    }
}