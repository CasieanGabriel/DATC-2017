using System.Collections.Generic;

namespace AlbumPhoto.Models
{
    public class Poza
    {
        public string Description { get; set; }
        public string Url { get; set; }
        public string ThumbnailUrl { get; set; }
        public List<Comentariu> Comments { get; set; }
    }
}