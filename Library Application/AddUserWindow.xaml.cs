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
using System.Windows.Shapes;

namespace Library_Application
{
    /// <summary>
    /// Interaction logic for AddMemberWindow.xaml
    /// </summary>
    public partial class AddUserWindow : Window
    {

        bool isAdmin = false;
        bool isEnabled = true;

        public AddUserWindow()
        {
            InitializeComponent();
        }

        private int GetNewPKFAddNewMember()
        {
            int newpkID = 0;

            using (var db = new DataContext())
            {
                var userTable = db.Users.ToList();
                for (int i = 0; i < userTable.Count(); i++)
                {
                    newpkID = userTable[i].ID;
                }
            }
            newpkID = newpkID + 1;
            return newpkID;
        }

        private void btnTestImageLink_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnAddNewMember_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new DataContext())
            {
                User newUser = new User();

                // Call getPKF method to get new ID
                newUser.ID = GetNewPKFAddNewMember();
                newUser.FirstName = FirstNameField.ToString();
                newUser.LastName = LastNameField.ToString();
                newUser.Username = UserNameField.ToString();
                newUser.IsAdmin = isAdmin;
                newUser.IsEnabled = isEnabled;
                newUser.ImageURL = ImageURLField.ToString();

                db.Add(newUser);
                db.SaveChanges();
            }

        }

        private void UserPrivilegesCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
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
                    case "AdminTrue":
                        // Handle admin privileges selection
                        isAdmin = true;

                        // Set current item as selected
                        selectedItem.IsSelected = true;
                        break;
                    case "AdminFalse":
                        // Handle standard privileges selection
                        isAdmin = false;

                        // Set current item as selected
                        selectedItem.IsSelected = true;
                        break;
                    default:
                        // Handle other selections or the default case
                        break;
                }
            }
        }


        private void UserEnabledCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
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
                    case "AccountEnabled":
                        // Handle admin privileges selection
                        isEnabled = true;
                        // Set current item as selected
                        selectedItem.IsSelected = true;
                        break;

                    case "AccountDisabled":
                        // Handle standard privileges selection
                        isEnabled = false;
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
public class User
{
    public int ID { get; set; }
    public string Username { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public bool IsAdmin { get; set; }
    public DateTime LastLoginDate { get; set; }
    public bool IsEnabled { get; set; }
    public string Password { get; set; }
    public string ImageURL { get; set; }
}





