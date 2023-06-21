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
            DisplayListData(listData);
        }

        public void DisplayListData(List<UserList> data)
        {
            //Display number of listing/sort results
            int resultsCount = listData.Count();
            ResultsCounter.Text = "Showing " + resultsCount.ToString() + " results";

            ListView LoanListControl = this.UserListControl;
            LoanListControl.ItemsSource = data; // Set the ItemsSource of the ListView to the loanList
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
                                        u.FirstName,
                                        u.LastName,
                                        DueDate = loan != null ? loan.DueDate : (DateTime?)null // Nullable DateTime - checks if the loan object is not null
                                    }).ToList().GroupBy(
                                        x => x.ID,
                                        (key, g) =>
                                        new UserList
                                        {
                                            ID = key,
                                            FirstName = g.First().FirstName,
                                            LastName = g.First().LastName,
                                            IssuedCount = g.Count(),
                                            OverdueBookCount = g.Count(s => s.DueDate.HasValue && DateTime.Compare(s.DueDate.Value, DateTime.Now) <= 0)
                                        }).ToList();

                listData = libraryUsers;
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
            User selectedUser = (User)UserListControl.SelectedItem;

            int editMode = 2; // Sets window to add edit selected member mode

            int selUserID = selectedUser.ID;

            UserWindow userWindow = new UserWindow(editMode, selUserID);

            // Handle the Closing event
            userWindow.Closing += AddUserWindowClosing;

            userWindow.ShowDialog(); // Show the new window as a modal dialog
        }

        private void AddUserWindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Method to execute when the UserWindow is closing
            DisplayListData(listData);
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


