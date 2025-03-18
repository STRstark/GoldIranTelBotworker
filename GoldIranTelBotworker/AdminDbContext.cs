using Microsoft.EntityFrameworkCore;

namespace GoldIranTelBotworker;

public class AdminDbContext : DbContext
{
    public AdminDbContext(DbContextOptions<AdminDbContext> options) : base(options) { }
    public DbSet<Admins> Admins { get; set; }
   
}