using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace subzicart.Models
{
    public class imageModel
    {
    
            public int Id { get; set; }

            public string Name { get; set; }

            public HttpPostedFileBase Filepic { get; set; }

            public DateTime UploadDate { get; set; }

        public int image_id { get; set; }
        public Nullable<int> product_id { get; set; }
        public string image_name { get; set; }
        public string image_desc { get; set; }
        public string image_url { get; set; }
        public byte[] image_binary { get; set; }
        public string isthumb { get; set; }
        public Nullable<int> sort_id { get; set; }
        public Nullable<System.DateTime> insdt { get; set; }
        public Nullable<int> active { get; set; }
        public Nullable<System.DateTime> updDt { get; set; }
        public Nullable<System.DateTime> userId { get; set; }

    }
}