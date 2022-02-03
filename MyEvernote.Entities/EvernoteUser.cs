using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.Entities
{
    [Table("EvernoteUsers")]
    public class EvernoteUser : MyEntityBase
    {
        [DisplayName("İsim"),
        StringLength(25,ErrorMessage = "{0} alanı max. {1} karakter olmalıdır")]
        public string Name { get; set; }

        [DisplayName("Soy İsim") ,
        StringLength(25, ErrorMessage = "{0} alanı max. {1} karakter olmalıdır")]
        public string Surname { get; set; }



        [DisplayName("Kullanıcı Adı"), 
        Required(ErrorMessage = "{0} alanı gereklidir" ), 
        StringLength(25, ErrorMessage = "{0} alanı max. {1} karakter olmalıdır")]
        public string Username { get; set; }

        [DisplayName("E-Posta"), 
         Required(ErrorMessage = "{0} alanı gereklidir"), 
         StringLength(70, ErrorMessage = "{0} alanı max. {1} karakter olmalıdır")]
        public string Email { get; set; }

        [DisplayName("Şifre"), 
         Required(ErrorMessage = "{0} alanı gereklidir"), 
         StringLength(100, ErrorMessage = "{0} alanı max. {1} karakter olmalıdır")]
        public string Password { get; set; }







        [StringLength(30),ScaffoldColumn(false)] // user12.jpg ScaffoldColumn özelliği otomatik olarak oluşturulan crud işleminte bu alan açılmasın özellğini tasır
        public string ProfileImageFilename { get; set; }

        [DisplayName("Is Active")]
        public bool IsActive { get; set; } // aktifmi 

        [DisplayName("Is IsAdmin")]
        public bool IsAdmin { get; set; } // adminmi

        [Required, ScaffoldColumn(false)]
        public Guid ActivateGuid { get; set; } // guidle aktif guidi olusturma 

       

        public virtual List<Note> Notes { get; set; } // bir userın birden çok notu vardır
        public virtual List<Comment> Comments { get; set; } // bir userın birden çok yorumu vardır
        public virtual List<Liked> Likes { get; set; }
    }
}
