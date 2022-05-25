using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;


namespace BooksForAdoption.Models
{
    public class Orders
    {
        [Key]
        public int OrderID { get; set; }
        public double OrderCost { get; set; }
        public DateTime OrderDate { get; set; }
        public int TransactionNumber { get; set; }
        public class OrdersDBContext : DbContext
        {
            public DbSet<Orders> Orderss { get; set; }
        }
    }
}
