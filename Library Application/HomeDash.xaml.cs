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

        //List<BookItem> bookItems;

        public HomeDash()
        {
            InitializeComponent();

            // Adding data contexts to loaded page
            this.DataContext = this;
            this.DataContext = new UserDetails();

            LoadUserLoanList();

            LoadUserWishList();

        }

        public class UserDetails
        {
            public string welcomeMessage { get; set; }
            public UserDetails()
            {
                string userFirstName = "Ben";
                string userLastName = "Tutheridge";
                welcomeMessage = "Welcome " + userFirstName + " " + userLastName;
            }
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
                                   where l.UserID == 4
                                   select new
                                   {
                                       u.FirstName,
                                       l.BookID,
                                       b.Title,
                                       b.CoverImageURL,
                                       b.GenreTags,
                                       b.AvailableToLoan
                                   }).ToList();


                // It's necessary to create a class for display of the data as some data conversion needs to be done
                // Also a foreach loop doesn't work with anonymous types (the 'var loanedBooks').
                for (int i = 0; i < loanedBooks.Count(); i++)
                {
                    LoanItem liItem = new LoanItem();
                    liItem.ID = loanedBooks[i].BookID.ToString();
                    liItem.genre = loanedBooks[i].GenreTags;
                    liItem.title = loanedBooks[i].Title;
                    liItem.imgURL = loanedBooks[i].CoverImageURL;
                    liItem.loanstate = ConvertLoanState(loanedBooks[i].AvailableToLoan);

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
                                     where l.UserID == 4
                                     select new
                                     {
                                         b.ID,
                                         u.FirstName,
                                         b.Title,
                                         b.CoverImageURL,
                                         b.GenreTags,
                                         b.AvailableToLoan
                                     }).ToList();

                for (int i = 0; i < wishlistBooks.Count(); i++)
                {
                    WishlistItem wishItem = new WishlistItem();
                    wishItem.ID = wishlistBooks[i].ID.ToString();
                    wishItem.genre = wishlistBooks[i].GenreTags;
                    wishItem.imgURL = wishlistBooks[i].CoverImageURL;
                    wishItem.title = wishlistBooks[i].Title;
                    wishItem.loanstate = ConvertLoanState(wishlistBooks[i].AvailableToLoan);

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
        private string ConvertLoanState(bool loanStateInput)
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

        private void LoanedListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LoanedListView.SelectedItem != null)
            {
                LoanItem selectedBook = (LoanItem)LoanedListView.SelectedItem;

                MainWindow mainWindow = (MainWindow)App.Current.MainWindow;

                // Pass the BookItem object to the new page via the constructor.
                mainWindow.MainFrame.Content = new ViewBook(null, Convert.ToInt32(selectedBook.ID));

            }
        }

        private void WishListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (WishListView.SelectedItem != null)
            {
                WishlistItem selectedBook = (WishlistItem)WishListView.SelectedItem;

                MainWindow mainWindow = (MainWindow)App.Current.MainWindow;

                // Pass the BookItem object to the new page via the constructor.
                mainWindow.MainFrame.Content = new ViewBook(null, Convert.ToInt32(selectedBook.ID));
            }
        }
    }
}
