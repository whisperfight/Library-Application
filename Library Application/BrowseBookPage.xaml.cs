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

            // Get the books from the AWS Cloud PostgreSQL database.
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

            // Display the book count
            ResultsCounter.Text = "Showing " + bookItems.Count().ToString() + " Results";
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

            TextBox textBox = (TextBox)sender;
            search_text = textBox.Text;

            if (search_text != "Enter text")
            {
                // Filter books on the 'title' DB column with a LIKE type operator.
                // Filtering is to be done based on a partial match op. For example
                // if the user enters 'Jar', the DB should be searched based on 'jar' being 
                // within the title. Also, the search term is forced into lowercase. The idea
                // behind this is to catch user error where case is concerned.
                var BooksFiltered = (from b in bookItems
                                     where b.title.ToLower().Contains(search_text.ToLower())
                                     select b).ToList();

                BookListView.ItemsSource = BooksFiltered;
                ResultsCounter.Text = "Showing " + BooksFiltered.Count().ToString() + " Results";
            }


        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            // the listing was not being updated to reflected the user's typed in search query.
            // As strange as it seems, it was necessary to put the following lines of code in to ensure
            // the ListView control updated to reflect user input.
            // BookListView.ItemsSource = null resets the ListView control;
            // bookItems.Clear(); empties the global bookitems object which holds the data from the DB.
            BookListView.ItemsSource = null;
            bookItems.Clear();


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

                // Ben made an object for use with the display control, 'BookItem'.
                // The code below is necessary to overcome the anonymous type issue with respect to using
                // the BookItem class in relation to the LINQ query. Also, Ben made some UI features tha require 
                // the data from the DB to be "modified" for use. One example of this is 'ConvertLoanState'. Special text needs
                // to be displayed instead of the boolean from the database.

                // In hindsight it might have been possible to not require this code. But there will also sorts
                // of issues during development that ate into time (and thinking)!
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
                ResultsCounter.Text = "Showing " + bookItems.Count().ToString() + " Results";
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
            // A book has been clicked on. Take the user to the ViewBook page and pass into this
            // page the BookItem object. This is necessary as a DB call has to be made to get the
            // book details and 'somehow' the BookID from this page needs to be passed to the page
            // being called. The Book ID is being retrieved by accessing the BookItem instance in the
            // called page.
            if (BookListView.SelectedItem != null)
            {
                BookItem selectedBook = (BookItem)BookListView.SelectedItem;

                MainWindow mainWindow = (MainWindow)App.Current.MainWindow;

                // Pass the BookItem object to the new page via the constructor.
                // This constructor has two parameters: BookItem and an ID.
                // called from the Home Dash page, an ID is passed in.
                // called from the Browse Book page, a BookItem object is passed in.
                // Both scenarios need to be supported.
                mainWindow.MainFrame.Content = new ViewBook(selectedBook, 0);

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

            // The user can sort by three different options: by genre, availability and sort order
            // This code sorts by any order of the user's choice.
            // Because this event handler is run on Page Load (yes), all the below checks are necessary
            // in order to stop an exception from being thrown.

            string selectedGenre = "";
            string selectedAvailability = "";
            string selectedSortBy = "";

            // Has a genre been chosen?
            if (GenreDropdown != null && GenreDropdown.SelectedIndex > 0)
            {
                selectedGenre = ((ComboBoxItem)GenreDropdown.SelectedItem).Content.ToString();
            }

            // Has an availability option been chosen, be it On Loan or Available
            if (AvailabilityDropdown != null && AvailabilityDropdown.SelectedIndex > 0)
            {
                selectedAvailability = ((ComboBoxItem)AvailabilityDropdown.SelectedItem).Content.ToString();
            }

            // Is sorting to be done?
            if (SortByDropdown != null && SortByDropdown.SelectedIndex > 0)
            {
                selectedSortBy = ((ComboBoxItem)SortByDropdown.SelectedItem).Content.ToString();
            }

            if (selectedSortBy == "A to Z")
            {
                if (bookItems != null)
                {
                    var BooksFiltered = (from b in bookItems
                                         orderby b.title ascending
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
                    ResultsCounter.Text = "Showing " + BooksFiltered.Count().ToString() + " Results";
                }
            }
            else if (selectedSortBy == "Z to A")
            {
                if (bookItems != null)
                {
                    var BooksFiltered = (from b in bookItems
                                         orderby b.title descending
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
                    ResultsCounter.Text = "Showing " + BooksFiltered.Count().ToString() + " Results";
                }
            }
            else
            {
                // no ordering of results.

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
                    ResultsCounter.Text = "Showing " + BooksFiltered.Count().ToString() + " Results";
                }
            }
        }
    }
}
