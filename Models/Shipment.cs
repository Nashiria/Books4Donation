using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;


namespace BooksForAdoption.Models
{
    public class Shipments
    {
        [Key]
        public int ShipID { get; set; }
        public double ShipCost { get; set; }
        public DateTime ShipDate { get; set; }
        public int TransactionNumber { get; set; }
        public string RecieverMail { get; set; }
        public class ShipmentsDBContext : DbContext
        {
            public DbSet<Shipments> Shipmentss { get; set; }
        }
    }
}
