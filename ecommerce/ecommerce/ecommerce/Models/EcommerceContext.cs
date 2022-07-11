using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace ecommerce.Models
{
    public class EcommerceContext:DbContext
    {
        public EcommerceContext(DbContextOptions<EcommerceContext> options):base (options)
        { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
            optionsBuilder.UseSqlServer("Data Source = LAPTOP-K7VMCFFS\\KADORYZ; Initial Catalog = EvCommerce; Integrated Security = True");
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Seller> Sellers { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<ItemStatus> ItemsStatus { get; set; }
        public DbSet<OrderDetailStatus> OrderDetailStatuses { get; set; }              
    }
}
