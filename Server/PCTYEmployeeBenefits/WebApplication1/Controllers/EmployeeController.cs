using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PCTYLibrary.Constants;
using PCTYLibrary.Queries;
using PCTYLibrary.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        public EmployeeController(IMediator mediator , IConfiguration configuration)
        {
            _mediator = mediator;
            _configuration = configuration;
        }

        private readonly IMediator _mediator;
        private readonly IConfiguration _configuration;
        // GET: api/<EmployeeController>
        [Authorize(AuthenticationSchemes
           = AuthSchemeConstants.CustomAuthScheme)]
        [HttpGet(Name = "GetAllEmployees")]
        public async Task<IActionResult> Get()
        {
            return Ok(await _mediator.Send(new GetAllEmployeeQuery()));
            
        }

        // GET api/<EmployeeController>/5
        [Authorize(AuthenticationSchemes
           = AuthSchemeConstants.CustomAuthScheme)]
        [HttpGet("GetEmployeeBenefitsCost")]
        public async Task<IActionResult> Get(int employeeId)
        {
            return Ok(await _mediator.Send(new GetEmployeeBenefitsCostQuery() { EmployeeId = employeeId}));
        }        
    }
}
