using Employee.Data.Models;

namespace Employee.Data.Forms;

public class CreateEmployeeForm
{
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Email { get; set; }
    public ulong PersonalId { get; set; }

    public string? Gender { get; set; }

    public string? MobileNumber { get; set; }

    public string Position { get; set; }

    public EmployeeStatus EmployeeStatus { get; set; }

    public DateTime? DateOfFire { get; set; }
}