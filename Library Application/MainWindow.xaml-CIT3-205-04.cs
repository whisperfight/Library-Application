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

        int loggedInUserID;

        public MainWindow()  // Initializes the main window
        {

            InitializeComponent();

            LoginFrame.Content = new LoginPage();
            MainFrame.Content = new BrowseBookPage();  // sets the initial content of the main frame to the browsebookpage
            this.DataContext = new UserDetails();  // Creates an instance of the UserDetails class and sets it as the data context for the main window

        }

        public void LoadUserDetails()
        {
            loggedInUserID = (int)App.Current.Properties["UserID"];

            using (var db = new DataContext())
            {
                userDetails = (from u in db.Users
                               where u.ID == loggedInUserID // Check what user is logged in and retrieve there details
                               select

                               new UserDetails
                               {
                                   Username = u.Username,
                                   FirstName = u.FirstName,
                                   LastName = u.LastName,
                                   IsAdmin = u.IsAdmin,
                                   ImageURL = u.ImageURL
                               }).FirstOrDefault();
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

        private void OverdueBooks_click(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = new OverdueBooks();
        }

        private void EditContent_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = new EditLibraryContent();
        }

        private void EditUsers_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = new EditLibraryUsers();
        }

        public UserDetails userDetails
        {
            get; set;
        }
    }
}

public class UserDetails
{
    public string Username { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }

    public string FullName
    {
        get { return FirstName + " " + LastName; }
    }

    public string UserType
    {
        get
        {
            if (IsAdmin == true)
            {
                return "Administrator";
            }
            else
            {
                return "Library member";
            }
        }
    }

    public bool IsAdmin { get; set; }
    public string ImageURL { get; set; }
}