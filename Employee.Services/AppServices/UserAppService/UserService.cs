using System.Security.Claims;
using Employee.Data.Forms;
using Employee.Data.Models;
using Employee.Database.DatabaseRepository;
using Employee.Services.AppServices.ParserService;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

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
        if (UserExists(createUserForm.Email)) return new RegistrationResponse(400, "User already exists!", null);
        if (PersonalIdExists(createUserForm.PersonalId))
            return new RegistrationResponse(400, "Personal Id already exists!", null);

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
        var token = GenerateJwtToken(user);
        return new RegistrationResponse(200, null, token);
    }

    public LoginResponse LoginUser(LoginUserForm loginUserForm)
    {
        var user = GetUser(loginUserForm.Email);
        if (user is null || !PasswordIsCorrect(loginUserForm.Password, user.Password))
            return new LoginResponse(404, "Incorrect data!", null);

        var token = GenerateJwtToken(user);
        return new LoginResponse(200, null, token);
    }

    private static string GenerateJwtToken(User user)
    {
        var handler = new JsonWebTokenHandler();
        var key = new RsaSecurityKey(RsaKey.GetRsaKey());
        var token = handler.CreateToken(new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email),
            }),
            Expires = DateTime.UtcNow.AddDays(1),
            SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.RsaSha256),
        });
        return token ?? "empty";
    }

    private bool UserExists(string email)
        => _ctx.GetAll().Any(x => x.Email.Equals(email));

    private User? GetUser(string email)
        => _ctx.GetAll().FirstOrDefault(x => x.Email.Equals(email));

    private bool PersonalIdExists(ulong? personalId)
        => personalId is not null && _ctx.GetAll().Any(x => x.PersonalId.Equals(personalId));


    private static bool PasswordIsCorrect(string providedPassword, string userPassword)
        => PasswordHasher.VerifyPassword(providedPassword, userPassword);
}