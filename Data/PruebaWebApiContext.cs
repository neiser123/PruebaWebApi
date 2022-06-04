using Microsoft.EntityFrameworkCore;

namespace PruebaWebApi.Data
{
    public class PruebaWebApiContext : DbContext
    {
        public PruebaWebApiContext (DbContextOptions<PruebaWebApiContext> options)
            : base(options)
        {
        }

       // public DbSet<PruebaWebApi.Models.Actividades> Actividades { get; set; }
    }
}
