using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AlbumPhoto.Models
{
    public class Poza
    {
        public string Description { get; set; }
        public string Url { get; set; }
        public string ThumbnailUrl { get; set; }

        public class Comentarii
        {
            public string Text { get; set; }
            public string MadeBy { get; set; }
        }
        public string Link { get; set; }
    }
}