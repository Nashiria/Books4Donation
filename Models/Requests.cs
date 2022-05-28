using BooksForDonation.Controllers;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using System.ComponentModel.DataAnnotations;
using Microsoft.Data.SqlClient;
namespace BooksForAdoption.Models
{
    public class Requests
    {
        private SqlConnection connection()
        {
            string connectionString = "Data Source=NASHIRIA\\SQLEXPRESS;Initial Catalog=BooksForAdoption;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection toReturn = new SqlConnection(connectionString);
            toReturn.Open();
            return toReturn;
        }
        [Key]
        public int RequestID { get; set; }
        public string ISBN { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime ShipDate { get; set; }
        public string RecieverMail { get; set; }
        public string? DonatorName { get; set; }
        public string? DonatorMail { get; set; }
        public string? DonatorNote { get; set; }
        public class RequestsDBContext : DbContext
        {
            public DbSet<Requests> Requestss { get; set; }
        }
        public Book bookData(string ISBN)
        {
            Book book = new Book();
            book.ISBN = ISBN;
            book=book.getFromISBN(book.ISBN,false);
            book.BookID=bookID(book.ISBN);
            return book;
        }
        public List<Book> searchBooks(string searchTerm)
        {
            List<Book> toReturn = new List<Book>();
            var url = "https://www.googleapis.com/books/v1/volumes?q=" + searchTerm + "&maxResults=15";
            var json = new System.Net.WebClient().DownloadString(url);
            BookJson bjs = JsonConvert.DeserializeObject<BookJson>(json);
            foreach (var item in bjs.Items)
            {
                Book book = new Book();
                string ISBN = (item.VolumeInfo.IndustryIdentifiers).Length == 2 ? item.VolumeInfo.IndustryIdentifiers[1].Identifier : item.VolumeInfo.IndustryIdentifiers[0].Identifier;
                book = book.getFromISBN(ISBN, false);
                book.ISBN=ISBN;
                toReturn.Add(book);
            }
            return toReturn;
        }
        public Requests fromRequestID(int requestID)
        {
            Requests req = new Requests();
            req.RequestID = requestID;
            SqlConnection conn = connection();
            SqlCommand comm = new SqlCommand("SELECT * FROM Requests WHERE (RequestID = @Val1)",conn);
            comm.Parameters.AddWithValue("@Val1", requestID.ToString());
            SqlDataReader reader = comm.ExecuteReader();
                req.ISBN = reader.GetValue(1).ToString();
                    req.RequestDate = DateTime.Parse(reader.GetValue(2).ToString());
                    req.ShipDate = DateTime.Parse(reader.GetValue(3).ToString());
                    req.RecieverMail = reader[4].ToString();
                    
            return req;
        }
        public int bookID(string ISBN)
        {
            SqlConnection conn = connection();
            SqlCommand comm = new SqlCommand("SELECT BookID FROM Book WHERE (ISBN = @Val1)", conn);
            comm.Parameters.AddWithValue("@Val1", ISBN);
            using (var reader = comm.ExecuteReader())
            {
                if (reader.Read())
                {
                    return reader.GetInt32(reader.GetOrdinal("BookID"));
                }
                return Convert.ToInt32(reader.GetValue(0));
            }
            
    }
        public void updateRequestFromISBN(Requests r)
        {
            SqlConnection conn = connection();
            SqlCommand comm = new SqlCommand("UPDATE Requests SET ShipDate=@val1,DonatorName=@val2,DonatorMail=@val3,DonatorNote=@val4 where RequestID = @val6;", conn);
            comm.Parameters.AddWithValue("@val1", r.ShipDate);
            comm.Parameters.AddWithValue("@val2", r.DonatorName);
            comm.Parameters.AddWithValue("@val3", r.DonatorMail);
            comm.Parameters.AddWithValue("@val4", r.DonatorNote);
            comm.Parameters.AddWithValue("@val6", r.RequestID);
            comm.ExecuteNonQuery();
        }
    }
}
