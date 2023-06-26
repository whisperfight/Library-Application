using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;


namespace Library_Application
{
    /// <summary>
    //The EditLibraryUsers class is a WPF page for managing library users.
    //It displays a list of users, allows sorting by different criteria, and supports adding, editing, and removing users.
    //The user data is retrieved from a database using LINQ queries.The class also defines a nested class UserList to represent the user data.
    /// </summary>
    public partial class EditLibraryUsers : Page
    {
        public List<UserList> listData = new List<UserList>();  // Declare a list to store user data

        public EditLibraryUsers()
        {
            InitializeComponent();  // Initialize the component
            LoadDatabase();  // Load user data from the database
            SortByID(listData);  // Sort the data by ID
        }

        public void DisplayListData(List<UserList> data)
        {
            // Display number of listing/sort results
            int resultsCount = listData.Count();  // Get the count of user data
            ResultsCounter.Text = "Showing " + resultsCount.ToString() + " results";  // Update the ResultsCounter Text property

            ListView UserListControl = this.UserListControl;  // Get the reference to the ListView control
            UserListControl.ItemsSource = data;  // Set the ItemsSource of the ListView to the userlist
        }

        // Sorting methods
        public void SortByID(List<UserList> input)
        {
            listData = input.OrderBy(item => item.ID).ToList();  // Sort the user data by ID
            DisplayListData(listData);  // Display the sorted data
        }

        public void SortByAtoZ(List<UserList> input)  // Sort alphabetically A to Z
        {
            listData = input.OrderBy(item => item.FirstName).ToList();
            DisplayListData(listData);
        }

        public void SortByZtoA(List<UserList> input)  // Sort alphabetically Z to A
        {
            listData = input.OrderByDescending(item => item.FirstName).ToList();
            DisplayListData(listData);
        }

        public void LoadDatabase()
        {
            using (var db = new DataContext())  // Create a new instance of the DataContext
            {
                var libraryUsers = (from u in db.Users  // Query the Users table
                                    join l in db.Loans on u.ID equals l.UserID into loans  // Perform a left join with the Loans table
                                    from loan in loans.DefaultIfEmpty()  // Perform a left join by using DefaultIfEmpty() method
                                    select new  // Create an anonymous type with selected properties
                                    {
                                        u.ID,
                                        u.Username,
                                        u.FirstName,
                                        u.LastName,
                                        u.IsAdmin,
                                        u.IsEnabled,
                                        DueDate = loan != null ? loan.DueDate : (DateTime?)null  // Nullable DateTime - checks if the loan object is not null
                                    }).ToList().GroupBy(
                                        x => x.ID,  // Group the data by user ID
                                        (key, g) =>
                                        new UserList  // Create a new UserList object for each group
                                {
                                            ID = key,
                                            UserName = g.First().Username,
                                            FirstName = g.First().FirstName,
                                            LastName = g.First().LastName,
                                            IsAdmin = g.First().IsAdmin,
                                            IsEnabled = g.First().IsEnabled,
                                            IssuedCount = g.Count(),
                                            OverdueBookCount = g.Count(s => s.DueDate.HasValue && DateTime.Compare(s.DueDate.Value, DateTime.Now) <= 0)
                                        }).ToList();  // Convert the grouped data to a list of UserList objects

                listData = libraryUsers;  // Assign the fetched user data to the listData variable
            }
        }


        private void RemoveEntry_Click(object sender, RoutedEventArgs e)
        {
            UserList selectedItem = (UserList)UserListControl.SelectedItem;  // Get the selected item from the UserListControl

            // Check if an item is selected
            if (selectedItem != null)
            {
                using (var db = new DataContext())  // Create a new instance of the DataContext
                {
                    User deleteUser = new User();  // Create a new User object

                    deleteUser.ID = selectedItem.ID;  // Set the ID of the User object to the ID of the selected item
                    db.Remove(deleteUser);  // Remove the User object from the DataContext
                    db.SaveChanges();  // Save the changes to the database

                }
                // Refresh list control
                LoadDatabase();  // Reload the user data from the database
                DisplayListData(listData);  // Display the updated list of users in the UserListControl
            }
        }


        private void AddNewMember_Click(object sender, RoutedEventArgs e)
        {

            int editMode = 1; // Sets window to add new member edit mode

            UserWindow userWindow = new UserWindow(editMode, 0);

            // Handle the Closing event
            userWindow.Closing += AddUserWindowClosing;
            userWindow.ShowDialog(); // Show the new window as a modal dialog

        }


        private void EditSelected_Click(object sender, RoutedEventArgs e)
        {

            UserList selectedUser = (UserList)UserListControl.SelectedItem;

            if (selectedUser != null)
            {
                int editMode = 2; // Sets window to add edit selected member mode
                int selUserID = selectedUser.ID;

                UserWindow userWindow = new UserWindow(editMode, selUserID);

                // Handle the Closing event
                userWindow.Closing += AddUserWindowClosing;
                userWindow.ShowDialog(); // Show the new window as a modal dialog
            }

        }

        private void AddUserWindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Method to execute when the UserWindow is closing, refresh listview data
            LoadDatabase();
            DisplayListData(listData);
        }

        private void SortByDropdown_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;

            // Check if an item is selected
            if (comboBox.SelectedItem != null)
            {
                var selectedItem = (ComboBoxItem)comboBox.SelectedValue;

                // Get the content of the selected item
                var selectedContent = selectedItem.Content;

                // Perform actions based on the selected item
                switch (selectedContent)
                {
                    case "ID number":
                        SortByID(listData);
                        break;
                    case "First name A to Z":
                        SortByAtoZ(listData);
                        break;
                    case "First name Z to A":
                        SortByZtoA(listData);
                        break;
                    default:;
                        break;
                }
            }
        }
    }
}


public class UserList
{
    public int ID { get; set; }
    public string UserName { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public bool IsAdmin { get; set; }
    public DateTime LastLoginDate { get; set; }
    public bool IsEnabled { get; set; }
    public string Password { get; set; }
    public string ImageURL { get; set; }
    public int IssuedCount { get; set; }
    public int OverdueBookCount { get; set; }

}


