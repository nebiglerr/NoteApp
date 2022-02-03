    using MyEvernote.BusinessLayer;
using MyEvernote.BusinessLayer.Result;
using MyEvernote.Entities;
using MyEvernote.Entities.Messages;
using MyEvernote.Entities.ValueObjects;
using MyEvernote.WebApp.Filters;
using MyEvernote.WebApp.Models;
using MyEvernote.WebApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MyEvernote.WebApp.Controllers
{
    [Exc]
    public class HomeController : Controller
    {
        private NoteManager noteManager = new NoteManager();
        private CategoryManager categoryManager = new CategoryManager();

        private EvernoteUserManager evernoteUserManager = new EvernoteUserManager();
        // GET: Home
        public ActionResult Index()
        {

            //object o = 0;
            //int a = 1;
            //int c = a / (int)o;
            //throw new Exception("Her hangi bir hata oluştu");
            // Category üzerinden gelen view talebi ve model 
            //
            //if (TempData["CategoryListed"] != null)
            //{
            //    return View(TempData["CategoryListed"] as List<Note>);
            //}

            //Test test = new Test();
            return View(noteManager.ListQueryable().Where(x => x.IsDraft == false).OrderByDescending(x => x.ModifiedOn).ToList());
            //return View(nm.GetAllNotesQueryable().OrderByDescending(x => x.ModifiedOn).ToList());
            // IQueryable Oluşturduğumuz  repositorlerdeki tip dönüşümünde  sql sorgularının c# tarafında değil sql tarafında çalışmasına olanak sağlar
            //GetAllNotesQueryable().OrderByDescending(x => x.ModifiedOn).ToList()  OrderByDescending içine alaraka  sql sorgulatır ve yük sql kalır 

        }

        public ActionResult ByCategory(int? id)


        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
           
            Category cat = categoryManager.Find(x => x.Id == id.Value); // nullable değişkenlerde tip uyuşmazlığı olabilir value yazarak değerei getir diye bilriz 

            if (cat == null)
            {
                return HttpNotFound();
                //return RedirectToAction("Indexx", "Home");
            }

           
            return View("Index",cat.Notes.Where(x => x.IsDraft == false).OrderByDescending(x => x.ModifiedOn).ToList());
        }

        public ActionResult MostLiked()
        {
           

            return View("Index",noteManager.ListQueryable().OrderByDescending(x => x.LikeCount).ToList());
        }

        public ActionResult About()
        {
            return View();
        }

        [Auth]
        public ActionResult ShowProfile()
        {

                     
            BusiessLayerResult<EvernoteUser> res = evernoteUserManager.GetUserById(CurrentSession.User.Id);

            if (res.Errors.Count > 0)
            {
                //TODO : KULLANICIYA BİR HATA EKRANI YÖNLENDİRİLMESİ LAZIM
                ErrorViewModel errorNotifyObj = new ErrorViewModel()
                {
                    Title = "Hata oluştu",
                    Item = res.Errors
                };
                return View("Error", errorNotifyObj);
            }

            return View(res.Result);
        }
        [Auth]
        public ActionResult EditProfile()
        {

                  
            BusiessLayerResult<EvernoteUser> res = evernoteUserManager.GetUserById(CurrentSession.User.Id);

            if (res.Errors.Count > 0)
            {
                //TODO : KULLANICIYA BİR HATA EKRANI YÖNLENDİRİLMESİ LAZIM
                ErrorViewModel errorNotifyObj = new ErrorViewModel()
                {
                    Title = "Hata oluştu",
                    Item = res.Errors
                };
                return View("Error", errorNotifyObj);
            }

            return View(res.Result);
        }
        [Auth]
        [HttpPost]
        public ActionResult EditProfile(EvernoteUser model, HttpPostedFileBase ProfileImage)
        {
            ModelState.Remove("ModifiedUsername");

            if (ModelState.IsValid)
            {
                if (ProfileImage != null &&
                (ProfileImage.ContentType == "image/jpeg" ||
                ProfileImage.ContentType == "image/jpg" ||
                ProfileImage.ContentType == "image/png"))
                {
                    string filename = $"user_{model.Id}.{ProfileImage.ContentType.Split('/')[1]}";

                    ProfileImage.SaveAs(Server.MapPath($"~/images/{filename}"));
                    model.ProfileImageFilename = filename;
                }

                BusiessLayerResult<EvernoteUser> res = evernoteUserManager.UpdateProfile(model);

                if (res.Errors.Count > 0)
                {

                    ErrorViewModel errorNotifyObj = new ErrorViewModel()
                    {
                        Title = "Profil Güncellenemedi",
                        Item = res.Errors,
                        RedirectingUrl = "/Home/EditProfile"

                    };
                    return View("Error", errorNotifyObj);
                }

                // Profil güncellendiği için session güncellendi.
                CurrentSession.Set<EvernoteUser>("login", res.Result);

                return RedirectToAction("ShowProfile");

            }
            return View(model);
        }

        [Auth]
        public ActionResult DeleteProfile()
        {
              
            BusiessLayerResult<EvernoteUser> res = evernoteUserManager.RemoveUserById(CurrentSession.User.Id);

            if (res.Errors.Count > 0)
            {
                ErrorViewModel errorNotifyObj = new ErrorViewModel()
                {
                    Item = res.Errors,
                    Title = "Profil Silinemedi.",
                    RedirectingUrl = "/Home/ShowProfile"
                };

                return View("Error", errorNotifyObj);
            }

            Session.Clear();

            return RedirectToAction("Index");
        }


        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
           
            if (ModelState.IsValid)// model tanımlımı değilmi 
            {
                
                BusiessLayerResult<EvernoteUser> res = evernoteUserManager.LoginUser(model);

                if (res.Errors.Count > 0)
                {
                    //hata koduna göre işlem yapılmak istenirse  aşşagıdaki sorgu işlemi ile yapıla bilir 
                    // login kısmındaki yorum satırını kaldırınız 
                    //if (res.Errors.Find(x => x.Code == ErrorMessageCode.UserIsNotActive) != null)
                    //{
                    //    ViewBag.SetLink = "http://Home/Activate/1234-4567-67890";
                    //}
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    return View(model);

                }

                CurrentSession.Set<EvernoteUser>("login", res.Result);  //Sessionda veri saklama
                return RedirectToAction("Index");  // yönlendirme


            }   

            return View(model);
        }
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(RegisterViewModel model)
        {

            //kullanıcı user kontrol
            //kullanıcı email kontrol
            // kayıt işlemi 
            // aktivasyon posta gönderimi 


            if (ModelState.IsValid)// model tanımlımı değilmi 
            {

               
                BusiessLayerResult<EvernoteUser> res = evernoteUserManager.RegisterUser(model);

                if (res.Errors.Count>0)
                {              
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    return View(model);
                        
                }


                #region açıklama satırı
                //EvernoteUser user = null;

                //try
                //{
                //    user = eum.RegisterUser(model);
                //}
                //catch (Exception ex)
                //{

                //    ModelState.AddModelError("", ex.Message);
                //}



                // ayrı 

                //if (model.Username == "aaa") // örnek bir veri kontrolü sağlanarak eğer mevcut ise bunları yap 
                //{
                //    ModelState.AddModelError("", "Kullanıcı adı kullanılıyor");

                //}
                //if (model.Email == "aaa@aa.com")
                //{
                //    ModelState.AddModelError("", "E-Mail  kullanılıyor");

                //}
                //foreach (var item in ModelState)
                //{
                //    if (item.Value.Errors.Count > 0)
                //    {
                //        return View(model);
                //    }
                //}

                //if (user == null)
                //{
                //    return View(model);
                //}





                #endregion

                OkViewModel notifyObj = new OkViewModel()
                {
                    Title = "Kayıt Oldunuz",
                    RedirectingUrl = "/Home/Login"
                };

                notifyObj.Item.Add("Lütfen e-posta adresinize gönderdiğimiz  aktivasyon linkine tıklayarak hesabınızı aktive ediniz." +
                    " Hesabı aktive etmeden not ekleyemez ve beğeni yapamazsınız.");

                return View("Ok", notifyObj);
            }


            return View(model);
        }
      
        public ActionResult UserActivate(Guid id)
        {
            // Kulllanıcı aktivasyonu sağlanacak     
           BusiessLayerResult <EvernoteUser> res = evernoteUserManager.AktivateUser(id);
            if (res.Errors.Count > 0)
            {
                ErrorViewModel errorNotifyObj = new ErrorViewModel()
                {
                    Title = "Geçersiz İşlem",
                    Item = res.Errors
                };
                
                return View("Error", errorNotifyObj);
            }

            OkViewModel okNotifyObj = new OkViewModel()
            {
                Title = "Hesap Aktifleştirildi",
                RedirectingUrl = "/Home/Login"
            };
            okNotifyObj.Item.Add(" Hesap aktifleştirildi artık not paylaşabilir ve  beğendiğiniz notları kaydedebilirsiniz .");
            return View("Ok",okNotifyObj);
        }
        
        public ActionResult Logout()
        {
            Session.Clear();

            return RedirectToAction("Index");
        }

        public ActionResult AccessDenied()
        {
            return View();
        }

        public ActionResult HasError()
        {
            return View();
        }
    }
}