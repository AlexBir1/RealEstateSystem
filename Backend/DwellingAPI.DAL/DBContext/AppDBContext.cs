using DwellingAPI.DAL.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DwellingAPI.DAL.DBContext
{
    public class AppDBContext : IdentityDbContext<Account>
    {
        public AppDBContext(DbContextOptions<AppDBContext> options, bool isTest = false) : base(options)
        {
            if(isTest)
                Database.EnsureCreated();
            else
                Database.Migrate();
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Apartment>(e =>
            {
                e.HasKey(x => x.Id);
                e.HasOne(x => x.Details).WithOne(x => x.Apartment).HasForeignKey<ApartmentDetails>(x => x.ApartmentId).OnDelete(deleteBehavior: DeleteBehavior.Cascade);
                e.HasMany(x=>x.Photos).WithOne(x=>x.Apartment).HasForeignKey(x => x.ApartmentId).OnDelete(deleteBehavior: DeleteBehavior.Cascade);
                e.HasIndex(x => x.Number).IsUnique();
            });

            builder.Entity<ApartmentDetails>(e =>
            {
                e.HasKey(x => x.ApartmentId);
            });

            builder.Entity<ApartmentPhoto>(e =>
            {
                e.HasKey(x => x.Id);
            });

            builder.Entity<Call>(e =>
            {
                e.HasKey(x => x.Id);
            });

            builder.Entity<Order>(e =>
            {
                e.HasKey(x => x.Id);
            });

            builder.Entity<OrderApartment>(e =>
            {
                e.HasKey(x => new { x.OrderId, x.ApartmentId });
                e.HasOne(x => x.Apartment).WithMany(x => x.ApartmentOrders);
                e.HasOne(x => x.Order).WithMany(x => x.OrderApartments);
            });

            builder.Entity<Agreement>(e =>
            {
                e.HasKey(x => x.Id);
                e.HasOne(x => x.Account).WithMany(x => x.Agreements);
                e.HasIndex(x => x.ApartmentAddress).IsUnique();
            });

            base.OnModelCreating(builder);
        }

        public DbSet<Apartment> Apartments { get; set; }
        public DbSet<Agreement> Agreements { get; set; }
        public DbSet<ApartmentDetails> ApartmentDetails { get; set; }
        public DbSet<ApartmentPhoto> ApartmentPhotos { get; set; }
        public DbSet<Call> Calls { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderApartment> OrdersApartments { get; set; }
    }
}
