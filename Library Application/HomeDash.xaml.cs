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

        List<BookItem> bookItems;

        public HomeDash()
        {
            InitializeComponent();

            this.DataContext = this;

            LoadUserLoanList();

            LoadUserWishList();

        }


        public class BookItem
        {
            public string name { get; set; }
            public string genre { get; set; }
            public string imgURL { get; set; }
            public string loanState { get; set; }

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
        }

        private void LoadUserLoanList()
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


                if (line[3] == "TRUE")
                {
                    line[3] = "Available";
                }
                else
                {
                    line[3] = "On Loan";
                }

                //create new animal item instance and add it to the animal list
                bookItems.Add(new BookItem
                {
                    name = line[0],
                    genre = line[1],
                    imgURL = line[2],
                    loanState = line[3]
                });
            }

            LoanedListView.ItemsSource = bookItems;

        }

        private void LoadUserWishList()
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


                if (line[3] == "TRUE")
                {
                    line[3] = "Available";
                }
                else
                {
                    line[3] = "On Loan";
                }

                //create new animal item instance and add it to the animal list
                bookItems.Add(new BookItem
                {
                    name = line[0],
                    genre = line[1],
                    imgURL = line[2],
                    loanState = line[3]
                });
            }

            WishListView.ItemsSource = bookItems;

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
    }
}
