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
    /// <summary>
    /// Interaction logic for AddMemberWindow.xaml
    /// </summary>
    public partial class UserWindow : Window, INotifyPropertyChanged
    {

        int editMode = 1; // 1 = add new member 2 = edit selected user
        int selectedID;

        public UserWindow(int mode, int ID)
        {
            editMode = mode;
            selectedID = ID;

            // Place class in main window context
            this.DataContext = this;

            InitializeComponent();

            switch (editMode)
            {
                case 1: //Add new user

                    break;
                case 2: //Load and edit existing user
                    LoadSelectedUserData(selectedID);
                    break;
                default:
                    break;


            }

        }

        private void LoadSelectedUserData(int userID)
        {
            using (var db = new DataContext())
            {

                List<User> selUser = db.Users.Where(x => x.ID == userID).ToList();


                // Update text fields to selected member
                FirstNameField.Text = selUser[0].FirstName;
                LastNameField.Text = selUser[0].LastName;
                UserNameField.Text = selUser[0].Username;
                UserPasswordField.Text = selUser[0].Password;
                ImageURLField.Text = selUser[0].ImageURL;

                // Update selected combobox states

                // Admin privs
                if (selUser[0].IsAdmin == true)
                {
                    AdminTrue.IsSelected = true;

                }
                else
                {
                    AdminFalse.IsSelected = true;
                }

                if (selUser[0].IsEnabled == true)
                {
                    AccountEnabled.IsSelected = true;
                }
                else
                {
                    AccountDisabled.IsSelected = true;
                }

            }
        }

        private int GetNewPKFAddNewMember()
        {
            int newpkID = 0;

            using (var db = new DataContext())
            {
                var userTable = db.Users.ToList();
                newpkID = userTable.Count() + 1;
            }
 
            return newpkID;
        }

        // Test and update image button / property change event
        private void btnTestImageLink_Click(object sender, RoutedEventArgs e)
        {
            ImageURL = ImageURLField.Text;
        }
        private string imageURL;
        public string ImageURL
        {
            get { return imageURL; }
            set
            {
                imageURL = value;
                // Call OnPropertyChanged whenever the property is updated
                OnPropertyChanged();
            }
        }

        private void btnAddNewMember_Click(object sender, RoutedEventArgs e)
        {

            //Change logic based on content mode
            switch (editMode)
            {
                case 1:
                    // Add new user to database
                    //Basic data validation to prevent empty user entries
                    if (FirstNameField.Text != "" ||
                        LastNameField.Text != "" ||
                        UserNameField.Text != "" ||
                        ImageURLField.Text != "")
                    {
                        using (var db = new DataContext())
                        {
                            User newUser = new User();

                            // Call getPKF method to get new ID
                            newUser.ID = GetNewPKFAddNewMember();
                            newUser.FirstName = FirstNameField.Text;
                            newUser.LastName = LastNameField.Text;
                            newUser.Username = UserNameField.Text;
                            newUser.Password = UserPasswordField.Text;
                            newUser.IsAdmin = isAdmin;
                            newUser.IsEnabled = isEnabled;
                            newUser.ImageURL = ImageURLField.Text;

                            db.Add(newUser);
                            db.SaveChanges();

                            ConfirmMessage.Text = "Changes saved!";
                        }
                    }
                    else
                    {
                        ConfirmMessage.Text = "Missing data, Please fill out all entry fields!";
                    }
                    break;

                case 2:
                    // Edit selected user data and update records

                    if (FirstNameField.Text != "" ||
                       LastNameField.Text != "" ||
                       UserNameField.Text != "" ||
                       ImageURLField.Text != "")
                    {
                        using (var db = new DataContext())
                        {
                            int userId = GetUserIdToUpdate(); // Replace this with your logic to fetch the user ID to update

                            // Retrieve the existing user entry from the database
                            User existingUser = db.Users.FirstOrDefault(u => u.ID == userId);

                            if (existingUser != null)
                            {
                                // Modify the properties of the existing user object
                                existingUser.FirstName = FirstNameField.Text;
                                existingUser.LastName = LastNameField.Text;
                                existingUser.Username = UserNameField.Text;
                                existingUser.Password = UserPasswordField.Text;
                                existingUser.IsAdmin = isAdmin;
                                existingUser.IsEnabled = isEnabled;
                                existingUser.ImageURL = ImageURLField.Text;

                                // Save the changes back to the database
                                db.SaveChanges();

                                ConfirmMessage.Text = "Changes saved!";
                            }
                            else
                            {
                                ConfirmMessage.Text = "User not found!";
                            }
                        }

                        ConfirmMessage.Text = "Changes saved!";
                    }
                    else
                    {
                        ConfirmMessage.Text = "Missing data, Please fill out all entry fields!";
                    }
                    break;
                default:
                    break;
            }
        }

        bool isAdmin = false;
        bool isEnabled = false;

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
                        SelectPriv.IsSelected = false;
                        break;
                    case "AdminFalse":
                        // Handle standard privileges selection
                        isAdmin = false;

                        // Set current item as selected
                        selectedItem.IsSelected = true;
                        SelectPriv.IsSelected = false;
                        break;
                    default:
                        // Handle other selections or the default case
                        SelectPriv.IsSelected = true;
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

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
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




