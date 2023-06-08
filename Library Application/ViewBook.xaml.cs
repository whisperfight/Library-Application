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

        private static List<BookItem> bookItems;

        //string bookID;

        public ViewBook(BookItem selectedBook)
        {
            InitializeComponent();

            // Adding data contexts to loaded page
            this.DataContext = this;

            BookItem = selectedBook;

            PrefaceContent();


        }

        public BookItem BookItem { get; set; }

        private void PrefaceContent()
        {
            BookItem.author = "Author : " + BookItem.author;
            BookItem.genre = "Genre : " + BookItem.genre;
        }



        private void ReturnPageButton_Click(object sender, RoutedEventArgs e)
        {
            //MainFrame.Content = new HomeDash();
        }


    }
}
