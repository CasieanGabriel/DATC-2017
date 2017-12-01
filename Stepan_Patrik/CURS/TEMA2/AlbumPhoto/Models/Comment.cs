using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AlbumPhoto.Models
{
    public class Comment
    {
        public string Text { get; set; }
        public string Author { get; set; }
        public DateTime PublishDate { get; set; }
    }
}