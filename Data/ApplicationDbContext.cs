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

    }
}