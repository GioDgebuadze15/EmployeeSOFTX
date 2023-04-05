using FluentValidation;

namespace Employee.Data.Forms.Validation;

public class UpdateEmployeeValidation:AbstractValidator<UpdateEmployeeForm>
{
    public UpdateEmployeeValidation()
    {
        RuleFor(x => x.Id).NotEqual(0);
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

        RuleFor(x => x.Position).NotEmpty().WithMessage("Position is required.");
        RuleFor(x => x.EmployeeStatus).IsInEnum().WithMessage("EmployeeStatus is not correct.");
    }
}