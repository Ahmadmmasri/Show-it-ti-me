//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Ecommerce.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;



    public partial class tbl_product
    {
        public int prod_id { get; set; }

        [Required(ErrorMessage = "*")]
        public string prod_name { get; set; }
        public string prod_img { get; set; }

        [Required(ErrorMessage = "*")]
        public string prod_description { get; set; }

        [Required(ErrorMessage = "*")]
        [DataType(DataType.Currency)]

        public Nullable<double> prod_price { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name ="Category")]
        public Nullable<int> prod_fk_cate { get; set; }
        public Nullable<int> prod_fk_user { get; set; }


        public virtual tbl_category tbl_category { get; set; }
        public virtual tbl_user tbl_user { get; set; }
    }
}