using Microsoft.EntityFrameworkCore;
using project.Models;

namespace project.DAL
{
    public class Context: DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Donor> Donors { get; set; }
        public DbSet<Gift> Gifts { get; set; }
        public DbSet<Manager> Managers { get; set; }
        public DbSet<Purchase> Purchases { get; set; }

        public Context(DbContextOptions<Context> contextOptions) : base(contextOptions)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>().Property(c => c.Id).UseIdentityColumn(1000, 10);
            modelBuilder.Entity<Category>().Property(cc => cc.Id).UseIdentityColumn(5, 5);
            modelBuilder.Entity<Donor>().Property(cc => cc.Id).UseIdentityColumn(100, 1);
            modelBuilder.Entity<Gift>().Property(cc => cc.Id).UseIdentityColumn(333, 3);
            modelBuilder.Entity<Purchase>().Property(cc => cc.Id).UseIdentityColumn(2000, 2);

            base.OnModelCreating(modelBuilder);
        }
    }

}
