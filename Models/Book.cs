using BooksForDonation.Controllers;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using System.ComponentModel.DataAnnotations;
using Microsoft.Data.SqlClient;

namespace BooksForAdoption.Models
{
    
    public class Book
    {
        [Key]
        public int BookID { get; set; }
        public string ISBN { get; set; }
        public string Title { get; set; }
        public string Synopsis { get; set; }
        public string PublisherName { get; set; }
        public string ImageLink { get; set; }
        public double Price { get; set; }

        public int pageCount { get; set; }
        public string PublicationDate { get; set; }
        private SqlConnection connection()
        {
            string connectionString = "Data Source=NASHIRIA\\SQLEXPRESS;Initial Catalog=BooksForAdoption;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection toReturn = new SqlConnection(connectionString);
            toReturn.Open();
            return toReturn;
        }
        public class BookDBContext : DbContext
        {
            public DbSet<Book> Books { get; set; }
        }
        public void registerBook(string bookISBN)
        {
            SqlConnection conn = connection();
            SqlCommand comm = new SqlCommand("INSERT INTO Book (ISBN,Title,Synopsis,PublisherName,ImageLink,pageCount,PublicationDate,Price) VALUES (@val1,@val2,@val3,@val4,@val5,@val6,@val7,@val8);", conn);
            comm.Connection = conn;
            Book toRegister = getFromISBN(bookISBN,true);
            comm.Parameters.AddWithValue("@val1", toRegister.ISBN);
            comm.Parameters.AddWithValue("@val2", toRegister.Title);
            comm.Parameters.AddWithValue("@val3", toRegister.Synopsis);
            comm.Parameters.AddWithValue("@val4", toRegister.PublisherName);
            comm.Parameters.AddWithValue("@val5", toRegister.ImageLink);
            comm.Parameters.AddWithValue("@val6", toRegister.pageCount);
            comm.Parameters.AddWithValue("@val7", toRegister.PublicationDate);
            comm.Parameters.AddWithValue("@val8", toRegister.Price);
            comm.ExecuteNonQuery();

        }
        public Book getFromISBN(string bookISBN,bool doRegister)
        {
            Book toReturn = new Book();
            BookJson bookJson = new BookJson();
            var url = "https://www.googleapis.com/books/v1/volumes?&q=isbn:" + bookISBN;
            var json = new System.Net.WebClient().DownloadString(url);
            BookJson bjs = JsonConvert.DeserializeObject<BookJson>(json);
            if (bjs.Items != null)
            {
            toReturn.ISBN = bookISBN;
            toReturn.Synopsis = bjs.Items[0].VolumeInfo.Description is null ? "There's no description for this book." : bjs.Items[0].VolumeInfo.Description; ;
            toReturn.Title = bjs.Items[0].VolumeInfo.Title is null ? "There's no title for this book." : bjs.Items[0].VolumeInfo.Title;
            toReturn.ImageLink = bjs.Items[0].VolumeInfo.ImageLinks is null ? "https://via.placeholder.com/128x200.png?text=No+Image+Available" : bjs.Items[0].VolumeInfo.ImageLinks.Thumbnail.ToString();
            toReturn.PublisherName = bjs.Items[0].VolumeInfo.Publisher is null ? "There's no publisher data for this book." : bjs.Items[0].VolumeInfo.Publisher;
            toReturn.PublicationDate = bjs.Items[0].VolumeInfo.PublishedDate is null ? "There's no publishing data for this book." : bjs.Items[0].VolumeInfo.PublishedDate;
            toReturn.pageCount = Convert.ToInt32(bjs.Items[0].VolumeInfo.PageCount);
            toReturn.Price = bjs.Items[0].SaleInfo.ListPrice is null ? 0.99d : bjs.Items[0].SaleInfo.ListPrice.Amount;
                if (doRegister) { 
            if (bjs.Items[0].VolumeInfo.Authors!=null)
            {
                for (int i = 0; i < bjs.Items[0].VolumeInfo.Authors.Length; i++)
                {
                    registerAuthor(bjs.Items[0].VolumeInfo.Authors[i]);
                    registerAuthorForWrittenBy(bookISBN, bjs.Items[0].VolumeInfo.Authors[i], i);
                }
            }
                }
                return toReturn;
            }
            return null;
        }
        public void registerAuthorForWrittenBy(string bookISBN,string AuthorName,int order)
        {
            SqlConnection conn = connection();
            SqlCommand comm = new SqlCommand("SELECT COUNT(*) FROM WrittenBy WHERE AuthorName=@val1 AND ISBN = @val2;");
            comm.Connection = conn;
            comm.Parameters.AddWithValue("@val1", AuthorName);
            comm.Parameters.AddWithValue("@val2", bookISBN);
            int UserExist = (int)comm.ExecuteScalar();
            if (UserExist > 0)
            {

            }
            else
            {
                comm = new SqlCommand("INSERT INTO WrittenBy (ISBN,AuthorName,OrderOfAuthorship) VALUES (@val1, @val2,@val3);",conn);
                comm.Parameters.AddWithValue("@val1", bookISBN);
                comm.Parameters.AddWithValue("@val2", AuthorName);
                comm.Parameters.AddWithValue("@val3", order);
                comm.ExecuteNonQuery();
            }
        }

        public void registerAuthor(string AuthorName)
        {
            Author toReturn = new Author();
            SqlConnection conn = connection();
            SqlCommand comm = new SqlCommand("SELECT COUNT(*) FROM Author WHERE (AuthorName = @val1);");
            comm.Connection = conn;
            comm.Parameters.AddWithValue("@val1", AuthorName);
            int UserExist = (int)comm.ExecuteScalar();
            if (UserExist > 0)
            {

            }
            else
            {
                var url = "https://en.wikipedia.org/w/api.php?format=json&action=query&prop=extracts&exlimit=max&explaintext&exintro&titles=" + AuthorName.Replace(" ", "%20") + "&redirects=";
                var json = new System.Net.WebClient().DownloadString(url);
                toReturn.AuthorName = AuthorName;

                if (json != null)
                {
                    AuthorJson ajs = JsonConvert.DeserializeObject<AuthorJson>(json);
                    toReturn.AuthorBiography = ajs.Query.Pages.Page is null ? "There's no biography data for the author." : ajs.Query.Pages.Page.Extract;
                }
                else
                {
                    toReturn.AuthorBiography = "There's no biography data for the author.";
                }

                comm = new SqlCommand("INSERT INTO Author (AuthorName,AuthorBiography) VALUES (@val1, @val2);", conn);
                comm.Parameters.AddWithValue("val1", AuthorName);
                comm.Parameters.AddWithValue("val2", toReturn.AuthorBiography);
                comm.ExecuteNonQuery();
             
            }
            
        }
    }
    

    
}
namespace BooksForAdoption.Models
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class AuthorJson
    {
        [JsonProperty("batchcomplete")]
        public string Batchcomplete { get; set; }

        [JsonProperty("query")]
        public Query Query { get; set; }

        [JsonProperty("limits")]
        public Limits Limits { get; set; }
    }

    public partial class Limits
    {
        [JsonProperty("extracts")]
        public long Extracts { get; set; }
    }

    public partial class Query
    {
        [JsonProperty("redirects")]
        public Redirect[] Redirects { get; set; }

        [JsonProperty("pages")]
        public Pages Pages { get; set; }
    }

    public partial class Pages
    {
        [JsonProperty("Page")]
        public Page Page { get; set; }
    }

    public partial class Page
    {
        [JsonProperty("pageid")]
        public long Pageid { get; set; }

        [JsonProperty("ns")]
        public long Ns { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("extract")]
        public string Extract { get; set; }
    }

    public partial class Redirect
    {
        [JsonProperty("from")]
        public string From { get; set; }

        [JsonProperty("to")]
        public string To { get; set; }
    }
}


namespace BooksForAdoption.Models
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class BookJson
    {
        [JsonProperty("kind")]
        public string Kind { get; set; }

        [JsonProperty("totalItems")]
        public long TotalItems { get; set; }

        [JsonProperty("items")]
        public Item[] Items { get; set; }
    }

    public partial class Item
    {
        [JsonProperty("kind")]
        public string Kind { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("etag")]
        public string Etag { get; set; }

        [JsonProperty("selfLink")]
        public Uri SelfLink { get; set; }

        [JsonProperty("volumeInfo")]
        public VolumeInfo VolumeInfo { get; set; }

        [JsonProperty("saleInfo")]
        public SaleInfo SaleInfo { get; set; }

        [JsonProperty("accessInfo")]
        public AccessInfo AccessInfo { get; set; }

        [JsonProperty("searchInfo")]
        public SearchInfo SearchInfo { get; set; }
    }

    public partial class AccessInfo
    {
        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("viewability")]
        public string Viewability { get; set; }

        [JsonProperty("embeddable")]
        public bool Embeddable { get; set; }

        [JsonProperty("publicDomain")]
        public bool PublicDomain { get; set; }

        [JsonProperty("textToSpeechPermission")]
        public string TextToSpeechPermission { get; set; }

        [JsonProperty("epub")]
        public Epub Epub { get; set; }

        [JsonProperty("pdf")]
        public Pdf Pdf { get; set; }

        [JsonProperty("webReaderLink")]
        public Uri WebReaderLink { get; set; }

        [JsonProperty("accessViewStatus")]
        public string AccessViewStatus { get; set; }

        [JsonProperty("quoteSharingAllowed")]
        public bool QuoteSharingAllowed { get; set; }
    }

    public partial class Epub
    {
        [JsonProperty("isAvailable")]
        public bool IsAvailable { get; set; }

        [JsonProperty("acsTokenLink")]
        public Uri AcsTokenLink { get; set; }
    }

    public partial class Pdf
    {
        [JsonProperty("isAvailable")]
        public bool IsAvailable { get; set; }
    }

    public partial class SaleInfo
    {
        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("saleability")]
        public string Saleability { get; set; }

        [JsonProperty("isEbook")]
        public bool IsEbook { get; set; }

        [JsonProperty("listPrice")]
        public SaleInfoListPrice ListPrice { get; set; }

        [JsonProperty("retailPrice")]
        public SaleInfoListPrice RetailPrice { get; set; }

        [JsonProperty("buyLink")]
        public Uri BuyLink { get; set; }

        [JsonProperty("offers")]
        public Offer[] Offers { get; set; }
    }

    public partial class SaleInfoListPrice
    {
        [JsonProperty("amount")]
        public double Amount { get; set; }

        [JsonProperty("currencyCode")]
        public string CurrencyCode { get; set; }
    }

    public partial class Offer
    {
        [JsonProperty("finskyOfferType")]
        public long FinskyOfferType { get; set; }

        [JsonProperty("listPrice")]
        public OfferListPrice ListPrice { get; set; }

        [JsonProperty("retailPrice")]
        public OfferListPrice RetailPrice { get; set; }

        [JsonProperty("giftable")]
        public bool Giftable { get; set; }
    }

    public partial class OfferListPrice
    {
        [JsonProperty("amountInMicros")]
        public long AmountInMicros { get; set; }

        [JsonProperty("currencyCode")]
        public string CurrencyCode { get; set; }
    }

    public partial class SearchInfo
    {
        [JsonProperty("textSnippet")]
        public string TextSnippet { get; set; }
    }

    public partial class VolumeInfo
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("subtitle")]
        public string Subtitle { get; set; }

        [JsonProperty("authors")]
        public string[] Authors { get; set; }

        [JsonProperty("publisher")]
        public string Publisher { get; set; }

        [JsonProperty("publishedDate")]
        public string PublishedDate { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("industryIdentifiers")]
        public IndustryIdentifier[] IndustryIdentifiers { get; set; }

        [JsonProperty("readingModes")]
        public ReadingModes ReadingModes { get; set; }

        [JsonProperty("pageCount")]
        public long PageCount { get; set; }

        [JsonProperty("printType")]
        public string PrintType { get; set; }

        [JsonProperty("averageRating")]
        public long AverageRating { get; set; }

        [JsonProperty("ratingsCount")]
        public long RatingsCount { get; set; }

        [JsonProperty("maturityRating")]
        public string MaturityRating { get; set; }

        [JsonProperty("allowAnonLogging")]
        public bool AllowAnonLogging { get; set; }

        [JsonProperty("contentVersion")]
        public string ContentVersion { get; set; }

        [JsonProperty("panelizationSummary")]
        public PanelizationSummary PanelizationSummary { get; set; }

        [JsonProperty("imageLinks")]
        public ImageLinks ImageLinks { get; set; }

        [JsonProperty("language")]
        public string Language { get; set; }

        [JsonProperty("previewLink")]
        public Uri PreviewLink { get; set; }

        [JsonProperty("infoLink")]
        public Uri InfoLink { get; set; }

        [JsonProperty("canonicalVolumeLink")]
        public Uri CanonicalVolumeLink { get; set; }
    }

    public partial class ImageLinks
    {
        [JsonProperty("smallThumbnail")]
        public Uri SmallThumbnail { get; set; }

        [JsonProperty("thumbnail")]
        public Uri Thumbnail { get; set; }
    }

    public partial class IndustryIdentifier
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("identifier")]
        public string Identifier { get; set; }
    }

    public partial class PanelizationSummary
    {
        [JsonProperty("containsEpubBubbles")]
        public bool ContainsEpubBubbles { get; set; }

        [JsonProperty("containsImageBubbles")]
        public bool ContainsImageBubbles { get; set; }
    }

    public partial class ReadingModes
    {
        [JsonProperty("text")]
        public bool Text { get; set; }

        [JsonProperty("image")]
        public bool Image { get; set; }
    }
}
