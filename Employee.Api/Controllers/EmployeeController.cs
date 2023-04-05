using Employee.Data.Forms;
using Employee.Services.AppServices.EmployeeAppService;
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

    [HttpGet]
    public IActionResult GetAll()
        => Ok(_iEmployeeService.GetAllEmployees());

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] CreateEmployeeForm createEmployeeForm)
        => Ok(await _iEmployeeService.AddEmployee(createEmployeeForm));
    
    [HttpPut]
    public async Task<IActionResult> Edit([FromBody] UpdateEmployeeForm updateEmployeeForm)
        => Ok(await _iEmployeeService.EditEmployee(updateEmployeeForm));
    
    [HttpDelete("{id::int}")]
    public async Task<IActionResult> Delete(int id)
        => Ok(await _iEmployeeService.DeleteEmployee(id));
}