using Microsoft.EntityFrameworkCore;
using WebApplication1.Core.BaseModels;

namespace WebApplication1.Core.ApplicationDatabaseContext;

public class dbContext : DbContext
{
    public dbContext(DbContextOptions<dbContext> options)
        : base(options)
    {
    }
    public DbSet<UserModel> users { get; set; }
    public DbSet<Words> words { get; set; }

}