using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library_Application
{
    public class WishlistItem
    {
        string _imgurl = "";
        string _title = "";
        string _genre = "";
        string _loanState = "";

        public string imgURL
        {
            get { return _imgurl; }
            set { _imgurl = value; }
        }

        public string title
        {
            get { return _title; }
            set { _title = value; }
        }

        public string genre
        {
            get { return _genre; }
            set { _genre = value; }
        }
        public string loanstate
        {
            get
            {
                return _loanState;
            }
            set
            {
                _loanState = value;
            }
        }
    }
}
