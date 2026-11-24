using Application.Dtos.Request.Branch;
using Application.Helper;
using Application.Service.Branch.Commands;
using Application.Service.Branch.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Host.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = Constants.Admin)]
    public class BranchController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BranchController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// جلب جميع الفروع مع تفاصيل القسم والإدارة العامة
        /// </summary>
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetBranchesQuery());
            return Ok(result);
        }

        /// <summary>
        /// جلب تفاصيل فرع محدد بالـ Id
        /// </summary>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetBranchByIdQuery(id));
            if (result is null)
                return NotFound(new { message = "Branch not found" });

            return Ok(result);
        }

        /// <summary>
        /// إضافة فرع جديد
        /// </summary>
        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] AddBranchRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _mediator.Send(new CreateBranchCommand(request));
            return result.Flag ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// تعديل فرع مسجل
        /// </summary>
        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] UpdateBranchRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _mediator.Send(new UpdateBranchCommand(request));
            return result.Flag ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// حذف فرع بالـ Id
        /// </summary>
        [HttpDelete("delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteBranchCommand(id));
            return result.Flag ? Ok(result) : BadRequest(result);
        }
    }
}
