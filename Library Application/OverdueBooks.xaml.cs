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
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class OverdueBooks : Page
    {

        int bookLoanPeriod = 30;
        public List<OverdueLoans> listData = new List<OverdueLoans>();

        public OverdueBooks()
        {
            InitializeComponent();
            LoadDatabase();
            DisplayListData(listData);
        }

        public void DisplayListData(List<OverdueLoans> data)
        {

            //Display number of listing/sort results
            int resultsCount = listData.Count();
            ResultsCounter.Text = "Showing " + resultsCount.ToString() + " results";

            ListView LoanListControl = this.LoanListControl;
            LoanListControl.ItemsSource = data; // Set the ItemsSource of the ListView to the loanList
        }

        public void LoadDatabase()
        {
            using (var db = new DataContext())
            {
                var books = (from b in db.Books
                            join l in db.Loans on b.ID equals l.BookID
                            join u in db.Users on l.UserID equals u.ID
                            where l.OverdueBy != 0
                            where DateTime.Now.AddDays(bookLoanPeriod) > l.DueDate
                            select

                            // Use new class to store formatted data
                            new OverdueLoans
                            {
                                ID = b.ID,
                                FirstName = u.FirstName,
                                LastName = u.LastName,
                                Title = b.Title,
                                IssueDate = l.IssueDate,
                                IssuePeriod = bookLoanPeriod.ToString(),
                                OverdueBy = l.OverdueBy.ToString()
                            }).ToList();

                listData = books;
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
            var selectedItem = (ComboBoxItem)comboBox.SelectedValue;

            // Get the content of the selected item
            var selectedContent = selectedItem.Content;

            // Perform actions based on the selected item
            switch (selectedContent)
            {
                case "Most overdue":
                    // Sort from max to min
                    listData = listData.OrderByDescending(item => item.OverdueBy).ToList();
                    DisplayListData(listData);
                    break;
                case "Least overdue":
                    listData = listData.OrderBy(item => item.OverdueBy).ToList();
                    DisplayListData(listData);
                    break;
                default:
                    // Handle other selections or the default case
                    break;
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