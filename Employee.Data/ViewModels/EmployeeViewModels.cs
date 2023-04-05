using System.Linq.Expressions;
using Employee.Data.Models;

namespace Employee.Data.ViewModels;

public static class EmployeeViewModels
{
    public static Expression<Func<Models.Employee, object>> Default =>
        employee => new
        {
            employee.Id,
            employee.FirstName,
            employee.LastName,
            employee.Email,
            employee.PersonalId,
            Gender = employee.Gender == Gender.Male ? "Male" : "Female",
            employee.MobileNumber,
            employee.Position,
            EmployeeStatus = employee.EmployeeStatus == EmployeeStatus.InState ? "In State" :
                employee.EmployeeStatus == EmployeeStatus.OutOfState ? "Out Of State" : "Fired",
            employee.DateOfFire,
            employee.CreatedDate
        };
}