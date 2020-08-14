using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace subzicart.Models
{
    public class category_Subcategory_join_class
    {
        public int subCatId { get; set; }
        public int catId { get; set; }
        public string subCatName { get; set; }
        public string description { get; set; }
        public Nullable<int> isActive { get; set; }
        public Nullable<int> userId { get; set; }
        public Nullable<System.DateTime> insDt { get; set; }

        public Nullable<System.DateTime> updDt { get; set; }

        public string catName { get; set; }
    }
}