using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcStok.Models.Entity;

namespace MvcStok.Controllers
{
    [Authorize]
    public class KategoriController : Controller
    {
        // GET: Kategori
        MvcDbStokEntities db = new MvcDbStokEntities();
        public ActionResult Index()
        {
            var kategoriler = db.tbl_Kategori.ToList();
            return View(kategoriler);
        }
        [HttpGet]
        public ActionResult YeniKategori()
        {
            return View();
        }

        [HttpPost]
        public ActionResult YeniKategori(tbl_Kategori p1)
        {
            db.tbl_Kategori.Add(p1);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Sil(int id)
        {
            var kategori = db.tbl_Kategori.Find(id);
            db.tbl_Kategori.Remove(kategori);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult KategoriGetir(int id)
        {
            var ktgr = db.tbl_Kategori.Find(id);
            return View("KategoriGetir", ktgr);
        }
        public ActionResult Guncelle(tbl_Kategori p2)
        {
            var gncl = db.tbl_Kategori.Find(p2.KategoriID);
            gncl.KategoriAd = p2.KategoriAd;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}