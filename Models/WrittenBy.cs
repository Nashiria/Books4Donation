using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;


namespace BooksForAdoption.Models
{
    public class WrittenBy
    {
        [Key]
        public string AuthorName { get; set; }
        public string ISBN { get; set; }
        public int OrderOfAuthorship { get; set; }
        public class WrittenByDBContext : DbContext
        {
            public DbSet<WrittenBy> WrittenBys { get; set; }
        }
    }
}
