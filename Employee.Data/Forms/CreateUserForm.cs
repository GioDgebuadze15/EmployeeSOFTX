namespace Employee.Data.Forms;

public class CreateUserForm
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public ulong? PersonalId { get; set; }

    public string? Gender { get; set; }

    public DateTime? DateOfBirth { get; set; }

    public string Password { get; set; }

    public string ConfirmPassword { get; set; }
}