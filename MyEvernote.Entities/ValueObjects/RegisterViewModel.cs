using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyEvernote.Entities.ValueObjects
{
    public class RegisterViewModel
    {
        [DisplayName("Kullanıcı Adı"), 
         Required(ErrorMessage = "{0} alanı boş geçilenemez"),
        StringLength(25, ErrorMessage = "{0} alanı max {1} olmalıdır.")]
        public string Username { get; set; }

        [DisplayName("E-posta"),
         Required(ErrorMessage = "{0} alanı boş geçilenemez"),
         StringLength(70 , ErrorMessage = "{0} alanı max {1} olmalıdır."), 
         EmailAddress(ErrorMessage ="{0} alanı için geçerli bir e-posta giriniz")]
        public string Email { get; set; }

        [DisplayName("Şifre"), 
         Required(ErrorMessage = "{0} alanı boş geçilenemez"), 
         DataType(DataType.Password), 
         StringLength(25, ErrorMessage = "{0} alanı max {1} olmalıdır.")]
        public string Password { get; set; }

        [DisplayName("Şifre Tekrar"), 
         Required(ErrorMessage = "{0} alanı boş geçilenemez"), 
         DataType(DataType.Password), 
         StringLength(25, ErrorMessage = "{0} alanı max {1} olmalıdır."),
            Compare("Password",ErrorMessage = "{0} ile {1} uyuşmuyor")]
        public string RePassword { get; set; }
    }
}