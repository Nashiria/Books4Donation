using BooksForAdoption.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BooksForDonation.Data
{
    public class ApplicationDBContext : IdentityDbContext<Customer>
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options)
            : base(options)
        {
        }
        public DbSet<BooksForAdoption.Models.Customer> Customers { get; set; }
        public DbSet<BooksForAdoption.Models.Requests>? Requests { get; set; }
        public DbSet<BooksForAdoption.Models.Book>? Book { get; set; }
        public DbSet<BooksForAdoption.Models.Author>? Author { get; set; }
        public DbSet<BooksForAdoption.Models.Orders>? Orders { get; set; }
        public DbSet<BooksForAdoption.Models.Shipments>? Shipments { get; set; }
        public DbSet<BooksForAdoption.Models.Stocks>? Stocks { get; set; }
        public DbSet<BooksForAdoption.Models.Transactions>? Transactions { get; set; }
        public DbSet<BooksForAdoption.Models.WrittenBy>? WrittenBy { get; set; }

    }
}