using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;


namespace Library_Application
{
    /// <summary>
    //The code represents a partial class named OverdueBooks, which is a WPF page in a library application.
    //It contains various methods and event handlers related to displaying and manipulating overdue books.

    /// </summary>
    public partial class OverdueBooks : Page
    {

        int bookLoanPeriod = 30;
        public List<OverdueLoans> listData = new List<OverdueLoans>();

        public OverdueBooks()
        {
            InitializeComponent();
            LoadDatabase();
            SortByID(listData);

        }

        public void DisplayListData(List<OverdueLoans> data)
        {


            // Display number of listing/sort results

            int resultsCount = listData.Count();
            ResultsCounter.Text = "Showing " + resultsCount.ToString() + " results";

            ListView LoanListControl = this.LoanListControl;
            LoanListControl.ItemsSource = data; // Set the ItemsSource of the ListView to the loanList
        }

        // List sorting methods
        public void SortByID(List<OverdueLoans> input)
        {
            listData = input.OrderBy(item => item.ID).ToList();
            DisplayListData(listData);
        }

        public void SortByMostOverdue(List<OverdueLoans> input)
        {
            listData = input.OrderByDescending(item => item.OverdueBy).ToList();
            DisplayListData(listData);
        }
        public void SortByLeastOverdue(List<OverdueLoans> input)
        {
            listData = input.OrderBy(item => item.OverdueBy).ToList();
            DisplayListData(listData);
        }

        public void LoadDatabase()
        {
            using (var db = new DataContext())
            {
                var books = (from b in db.Books // Join the Books table
                             join l in db.Loans on b.ID equals l.BookID // Join the Loans table based on matching book IDs
                             join u in db.Users on l.UserID equals u.ID // Join the Users table based on matching user IDs
                             where l.OverdueBy != 0 // Filter out loans that are not overdue
                             where DateTime.Now.AddDays(bookLoanPeriod) > l.DueDate // Filter loans based on the due date and loan period
                             select new OverdueLoans // Use new class to store formatted data
                             {
                                 ID = b.ID,
                                 FirstName = u.FirstName,
                                 LastName = u.LastName,
                                 Title = b.Title,
                                 IssueDate = l.IssueDate,
                                 IssuePeriod = bookLoanPeriod.ToString(),
                                 OverdueBy = l.OverdueBy.ToString()
                             }).ToList(); // Convert the query results to a list

                listData = books; // Update the listData with the retrieved books
            }
        }

        private void RemoveEntry_Click(object sender, RoutedEventArgs e)
        {
            OverdueLoans selectedItem = (OverdueLoans)LoanListControl.SelectedItem;

            // Check if an item is selected
            if (selectedItem != null)
            {

                using (var db = new DataContext())
                {
                    Loan deleteLoanBook = new Loan();

                    deleteLoanBook.ID = selectedItem.ID;
                    db.Remove(deleteLoanBook);
                    db.SaveChanges();

                }
                // Refresh list control
                LoadDatabase();
                DisplayListData(listData);
            }
        }

        private void SortByDropdown_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = (ComboBox)sender;

            if (comboBox.SelectedItem != null)
            {
                var selectedItem = (ComboBoxItem)comboBox.SelectedValue;

                // Get the content of the selected item
                var selectedContent = selectedItem.Content;

                // Perform actions based on the selected item
                switch (selectedContent)
                {
                    case "Most overdue":

                        SortByMostOverdue(listData);
                        break;
                    case "Least overdue":
                        SortByLeastOverdue(listData);
                        break;
                    case "Book ID":
                        SortByID(listData);


                        break;
                    default:
                        // Handle other selections or the default case
                        break;
                }
            }


        }
    }
}

public class OverdueLoans
{
    public int ID { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Title { get; set; }
    public DateTime IssueDate { get; set; }
    public string IssuePeriod { get; set; }
    public string OverdueBy { get; set; }
}