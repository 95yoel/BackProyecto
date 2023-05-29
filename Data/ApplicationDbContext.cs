using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AsturTravel.Models;

namespace AsturTravel.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<AsturTravel.Models.Usuario> Usuario { get; set; }
        public DbSet<AsturTravel.Models.TiposViaje> TiposViaje { get; set; }
        public DbSet<AsturTravel.Models.Destinos> Destinos { get; set; }
        public DbSet<AsturTravel.Models.Viajes> Viajes { get; set; }
        public DbSet<AsturTravel.Models.Reservas> Reservas { get; set; }
        public DbSet<AsturTravel.Models.Destacados>? Destacados { get; set; }
    }
}