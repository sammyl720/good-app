using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace GoodApp.Backend.Models
{
    public class PagerModel
    {
        /// <summary>
        /// Gets or sets the item count.
        /// </summary>
        public int ItemCount { get; set; }

        public int StartItem {
            get { return (PageIndex - 1)*PageSize + ItemCount>0?1:0; }
        }

        public int EndItem
        {
            get { return PageIndex*PageSize > ItemCount ? ItemCount : PageIndex*PageSize; }
        }

        /// <summary>
        /// Gets or sets the link format.
        /// </summary>
        public string LinkFormat { get; set; }

        /// <summary>
        /// Gets the page count.
        /// </summary>
        public int PageCount
        {
            get
            {
                return Convert.ToInt32(Math.Ceiling(this.ItemCount * 1.0 / this.PageSize));
            }
        }

        /// <summary>
        /// Gets or sets the page index.
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// Gets or sets the page size.
        /// </summary>
        public int PageSize { get; set; }
    }
}
