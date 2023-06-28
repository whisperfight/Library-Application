using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library_Application
{
    // Represents the data context for the library application
    public class DataContext : DbContext
    {
        // Represents a table of books in the library
        public DbSet<Book> Books { get; set; }
        // Represents a table of users in the library
        public DbSet<User> Users { get; set; }
        // Represents a table of loans in the library
        public DbSet<Loan> Loans { get; set; }
        // Represents a table of wishlist in the library
        public DbSet<Wishlist> Wishlist { get; set; }
        // Represents a table of book reviews in the library
        public DbSet<BookReview> BookReview { get; set; }

        // Configures the database connection for the library application
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseNpgsql("Host=library.ctovtf8jnr2m.ap-southeast-2.rds.amazonaws.com;Port=5432;Username=postgres;Password=2021Shades!;Database=Library");
    }

    // Represents a book in the library
    public class Book
    {
        public int ID { get; set; }  // The unique identifier of the book
        public string Title { get; set; }  // The title of the book
        public string Author { get; set; }  // The author of the book
        public string Summary { get; set; }  // The summary of the book
        public int TimeToRead { get; set; }  // The estimated tme to read the book
        public double Rating { get; set; }  // The rating of the book
        public bool NewRelease { get; set; }  // Indicates if the book is a new release
        public string GenreTags { get; set; }  // The genre tags associated with the book
        public string CoverImageURL { get; set; }  // The URL of the book cover image
        public bool AvailableToLoan { get; set; }  // Indicates if the book is available for loan
        public string DueDate { get; set; }  // The due date of the book if it is on loan
    }

    // Represents a user in the library
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
        public string ImageURL { get; set; }

    }

    // Represents a loan of a book in the library
    public class Loan
    {
        public int ID { get; set; }
        public int BookID { get; set; }
        public int UserID { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime DueDate { get; set; }
        public bool FineDue { get; set; }
        public double FineAmount { get; set; }
        public int OverdueBy { get; set; }


    }

    // Represents a wishlist item of a user in the library
    public class Wishlist
    {
        public int ID { get; set; }  // The unique identifier of the wishlist item
        public int UserID { get; set; }  // The ID of the user who added the book to the wishlist
        public int BookID { get; set; }  // The ID of the book in the wishlist
        public DateTime AddedDate { get; set; }  // The date and time when the book was added to the wishlist
    }

    // Represents a book review in the library
    public class BookReview
    {
        public int ID { get; set; }  // The unique identifier of the book review
        public int BookID { get; set; }  // The ID of the book being reviewed
        public int Rating { get; set; }  // The rating given to the book in the review
    }

}
