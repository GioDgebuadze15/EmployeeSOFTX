using FluentValidation;

namespace Employee.Data.Forms.Validation;

public class LoginUserFormValidation : AbstractValidator<CreateUserForm>
{
    public LoginUserFormValidation()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email is required.")
            .EmailAddress()
            .WithMessage("Invalid email address.");

        RuleFor(user => user.Password)
            .NotEmpty()
            .WithMessage("Password is required.");
    }
}