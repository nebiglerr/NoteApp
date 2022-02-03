﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.Entities
{
    [Table("Notes")]
    public class Note : MyEntityBase   
    {

        [DisplayName("Not Başlığı"),Required, StringLength(60)]
        public string Title { get; set; }

        [DisplayName("Not Metni"), Required, StringLength(2000)]
        public string Text { get; set; }

        [DisplayName("Taslak")]
        public bool IsDraft { get; set; } // Taslak mı ? hemen yayınlamak istemez

        [DisplayName("Beğeni Sayısı")]
        public int LikeCount { get; set; } // beğendiği notlar 

        [DisplayName("Kategori")]
        public int CategoryId { get; set; } // bu özellik bize direk notun bağlı oldugu category ıd verecek altdaki listeden gidersek bir tane daha sorgu çalıştırır


        public virtual EvernoteUser Owner { get; set; } // bir notu bir tane kullanıcısı vardır 
        public virtual Category Category { get; set; }
        public virtual List<Comment> Comments { get; set; }
        public virtual List<Liked> Likes { get; set; }

        public Note()
        {
            Comments = new List<Comment>();
            Likes = new List<Liked>();
        }

    }
}
