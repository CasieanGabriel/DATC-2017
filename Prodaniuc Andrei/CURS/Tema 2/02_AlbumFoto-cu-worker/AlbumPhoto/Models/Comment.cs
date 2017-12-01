using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AlbumPhoto.Models
{
    public class Comment
    {
        public string Text { get; set; }
        public string MadeBy { get; set; }
        public string File { get; set; }
        public DateTime Timestamp { get; set; }
        public string User { get; set; }
    }
}