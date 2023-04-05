using Employee.Data.Forms;
using Employee.Data.Models;
using Employee.Database.DatabaseRepository;
using Employee.Services.AppServices.ParserService;

namespace Employee.Services.AppServices.UserAppService;

public class UserService : IUserService
{
    private readonly IRepository<User> _ctx;
    private readonly CustomParser<Gender?> _genderParser = new EnumParser<Gender>();

    public UserService(IRepository<User> ctx)
    {
        _ctx = ctx;
    }

    public async Task<RegistrationResponse> RegisterUser(CreateUserForm createUserForm)
    {
        //Todo: correct the logic here
        var existedUser = _ctx.GetAll().FirstOrDefault(x => x.Email.Equals(createUserForm.Email));
        var existedPersonalNumber = _ctx.GetAll().FirstOrDefault(x => x.PersonalId.Equals(createUserForm.PersonalId));
        if (existedUser is not null) return new RegistrationResponse(400, "User already exists!", null);
        if (existedPersonalNumber is not null) return new RegistrationResponse(400, "Personal Id already exists!", null);

        var user = new User
        {
            Id = Guid.NewGuid().ToString(),
            FirstName = createUserForm.FirstName,
            LastName = createUserForm.LastName,
            Email = createUserForm.Email,
            PersonalId = createUserForm.PersonalId,
            Gender = createUserForm.Gender != null ? _genderParser.Parse(createUserForm.Gender) : null,
            DateOfBirth = createUserForm.DateOfBirth,
            Password = PasswordHasher.HashPassword(createUserForm.Password),
        };
        await _ctx.Add(user);
        //Todo: generate token here
        return new RegistrationResponse(200, null, "token");
    }
}