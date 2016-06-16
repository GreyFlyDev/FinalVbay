using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Vbay.Models
{
    public class Ad
    {
        public int Id { get; set; }
        public string Headline { get; set; }
        [AllowHtml]
        public string Description { get; set; }
        public decimal Price { get; set; }
        public bool Approved { get; set; }
        public DateTime DatePosted { get; set; }
        
        public string UserId { get; set; }
    }
}