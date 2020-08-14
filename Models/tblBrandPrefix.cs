//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace subzicart.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;



    public partial class tblBrandPrefix
    {
        
        public long brand_id { get; set; }
        
        [Required(ErrorMessage = "Brand Name is Required")]
        
        public string brand_name { get; set; }
        [Required(ErrorMessage = "Brand Prefix is Required")]
       
        public string brand_prefix { get; set; }
        [Required(ErrorMessage = "Start Range is Required")]
        [Range(0, Int64.MaxValue, ErrorMessage = "Start Range should not contain characters")]
        public string sku_start_range { get; set; }
        [Required(ErrorMessage = "End Range is Required")]
        [Range(0, Int64.MaxValue, ErrorMessage = "End Range should not contain characters")]
        public string sku_end_range { get; set; }
        public Nullable<System.DateTime> insdt { get; set; }
        public Nullable<int> active { get; set; }
        public Nullable<System.DateTime> updDt { get; set; }
        public Nullable<int> userId { get; set; }
    }
}
