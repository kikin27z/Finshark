using Api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Api.Data
{
    public class AppDBContext : IdentityDbContext<AppUser>
    {
        public AppDBContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {

        }

        // Alternative form to configure the connection to database,
        // you only need to inject the json configuration to get the connection string
        //protected override void OnConfiguring(DbContextOptionsBuilder options) =>
        //    options.UseSqlServer(.GetConnectionString("DefaultConnection"));

        public DbSet<Comment> Comments { get; set; }
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<Portafolio> Portafolios{ get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Portafolio>()
                .HasKey(p => new { p.AppUserId, p.StockId });
            modelBuilder.Entity<Portafolio>()
                .HasOne(p => p.AppUser)
                .WithMany(p => p.Portafolios)
                .HasForeignKey(p => p.AppUserId);

            modelBuilder.Entity<Portafolio>()
                .HasOne(p => p.Stock)
                .WithMany(p => p.Portafolios)
                .HasForeignKey(p => p.StockId);

            List<IdentityRole>  roles = new List<IdentityRole>()
            {
                new IdentityRole { Name = "Admin", NormalizedName = "ADMIN" },
                new IdentityRole { Name = "User", NormalizedName = "USER" }
            };

            modelBuilder.Entity<IdentityRole>().HasData(roles);
        }
    }
}
