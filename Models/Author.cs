using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BooksForAdoption.Models
{
    public class Author
    {
        [Key]
        public int AuthorID { get; set; }
        public string AuthorName { get; set; }
        public string AuthorBiography { get; set; }
        public class AuthorDBContext : DbContext
        {
            public DbSet<Author> Authors { get; set; }
        }
    }
}

