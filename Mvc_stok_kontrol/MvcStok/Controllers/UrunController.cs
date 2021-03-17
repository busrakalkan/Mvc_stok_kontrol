using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using MvcStok.Models.Entity;

namespace MvcStok.Controllers
{
    [Authorize]
    public class UrunController : Controller
    {
        // GET: Urun
        MvcDbStokEntities db = new MvcDbStokEntities();
        public ActionResult Index(int sayfa=1)
        {
            //var urunler = db.tbl_Urunler.ToList();
            var urunler = db.tbl_Urunler.ToList().ToPagedList(sayfa,4);
            return View(urunler);
        }
        [HttpGet]
        public ActionResult UrunEkle()
        {
            List<SelectListItem> degerler = (from i in db.tbl_Kategori.ToList()
                                             select new SelectListItem
                                             {
                                                 Text = i.KategoriAd,
                                                 Value = i.KategoriID.ToString()
                                             }).ToList();
            ViewBag.dgr = degerler;
            return View();
        }
        [HttpPost]
        public ActionResult UrunEkle(tbl_Urunler p1, HttpPostedFileBase uploadfile)
        {
            var ktg = db.tbl_Kategori.Where(m => m.KategoriID == p1.tbl_Kategori.KategoriID).FirstOrDefault();
            p1.tbl_Kategori = ktg;

            uploadfile.SaveAs(Server.MapPath("~/Content/images/") + uploadfile.FileName);
            p1.ResimYol = uploadfile.FileName;

            db.tbl_Urunler.Add(p1);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Sil(int id)
        {
            var urun = db.tbl_Urunler.Find(id);
            db.tbl_Urunler.Remove(urun);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult UrunGetir(int id)
        {
            var urun = db.tbl_Urunler.Find(id);
            List<SelectListItem> degerler = (from i in db.tbl_Kategori.ToList()
                                             select new SelectListItem
                                             {
                                                 Text = i.KategoriAd,
                                                 Value = i.KategoriID.ToString()
                                             }).ToList();
            ViewBag.dgr = degerler;

            return View("UrunGetir", urun);
        }
        public ActionResult Guncelle(tbl_Urunler p2, HttpPostedFileBase uploadfile)
        {
            var urun = db.tbl_Urunler.Find(p2.UrunID);
            urun.UrunAd = p2.UrunAd;
            urun.Marka = p2.Marka;
            urun.Fiyat = p2.Fiyat;
            urun.Stok = p2.Stok;
            var ktg = db.tbl_Kategori.Where(m => m.KategoriID == p2.tbl_Kategori.KategoriID).FirstOrDefault();
            urun.UrunKategori = ktg.KategoriID;

            if (uploadfile != null)
            {
                uploadfile.SaveAs(Server.MapPath("~/Content/images/") + uploadfile.FileName);
                urun.ResimYol = uploadfile.FileName;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}