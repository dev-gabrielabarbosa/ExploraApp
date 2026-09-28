using Microsoft.EntityFrameworkCore;
using PontoTuristicoApp.Models;

namespace PontoTuristicoApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<PontoTuristico> PontosTuristicos { get; set; }
    }
}
