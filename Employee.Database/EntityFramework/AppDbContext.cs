﻿using Employee.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Employee.Database.EntityFramework;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Data.Models.Employee> Employees { get; set; }
}