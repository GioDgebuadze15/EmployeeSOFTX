using Employee.Data.Forms;
using Employee.Data.Models;

namespace Employee.Services.AppServices.EmployeeAppService;

public interface IEmployeeService
{
    object GetEmployeeById(int id);

    object GetEmployeeBySearchValue(string searchString);
    IEnumerable<object> GetAllEmployees();

    Task<AddEmployeeResponse> AddEmployee(CreateEmployeeForm createEmployeeForm);

    Task<EditEmployeeResponse> EditEmployee(UpdateEmployeeForm updateEmployeeForm);

    Task<DeleteEmployeeResponse> DeleteEmployee(int id);
}