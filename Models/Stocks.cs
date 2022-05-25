using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BooksForAdoption.Models
{
    public class Stocks
    {
        [Key]
        public int StockID { get; set; }
        public int TransactionNumber { get; set; }
        public int Quantity { get; set; }
        public string ISBN { get; set; }
        public class StocksDBContext : DbContext
        {
            public DbSet<Stocks> Stockss { get; set; }
        }
    }
}
