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
            {

                //Perform two separate queries one to retrieve the loan count and another to retrieve the overdue book count
                //Then join between the Users table and the results of userLoans and userBooks to obtain the desired information in the libraryUsers query.

                // Retrieve loaned items based on matching user table "ID" with the loaned items table "User ID"
                var userLoans = (from u in db.Users
                                 join l in db.Loans on u.ID equals l.UserID into loans
                                 from loan in loans
                                 select new //Select user book
                                 {
                                     UserId = u.ID, // Assign user to book
                                     BookID = loan.BookID, // Retrieve Users loaned book ID
                                     BookDueDate = loan.DueDate, // Retrieve Users loaned book due date
                                 }).ToList();



                //// Retrieve loaned items based on matching user table "ID" with the loaned items table "User ID"
                //var userBookCount = (from u in db.Users
                //                    join l in db.Loans on u.ID equals l.UserID into loans
                //                     select new //Select user book
                //                     {
                //                         UserID = u.ID, // Assign user to book
                //                         BooksIssued = loans.Count(),

                //                     }).ToList();


                //// Debug
                //foreach (var loan in userLoans)
                //{
                //    Console.WriteLine("User ID = " + loan.UserId);
                //    Console.WriteLine("Loan ID = " + loan.BookID);
                //    Console.WriteLine("Issue date = " + loan.BookDueDate);

                //    int issuedCount = userLoans.Count();
                //}


                // Retrieve book due-date information based on referencing the loan table "BookID" with the books table "ID"
                var userBooks = (from ul in userLoans
                                 join b in db.Books on ul.BookID equals b.ID
                                 join u in db.Users on ul.UserId equals u.ID
                                 select new
                                 {
                                     UserID = u.ID,
                                     BookTitle = b.Title,
                                     BookDueDate = ul.BookDueDate
                                  }).ToList();


                //Debug
                foreach (var book in userBooks)
                {
                    Console.WriteLine("User = " + book.UserID);
                    Console.WriteLine("Title = " + book.BookTitle);

                    int overdueCount = userBooks.Count(b => DateTime.Compare(b.BookDueDate, DateTime.Now) < 0);

                    Console.WriteLine("Overdue count = " + overdueCount);
                }


                //var libraryUsers = (from ul in userLoans
                //                    join ub in userBooks on ul.UserId equals ub.UserId
                //                    join u in db.Users on ul.UserId equals u.ID
                //                    select new UserList
                //                    {
                //                        ID = u.ID,
                //                        UserName = u.Username,
                //                        FirstName = u.FirstName,
                //                        LastName = u.LastName,
                //                        IsAdmin = u.IsAdmin,
                //                        IsEnabled = u.IsEnabled,
                //                        //IssuedCount = userLoans.Count(),
                //                        //OverdueBookCount = userBooks.Count(b => DateTime.Compare(b.BookDueDate, DateTime.Now) < 0) 
                //                    }).ToList();


                var libraryUsers = (from u in db.Users
                                    //join ubc in userBookCount on u.ID equals ubc.UserID
                                    select new UserList
                                    {
                                        ID = u.ID,
                                        UserName = u.Username,
                                        FirstName = u.FirstName,
                                        LastName = u.LastName,
                                        IsAdmin = u.IsAdmin,
                                        IsEnabled = u.IsEnabled,
                                        //IssuedCount = ubc.BooksIssued,
                                        //OverdueBookCount = userBooks.Count(b => DateTime.Compare(b.BookDueDate, DateTime.Now) < 0) 
                                    }).ToList();

                listData = libraryUsers;
            } //Check with Robert

            using (var db = new DataContext)
            {

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
