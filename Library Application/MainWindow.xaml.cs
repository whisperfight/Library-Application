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
    public partial class MainWindow : Window  // Represents the main window of the library application
    {
        public MainWindow()  // Initializes the main window
        {

            InitializeComponent();  

            MainFrame.Content = new BrowseBookPage();  // Sets the initial content of the main frame to the BrowseBookPage

            this.DataContext = new UserDetails();  // Creates an instance of the UserDetails class and sets it as the data context for the main window

        }

        public class UserDetails  // Represents the user details in the library application
        {
            public string imageSourceUrl { get; set; }  // The URL of the user's profile image
            public string userFullName { get; set; }  // The full name of the user
            public UserDetails()
            {

                using (var db = new DataContext())
                {

                    // need to get the UserID from the Global Variable.
                    // This has yet to implemented.
                    List<User> userItem = db.Users.Where(x => x.ID == 4).ToList();  // Retrieves the user item based on the UserID (which needs to be implemented)
                    imageSourceUrl = userItem[0].ImageURL;
                }
                string userFirstName = "Ben";
                string userLastName = "Tutheridge";
                userFullName = userFirstName + " " + userLastName;
            }
        }

        private void Home_Click(object sender, RoutedEventArgs e)  // Handles the click event for the Home button
        {
            // Display the Home page
            MainFrame.Content = new HomeDash();
        }

        private void Browsebooks_Click(object sender, RoutedEventArgs e)  // Handles the click event for the Browse Books button
        {
            // Display the Browse book page
            MainFrame.Content = new BrowseBookPage();
        }
    }
}
