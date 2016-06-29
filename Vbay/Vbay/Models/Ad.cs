using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        [UIHint("tinymce_full_compressed")]
        public string Description { get; set; }
        public decimal Price { get; set; }
        public bool ? Approved { get; set; }
        public bool Active { get; set; }
        public DateTime DatePosted { get; set; }
        public string AdminComments { get; set; }
        public string AdminFilter { get; set; }

        private int shortDescriptionLength = 35;
        private int shortHeadlineLength = 15;

        public string ShortHeadline
        {
            get
            {
                if (Headline.Length > shortHeadlineLength)
                    return Headline.Substring(0, shortHeadlineLength) + "...";
                else
                    return Headline;
            }
        }
        public string ShortDescription
        {
            get
            {
                if (Description.Length > shortDescriptionLength)
                    return Description.Substring(0, shortDescriptionLength) + "...";
                else
                    return Description;
            }
        }
        
        public string UserId { get; set; }
    }
}