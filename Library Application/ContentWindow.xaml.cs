using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Library_Application
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class ContentWindow : Window, INotifyPropertyChanged
    {

        int editMode = 1; // 1 = add new member 2 = edit selected user
        int selectedBookID;

        public ContentWindow(int mode, int ID)
        {
            editMode = mode;
            selectedBookID = ID;

            // Place class in main window context
            this.DataContext = this;

            InitializeComponent();

            if (editMode == 2)
            {
                //Load and edit existing user
                LoadSelectedBookData(selectedBookID);
            }

        }

        private void LoadSelectedBookData(int bookID)
        {
            using (var db = new DataContext())
            {

                List<Book> selBook = db.Books.Where(x => x.ID == bookID).ToList();


                // Update text fields to selected member
                BookTitleField.Text = selBook[0].Title;
                BookAuthorField.Text = selBook[0].Author;
                GenreField.Text = selBook[0].GenreTags;
                TimeToRead.Text = selBook[0].TimeToRead.ToString();
                RatingField.Text = selBook[0].Rating.ToString();
                BookDescriptionField.Text = selBook[0].Summary;
                ImageURLField.Text = selBook[0].CoverImageURL;

                // Update selected combobox states
                if (selBook[0].AvailableToLoan == true)
                {
                    Available.IsSelected = true;
                }
                else
                {
                    Loaned.IsSelected = true;
                }
            }
        }

        private int GetNewPKFAddNewBook()
        {
            int newpkID = 0;

            using (var db = new DataContext())
            {
                var bookTable = db.Books.ToList();
                newpkID = bookTable.Count() + 1;
            }

            return newpkID;
        }

        private void btnTestImageLink_Click(object sender, RoutedEventArgs e)
        {
            ImageURL = ImageURLField.Text;
        }

        private string imageURL;
        public string ImageURL
        {
            get { return imageURL; }
            set
            {
                imageURL = value;
                // Call OnPropertyChanged whenever the property is updated
                OnPropertyChanged();
            }
        }

        private void btnAddNewContent_Click(object sender, RoutedEventArgs e)
        {

            double bookRating = Double.Parse(RatingField.Text);
            int timeToRead = Int32.Parse(TimeToRead.Text);

            //Change logic based on content mode
            switch (editMode)
            {
                
                case 1:
                    // Add new book to database
                    //Basic data validation to prevent empty book entries
                    if (BookTitleField.Text != "" ||
                        BookAuthorField.Text != "" ||
                        GenreField.Text != "" ||
                        bookRating >= 0 && bookRating <= 5 ||
                        BookDescriptionField.Text != "" ||
                        ImageURLField.Text != "")
                    {
                        using (var db = new DataContext())
                        {

                            Book newBook = new Book();

                            // Call getPKF method to get new ID
                            newBook.ID = GetNewPKFAddNewBook();
                            newBook.Title = BookTitleField.Text;
                            newBook.Author = BookAuthorField.Text;
                            newBook.GenreTags = GenreField.Text;
                            newBook.Rating = bookRating;
                            newBook.TimeToRead = timeToRead;
                            newBook.Summary = BookDescriptionField.Text;
                            newBook.CoverImageURL = ImageURLField.Text;

                            db.Add(newBook);
                            db.SaveChanges();

                            ConfirmMessage.Text = "Changes saved!";
                        }
                    }
                    else
                    {
                        ConfirmMessage.Text = "Missing data, Please fill out all entry fields!";
                    }
                    break;
                case 2:
                    // Edit existing book on database
                    //Basic data validation to prevent empty book entries
                    if (BookTitleField.Text != "" ||
                        BookAuthorField.Text != "" ||
                        GenreField.Text != "" ||
                        bookRating >= 0 && bookRating <= 5 ||
                        BookDescriptionField.Text != "" ||
                        ImageURLField.Text != "")
                    {
                        using (var db = new DataContext())
                        {
                            int bookID = selectedBookID; // Assign book ID from selected book

                            // Retrieve the existing book entry from the database
                            Book existingBook = db.Books.FirstOrDefault(u => u.ID == bookID);

                            if (existingBook != null)
                            {
                                // Modify the properties of the existing book object
                                existingBook.Title = BookTitleField.Text;
                                existingBook.Author = BookAuthorField.Text;
                                existingBook.GenreTags = GenreField.Text;
                                existingBook.TimeToRead = timeToRead;
                                existingBook.Rating = bookRating;
                                existingBook.AvailableToLoan = isAvailable;
                                existingBook.CoverImageURL = ImageURLField.Text;

                                // Save the changes back to the database
                                db.SaveChanges();

                                ConfirmMessage.Text = "Changes saved!";
                            }
                            else
                            {
                                ConfirmMessage.Text = "User not found!";
                            }
                        }

                        ConfirmMessage.Text = "Changes saved!";
                    }
                    else
                    {
                        ConfirmMessage.Text = "Missing data, Please fill out all entry fields!";
                    }
                    break;
                default:
                    break;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        bool isAvailable = true;

        private void LoanStatusCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;

            // Check if an item is selected
            if (comboBox.SelectedItem != null)
            {
                ComboBoxItem selectedItem = (ComboBoxItem)comboBox.SelectedItem;
                string selectedContent = selectedItem.Name.ToString();

                // Perform actions based on the selected item
                switch (selectedContent)
                {
                    case "Available":
                        isAvailable = true;

                        // Set current item as selected
                        selectedItem.IsSelected = true;
                        break;
                    case "Loaned":
                        // Handle standard privileges selection
                        isAvailable = false;

                        // Set current item as selected
                        selectedItem.IsSelected = true;
                        break;
                    default:
                        // Handle other selections or the default case
                        break;
                }
            }
        }
    }


}

public class Book
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
    public bool AvailableToLoan { get; set; }
    public string DueDate { get; set; }
}
