using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library_Application
{
    // Represents a loan item in the library application
    public class LoanItem
    {
        string _imgurl = "";  // The URL of the loan item's image
        string _title = "";  // The title of the loan item
        string _genre = "";  // The genre of the loan item
        string _loanState = "";  // The loan state of the loan item

        // The URL of the loan item's image
        public string imgURL
        {
            get { return _imgurl; }
            set { _imgurl = value; }
        }

        // The title of the loan item
        public string title
        {
            get { return _title; }
            set { _title = value; }
        }

        // The genre of the loan item
        public string genre
        {
            get { return _genre; }
            set { _genre = value; }
        }

        // The loan state of the loan item
        public string loanstate
        {
            get
            {
                return _loanState ;
            }
            set
            {
                _loanState = value;
            }
        }
    }
}
