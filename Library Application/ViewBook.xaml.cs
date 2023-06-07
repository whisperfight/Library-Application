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

namespace Library_Application
{
    /// <summary>
    /// Interaction logic for ViewBook.xaml
    /// </summary>
    public partial class ViewBook : Page
    {

        private static List<BookItem> bookItems;

        //string bookID;

        public ViewBook()
        {
            InitializeComponent();

            // Adding data contexts to loaded page
            this.DataContext = this;

            LoadBookListings();

        }

        public class BookItem
        {

            public string bookID { get; set; }
            public string title { get; set; }
            public string author { get; set; }
            public string summary { get; set; }
            public string timeToRead { get; set; }
            public string rating { get; set; }
            public string genre { get; set; }
            public string imgURL { get; set; }
            public string loanState { get; set; }
            public string newRelease { get; set; }
            public string dueDate { get; set; }

            public Brush LoanStatusFill
            {
                get
                {
                    if (loanState == "On Loan")
                    {
                        return new SolidColorBrush(Color.FromRgb(114, 114, 114));
                    }
                    else
                    {
                        return new SolidColorBrush(Color.FromRgb(58, 177, 155));
                    }
                }
            }

            public static string GetCoverImage(string bookID)
            {
                // Find the book with the specified bookID in the bookItems list
                BookItem book = bookItems.FirstOrDefault(b => b.bookID == bookID);

                if (book != null)
                {
                    // Return the image URL of the found book
                    return book.imgURL;
                }

                // Return an empty string if the book is not found
                return string.Empty;
            }
        }

        private static void LoadBookListings()
        {
            // Read the csv file
            // Get the current working directory of the application
            string currentDirectory = Directory.GetCurrentDirectory();

            // Remove the last "\bin\Debug" from the current directory path
            string parentDirectory = Directory.GetParent(currentDirectory).FullName;


            // Combine the parent directory with the relative path to the CSV file
            string filePath = System.IO.Path.Combine(parentDirectory, "BookList.csv");

            Console.WriteLine(filePath);

            // Read the CSV file

            var lines = File.ReadAllLines(filePath);

            bookItems = new List<BookItem>();

            //skip the first header line
            for (var i = 1; i < lines.Length; i++)
            {
                //split each line into array of string
                var line = lines[i].Split(',');

                string loanState = line[8];

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
                    summary = line[3],
                    timeToRead = line[4],
                    rating = line[5],
                    genre = line[6],
                    imgURL = line[7],
                    loanState = loanState, // Assign the loanstate variable
                    newRelease = line[9],
                    dueDate = line[10]
                }); ;
            }
        }

        private void ReturnPageButton_Click(object sender, RoutedEventArgs e)
        {
            string bookID = "1"; // Specify the book's ID

            string bookCoverURL = BookItem.GetCoverImage(bookID);
        }


    }
}
