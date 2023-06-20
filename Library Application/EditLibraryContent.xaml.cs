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
    /// Interaction logic for Page2.xaml
    /// </summary>
    public partial class EditLibraryContent : Page
    {

        public List<Book> listData = new List<Book>();

        public EditLibraryContent()
        {
            InitializeComponent();

            LoadDatabase();
            //DisplayListData(listData);

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

            }
        }


    }

}
