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
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    /// 

    public partial class LoginPage : Page
    {
        string enteredLoginUserName;
        string enteredPassword;
        private SelectedUser selectedUser;
        bool showLoginPage = true;

        public LoginPage()
        {
            InitializeComponent();
            selectedUser = new SelectedUser();
        }

        private void Check_Credentials()
        {
            // Get field values
            enteredLoginUserName = UserNameField.Text;
            enteredPassword = PasswordField.Text;

            using (var db = new DataContext())
            {
                selectedUser = (from u in db.Users
                                where u.Username == enteredLoginUserName
                                select new SelectedUser
                                {
                                    UserID = u.ID,
                                    Username = u.Username,
                                    IsAdmin = u.IsAdmin,
                                    IsEnabled = u.IsEnabled,
                                    Password = u.Password
                                }).FirstOrDefault(); // Retrieve the first element from the selected collection.
            }

            if (selectedUser != null)
            {

                if (selectedUser.Password == enteredPassword && selectedUser.IsEnabled == true) //Check password and if account is enabled
                {
                    // Password correct - allow hiding of login page
                    showLoginPage = false;

                    if (selectedUser.IsAdmin == true) //Admin = true
                    {
                        // Toggle button view states
                        ((MainWindow)App.Current.MainWindow).AdminButtons.Visibility = Visibility.Visible;
                        ((MainWindow)App.Current.MainWindow).MemberButtons.Visibility = Visibility.Hidden;
                    }
                    else
                    {
                        ((MainWindow)App.Current.MainWindow).MemberButtons.Visibility = Visibility.Visible;
                        ((MainWindow)App.Current.MainWindow).AdminButtons.Visibility = Visibility.Hidden;
                    }

                }
                else
                {
                    // Display error message below entry fields
                    ErrorMessage.Text = "Incorrect details, try again!";
                }
            }
            else
            {
                // Display error message below entry fields
                ErrorMessage.Text = "No details entered, try again!";
            }
        }

        private void Submit_Button_Click(object sender, RoutedEventArgs e)
        {
            // Run compare password check and toggle settings
            Check_Credentials();

            // Hide page if credentials pass
            if (showLoginPage == false)
            {
                // Toggle visibility states
                ((MainWindow)App.Current.MainWindow).LoginFrame.Visibility = Visibility.Hidden;
                ((MainWindow)App.Current.MainWindow).AfterLogin.Visibility = Visibility.Visible;

                // In the other page where LoadUserDetails is called
                MainWindow mainWindow = Application.Current.MainWindow as MainWindow;
                if (mainWindow != null)
                {
                    mainWindow.UserDetailsLoaded += MainWindow_UserDetailsLoaded;
                    mainWindow.LoadUserDetails(selectedUser.UserID);
                }
            }
        }

        private void MainWindow_UserDetailsLoaded(object sender, UserDetailsEventArgs e)
        {
            // Update the displayed content using the loaded user details
            UserDetails loadedUserDetails = e.UserDetails;
            // Update the content in the other page based on the loaded user details
            // ...
        }

        private void UserInput_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            textBox.Text = "";      
        }
    }
}

public class SelectedUser
{
    public int UserID { get; set; }
    public string Username { get; set; }
    public bool IsAdmin { get; set; }
    public bool IsEnabled { get; set; }
    public string Password { get; set; }
}

