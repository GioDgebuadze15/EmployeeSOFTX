using System.ComponentModel.DataAnnotations;

namespace Employee.Data.Models;

public abstract class BaseModel<TKey>
{
    [Key] public TKey Id { get; set; }

    public DateTime CreatedDate { get; set; } = DateTime.Now;
}