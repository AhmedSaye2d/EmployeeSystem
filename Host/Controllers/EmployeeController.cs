using Application.Dtos.Request.Employee;
using Application.Helper;
using Application.Service.Employee.Commands;
using Application.Service.Employee.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Host.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = Constants.Admin)]
    public class EmployeeController : ControllerBase
    {
        private readonly IMediator _mediator;

        public EmployeeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetEmployeesQuery());
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetEmployeeByIdQuery(id));
            if (result is null)
                return NotFound(new { message = "Employee not found" });

            return Ok(result);
        }

     
        [HttpGet("search/{searchTerm}")]
        public async Task<IActionResult> Search(string searchTerm)
        {
            var result = await _mediator.Send(new SearchEmployeesQuery(searchTerm));
            return Ok(result);
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] AddEmployeeRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _mediator.Send(new CreateEmployeeCommand(request));
            return result.Flag ? Ok(result) : BadRequest(result);
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] UpdateEmployeeRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _mediator.Send(new UpdateEmployeeCommand(request));
            return result.Flag ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteEmployeeCommand(id));
            return result.Flag ? Ok(result) : BadRequest(result);
        }
    }
}
