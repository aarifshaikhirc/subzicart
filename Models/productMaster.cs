using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace subzicart.Models
{
    public class productMaster
    {
        public int productId { get; set; }
        [Display(Name = "Product Name")]
        public string productName { get; set; }
        public string SKU { get; set; }
        [Display(Name = "About Product")]
        public string aboutProduct { get; set; }
        [Display(Name = "Description")]
        public string description { get; set; }
        [Display(Name = "Price")]
        public int price { get; set; }
        [Display(Name = "Cost Price")]
        public int costPrice { get; set; }
        [Display(Name = "Retail Price")]
        public int retailPrice { get; set; }
        [Display(Name = "Sale Price")]
        public int salePrice { get; set; }
        [Display(Name = "Fixed Shipping Cost")]
        public string fixedShippingCost { get; set; }
        [Display(Name = "Free Shipping")]
        public string freeShipping { get; set; }
        [Display(Name = "Product Weight")]
        public string pWeight { get; set; }
        [Display(Name = "Product Width")]
        public string pWidth { get; set; }
        [Display(Name = "Product Height")]
        public string pHeight { get; set; }
        [Display(Name = "Product Depth")]
        public string pDepth { get; set; }
        [Display(Name = "Search Keywords")]
        public string searchKeywords { get; set; }
        [Display(Name = "Page Title")]
        public string pageTitle { get; set; }
        [Display(Name = "Meta Keyword")]
        public string metaKeyword { get; set; }
        [Display(Name = "Meta Description")]
        public string metaDescription { get; set; }
        public int sortOrder { get; set; }
        [Display(Name = "Category")]
        public string categoryName { get; set; }
        public string catId { get; set; }
        public string subCatId { get; set; }
        public string brandId { get; set; }

        [Display(Name = "Sub Category")]
        public string subCategoryName { get; set; }
        [Display(Name = "Brand")]
        public string brandName { get; set; }
        public string subCatName { get; set; }
    }
}