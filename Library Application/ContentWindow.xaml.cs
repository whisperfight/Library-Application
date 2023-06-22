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
        public ContentWindow()
        {
            // Place class in main window context
            this.DataContext = this;

            InitializeComponent();
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

            //Basic data validation to prevent empty user entries

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
