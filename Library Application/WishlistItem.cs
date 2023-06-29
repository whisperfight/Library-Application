using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Library_Application
{
    public class WishlistItem
    {
        public int bookID { get; set; }
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
    }
}
