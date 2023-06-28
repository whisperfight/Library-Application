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
    public partial class MainWindow : Window, INotifyPropertyChanged  // Represents the main window of the library application
    {
        public event EventHandler<UserDetailsEventArgs> UserDetailsLoaded;

        public MainWindow()  // Initializes the main window
        {
            DataContext = this;
            InitializeComponent();
            LoginFrame.Content = new LoginPage();
        }

        public void LoadUserDetails(int loggedInUserID)
        {
            using (var db = new DataContext())
            {
                UserDetails loadedUserDetails = (from u in db.Users
                                                 where u.ID == loggedInUserID
                                                 select new UserDetails
                                                 {
                                                     Username = u.Username,
                                                     FirstName = u.FirstName,
                                                     LastName = u.LastName,
                                                     IsAdmin = u.IsAdmin,
                                                     ImageURL = u.ImageURL
                                                 }).FirstOrDefault();

                userDetails = loadedUserDetails;

                // Raise the event with the loaded user details
                UserDetailsLoaded?.Invoke(this, new UserDetailsEventArgs(loadedUserDetails));
            }

            // Load default page content based on user type.
            if (userDetails.IsAdmin == true)
            {
                MainFrame.Content = new OverdueBooks();
            }
            else
            {
                MainFrame.Content = new HomeDash();
            }
        }

        private UserDetails updateDetails;
        public UserDetails userDetails
        {
            get { return updateDetails; }
            set
            {
                updateDetails = value;
                OnPropertyChanged();

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

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private void LogoutUser_Click(object sender, RoutedEventArgs e)
        {
            LoginFrame.Content = new LoginPage();
            // Toggle visibility states
            ((MainWindow)App.Current.MainWindow).LoginFrame.Visibility = Visibility.Visible;
            ((MainWindow)App.Current.MainWindow).AfterLogin.Visibility = Visibility.Hidden;
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

public class UserDetailsEventArgs : EventArgs
{
    public UserDetails UserDetails { get; }

    public UserDetailsEventArgs(UserDetails userDetails)
    {
        UserDetails = userDetails;
    }
}
