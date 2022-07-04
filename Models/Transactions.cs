using BooksForDonation.Controllers;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using System.ComponentModel.DataAnnotations;
using Microsoft.Data.SqlClient;
namespace BooksForAdoption.Models
{
    public class Transactions
    {
        private SqlConnection connection()
        {
            string connectionString = "";            
            SqlConnection toReturn = new SqlConnection(connectionString);
            toReturn.Open();
            return toReturn;
        }
        [Key]
        public int TransactionsNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public string Mail { get; set; }
        public int RequestID { get; set; }

        public class TransactionsDBContext : DbContext
        {
            public DbSet<Transactions> Transactionss { get; set; }
        }

        public bool makeTransaction(double cost)
        {
            return true;
        }
        public void donateRequest(Requests req)
        {
            Book book = req.bookData(req.ISBN);

            if (makeTransaction(book.Price))
            {
                completeTransaction(req.DonatorMail,book, req);
                req.ShipDate=DateTime.Now;
                req.updateRequestFromISBN(req);
            }
        }
        public void completeTransaction(string mail,Book b,Requests r)
        {
            SqlConnection conn = connection();
            Transactions transactions = new Transactions();
            transactions.OrderDate = DateTime.Now;
            transactions.RequestID = r.RequestID;
            transactions.Mail = mail;
            SqlCommand comm = new SqlCommand("INSERT INTO Transactions (OrderDate,Mail,RequestID) VALUES (@val1,@val2,@val3);", conn);
            comm.Parameters.AddWithValue("@val1", transactions.OrderDate);
            comm.Parameters.AddWithValue("@val2", transactions.Mail);
            comm.Parameters.AddWithValue("@val3", transactions.RequestID);
            comm.ExecuteNonQuery();
        }
    }
}
