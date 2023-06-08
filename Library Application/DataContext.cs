using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library_Application
{
    public class DataContext : DbContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Loan> Loans { get; set; }
        public DbSet<Wishlist> Wishlists { get; set; }
        public DbSet<BookReview> BookReviews { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseNpgsql("Host=library.ctovtf8jnr2m.ap-southeast-2.rds.amazonaws.com;Port=5432;Username=postgres;Password=2021Shades!;Database=Library");
    }

    public class Book
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Summary { get; set; }
        public int TimeToRead { get; set; }
        public int Rating { get; set; }
        public bool NewRelease { get; set; }
        public string Genre { get; set; }
        public string FileName { get; set; }
    }

    public class User
    {
        public int ID { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsAdmin { get; set; }
        public DateTime LastLoginDate { get; set; }
        public bool IsEnabled { get; set; }
        public string Password { get; set; }
    }

    public class Loan
    {
        public int ID { get; set; }
        public int BookID { get; set; }
        public int UserID { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime DueDate { get; set; }
        public bool FineDue { get; set; }
        public double FineAmount { get; set; }

    }

    public class Wishlist
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public int BookID { get; set; }
        public DateTime AddedDate { get; set; }
    }

    public class BookReview
    {
        public int ID { get; set; }
        public int BookID { get; set; }
        public int Rating { get; set; }
    }

}
