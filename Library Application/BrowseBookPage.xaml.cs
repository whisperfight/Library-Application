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
    /// Interaction logic for BrowseBookPage.xaml
    /// </summary>
    public partial class BrowseBookPage : Page
    {
        string search_text;

        List<BookItem> bookItems;

        public BrowseBookPage()
        {
            InitializeComponent();

            UserName = "Robert";
            this.DataContext = this;

            search_text = "empty";

            LoadBookListings();
        }

        public string UserName { get; set; }
        private void LoadBookListings()
        {
            bookItems = new List<BookItem>();

            using (var db = new DataContext())
            {
                var bookslist = db.Books.ToList();

                for (int i = 0; i < bookslist.Count(); i++)
                {
                    bookItems.Add(new BookItem
                    {
                        bookID = Convert.ToString(bookslist[i].ID),
                        title = bookslist[i].Title,
                        author = bookslist[i].Author,
                        summary = bookslist[i].Summary,
                        timeToRead = Convert.ToString(bookslist[i].TimeToRead),
                        rating = Convert.ToString(bookslist[i].Rating),
                        genre = bookslist[i].GenreTags,
                        imgURL = bookslist[i].CoverImageURL,
                        loanState = ConvertLoanState(bookslist[i].AvailableToLoan), // Assign the loanstate variable
                        newRelease = ConvertNewRelease(bookslist[i].NewRelease),
                        dueDate = bookslist[i].DueDate
                    });
                }
            }
            BookListView.ItemsSource = bookItems;
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

            TextBox textBox = (TextBox)sender;
            search_text = textBox.Text;

            if (search_text != "Enter text")
            {

                var BooksFiltered = (from b in bookItems
                                     where b.title.ToLower().Contains(search_text.ToLower())
                                     select b).ToList();

                BookListView.ItemsSource = BooksFiltered;

            }
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {

            BookListView.ItemsSource = null;

            bookItems.Clear();

            // Dom's note: for some reason the result is not being reflected in the ListView control.

            using (var db = new DataContext())
            {
                // gets books by filter on the Title column - works ok.
                var BooksFiltered = (from b in db.Books
                                     where b.Title.Contains(SearchTextBox.Text)
                                     select
                                     new
                                     {
                                         b.ID,
                                         b.Title,
                                         b.CoverImageURL,
                                         b.Author,
                                         b.Summary,
                                         b.TimeToRead,
                                         b.Rating,
                                         b.GenreTags,
                                         b.AvailableToLoan,
                                         b.NewRelease,
                                         b.DueDate

                                     }).ToList();


                for (int i = 0; i < BooksFiltered.Count(); i++)
                {
                    bookItems.Add(new BookItem
                    {
                        bookID = Convert.ToString(BooksFiltered[i].ID),
                        title = BooksFiltered[i].Title,
                        author = BooksFiltered[i].Author,
                        summary = BooksFiltered[i].Summary,
                        timeToRead = Convert.ToString(BooksFiltered[i].TimeToRead),
                        rating = Convert.ToString(BooksFiltered[i].Rating),
                        genre = BooksFiltered[i].GenreTags,
                        imgURL = BooksFiltered[i].CoverImageURL,
                        loanState = ConvertLoanState(BooksFiltered[i].AvailableToLoan), // Assign the loanstate variable
                        newRelease = ConvertNewRelease(BooksFiltered[i].NewRelease),
                        dueDate = BooksFiltered[i].DueDate
                    });
                }


                BookListView.ItemsSource = bookItems;
            }

        }

        private void SearchTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            SearchTextBox.Text = ""; //On focus of search bar delete default text
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

        private void BookList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (BookListView.SelectedItem != null)
            {
                BookItem selectedBook = (BookItem)BookListView.SelectedItem;

                MainWindow mainWindow = (MainWindow)App.Current.MainWindow;
                mainWindow.MainFrame.Content = new ViewBook(selectedBook);

            }
        }

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

        private string ConvertNewRelease(bool newreleaseStateInput)
        {
            if (newreleaseStateInput == true)
            {
                return "true";
            }
            else
            {
                return "false";
            }
        }

        private void GenreDropdown_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedGenre = "";
            string selectedAvailability = "";
            string selectedSortBy = "";

            if (GenreDropdown != null && GenreDropdown.SelectedIndex > 0)
            {
                selectedGenre = ((ComboBoxItem)GenreDropdown.SelectedItem).Content.ToString();
            }

            if (AvailabilityDropdown != null && AvailabilityDropdown.SelectedIndex > 0)
            {
                selectedAvailability = ((ComboBoxItem)AvailabilityDropdown.SelectedItem).Content.ToString();
            }

            if (SortByDropdown != null && SortByDropdown.SelectedIndex > 0)
            {
                selectedSortBy = ((ComboBoxItem)SortByDropdown.SelectedItem).Content.ToString();
            }

            // TO DO: Cater for Ordering, ASC / DESC

            if (bookItems != null)
            {
                var BooksFiltered = (from b in bookItems
                                     where b.genre.Contains(selectedGenre) &&
                                     b.loanState.Contains(selectedAvailability)
                                     select
                                     new
                                     {
                                         b.bookID,
                                         b.title,
                                         b.imgURL,
                                         b.author,
                                         b.summary,
                                         b.timeToRead,
                                         b.rating,
                                         b.genre,
                                         b.loanState,
                                         b.newRelease,
                                         b.dueDate

                                     }).ToList();
                BookListView.ItemsSource = BooksFiltered;
            }

        }
    }
}
