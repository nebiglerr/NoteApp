using MyEvernote.BusinessLayer.Abstract;
using MyEvernote.BusinessLayer.Result;
using MyEvernote.Common.Helpers;
using MyEvernote.Entities;
using MyEvernote.Entities.Messages;
using MyEvernote.Entities.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.BusinessLayer
{
    public class EvernoteUserManager:ManagerBase<EvernoteUser>
    {
 

        public BusiessLayerResult<EvernoteUser> RegisterUser(RegisterViewModel data)
        {
            //kullanıcı user kontrol
            //kullanıcı email kontrol
            // kayıt işlemi 
            // aktivasyon posta gönderimi 

            EvernoteUser user = Find(x => x.Username == data.Username || x.Email == data.Email);
            BusiessLayerResult<EvernoteUser> res = new BusiessLayerResult<EvernoteUser>();
            if (user != null)
            {
                if (user.Username == data.Username)
                {
                    res.AddError(ErrorMessageCode.UsernameAlreadyExists , "Kullanıcı adı kayıtlı");
                }
                if (user.Email == data.Email)
                {
                    res.AddError(ErrorMessageCode.EmailAlreadyExists, "E-posta adresi kayıtlı !");
                }
            }
            else
            {
                int dbResult = base.Insert(new EvernoteUser() //base. diyerek miras alınan klası seçiyor ve ezdiğimiz klası çağırmıyor 
                {

                    Username = data.Username,
                    Email = data.Email,
                    Password = data.Password,
                    ProfileImageFilename = "user.png",
                    ActivateGuid = Guid.NewGuid(),               
                    IsActive = false,
                    IsAdmin = false
                   
                });
                if (dbResult > 0)
                {
                    res.Result = Find(x => x.Email == data.Email || x.Username == data.Username);

                    //TODO : AKTİVASYON MAİLİ ATILACAK 
                    //layerResult.Result.ActivateGuid
                    string siteUri = ConfigHelpers.Get<string>("SiteRootUri");
                    string activeUri = $"{siteUri}/Home/UserActivate/{res.Result.ActivateGuid}";
                    string body = $"Merhaba {res.Result.Username} ; Hesabınızı aktifleştirmk için <a href='{activeUri}' target='_blank'>tıklanıyınız</a>";
                    MailHelper.SendMail(body, res.Result.Email,"MyEvernote Hesap Aktivasyonu");


                }
            }

            return res;
        }

        public BusiessLayerResult<EvernoteUser> GetUserById(int ıd)
        {
            BusiessLayerResult<EvernoteUser> res = new BusiessLayerResult<EvernoteUser>();

            res.Result = Find(x => x.Id == ıd);

            if (res.Result == null)
            {
                res.AddError(ErrorMessageCode.UserNotFound, "Kullanıcı Bulunamadı");
            }
            return res;
        }

        public BusiessLayerResult<EvernoteUser> LoginUser(LoginViewModel data)
        {

            //Giriş kontrolü ve yönlendirme 
            // HESAP AKTİVE EDİLMİŞ Mİ       
            
            BusiessLayerResult<EvernoteUser> res = new BusiessLayerResult<EvernoteUser>();

            res.Result = Find(x => x.Username == data.Username && x.Password == data.Password);
           
            if (res.Result !=  null)
            {
                if (!res.Result.IsActive)
                {
                    res.AddError(ErrorMessageCode.UserIsNotActive, "Kullanıcı hesabı aktif edilmemiştir.");
                    res.AddError(ErrorMessageCode.CheckYourEmail, "Lütfen e-posta adresinizi konrol ediniz.");
                }
                
            }
            else
            {
                res.AddError(ErrorMessageCode.UsernameOrPasswrong, "Kullanıcı adı ve ya şifre yanlış");
            }

            return res;
        }

        public BusiessLayerResult<EvernoteUser> AktivateUser (Guid aktivateId)
        {
         
            BusiessLayerResult<EvernoteUser> res = new BusiessLayerResult<EvernoteUser>();
            res.Result = Find(x => x.ActivateGuid == aktivateId);
            if (res.Result.ActivateGuid != null)
            {
                if (res.Result.IsActive)
                {
                    res.AddError(ErrorMessageCode.UserAlreadyActive, "Kullanıcı zaten aktif !");
                    return res;
                }
                res.Result.IsActive = true;
                Update(res.Result);
            }
            else
            {
                res.AddError(ErrorMessageCode.ActivateIdDoesNotExists, "Aktifleştirilecek kullanıcı bulunamadı ! ");
            }
            return res;
        }

        public BusiessLayerResult<EvernoteUser> UpdateProfile(EvernoteUser data)
        {
            EvernoteUser db_user = Find(x => x.Id != data.Id && (x.Username == data.Username || x.Email == data.Email));
            BusiessLayerResult<EvernoteUser> res = new BusiessLayerResult<EvernoteUser>();

            if (db_user != null && db_user.Id != data.Id)
            {
                if (db_user.Username == data.Username)
                {
                    res.AddError(ErrorMessageCode.UsernameAlreadyExists, "Kullanıcı adı kayıtlı.");
                }

                if (db_user.Email == data.Email)
                {
                    res.AddError(ErrorMessageCode.EmailAlreadyExists, "E-posta adresi kayıtlı.");
                }

                return res;
            }

            res.Result = Find(x => x.Id == data.Id);
            res.Result.Email = data.Email;
            res.Result.Name = data.Name;
            res.Result.Surname = data.Surname;
            res.Result.Password = data.Password;
            res.Result.Username = data.Username;

            if (string.IsNullOrEmpty(data.ProfileImageFilename) == false)
            {
                res.Result.ProfileImageFilename = data.ProfileImageFilename;
            }

            if (base.Update(res.Result) == 0)
            {
                res.AddError(ErrorMessageCode.ProfileCouldNotUpdated, "Profil güncellenemedi.");
            }

            return res;
        }

        public BusiessLayerResult<EvernoteUser> RemoveUserById(int id)
        {
            BusiessLayerResult<EvernoteUser> res = new BusiessLayerResult<EvernoteUser>();
            EvernoteUser user = Find(x => x.Id == id);
            if (user != null)
            {
                if (Delete(user) == 0)
                {
                    res.AddError(ErrorMessageCode.UserCouldNotRemove, "Kullanıcı silinemedi.");
                    return res;
                }
            }
            else
            {
                res.AddError(ErrorMessageCode.UserCouldNotFind, "Kullanıcı bulunamadı.");
            }
            return res;
        }




        public new  BusiessLayerResult<EvernoteUser> Insert(EvernoteUser data)
        {
            //Method hiding... miras alınan clastan gelen özelliği geri dönüş  tipi (olan BusiessLayerResult<EvernoteUser>) 
            //olarak değitirmek için new kullanılır ve miras alınanı değil şuan kullanılanı seçmesini sağlar


            EvernoteUser user = Find(x => x.Username == data.Username || x.Email == data.Email);
            BusiessLayerResult<EvernoteUser> res = new BusiessLayerResult<EvernoteUser>();

            res.Result = data;
            if (user != null)
            {
                if (user.Username == data.Username)
                {
                    res.AddError(ErrorMessageCode.UsernameAlreadyExists, "Kullanıcı adı kayıtlı");
                }
                if (user.Email == data.Email)
                {
                    res.AddError(ErrorMessageCode.EmailAlreadyExists, "E-posta adresi kayıtlı !");
                }
            }
            else
            {
                res.Result.ProfileImageFilename = "user.png";
                res.Result.ActivateGuid = Guid.NewGuid();

                if (base.Insert(res.Result) == 0)
                {
                    res.AddError(ErrorMessageCode.UserCouldNotInserted, "Kullanıcı eklenemedi");
                }

      
            }

            return res;


        }

        public new BusiessLayerResult<EvernoteUser> Update(EvernoteUser data)
        {


            EvernoteUser db_user = Find(x => x.Id != data.Id && (x.Username == data.Username || x.Email == data.Email));
            BusiessLayerResult<EvernoteUser> res = new BusiessLayerResult<EvernoteUser>();
            res.Result = data;

            if (db_user != null && db_user.Id != data.Id)
            {
                if (db_user.Username == data.Username)
                {
                    res.AddError(ErrorMessageCode.UsernameAlreadyExists, "Kullanıcı adı kayıtlı.");
                }

                if (db_user.Email == data.Email)
                {
                    res.AddError(ErrorMessageCode.EmailAlreadyExists, "E-posta adresi kayıtlı.");
                }

                return res;
            }

            res.Result = Find(x => x.Id == data.Id);
            res.Result.Email = data.Email;
            res.Result.Name = data.Name;
            res.Result.Surname = data.Surname;
            res.Result.Password = data.Password;
            res.Result.Username = data.Username;
            res.Result.IsActive = data.IsActive;
            res.Result.IsAdmin = data.IsAdmin;


            if (base.Update(res.Result) == 0)
            {
                res.AddError(ErrorMessageCode.ProfileCouldNotUpdated, "Kullanıcı güncellenemedi.");
            }

            return res;
        }
    } 
}
