using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace subzicart.Models
{
    public class MyViewModel
    {
        public List<CategoryClass> category { get; set; }
    }
    public class CategoryClass
    {
        public int cat_id { get; set; }  
        public string cat_name { get; set; }
        public int p_cat_id { get; set; }      
        public string root_cat_name { get; set; }

    }
}