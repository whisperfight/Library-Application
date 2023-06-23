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

            ListView ContentListControl = this.ContentListControl;
            ContentListControl.ItemsSource = data;
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

        private void AddNewBook_Click(object sender, RoutedEventArgs e)
        {
            int editMode = 1; // Sets window to add new book edit mode

            ContentWindow contentWindow = new ContentWindow(editMode, 0);

            // Handle the Closing event
            contentWindow.Closing += ContentWindow_Closing; ;
            contentWindow.ShowDialog(); // Show the new window as a modal dialog
        }



        private void EditSelected_Click(object sender, RoutedEventArgs e)
        {
            Book selectedBook = (Book)ContentListControl.SelectedItem;

            if (selectedBook != null)
            {
                int editMode = 2; // Sets window to add edit selected book mode
                int selUserID = selectedBook.ID;

                ContentWindow contentWindow = new ContentWindow(editMode, selUserID);

                // Handle the Closing event
                contentWindow.Closing += ContentWindow_Closing;
                contentWindow.ShowDialog(); // Show the new window as a modal dialog
            }
        }



        private void ContentWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Method to execute when the content window is closing, refresh listview data
            LoadDatabase();
            DisplayListData(listData);
        }

        private void RemoveEntry_Click(object sender, RoutedEventArgs e)
        {
            Book selectedItem = (Book)ContentListControl.SelectedItem;

            // Check if an item is selected
            if (selectedItem != null)
            {

                using (var db = new DataContext())
                {
                    Book deleteBook = new Book();

                    deleteBook.ID = selectedItem.ID;
                    db.Remove(deleteBook);
                    db.SaveChanges();

                }
                // Refresh list control
                LoadDatabase();
                DisplayListData(listData);
            }
        }
    }

}


