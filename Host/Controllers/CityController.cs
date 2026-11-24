using Application.Dtos.Request.City;
using Application.Helper;
using Application.Service.City.Commands;
using Application.Service.City.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Host.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = Constants.Admin)]
    public class CityController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CityController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// جلب جميع المدن مع تفاصيل الدولة
        /// </summary>
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetCitiesQuery());
            return Ok(result);
        }

        /// <summary>
        /// جلب تفاصيل مدينة محددة برقمها
        /// </summary>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetCityByIdQuery(id));
            if (result is null)
                return NotFound(new { message = "City not found" });

            return Ok(result);
        }

        /// <summary>
        /// إضافة مدينة جديدة
        /// </summary>
        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] AddCityRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _mediator.Send(new CreateCityCommand(request));
            return result.Flag ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// تعديل مدينة مسجلة
        /// </summary>
        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] UpdateCityRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _mediator.Send(new UpdateCityCommand(request));
            return result.Flag ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// حذف مدينة برقمها
        /// </summary>
        [HttpDelete("delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteCityCommand(id));
            return result.Flag ? Ok(result) : BadRequest(result);
        }
    }
}
