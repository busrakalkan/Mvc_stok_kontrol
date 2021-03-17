using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcStok.Models.Entity;

namespace MvcStok.Controllers
{
    [Authorize]
    public class MusteriController : Controller
    {
        // GET: Musteri
        MvcDbStokEntities db = new MvcDbStokEntities();
        public ActionResult Index()
        {
            var musteriler = db.tbl_Musteriler.ToList();
            return View(musteriler);
        }
        [HttpGet]
        public ActionResult YeniMusteri()
        {
            return View();
        }
        [HttpPost]
        public ActionResult YeniMusteri(tbl_Musteriler p1)
        {
            db.tbl_Musteriler.Add(p1);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Sil(int id)
        {
            var mus = db.tbl_Musteriler.Find(id);
            db.tbl_Musteriler.Remove(mus);
            db.SaveChanges();
            return RedirectToAction("Index");

        }
        public ActionResult MusteriGetir(int id)
        {
            var mus = db.tbl_Musteriler.Find(id);
            return View("MusteriGetir", mus);
        }
        public ActionResult Guncelle(tbl_Musteriler p2)
        {
            var gncl = db.tbl_Musteriler.Find(p2.MusteriID);
            gncl.MusteriAd = p2.MusteriAd;
            gncl.MusteriSoyad = p2.MusteriSoyad;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}