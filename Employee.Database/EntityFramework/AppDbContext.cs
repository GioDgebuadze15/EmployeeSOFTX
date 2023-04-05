using Microsoft.EntityFrameworkCore;

namespace Employee.Database.EntityFramework;

public class AppDbContext: DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
    {
        
    }
}