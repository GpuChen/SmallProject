using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KMAPI.Models
{
    public class Doc
    {
        public string title { set; get; }
        public string path { set; get; }
        public string category { set; get; }
        public string subCategory { set; get; }
        public string updateUser { set; get; }
        public DateTime updateTime { set; get; }
        public string creator { set; get; }
        public DateTime createdTime { set; get; }

    }
}