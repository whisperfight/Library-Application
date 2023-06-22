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
    /// Interaction logic for EditLibraryUsers.xaml
    /// </summary>
    public partial class EditLibraryUsers : Page
    {
        public List<UserList> listData = new List<UserList>();

        public EditLibraryUsers()
        {
            InitializeComponent();
            LoadDatabase();
            SortByID(listData);

        }

        public void DisplayListData(List<UserList> data)
        {
            //Display number of listing/sort results
            int resultsCount = listData.Count();
            ResultsCounter.Text = "Showing " + resultsCount.ToString() + " results";

            ListView LoanListControl = this.UserListControl;
            LoanListControl.ItemsSource = data; // Set the ItemsSource of the ListView to the loanList
        }


        
        public void SortByID(List<UserList> input)
        {
            listData = input.OrderBy(item => item.ID).ToList();
            DisplayListData(listData);
        }

        public void SortByAtoZ(List<UserList> input) // Sort alphabetically A to Z
        {
            listData = input.OrderBy(item => item.FirstName).ToList();
            DisplayListData(listData);
        }

        public void SortByZtoA(List<UserList> input) // Sort alphabetically A to Z
        {
            listData = input.OrderByDescending(item => item.FirstName).ToList();
            DisplayListData(listData);
        }

        public void LoadDatabase()
        {
            using (var db = new DataContext())
            {
                var libraryUsers = (from u in db.Users
                                    join l in db.Loans on u.ID equals l.UserID into loans
                                    from loan in loans.DefaultIfEmpty() // Perform a left join

                                    select new
                                    {
                                        u.ID,
                                        u.Username,
                                        u.FirstName,
                                        u.LastName,
                                        u.IsAdmin,
                                        u.IsEnabled,
                                        DueDate = loan != null ? loan.DueDate : (DateTime?)null // Nullable DateTime - checks if the loan object is not null
                                    }).ToList().GroupBy(
                                        x => x.ID,
                                        (key, g) =>
                                        new UserList
                                        {
                                            ID = key,
                                            UserName = g.First().Username,
                                            FirstName = g.First().FirstName,
                                            LastName = g.First().LastName,
                                            IsAdmin = g.First().IsAdmin,
                                            IsEnabled = g.First().IsEnabled,
                                            IssuedCount = g.Count(),
                                            OverdueBookCount = g.Count(s => s.DueDate.HasValue && DateTime.Compare(s.DueDate.Value, DateTime.Now) <= 0)
                                        }).ToList();

                listData = libraryUsers;
            }
        }


        private void RemoveEntry_Click(object sender, RoutedEventArgs e)
        {
            UserList selectedItem = (UserList)UserListControl.SelectedItem;

            // Check if an item is selected
            if (selectedItem != null)
            {

                using (var db = new DataContext())
                {
                    User deleteUser = new User();

                    deleteUser.ID = selectedItem.ID;
                    db.Remove(deleteUser);
                    db.SaveChanges();

                }
                // Refresh list control
                LoadDatabase();
                DisplayListData(listData);
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


