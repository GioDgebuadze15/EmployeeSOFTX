using System.ComponentModel.DataAnnotations;

namespace Employee.Data.Models;

public class Employee : BaseModel<int>
{
    [Required] public string FirstName { get; set; }

    [Required] public string LastName { get; set; }

    [Required] public string Email { get; set; }

    [Required] public ulong PersonalId { get; set; }

    public Gender? Gender { get; set; }

    public string? MobileNumber { get; set; }

    [Required] public string Position { get; set; }

    [Required] public EmployeeStatus EmployeeStatus { get; set; }

    public DateTime? DateOfFire { get; set; }
}