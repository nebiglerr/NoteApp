using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyEvernote.Entities.ValueObjects
{
    public class LoginViewModel
    {

        [DisplayName("Kullanıcı Adı"), 
            Required(ErrorMessage = "{0} alanı boş geçilenemez"),
            StringLength(25, ErrorMessage = "{0} alanı max {1} olmalıdır.")]
        public string Username { get; set; }

        [DisplayName("Şifre"), 
            Required(ErrorMessage = "{0} alanı boş geçilenemez"),
            DataType(DataType.Password), 
            StringLength(25, ErrorMessage = "{0} alanı max {1} olmalıdır.")]
        public string Password { get; set; }
    }
}