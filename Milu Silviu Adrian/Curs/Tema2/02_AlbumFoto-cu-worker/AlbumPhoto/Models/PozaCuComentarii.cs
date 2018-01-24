using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AlbumPhoto.Service.Entities;

namespace AlbumPhoto.Models
{
    public class PozaCuComentarii
    {
        public Poza Poza { get; set; }
        public List<CommentEntity> Comentarii { get; set; }
    }
}