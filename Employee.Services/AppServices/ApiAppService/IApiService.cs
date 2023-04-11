using Employee.Data.Forms;
using Employee.Services.AppServices.ApiAppService.ApiResponseAppService;
using Microsoft.AspNetCore.Http;

namespace Employee.Services.AppServices.ApiAppService;

public interface IApiService
{
    Task<List<Data.Models.Employee>> GetAllEmployees();
    Task<UpdateEmployeeForm> GetEmployee(int id);
    Task<ApiResponseEmployeeBase?> AddEmployee(CreateEmployeeForm createEmployeeForm, HttpContext httpContext);
    Task<ApiResponseEmployeeBase?> EditEmployee(UpdateEmployeeForm updateEmployeeForm, HttpContext httpContext);
    Task<ApiResponseEmployeeBase?> DeleteEmployee(int id, HttpContext httpContext);

    Task<ApiResponseUserBase?> LoginUser(LoginUserForm loginUserForm);
    Task<ApiResponseUserBase?> RegisterUser(CreateUserForm createUserForm);
}