using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;

using Library_Application;
using static Library_Application.BrowseBookPage;


namespace Library_Application
{
    /// <summary>
    /// Interaction logic for ViewBook.xaml
    /// </summary>
    public partial class ViewBook : Page
    {
        private static List<BookItem> bookItem;  // Stores a list of book items

        // This page is now to be called from three different places: 
        // Browse books, and Home Dash (loaned books listing and wishlist listing).
        // Browse books requires the BookItem object, whereas the two places from the
        // home dash requires the ID (of the book).
        public ViewBook(BookItem selectedBook, int IDofBook)
        {
            InitializeComponent();

            // Dom's code.

            if (IDofBook > 0)
            {
                // Get the book details from the database. Only run if coming from HomeDash.
                List<Book> bookObj = new List<Book>();
                BookItem bi = null;
                using (var db = new DataContext())
                {
                    bookObj = db.Books.Where(x => x.ID == IDofBook).ToList();
                }
                // convert into a BookItem object
                if (bookObj != null && bookObj.Count() > 0)
                {
                    for (int i = 0; i < bookObj.Count(); i++)
                    {
                        bi = new BookItem();
                        bi.author = bookObj[0].Author;
                        bi.bookID = bookObj[0].ID.ToString();
                        bi.dueDate = bookObj[0].DueDate;
                        bi.genre = bookObj[0].GenreTags;
                        bi.imgURL = bookObj[0].CoverImageURL;
                        bi.loanState = ConvertLoanState(bookObj[0].AvailableToLoan);
                        bi.newRelease = ConvertNewRelease(bookObj[0].NewRelease);
                        bi.rating = bookObj[0].Rating.ToString();
                        bi.summary = bookObj[0].Summary;
                        bi.timeToRead = bookObj[0].TimeToRead.ToString();
                        bi.title = bookObj[0].Title;
                    }
                    // Assign to Ben's BookItem property.
                    BookItem = bi;
                }
            }
            else
            {
                // Coming from the browse books page.
                BookItem = selectedBook;
            }

            // Adding data contexts to loaded page
            this.DataContext = this;  // Set the data context of the loaded page to itself

            SubheadingContent();

        }

        public BookItem BookItem { get; set; }  // Represents the selected book item

        private void SubheadingContent()

        {
            BookItem.author = "Author : " + BookItem.author;
            BookItem.genre = "Genre : " + BookItem.genre;

            txtRating.Text = BookItem.rating;
        }

        private void ReturnPageButton_Click(object sender, RoutedEventArgs e)
        {
            // Retrieves the reference to the main window to access the window frame
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.MainFrame.Content = new HomeDash();
        }

        private void btnLoanBook_Click(object sender, RoutedEventArgs e)
        {
            // insert an entry into the Loans table
            using (var db = new DataContext())
            {
                Loan liItem = new Loan();
                liItem.BookID = Convert.ToInt32(BookItem.bookID);

                // temporarily hard coded. It will be necessary to access the global variable that will be added later.
                liItem.UserID = 4;
                liItem.DueDate = DateTime.Now.AddDays(30);
                liItem.FineDue = false;
                liItem.IssueDate = DateTime.Now;
                liItem.ID = GetNewPKFromLoanTable(); // need to get PK ID from table.

                db.Add(liItem);
                db.SaveChanges();
            }
        }

        private void btnAddToWishList_Click(object sender, RoutedEventArgs e)
        {
            // insert an entry into the Wishlist table
            using (var db = new DataContext())
            {
                Wishlist wlItem = new Wishlist();
                wlItem.AddedDate = DateTime.Now;
                wlItem.BookID = Convert.ToInt32(BookItem.bookID);
                wlItem.ID = GetNewPKFromWishlistTable(); // need to get the latest PK ID.

                // temporarily hard coded. It will be necessary to access the global variable that will be added later.
                wlItem.UserID = 4;

                db.Add(wlItem);
                db.SaveChanges();

            }
        }

        // Utility functions specific to this page.
        // These get the last issued Primary Key number found in the table concerned, then increase by one.
        // This is necessary for the calling routine.
        private int GetNewPKFromLoanTable()
        {
            int newpkID = 0;

            using (var db = new DataContext())
            {
                var loanTable = db.Loans.ToList();
                for (int i = 0; i < loanTable.Count(); i++)
                {
                    newpkID = loanTable[i].ID;
                }
            }
            newpkID = newpkID + 1;
            return newpkID;
        }

        private int GetNewPKFromWishlistTable()
        {
            int newpkID = 0;

            using (var db = new DataContext())
            {
                var wishTable = db.Wishlist.ToList();
                for (int i = 0; i < wishTable.Count(); i++)
                {
                    newpkID = wishTable[i].ID;
                }
            }
            newpkID = newpkID + 1;
            return newpkID;
        }

        private int GetNewPKFromBookReviewTable()
        {
            int newpkID = 0;

            using (var db = new DataContext())
            {
                var bkrvTable = db.BookReview.ToList();
                for (int i = 0; i < bkrvTable.Count(); i++)
                {
                    newpkID = bkrvTable[i].ID;
                }
            }
            newpkID = newpkID + 1;
            return newpkID;
        }

        string ConvertLoanState(bool input)
        {
            if (input == true)
            {
                return "On Loan";
            }
            else
            {
                return "Available";
            }
        }

        string ConvertNewRelease(bool input)
        {
            if (input == true)
            {
                return "Yes";
            }
            else
            {
                return "No";
            }
        }

        private void cmbRating_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Insert an entry into the BookReview table.
            // The rating as found in the BookItem object is an AVG of the values in this table.
            // The functionality to make this happen is not in place.

            if (BookItem != null && cmbRating.SelectedIndex > 0)
            {
                using (var db = new DataContext())
                {
                    string selection = ((ComboBoxItem)cmbRating.SelectedItem).Content.ToString();

                    BookReview bkRv = new BookReview();
                    bkRv.ID = GetNewPKFromBookReviewTable();
                    bkRv.BookID = Convert.ToInt32(BookItem.bookID);
                    bkRv.Rating = Convert.ToInt32(selection);

                    db.Add(bkRv);
                    db.SaveChanges();
                }
            }
        }
    }
}
