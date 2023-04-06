using Employee.Data.Forms;
using Employee.Services.AppServices.EmployeeAppService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Employee.Api.Controllers;

[Route("api/employee")]
public class EmployeeController : ApiController
{
    private readonly IEmployeeService _iEmployeeService;

    public EmployeeController(IEmployeeService iEmployeeService)
    {
        _iEmployeeService = iEmployeeService;
    }

    [HttpGet("{id::int}")]
    public IActionResult GetOne(int id)
        => Ok(_iEmployeeService.GetEmployeeById(id));

    [HttpGet("search")]
    public IActionResult Get([FromQuery] string searchString)
        => Ok(_iEmployeeService.GetEmployeeBySearchValue(searchString));

    [HttpGet]
    public IActionResult GetAll()
        => Ok(_iEmployeeService.GetAllEmployees());

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Add([FromBody] CreateEmployeeForm createEmployeeForm)
    {
        var result = await _iEmployeeService.AddEmployee(createEmployeeForm);
        return result.StatusCode switch
        {
            400 => BadRequest(result),
            _ => Ok(result)
        };
    }

    [HttpPut]
    [Authorize]
    public async Task<IActionResult> Edit([FromBody] UpdateEmployeeForm updateEmployeeForm)
    {
        var result = await _iEmployeeService.EditEmployee(updateEmployeeForm);
        return result.StatusCode switch
        {
            400 => BadRequest(result),
            404 => NotFound(result),
            _ => Ok(result)
        };
    }

    [HttpDelete("{id::int}")]
    [Authorize]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _iEmployeeService.DeleteEmployee(id);
        if (result.StatusCode is 404) return NotFound(result);
        return Ok(result);
    }
}