using MvcStok.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcStok.Controllers
{
    [Authorize]
    public class SatisController : Controller
    {
        
        // GET: Satis
        MvcDbStokEntities db = new MvcDbStokEntities();
        public ActionResult Index()
        {
            List<SelectListItem> urn = (from i in db.tbl_Urunler.ToList()
                                        select new SelectListItem
                                        {
                                            Text = i.UrunAd,
                                            Value = i.UrunID.ToString()
                                        }).ToList();
            ViewBag.urn= urn;
            List<SelectListItem> mus = (from i in db.tbl_Musteriler.ToList()
                                        select new SelectListItem
                                        {
                                            Text = i.MusteriAd + " " + i.MusteriSoyad,
                                            Value = i.MusteriID.ToString()
                                        }).ToList();
            ViewBag.mus = mus;

            List<tbl_Satislar> sts = db.tbl_Satislar.ToList();
            ViewBag.Satislar = sts;

            return View();
        }
        [HttpGet]
        public ActionResult YeniSatis()
        {
            return View();
        }
        [HttpPost]
        public ActionResult YeniSatis(tbl_Satislar p)
        {
            var urn = db.tbl_Urunler.Where(m => m.UrunID == p.tbl_Urunler.UrunID).FirstOrDefault();
            p.tbl_Urunler = urn;
            urn.Stok = Convert.ToByte(urn.Stok.Value - p.Adet.Value);
            
            var mus = db.tbl_Musteriler.Where(m => m.MusteriID == p.tbl_Musteriler.MusteriID).FirstOrDefault();
            p.tbl_Musteriler = mus;

            db.tbl_Satislar.Add(p);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}