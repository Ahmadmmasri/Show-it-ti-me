using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ecommerce.Models
{
    public class AdModel
    {
        public int prod_id { get; set; }

        public string prod_name { get; set; }
        public string prod_img { get; set; }

        public string prod_description { get; set; }



        public Nullable<double> prod_price { get; set; }

        public Nullable<int> prod_fk_cate { get; set; }
        public Nullable<int> prod_fk_user { get; set; }

        public int cate_id { get; set; }
        public string cate_name { get; set; }

        public string u_name { get; set; }

        public string u_img { get; set; }

        public string u_contact { get; set; }

    }
}