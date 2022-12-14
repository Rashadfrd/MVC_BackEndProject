using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Riode.Models;

namespace Riode.DAL
{
    public class RiodeContext:IdentityDbContext
    {
        public RiodeContext(DbContextOptions<RiodeContext>options):base(options)
        {
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Feature> Features { get; set; }
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<Badge> Badges { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<ProductColor> ProductColors { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<ProductBadge> ProductBadges { get; set; }
        public DbSet<Advertisement> Advertisements { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<ProductComment> ProductComments { get; set; }
        public DbSet<AboutUs> AboutUs { get; set; }
    }
}
