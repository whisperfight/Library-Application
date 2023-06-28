using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Library_Application
{
    // Represents a loan item in the library application
    public class LoanItem
    {

        public string bookID { get; set; }
        public string title { get; set; }
        public string author { get; set; }
        public string summary { get; set; }
        public string timeToRead { get; set; }
        public string rating { get; set; }
        public string genre { get; set; }
        public string imgURL { get; set; }
        public string loanState { get; set; }
        public string newRelease { get; set; }
        public string dueDate { get; set; }

        public Brush LoanStatusFill
        {
            get
            {
                if (loanState == "On Loan")
                {
                    return new SolidColorBrush(Color.FromRgb(114, 114, 114));
                }
                else
                {
                    return new SolidColorBrush(Color.FromRgb(58, 177, 155));
                }
            }
        }



        //string _imgurl = "";  // The URL of the loan item's image
        //string _title = "";  // The title of the loan item
        //string _genre = "";  // The genre of the loan item
        //string _loanState = "";  // The loan state of the loan item

        //// The URL of the loan item's image
        //public string imgURL
        //{
        //    get { return _imgurl; }
        //    set { _imgurl = value; }
        //}

        //// The title of the loan item
        //public string title
        //{
        //    get { return _title; }
        //    set { _title = value; }
        //}

        //// The genre of the loan item
        //public string genre
        //{
        //    get { return _genre; }
        //    set { _genre = value; }
        //}

        //public Brush LoanStatusFill
        //{
        //    get
        //    {
        //        if (loanstate == "On Loan")
        //        {
        //            return new SolidColorBrush(Color.FromRgb(114, 114, 114));
        //        }
        //        else
        //        {
        //            return new SolidColorBrush(Color.FromRgb(58, 177, 155));
        //        }
        //    }
        //}

        //// The loan state of the loan item
        //public string loanstate
        //{
        //    get
        //    {
        //        return _loanState ;
        //    }
        //    set
        //    {
        //        _loanState = value;
        //    }
        //}
    }
}
