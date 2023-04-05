using FluentValidation;

namespace Employee.Data.Forms.Validation;

public class CreateUserFormValidation : AbstractValidator<CreateUserForm>
{
    public CreateUserFormValidation()
    {
        RuleFor(x => x.FirstName).NotEmpty().WithMessage("FirstName is required.");
        RuleFor(x => x.LastName).NotEmpty().WithMessage("LastName is required.");
        
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email is required.")
            .EmailAddress()
            .WithMessage("Invalid email address.");

        RuleFor(x => x.PersonalId.ToString())
            .Must(pid => string.IsNullOrEmpty(pid) || pid.Length == 11)
            .WithName("PersonalId")
            .WithMessage("PersonalId must be empty or exactly 11 digits long.");

        RuleFor(user => user.Password)
            .NotEmpty()
            .WithMessage("Password is required.")
            .MinimumLength(8)
            .WithMessage("Password must be at least 8 digits long.")
            .Matches("[A-Z]")
            .WithMessage("Password must contain at least one uppercase letter.")
            .Matches("[a-z]")
            .WithMessage("Password must contain at least one lowercase letter.")
            .Matches("[0-9]")
            .WithMessage("Password must contain at least one digit.");

        RuleFor(user => user.ConfirmPassword)
            .Equal(user => user.Password)
            .WithMessage("Password and confirm password do not match.");
    }
}