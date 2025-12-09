using Microsoft.EntityFrameworkCore;
using CaseManagementSystem.Settings;
using CaseManagementSystem.Models;
public class DatabaseService : DbContext
{
    public DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlServer(AppValues.ConString);
    }
}
