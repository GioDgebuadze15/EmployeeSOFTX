using System.ComponentModel.DataAnnotations;

namespace Employee.Data.Models;

public class User : BaseModel<string>
{
    [Required] public string FirstName { get; set; }

    [Required] public string LastName { get; set; }

    [Required] public string Email { get; set; }
    public ulong? PersonalId { get; set; }

    public Gender? Gender { get; set; }

    public DateTime? DateOfBirth { get; set; }

    [Required] public string Password { get; set; }
}