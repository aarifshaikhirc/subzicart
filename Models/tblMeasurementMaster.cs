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
    public partial class tblMeasurementMaster
    {
        public int m_id { get; set; }
        [Required(ErrorMessage = "Unit Type is Required")]
        [RegularExpression(@"^[a-zA-Z]+[ a-zA-Z-_]*$", ErrorMessage = "Use Characters only")]

        public string unit_type { get; set; }
        [Required(ErrorMessage = "Unit Value is Required")]
        [DataType(DataType.Currency)]
        public Nullable<double> unit_value { get; set; }
        [Required(ErrorMessage = "Equivalent Unit Type is Required")]
        [RegularExpression(@"^[a-zA-Z]+[ a-zA-Z-_]*$", ErrorMessage = "Use Characters only")]

        public string eq_unit_type { get; set; }
        [Required(ErrorMessage = "Equivalent Unit Value is Required")]
        [DataType(DataType.Currency)]
        public Nullable<double> eq_unit_value { get; set; }
        public Nullable<System.DateTime> insDt { get; set; }
        public Nullable<int> active { get; set; }
        public Nullable<System.DateTime> updDt { get; set; }
        public Nullable<int> userId { get; set; }
    }
}
