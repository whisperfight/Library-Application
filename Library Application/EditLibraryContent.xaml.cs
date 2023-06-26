using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;



namespace Library_Application
{
    /// <summary>

    //The EditLibraryContent class is a WPF page for managing library content.
    //It displays a list of books, allows sorting by different criteria,
    //and supports adding, editing, and removing books.


    /// </summary>
    public partial class EditLibraryContent : Page
    {


        public List<BookListing> listData = new List<BookListing>();


        public EditLibraryContent()
        {
            InitializeComponent();

            LoadDatabase();

            SortByID(listData);



            DisplayListData(listData);

        }


        public void DisplayListData(List<BookListing> data)
        {
            // Display the number of listings/sort results
            int resultsCount = listData.Count();
            ResultsCounter.Text = "Showing " + resultsCount.ToString() + " results";

            // Get a reference to the ContentListControl ListView
            ListView ContentListControl = this.ContentListControl;

            // Set the ItemsSource of the ListView to the provided data
            ContentListControl.ItemsSource = data;
        }

        // Sorting methods
        public void SortByID(List<BookListing> input)


        {
            listData = input.OrderBy(item => item.ID).ToList();
            DisplayListData(listData);
        }


        public void SortTitleByAtoZ(List<BookListing> input)

        {
            listData = input.OrderBy(item => item.Title).ToList();
            DisplayListData(listData);
        }


        public void SortTitleByZtoA(List<BookListing> input)

        {
            listData = input.OrderByDescending(item => item.Title).ToList();
            DisplayListData(listData);
        }


        public void SortByRating(List<BookListing> input)

        {
            listData = input.OrderByDescending(item => item.Rating).ToList();
            DisplayListData(listData);
        }

        public void LoadDatabase()
        {

            // Create a new DataContext using a using statement
            using (var db = new DataContext())
            {
                // Query the Books table in the database and select the desired fields to create a catalog of BookListing objects
                var catalog = (from b in db.Books
                               select new BookListing

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

                                   AvailableToLoan = b.AvailableToLoan.ToString(), // Convert to string value,
                                   DueDate = b.DueDate
                               }).ToList();

                // Iterate over each BookListing in the catalog
                foreach (BookListing listing in catalog)
                {
                    // Check if AvailableToLoan is equal to the string "true"
                    if (listing.AvailableToLoan == "true")
                    {
                        // If true, set AvailableToLoan to "Yes"
                        listing.AvailableToLoan = "Yes";
                    }
                    else
                    {
                        // If false, set AvailableToLoan to "No"
                        listing.AvailableToLoan = "No";
                    }
                }
                // Assign the catalog to the listData variable


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
                // Create a new DataContext using a using statement
                using (var db = new DataContext())
                {
                    // Create a new instance of the Book class to represent the book to be deleted
                    Book deleteBook = new Book();

                    // Set the ID of the deleteBook object to the ID of the selected item
                    deleteBook.ID = selectedItem.ID;

                    // Remove the deleteBook object from the database
                    db.Remove(deleteBook);

                    // Save the changes made to the database
                    db.SaveChanges();
                }
                // Refresh the list control by reloading the database and displaying the updated data



                LoadDatabase();
                DisplayListData(listData);
            }
        }
    }

}

    // BookListing class with changes to accept boolean availbility as string
    public class BookListing
{
    public int ID { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public string Summary { get; set; }
    public int TimeToRead { get; set; }
    public double Rating { get; set; }
    public bool NewRelease { get; set; }
    public string GenreTags { get; set; }
    public string CoverImageURL { get; set; }
    public string AvailableToLoan { get; set; }
    public string DueDate { get; set; }
}

    // BookListing class with changes to accept boolean availbility as string
    public class BookListing
{
    public int ID { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public string Summary { get; set; }
    public int TimeToRead { get; set; }
    public double Rating { get; set; }
    public bool NewRelease { get; set; }
    public string GenreTags { get; set; }
    public string CoverImageURL { get; set; }
    public string AvailableToLoan { get; set; }
    public string DueDate { get; set; }
}


