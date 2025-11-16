using EngWordApp.Entity;
using Microsoft.EntityFrameworkCore;

namespace EngWordApp.Context
{
    public class WordContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=Ali-Pc\\SQLEXPRESS;Initial Catalog=EngWordAppDb;Integrated Security=true;TrustServerCertificate=true");
        }

        

        public DbSet<Word> Words { get; set; }
        public DbSet<Mean> Means { get; set; }
    }
}
