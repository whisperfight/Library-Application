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

    public partial class AdminMainWindow : Window
    {

        int currentActiveUserID = 3;

        public AdminMainWindow()
        {
            InitializeComponent();

            AdminMainFrame.Content = new OverdueBooks();


            LoadUserDetails();

            DisplayCurrentUserDetails();

            //// Place class in main window context
            //this.DataContext = new UserDetails();

        }


        public void LoadUserDetails()
        {
            using (var db = new DataContext())
            {
                var userDetails = (from u in db.Users
                                   where u.ID == currentActiveUserID // Only retrieve the details of the active user
                                   select

                                   new UserDetails
                                   {
                                       Username = u.Username,
                                       FirstName = u.FirstName,
                                       LastName = u.LastName,
                                       IsAdmin = u.IsAdmin,
                                       ImageURL = u.ImageURL
                                   });

            }
                            
        }

        public void DisplayCurrentUserDetails()
        {

            UserDetails currentUser = new UserDetails();

            // Set the URL for the image source
            var imageSourceUrl = currentUser.ImageURL;

        }



        public class UserDetails
        {
            public string Username { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public bool IsAdmin { get; set; }
            public string ImageURL { get; set; }

        }

        private void OverdueBooks_click(object sender, RoutedEventArgs e)
        {
            AdminMainFrame.Content = new OverdueBooks();
        }

        private void EditContent_Click(object sender, RoutedEventArgs e)
        {
            AdminMainFrame.Content = new EditLibraryContent();
        }

        private void EditUsers_Click(object sender, RoutedEventArgs e)
        {
            AdminMainFrame.Content = new EditLibraryUsers();
        }
    }
}
