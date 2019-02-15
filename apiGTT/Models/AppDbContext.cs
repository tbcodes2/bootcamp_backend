using Microsoft.EntityFrameworkCore;

namespace apiGTT.Models
{
    public class AppDbContext: DbContext
    {
        /*public AppDBContext(DbContextOptions<AppDBContext> options) :
        base(options)
        {
        }*/
        public AppDbContext(DbContextOptions<AppDbContext> options) :
        base(options)
        {
        }

        public DbSet<Jira> Jira {get; set;} 
        public DbSet<Users> Users {get; set;}
        public DbSet<Certificates> Certificates {get; set;}

    }
}