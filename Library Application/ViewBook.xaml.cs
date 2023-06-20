using System;
using System.IO;
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
using System.Diagnostics;

using Library_Application;
using static Library_Application.BrowseBookPage;

namespace Library_Application
{
    /// <summary>
    /// Interaction logic for ViewBook.xaml
    /// </summary>
    public partial class ViewBook : Page
    {
        private static List<BookItem> bookItems;  // Stores a list of book items

        //string bookID;

        public ViewBook(BookItem selectedBook)
        {
            InitializeComponent();

            // Adding data contexts to loaded page
            this.DataContext = this;  // Set the data context of the loaded page to itself

            BookItem = selectedBook;

            PrefaceContent();

        }

        public BookItem BookItem { get; set; }  // Represents the selected book item

        private void PrefaceContent()  // Prepend additional content to the book item
        {
            BookItem.author = "Author : " + BookItem.author;
            BookItem.genre = "Genre : " + BookItem.genre;

            txtRating.Text = BookItem.rating;
        }

        private void ReturnPageButton_Click(object sender, RoutedEventArgs e)
        {
            //MainFrame.Content = new HomeDash();
        }

        private void btnLoanBook_Click(object sender, RoutedEventArgs e)
        {
            // insert an entry into the Loans table
            using (var db = new DataContext())
            {
                Loan liItem = new Loan();
                liItem.BookID = Convert.ToInt32(BookItem.bookID);

                // temporarily hard coded. It will be necessary to access the global variable that will be added later.
                liItem.UserID = 4;
                liItem.DueDate = DateTime.Now.AddDays(30);
                liItem.FineDue = false;
                liItem.IssueDate = DateTime.Now;
                liItem.ID = GetNewPKFromLoanTable(); // need to get PK ID from table.

                db.Add(liItem);
                db.SaveChanges();
            }
        }

        private void btnAddToWishList_Click(object sender, RoutedEventArgs e)
        {
            // insert an entry into the Wishlist table
            using (var db = new DataContext())
            {
                Wishlist wlItem = new Wishlist();
                wlItem.AddedDate = DateTime.Now;
                wlItem.BookID = Convert.ToInt32(BookItem.bookID);
                wlItem.ID = GetNewPKFromWishlistTable(); // need to get the latest PK ID.

                // temporarily hard coded. It will be necessary to access the global variable that will be added later.
                wlItem.UserID = 4;

                db.Add(wlItem);
                db.SaveChanges();

            }
        }

        // Utility functions specific to this page.
        // These get the last issued Primary Key number found in the table concerned, then increase by one.
        // This is necessary for the calling routine.
        private int GetNewPKFromLoanTable()
        {
            int newpkID = 0;

            using (var db = new DataContext())
            {
                var loanTable = db.Loans.ToList();
                for (int i = 0; i < loanTable.Count(); i++)
                {
                    newpkID = loanTable[i].ID;
                }
            }
            newpkID = newpkID + 1;
            return newpkID;
        }

        private int GetNewPKFromWishlistTable()
        {
            int newpkID = 0;

            using (var db = new DataContext())
            {
                var wishTable = db.Wishlist.ToList();
                for (int i = 0; i < wishTable.Count(); i++)
                {
                    newpkID = wishTable[i].ID;
                }
            }
            newpkID = newpkID + 1;
            return newpkID;
        }

        private int GetNewPKFromBookReviewTable()
        {
            int newpkID = 0;

            using (var db = new DataContext())
            {
                var bkrvTable = db.BookReview.ToList();
                for (int i = 0; i < bkrvTable.Count(); i++)
                {
                    newpkID = bkrvTable[i].ID;
                }
            }
            newpkID = newpkID + 1;
            return newpkID;
        }

        private void cmbRating_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Insert an entry into the BookReview table.
            // The rating as found in the BookItem object is an AVG of the values in this table.
            // The functionality to make this happen is not in place.

            if (BookItem != null && cmbRating.SelectedIndex > 0)
            {
                using (var db = new DataContext())
                {
                    string selection = ((ComboBoxItem)cmbRating.SelectedItem).Content.ToString();

                    BookReview bkRv = new BookReview();
                    bkRv.ID = GetNewPKFromBookReviewTable();
                    bkRv.BookID = Convert.ToInt32(BookItem.bookID);
                    bkRv.Rating = Convert.ToInt32(selection);

                    db.Add(bkRv);
                    db.SaveChanges();
                }
            }
        }
    }
}
