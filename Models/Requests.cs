using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BooksForAdoption.Models
{
    public class Requests
    {
        [Key]
        public int RequestID { get; set; }
        public string ISBN { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime ShipDate { get; set; }
        public string RecieverMail { get; set; }
        public class RequestsDBContext : DbContext
        {
            public DbSet<Requests> Requestss { get; set; }
        }

    }
}
