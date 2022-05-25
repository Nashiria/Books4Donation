using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BooksForAdoption.Models
{
    public class Customer : IdentityUser
    {
        [Key]
        public int ID { get; set; }
        public string Mail { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateOnly BirthDate { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public class CustomerDBContext : DbContext
        {
            public DbSet<Customer> Customers { get; set; }
        }
        public Customer(string input_mail, string input_password, string input_name, string input_surname, DateOnly input_birthdate, string input_adress, string input_city,string input_region, string input_postalcode, string input_country, string input_phone)
        {
            Mail = input_mail;
            Password = input_password;
            Name = input_name;
            Surname = input_surname;
            BirthDate = input_birthdate;
            Address = input_adress;
            City = input_city;
            Region = input_region;
            PostalCode = input_postalcode;
            Country = input_country;
            Phone = input_phone;
        }
    }
}
