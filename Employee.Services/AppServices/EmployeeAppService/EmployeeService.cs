using Employee.Data.Forms;
using Employee.Data.Models;
using Employee.Data.ViewModels;
using Employee.Database.DatabaseRepository;
using Employee.Services.AppServices.ParserService;

namespace Employee.Services.AppServices.EmployeeAppService;

public class EmployeeService : IEmployeeService
{
    private readonly IRepository<Data.Models.Employee> _ctx;
    private readonly CustomParser<Gender?> _genderParser = new EnumParser<Gender>();

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

    public object GetEmployeeBySearchValue(string searchString)
    {
        var lowerCaseSearchString = searchString.ToLower().Trim().Split(" ").First();
        var employees = _ctx.GetAll();
        return employees.Where(x =>
                x.FirstName.ToLower().Contains(lowerCaseSearchString) ||
                x.LastName.ToLower().Contains(lowerCaseSearchString))
            .Select(EmployeeViewModels.Default.Compile())
            .ToList();
    }

    public IEnumerable<object> GetAllEmployees()
        => _ctx.GetAll().Select(EmployeeViewModels.Default.Compile());

    public async Task<AddEmployeeResponse> AddEmployee(CreateEmployeeForm createEmployeeForm)
    {
        if (PersonalIdExists(createEmployeeForm.PersonalId))
            return new AddEmployeeResponse(400, "Personal Id already exists", null);
        if (EmailExists(createEmployeeForm.Email))
            return new AddEmployeeResponse(400, "Email already exists", null);

        var employee = new Data.Models.Employee
        {
            FirstName = createEmployeeForm.FirstName,
            LastName = createEmployeeForm.LastName,
            Email = createEmployeeForm.Email,
            PersonalId = createEmployeeForm.PersonalId,
            Gender = createEmployeeForm.Gender != null ? _genderParser.Parse(createEmployeeForm.Gender) : null,
            MobileNumber = createEmployeeForm.MobileNumber,
            Position = createEmployeeForm.Position,
            EmployeeStatus = createEmployeeForm.EmployeeStatus,
            DateOfFire = createEmployeeForm.DateOfFire
        };
        var result = await _ctx.Add(employee);
        return new AddEmployeeResponse(200, null, EmployeeViewModels.Default.Compile().Invoke(result));
    }


    public async Task<EditEmployeeResponse> EditEmployee(UpdateEmployeeForm updateEmployeeForm)
    {
        if (PersonalIdExists(updateEmployeeForm.Id, updateEmployeeForm.PersonalId))
            return new EditEmployeeResponse(400, "Personal Id already exists.", null);

        if (EmailExists(updateEmployeeForm.Id, updateEmployeeForm.Email))
            return new EditEmployeeResponse(400, "Email already exists.", null);

        try
        {
            var employee = _ctx.GetById(updateEmployeeForm.Id);
            employee.FirstName = updateEmployeeForm.FirstName;
            employee.LastName = updateEmployeeForm.LastName;
            employee.Email = updateEmployeeForm.Email;
            employee.PersonalId = updateEmployeeForm.PersonalId;
            employee.Gender = updateEmployeeForm.Gender != null ? _genderParser.Parse(updateEmployeeForm.Gender) : null;
            employee.MobileNumber = updateEmployeeForm.MobileNumber;
            employee.Position = updateEmployeeForm.Position;
            employee.EmployeeStatus = updateEmployeeForm.EmployeeStatus;
            employee.DateOfFire = updateEmployeeForm.DateOfFire;
            
            await _ctx.Update(employee);
            return new EditEmployeeResponse(200, null, EmployeeViewModels.Default.Compile().Invoke(employee));

        }
        catch (InvalidOperationException ex)
        {
            //Todo: log the exception into file
            return new EditEmployeeResponse(404, "Cant find employee to edit.", null);
        }
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

    private bool PersonalIdExists(ulong personalId)
        => _ctx.GetAll().Any(x => x.PersonalId.Equals(personalId));

    private bool PersonalIdExists(int id, ulong personalId)
        => _ctx.GetAll().Any(x => x.PersonalId.Equals(personalId) && x.Id != id);

    private bool EmailExists(string email)
        => _ctx.GetAll().Any(x => x.Email.Contains(email));

    private bool EmailExists(int id, string email)
        => _ctx.GetAll().Any(x => x.Email.Contains(email) && x.Id != id);
}