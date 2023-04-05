using Employee.Data.Forms;
using Employee.Data.Models;

namespace Employee.Services.AppServices.UserAppService;

public interface IUserService
{
    Task<RegistrationResponse> RegisterUser(CreateUserForm createUserForm);
}