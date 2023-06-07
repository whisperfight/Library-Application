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

    public partial class MainWindow : Window
    {

        public MainWindow()
        {

            InitializeComponent();

            MainFrame.Content = new BrowseBookPage();

            // Place class in main window context
            this.DataContext = new UserDetails();

        }

        public class UserDetails
        {
            public string ImageSourceUrl { get; set; }
            public string UsersName { get; set; }
            public UserDetails()
            {
                // Set the URL for the image source
                ImageSourceUrl = "https://via.placeholder.com/150x150";
                UsersName = "Ben Tutheridge";
            }
        }

        private void Home_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = new HomeDash();
        }

        private void Browsebooks_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = new BrowseBookPage();
        }

  
    }
}
