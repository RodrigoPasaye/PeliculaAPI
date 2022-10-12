using Microsoft.EntityFrameworkCore;
using PeliculaAPI.Models;

namespace PeliculaAPI.Data {
    public class AplicationDbContext : DbContext {

        public AplicationDbContext(DbContextOptions<AplicationDbContext> options) : base(options) {

        }

        public DbSet<Categoria> Categorias { get; set; }
    }
}
