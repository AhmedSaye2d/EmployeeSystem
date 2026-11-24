using Application.Dtos.Request.Country;
using Application.Helper;
using Application.Service.Country.Commands;
using Application.Service.Country.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Host.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = Constants.Admin)]
    public class CountryController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CountryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// جلب جميع الدول
        /// </summary>
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetCountriesQuery());
            return Ok(result);
        }

        /// <summary>
        /// جلب تفاصيل دولة محددة برقمها
        /// </summary>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetCountryByIdQuery(id));
            if (result is null)
                return NotFound(new { message = "Country not found" });

            return Ok(result);
        }

        /// <summary>
        /// إضافة دولة جديدة
        /// </summary>
        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] AddCountryRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _mediator.Send(new CreateCountryCommand(request));
            return result.Flag ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// تعديل دولة مسجلة
        /// </summary>
        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] UpdateCountryRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _mediator.Send(new UpdateCountryCommand(request));
            return result.Flag ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// حذف دولة برقمها
        /// </summary>
        [HttpDelete("delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteCountryCommand(id));
            return result.Flag ? Ok(result) : BadRequest(result);
        }
    }
}
