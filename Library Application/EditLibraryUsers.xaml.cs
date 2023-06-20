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
            //using (var db = new DataContext())
            //{
            //    var LibraryUsers = (from l in db.Loans
            //                        join u in db.Users on l.UserID equals u.ID
            //                        select new
            //                        {
            //                            u.ID,
            //                            u.Username,
            //                            u.FirstName,
            //                            u.LastName,
            //                            u.IsAdmin,
            //                            u.IsEnabled,
            //                            l.DueDate

            //                        }).ToList().GroupBy(
            //                            x => x.ID,
            //                            (key, g) =>
            //                            new UserList
            //                            {
            //                                ID = key,
            //                                UserName = g.ToList().First().Username,
            //                                FirstName = g.ToList().First().FirstName,
            //                                LastName = g.ToList().First().LastName,
            //                                IsAdmin = g.ToList().First().IsAdmin,
            //                                IsEnabled = g.ToList().First().IsEnabled,
            //                                IssuedCount = g.ToList().Count(),
            //                                OverdueBookCount = g.ToList().Where(s => DateTime.Compare(s.DueDate, DateTime.Now) < 0).Count()
            //                            }
            //                        ).ToList();

            //    listData = LibraryUsers;
            //}

            using (var db = new DataContext())
            {
                var LibraryUsers = (from u in db.Users
                        
                                    //join l in db.Loans on u.ID equals l.UserID
                                    select

                                    new UserList
                                    {
                                        ID = u.ID,
                                        UserName = u.Username,
                                        FirstName = u.FirstName,
                                        LastName = u.LastName,
                                        IsAdmin = u.IsAdmin,
                                        IsEnabled = u.IsEnabled,
                                        //IssuedCount = l.ID,
                                        //OverdueBookCount = g.ToList().Where(s => DateTime.Compare(s.DueDate, DateTime.Now) < 0).Count()


                                    }).ToList();

                listData = LibraryUsers;
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
