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

        public AdminMainWindow()
        {

            InitializeComponent();

            AdminMainFrame.Content = new OverdueBooks();

            // Place class in main window context
            this.DataContext = new UserDetails();

        }

        public class UserDetails
        {
            public string imageSourceUrl { get; set; }
            public string userFullName { get; set; }
            public UserDetails()
            {
                // Set the URL for the image source
                imageSourceUrl = "https://via.placeholder.com/150x150";

                string userFirstName = "Ben";
                string userLastName = "Tutheridge";
                userFullName = userFirstName + " " + userLastName;
            }
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
