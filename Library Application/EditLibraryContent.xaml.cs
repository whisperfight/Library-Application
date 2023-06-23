using System;
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

namespace Library_Application
{
    /// <summary>
    /// Interaction logic for Page2.xaml
    /// </summary>
    public partial class EditLibraryContent : Page
    {

        public List<Book> listData = new List<Book>();

        public EditLibraryContent()
        {
            InitializeComponent();

            LoadDatabase();
            SortByID(listData); 
            DisplayListData(listData);

        }

        public void DisplayListData(List<Book> data)
        {

            //Display number of listing/sort results
            int resultsCount = listData.Count();
            ResultsCounter.Text = "Showing " + resultsCount.ToString() + " results";

            ListView LoanListControl = this.LoanListControl;
            LoanListControl.ItemsSource = data; // Set the ItemsSource of the ListView to the loanList
        }

        public void SortByID(List<Book> input)
        {
            listData = input.OrderBy(item => item.ID).ToList();
            DisplayListData(listData);
        }

        public void SortTitleByAtoZ(List<Book> input)
        {
            listData = input.OrderBy(item => item.Title).ToList();
            DisplayListData(listData);
        }

        public void SortTitleByZtoA(List<Book> input) 
        {
            listData = input.OrderByDescending(item => item.Title).ToList();
            DisplayListData(listData);
        }

        public void SortByRating(List<Book> input)
        {
            listData = input.OrderByDescending(item => item.Rating).ToList();
            DisplayListData(listData);
        }

        public void LoadDatabase()
        {
            using (var db = new DataContext())
            {
                var catalog = (from b in db.Books
                               select new Book
                               {
                                   ID = b.ID,
                                   Title = b.Title,
                                   Author = b.Author,
                                   Summary = b.Summary,
                                   TimeToRead = b.TimeToRead,
                                   Rating = b.Rating,
                                   NewRelease = b.NewRelease,
                                   GenreTags = b.GenreTags,
                                   CoverImageURL = b.CoverImageURL,
                                   AvailableToLoan = b.AvailableToLoan,
                                   DueDate = b.DueDate
                               }).ToList();

                listData = catalog;
            }
        }

        private void SortByDropdown_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;

            // Check if an item is selected
            if (comboBox.SelectedItem != null)
            {
                var selectedItem = (ComboBoxItem)comboBox.SelectedValue;

                // Get the content of the selected item
                var selectedContent = selectedItem.Content;

                // Perform actions based on the selected item
                switch (selectedContent)
                {
                    case "Book ID number":
                        SortByID(listData);
                        break;
                    case "Title A to Z":
                        SortTitleByAtoZ(listData);
                        break;
                    case "Title Z to A":
                        SortTitleByZtoA(listData);
                        break;
                    case "Rating":
                        SortByRating(listData);
                        break;
                    default:
                        ;
                        break;
                }
            }
        }
    }

}


