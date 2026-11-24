using Application.Dtos.Request.Vacation;
using Application.Helper;
using Application.Service.Vacation.Commands;
using Application.Service.Vacation.Queries;
using Domain.Entities.Enum;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Host.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = Constants.Admin)]
    public class VacationController : ControllerBase
    {
        private readonly IMediator _mediator;

        public VacationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// جلب جميع الإجازات مع تفاصيل الموظف ونوع الإجازة
        /// </summary>
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetVacationsQuery());
            return Ok(result);
        }

        /// <summary>
        /// جلب تفاصيل إجازة محددة برقمها
        /// </summary>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetVacationByIdQuery(id));
            if (result is null)
                return NotFound(new { message = "Vacation not found" });

            return Ok(result);
        }

        /// <summary>
        /// جلب جميع إجازات موظف محدد برقم الموظف
        /// </summary>
        [HttpGet("employee/{employeeId:int}")]
        public async Task<IActionResult> GetByEmployeeId(int employeeId)
        {
            var result = await _mediator.Send(new GetVacationsByEmployeeIdQuery(employeeId));
            return Ok(result);
        }

        /// <summary>
        /// جلب جميع طلبات الإجازات المعلقة (Pending) في انتظار مراجعة المسؤول
        /// </summary>
        [HttpGet("pending")]
        public async Task<IActionResult> GetPending()
        {
            var result = await _mediator.Send(new GetPendingVacationsQuery());
            return Ok(result);
        }

        /// <summary>
        /// تقديم طلب إجازة جديد (يتم حفظه تلقائياً بحالة Pending)
        /// </summary>
        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] AddVacationRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _mediator.Send(new CreateVacationCommand(request));
            return result.Flag ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// مراجعة طلب إجازة واتخاذ قرار بالقبول أو الرفض
        /// </summary>
        [HttpPut("review")]
        public async Task<IActionResult> Review([FromBody] ReviewVacationRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var reviewer = User.Identity?.Name 
                           ?? User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value 
                           ?? "Admin";

            var result = await _mediator.Send(new ReviewVacationCommand(request, reviewer));
            return result.Flag ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// قبول طلب الإجازة سريعاً
        /// </summary>
        [HttpPut("{id:int}/approve")]
        public async Task<IActionResult> Approve(int id, [FromQuery] string? reason = null)
        {
            var reviewer = User.Identity?.Name 
                           ?? User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value 
                           ?? "Admin";

            var request = new ReviewVacationRequest
            {
                VacationId = id,
                Decision = VacationStatus.Approved,
                Reason = reason
            };

            var result = await _mediator.Send(new ReviewVacationCommand(request, reviewer));
            return result.Flag ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// رفض طلب الإجازة سريعاً مع سبب الرفض
        /// </summary>
        [HttpPut("{id:int}/reject")]
        public async Task<IActionResult> Reject(int id, [FromQuery] string? reason = null)
        {
            var reviewer = User.Identity?.Name 
                           ?? User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value 
                           ?? "Admin";

            var request = new ReviewVacationRequest
            {
                VacationId = id,
                Decision = VacationStatus.Rejected,
                Reason = reason
            };

            var result = await _mediator.Send(new ReviewVacationCommand(request, reviewer));
            return result.Flag ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// تعديل إجازة مسجلة
        /// </summary>
        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] UpdateVacationRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _mediator.Send(new UpdateVacationCommand(request));
            return result.Flag ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// حذف إجازة برقمها
        /// </summary>
        [HttpDelete("delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteVacationCommand(id));
            return result.Flag ? Ok(result) : BadRequest(result);
        }
    }
}
