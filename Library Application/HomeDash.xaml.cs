using System;
using System.Collections.Generic;
using System.IO;
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

namespace Library_Application
{
    /// <summary>
    /// Interaction logic for HomeDash.xaml
    /// </summary>
    public partial class HomeDash : Page
    {

        int loggedInUserID = App.LoggedInUserID;

        public HomeDash()
        {
            InitializeComponent();

            // Adding data contexts to loaded page
            this.DataContext = this;
            this.DataContext = new UserDetails();

            LoadUserLoanList();
            LoadUserWishList();
        }

        private void LoadUserLoanList()
        {
            List<LoanItem> loanedbookstoshow = new List<LoanItem>();

            using (var db = new DataContext())
            {
                var loanedBooks = (from u in db.Users
                                   join l in db.Loans
                                   on u.ID equals l.UserID
                                   join b in db.Books
                                   on l.BookID equals b.ID
                                   where l.UserID == loggedInUserID // Select data based on current logged in UserID
                                   select new
                                   {
                                       u.FirstName,
                                       l.BookID,
                                       b.Title,
                                       b.ID,
                                       b.CoverImageURL,
                                       b.GenreTags,
                                       b.AvailableToLoan
                                   }).ToList();


                // It's necessary to create a class for display of the data as some data conversion needs to be done
                // Also a foreach loop doesn't work with anonymous types (the 'var loanedBooks').
                for (int i = 0; i < loanedBooks.Count(); i++)
                {
                    LoanItem liItem = new LoanItem();
                    liItem.genre = loanedBooks[i].GenreTags;
                    liItem.title = loanedBooks[i].Title;
                    liItem.bookID = loanedBooks[i].ID;
                    liItem.imgURL = loanedBooks[i].CoverImageURL;
                    liItem.loanState = ConvertLoanState(loanedBooks[i].AvailableToLoan);

                    loanedbookstoshow.Add(liItem);
                }
            }

            LoanedListView.ItemsSource = loanedbookstoshow;
        }

        private void LoadUserWishList()
        {

            List<WishlistItem> wishlistbookstoshow = new List<WishlistItem>();
            using (var db = new DataContext())
            {
                // Get the wishlist books data for the user dashboard.
                // three tables involved: 1) Users 2) Wishlist and 3) Books.
                var wishlistBooks = (from u in db.Users
                                     join l in db.Wishlist
                                     on u.ID equals l.UserID
                                     join b in db.Books
                                     on l.BookID equals b.ID
                                     where l.UserID == loggedInUserID // Select data based on current logged in UserID
                                     select new
                                     {
                                         u.FirstName,
                                         b.Title,
                                         b.ID,
                                         b.CoverImageURL,
                                         b.GenreTags,
                                         b.AvailableToLoan
                                     }).ToList();

                for (int i = 0; i < wishlistBooks.Count(); i++)
                {
                    WishlistItem wishItem = new WishlistItem();
                    wishItem.genre = wishlistBooks[i].GenreTags;
                    wishItem.imgURL = wishlistBooks[i].CoverImageURL;
                    wishItem.title = wishlistBooks[i].Title;
                    wishItem.bookID = wishlistBooks[i].ID;
                    wishItem.loanState = ConvertLoanState(wishlistBooks[i].AvailableToLoan);

                    wishlistbookstoshow.Add(wishItem);
                }
            }

            WishListView.ItemsSource = wishlistbookstoshow;
        }

        // Mouse over event - Show Hover Information
        private void WrapPanel_MouseEnter(object sender, MouseEventArgs e)
        {
            WrapPanel wrapPanel = (WrapPanel)sender;

            // Hide book cover on mouse over
            WrapPanel hidebook = (WrapPanel)wrapPanel.FindName("SelectedBook");
            hidebook.Visibility = Visibility.Collapsed;

            // Show book info on mouse over
            WrapPanel showinfo = (WrapPanel)wrapPanel.FindName("HiddenBookInfo");
            showinfo.Visibility = Visibility.Visible;
        }

        // Mouse leave event - Hide Hover Information
        private void WrapPanel_MouseLeave(object sender, MouseEventArgs e)
        {
            WrapPanel wrapPanel = (WrapPanel)sender;

            // Show book cover on mouse leave
            WrapPanel showbook = (WrapPanel)wrapPanel.FindName("SelectedBook");
            showbook.Visibility = Visibility.Visible;

            // Hide book info on mouse leave
            WrapPanel hideinfo = (WrapPanel)wrapPanel.FindName("HiddenBookInfo");
            hideinfo.Visibility = Visibility.Collapsed;

        }

        // Utility method to address the conversion of a bool data type to a string.
        // This is for the AvailableToLoan property.
        private static string ConvertLoanState(bool loanStateInput)
        {
            if (loanStateInput == true)
            {
                return "Available";
            }
            else
            {
                return "On Loan";
            }
        }

        // Event to pass LoanItem details through to the viewbook window - converts from LoanItem to BookItem
        private void LoanedListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (LoanedListView.SelectedItem != null)
            {

                using (var db = new DataContext())
                {
                    // Get the book ID from the selected loan item
                    LoanItem selectedLoanedBook = (LoanItem)LoanedListView.SelectedItem;

                    // Select and pass book item that matches loanitem ID
                    var selectedBookItem = (from b in db.Books
                                            where b.ID == Convert.ToInt32(selectedLoanedBook.bookID)
                                            select new BookItem
                                            {
                                            bookID = b.ID.ToString(),
                                            title = b.Title,
                                            author = b.Author,
                                            summary = b.Summary,
                                            timeToRead = b.TimeToRead.ToString(),
                                            rating = b.Rating.ToString(),
                                            genre = b.GenreTags,
                                            imgURL = b.CoverImageURL,
                                            loanState = ConvertLoanState(b.AvailableToLoan),
                                                
                                            newRelease = b.NewRelease.ToString(),
                                            dueDate = b.DueDate
                                            }).FirstOrDefault();

                    if (selectedBookItem != null)
                    {
                        MainWindow mainWindow = (MainWindow)App.Current.MainWindow;
                        mainWindow.MainFrame.Content = new ViewBook(selectedBookItem);
                    }

                }
            }
        }

        private void WishListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (WishListView.SelectedItem != null)
            {

                using (var db = new DataContext())
                {
                    // Get the book ID from the selected loan item
                    WishlistItem selectedWishlistBook = (WishlistItem)WishListView.SelectedItem;


                    // Select and pass book item that matches loanitem ID
                    var selectedBookItem = (from b in db.Books
                                            where b.ID == Convert.ToInt32(selectedWishlistBook.bookID)
                                            select new BookItem
                                            {
                                                bookID = b.ID.ToString(),
                                                title = b.Title,
                                                author = b.Author,
                                                summary = b.Summary,
                                                timeToRead = b.TimeToRead.ToString(),
                                                rating = b.Rating.ToString(),
                                                genre = b.GenreTags,
                                                imgURL = b.CoverImageURL,
                                                loanState = ConvertLoanState(b.AvailableToLoan),
                                                newRelease = b.NewRelease.ToString(),
                                                dueDate = b.DueDate
                                            }).FirstOrDefault();

                    if (selectedBookItem != null)
                    {
                        MainWindow mainWindow = (MainWindow)App.Current.MainWindow;
                        mainWindow.MainFrame.Content = new ViewBook(selectedBookItem);
                    }

                }
            }
        }
    }
}
