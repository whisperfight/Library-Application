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
    public partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();

            string CurrentLoginUserName = UserNameField.Text;
            string type = PasswordField.Text;

            //string CurrentLoginUserName = (string)App.Current.Properties["CurrentLoginUserName"];
            //string type = (string)App.Current.Properties["CurrentLoginUserNameType"];
            //int ID = (int)App.Current.Properties["CurrentLoginUserNameID"];

            // Display the user's name on the form.
            List<User> ulist = null;
            using (var db = new DataContext())
            {
                // Get the user's details from the database.
                var userlist = db.Users.Where(x => x.Username == CurrentLoginUserName);
                if (userlist != null)
                {
                    ulist = userlist.ToList();
                }
            }
            // this will be a row count of 1
            if (ulist.Count > 0)
            {
                if (type == "admin")
                {
                    lblUsersname.Content += "Administrator" + Environment.NewLine;
                    lblUsersname.Content += ulist[0].FirstName + " " + ulist[0].LastName;
                }
                else
                {
                    lblUsersname.Content = ulist[0].FirstName + " " + ulist[0].LastName;
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)  // Handles the click event for the SUBMIT button
        {
           
            
        }

        private void MainFrame_Navigated(object sender, NavigationEventArgs e)
        {

        }
    }
}
