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

        List<Loan> loanedItems;

        public OverdueBooks()
        {
            InitializeComponent();

            LoadLoanListings();


        }

        public void LoadLoanListings()
        {
            ListView loanList = this.loanList;

            loanedItems = new List<Loan>();

            using (var db = new DataContext())
            {
                var BooksOverdue = (from b in db.Books
                                    join l in db.Loans on b.ID equals l.BookID
                                    join u in db.Users on l.UserID equals u.ID
                                    where DateTime.Now.AddDays(15) > l.DueDate
                                    //where DateTime.Now > l.DueDate
                                    select
                                    new DummyClass
                                    {
                                        ID = b.ID,
                                        FirstName = u.FirstName,
                                        LastName = u.LastName,
                                        Title = b.Title,
                                        IssueDate = l.IssueDate,
                                        IssuePeriod = "30 Days",
                                        OverdueBy = "101 Days"
                                    }).ToList();

        
                loanList.ItemsSource = BooksOverdue;
            }

        }


        private void RemoveEntry_Click(object sender, RoutedEventArgs e)
        {
            DummyClass selectedItem = (DummyClass)loanList.SelectedItem;

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

                LoadLoanListings();

            }
        }

    }

}

public class DummyClass
{
    public int ID { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Title { get; set; }
    public DateTime IssueDate { get; set; }
    public string IssuePeriod { get; set; }
    public string OverdueBy { get; set; }
}