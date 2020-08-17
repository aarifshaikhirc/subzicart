using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace subzicart.Models
{
    public class MyViewModel1
    {
        public List<CategoryGrid> category { get; set; }
    }
    public class CategoryGrid
    {
        public int cat_id { get; set; }
        public string cat_name { get; set; }
        public string sub_Cat1 { get; set; }
        public string sub_Cat2 { get; set; }
        public string sub_Cat3 { get; set; }
        public string description { get; set; }
        public string isactive { get; set; }
    }
}