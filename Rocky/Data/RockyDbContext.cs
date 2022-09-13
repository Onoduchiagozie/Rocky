using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rocky.Models;

namespace Rocky.Data;

public class RockyDbContext:IdentityDbContext
{
    public RockyDbContext(DbContextOptions<RockyDbContext> options):base(options)
    {
    }
     public DbSet<Category> Category { get; set; }  
     public DbSet<ApplicationType> ApplicationTypes { get; set; }
     public DbSet<Product> Products { get; set; }
     public DbSet<ApplicationUser> ApplicationUser { get; set; }
     public DbSet<InquiryDetail> InquiryDetail { get; set; }
     public DbSet<InquiryHeader> InquiryHeader { get; set; }
     public DbSet<OrderHeader> OrderHeader { get; set; }
     public DbSet<OrderDetails> OrderDetails { get; set; }


     protected override void OnModelCreating(ModelBuilder modelBuilder)
     {
         base.OnModelCreating(modelBuilder);
         modelBuilder.ApplyConfiguration(new ApplicationUserEntityConfiguration());
     }

}

public class ApplicationUserEntityConfiguration:IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.Property(u => u.Fullname).HasMaxLength(255);
    }
}