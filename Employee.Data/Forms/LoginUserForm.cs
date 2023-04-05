using System.ComponentModel.DataAnnotations;

namespace Employee.Data.Forms;

public class LoginUserForm
{
    public string Email { get; set; }
    
    public string Password { get; set; }

}