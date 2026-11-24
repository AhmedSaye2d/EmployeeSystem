using Application.Dtos.Request.VacationType;
using Application.Helper;
using Application.Service.VacationType.Commands;
using Application.Service.VacationType.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Host.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = Constants.Admin)]
    public class VacationTypeController : ControllerBase
    {
        private readonly IMediator _mediator;

        public VacationTypeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// جلب جميع أنواع الإجازات
        /// </summary>
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetVacationTypesQuery());
            return Ok(result);
        }

        /// <summary>
        /// جلب تفاصيل نوع إجازة محدد برقمه
        /// </summary>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetVacationTypeByIdQuery(id));
            if (result is null)
                return NotFound(new { message = "Vacation type not found" });

            return Ok(result);
        }

        /// <summary>
        /// إضافة نوع إجازة جديد
        /// </summary>
        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] AddVacationTypeRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _mediator.Send(new CreateVacationTypeCommand(request));
            return result.Flag ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// تعديل نوع إجازة مسجل
        /// </summary>
        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] UpdateVacationTypeRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _mediator.Send(new UpdateVacationTypeCommand(request));
            return result.Flag ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// حذف نوع إجازة برقمه
        /// </summary>
        [HttpDelete("delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteVacationTypeCommand(id));
            return result.Flag ? Ok(result) : BadRequest(result);
        }
    }
}
