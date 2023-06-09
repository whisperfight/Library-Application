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
            // Read the csv file
            // Get the current working directory of the application
            string currentDirectory = Directory.GetCurrentDirectory();

            // Remove the last "\bin\Debug" from the current directory path
            string parentDirectory = Directory.GetParent(currentDirectory).FullName;


            // Combine the parent directory with the relative path to the CSV file
            string filePath = System.IO.Path.Combine(parentDirectory, "BookList.csv");
            string bookSummaryPath = System.IO.Path.Combine(parentDirectory, "BookSummary.csv");

            Console.WriteLine(filePath);

            // Read the CSV file

            var lines = File.ReadAllLines(filePath);
            var BookSummaryLines = File.ReadAllLines(bookSummaryPath);

            bookItems = new List<BookItem>();

            //skip the first header line
            for (var i = 1; i < lines.Length; i++)
            {
                //split each line into array of string
                var line = lines[i].Split(',');

                string loanState = line[7];

                if (loanState == "TRUE")
                {
                    loanState = "Available";
                }
                else
                {
                    loanState = "On Loan";
                }

                //create new animal item instance and add it to the animal list
                bookItems.Add(new BookItem
                {
                    bookID = line[0],
                    title = line[1],
                    author = line[2],
                    summary = BookSummaryLines[i],
                    timeToRead = line[3],
                    rating = line[4],
                    genre = line[5],
                    imgURL = line[6],
                    loanState = loanState, // Assign the loanstate variable
                    newRelease = line[8],
                    dueDate = line[9]
                }); ;
            }

            BookListView.ItemsSource = bookItems;

        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            search_text = textBox.Text;
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine(search_text);
            ResultsCounter.Text = search_text;
            SearchTextBox.Text = "Enter Text";

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


    }
}
