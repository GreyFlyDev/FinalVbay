using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vbay.Models
{
    public class Ad
    {
        public int Id { get; set; }
        public string Headline { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public bool Approved { get; set; }
        public DateTime DatePosted { get; set; }
        
        public string UserId { get; set; }
    }
}