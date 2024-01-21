using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PlatformaMultimedialna.Models;

namespace PlatformaMultimedialna.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<MediaModel> Media { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<PlatformaMultimedialna.Models.UserModel>? UserModel { get; set; }
    }
}