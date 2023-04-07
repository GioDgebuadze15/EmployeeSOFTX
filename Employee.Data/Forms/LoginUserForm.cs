using System.ComponentModel.DataAnnotations;

namespace Employee.Data.Forms;

public class LoginUserForm
{
    [Required] public string Email { get; set; }

    [Required] public string Password { get; set; }
}