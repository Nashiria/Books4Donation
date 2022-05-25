using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;


namespace BooksForAdoption.Models
{
    public class Transactions
    {
        [Key]
        public int TransactionsNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime PaymentClearanceDate { get; set; }
        public string Mail { get; set; }
        public int CreditCardNo { get; set; }
        public class TransactionsDBContext : DbContext
        {
            public DbSet<Transactions> Transactionss { get; set; }
        }
    }
}
