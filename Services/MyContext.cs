using Microsoft.EntityFrameworkCore;

namespace WpfEfAuthen.Services
{
    public class MyContext:DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(Properties.Settings.Default.SqlConnection);
        }

        public DbSet<tblUser> tblUser { get; set; }
    }
}
