using Employee.Data.Forms;
using Employee.Data.Models;
using Employee.Data.ViewModels;
using Employee.Database.DatabaseRepository;

namespace Employee.Services.AppServices.EmployeeAppService;

public class EmployeeService : IEmployeeService
{
    private readonly IRepository<Data.Models.Employee> _ctx;

    public EmployeeService(IRepository<Data.Models.Employee> ctx)
    {
        _ctx = ctx;
    }

    public object GetEmployeeById(int id)
    {
        try
        {
            return EmployeeViewModels.Default.Compile().Invoke(_ctx.GetById(id));
        }
        catch (InvalidOperationException ex)
        {
            //Todo: log the exception into file
            return new object();
        }
    }

    public IEnumerable<object> GetAllEmployees()
        => _ctx.GetAll().Select(EmployeeViewModels.Default.Compile());

    public Task<AddEmployeeResponse> AddEmployee(CreateEmployeeForm createEmployeeForm)
    {
        throw new NotImplementedException();
    }

    public Task<EditEmployeeResponse> EditEmployee(UpdateEmployeeForm updateEmployeeForm)
    {
        throw new NotImplementedException();
    }

    public async Task<DeleteEmployeeResponse> DeleteEmployee(int id)
    {
        try
        {
            var employee = _ctx.GetById(id);
            await _ctx.Remove(employee);
            return new DeleteEmployeeResponse(200, null, employee);
        }
        catch (InvalidOperationException ex)
        {
            //Todo: log the exception into file
            return new DeleteEmployeeResponse(404, "Cant find person to delete.", null);
        }
    }
}