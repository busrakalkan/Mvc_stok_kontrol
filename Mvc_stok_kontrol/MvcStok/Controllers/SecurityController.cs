using MvcStok.Models;
using MvcStok.Models.Entity;
using SRVTextToImage;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;

namespace MvcStok.Controllers
{
    public class SecurityController : Controller
    {
        MvcDbStokEntities db = new MvcDbStokEntities();
        // GET: Security
        public ActionResult Index()
        {
            ViewBag.Mesaj = null;
            return View();
        }

        [HttpPost]
        public ActionResult Login(tbl_admin admin, string GuvenlikKodu)
        {
            if (this.Session["GuvenlikKodu"].ToString() == GuvenlikKodu)
            {
                var kontrol = db.tbl_admin.FirstOrDefault(w => w.adminUsername == admin.adminUsername && w.adminPassword == admin.adminPassword);
                if (kontrol != null)
                {
                    FormsAuthentication.SetAuthCookie(admin.adminUsername, true);
                    return RedirectToAction("Index", "Kategori");
                }
                else
                {
                    ViewBag.Mesaj = "Kullanıcı Adı veya Şifre Yanlış";
                    return View("Index");
                }
            }
            else ViewBag.Mesaj = "Güvenlik kodu yanlış";
            return View("Index");
        }
        

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public FileResult CaptchaImageOlustur()
        {
            CaptchaRandomImage CI = new CaptchaRandomImage();
            this.Session["GuvenlikKodu"] = CI.GetRandomString(5); //Burda güvenlik kodumuzun 5 karekter olması istedik
            CI.GenerateImage(this.Session["GuvenlikKodu"].ToString(), 212, 75, Color.DarkGray, Color.White);
            //Yukarda Güvenlik resmimizin 300x75 boyutunda olmasını ve yazının koyu gri, arkaplanıda beyaz yaptık
            MemoryStream stream = new MemoryStream();
            CI.Image.Save(stream, ImageFormat.Png);
            stream.Seek(0, SeekOrigin.Begin);
            return new FileStreamResult(stream, "~/Content/images/");
        }

        [HttpPost]
        public ActionResult Register(tbl_admin kullanici,string GuvenlikKodu)
        {
            if (this.Session["GuvenlikKodu"].ToString() == GuvenlikKodu)
            {
                var kontrol = db.tbl_admin.FirstOrDefault(w => w.adminUsername == kullanici.adminUsername);
                if (kontrol != null)
                {
                    ViewBag.Mesaj = "Başka bir kullanıcı adı giriniz";
                    return View();
                }
                else if (kullanici.adminUsername != null && kullanici.adminPassword != null && kullanici.adminPassword == kullanici.reg_password_confirm)
                {
                    //kullanıcı oluştur
                    db.tbl_admin.Add(kullanici);
                    db.SaveChanges();
                    FormsAuthentication.SetAuthCookie(kullanici.adminUsername, false);
                    return RedirectToAction("Index", "Kategori");
                }
                else
                {
                    ViewBag.Mesaj = "Parolayı Tekrar Giriniz";
                    return View();
                }
            }
            else ViewBag.Mesaj = "Güvenlik kodu yanlış";
            return View();
               
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return View("Index");
        }
    }
}