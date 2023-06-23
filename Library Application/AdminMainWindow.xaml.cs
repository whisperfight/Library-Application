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
            // Place class in main window context
            this.DataContext = this;

            LoadUserDetails();

            InitializeComponent();

            AdminMainFrame.Content = new OverdueBooks();
        }


        public void LoadUserDetails()
        {
            using (var db = new DataContext())
            {
                userDetails = (from u in db.Users
                                   where u.ID == currentActiveUserID // Only retrieve the details of the active user
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

        //The statement public UserDetails userDetails { get; set; } is a property declaration in the AdminMainWindow class.
        //Properties provide a way to encapsulate data and define access to that data through getter and setter methods.
        //By using this property, other parts of the code can access and modify the UserDetails object associated with the AdminMainWindow instance. 

        public UserDetails userDetails
        {
            get;set;
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
